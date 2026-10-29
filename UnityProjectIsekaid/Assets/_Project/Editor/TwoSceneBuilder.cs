using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Isekaid.Editor
{
    public static class TwoSceneBuilder
    {
        private const string Root = "Assets/_Project";

        [MenuItem("Isekaid/Open character creation")]
        public static void Open()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            CreateAndValidate();
            EditorSceneManager.OpenScene(CharacterCreationController.ScenePath);
        }

        public static void CreateAndValidate()
        {
            if (!File.Exists(CharacterCreationController.GameScenePath))
            {
                var game = EditorSceneManager.OpenScene(PrototypeSceneBuilder.ScenePath);
                EditorSceneManager.SaveScene(game, CharacterCreationController.GameScenePath, true);
            }
            if (!File.Exists(CharacterCreationController.ScenePath))
                CreateCharacterScene();

            var scenes = new List<EditorBuildSettingsScene>
            {
                new EditorBuildSettingsScene(CharacterCreationController.ScenePath, true),
                new EditorBuildSettingsScene(CharacterCreationController.GameScenePath, true)
            };
            foreach (var existing in EditorBuildSettings.scenes)
                if (existing.path != CharacterCreationController.ScenePath && existing.path != CharacterCreationController.GameScenePath)
                    scenes.Add(new EditorBuildSettingsScene(existing.path, false));
            EditorBuildSettings.scenes = scenes.ToArray();
            AssetDatabase.SaveAssets();
            PrototypeChecks.Run();
            Debug.Log("ISEKAID_TWO_SCENES_CREATED_AND_VALIDATED");
        }

        private static void CreateCharacterScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var source = PrefabUtility.LoadPrefabContents(Root + "/Prefabs/Companion.prefab");
            GameObject previewPrefab;
            try
            {
                Object.DestroyImmediate(source.GetComponent<CompanionController>());
                Object.DestroyImmediate(source.GetComponent<NavMeshAgent>());
                Object.DestroyImmediate(source.transform.Find("BanditTrap").gameObject);
                source.name = "CompanionPreview";
                previewPrefab = PrefabUtility.SaveAsPrefabAsset(source, Root + "/Prefabs/CompanionPreview.prefab");
            }
            finally { PrefabUtility.UnloadPrefabContents(source); }

            var preview = (GameObject)PrefabUtility.InstantiatePrefab(previewPrefab);
            preview.transform.SetPositionAndRotation(new Vector3(-0.9f, 0f, 0f), Quaternion.Euler(0f, 180f, 0f));
            PrefabUtility.RecordPrefabInstancePropertyModifications(preview.transform);
            var camera = new GameObject("PreviewCamera").AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.orthographic = true;
            camera.orthographicSize = 1.6f;
            camera.transform.position = new Vector3(0f, 1.1f, -5f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.055f, 0.10f, 0.115f);
            camera.gameObject.AddComponent<AudioListener>();
            var stage = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stage.name = "PreviewStage";
            stage.transform.position = new Vector3(-0.9f, -0.1f, 0f);
            stage.transform.localScale = new Vector3(2.2f, 0.1f, 2.2f);
            stage.GetComponent<Renderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(Root + "/Materials/Stone.mat");
            var key = new GameObject("KeyLight").AddComponent<Light>();
            key.type = LightType.Directional;
            key.intensity = .65f;
            key.color = Color.white;
            key.transform.rotation = Quaternion.Euler(30f, -25f, 0f);
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(.4f, .4f, .4f);
            RenderSettings.fog = false;

            var ui = new GameObject("CharacterCreation");
            ui.AddComponent<UIDocument>().panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>(Root + "/Settings/PrototypePanelSettings.asset");
            var controller = ui.AddComponent<CharacterCreationController>();
            var serialized = new SerializedObject(controller);
            serialized.FindProperty("appearance").objectReferenceValue = preview.GetComponent<CompanionAppearance>();
            serialized.FindProperty("preview").objectReferenceValue = preview.transform;
            serialized.FindProperty("styleSheet").objectReferenceValue = AssetDatabase.LoadAssetAtPath<StyleSheet>(Root + "/UI/Prototype.uss");
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorSceneManager.SaveScene(scene, CharacterCreationController.ScenePath);
        }
    }
}
