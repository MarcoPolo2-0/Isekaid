using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniGLTF;
using UniVRM10;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace Isekaid.ExperimentalVrm.Editor
{
    [InitializeOnLoad]
    public static class VrmStabilityChecks
    {
        private const string Pending = "Isekaid.VrmStability.Pending";
        private static bool running;
        private static double started;
        private static Report report;
        private static string output;
        private static int released;

        [Serializable] private sealed class MemorySample
        {
            public string label;
            public long allocated, reserved, managed;
            public int meshes, textures, materials, avatars;
        }
        [Serializable] private sealed class Report
        {
            public string unity, error, sourceSha256;
            public int faultCases, replacementCycles, unloadCycles, releasedOwnedObjects;
            public bool firstImportRollback, concurrentLoadRejected, unloadDuringLoadRejected, ownerDestructionHandled;
            public long maximumSettledGrowth, settledToleranceBytes = 16 * 1024 * 1024;
            public long peakAllocated;
            public List<string> faults = new List<string>();
            public List<MemorySample> memory = new List<MemorySample>();
        }

        static VrmStabilityChecks()
        {
            if (SessionState.GetBool(Pending, false)) EditorApplication.delayCall += Begin;
        }

        public static void Run()
        {
            EditorSceneManager.OpenScene("Assets/Experiments/VrmCompanion/VrmLocalTrial.unity");
            SessionState.SetBool(Pending, true);
            EditorApplication.EnterPlaymode();
            EditorApplication.delayCall += Begin;
        }

        private static async void Begin()
        {
            if (running || !SessionState.GetBool(Pending, false)) return;
            if (!EditorApplication.isPlaying) { EditorApplication.delayCall += Begin; return; }
            running = true; started = EditorApplication.timeSinceStartup; released = 0;
            report = new Report { unity = Application.unityVersion };
            output = Argument("-vrmOutput"); Directory.CreateDirectory(output);
            EditorApplication.update += Watchdog;
            try { await Exercise(Argument("-vrmTestPath")); Finish(true); }
            catch (Exception error) { report.error = error.ToString(); Finish(false); }
        }

        private static async Task Exercise(string path)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
                report.sourceSha256 = BitConverter.ToString(sha.ComputeHash(File.ReadAllBytes(path))).Replace("-", "").ToLowerInvariant();
            var view = Object.FindAnyObjectByType<VrmAppearance>();
            Check(view != null, "The saved experimental scene must contain its presenter.");
            var fallback = view.Anchor.GetComponentsInChildren<Renderer>().ToDictionary(r => r, r => r.enabled);

            view.ImportStageProbe = (stage, candidate) => { if (stage == "PreviousHidden") throw new IOException("Injected first activation failure"); };
            await ExpectFailure(view.LoadLocal(path)); view.ImportStageProbe = null;
            await Frames(3);
            Check(view.Avatar == null && fallback.All(pair => pair.Key.enabled == pair.Value), "First import must restore the mannequin on failure.");
            report.firstImportRollback = true;

            await view.LoadLocal(path); await Frames(3);
            foreach (string fault in new[] { "ValidatedBytes", "Imported", "Prepared", "Activated", "PreviousHidden" })
            {
                var previous = view.Avatar;
                Check(view.RequestExpression("happy", .7f), "Warm avatar must support the fixture expression.");
                await Frames(2);
                var beforeVisibility = previous.GetComponentsInChildren<Renderer>().ToDictionary(r => r, r => r.enabled);
                string[] expressions = view.Expressions.ToArray();
                var face = previous.Runtime.Expression;
                var rejectedObjects = new List<Object>();
                view.ImportStageProbe = (stage, candidate) =>
                {
                    Check(ReferenceEquals(view.Avatar, previous), "Candidate must not publish before commit.");
                    if (candidate != null) rejectedObjects = rejectedObjects.Concat(Owned(candidate)).Distinct().ToList();
                    if (stage == "Activated") Check(Visible(candidate) && Visible(previous), "New avatar must activate while the previous one is still visible.");
                    if (stage == fault) throw new IOException("Injected failure: " + stage);
                };
                await ExpectFailure(view.LoadLocal(path)); view.ImportStageProbe = null;
                await Frames(3);
                Check(ReferenceEquals(view.Avatar, previous) && Visible(previous), "Failed replacement lost the previous avatar.");
                Check(beforeVisibility.All(pair => pair.Key.enabled == pair.Value) && expressions.SequenceEqual(view.Expressions), "Rollback changed the previous presentation.");
                Check(ReferenceEquals(previous.Runtime.Expression, face), "Rollback changed the expression runtime.");
                Check(previous.Runtime.Expression.GetWeight(ExpressionKey.Happy) > .69f, "Rollback lost the requested expression.");
                CheckReleased(rejectedObjects);
                report.faultCases++; report.faults.Add(fault);
            }

            var warmObjects = Owned(view.Avatar); view.Unload(); await Settle(); CheckReleased(warmObjects);
            Check(fallback.All(pair => pair.Key.enabled == pair.Value), "Unload must restore the original renderer flags.");
            report.memory.Add(Sample("warm-baseline"));
            for (int cycle = 0; cycle < 6; cycle++)
            {
                await view.LoadLocal(path); await Frames(2);
                var previous = view.Avatar; var previousObjects = Owned(previous);
                view.ImportStageProbe = (stage, candidate) =>
                {
                    if (stage == "Activated") Check(Visible(candidate) && Visible(previous) && ReferenceEquals(view.Avatar, previous), "Replacement committed too early.");
                };
                var replacement = view.LoadLocal(path);
                await ExpectFailure(view.LoadLocal(path)); report.concurrentLoadRejected = true;
                bool unloadRejected = false; try { view.Unload(); } catch (InvalidOperationException) { unloadRejected = true; }
                Check(unloadRejected, "Unload must not race an import."); report.unloadDuringLoadRejected = true;
                await replacement; view.ImportStageProbe = null; await Frames(3);
                Check(!ReferenceEquals(view.Avatar, previous) && Visible(view.Avatar), "Replacement did not publish an active avatar.");
                CheckReleased(previousObjects); report.replacementCycles++;
                var currentObjects = Owned(view.Avatar); view.Unload(); view.Unload(); await Settle();
                Check(view.Avatar == null && view.Expressions.Length == 0 && !view.RequestExpression("happy", 1), "Unload retained the presentation API.");
                Check(fallback.All(pair => pair.Key.enabled == pair.Value), "Repeated unload lost the fallback renderers.");
                CheckReleased(currentObjects); report.unloadCycles++; report.memory.Add(Sample("cycle-" + cycle));
            }
            long baseline = report.memory[0].allocated;
            report.maximumSettledGrowth = report.memory.Max(m => m.allocated - baseline);
            Check(report.maximumSettledGrowth <= report.settledToleranceBytes, "Settled allocated memory grew beyond the diagnostic tolerance.");

            var before = new HashSet<Object>(TransientAssets());
            var ownerLoad = view.LoadLocal(path);
            Check(!ownerLoad.IsCompleted, "Owner destruction case needs an in-flight runtime import.");
            Object.Destroy(view.gameObject);
            await ExpectFailure(ownerLoad); await Settle();
            var leaked = TransientAssets().Where(o => !before.Contains(o)).ToArray();
            Check(leaked.Length == 0, "Owner destruction left transient assets: " + string.Join(",", leaked.Select(o => o.GetType().Name + ":" + o.name)));
            Check(Object.FindObjectsByType<Vrm10Instance>(FindObjectsSortMode.None).Length == 0, "An imported root survived the owner.");
            report.ownerDestructionHandled = true; report.memory.Add(Sample("owner-destroyed"));
            report.maximumSettledGrowth = report.memory.Max(m => m.allocated - baseline);
            Check(report.maximumSettledGrowth <= report.settledToleranceBytes, "Owner cleanup exceeded the settled memory tolerance.");
            report.releasedOwnedObjects = released;
        }

        private static List<Object> Owned(Vrm10Instance avatar)
        {
            var objects = avatar.GetComponent<RuntimeGltfInstance>().RuntimeResources.Select(pair => pair.Item2).ToList();
            objects.Add(avatar.GetComponent<Animator>().avatar); objects.Add(avatar.Vrm); objects.Add(avatar.gameObject);
            return objects.Where(o => o != null && !EditorUtility.IsPersistent(o)).Distinct().ToList();
        }
        private static bool Visible(Vrm10Instance avatar) => avatar != null && avatar.gameObject.activeInHierarchy && avatar.GetComponentsInChildren<Renderer>().All(r => r.enabled);
        private static void CheckReleased(List<Object> objects)
        {
            var alive = objects.Where(o => o != null).ToArray();
            Check(alive.Length == 0, "Owned resources survived Destroy: " + string.Join(",", alive.Select(o => o.GetType().Name + ":" + o.name)));
            released += objects.Count;
        }
        private static Object[] TransientAssets() => Resources.FindObjectsOfTypeAll<Mesh>().Cast<Object>()
            .Concat(Resources.FindObjectsOfTypeAll<Texture>()).Concat(Resources.FindObjectsOfTypeAll<Material>())
            .Concat(Resources.FindObjectsOfTypeAll<Avatar>()).Where(o => !EditorUtility.IsPersistent(o)).ToArray();
        private static MemorySample Sample(string label) => new MemorySample
        {
            label = label, allocated = Profiler.GetTotalAllocatedMemoryLong(), reserved = Profiler.GetTotalReservedMemoryLong(),
            managed = GC.GetTotalMemory(false), meshes = Resources.FindObjectsOfTypeAll<Mesh>().Length,
            textures = Resources.FindObjectsOfTypeAll<Texture>().Length, materials = Resources.FindObjectsOfTypeAll<Material>().Length,
            avatars = Resources.FindObjectsOfTypeAll<Avatar>().Length
        };
        private static async Task ExpectFailure(Task operation)
        {
            bool failed = false; try { await operation; } catch (Exception) { failed = true; }
            Check(failed, "Expected an import failure.");
        }
        private static Task Frames(int count)
        {
            var done = new TaskCompletionSource<bool>(); int target = Time.frameCount + count;
            void Tick()
            {
                if (Time.frameCount < target && EditorApplication.isPlaying) return;
                EditorApplication.update -= Tick;
                if (EditorApplication.isPlaying) done.TrySetResult(true); else done.TrySetException(new OperationCanceledException("Play Mode stopped."));
            }
            EditorApplication.update += Tick; return done.Task;
        }
        private static async Task Settle()
        {
            await Frames(3); var unload = Resources.UnloadUnusedAssets();
            while (!unload.isDone) await Frames(1);
            GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); await Frames(3);
        }
        private static void Check(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
        private static string Argument(string name) => Environment.GetCommandLineArgs().SkipWhile(a => a != name).Skip(1).First();
        private static void Watchdog()
        {
            report.peakAllocated = Math.Max(report.peakAllocated, Profiler.GetTotalAllocatedMemoryLong());
            if (EditorApplication.timeSinceStartup - started > 300) { report.error = "Stability test timed out."; Finish(false); }
        }
        private static void Finish(bool success)
        {
            File.WriteAllText(Path.Combine(output, "stability-results.json"), JsonUtility.ToJson(report, true));
            SessionState.SetBool(Pending, false); EditorApplication.update -= Watchdog;
            Debug.Log(success ? "ISEKAID_VRM_STABILITY_PASSED" : "ISEKAID_VRM_STABILITY_FAILED: " + report.error);
            EditorApplication.Exit(success ? 0 : 1);
        }
    }
}
