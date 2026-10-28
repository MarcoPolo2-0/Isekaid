using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Isekaid.ExperimentalVrm;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace Isekaid.ExperimentalVrm.Editor
{
    [InitializeOnLoad]
    public static class VrmProbeEditor
    {
        private const string Folder = "Assets/Experiments/VrmCompanion";
        private const string Pending = "Isekaid.VrmProbe.Pending";
        private static VrmAppearance view;
        private static Task loading;
        private static int stage, frame;
        private static double started, previous;
        private static string output;
        private static Camera camera;
        private static Animator animator;
        private static Quaternion[] joints;
        private static Transform[] springTransforms;
        private static Vector3 followStart;
        private static int lastUnityFrame = -1;
        private static int motionIndex;
        private static double motionStarted, movementStarted;
        private static bool motionCaptured;
        private static readonly string[] Faces = { "neutral", "happy", "angry", "sad", "relaxed", "surprised", "blink", "blinkLeft", "blinkRight", "aa", "ih", "ou", "ee", "oh" };
        private static readonly string[] Motions = { "Idle", "Walk", "Run", "Turn" };
        private static Report report;

        [Serializable] private sealed class ExpressionResult { public string name; public bool available; public float maximumMorphWeight; }
        [Serializable] private sealed class MotionResult { public string name; public float maximumThighRotation; public float maximumSpringRotation; public float maximumRootRotation; }
        [Serializable] private sealed class Report
        {
            public string unity;
            public string graphicsDevice, processor;
            public float importMilliseconds, meanEditorFrameMs, p95EditorFrameMs;
            public long allocatedBefore, allocatedAfter, allocatedEnd;
            public int vertices, triangles, materials, renderers, springs, rejectionChecks;
            public bool humanoid, safeFallback, personalityIndependent, followMoved, failedImportPreservedAvatar;
            public float followDistance;
            public Vector3 avatarBoundsSize;
            public Vector3 anchorScale;
            public List<float> captureRenderReadbackPngMs = new List<float>();
            public string[] expressions, shaders;
            public string error;
            public List<ExpressionResult> faceTests = new List<ExpressionResult>();
            public List<MotionResult> motionTests = new List<MotionResult>();
        }
        private static readonly List<float> intervals = new List<float>();
        private static Quaternion thighInitial;
        private static Transform thigh;

        static VrmProbeEditor() { if (SessionState.GetBool(Pending, false)) EditorApplication.delayCall += Attach; }

        [MenuItem("Isekaid/Experiments/Build VRM test scene")]
        public static void BuildScene()
        {
            var controllerPath = Folder + "/TrialAnimations.controller";
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
            if (controller == null)
            {
                controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
                for (int motion = 0; motion < Motions.Length; motion++)
                {
                    var clip = new AnimationClip { name = Motions[motion] };
                    foreach (string muscle in HumanTrait.MuscleName)
                    {
                        float rest = muscle.Contains("Arm Down-Up") ? -.75f : 0f;
                        float amplitude = 0;
                        if (muscle.Contains("Upper Leg Front-Back")) amplitude = motion == 0 ? 0 : motion == 2 ? .6f : .3f;
                        if (muscle.Contains("Arm Front-Back")) amplitude = motion == 0 ? 0 : .25f;
                        float sign = muscle.StartsWith("Right") ? -1 : 1;
                        var curve = new AnimationCurve();
                        for (int sample = 0; sample <= 16; sample++)
                        {
                            float t = sample / 16f;
                            curve.AddKey(t, rest + sign * amplitude * Mathf.Sin(t * Mathf.PI * 2));
                        }
                        AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Animator), muscle), curve);
                    }
                    var settings = AnimationUtility.GetAnimationClipSettings(clip); settings.loopTime = true;
                    AnimationUtility.SetAnimationClipSettings(clip, settings);
                    AssetDatabase.CreateAsset(clip, Folder + "/" + clip.name + ".anim");
                    var state = controller.layers[0].stateMachine.AddState(clip.name); state.motion = clip;
                    state.speed = motion == 2 ? 2.2f : 1;
                }
            }
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/Game.unity");
            var companion = Object.FindAnyObjectByType<CompanionController>();
            if (companion == null) throw new InvalidOperationException("Game has no CompanionController.");
            var probe = new GameObject("VRM local appearance trial");
            var appearance = probe.AddComponent<VrmAppearance>();
            appearance.Anchor = (Transform)new SerializedObject(companion).FindProperty("visual").objectReferenceValue;
            appearance.TrialAnimations = controller;
            appearance.FacingYaw = 0;
            var adapter = probe.AddComponent<VrmCompanionAdapter>(); adapter.Behaviour = companion; adapter.Appearance = appearance;
            var capture = new GameObject("VRM evidence camera").AddComponent<Camera>(); capture.enabled = false;
            capture.clearFlags = CameraClearFlags.SolidColor; capture.backgroundColor = new Color(.12f, .15f, .20f);
            capture.orthographic = true; capture.nearClipPlane = .01f;
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), Folder + "/VrmLocalTrial.unity");
            AssetDatabase.SaveAssets();
        }

        public static void Run()
        {
            BuildScene(); SessionState.SetBool(Pending, true);
            EditorApplication.EnterPlaymode(); Attach();
        }

        private static void Attach()
        {
            EditorApplication.update -= Tick; EditorApplication.update += Tick;
            stage = -1; frame = 0; started = EditorApplication.timeSinceStartup; previous = started;
            lastUnityFrame = -1;
            report = new Report { unity = Application.unityVersion, graphicsDevice = SystemInfo.graphicsDeviceName, processor = SystemInfo.processorType }; intervals.Clear(); view = null; loading = null;
            var args = Environment.GetCommandLineArgs();
            output = args.SkipWhile(x => x != "-vrmOutput").Skip(1).First(); Directory.CreateDirectory(output);
        }

        private static void Tick()
        {
            try
            {
                double now = EditorApplication.timeSinceStartup;
                if (now - started > 240) throw new TimeoutException("VRM trial timed out at " + stage);
                if (!EditorApplication.isPlaying) return;
                if (Time.frameCount == lastUnityFrame) return;
                lastUnityFrame = Time.frameCount;
                if (stage == -1)
                {
                    view = Object.FindAnyObjectByType<VrmAppearance>(); if (view == null) return;
                    camera = GameObject.Find("VRM evidence camera").GetComponent<Camera>();
                    var path = Environment.GetCommandLineArgs().SkipWhile(x => x != "-vrmTestPath").Skip(1).First();
                    var data = VrmFileGuard.ReadLocal(path);
                    report.rejectionChecks = VrmGuardChecks.Run(data);
                    report.allocatedBefore = Profiler.GetTotalAllocatedMemoryLong();
                    loading = view.LoadLocal(path); stage = 0; return;
                }
                if (stage == 0)
                {
                    if (!loading.IsCompleted) return; if (loading.IsFaulted) throw loading.Exception;
                    if (view.Avatar == null) throw new InvalidOperationException("No runtime avatar.");
                    animator = view.Avatar.GetComponent<Animator>(); report.humanoid = animator.avatar.isHuman && animator.avatar.isValid;
                    report.importMilliseconds = view.ImportMilliseconds; report.expressions = view.Expressions;
                    report.anchorScale = view.Anchor.lossyScale;
                    var meshes = view.Avatar.GetComponentsInChildren<SkinnedMeshRenderer>(); report.renderers = meshes.Length;
                    report.vertices = meshes.Sum(x => x.sharedMesh.vertexCount); report.triangles = meshes.Sum(x => x.sharedMesh.triangles.Length / 3);
                    report.materials = meshes.SelectMany(x => x.sharedMaterials).Distinct().Count();
                    report.shaders = meshes.SelectMany(x => x.sharedMaterials).Select(x => x.shader.name).Distinct().ToArray();
                    report.springs = view.Avatar.SpringBone.Springs.Count;
                    springTransforms = view.Avatar.SpringBone.Springs.SelectMany(s => s.Joints).Select(j => j.transform).Distinct().ToArray();
                    joints = springTransforms.Select(t => t.localRotation).ToArray();
                    thigh = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg); thighInitial = thigh.localRotation;
                    report.allocatedAfter = Profiler.GetTotalAllocatedMemoryLong(); report.safeFallback = !view.RequestExpression("absent-expression", 1);
                    var adapter = view.GetComponent<VrmCompanionAdapter>(); var originalAvatar = view.Avatar;
                    foreach (CompanionPersonality personality in Enum.GetValues(typeof(CompanionPersonality))) adapter.Behaviour.SetPersonality(personality);
                    report.personalityIndependent = ReferenceEquals(view.Avatar, originalAvatar);
                    var invalidLoad = view.LoadLocal(Path.Combine(output, "missing-avatar.vrm"));
                    report.failedImportPreservedAvatar = invalidLoad.IsFaulted && invalidLoad.Exception != null && ReferenceEquals(view.Avatar, originalAvatar);
                    if (!report.failedImportPreservedAvatar) throw new InvalidOperationException("Failed import replaced the live avatar.");
                    if (!adapter.Behaviour.Interact()) throw new InvalidOperationException("Existing rescue failed with VRM appearance.");
                    if (adapter.Behaviour.State.Trust != 1) throw new InvalidOperationException("Existing rescue memory changed.");
                    view.PreviewingMotion = true;
                    stage = 1; frame = 0; motionIndex = 0; motionStarted = now; motionCaptured = false;
                    previous = now; view.SetMotion("Idle"); report.motionTests.Add(new MotionResult { name = "Idle" }); return;
                }
                if (stage == 1)
                {
                    if (motionIndex > 0 && now - previous < .1) intervals.Add((float)((now - previous) * 1000)); previous = now;
                    if (motionIndex == 3) view.Avatar.transform.Rotate(0, 90f * Time.deltaTime, 0);
                    var result = report.motionTests[motionIndex]; result.maximumThighRotation = Mathf.Max(result.maximumThighRotation, Quaternion.Angle(thighInitial, thigh.localRotation));
                    result.maximumRootRotation = Mathf.Max(result.maximumRootRotation, Quaternion.Angle(Quaternion.Euler(0, view.FacingYaw, 0), view.Avatar.transform.localRotation));
                    for (int i = 0; i < springTransforms.Length; i++) result.maximumSpringRotation = Mathf.Max(result.maximumSpringRotation, Quaternion.Angle(joints[i], springTransforms[i].localRotation));
                    if (!motionCaptured && now - motionStarted > .75) { Capture("motion-" + Motions[motionIndex], false); motionCaptured = true; }
                    if (now - motionStarted > 1.5)
                    {
                        motionIndex++;
                        if (motionIndex == Motions.Length) { stage = 2; frame = 0; view.SetMotion("Idle"); return; }
                        motionStarted = now; motionCaptured = false; view.SetMotion(Motions[motionIndex]);
                        report.motionTests.Add(new MotionResult { name = Motions[motionIndex] });
                    }
                    return;
                }
                if (stage == 2)
                {
                    view.Avatar.transform.localRotation = Quaternion.Euler(0, view.FacingYaw, 0);
                    int face = frame / 5;
                    if (face >= Faces.Length)
                    {
                        var behaviour = view.GetComponent<VrmCompanionAdapter>().Behaviour;
                        followStart = behaviour.transform.position;
                        var player = Object.FindAnyObjectByType<CompanionPrototype>().Player;
                        if (NavMesh.SamplePosition(followStart + Vector3.forward * 5f, out var hit, 3, NavMesh.AllAreas)) player.transform.position = hit.position;
                        view.PreviewingMotion = false; behaviour.SetFollowing(true); stage = 3; frame = 0; movementStarted = now; return;
                    }
                    if (frame % 5 == 0) report.faceTests.Add(new ExpressionResult { name = Faces[face], available = view.RequestExpression(Faces[face], 1) });
                    if (frame % 5 == 4)
                    {
                        report.faceTests[face].maximumMorphWeight = view.Avatar.GetComponentsInChildren<SkinnedMeshRenderer>()
                            .SelectMany(r => Enumerable.Range(0, r.sharedMesh.blendShapeCount).Select(r.GetBlendShapeWeight)).DefaultIfEmpty().Max();
                        Capture("face-" + Faces[face], true);
                    }
                    frame++; return;
                }
                if (stage == 3)
                {
                    var adapter = view.GetComponent<VrmCompanionAdapter>(); adapter.FollowAgentMotion();
                    if (now - movementStarted < 3) return;
                    report.followDistance = Vector3.Distance(followStart, adapter.Behaviour.transform.position);
                    report.followMoved = report.followDistance > .15f; Capture("existing-controller-follow", false);
                    if (!report.followMoved) throw new InvalidOperationException("Existing controller did not move sufficiently in the trial.");
                    report.allocatedEnd = Profiler.GetTotalAllocatedMemoryLong(); intervals.Sort();
                    report.meanEditorFrameMs = intervals.Average(); report.p95EditorFrameMs = intervals[(int)(intervals.Count * .95f)];
                    Finish(true);
                }
            }
            catch (Exception error) { report.error = error.ToString(); Finish(false); }
        }

        private static void Capture(string name, bool face)
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var bounds = new Bounds(); bool first = true;
            foreach (var renderer in view.Avatar.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                var baked = new Mesh(); renderer.BakeMesh(baked);
                foreach (var vertex in baked.vertices)
                {
                    Vector3 point = renderer.transform.TransformPoint(vertex);
                    if (first) { bounds = new Bounds(point, Vector3.zero); first = false; } else bounds.Encapsulate(point);
                }
                Object.DestroyImmediate(baked);
            }
            report.avatarBoundsSize = bounds.size;
            Vector3 target = face ? animator.GetBoneTransform(HumanBodyBones.Head).position + Vector3.up * .10f : bounds.center;
            camera.transform.position = target + view.Avatar.transform.forward * 3;
            camera.transform.LookAt(target); camera.orthographicSize = face ? .24f : Mathf.Max(bounds.size.y * .56f, bounds.size.x * .70f);
            var targetTexture = new RenderTexture(960, 1200, 24); camera.targetTexture = targetTexture; camera.Render();
            var previousTarget = RenderTexture.active; RenderTexture.active = targetTexture;
            var texture = new Texture2D(960, 1200, TextureFormat.RGB24, false); texture.ReadPixels(new Rect(0, 0, 960, 1200), 0, 0); texture.Apply();
            File.WriteAllBytes(Path.Combine(output, name + ".png"), texture.EncodeToPNG());
            RenderTexture.active = previousTarget; camera.targetTexture = null; targetTexture.Release(); Object.DestroyImmediate(texture); Object.DestroyImmediate(targetTexture);
            report.captureRenderReadbackPngMs.Add((float)timer.Elapsed.TotalMilliseconds);
        }

        private static void Finish(bool success)
        {
            File.WriteAllText(Path.Combine(output, "runtime-results.json"), JsonUtility.ToJson(report, true));
            SessionState.SetBool(Pending, false); EditorApplication.update -= Tick;
            Debug.Log(success ? "ISEKAID_VRM_RUNTIME_TRIAL_PASSED" : "ISEKAID_VRM_RUNTIME_TRIAL_FAILED: " + report.error);
            EditorApplication.Exit(success ? 0 : 1);
        }
    }
}
