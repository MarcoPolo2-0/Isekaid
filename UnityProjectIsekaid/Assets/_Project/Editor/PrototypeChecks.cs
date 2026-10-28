using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Isekaid.Editor
{
    public static class PrototypeChecks
    {
        [MenuItem("Isekaid/Validate prototype")]
        public static void Run()
        {
            var state = new CompanionState();
            state.Tick(100f);
            Check(state.LifeState == CompanionLifeState.Trapped, "The first trap must not run the death timer.");
            Check(!state.Incapacitate(), "A trapped companion cannot enter the combat rescue state.");
            Check(state.Rescue() && state.Trust == 1 && state.RemembersFirstRescue, "First rescue must register a memory.");
            Check(!state.Rescue() && state.Trust == 1, "Repeated rescue must not duplicate trust.");
            string previousLine = null;
            foreach (CompanionPersonality personality in Enum.GetValues(typeof(CompanionPersonality)))
            {
                state.Personality = personality;
                string line = state.RescueLine(true);
                Check(!string.IsNullOrEmpty(line) && line != previousLine, "Personality must change the first reaction.");
                previousLine = line;
            }
            Check(state.Incapacitate(), "An active companion can become incapacitated.");
            state.Tick(0f);
            state.Tick(-1f);
            state.Tick(float.NaN);
            state.Tick(float.PositiveInfinity);
            Check(state.RescueSecondsRemaining == 30f, "Invalid or paused time must not advance the timer.");
            state.Tick(29f);
            Check(state.LifeState == CompanionLifeState.Downed && state.RescueSecondsRemaining == 1f, "Rescue must remain possible before the deadline.");
            Check(state.Rescue() && state.Trust == 1, "Reviving must preserve the unique memory.");
            state.Incapacitate();
            state.Tick(30f);
            Check(state.LifeState == CompanionLifeState.Dead && !state.Rescue(), "The deadline must end the survival trial.");
            Check(state.TalkLine(0) == string.Empty, "Social dialogue must not run after death.");

            var profile = new CompanionProfile { personality = CompanionPersonality.Yandere, hair = 2, eyes = 3, top = 1, trousers = 2 };
            Check(profile.IsValid, "A supported character profile must be valid.");
            var copy = JsonUtility.FromJson<CompanionProfile>(JsonUtility.ToJson(profile));
            Check(copy.IsValid && copy.personality == profile.personality && copy.hair == 2 && copy.eyes == 3 && copy.top == 1 && copy.trousers == 2,
                "Character serialization must preserve all choices.");
            copy.hair = 4;
            Check(!copy.IsValid, "Unsupported palette indices must be rejected.");
            copy.hair = 0;
            copy.version = 2;
            Check(!copy.IsValid, "Unknown profile versions must be rejected.");

            var scene = EditorSceneManager.OpenScene(CharacterCreationController.GameScenePath);
            foreach (var root in scene.GetRootGameObjects())
                ValidateHierarchy(root);
            var prototype = UnityEngine.Object.FindAnyObjectByType<CompanionPrototype>();
            Check(prototype != null && prototype.Player != null && prototype.Companion != null, "Scene wiring is incomplete.");
            var input = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/InputSystem_Actions.inputactions");
            foreach (var action in new[] { "Move", "Look", "Sprint", "Jump", "Interact" })
                Check(input.FindAction("Player/" + action) != null, "Missing player action: " + action);
            var ui = UnityEngine.Object.FindAnyObjectByType<UIDocument>();
            Check(ui != null && ui.panelSettings != null && ui.panelSettings.themeStyleSheet != null, "UI settings are missing.");
            foreach (var sheet in new StyleSheet[] { ui.panelSettings.themeStyleSheet,
                AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_Project/UI/Prototype.uss") })
                Check(!new SerializedObject(sheet).FindProperty("m_ImportedWithErrors").boolValue, "A UI stylesheet has import errors.");

            var data = AssetDatabase.LoadAssetAtPath<NavMeshData>("Assets/_Project/Settings/ClearingNavigation.asset");
            Check(data != null, "Navigation data is missing.");
            var instance = NavMesh.AddNavMeshData(data);
            try
            {
                var path = new NavMeshPath();
                Check(NavMesh.CalculatePath(Vector3.zero, new Vector3(0f, 0f, 12f), NavMesh.AllAreas, path)
                    && path.status == NavMeshPathStatus.PathComplete && path.corners.Length > 2,
                    "The companion must be able to route around the central obstacle.");
            }
            finally { instance.Remove(); }
            var creation = EditorSceneManager.OpenScene(CharacterCreationController.ScenePath);
            foreach (var root in creation.GetRootGameObjects())
                ValidateHierarchy(root);
            Check(UnityEngine.Object.FindAnyObjectByType<CharacterCreationController>() != null, "The character creation scene is missing its controller.");
            Check(UnityEngine.Object.FindAnyObjectByType<CompanionController>() == null && UnityEngine.Object.FindAnyObjectByType<NavMeshAgent>() == null,
                "The creation preview must not simulate companion movement or survival.");
            Check(EditorBuildSettings.scenes[0].path == CharacterCreationController.ScenePath && EditorBuildSettings.scenes[0].enabled &&
                EditorBuildSettings.scenes[1].path == CharacterCreationController.GameScenePath && EditorBuildSettings.scenes[1].enabled,
                "The creator and adventure must be the active launch scenes.");
            Debug.Log("ISEKAID_PROTOTYPE_CHECKS_PASSED");
        }

        private static void ValidateHierarchy(GameObject obj)
        {
            Check(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(obj) == 0, "Missing script: " + obj.name);
            foreach (var component in obj.GetComponents<MonoBehaviour>())
            {
                if (component == null || component.GetType().Namespace != "Isekaid")
                    continue;
                var serialized = new SerializedObject(component);
                var property = serialized.GetIterator();
                while (property.NextVisible(true))
                    if (property.propertyType == SerializedPropertyType.ObjectReference)
                        Check(property.objectReferenceValue != null, "Missing serialized reference: " + obj.name + "." + property.propertyPath);
            }
            foreach (Transform child in obj.transform)
                ValidateHierarchy(child.gameObject);
        }

        private static void Check(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }
    }
}
