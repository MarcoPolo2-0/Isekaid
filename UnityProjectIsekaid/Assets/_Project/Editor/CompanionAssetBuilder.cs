using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Isekaid.Editor
{
    public static class CompanionAssetBuilder
    {
        public const string Root = "Assets/_Project/Art/Companion/Adventurer";
        public const string ModelPath = Root + "/Meshes/Character/AnimeFemaleAdventurer_Outfit_02.fbx";

        public static void Audit()
        {
            Directory.CreateDirectory(Root + "/Materials");
            AssetDatabase.Refresh();
            foreach (var path in new[] { ModelPath, ModelPath.Replace("02.fbx", "01.fbx") })
            {
                var importer = (ModelImporter)AssetImporter.GetAtPath(path);
                importer.importCameras = false;
                importer.importLights = false;
                importer.importBlendShapes = true;
                importer.isReadable = true;
                importer.animationType = ModelImporterAnimationType.Generic;
                importer.SaveAndReimport();
                var model = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                var lines = new List<string> { "MODEL " + path };
                foreach (var renderer in model.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    var mesh = renderer.sharedMesh;
                    lines.Add($"MESH {renderer.name} vertices={mesh.vertexCount} triangles={mesh.triangles.Length / 3} materials={string.Join(",", renderer.sharedMaterials.Select(m => m.name))}");
                    for (int i = 0; i < mesh.blendShapeCount; i++)
                        lines.Add("SHAPE " + mesh.GetBlendShapeName(i));
                }
                lines.Add("BONES " + string.Join(",", model.GetComponentsInChildren<Transform>().Select(t => t.name)));
                Debug.Log(string.Join("\n", lines));
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                var instance = Object.Instantiate(model);
                ApplyMaterials(instance);
                Render(instance, path.Contains("02.fbx") ? "candidate-02" : "candidate-01");
            }
            Debug.Log("ISEKAID_COMPANION_ASSET_AUDIT_PASSED");
        }

        public static void ApplyMaterials(GameObject instance)
        {
            foreach (var renderer in instance.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                var materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    string name = materials[i].name;
                    string materialPath = Root + "/Materials/" + name + ".mat";
                    var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                    if (material == null)
                    {
                        material = new Material(Shader.Find("Universal Render Pipeline/Simple Lit")) { name = name };
                        material.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(Root + "/Textures/Character/" + name + "_Base.png"));
                        material.SetColor("_BaseColor", Color.white);
                        material.SetFloat("_Smoothness", 0f);
                        AssetDatabase.CreateAsset(material, materialPath);
                    }
                    materials[i] = material;
                }
                renderer.sharedMaterials = materials;
            }
        }

        public static void Render(GameObject instance, string filename)
        {
            var renderers = instance.GetComponentsInChildren<SkinnedMeshRenderer>();
            var bounds = renderers[0].bounds;
            foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);
            Debug.Log("MODEL_BOUNDS " + bounds);
            var light = new GameObject("InspectionLight").AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.3f;
            light.transform.rotation = Quaternion.Euler(35f, 160f, 0f);
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.6f, 0.65f, 0.7f);
            var camera = new GameObject("InspectionCamera").AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.07f, 0.1f, 0.13f);
            camera.transform.position = bounds.center + new Vector3(0f, 0f, bounds.size.y * 2.3f);
            camera.transform.LookAt(bounds.center);
            camera.orthographic = true;
            camera.orthographicSize = bounds.size.y * 0.58f;
            var target = new RenderTexture(900, 1100, 24);
            camera.targetTexture = target;
            camera.Render();
            var previous = RenderTexture.active;
            RenderTexture.active = target;
            var image = new Texture2D(900, 1100, TextureFormat.RGB24, false);
            image.ReadPixels(new Rect(0, 0, 900, 1100), 0, 0);
            image.Apply();
            Directory.CreateDirectory("../Documentation/Evidence/H1");
            File.WriteAllBytes("../Documentation/Evidence/H1/" + filename + ".png", image.EncodeToPNG());
            RenderTexture.active = previous;
            camera.targetTexture = null;
            target.Release();
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(image);
            Object.DestroyImmediate(camera.gameObject);
            Object.DestroyImmediate(light.gameObject);
        }
    }
}
