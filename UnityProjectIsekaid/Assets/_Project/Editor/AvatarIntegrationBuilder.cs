using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Isekaid.Editor
{
    public static class AvatarIntegrationBuilder
    {
        private const string Root = "Assets/_Project/AvatarCatalog";
        public static void Run()
        {
            Directory.CreateDirectory(Root + "/Materials");
            AssetDatabase.Refresh();
            string controllerPath = Root + "/Adventurer.controller";
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
            if (controller == null)
            {
                controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
                foreach (var entry in new[] { ("Idle", "Unarmed_Idle"), ("Walk", "Unarmed_Walk"), ("Run", "Unarmed_Run") })
                {
                    var clip = AssetDatabase.LoadAllAssetsAtPath(CompanionAssetBuilder.Root + "/Animations/" + entry.Item2 + ".fbx")
                        .OfType<AnimationClip>().First(c => !c.name.StartsWith("__preview__"));
                    var state = controller.layers[0].stateMachine.AddState(entry.Item1);
                    state.motion = clip;
                    if (entry.Item1 == "Idle") controller.layers[0].stateMachine.defaultState = state;
                }
            }
            var instance = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(CompanionAssetBuilder.ModelPath));
            instance.name = "Adventurer_CC0_F2";
            try
            {
                foreach (var renderer in instance.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    var materials = renderer.sharedMaterials;
                    for (int i = 0; i < materials.Length; i++)
                    {
                        string name = materials[i].name;
                        string path = Root + "/Materials/" + name + ".mat";
                        var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                        if (material == null)
                        {
                            material = new Material(Shader.Find("Universal Render Pipeline/Simple Lit")) { name = name };
                            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(CompanionAssetBuilder.Root + "/Textures/Character/" + (name == "Eyes" ? "Eye" : name) + "_Base.png");
                            if (texture == null) throw new InvalidOperationException("Missing catalog texture: " + name);
                            material.SetTexture("_BaseMap", texture); material.SetColor("_BaseColor", Color.white); material.SetFloat("_Smoothness", 0);
                            material.SetFloat("_Cull", 0);
                            AssetDatabase.CreateAsset(material, path);
                        }
                        materials[i] = material;
                    }
                    renderer.sharedMaterials = materials;
                    Debug.Log("CATALOG_SHAPES " + renderer.name + ": " + string.Join(",", Enumerable.Range(0, renderer.sharedMesh.blendShapeCount).Select(renderer.sharedMesh.GetBlendShapeName)));
                }
                var animator = instance.GetComponent<Animator>();
                if (animator == null) animator = instance.AddComponent<Animator>();
                animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(LocomotionBuilder.ControllerPath) ?? controller;
                animator.avatar = AssetDatabase.LoadAssetAtPath<Avatar>(LocomotionBuilder.Root + "/AdventurerHumanoid.asset");
                animator.applyRootMotion = false; animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                var renderers = instance.GetComponentsInChildren<Renderer>();
                var bounds = renderers[0].bounds; foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);
                instance.transform.localScale *= 1.7f / bounds.size.y;
                bounds = renderers[0].bounds; foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);
                // Store the offset on a wrapper so FBX animation bindings retain their original root.
                var wrapper = new GameObject("Adventurer_CC0_F2");
                instance.transform.SetParent(wrapper.transform, true);
                if (animator.avatar == null) instance.transform.position -= Vector3.up * bounds.min.y;
                var prefab = PrefabUtility.SaveAsPrefabAsset(wrapper, Root + "/Adventurer.prefab");
                Object.DestroyImmediate(wrapper);
                foreach (var scenePath in new[] { CharacterCreationController.ScenePath, CharacterCreationController.GameScenePath })
                {
                    var scene = EditorSceneManager.OpenScene(scenePath);
                    var appearance = Object.FindAnyObjectByType<CompanionAppearance>();
                    var serialized = new SerializedObject(appearance);
                    var visual = appearance.transform.Find("Mannequin");
                    if (visual == null) throw new InvalidOperationException("Visual root missing: " + scenePath);
                    serialized.FindProperty("visualRoot").objectReferenceValue = visual;
                    serialized.FindProperty("catalogPrefab").objectReferenceValue = prefab;
                    serialized.FindProperty("vrmAnimations").objectReferenceValue = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(LocomotionBuilder.ControllerPath);
                    serialized.ApplyModifiedPropertiesWithoutUndo();
                    if (scenePath == CharacterCreationController.ScenePath)
                    {
                        var camera = Object.FindAnyObjectByType<Camera>(); camera.cullingMask = 1 << 31;
                        camera.orthographicSize = 1.65f; camera.transform.position = new Vector3(0, .75f, -5);
                        foreach (var root in scene.GetRootGameObjects())
                        {
                            if (root.GetComponent<UIDocument>() != null) continue;
                            foreach (var child in root.GetComponentsInChildren<Transform>(true)) child.gameObject.layer = 31;
                        }
                    }
                    EditorSceneManager.SaveScene(scene);
                }
            }
            finally { if (instance != null) Object.DestroyImmediate(instance); }
            AssetDatabase.SaveAssets();
            Debug.Log("ISEKAID_F2_INTEGRATION_BUILT");
        }
    }
}
