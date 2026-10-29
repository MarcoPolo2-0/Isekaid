using System;
using System.IO;
using System.Linq;
using Isekaid.Validation;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Isekaid.Editor
{
    [InitializeOnLoad]
    public static class PresentationAudit
    {
        private const string Pending = "Isekaid.PresentationAudit.Pending";
        private static bool running;
        static PresentationAudit() { if (SessionState.GetBool(Pending, false)) EditorApplication.update += Poll; }
        public static void Run()
        {
            string output = F2RuntimeChecks.Argument("-isekTestData");
            if (string.IsNullOrEmpty(output)) throw new InvalidOperationException("An isolated output directory is required.");
            Directory.CreateDirectory(output);
            using (var writer = new StreamWriter(Path.Combine(output, "asset-audit.txt")))
            {
                foreach (string path in new[] { "Assets/_Project/AvatarCatalog/Adventurer.controller", "Assets/Experiments/VrmCompanion/TrialAnimations.controller" })
                {
                    var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
                    writer.WriteLine("CONTROLLER " + path);
                    foreach (var state in controller.layers[0].stateMachine.states)
                    {
                        var clip = state.state.motion as AnimationClip;
                        writer.WriteLine($"STATE {state.state.name} speed={state.state.speed} clip={clip?.name} human={clip?.humanMotion} loop={clip?.isLooping} length={clip?.length}");
                    }
                }
                var model = AssetDatabase.LoadAssetAtPath<GameObject>(CompanionAssetBuilder.ModelPath);
                foreach (var bone in model.GetComponentsInChildren<Transform>()) writer.WriteLine("BONE " + bone.name);
                var scene = EditorSceneManager.OpenScene(CharacterCreationController.GameScenePath);
                writer.WriteLine("LIGHTING " + RenderSettings.ambientMode + " ambient=" + RenderSettings.ambientLight + " sky=" + RenderSettings.ambientSkyColor + " intensity=" + RenderSettings.ambientIntensity);
                foreach (var light in UnityEngine.Object.FindObjectsByType<Light>(FindObjectsSortMode.None)) writer.WriteLine("LIGHT " + light.name + " " + light.type + " intensity=" + light.intensity + " color=" + light.color);
            }
            ShaderUtil.allowAsyncCompilation = false;
            SessionState.SetBool(Pending, true);
            EditorSceneManager.OpenScene(CharacterCreationController.ScenePath);
            EditorApplication.EnterPlaymode();
            EditorApplication.update += Poll;
        }
        private static void Poll()
        {
            if (!EditorApplication.isPlaying || running) return;
            EditorApplication.update -= Poll;
            Begin();
        }
        private static async void Begin()
        {
            running = true;
            try
            {
                F2RuntimeChecks.ResizeForEditor = F2IntegrationChecks.SetViewSize;
                F2IntegrationChecks.SetViewSize(1600, 900);
                await PresentationChecks.Run();
                PresentationChecks.Write(true);
                SessionState.SetBool(Pending, false);
                EditorApplication.Exit(0);
            }
            catch (Exception error)
            {
                PresentationChecks.Report ??= new PresentationChecks.Results();
                PresentationChecks.Report.error = error.ToString();
                PresentationChecks.Write(false);
                SessionState.SetBool(Pending, false);
                Debug.LogError(error);
                EditorApplication.Exit(1);
            }
        }
    }
}
