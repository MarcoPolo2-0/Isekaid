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
using Cursor = UnityEngine.Cursor;

namespace Isekaid.Validation
{
    public static class F3ARuntimeChecks
    {
        [Serializable]
        public sealed class Results
        {
            public bool passed;
            public string unity, platform, error, avatarHash;
            public List<string> checks = new List<string>();
            public List<string> rescueVariants = new List<string>();
            public List<string> answerVariants = new List<string>();
            public List<string> engineErrors = new List<string>();
            public List<string> expressionMappings = new List<string>();
            public List<string> limits = new List<string>();
        }
        public static Results Report;
        private static Keyboard keyboard;

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
            while (!ready()) { if (clock.Elapsed.TotalSeconds > 45) throw new TimeoutException(message); await Task.Delay(50); }
            await Task.Delay(200);
        }
        private static async Task Press(Key key, int milliseconds = 100)
        {
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
            await Task.Delay(milliseconds);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            await Task.Delay(100);
        }
        private static void Submit(Button button)
        {
            if (button == null) throw new InvalidOperationException("Missing dialogue button");
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

        public static async Task Run()
        {
            Report = new Results { unity = Application.unityVersion, platform = Application.platform.ToString() };
            if (F2RuntimeChecks.Argument("-isekTestData") == null) throw new InvalidOperationException("An isolated test data directory is required.");
            Application.runInBackground = true;
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
                CheckContentAndFallbacks();
                await Until(() => Object.FindAnyObjectByType<CharacterCreationController>()?.Appearance.Ready == true, "Creator did not load");
                var creator = Object.FindAnyObjectByType<CharacterCreationController>();
                creator.SelectCatalog(); creator.SetPersonality(CompanionPersonality.Tsundere);
                Check(creator.BeginGame(), "The existing creator starts Game with catalog");
                await Until(() => Object.FindAnyObjectByType<CompanionPrototype>()?.Companion.GetComponent<CompanionAppearance>().Ready == true, "Game did not load");
                var prototype = Object.FindAnyObjectByType<CompanionPrototype>();
                var companion = prototype.Companion;
                var director = companion.Dialogue;
                var appearance = companion.GetComponent<CompanionAppearance>();
                var root = Object.FindAnyObjectByType<PrototypeHud>().GetComponent<UIDocument>().rootVisualElement;
                prototype.SetPaused(false);
                Check(!director.Begin() && companion.State.LifeState == CompanionLifeState.Trapped, "Trapped state rejects casual dialogue");
                string lastSpoken = null;
                int spokenCount = 0;
                companion.Spoke += line => { lastSpoken = line; spokenCount++; };

                foreach (var personality in new[] { CompanionPersonality.Tsundere, CompanionPersonality.Yandere, CompanionPersonality.Shy })
                {
                    companion.SetPersonality(personality); prototype.Restart();
                    Check(director.ConsumedCount == 0 && !director.Active && companion.State.Trust == 0, "Reset clears only the current encounter: " + personality);
                    if (personality == CompanionPersonality.Tsundere)
                    {
                        Check(prototype.CanInteract(), "Original Game ray reaches the real companion");
                        await Press(Key.E, 900);
                        Check(companion.State.LifeState == CompanionLifeState.Active, "Held E rescues through the existing input action");
                    }
                    else Check(companion.Interact(), "Rescue resolves: " + personality);
                    Report.rescueVariants.Add(lastSpoken);
                    Check(companion.State.RemembersFirstRescue && companion.State.Trust == 1 && !companion.State.Rescue(), "First rescue is unique: " + personality);
                    Check(companion.Interact() && director.Current.id == "talk.first", "Active interaction opens first dialogue: " + personality);
                    await Task.Delay(100);
                    Check(!prototype.Player.InputEnabled && Cursor.visible && Time.timeScale == 1 && director.Choices.Count == 3,
                        "Dialogue owns cursor and movement without pausing gameplay: " + personality);
                    Check(root.Q<Label>("conversation-text").text == director.Current.Text(personality), "UI displays the current personality variant: " + personality);
                    Check(!director.Choose("not-an-option") && director.Current.id == "talk.first", "Unknown options cannot change conversation: " + personality);
                    if (personality == CompanionPersonality.Tsundere)
                    {
                        foreach (var size in new[] { new Vector2Int(1920, 1080), new Vector2Int(1600, 900), new Vector2Int(1280, 720) })
                        {
                            await F2RuntimeChecks.Capture("dialogue-choices-" + size.x, size.x, size.y);
                            CheckLayout(root, size.x);
                        }
                        float seconds = director.ActiveSeconds;
                        string id = director.Current.id;
                        int beforePause = spokenCount;
                        await Press(Key.Tab);
                        await Task.Delay(250);
                        Check(prototype.IsPaused && director.Suspended && director.ActiveSeconds <= seconds + .3f && director.Current.id == id &&
                            root.Q("conversation").resolvedStyle.display == DisplayStyle.None && !director.Choose("question.health"), "Pause suspends choices and preserves dialogue");
                        float pausedSeconds = director.ActiveSeconds;
                        await Task.Delay(250);
                        Check(director.ActiveSeconds == pausedSeconds, "Real paused frames consume no narrative time");
                        await Press(Key.Tab);
                        Check(!prototype.IsPaused && director.Active && !prototype.Player.InputEnabled, "Resume preserves the same conversation and control ownership");
                        Check(spokenCount == beforePause, "Pause and resume do not emit the same spoken line again");
                        await Press(Key.Digit1);
                    }
                    else Submit(root.Q<Button>("question.health"));
                    Check(director.Current.id == "answer.health" && director.Choices.Count == 0, "A real choice transitions to its target: " + personality);
                    Report.answerVariants.Add(director.Current.Text(personality));
                    Check(companion.State.Trust == 1, "Conversation does not award trust: " + personality);
                    await F2RuntimeChecks.Capture("dialogue-answer-" + personality);
                    Submit(root.Q<Button>("conversation-continue"));
                    await Task.Delay(120);
                    Check(!director.Active && prototype.Player.InputEnabled, "Continue closes UI and restores movement: " + personality);
                    if (!Application.isBatchMode && Application.isFocused)
                        Check(Cursor.lockState == CursorLockMode.Locked, "Focused player restores cursor capture: " + personality);
                    else Report.limits.Add("OS cursor capture requires a focused non-batch window: " + personality);
                    if (personality == CompanionPersonality.Tsundere)
                    {
                        var position = prototype.Player.transform.position;
                        await Press(Key.W, 300);
                        Check(Vector3.Distance(position, prototype.Player.transform.position) > .15f, "W moves the player after dialogue closes");
                        prototype.Player.ResetPosition();
                    }
                    Check(companion.Interact() && director.Current.id == "talk.following" && director.Choices.All(option => option.target != "answer.health"),
                        "Reopening uses actual following context and removes consumed answers: " + personality);
                    await Press(Key.Escape);
                    Check(!director.Active && !prototype.IsPaused && prototype.Player.InputEnabled, "Escape closes dialogue without opening pause: " + personality);
                    Check(companion.Interact() && director.Current.id == "talk.again", "Introduction is not replayed after skipping: " + personality);
                    companion.SetFollowing(false);
                    Check(!director.Active && !companion.IsFollowing && prototype.Player.InputEnabled, "Wait command interrupts conversation safely: " + personality);
                    Check(companion.Interact() && director.Current.id == "talk.waiting", "Waiting fact selects distinct context: " + personality);
                    director.Interrupt(DialoguePriority.Survival);
                    Check(!director.Active && prototype.Player.InputEnabled, "Future priority entry releases UI and controls: " + personality);
                    Check(!companion.Interact(), "Exhausted local topics do not loop introductory content: " + personality);
                    companion.SetFollowing(true);
                    Check(companion.IsFollowing && companion.State.Trust == 1, "Follow command preserves initial relationship: " + personality);
                }
                Check(Report.rescueVariants.Distinct().Count() == 3 && Report.answerVariants.Distinct().Count() == 3, "Same rescue and question have three distinct personality responses");

                prototype.Restart(); companion.Interact(); companion.Interact();
                Check(companion.Incapacitate() && !director.Active && prototype.Player.InputEnabled, "Actual incapacitation preempts casual conversation");
                Check(!director.Begin() && lastSpoken == DialogueContent.Current.Get("fact.downed").Text(companion.State.Personality), "Downed state prioritizes the actual help request");
                prototype.SetPaused(true);
                float remaining = companion.State.RescueSecondsRemaining;
                await Task.Delay(350);
                Check(companion.State.RescueSecondsRemaining == remaining, "Pause preserves the real rescue window");
                prototype.SetPaused(false); companion.Interact();
                Check(companion.State.LifeState == CompanionLifeState.Active && companion.State.Trust == 1 && companion.State.RemembersFirstRescue,
                    "Revival keeps one rescue memory and one trust reward");
                Check(companion.Interact() && director.Current.id == "talk.recovered" && companion.Reactions.Emotion == CompanionEmotion.Relief,
                    "Revival selects contextual recovery conversation and relief");
                Submit(root.Q<Button>("conversation-close"));
                Check(!director.Active && prototype.Player.InputEnabled, "Close button releases all conversation controls");
                var liveState = companion.State;
                int consumed = director.ConsumedCount;
                prototype.SetPaused(true); prototype.OpenCharacterCreation();
                await Until(() => Object.FindAnyObjectByType<CharacterCreationController>()?.Appearance.Ready == true, "Additive creator failed");
                creator = Object.FindAnyObjectByType<CharacterCreationController>();
                await Press(Key.Tab);
                Check(prototype.IsPaused && Time.timeScale == 0 && !prototype.Player.InputEnabled, "Game shortcuts cannot resume the encounter behind the creator");
                string fixture = F2RuntimeChecks.Argument("-isekVrmFixture");
                if (string.IsNullOrEmpty(fixture)) throw new InvalidOperationException("The private VRM fixture is required for the appearance regression.");
                await creator.ImportAvatarAsync(fixture, true);
                Report.avatarHash = creator.Appearance.Selection.hash;
                Check(creator.BeginGame(), "Existing F2 save accepts the prepared local VRM");
                await Until(() => CompanionPrototype.Editing == null && Object.FindAnyObjectByType<CharacterCreationController>() == null, "Return from creator failed");
                Check(ReferenceEquals(liveState, companion.State) && director.ConsumedCount == consumed && liveState.Trust == 1 && liveState.RemembersFirstRescue,
                    "Catalog to VRM preserves the live encounter and consumed conversation history");
                Check(appearance.Importer.HasAvatar && CompanionProfileStore.LoadRecord().appearance.hash == Report.avatarHash,
                    "F2 avatar persistence and actual Game VRM remain functional");
                prototype.SetPaused(false);
                CheckActualReactions(companion, "VRM");
                companion.Reactions.Show(CompanionEmotion.Joy);
                await Task.Delay(250);
                Check(companion.Reactions.AppliedExpression == "happy" && !companion.Reactions.UsedFallback &&
                    appearance.Importer.Avatar.Runtime.Expression.GetWeight(UniVRM10.ExpressionKey.CreateFromPreset(UniVRM10.ExpressionPreset.happy)) > .9f,
                    "The imported VRM receives the actual semantic happy expression");
                companion.Reactions.Clear();
                Check(appearance.Importer.Avatar.Runtime.Expression.GetWeight(UniVRM10.ExpressionKey.CreateFromPreset(UniVRM10.ExpressionPreset.happy)) == 0,
                    "Clearing reactions removes temporary VRM expression weights");
                Check(liveState.Trust == 1 && director.Begin(), "Appearance expressions do not mutate narrative facts");
                await F2RuntimeChecks.Capture("dialogue-vrm-private");
                director.Close();
                prototype.SetPaused(true); prototype.OpenCharacterCreation();
                await Until(() => Object.FindAnyObjectByType<CharacterCreationController>()?.Appearance.Ready == true, "Second creator failed");
                creator = Object.FindAnyObjectByType<CharacterCreationController>();
                creator.SelectCatalog();
                Check(creator.BeginGame(), "Catalog can replace the imported VRM through the existing creator");
                await Until(() => CompanionPrototype.Editing == null && Object.FindAnyObjectByType<CharacterCreationController>() == null, "Catalog return failed");
                Check(ReferenceEquals(liveState, companion.State) && director.ConsumedCount >= consumed && liveState.Trust == 1 && !appearance.Importer.HasAvatar,
                    "VRM to catalog retains relationship and encounter history");
                prototype.SetPaused(false);
                CheckActualReactions(companion, "Catalog");
                companion.Reactions.Show(CompanionEmotion.Joy);
                await Task.Delay(150);
                Check(companion.Reactions.AppliedExpression == "happy" && appearance.Catalog.GetComponentsInChildren<SkinnedMeshRenderer>().Any(renderer =>
                    Enumerable.Range(0, renderer.sharedMesh.blendShapeCount).Any(index => renderer.GetBlendShapeWeight(index) > 0)), "Catalog receives real blend shape weights under the same semantic rule");
                companion.Reactions.Clear();
                Check(appearance.Catalog.GetComponentsInChildren<SkinnedMeshRenderer>().All(renderer =>
                    Enumerable.Range(0, renderer.sharedMesh.blendShapeCount).All(index => renderer.GetBlendShapeWeight(index) == 0)), "Catalog temporary expressions reset to neutral");

                companion.Incapacitate(); Time.timeScale = 30f;
                await Until(() => companion.State.LifeState == CompanionLifeState.Dead, "Death did not resolve");
                Check(prototype.IsPaused && !director.Begin() && !companion.Interact() && !director.Active, "Dead state rejects conversation and preserves existing game over");
                prototype.Restart();
                Check(companion.State.LifeState == CompanionLifeState.Trapped && director.ConsumedCount == 0 && prototype.Player.InputEnabled && !director.Active, "Restart restores gameplay with a fresh encounter");
                companion.Interact(); prototype.SetPaused(true); prototype.SimulateIncapacitation();
                Check(!prototype.IsPaused && companion.State.LifeState == CompanionLifeState.Downed && companion.Reactions.Emotion == CompanionEmotion.Sadness,
                    "Pause menu incapacitation preserves its help reaction when gameplay resumes");
                companion.Interact(); prototype.Restart();
                Check(Report.engineErrors.Count == 0, "No runtime errors during the integrated dialogue scenario");
            }
            finally
            {
                Application.logMessageReceived -= OnLog;
                if (keyboard != null) InputSystem.RemoveDevice(keyboard);
                if (originalSettings != null) { InputSystem.settings = originalSettings; originalSettings.hideFlags = originalFlags; }
                Object.Destroy(settings);
            }
        }

        private static void CheckContentAndFallbacks()
        {
            var content = DialogueContent.Current;
            content.Validate();
            Check(content.provisional && content.entries.Select(entry => entry.id).Distinct().Count() == content.entries.Length, "Provisional declarative content has unique stable IDs and valid targets");
            foreach (var emotion in (CompanionEmotion[])Enum.GetValues(typeof(CompanionEmotion)))
            {
                var requests = new List<string>();
                var presenter = new CompanionReactionPresenter(name => { requests.Add(name); return name == "neutral"; });
                presenter.Show(emotion);
                Check(presenter.AppliedExpression == "neutral" && requests.Last() == "neutral", "Missing avatar capabilities safely fall back: " + emotion);
                presenter.Clear();
            }
            var timed = new CompanionReactionPresenter(_ => true);
            timed.Show(CompanionEmotion.Concern, 1);
            timed.Tick(0); timed.Tick(float.NaN);
            Check(timed.Emotion == CompanionEmotion.Concern, "Paused and invalid reaction time cannot expire emotion");
            timed.Tick(1);
            Check(timed.Emotion == CompanionEmotion.Neutral, "Temporary bark reactions expire using active game time");
        }

        private static void CheckLayout(VisualElement root, int width)
        {
            var panel = root.Q("conversation");
            var bounds = panel.worldBound;
            Check(bounds.width > 0 && bounds.xMin >= 0 && bounds.yMin >= 0 && bounds.xMax <= root.worldBound.xMax + 1 && bounds.yMax <= root.worldBound.yMax + 1,
                "Conversation stays inside viewport at " + width);
            var labels = panel.Query<Label>().ToList();
            foreach (var label in labels)
            {
                var measured = label.MeasureTextSize(label.text, label.contentRect.width, VisualElement.MeasureMode.Exactly, 0, VisualElement.MeasureMode.Undefined);
                Check(label.contentRect.height + 2 >= measured.y, "Dialogue text is not vertically clipped at " + width + ": " + label.name);
            }
            var buttons = panel.Query<Button>().ToList().Where(button => button.resolvedStyle.display != DisplayStyle.None).ToList();
            Check(buttons.All(button => bounds.Contains(button.worldBound.min) && bounds.Contains(button.worldBound.max)) &&
                buttons.Zip(buttons.Skip(1), (a, b) => a.worldBound.yMax <= b.worldBound.yMin + 1).All(value => value), "Conversation choices are inside panel without overlap at " + width);
        }

        private static void CheckActualReactions(CompanionController companion, string kind)
        {
            int trust = companion.State.Trust;
            foreach (var emotion in (CompanionEmotion[])Enum.GetValues(typeof(CompanionEmotion)))
            {
                companion.Reactions.Show(emotion);
                Report.expressionMappings.Add(kind + ": " + emotion + " -> " + companion.Reactions.AppliedExpression + " fallback=" + companion.Reactions.UsedFallback);
                Check(companion.State.Trust == trust, "Semantic reaction preserves relationship: " + kind + "/" + emotion);
            }
            companion.Reactions.Clear();
        }
    }
}
#endif
