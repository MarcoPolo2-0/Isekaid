using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Isekaid.Editor
{
    [InitializeOnLoad]
    public static class PrototypePlayModeChecks
    {
        private const string Pending = "Isekaid.PrototypePlayModeChecks";
        private const string SavedPreference = Pending + ".SavedPreference";
        private const string HadPreference = Pending + ".HadPreference";
        private static CompanionPrototype prototype;
        private static Keyboard keyboard;
        private static InputSettings originalInputSettings;
        private static int stage;
        private static double started;
        private static double stageStarted;
        private static Vector3 waitingPosition;

        static PrototypePlayModeChecks()
        {
            if (SessionState.GetBool(Pending, false))
                EditorApplication.delayCall += Attach;
        }

        public static void Run()
        {
            PrototypeChecks.Run();
            SessionState.SetBool(HadPreference, PlayerPrefs.HasKey(CompanionProfileStore.PreferenceKey));
            SessionState.SetString(SavedPreference, PlayerPrefs.GetString(CompanionProfileStore.PreferenceKey, string.Empty));
            SessionState.SetBool(Pending, true);
            EditorSceneManager.OpenScene(CharacterCreationController.ScenePath);
            Attach();
            EditorApplication.EnterPlaymode();
        }

        private static void Attach()
        {
            EditorApplication.update -= Tick;
            Application.logMessageReceived -= OnLog;
            stage = -1;
            prototype = null;
            started = EditorApplication.timeSinceStartup;
            stageStarted = started;
            Application.logMessageReceived += OnLog;
            EditorApplication.update += Tick;
        }

        private static void OnLog(string message, string stack, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
                Finish(false, message);
        }

        private static void Tick()
        {
            try
            {
                double now = EditorApplication.timeSinceStartup;
                if (now - started > 60d)
                    throw new InvalidOperationException("Play Mode checks timed out at stage " + stage);
                if (!EditorApplication.isPlaying || now - stageStarted < 1d)
                    return;

                if (stage == -1)
                {
                    var creator = UnityEngine.Object.FindAnyObjectByType<CharacterCreationController>();
                    if (creator == null)
                        return;
                    creator.SetPersonality(CompanionPersonality.Yandere);
                    creator.SetColor(0, 2);
                    creator.SetColor(1, 3);
                    creator.SetColor(2, 1);
                    creator.SetColor(3, 2);
                    Check(creator.BeginGame(), "The creator must start the adventure.");
                    stage = 0;
                    stageStarted = now;
                    return;
                }

                if (stage == 9)
                {
                    var creator = UnityEngine.Object.FindAnyObjectByType<CharacterCreationController>();
                    if (creator == null)
                        return;
                    Check(Time.timeScale == 1f && Cursor.lockState == CursorLockMode.None,
                        "Returning from pause must restore creator time and cursor.");
                    Check(creator.Profile.personality == CompanionPersonality.Yandere && creator.Profile.hair == 2 &&
                        creator.Profile.eyes == 3 && creator.Profile.top == 1 && creator.Profile.trousers == 2,
                        "Returning to creation must reload the saved design.");
                    Check(creator.BeginGame(), "The creator must permit a second adventure.");
                    stage = 10;
                    stageStarted = now;
                    prototype = null;
                    return;
                }

                if (prototype == null)
                    prototype = UnityEngine.Object.FindAnyObjectByType<CompanionPrototype>();
                if (prototype == null)
                    return;

                var companion = prototype.Companion;
                switch (stage)
                {
                    case 0:
                        Check(!prototype.IsPaused && Time.timeScale == 1f, "The adventure must start with gameplay enabled.");
                        Check(companion.State.Personality == CompanionPersonality.Yandere, "The chosen personality must reach the adventure.");
                        CheckAppearance(companion);
                        Check(prototype.CanInteract(), "The initial view must reach the companion through the interaction ray.");
                        originalInputSettings = InputSystem.settings;
                        InputSystem.settings = UnityEngine.Object.Instantiate(originalInputSettings);
                        InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
                        InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
                        Application.runInBackground = true;
                        prototype.SetPaused(false);
                        keyboard = InputSystem.AddDevice<Keyboard>();
                        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E));
                        break;
                    case 1:
                        Check(companion.State.LifeState == CompanionLifeState.Active,
                            "Holding E must release the trap through the input action. Paused=" + prototype.IsPaused + ", CanInteract=" + prototype.CanInteract());
                        Check(companion.State.Trust == 1, "The first rescue must register once.");
                        InputSystem.QueueStateEvent(keyboard, new KeyboardState());
                        var controller = prototype.Player.GetComponent<CharacterController>();
                        controller.enabled = false;
                        prototype.Player.transform.position = new Vector3(0f, 0.05f, 12f);
                        controller.enabled = true;
                        break;
                    case 2:
                        if (companion.transform.position.z <= 1f && now - stageStarted < 4d)
                            return;
                        var agent = companion.GetComponent<UnityEngine.AI.NavMeshAgent>();
                        Check(companion.transform.position.z > 1f,
                            "The companion must follow the player. Position=" + companion.transform.position +
                            ", paused=" + prototype.IsPaused + ", onNavMesh=" + agent.isOnNavMesh +
                            ", path=" + agent.pathStatus + ", remaining=" + agent.remainingDistance + ", gameTime=" + Time.time);
                        companion.SetFollowing(false);
                        waitingPosition = companion.transform.position;
                        break;
                    case 3:
                        Check(Vector3.Distance(waitingPosition, companion.transform.position) < 0.1f, "Wait must stop the companion.");
                        companion.SetFollowing(true);
                        break;
                    case 4:
                        if (Vector3.Distance(companion.transform.position, prototype.Player.transform.position) >= 2.3f && now - stageStarted < 10d)
                            return;
                        Check(Vector3.Distance(companion.transform.position, prototype.Player.transform.position) < 2.3f,
                            "The companion must reach the player around the obstacle.");
                        companion.Incapacitate();
                        prototype.SetPaused(true);
                        break;
                    case 5:
                        Check(companion.State.RescueSecondsRemaining == 30f, "Real paused frames must not consume rescue time.");
                        var visual = companion.transform.Find("Mannequin");
                        Check(companion.GetComponentInChildren<CapsuleCollider>().transform == visual, "The interaction collider must follow the visible downed pose.");
                        prototype.Player.ViewCamera.transform.LookAt(visual.TransformPoint(new Vector3(0f, 0.9f, 0f)));
                        Physics.SyncTransforms();
                        Check(prototype.CanInteract(), "The downed body must be reachable with the interaction ray.");
                        prototype.SetPaused(false);
                        break;
                    case 6:
                        Check(companion.State.RescueSecondsRemaining < 30f && companion.State.RescueSecondsRemaining > 26f,
                            "Active frames must consume rescue time.");
                        companion.Interact();
                        Check(companion.State.LifeState == CompanionLifeState.Active && companion.State.Trust == 1, "Reanimation must preserve the first memory.");
                        companion.Incapacitate();
                        Time.timeScale = 30f;
                        break;
                    case 7:
                        if (companion.State.LifeState != CompanionLifeState.Dead)
                            return;
                        Check(prototype.IsPaused && Time.timeScale == 0f, "Death must pause the trial.");
                        prototype.Restart();
                        break;
                    case 8:
                        Check(companion.State.LifeState == CompanionLifeState.Trapped && companion.State.Trust == 0,
                            "Restart must restore the initial scenario.");
                        Check(Vector3.Distance(prototype.Player.transform.position, new Vector3(0f, 0f, -3f)) < 0.2f,
                            "Restart must restore the player position.");
                        CheckAppearance(companion);
                        prototype.SetPaused(true);
                        prototype.OpenCharacterCreation();
                        break;
                    case 10:
                        Check(companion.State.Personality == CompanionPersonality.Yandere && companion.State.LifeState == CompanionLifeState.Trapped,
                            "A second adventure must preserve identity and start a fresh encounter.");
                        CheckAppearance(companion);
                        Finish(true, "ISEKAID_TWO_SCENE_PLAYMODE_CHECKS_PASSED");
                        return;
                }
                stage++;
                stageStarted = now;
            }
            catch (Exception exception) { Finish(false, exception.ToString()); }
        }

        private static void Check(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }

        private static void CheckAppearance(CompanionController companion)
        {
            var appearance = new SerializedObject(companion.GetComponent<CompanionAppearance>());
            string[] parts = { "hair", "eyes", "top", "trousers" };
            int[] choices = { 2, 3, 1, 2 };
            var properties = new MaterialPropertyBlock();
            for (int part = 0; part < parts.Length; part++)
            {
                var renderer = (Renderer)appearance.FindProperty(parts[part]).GetArrayElementAtIndex(0).objectReferenceValue;
                renderer.GetPropertyBlock(properties);
                Check(properties.GetColor("_BaseColor") == CompanionPalette.Colors[part][choices[part]], "The chosen color must reach the adventure: " + parts[part]);
            }
        }

        private static void Finish(bool success, string message)
        {
            SessionState.SetBool(Pending, false);
            EditorApplication.update -= Tick;
            Application.logMessageReceived -= OnLog;
            if (SessionState.GetBool(HadPreference, false))
                PlayerPrefs.SetString(CompanionProfileStore.PreferenceKey, SessionState.GetString(SavedPreference, string.Empty));
            else
                PlayerPrefs.DeleteKey(CompanionProfileStore.PreferenceKey);
            PlayerPrefs.Save();
            if (keyboard != null)
                InputSystem.RemoveDevice(keyboard);
            if (originalInputSettings != null)
            {
                var testSettings = InputSystem.settings;
                InputSystem.settings = originalInputSettings;
                UnityEngine.Object.DestroyImmediate(testSettings);
            }
            if (success)
                Debug.Log(message);
            else
                Debug.LogError(message);
            EditorApplication.Exit(success ? 0 : 1);
        }
    }
}
