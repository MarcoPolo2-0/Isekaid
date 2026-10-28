using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

public static class WomanImportAudit
{
    [Serializable] public sealed class MeshResult
    {
        public string name;
        public int vertices;
        public int bones;
        public string[] blendShapes;
        public float[] deformationMaxDistances;
    }
    [Serializable] public sealed class Result
    {
        public string unity;
        public string scope = "Isolated editor import of diagnostic FBX; no production integration, animation clips or Play Mode.";
        public bool avatarValid;
        public bool avatarHuman;
        public MeshResult[] meshes;
    }

    public static void Run()
    {
        const string source = "Assets/woman_deformation_probe.fbx";
        AssetDatabase.Refresh();
        var importer = (ModelImporter)AssetImporter.GetAtPath(source);
        importer.animationType = ModelImporterAnimationType.Human;
        importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
        importer.importBlendShapes = true;
        importer.isReadable = true;
        importer.importCameras = false;
        importer.importLights = false;
        importer.importAnimation = false;
        importer.SaveAndReimport();
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(source);
        var avatar = AssetDatabase.LoadAllAssetsAtPath(source).OfType<Avatar>().FirstOrDefault();
        var report = new Result { unity = Application.unityVersion, avatarValid = avatar != null && avatar.isValid, avatarHuman = avatar != null && avatar.isHuman };
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var instance = Object.Instantiate(prefab);
        var renderers = instance.GetComponentsInChildren<SkinnedMeshRenderer>();
        report.meshes = new MeshResult[renderers.Length];
        for (int index = 0; index < renderers.Length; index++)
        {
            var renderer = renderers[index];
            var mesh = renderer.sharedMesh;
            var result = new MeshResult { name = renderer.name, vertices = mesh.vertexCount, bones = renderer.bones.Length,
                blendShapes = Enumerable.Range(0, mesh.blendShapeCount).Select(mesh.GetBlendShapeName).ToArray(),
                deformationMaxDistances = new float[mesh.blendShapeCount] };
            var baseline = new Mesh();
            renderer.BakeMesh(baseline);
            var rest = baseline.vertices;
            for (int key = 0; key < mesh.blendShapeCount; key++)
            {
                renderer.SetBlendShapeWeight(key, 100);
                var deformed = new Mesh();
                renderer.BakeMesh(deformed);
                var points = deformed.vertices;
                float maximum = 0f;
                for (int vertex = 0; vertex < rest.Length; vertex++)
                    maximum = Mathf.Max(maximum, Vector3.Distance(rest[vertex], points[vertex]));
                result.deformationMaxDistances[key] = maximum;
                renderer.SetBlendShapeWeight(key, 0);
                Object.DestroyImmediate(deformed);
            }
            report.meshes[index] = result;
            Object.DestroyImmediate(baseline);
            var material = new Material(Shader.Find("Standard"));
            material.color = renderer.name.Contains("Body") ? new Color(.62f, .65f, .69f) : new Color(.2f, .4f, .55f);
            material.SetFloat("_Glossiness", .1f);
            renderer.sharedMaterial = material;
        }
        string output = Environment.GetCommandLineArgs().SkipWhile(arg => arg != "-auditOutput").Skip(1).First();
        Directory.CreateDirectory(output);
        File.WriteAllText(Path.Combine(output, "unity-import-results.json"), JsonUtility.ToJson(report, true));
        var light = new GameObject("AuditLight").AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.1f;
        light.transform.rotation = Quaternion.Euler(35, -155, 0);
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(.45f, .48f, .52f);
        var camera = new GameObject("AuditCamera").AddComponent<Camera>();
        camera.transform.position = new Vector3(0, 1.10f, 4);
        camera.transform.LookAt(new Vector3(0, 1.10f, 0));
        camera.orthographic = true;
        camera.orthographicSize = 1.38f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(.07f, .1f, .13f);
        var target = new RenderTexture(960, 1200, 24);
        camera.targetTexture = target;
        camera.Render();
        var previous = RenderTexture.active;
        RenderTexture.active = target;
        var image = new Texture2D(960, 1200, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, 960, 1200), 0, 0);
        image.Apply();
        File.WriteAllBytes(Path.Combine(output, "13-unity-isolated-import.png"), image.EncodeToPNG());
        RenderTexture.active = previous;
        camera.targetTexture = null;
        target.Release();
        Object.DestroyImmediate(target);
        Object.DestroyImmediate(image);
        if (!report.avatarValid || !report.avatarHuman || report.meshes.Length != 3 ||
            report.meshes.Any(mesh => mesh.blendShapes.Length != 6 || mesh.bones == 0) ||
            !report.meshes.First(mesh => mesh.name.Contains("Body")).deformationMaxDistances.All(distance => distance > .001f))
            throw new InvalidOperationException("The isolated import did not preserve the expected avatar, shapes or weights. See the JSON report.");
        Debug.Log("ISEK_001_ISOLATED_UNITY_IMPORT_PASSED");
    }
}
