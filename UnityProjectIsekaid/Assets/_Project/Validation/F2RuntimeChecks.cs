#if UNITY_EDITOR || DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Isekaid.ExperimentalVrm;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Isekaid.Validation
{
    public static class F2RuntimeChecks
    {
        [Serializable]
        public sealed class Results
        {
            public string unity, platform, avatarHash, error;
            public bool passed;
            public int releasedResources;
            public float importMilliseconds, averageFrameMilliseconds;
            public float previewHeight, previewFeetOffset;
            public long memoryBefore, memoryAfter, peakMemory;
            public List<string> checks = new List<string>();
        }
        public static Results Report;
        public static Action<int, int> ResizeForEditor;
        public static string Argument(string key)
        {
            var args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, key);
            return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static async void AutoRun()
        {
            if (!Debug.isDebugBuild || Argument("-isekF2Test") == null) return;
            try { await Run(); Write(true); Application.Quit(0); }
            catch (Exception error) { Report.error = error.ToString(); Write(false); Application.Quit(1); }
        }
        public static void Write(bool passed)
        {
            Report.passed = passed;
            Directory.CreateDirectory(AvatarFiles.Root);
            File.WriteAllText(Path.Combine(AvatarFiles.Root, "results.json"), JsonUtility.ToJson(Report, true));
            Debug.Log(passed ? "ISEKAID_F2_RUNTIME_CHECKS_PASSED" : "ISEKAID_F2_RUNTIME_CHECKS_FAILED");
        }
        private static void Check(bool condition, string name)
        {
            if (!condition) throw new InvalidOperationException(name);
            Report.checks.Add(name);
        }
        private static async Task Until(Func<bool> ready, string name, int milliseconds = 45000)
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            while (!ready())
            { if (timer.ElapsedMilliseconds > milliseconds) throw new TimeoutException(name); await Task.Delay(40); }
            await Task.Delay(150);
        }
        public static async Task Capture(string filename, int width = 1920, int height = 1080)
        {
            if (ResizeForEditor != null) ResizeForEditor(width, height);
            else Screen.SetResolution(width, height, FullScreenMode.Windowed);
            await Task.Delay(600);
            string directory = Argument("-isekCaptureDir") ?? Path.Combine(AvatarFiles.Root, "PrivateCaptures");
            Directory.CreateDirectory(directory);
            ScreenCapture.CaptureScreenshot(Path.Combine(directory, filename + ".png"));
            await Task.Delay(700);
        }
        public static async Task Run()
        {
            Report = new Results { unity = Application.unityVersion, platform = Application.platform.ToString() };
            if (Argument("-isekTestData") == null) throw new InvalidOperationException("An isolated test data directory is required.");
            Application.runInBackground = true;
            await Until(() => Object.FindAnyObjectByType<CharacterCreationController>()?.Appearance.Ready == true, "Creator did not initialize");
            var creator = Object.FindAnyObjectByType<CharacterCreationController>();
            var personalityField = creator.GetComponent<UIDocument>().rootVisualElement.Query<DropdownField>().ToList()
                .FirstOrDefault(field => field.choices.SequenceEqual(CompanionPalette.PersonalityNames));
            Check(personalityField != null && personalityField.choices.SequenceEqual(new[] { "Tsundere", "Yandere", "Dandere" }) &&
                (int)CompanionPersonality.Shy == 2, "Actual creator shows three personality labels with Dandere at Shy index 2");
            if (Argument("-isekF2Test") == "restart")
            {
                Check(creator.Profile.personality == CompanionPersonality.Shy && personalityField.value == "Dandere" &&
                    (int)CompanionProfileStore.Load().personality == 2, "New process preserves the saved Shy value behind the Dandere label");
                Check(creator.Appearance.Selection.kind == "vrm" && creator.Appearance.Importer.HasAvatar, "A new process reloads the managed VRM without the source file");
                Report.avatarHash = creator.Appearance.Selection.hash;
                await Capture("restart-managed-vrm");
                Check(creator.BeginGame(), "Restarted creator starts Game");
                await Until(() => Object.FindAnyObjectByType<CompanionPrototype>()?.Companion.GetComponent<CompanionAppearance>().Importer?.HasAvatar == true, "Restarted Game did not load VRM");
                await Capture("restart-game-vrm");
                return;
            }
            Check(creator.Appearance.Selection.kind == "catalog" && creator.Appearance.Catalog.activeInHierarchy, "Offline included catalog is available");
            Check(Object.FindAnyObjectByType<CompanionController>() == null, "Creator previews do not simulate a companion");
            string identity = creator.Profile.identity;
            foreach (var value in new[] { CompanionPersonality.Tsundere, CompanionPersonality.Yandere, CompanionPersonality.Shy })
            { creator.SetPersonality(value); Check(creator.Profile.personality == value && creator.Appearance.Selection.kind == "catalog", "Catalog personality independent: " + value); }
            personalityField.index = 0;
            Check(creator.Profile.personality == CompanionPersonality.Tsundere, "Actual personality selector dispatches the Tsundere choice");
            personalityField.index = 2;
            Check(personalityField.value == "Dandere" && creator.Profile.personality == CompanionPersonality.Shy,
                "Visible Dandere selection binds to the existing Shy value");
            foreach (string value in new[] { "Idle", "Walk", "Run" }) { creator.Appearance.PreviewMotion(value); await Task.Delay(150); }
            Check(creator.Appearance.PreviewExpression("happy") && creator.Appearance.PreviewExpression("blink"), "Catalog expressions use inspected channel names");
            creator.Appearance.PreviewExpression("neutral");
            creator.Appearance.PreviewMotion("Idle");
            await Capture("creator-catalog-1920");
            await Capture("creator-catalog-1280", 1280, 720);
            await Capture("creator-catalog-1600", 1600, 900);
            creator.SetPersonality(CompanionPersonality.Yandere);
            creator.SetColor(0, 2); creator.SetColor(1, 3); creator.SetColor(2, 1); creator.SetColor(3, 2);
            Check(creator.BeginGame(), "Catalog creator saves and starts Game");
            await Until(() => Object.FindAnyObjectByType<CompanionPrototype>()?.Companion.GetComponent<CompanionAppearance>().Ready == true, "Catalog Game did not load");
            var prototype = Object.FindAnyObjectByType<CompanionPrototype>();
            prototype.SetPaused(false);
            var companion = prototype.Companion;
            var presentation = companion.GetComponent<CompanionAppearance>();
            var state = companion.State;
            var agent = companion.GetComponent<NavMeshAgent>();
            float speed = agent.speed, radius = agent.radius;
            Check(presentation.Selection.kind == "catalog" && state.Personality == CompanionPersonality.Yandere, "Same catalog appearance and personality reach Game");
            await Capture("game-catalog");
            companion.Interact(); companion.SetFollowing(false);
            Check(state.Trust == 1 && state.RemembersFirstRescue, "Existing rescue and memory still work");
            prototype.SetPaused(true);
            prototype.OpenCharacterCreation();
            await Until(() => Object.FindAnyObjectByType<CharacterCreationController>()?.Appearance.Ready == true, "Additive creator did not load");
            creator = Object.FindAnyObjectByType<CharacterCreationController>();
            Check(ReferenceEquals(state, companion.State) && Time.timeScale == 0 && companion.State.Trust == 1, "Creator preserves the paused encounter and state instance");
            string source = Path.Combine(AvatarFiles.Root, "source-copy.vrm");
            File.Copy(Argument("-isekVrmFixture"), source, true);
            if (Argument("-isekPickerExpected") != null)
            {
                string picked = WindowsAvatarPicker.Pick(AvatarFiles.Root);
                File.WriteAllText(Path.Combine(AvatarFiles.Root, "picker-path.txt"), picked ?? "<cancelled>");
                Check(string.Equals(picked, source, StringComparison.OrdinalIgnoreCase), "Native Windows picker returns the selected local file");
                File.WriteAllText(Path.Combine(AvatarFiles.Root, "picker-selected.ok"), "PASS");
                Check(WindowsAvatarPicker.Pick() == null, "Native Windows picker cancellation returns no selection");
            }
            var sourceBytes = File.ReadAllBytes(source);
            Report.avatarHash = AvatarFiles.Hash(sourceBytes);
            var view = creator.Appearance;
            await creator.ImportAvatarAsync(source, true);
            Check(view.Selection.hash == Report.avatarHash && view.Importer.HasAvatar, "Local VRM is prepared in the creator");
            var metadata = JsonUtility.FromJson<AvatarPermissions>(view.Selection.metadataJson);
            Check(metadata.authors != null && metadata.authors.Length > 0 && metadata.avatarPermission == "onlyAuthor",
                "Actual fixture authors and restricted permissions are displayed from the VRM");
            await Task.Delay(200);
            var renderers = view.Importer.Avatar.GetComponentsInChildren<Renderer>();
            var bounds = renderers[0].bounds; foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);
            Report.previewHeight = bounds.size.y;
            Report.previewFeetOffset = bounds.min.y - view.Importer.Anchor.position.y;
            Check(Mathf.Abs(Report.previewFeetOffset) < .1f && bounds.size.y > 1.3f && bounds.size.y < 2.2f,
                "Prepared VRM fits the stage after animation and control rig evaluation");
            Check(creator.Profile.identity == identity && companion.State.Trust == 1, "VRM preparation preserves identity and live memory");
            Report.importMilliseconds = view.Importer.ImportMilliseconds;
            var previous = view.Importer.Avatar;
            using (var cancel = new CancellationTokenSource())
            {
                cancel.Cancel();
                try { await view.LoadVrm(source, true, cancel.Token); throw new InvalidOperationException("Cancelled import succeeded"); }
                catch (OperationCanceledException) { }
            }
            Check(view.Importer.Avatar == previous, "Cancelled import preserves the previous avatar");
            var pendingImport = creator.ImportAvatarAsync(source, true);
            creator.CancelImport();
            bool cancelledDuringLoad = false;
            try { await pendingImport; } catch (OperationCanceledException) { cancelledDuringLoad = true; }
            Check(cancelledDuringLoad && view.Importer.Avatar == previous && !view.Busy, "Cancelling an in-flight import preserves the previous avatar");
#if UNITY_EDITOR
            foreach (string fault in new[] { "ValidatedBytes", "Imported", "Prepared", "Activated", "PreviousHidden" })
            {
                view.Importer.ImportStageProbe = (stage, candidate) => { if (stage == fault) throw new IOException("Injected preparation failure"); };
                bool failed = false;
                try { await creator.ImportAvatarAsync(source, true); } catch (IOException) { failed = true; }
                view.Importer.ImportStageProbe = null;
                Check(failed && view.Importer.Avatar == previous && previous.gameObject.activeInHierarchy, "Transactional rollback at " + fault);
            }
#endif
            bool rejected = false;
            string invalid = Path.Combine(AvatarFiles.Root, "invalid.vrm"); File.WriteAllBytes(invalid, new byte[32]);
            try { await creator.ImportAvatarAsync(invalid, true); } catch (Exception) { rejected = true; }
            Check(rejected && view.Importer.Avatar == previous, "False or truncated VRM does not replace the previous avatar");
            var warmup = creator.ImportAvatarAsync(source, true);
            bool concurrentRejected = false;
            try { await creator.ImportAvatarAsync(source, true); } catch (InvalidOperationException) { concurrentRejected = true; }
            await warmup;
            Check(concurrentRejected || warmup.IsCompletedSuccessfully, "Consecutive requests are serialized or superseded safely");
            Report.memoryBefore = Profiler.GetTotalAllocatedMemoryLong();
            for (int cycle = 0; cycle < 3; cycle++)
            {
                var resources = view.Importer.Avatar.GetComponent<UniGLTF.RuntimeGltfInstance>().RuntimeResources.Select(pair => pair.Item2).ToArray();
                creator.SelectCatalog();
                await Task.Delay(350);
                Check(resources.All(item => item == null), "Owned resources released on catalog replacement cycle " + cycle);
                Report.releasedResources += resources.Length;
                await creator.ImportAvatarAsync(source, true);
                var unload = Resources.UnloadUnusedAssets();
                await Until(() => unload.isDone, "Unused assets did not unload");
                GC.Collect(); await Task.Delay(150);
                Report.peakMemory = Math.Max(Report.peakMemory, Profiler.GetTotalAllocatedMemoryLong());
            }
            Report.memoryAfter = Profiler.GetTotalAllocatedMemoryLong();
            foreach (var value in new[] { CompanionPersonality.Tsundere, CompanionPersonality.Yandere, CompanionPersonality.Shy })
            { creator.SetPersonality(value); Check(view.Selection.hash == Report.avatarHash && creator.Profile.personality == value, "VRM personality independent: " + value); }
            foreach (string expression in view.Importer.Expressions) view.PreviewExpression(expression);
            Check(!view.PreviewExpression("definitely-missing"), "Missing expressions safely reset to neutral");
            view.PreviewExpression("happy");
            var frameTimer = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 60; i++)
            { int frame = Time.frameCount; while (frame == Time.frameCount) await Task.Delay(1); }
            Report.averageFrameMilliseconds = (float)frameTimer.Elapsed.TotalMilliseconds / 60;
            await Capture("creator-vrm-private");
            var originalRecord = File.ReadAllBytes(CompanionProfileStore.SavePath);
            CompanionProfileStore.BeforeCommit = () => throw new IOException("Injected disk failure");
            Check(!creator.BeginGame() && originalRecord.SequenceEqual(File.ReadAllBytes(CompanionProfileStore.SavePath)), "Failed save preserves the exact previous record");
            CompanionProfileStore.BeforeCommit = null;
#if UNITY_EDITOR
            presentation.Importer.ImportStageProbe = (stage, candidate) => { if (stage == "Activated") throw new IOException("Injected Game activation failure"); };
            Check(creator.BeginGame(), "Live replacement starts with a fully prepared draft");
            await Until(() => creator.Feedback.Contains("No se pudo actualizar"), "Live replacement failure did not return to the creator");
            Check(presentation.Selection.kind == "catalog" && !presentation.Importer.HasAvatar &&
                presentation.Catalog.GetComponentsInChildren<Renderer>().All(renderer => renderer.enabled) &&
                CompanionProfileStore.LoadRecord().appearance.kind == "catalog" && companion.State.Trust == 1,
                "Failed Game activation preserves the previous appearance, saved record and encounter");
            presentation.Importer.ImportStageProbe = null;
#endif
            Check(creator.BeginGame(), "VRM creator saves and resumes Game");
            await Until(() => CompanionPrototype.Editing == null && Object.FindAnyObjectByType<CharacterCreationController>() == null, "Game did not resume");
            Check(prototype.Companion == companion && ReferenceEquals(state, companion.State) && state.Trust == 1 && state.RemembersFirstRescue, "Appearance change preserves live companion, trust and first rescue");
            Check(agent.speed == speed && agent.radius == radius && presentation.Importer.HasAvatar, "VRM changes presentation without changing movement or navigation radius");
            var hud = Object.FindAnyObjectByType<PrototypeHud>().GetComponent<UIDocument>().rootVisualElement;
            Check(hud.Q<VisualElement>("header") != null && hud.resolvedStyle.display != DisplayStyle.None,
                "Existing Game HUD remains visible and populated after returning from the creator");
            Check(CompanionProfileStore.Load().identity == identity, "Identity survives save and appearance changes");
            File.Delete(source);
            await presentation.LoadSaved();
            Check(presentation.Importer.HasAvatar, "Managed copy loads after the source file is deleted");
            await Capture("game-vrm-private");
            var saved = CompanionProfileStore.LoadRecord();
            string managed = AvatarFiles.ManagedPath(saved.appearance);
            File.WriteAllBytes(managed, new byte[32]);
            await presentation.LoadSaved();
            Check(presentation.RecoveredFallback && !presentation.Importer.HasAvatar && companion.State.Trust == 1 && CompanionProfileStore.LoadRecord().appearance.hash == saved.appearance.hash,
                "Corrupt managed file falls back visibly without erasing state or reference");
            File.Delete(managed);
            await presentation.LoadSaved();
            Check(presentation.RecoveredFallback && companion.State.Trust == 1, "Missing managed file preserves narrative state");
            AvatarFiles.Admit(saved.appearance, sourceBytes);
            await presentation.LoadSaved();
            Check(presentation.Importer.HasAvatar && !presentation.RecoveredFallback, "Reimported managed copy recovers the selected avatar");
            prototype.SetPaused(false); companion.Incapacitate(); prototype.SetPaused(true);
            prototype.OpenCharacterCreation();
            Check(CompanionPrototype.Editing == null && state.LifeState == CompanionLifeState.Downed && state.RescueSecondsRemaining == 30,
                "Creator is deferred during incapacitation without resetting its timer");
        }
    }
}
#endif
