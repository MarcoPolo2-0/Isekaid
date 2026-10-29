using System;
using Isekaid.Validation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Isekaid.Editor
{
    [InitializeOnLoad]
    public static class F3AIntegrationChecks
    {
        private const string Pending = "Isekaid.F3A.Pending";
        private static bool running;
        static F3AIntegrationChecks()
        {
            if (SessionState.GetBool(Pending, false)) EditorApplication.update += Poll;
        }
        public static void Run()
        {
            ShaderUtil.allowAsyncCompilation = false;
            PrototypeChecks.Run();
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
                F2IntegrationChecks.SetViewSize(1920, 1080);
                await F3ARuntimeChecks.Run();
                F3ARuntimeChecks.Write(true);
                SessionState.SetBool(Pending, false);
                EditorApplication.Exit(0);
            }
            catch (Exception error)
            {
                F3ARuntimeChecks.Report ??= new F3ARuntimeChecks.Results();
                F3ARuntimeChecks.Report.error = error.ToString();
                F3ARuntimeChecks.Write(false);
                SessionState.SetBool(Pending, false);
                Debug.LogError(error);
                EditorApplication.Exit(1);
            }
        }
    }
}
