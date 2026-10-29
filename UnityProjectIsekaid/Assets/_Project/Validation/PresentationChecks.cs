#if UNITY_EDITOR || DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Isekaid.ExperimentalVrm;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace Isekaid.Validation
{
    public static class PresentationChecks
    {
        [Serializable] public sealed class Motion
        {
            public string avatar, requested, clip;
            public bool human, loop;
            public float length, normalizedStart, normalizedEnd, tailBoneDegrees;
        }
        [Serializable] public sealed class Follow
        {
            public string avatar, scenario;
            public int samples, stateChanges, timeResets, nearThresholdCrossings, transitionFrames, requestedStateChanges, rapidStateChanges;
            public float distance, maxSpeed;
        }
        [Serializable] public sealed class Results
        {
            public bool passed;
            public string unity, phase, error, avatarHash;
            public List<Motion> motions = new List<Motion>();
            public List<Follow> follows = new List<Follow>();
            public List<string> rendering = new List<string>();
            public List<string> gait = new List<string>();
            public List<string> stride = new List<string>();
        }
        public static Results Report;
        private static CompanionPrototype prototype;
        private static CompanionAppearance appearance;
        private static string avatar;
        private static StreamWriter samples;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static async void AutoRun()
        {
            if (Application.isEditor || !Debug.isDebugBuild || F2RuntimeChecks.Argument("-isekPresentationTest") == null) return;
            try { await Run(); Write(true); Application.Quit(0); }
            catch (Exception e) { Report ??= new Results(); Report.error = e.ToString(); Write(false); Application.Quit(1); }
        }
        public static void Write(bool passed)
        {
            Report.passed = passed;
            Directory.CreateDirectory(AvatarFiles.Root);
            File.WriteAllText(Path.Combine(AvatarFiles.Root, "presentation-results.json"), JsonUtility.ToJson(Report, true));
            Debug.Log(passed ? "ISEKAID_PRESENTATION_CHECKS_FINISHED" : "ISEKAID_PRESENTATION_CHECKS_FAILED");
        }
        private static async Task Until(Func<bool> condition)
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            while (!condition()) { if (timer.ElapsedMilliseconds > 60000) throw new TimeoutException("Presentation scenario timed out"); await Task.Delay(20); }
            await Task.Delay(100);
        }
        public static async Task Run()
        {
            Report = new Results { unity = Application.unityVersion, phase = F2RuntimeChecks.Argument("-isekPresentationTest") ?? "baseline" };
            Application.runInBackground = true; Application.targetFrameRate = 60; QualitySettings.vSyncCount = 0;
            await Until(() => Object.FindAnyObjectByType<CharacterCreationController>()?.Appearance.Ready == true);
            var creator = Object.FindAnyObjectByType<CharacterCreationController>();
            creator.SelectCatalog(); if (!creator.BeginGame()) throw new InvalidOperationException("Creator could not start Game");
            await Until(() => Object.FindAnyObjectByType<CompanionPrototype>()?.Companion.GetComponent<CompanionAppearance>().Ready == true);
            prototype = Object.FindAnyObjectByType<CompanionPrototype>();
            appearance = prototype.Companion.GetComponent<CompanionAppearance>();
            prototype.SetPaused(false); prototype.Companion.Interact(); prototype.Player.InputEnabled = false;
            samples = new StreamWriter(Path.Combine(AvatarFiles.Root, "animation-samples.csv"));
            samples.WriteLine("avatar,scenario,time,speed,state,normalized,transition,leftFootY,rightFootY");
            try
            {
                avatar = "catalog"; await Inspect();
                await appearance.LoadVrm(F2RuntimeChecks.Argument("-isekVrmFixture"), true);
                avatar = "vrm"; Report.avatarHash = appearance.Selection.hash;
                await Inspect();
                if (Report.phase != "calibration") await RenderComparison();
                if (Report.phase == "candidate")
                {
                    if (Report.motions.Any(m => !m.human || !m.loop || m.normalizedEnd <= m.normalizedStart)) throw new InvalidOperationException("Humanoid looping animation did not advance");
                    if (Report.follows.Any(f => f.timeResets != 0 || f.rapidStateChanges != 0 || (f.scenario == "threshold-noise" && f.stateChanges > 2))) throw new InvalidOperationException("Locomotion state restarted or oscillated");
                    if (Report.follows.Any(f => f.samples < 100 || f.maxSpeed < 3)) throw new InvalidOperationException("Follow scenario did not exercise movement");
                }
            }
            finally { samples.Dispose(); samples = null; }
        }

        private static Animator Animator => appearance.Importer.HasAvatar ? appearance.Importer.Avatar.GetComponent<Animator>() : appearance.Catalog.GetComponentInChildren<Animator>();
        private static Transform[] Joints => Animator.GetComponentsInChildren<Transform>().Where(t => t.name.IndexOf("leg", StringComparison.OrdinalIgnoreCase) >= 0 || t.name.IndexOf("thigh", StringComparison.OrdinalIgnoreCase) >= 0).Take(12).ToArray();

        private static async Task Inspect()
        {
            prototype.Companion.SetFollowing(false); await Task.Delay(250);
            if (Report.phase == "calibration") { await MeasureGait(); return; }
            foreach (string name in new[] { "Idle", "Walk", "Run" })
            {
                appearance.PreviewMotion(name); await Task.Delay(550);
                await Until(() => !Animator.IsInTransition(0));
                var clip = Animator.GetCurrentAnimatorClipInfo(0).FirstOrDefault().clip;
                var record = new Motion { avatar = avatar, requested = name, clip = clip != null ? clip.name : "", human = Animator.isHuman,
                    loop = clip != null && clip.isLooping, length = clip != null ? clip.length : 0, normalizedStart = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime };
                await Task.Delay(2200);
                var joints = Joints; var rotations = joints.Select(t => t.localRotation).ToArray();
                await Task.Delay(350);
                for (int i = 0; i < joints.Length; i++) record.tailBoneDegrees = Mathf.Max(record.tailBoneDegrees, Quaternion.Angle(rotations[i], joints[i].localRotation));
                record.normalizedEnd = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                Report.motions.Add(record);
                CameraAtActor(); await F2RuntimeChecks.Capture(avatar + "-" + name);
            }
            DumpMaterials();
            if (Animator.isHuman) await MeasureGait();
            await FollowScenario("continuous", false);
            await FollowScenario("threshold-noise", true);
            if (Animator.isHuman) { await MeasureStride(1.4f); await MeasureStride(3.8f); }
        }
        private static void MoveTarget(Vector3 position)
        {
            var controller = prototype.Player.GetComponent<CharacterController>(); controller.enabled = false;
            prototype.Player.transform.position = position; controller.enabled = true;
        }
        private static void CameraAtActor()
        {
            var camera = prototype.Player.ViewCamera;
            camera.transform.position = prototype.Companion.transform.position + new Vector3(0, 1.05f, 3.3f);
            camera.transform.LookAt(prototype.Companion.transform.position + Vector3.up * .95f);
        }
        private static async Task FollowScenario(string name, bool noise)
        {
            var actor = prototype.Companion; var agent = actor.GetComponent<NavMeshAgent>();
            float speed = agent.speed, acceleration = agent.acceleration;
            prototype.Restart(); actor.Interact(); prototype.Player.InputEnabled = false;
            var start = actor.transform.position;
            MoveTarget(new Vector3(0, .05f, 12));
            var result = new Follow { avatar = avatar, scenario = name };
            int previousHash = 0; float previousTime = 0, previousSpeed = 0;
            string previousRequested = null;
            var motionField = typeof(CompanionAppearance).GetField("motion", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            float started = Time.time, lastStateChange = -10;
            try
            {
                if (noise) agent.acceleration = 100;
                while (Time.time - started < (noise ? 5 : 10))
                {
                    float elapsed = Time.time - started;
                    if (noise) agent.speed = (Time.frameCount / 3) % 2 == 0 ? 2.96f : 3.04f;
                    else if (elapsed > 7) MoveTarget(new Vector3(-5, .05f, 6));
                    else if (elapsed > 5) actor.SetFollowing(true);
                    else if (elapsed > 3) actor.SetFollowing(false);
                    var state = Animator.GetCurrentAnimatorStateInfo(0);
                    float actual = agent.velocity.magnitude;
                    if (previousHash != 0 && state.fullPathHash != previousHash)
                    {
                        result.stateChanges++;
                        if (Time.time - lastStateChange < .25f) result.rapidStateChanges++;
                        lastStateChange = Time.time;
                    }
                    if (previousHash == state.fullPathHash && state.normalizedTime < previousTime - .05f) result.timeResets++;
                    if ((actual > 3) != (previousSpeed > 3)) result.nearThresholdCrossings++;
                    if (Animator.IsInTransition(0)) result.transitionFrames++;
                    string requested = motionField?.GetValue(appearance) as string;
                    if (previousRequested != null && requested != previousRequested) result.requestedStateChanges++;
                    previousRequested = requested;
                    previousHash = state.fullPathHash; previousTime = state.normalizedTime; previousSpeed = actual;
                    result.samples++; result.maxSpeed = Mathf.Max(result.maxSpeed, actual);
                    float left = 0, right = 0;
                    if (Animator.isHuman) { left = Animator.GetBoneTransform(HumanBodyBones.LeftFoot).position.y; right = Animator.GetBoneTransform(HumanBodyBones.RightFoot).position.y; }
                    samples.WriteLine(FormattableString.Invariant($"{avatar},{name},{elapsed:F4},{actual:F4},{state.fullPathHash},{state.normalizedTime:F5},{Animator.IsInTransition(0)},{left:F5},{right:F5}"));
                    await Task.Delay(16);
                }
                result.distance = Vector3.Distance(start, actor.transform.position); Report.follows.Add(result);
                CameraAtActor(); await F2RuntimeChecks.Capture(avatar + "-" + name);
            }
            finally { agent.speed = speed; agent.acceleration = acceleration; }
        }
        private static void DumpMaterials()
        {
            using var writer = new StreamWriter(Path.Combine(AvatarFiles.Root, avatar + "-materials.txt"));
            var root = appearance.Importer.HasAvatar ? appearance.Importer.Avatar.gameObject : appearance.Catalog;
            foreach (var material in root.GetComponentsInChildren<Renderer>().SelectMany(r => r.sharedMaterials).Distinct())
            {
                writer.WriteLine("MATERIAL " + material.name + " shader=" + material.shader.name);
                foreach (string key in new[] { "_MainTex", "_BaseMap", "_ShadeTexture", "_BumpMap", "_EmissionMap", "_SphereAdd" })
                    if (material.HasProperty(key)) { var texture = material.GetTexture(key); writer.WriteLine(key + "=" + (texture != null ? texture.name + " " + texture.width + "x" + texture.height : "null")); }
                foreach (string key in new[] { "_Color", "_BaseColor", "_ShadeColor", "_EmissionColor" })
                    if (material.HasProperty(key)) writer.WriteLine(key + "=" + material.GetColor(key));
            }
        }

        private static async Task MeasureGait()
        {
            foreach (string motion in new[] { "Idle", "Walk", "Run" })
            {
                appearance.PreviewMotion(motion); await Task.Delay(350);
                var points = new List<(float time, Vector3 left, Vector3 right)>();
                var root = prototype.Companion.transform;
                float started = Time.time;
                while (Time.time - started < 2.5f)
                {
                    points.Add((Time.time, root.InverseTransformPoint(Animator.GetBoneTransform(HumanBodyBones.LeftFoot).position), root.InverseTransformPoint(Animator.GetBoneTransform(HumanBodyBones.RightFoot).position)));
                    await Task.Delay(16);
                }
                float floor = points.Min(p => Mathf.Min(p.left.y, p.right.y));
                var stance = new List<float>();
                for (int i = 1; i < points.Count; i++)
                {
                    float dt = points[i].time - points[i - 1].time;
                    if (dt <= 0) continue;
                    foreach (bool left in new[] { true, false })
                    {
                        var current = left ? points[i].left : points[i].right;
                        var previous = left ? points[i - 1].left : points[i - 1].right;
                        float velocity = -(current.z - previous.z) / dt;
                        if (current.y < floor + .035f && velocity > .1f) stance.Add(velocity);
                    }
                }
                Report.gait.Add(FormattableString.Invariant($"{avatar} {motion} humanScale={Animator.humanScale:F5} footBoneMinY={floor:F4} footBottom={Animator.leftFeetBottomHeight:F4} stanceBackSpeed={(stance.Count > 0 ? stance.Average() : 0):F4} samples={stance.Count}"));
                if (motion == "Idle")
                {
                    float meshFloor = float.PositiveInfinity;
                    foreach (var renderer in Animator.GetComponentsInChildren<SkinnedMeshRenderer>())
                    {
                        var mesh = new Mesh(); renderer.BakeMesh(mesh);
                        foreach (var vertex in mesh.vertices) meshFloor = Mathf.Min(meshFloor, root.InverseTransformPoint(renderer.transform.TransformPoint(vertex)).y);
                        Object.Destroy(mesh);
                    }
                    Report.gait.Add(FormattableString.Invariant($"{avatar} Idle actualMeshMinY={meshFloor:F5}"));
                }
                using var writer = new StreamWriter(Path.Combine(AvatarFiles.Root, avatar + "-" + motion + "-gait.csv"));
                writer.WriteLine("time,leftX,leftY,leftZ,rightX,rightY,rightZ");
                foreach (var p in points) writer.WriteLine(FormattableString.Invariant($"{p.time:F5},{p.left.x:F5},{p.left.y:F5},{p.left.z:F5},{p.right.x:F5},{p.right.y:F5},{p.right.z:F5}"));
            }
        }

        private static async Task MeasureStride(float speed)
        {
            var actor = prototype.Companion; var agent = actor.GetComponent<NavMeshAgent>();
            float previousSpeed = agent.speed;
            var frames = new List<(float time, Vector3 left, Vector3 right, float leftY, float rightY, float speed)>();
            var camera = prototype.Player.ViewCamera;
            var target = new RenderTexture(640, 360, 24);
            var image = new Texture2D(640, 360, TextureFormat.RGB24, false);
            var previousTarget = camera.targetTexture;
            try
            {
                prototype.Restart(); actor.Interact(); prototype.Player.InputEnabled = false;
                agent.speed = speed; MoveTarget(actor.transform.position + Vector3.forward * 11);
                await Task.Delay(600);
                float started = Time.time;
                float captureAt = started;
                int frameNumber = 0;
                string captureFolder = Path.Combine(F2RuntimeChecks.Argument("-isekCaptureDir"), avatar + "-stride-" + speed.ToString("F1", System.Globalization.CultureInfo.InvariantCulture));
                Directory.CreateDirectory(captureFolder);
                while (Time.time - started < 1.8f)
                {
                    var left = Animator.GetBoneTransform(HumanBodyBones.LeftFoot).position;
                    var right = Animator.GetBoneTransform(HumanBodyBones.RightFoot).position;
                    frames.Add((Time.time, left, right, left.y - actor.transform.position.y, right.y - actor.transform.position.y, agent.velocity.magnitude));
                    if (Time.time >= captureAt)
                    {
                        captureAt = Time.time + .08f;
                        camera.transform.position = actor.transform.position + new Vector3(2.6f, 1.1f, 2.2f);
                        camera.transform.LookAt(actor.transform.position + Vector3.up * .85f);
                        camera.targetTexture = target; camera.Render();
                        var previous = RenderTexture.active; RenderTexture.active = target;
                        image.ReadPixels(new Rect(0, 0, 640, 360), 0, 0); image.Apply();
                        File.WriteAllBytes(Path.Combine(captureFolder, (frameNumber++).ToString("D3") + ".png"), image.EncodeToPNG());
                        RenderTexture.active = previous; camera.targetTexture = previousTarget;
                    }
                    await Task.Delay(16);
                }
                float floor = frames.Min(f => Mathf.Min(f.leftY, f.rightY));
                var slips = new List<float>();
                for (int i = 1; i < frames.Count; i++)
                {
                    float dt = frames[i].time - frames[i - 1].time;
                    if (dt <= 0 || frames[i].speed < speed * .9f) continue;
                    if (frames[i].leftY < floor + .035f) { var delta = frames[i].left - frames[i - 1].left; delta.y = 0; slips.Add(delta.magnitude / dt); }
                    if (frames[i].rightY < floor + .035f) { var delta = frames[i].right - frames[i - 1].right; delta.y = 0; slips.Add(delta.magnitude / dt); }
                }
                Report.stride.Add(FormattableString.Invariant($"{avatar} target={speed:F2} measuredSpeed={frames.Average(f => f.speed):F3} lowFootWorldSpeed={(slips.Count > 0 ? slips.Average() : -1):F3} contacts={slips.Count} effectiveHumanScale={Animator.humanScale * Animator.transform.lossyScale.y:F5}"));
                using var writer = new StreamWriter(Path.Combine(AvatarFiles.Root, avatar + "-stride-" + speed.ToString("F1", System.Globalization.CultureInfo.InvariantCulture) + ".csv"));
                writer.WriteLine("time,leftX,leftY,leftZ,rightX,rightY,rightZ,leftLocalY,rightLocalY,speed");
                foreach (var f in frames) writer.WriteLine(FormattableString.Invariant($"{f.time:F5},{f.left.x:F5},{f.left.y:F5},{f.left.z:F5},{f.right.x:F5},{f.right.y:F5},{f.right.z:F5},{f.leftY:F5},{f.rightY:F5},{f.speed:F5}"));
                if (frames.Average(f => f.speed) < speed * .85f || slips.Count < 5) throw new InvalidOperationException("Stride measurement did not exercise sustained motion");
            }
            finally { agent.speed = previousSpeed; camera.targetTexture = previousTarget; target.Release(); Object.Destroy(target); Object.Destroy(image); }
        }

        private static async Task RenderComparison()
        {
            var lights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
            var enabled = lights.Select(l => l.enabled).ToArray();
            var ambientMode = RenderSettings.ambientMode; var ambient = RenderSettings.ambientLight;
            var host = new GameObject("PrivateRenderComparison") { layer = 30 };
            var view = host.AddComponent<VrmAppearance>(); view.ShowDiagnosticGUI = false;
            Camera camera = null; RenderTexture target = null;
            try
            {
                await view.LoadLocal(F2RuntimeChecks.Argument("-isekVrmFixture"));
                var animator = view.Avatar.GetComponent<Animator>(); animator.enabled = false;
                foreach (var light in lights) light.enabled = false;
                var key = new GameObject("ReferenceKey").AddComponent<Light>(); key.transform.SetParent(host.transform);
                key.type = LightType.Directional; key.cullingMask = 1 << 30;
                key.transform.rotation = Quaternion.LookRotation(new Vector3(-1, -2, -2));
                camera = new GameObject("ReferenceCamera").AddComponent<Camera>(); camera.transform.SetParent(host.transform);
                camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = new Color(.01f, .015f, .02f);
                camera.cullingMask = 1 << 30; camera.fieldOfView = 30;
                var renderers = view.Avatar.GetComponentsInChildren<SkinnedMeshRenderer>();
                var bounds = renderers[0].bounds; foreach (var r in renderers) bounds.Encapsulate(r.bounds);
                camera.transform.position = bounds.center + new Vector3(0, 0, bounds.size.y * 2.1f); camera.transform.LookAt(bounds.center);
                target = new RenderTexture(960, 1200, 24); camera.targetTexture = target;
                foreach (var setup in new[] { ("scene-before", 1.7f, new Color(1, .91f, .75f), new Color(.55f, .68f, .76f)), ("reference", 1f, Color.white, new Color(.2f, .2f, .2f)), ("balanced", .65f, Color.white, new Color(.4f, .4f, .4f)) })
                {
                    key.intensity = setup.Item2; key.color = setup.Item3;
                    RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat; RenderSettings.ambientLight = setup.Item4;
                    await Task.Delay(150); camera.Render();
                    var previous = RenderTexture.active; RenderTexture.active = target;
                    var image = new Texture2D(960, 1200, TextureFormat.RGB24, false);
                    image.ReadPixels(new Rect(0, 0, 960, 1200), 0, 0); image.Apply();
                    string folder = F2RuntimeChecks.Argument("-isekCaptureDir"); Directory.CreateDirectory(folder);
                    File.WriteAllBytes(Path.Combine(folder, "vrm-tpose-" + setup.Item1 + ".png"), image.EncodeToPNG());
                    int bright = image.GetPixels32().Count(p => p.r >= 250 && p.g >= 250 && p.b >= 250);
                    Report.rendering.Add(setup.Item1 + " clippedWhitePixels=" + bright);
                    Object.Destroy(image); RenderTexture.active = previous;
                }
            }
            finally
            {
                RenderSettings.ambientMode = ambientMode; RenderSettings.ambientLight = ambient;
                for (int i = 0; i < lights.Length; i++) if (lights[i] != null) lights[i].enabled = enabled[i];
                if (camera != null) camera.targetTexture = null;
                if (target != null) { target.Release(); Object.Destroy(target); }
                Object.Destroy(host);
            }
        }
    }
}
#endif
