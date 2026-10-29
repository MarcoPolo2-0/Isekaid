#if UNITY_EDITOR || DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Isekaid.ExperimentalVrm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Isekaid.Validation
{
    public static class F3ARuntimeChecks
    {
        [Serializable]
        public sealed class EntryRecord
        {
            public string fixture, id, eventType, personality, lifeState, expression;
            public int episode, trust, consumed;
        }
        [Serializable]
        public sealed class RouteRecord
        {
            public string personality, topic, root, option, target;
        }
        [Serializable]
        public sealed class Results
        {
            public bool passed;
            public int testFrameRate = 60;
            public string unity, platform, error, avatarHash, libraryId, sourceSha256;
            public List<string> checks = new List<string>();
            public List<EntryRecord> presentations = new List<EntryRecord>();
            public List<RouteRecord> routes = new List<RouteRecord>();
            public List<string> engineErrors = new List<string>();
            public List<string> limits = new List<string>();
            public List<string> criteria = new List<string>();
        }
        public static Results Report;
        private static Keyboard keyboard;
        private static CompanionPrototype prototype;
        private static CompanionController companion;
        private static DialogueDirector director;
        private static VisualElement root;
        private static string fixtureName;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static async void AutoRun()
        {
            if (Application.isEditor || !Debug.isDebugBuild || F2RuntimeChecks.Argument("-isekF3ATest") == null) return;
            try { await Run(); Write(true); Application.Quit(0); }
            catch (Exception error) { Report ??= new Results(); Report.error = error.ToString(); Write(false); Application.Quit(1); }
        }
        public static void Write(bool passed)
        {
            Report.passed = passed;
            Directory.CreateDirectory(AvatarFiles.Root);
            File.WriteAllText(Path.Combine(AvatarFiles.Root, "f3a-results.json"), JsonUtility.ToJson(Report, true));
            Debug.Log(passed ? "ISEKAID_F3A_PLAYMODE_CHECKS_PASSED" : "ISEKAID_F3A_PLAYMODE_CHECKS_FAILED");
        }
        private static void Check(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
            Report.checks.Add(message);
        }
        private static async Task Until(Func<bool> ready, string message)
        {
            var clock = System.Diagnostics.Stopwatch.StartNew();
            while (!ready()) { if (clock.Elapsed.TotalSeconds > 45) throw new TimeoutException(message); await Task.Delay(40); }
            await Task.Delay(100);
        }
        private static async Task Press(Key key, int milliseconds = 100)
        {
            int frame = Time.frameCount;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
            // A stalled frame must not receive both key edges before the player updates.
            await Until(() => timer.ElapsedMilliseconds >= milliseconds && Time.frameCount >= frame + 3, "Player did not process held input");
            frame = Time.frameCount;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            await Until(() => Time.frameCount >= frame + 2, "Player did not process released input");
        }
        private static void Submit(string name)
        {
            var button = root.Q<Button>(name);
            if (button == null) throw new InvalidOperationException("Missing dialogue button: " + name);
            button.Focus();
            using var submit = NavigationSubmitEvent.GetPooled();
            submit.target = button;
            button.SendEvent(submit);
        }
        private static void OnLog(string message, string stack, LogType type)
        {
            if (type == LogType.Exception && stack.Contains("UnityEditor.Search.SearchDatabase")) return;
            if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert) Report.engineErrors.Add(message + "\n" + stack);
        }
        private static int Count(string id) => Report.presentations.Count(record => record.fixture == fixtureName && record.id == id);
        private static void Record(DialogueEntry entry)
        {
            Report.presentations.Add(new EntryRecord { fixture = fixtureName, id = entry.id, eventType = entry.eventType,
                personality = companion.State.Personality.ToString(), lifeState = companion.State.LifeState.ToString(), episode = director.RecoveryEpisode,
                trust = companion.State.Trust, consumed = director.ConsumedCount, expression = companion.Reactions.AppliedExpression });
        }
        private static void Fresh(string name, CompanionPersonality personality = CompanionPersonality.Tsundere)
        {
            fixtureName = name;
            companion.SetPersonality(personality); prototype.Restart();
            Check(!director.Begin(), "Trapped rejects ordinary conversation: " + name);
            Check(companion.Interact() && companion.State.Trust == 1 && companion.State.RemembersFirstRescue, "Actual first rescue: " + name);
            Check(!companion.State.Rescue(), "Rescue cannot award trust twice: " + name);
        }

        public static async Task Run()
        {
            Report = new Results { unity = Application.unityVersion, platform = Application.platform.ToString(),
                libraryId = DialogueContent.Current.libraryId, sourceSha256 = DialogueContent.Current.sourceSha256 };
            if (F2RuntimeChecks.Argument("-isekTestData") == null) throw new InvalidOperationException("An isolated test data directory is required.");
            Application.runInBackground = true;
            int previousFrameRate = Application.targetFrameRate, previousSync = QualitySettings.vSyncCount;
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            var originalSettings = InputSystem.settings;
            var originalFlags = originalSettings.hideFlags;
            originalSettings.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            var settings = Object.Instantiate(originalSettings);
            settings.hideFlags = HideFlags.HideAndDontSave;
            settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            InputSystem.settings = settings;
            keyboard = InputSystem.AddDevice<Keyboard>();
            Application.logMessageReceived += OnLog;
            try
            {
                CheckContent();
                await Until(() => Object.FindAnyObjectByType<CharacterCreationController>()?.Appearance.Ready == true, "Creator did not load");
                var creator = Object.FindAnyObjectByType<CharacterCreationController>();
                creator.SelectCatalog(); creator.SetPersonality(CompanionPersonality.Tsundere);
                Check(creator.BeginGame(), "Existing F2 creator starts Game");
                await Until(() => Object.FindAnyObjectByType<CompanionPrototype>()?.Companion.GetComponent<CompanionAppearance>().Ready == true, "Game did not load");
                prototype = Object.FindAnyObjectByType<CompanionPrototype>(); companion = prototype.Companion; director = companion.Dialogue;
                root = Object.FindAnyObjectByType<PrototypeHud>().GetComponent<UIDocument>().rootVisualElement;
                director.Presented += Record;
                prototype.SetPaused(false);
                fixtureName = "real-input";
                Check(prototype.CanInteract(), "Original Game interaction ray reaches the real companion");
                await Press(Key.E, 900);
                Check(companion.State.LifeState == CompanionLifeState.Active && companion.State.Trust == 1, "Held E rescues through existing input");
                companion.Interact(); await Task.Delay(180);
                Check(root.Q<Label>(className: "dialogue").text == "", "Opening a topic menu retires the earlier transient rescue caption");
                director.Close();
                await TestRoutes();
                await TestCasualAndOrders();
                await TestEpisodes();
                await TestCreatorContinuity(true);
                await TestCreatorContinuity(false);
                var appearance = companion.GetComponent<CompanionAppearance>();
                companion.Reactions.Show(CompanionEmotion.Joy, -1, "happy", "reset");
                await Task.Delay(120);
                Check(appearance.Catalog.GetComponentsInChildren<SkinnedMeshRenderer>().Any(renderer => Enumerable.Range(0, renderer.sharedMesh.blendShapeCount)
                    .Any(index => renderer.GetBlendShapeWeight(index) > 0)), "Catalog uses actual expression weights");
                companion.Reactions.Clear();
                Check(companion.State.Trust == 1, "Expression samples do not award relationship");
                director.Close(); companion.Incapacitate(); Time.timeScale = 30;
                await Until(() => companion.State.LifeState == CompanionLifeState.Dead, "Death did not resolve");
                Check(prototype.IsPaused && !director.Begin() && !companion.Interact() && !director.Active && director.Current == null,
                    "Death discards invalid cursor and preserves game over");
                prototype.Restart();
                Check(director.ConsumedCount == 0 && director.RecoveryEpisode == 0 && !director.Active && companion.State.LifeState == CompanionLifeState.Trapped && prototype.Player.InputEnabled,
                    "Explicit restart resets only the encounter");
                Check(Report.engineErrors.Count == 0, "No runtime errors in revised F3A scenario");
                foreach (string id in new[] { "R01", "R02", "R03", "R04", "R05", "R06", "R07", "R08", "R09", "R11", "R12" }) Report.criteria.Add("F3A-" + id + ": PASS (bounded fixture; see report)");
                Report.criteria.Add("F3A-R10: PARTIAL (distinct canonical text verified; human rhythm/posture/editorial review pending)");
                Report.limits.Add("OS cursor capture and physical mouse clicks require manual testing in a focused window.");
                Report.limits.Add("Conditional soft/relaxed expressions use neutral until visual certification; no new gesture clips or danger detector.");
                Report.limits.Add("Test scenario is capped at 60 FPS; the unchanged player prefab discards movement steps below 0.001 m at extreme uncapped background rates.");
            }
            finally
            {
                if (director != null) director.Presented -= Record;
                Application.logMessageReceived -= OnLog;
                if (keyboard != null) InputSystem.RemoveDevice(keyboard);
                if (originalSettings != null) { InputSystem.settings = originalSettings; originalSettings.hideFlags = originalFlags; }
                Object.Destroy(settings);
                Application.targetFrameRate = previousFrameRate;
                QualitySettings.vSyncCount = previousSync;
            }
        }

        private static async Task TestRoutes()
        {
            foreach (CompanionPersonality personality in Enum.GetValues(typeof(CompanionPersonality)))
            foreach (var topic in DialogueContent.Current.topics)
            for (int branch = 0; branch < 2; branch++)
            {
                Fresh("route-" + personality + "-" + topic.id + "-" + branch, personality);
                Check(companion.Interact() && director.Current == null && director.Choices.Count == 3, "Talk opens topic menu: " + fixtureName);
                Check(director.Choose(topic.id), "Topic selection: " + fixtureName);
                string rootId = director.Current.id;
                var option = director.Choices[branch];
                Check(director.Choices.Count == 2 && root.Q<Label>("conversation-text").text == director.Current.text, "Canonical root and two options: " + fixtureName);
                Check(!director.Choose("not-a-choice"), "Foreign option rejected: " + fixtureName);
                if (personality == CompanionPersonality.Tsundere && topic.id == "F3A.TREE.TRAP" && branch == 0)
                {
                    root.Q<Button>(director.Choices[1].id).Focus();
                    string focus = director.FocusedOptionId;
                    Submit("conversation-close");
                    Check(!director.Active && prototype.Player.InputEnabled && director.Current.id == rootId && director.Choices.Count == 2, "Voluntary close retains pending root and restores movement");
                    var position = prototype.Player.transform.position;
                    int moveFrame = Time.frameCount;
                    await Press(Key.W, 250);
                    float moved = Vector3.Distance(position, prototype.Player.transform.position);
                    Check(moved > .15f, "W moves after closing dialogue: meters=" + moved + ", frames=" + (Time.frameCount - moveFrame) +
                        ", input=" + prototype.Player.InputEnabled + ", paused=" + prototype.IsPaused + ", scale=" + Time.timeScale +
                        ", delta=" + Time.deltaTime + ", minMove=" + prototype.Player.GetComponent<CharacterController>().minMoveDistance);
                    prototype.Player.ResetPosition();
                    Check(companion.Interact() && director.Current.id == rootId && director.FocusedOptionId == focus && Count(rootId) == 1, "Explicit reopen retains options/focus without respoken root");
                    foreach (var size in new[] { new Vector2Int(1920,1080), new Vector2Int(1600,900), new Vector2Int(1280,720) })
                    { await F2RuntimeChecks.Capture("canonical-root-" + size.x, size.x, size.y); CheckLayout(size.x); }
                    int before = Count(rootId);
                    await Press(Key.Tab); float clock = director.ActiveSeconds;
                    await Task.Delay(250);
                    Check(prototype.IsPaused && director.Suspended && director.ActiveSeconds == clock && !director.Choose(option.id), "Pause freezes active clock and choices");
                    await Press(Key.Tab);
                    Check(director.Active && !prototype.Player.InputEnabled && Count(rootId) == before, "Resume preserves root without Spoke duplication");
                    await Press(Key.Digit1);
                }
                else Submit(option.id);
                Check(director.Current.id == option.target && director.Choices.Count == 0 && companion.State.Trust == 1 && companion.IsFollowing,
                    "Selected leaf only; no trust/order effects: " + fixtureName);
                Check(!director.Choose(option.id), "Double choice cannot produce a second leaf: " + fixtureName);
                Report.routes.Add(new RouteRecord { personality = personality.ToString(), topic = topic.id, root = rootId, option = option.id, target = director.Current.id });
                if (topic.id == "F3A.TREE.TRAP" && branch == 0) await F2RuntimeChecks.Capture("canonical-answer-" + personality);
                Submit("conversation-continue");
                Check(director.Current == null && director.Active && director.Choices.Count == 2 && director.Choices.All(choice => choice.id != topic.id), "Completed leaf returns to remaining topics: " + fixtureName);
                await Press(Key.Escape);
                Check(!director.Active && !prototype.IsPaused && prototype.Player.InputEnabled, "Escape ends without pause: " + fixtureName);
            }
            Check(Report.routes.Count == 18 && Report.routes.Select(route => route.option).Distinct().Count() == 18, "All 18 canonical alternative routes verified independently");
        }

        private static async Task TestCasualAndOrders()
        {
            Fresh("casual-and-orders");
            companion.Interact();
            foreach (var topic in DialogueContent.Current.topics)
            { director.Choose(topic.id); director.Choose(director.Choices[0].id); director.Continue(); }
            Check(director.Choices.Count == 0, "All three completed topics leave the menu");
            var seen = new HashSet<string>();
            for (int i = 0; i < 3; i++)
            {
                if (i > 0)
                {
                    Check(!director.CanChat && !director.ChatCasually(), "Casual group cannot bypass its shared cooldown: " + i);
                    Time.timeScale = 20;
                    await Until(() => director.CanChat, "Active cooldown did not elapse");
                    Time.timeScale = 1;
                }
                Check(director.ChatCasually() && seen.Add(director.Current.id), "First unconsumed canonical casual variant: " + i);
                director.Continue();
            }
            Check(!director.CanChat && !director.HasTopics && !director.ChatCasually() && root.Q<Label>("conversation-text").text.Contains("No hay temas nuevos"), "Exhaustion uses UI silence instead of an NPC loop");
            director.Close();
            companion.SetFollowing(false); int count = Report.presentations.Count;
            companion.SetFollowing(false);
            Check(Report.presentations.Count == count && !companion.IsFollowing, "Repeated no-op wait has no repeated acceptance");
            companion.SetFollowing(true); count = Report.presentations.Count;
            companion.SetFollowing(true);
            Check(Report.presentations.Count == count && companion.IsFollowing, "Repeated no-op follow has no repeated acceptance");
            companion.SetFollowing(false);
            Check(Report.presentations.Last().id.EndsWith("D.WAIT_OPINION") && !companion.IsFollowing && companion.State.Trust == 1, "Deterministic wait opinion respects the actual order");
        }

        private static async Task TestEpisodes()
        {
            Fresh("two-recovery-episodes"); companion.Interact(); director.Choose("F3A.TREE.TRAP");
            string rootId = director.Current.id;
            root.Q<Button>(director.Choices[1].id).Focus(); string focus = director.FocusedOptionId;
            companion.Incapacitate(); int count = Report.presentations.Count;
            director.Downed();
            director.Interrupt(DialoguePriority.Survival); director.Interrupt(DialoguePriority.Survival);
            Check(!director.Active && director.Current.id == rootId && prototype.Player.InputEnabled && Report.presentations.Count == count, "Downed preempts and deduplicates while retaining cursor");
            prototype.SetPaused(true); float seconds = companion.State.RescueSecondsRemaining;
            await Task.Delay(250);
            Check(companion.State.RescueSecondsRemaining == seconds, "Paused frames preserve rescue timer");
            prototype.SetPaused(false); companion.Interact(); count = Report.presentations.Count; director.Revived();
            Check(Report.presentations.Count == count && companion.State.Trust == 1 && companion.State.RemembersFirstRescue, "First revival has no duplicate reward or gratitude");
            companion.Interact();
            Check(director.Current.eventType == "ResumeConversationRequested", "Eligible explicit talk presents canonical recovery bridge");
            director.Continue();
            Check(director.Current.id == rootId && director.Choices.Count == 2 && director.FocusedOptionId == focus && Count(rootId) == 1, "Recovery resumes the pending root/options/focus without another root presentation");
            director.Choose(director.Choices[0].id); director.Continue(); director.Close();
            companion.Incapacitate(); companion.Interact(); companion.Interact();
            Check(director.RecoveryEpisode == 2 && director.Current.eventType == "SafeReturnTalkRequested", "Second independent recovery gets its own eligible comment");
            Check(Report.presentations.Count(record => record.fixture == fixtureName && record.eventType == "DownedEntered") == 2 &&
                Report.presentations.Count(record => record.fixture == fixtureName && record.eventType == "ReanimationCompleted") == 2 &&
                Report.presentations.Count(record => record.fixture == fixtureName && record.eventType == "FirstRescueCompleted") == 1 && companion.State.Trust == 1,
                "Two downed/revived pairs preserve a single first-rescue memory");
            director.Continue(); director.Choose("F3A.TREE.FEEL"); director.Close();
            companion.Incapacitate(); companion.Interact();
            Check(!director.Active && prototype.Player.InputEnabled, "Voluntarily closed dialogue is not forced open after recovery");
            companion.Interact(); rootId = director.Current.id; count = Count(rootId);
            director.Interrupt(DialoguePriority.Survival);
            Check(!director.Active && prototype.Player.InputEnabled && director.Current.id == rootId, "Explicit injected priority fixture suspends a valid cursor without inventing danger");
            companion.Interact();
            Check(director.Current.id == rootId && Count(rootId) == count, "Explicit re-entry after injected eligibility loss does not re-emit root");
        }

        private static async Task TestCreatorContinuity(bool import)
        {
            if (import) { Fresh("creator-continuity"); companion.Interact(); director.Choose("F3A.TREE.TRAP"); }
            else { director.Continue(); director.Choose("F3A.TREE.FEEL"); }
            var liveState = companion.State;
            string currentId = director.Current.id;
            string[] options = director.Choices.Select(option => option.id).ToArray();
            root.Q<Button>(options[1]).Focus(); string focus = director.FocusedOptionId;
            int consumed = director.ConsumedCount, staleRevision = director.Revision, spoken = Count(currentId);
            prototype.SetPaused(true); prototype.OpenCharacterCreation();
            await Until(() => Object.FindAnyObjectByType<CharacterCreationController>()?.Appearance.Ready == true, "Additive creator failed");
            var creator = Object.FindAnyObjectByType<CharacterCreationController>();
            creator.SetPersonality(companion.State.Personality);
            await Press(Key.Tab);
            Check(prototype.IsPaused && Time.timeScale == 0 && !prototype.Player.InputEnabled, "Game shortcuts cannot unpause behind creator");
            for (int i = 0; i < 2; i++) { creator.Appearance.PreviewExpression("happy"); creator.CancelImport(); }
            Check(director.ConsumedCount == consumed && companion.State.Trust == 1 && director.Current.id == currentId, "Creator previews/cancel do not consume narrative content");
            if (import)
            {
                string path = F2RuntimeChecks.Argument("-isekVrmFixture");
                if (string.IsNullOrEmpty(path)) throw new InvalidOperationException("A private VRM fixture is required.");
                await creator.ImportAvatarAsync(path, true); Report.avatarHash = creator.Appearance.Selection.hash;
            }
            else creator.SelectCatalog();
            Check(creator.BeginGame(), "Existing F2 commit succeeds with pending conversation: " + import);
            await Until(() => CompanionPrototype.Editing == null && Object.FindAnyObjectByType<CharacterCreationController>() == null, "Return from creator failed");
            prototype.SetPaused(false);
            Check(ReferenceEquals(liveState, companion.State) && director.Active && director.Current.id == currentId && director.Choices.Select(option => option.id).SequenceEqual(options) &&
                director.FocusedOptionId == focus && director.ConsumedCount == consumed && Count(currentId) == spoken && liveState.Trust == 1 && liveState.RemembersFirstRescue,
                "Appearance change preserves cursor, pending choices, focus, consumption and relationship: " + import);
            Check(!director.Choose(options[1], staleRevision) && director.Current.id == currentId, "Stale visual revision cannot select a leaf after avatar change");
            var appearance = companion.GetComponent<CompanionAppearance>();
            Check(appearance.Importer.HasAvatar == import, "Actual Game appearance matches the committed selection: " + import);
            if (import)
            {
                Check(CompanionProfileStore.LoadRecord().appearance.hash == Report.avatarHash, "Existing F2 persistence retains the imported avatar");
                companion.Reactions.Show(CompanionEmotion.Joy, -1, "happy", "reset"); await Task.Delay(120);
                Check(appearance.Importer.Avatar.Runtime.Expression.GetWeight(UniVRM10.ExpressionKey.CreateFromPreset(UniVRM10.ExpressionPreset.happy)) > .9f, "VRM receives actual preferred happy weight");
                companion.Reactions.Clear();
                Check(appearance.Importer.Avatar.Runtime.Expression.GetWeight(UniVRM10.ExpressionKey.CreateFromPreset(UniVRM10.ExpressionPreset.happy)) == 0, "Temporary VRM weights clear safely");
                await F2RuntimeChecks.Capture("canonical-vrm-private");
            }
            string target = director.Choices[1].target;
            Submit(options[1]);
            Check(director.Current.id == target && Count(currentId) == spoken && companion.State.Trust == 1, "Preserved option selects exactly its canonical leaf after avatar change: " + import);
        }

        private static void CheckContent()
        {
            var content = DialogueContent.Current; content.Validate();
            Check(content.entries.Length == 60 && content.topics.Length == 3 && content.entries.Count(entry => entry.eventType == "TopicSelected") == 9 &&
                content.entries.Sum(entry => entry.options?.Length ?? 0) == 18, "Validated canonical library has 60 entries, 3 topics, 9 roots and 18 alternatives");
            Check(Enumerable.Range(0,3).All(personality => content.entries.Count(entry => entry.personality == personality) == 20), "Each personality has exactly 20 canonical entries");
            var invalid = JsonUtility.FromJson<DialogueContent>(JsonUtility.ToJson(content)); invalid.topics[0].roots[0] = "missing";
            bool rejected = false; try { invalid.Validate(); } catch (InvalidOperationException) { rejected = true; }
            Check(rejected, "Runtime validation rejects a broken topic reference");
            foreach (CompanionEmotion emotion in Enum.GetValues(typeof(CompanionEmotion)))
            {
                var requests = new List<string>();
                var presenter = new CompanionReactionPresenter(name => { requests.Add(name); return name == "neutral"; });
                presenter.Show(emotion);
                Check(presenter.AppliedExpression == "neutral" && requests.Last() == "neutral", "Avatar without emotional channels uses safe neutral: " + emotion);
            }
            var conditional = new CompanionReactionPresenter(_ => true);
            conditional.Show(CompanionEmotion.Concern, -1, "sad_soft_if_validated", "reset");
            Check(conditional.UsedFallback && conditional.AppliedExpression == "neutral" && conditional.Emotion == CompanionEmotion.Concern,
                "Uncertified soft expression preserves emotion/text using neutral instead of inventing validation");
        }

        private static void CheckLayout(int width)
        {
            var panel = root.Q("conversation"); var bounds = panel.worldBound;
            Check(bounds.width > 0 && bounds.xMin >= 0 && bounds.yMin >= 0 && bounds.xMax <= root.worldBound.xMax + 1 && bounds.yMax <= root.worldBound.yMax + 1, "Canonical dialogue inside viewport: " + width);
            foreach (var label in panel.Query<Label>().ToList())
            {
                var measured = label.MeasureTextSize(label.text, label.contentRect.width, VisualElement.MeasureMode.Exactly, 0, VisualElement.MeasureMode.Undefined);
                Check(label.contentRect.height + 2 >= measured.y, "Canonical text not clipped: " + width + "/" + label.name);
            }
            var buttons = panel.Query<Button>().ToList().Where(button => button.resolvedStyle.display != DisplayStyle.None).ToList();
            Check(buttons.All(button => bounds.Contains(button.worldBound.min) && bounds.Contains(button.worldBound.max)) &&
                buttons.Zip(buttons.Skip(1), (a,b) => a.worldBound.yMax <= b.worldBound.yMin + 1).All(value => value), "Canonical choices do not overlap: " + width);
        }
    }
}
#endif
