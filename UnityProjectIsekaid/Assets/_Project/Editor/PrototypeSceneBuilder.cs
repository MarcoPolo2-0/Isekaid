using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Isekaid.Editor
{
    public static class PrototypeSceneBuilder
    {
        public const string ScenePath = "Assets/_Project/Scenes/CompanionPrototype.unity";
        private const string Root = "Assets/_Project";

        [MenuItem("Isekaid/Open companion prototype")]
        public static void Open()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            CreateIfMissing();
            EditorSceneManager.OpenScene(ScenePath);
        }

        public static void CreateAndValidate()
        {
            CreateIfMissing();
            var panel = AssetDatabase.LoadAssetAtPath<PanelSettings>(Root + "/Settings/PrototypePanelSettings.asset");
            panel.themeStyleSheet = AssetDatabase.LoadAssetAtPath<ThemeStyleSheet>(Root + "/Settings/PrototypeTheme.tss");
            EditorUtility.SetDirty(panel);
            AssetDatabase.SaveAssets();
            PrototypeChecks.Run();
        }

        private static void CreateIfMissing()
        {
            if (File.Exists(ScenePath))
                return;

            foreach (string directory in new[] { "Scenes", "Materials", "Prefabs", "Settings" })
                Directory.CreateDirectory(Root + "/" + directory);
            AssetDatabase.Refresh();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var grass = Material("Grass", new Color(0.25f, 0.39f, 0.28f));
            var stone = Material("Stone", new Color(0.42f, 0.47f, 0.43f));
            var path = Material("Path", new Color(0.63f, 0.54f, 0.38f));
            var wood = Material("Wood", new Color(0.26f, 0.16f, 0.10f));
            var leaves = Material("Leaves", new Color(0.16f, 0.31f, 0.22f));
            var foliageLight = Material("LeavesLight", new Color(0.36f, 0.47f, 0.24f));
            var skin = Material("Skin", new Color(0.88f, 0.65f, 0.48f));
            var hair = Material("Hair", new Color(0.19f, 0.10f, 0.08f));
            var eyes = Material("Eyes", new Color(0.13f, 0.55f, 0.31f));
            var top = Material("Top", new Color(0.89f, 0.83f, 0.67f));
            var trousers = Material("Trousers", new Color(0.22f, 0.27f, 0.32f));
            var white = Material("EyeWhite", new Color(0.96f, 0.96f, 0.88f));
            var rope = Material("Rope", new Color(0.76f, 0.56f, 0.28f));

            var world = new GameObject("Clearing").transform;
            Shape("Ground", PrimitiveType.Cube, world, new Vector3(0f, -0.3f, 5f), new Vector3(28f, 0.6f, 30f), grass, true);
            Shape("Trail", PrimitiveType.Cube, world, new Vector3(0f, 0.005f, 5f), new Vector3(4f, 0.02f, 29f), path, false);
            Shape("WestBoundary", PrimitiveType.Cube, world, new Vector3(-14f, 1f, 5f), new Vector3(1f, 2.5f, 30f), stone, true);
            Shape("EastBoundary", PrimitiveType.Cube, world, new Vector3(14f, 1f, 5f), new Vector3(1f, 2.5f, 30f), stone, true);
            Shape("NorthBoundary", PrimitiveType.Cube, world, new Vector3(0f, 1f, 20f), new Vector3(28f, 2.5f, 1f), stone, true);
            Shape("SouthBoundary", PrimitiveType.Cube, world, new Vector3(0f, 1f, -10f), new Vector3(28f, 2.5f, 1f), stone, true);
            Shape("PathObstacle", PrimitiveType.Cube, world, new Vector3(0f, 0.65f, 7f), new Vector3(2.4f, 1.3f, 2f), stone, true);
            Shape("WestRock", PrimitiveType.Sphere, world, new Vector3(-5f, 0.4f, 2f), new Vector3(2.8f, 1.7f, 2.4f), stone, true);
            Shape("EastRock", PrimitiveType.Sphere, world, new Vector3(6f, 0.3f, 11f), new Vector3(2.5f, 1.6f, 2f), stone, true);

            for (int i = 0; i < 12; i++)
            {
                float x = i % 2 == 0 ? -8.5f : 8.5f;
                float z = -5f + (i / 2) * 4.2f;
                float height = 3.6f + (i % 3) * 0.35f;
                var tree = new GameObject("Tree_" + i).transform;
                tree.SetParent(world);
                tree.position = new Vector3(x + (i % 3) * 0.4f, 0f, z);
                Shape("Trunk", PrimitiveType.Cylinder, tree, new Vector3(0f, height / 2f, 0f), new Vector3(0.5f, height / 2f, 0.5f), wood, true);
                Shape("Canopy", PrimitiveType.Sphere, tree, new Vector3(0f, height, 0f), new Vector3(4.5f, 3.3f, 4.5f), i % 2 == 0 ? leaves : foliageLight, false);
            }

            var light = new GameObject("Sun").AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.7f;
            light.color = new Color(1f, 0.91f, 0.75f);
            light.shadows = LightShadows.Soft;
            light.transform.rotation = Quaternion.Euler(48f, -35f, 0f);
            RenderSettings.sun = light;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.55f, 0.68f, 0.76f);
            RenderSettings.ambientEquatorColor = new Color(0.43f, 0.48f, 0.42f);
            RenderSettings.ambientGroundColor = new Color(0.22f, 0.28f, 0.24f);
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.57f, 0.68f, 0.65f);
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = 20f;
            RenderSettings.fogEndDistance = 65f;

            var sources = new List<NavMeshBuildSource>();
            UnityEngine.AI.NavMeshBuilder.CollectSources(world, ~0, NavMeshCollectGeometry.PhysicsColliders, 0,
                new List<NavMeshBuildMarkup>(), sources);
            var settings = NavMesh.GetSettingsByIndex(0);
            var navigation = UnityEngine.AI.NavMeshBuilder.BuildNavMeshData(settings, sources,
                new Bounds(new Vector3(0f, 3f, 5f), new Vector3(32f, 10f, 34f)), Vector3.zero, Quaternion.identity);
            if (navigation == null)
                throw new System.InvalidOperationException("Could not bake the clearing navigation.");
            AssetDatabase.CreateAsset(navigation, Root + "/Settings/ClearingNavigation.asset");

            var playerObject = new GameObject("Player");
            playerObject.transform.position = new Vector3(0f, 0.05f, -3f);
            var character = playerObject.AddComponent<CharacterController>();
            character.height = 1.8f;
            character.center = new Vector3(0f, 0.9f, 0f);
            character.radius = 0.3f;
            character.stepOffset = 0.25f;
            var camera = new GameObject("FirstPersonCamera").AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.transform.SetParent(playerObject.transform, false);
            camera.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            camera.fieldOfView = 65f;
            camera.nearClipPlane = 0.05f;
            camera.farClipPlane = 100f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = RenderSettings.fogColor;
            camera.gameObject.AddComponent<AudioListener>();
            var player = playerObject.AddComponent<FirstPersonController>();
            Reference(player, "viewCamera", camera);
            Reference(player, "inputActions", AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/InputSystem_Actions.inputactions"));
            PrefabUtility.SaveAsPrefabAssetAndConnect(playerObject, Root + "/Prefabs/FirstPersonPlayer.prefab", InteractionMode.AutomatedAction);

            var companionObject = new GameObject("Companion");
            var agent = companionObject.AddComponent<NavMeshAgent>();
            agent.agentTypeID = settings.agentTypeID;
            agent.radius = 0.3f;
            agent.height = 1.8f;
            agent.speed = 3.8f;
            agent.angularSpeed = 360f;
            agent.acceleration = 14f;
            agent.stoppingDistance = 1.8f;
            var visual = new GameObject("Mannequin").transform;
            visual.SetParent(companionObject.transform, false);
            var capsule = visual.gameObject.AddComponent<CapsuleCollider>();
            capsule.height = 1.8f;
            capsule.radius = 0.32f;
            capsule.center = new Vector3(0f, 0.9f, 0f);
            var topRenderer = Shape("Top", PrimitiveType.Capsule, visual, new Vector3(0f, 1.14f, 0f), new Vector3(0.51f, 0.32f, 0.3f), top, false);
            Shape("Head", PrimitiveType.Sphere, visual, new Vector3(0f, 1.65f, 0f), new Vector3(0.37f, 0.43f, 0.35f), skin, false);
            var hairBack = Shape("LongHair", PrimitiveType.Capsule, visual, new Vector3(0f, 1.41f, -0.115f), new Vector3(0.43f, 0.43f, 0.21f), hair, false);
            var hairTop = Shape("HairCrown", PrimitiveType.Sphere, visual, new Vector3(0f, 1.79f, -0.02f), new Vector3(0.39f, 0.18f, 0.36f), hair, false);
            var trouserRenderers = new List<Renderer>();
            var eyeRenderers = new List<Renderer>();
            for (int side = -1; side <= 1; side += 2)
            {
                Shape("Arm", PrimitiveType.Capsule, visual, new Vector3(side * 0.32f, 1.03f, 0f), new Vector3(0.14f, 0.29f, 0.14f), skin, false);
                trouserRenderers.Add(Shape("TrouserLeg", PrimitiveType.Capsule, visual, new Vector3(side * 0.14f, 0.48f, 0f), new Vector3(0.21f, 0.38f, 0.23f), trousers, false));
                Shape("Boot", PrimitiveType.Cube, visual, new Vector3(side * 0.14f, 0.10f, 0.045f), new Vector3(0.22f, 0.20f, 0.36f), wood, false);
                Shape("EyeWhite", PrimitiveType.Sphere, visual, new Vector3(side * 0.085f, 1.67f, 0.151f), new Vector3(0.105f, 0.10f, 0.045f), white, false);
                eyeRenderers.Add(Shape("Iris", PrimitiveType.Sphere, visual, new Vector3(side * 0.085f, 1.67f, 0.178f), new Vector3(0.053f, 0.069f, 0.025f), eyes, false));
            }
            var trap = new GameObject("BanditTrap").transform;
            trap.SetParent(companionObject.transform, false);
            Shape("TrapBase", PrimitiveType.Cube, trap, new Vector3(0f, 0.025f, 0f), new Vector3(0.95f, 0.05f, 0.85f), wood, false);
            Shape("Binding", PrimitiveType.Cube, trap, new Vector3(0f, 0.2f, 0.15f), new Vector3(0.55f, 0.07f, 0.08f), rope, false);
            Shape("Stake", PrimitiveType.Cylinder, trap, new Vector3(0.65f, 0.3f, 0f), new Vector3(0.08f, 0.3f, 0.08f), wood, false);
            var companion = companionObject.AddComponent<CompanionController>();
            Reference(companion, "visual", visual);
            Reference(companion, "trap", trap.gameObject);
            var appearance = companionObject.AddComponent<CompanionAppearance>();
            References(appearance, "hair", new[] { hairBack, hairTop });
            References(appearance, "eyes", eyeRenderers.ToArray());
            References(appearance, "top", new[] { topRenderer });
            References(appearance, "trousers", trouserRenderers.ToArray());
            PrefabUtility.SaveAsPrefabAssetAndConnect(companionObject, Root + "/Prefabs/Companion.prefab", InteractionMode.AutomatedAction);
            Reference(companion, "followTarget", player.transform);
            companion.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            PrefabUtility.RecordPrefabInstancePropertyModifications(companion);
            PrefabUtility.RecordPrefabInstancePropertyModifications(companion.transform);

            var prototype = new GameObject("CompanionPrototype").AddComponent<CompanionPrototype>();
            Reference(prototype, "player", player);
            Reference(prototype, "companion", companion);
            Reference(prototype, "navigation", navigation);
            var panel = ScriptableObject.CreateInstance<PanelSettings>();
            panel.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            panel.referenceResolution = new Vector2Int(1600, 900);
            panel.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
            panel.match = 0.5f;
            panel.themeStyleSheet = AssetDatabase.LoadAssetAtPath<ThemeStyleSheet>(Root + "/Settings/PrototypeTheme.tss");
            AssetDatabase.CreateAsset(panel, Root + "/Settings/PrototypePanelSettings.asset");
            var ui = new GameObject("Interface");
            ui.AddComponent<UIDocument>().panelSettings = panel;
            var hud = ui.AddComponent<PrototypeHud>();
            Reference(hud, "prototype", prototype);
            Reference(hud, "styleSheet", AssetDatabase.LoadAssetAtPath<StyleSheet>(Root + "/UI/Prototype.uss"));

            EditorSceneManager.SaveScene(scene, ScenePath);
            var scenes = new List<EditorBuildSettingsScene> { new EditorBuildSettingsScene(ScenePath, true) };
            foreach (var existing in EditorBuildSettings.scenes)
                if (existing.path != ScenePath)
                    scenes.Add(existing);
            EditorBuildSettings.scenes = scenes.ToArray();
            AssetDatabase.SaveAssets();
            Debug.Log("ISEKAID_PROTOTYPE_SCENE_CREATED");
        }

        private static Material Material(string name, Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Simple Lit");
            if (shader == null)
                throw new System.InvalidOperationException("URP Simple Lit shader is missing.");
            var material = new Material(shader) { name = name };
            material.SetColor("_BaseColor", color);
            material.SetFloat("_Smoothness", 0.12f);
            AssetDatabase.CreateAsset(material, Root + "/Materials/" + name + ".mat");
            return material;
        }

        private static Renderer Shape(string name, PrimitiveType primitive, Transform parent, Vector3 position, Vector3 scale, Material material, bool collision)
        {
            var obj = GameObject.CreatePrimitive(primitive);
            obj.name = name;
            obj.transform.SetParent(parent, false);
            obj.transform.localPosition = position;
            obj.transform.localScale = scale;
            var renderer = obj.GetComponent<Renderer>();
            renderer.sharedMaterial = material;
            if (!collision)
                Object.DestroyImmediate(obj.GetComponent<Collider>());
            return renderer;
        }

        private static void Reference(Object target, string field, Object value)
        {
            if (value == null)
                throw new System.InvalidOperationException("Missing reference: " + field);
            var serialized = new SerializedObject(target);
            serialized.FindProperty(field).objectReferenceValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void References(Object target, string field, Renderer[] values)
        {
            var serialized = new SerializedObject(target);
            var property = serialized.FindProperty(field);
            property.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
                property.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
