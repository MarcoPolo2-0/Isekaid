using System;
using System.IO;
using Isekaid.ExperimentalVrm;
using Isekaid.Validation;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Isekaid.Editor
{
    [InitializeOnLoad]
    public static class F2IntegrationChecks
    {
        private const string Pending = "Isekaid.F2Checks.Pending";
        private static bool running;
        static F2IntegrationChecks()
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
            if (!SessionState.GetBool(Pending, false) || running) return;
            if (!EditorApplication.isPlaying) return;
            running = true;
            try
            {
                SetViewSize(1920, 1080);
                F2RuntimeChecks.ResizeForEditor = SetViewSize;
                await F2RuntimeChecks.Run();
                F2RuntimeChecks.Write(true);
                SessionState.SetBool(Pending, false);
                EditorApplication.Exit(0);
            }
            catch (Exception error)
            {
                F2RuntimeChecks.Report ??= new F2RuntimeChecks.Results();
                F2RuntimeChecks.Report.error = error.ToString();
                F2RuntimeChecks.Write(false);
                SessionState.SetBool(Pending, false);
                Debug.LogError(error);
                EditorApplication.Exit(1);
            }
        }
        public static void SetViewSize(int width, int height)
        {
            var assembly = typeof(UnityEditor.Editor).Assembly;
            var sizesType = assembly.GetType("UnityEditor.GameViewSizes");
            var singleton = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
            var sizes = singleton.GetProperty("instance").GetValue(null);
            var groupType = assembly.GetType("UnityEditor.GameViewSizeGroupType");
            var group = sizesType.GetMethod("GetGroup").Invoke(sizes, new[] { Enum.Parse(groupType, "Standalone") });
            var sizeType = assembly.GetType("UnityEditor.GameViewSize");
            var kindType = assembly.GetType("UnityEditor.GameViewSizeType");
            var size = Activator.CreateInstance(sizeType, Enum.Parse(kindType, "FixedResolution"), width, height, "F2 validation");
            group.GetType().GetMethod("AddCustomSize").Invoke(group, new[] { size });
            int count = (int)group.GetType().GetMethod("GetTotalCount").Invoke(group, null);
            var viewType = assembly.GetType("UnityEditor.GameView");
            var view = EditorWindow.GetWindow(viewType);
            viewType.GetProperty("selectedSizeIndex").SetValue(view, count - 1);
        }
        public static void Storage()
        {
            if (F2RuntimeChecks.Argument("-isekTestData") == null) throw new InvalidOperationException("An isolated test directory is required.");
            bool had = PlayerPrefs.HasKey(CompanionProfileStore.PreferenceKey);
            string previous = PlayerPrefs.GetString(CompanionProfileStore.PreferenceKey, "");
            int passed = 0;
            void Check(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); passed++; }
            try
            {
                string legacy = "{\"version\":1,\"personality\":1,\"hair\":2,\"eyes\":3,\"top\":1,\"trousers\":2,\"memoryFixture\":{\"firstRescue\":true,\"trust\":7},\"classFixture\":\"archer\"}";
                PlayerPrefs.SetString(CompanionProfileStore.PreferenceKey, legacy);
                var record = CompanionProfileStore.LoadRecord();
                Check(record.profile.version == 2 && record.profile.personality == CompanionPersonality.Yandere && record.profile.hair == 2 && record.profile.eyes == 3 && record.profile.top == 1 && record.profile.trousers == 2, "Migration must preserve v1 personality and historical colors");
                Check(record.legacyProfileJson == legacy && !string.IsNullOrEmpty(record.profile.identity), "Migration retains unknown legacy fields as exact source JSON");
                string identity = record.profile.identity;
                Check(CompanionProfileStore.TrySave(record, out _), "Migrated profile must save");
                Check(PlayerPrefs.GetString(CompanionProfileStore.PreferenceKey) == legacy, "Legacy PlayerPrefs remains unchanged");
                record.profile.personality = CompanionPersonality.Shy;
                Check(CompanionProfileStore.TrySave(record, out _), "Second profile revision must save");
                string before = File.ReadAllText(CompanionProfileStore.SavePath);
                CompanionProfileStore.BeforeCommit = () => throw new IOException("Injected disk failure");
                record.profile.personality = CompanionPersonality.Tsundere;
                Check(!CompanionProfileStore.TrySave(record, out _) && before == File.ReadAllText(CompanionProfileStore.SavePath), "Disk failure leaves previous record byte-identical");
                CompanionProfileStore.BeforeCommit = null;
                File.WriteAllText(CompanionProfileStore.SavePath, "truncated");
                var recovered = CompanionProfileStore.LoadRecord();
                Check(recovered.profile.identity == identity && recovered.profile.personality == CompanionPersonality.Yandere && recovered.legacyProfileJson == legacy, "Corrupt primary recovers verified backup with identity and original fields");
                Check(CompanionProfileStore.TrySave(recovered, out _) && CompanionProfileStore.Load().identity == identity, "Saving recovery preserves the valid backup");
                var permissions = AvatarPermissions.Describe("{\"name\":\"Authorized test metadata\",\"authors\":[\"Fixture author\"],\"avatarPermission\":\"onlyAuthor\",\"commercialUsage\":\"personalNonProfit\",\"creditNotation\":\"required\",\"modification\":\"prohibited\",\"allowRedistribution\":false}");
                Check(permissions.Contains("onlyAuthor") && permissions.Contains("personalNonProfit") && permissions.Contains("required") && permissions.Contains("no verifica"), "Metadata reports restrictions separately from human permissions");
                string atomicPath = Path.Combine(AvatarFiles.Root, "atomic-check.bin");
                File.WriteAllText(atomicPath, "previous");
                bool lockedFailure = false;
                using (var locked = new FileStream(atomicPath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    try { AvatarFiles.AtomicWrite(atomicPath, new byte[] { 1, 2, 3 }); }
                    catch (IOException) { lockedFailure = true; }
                }
                Check(lockedFailure && File.ReadAllText(atomicPath) == "previous", "A real filesystem replacement failure preserves the original file");
                Check(Directory.GetFiles(AvatarFiles.Root, "atomic-check.bin.tmp-*").Length == 0, "A failed atomic replacement cleans its temporary file");
                Directory.CreateDirectory(AvatarFiles.Root);
                File.WriteAllText(Path.Combine(AvatarFiles.Root, "storage-results.json"), "{\"passed\":true,\"assertions\":" + passed + "}");
                Debug.Log("ISEKAID_F2_STORAGE_CHECKS_PASSED " + passed);
            }
            finally
            {
                CompanionProfileStore.BeforeCommit = null;
                if (had) PlayerPrefs.SetString(CompanionProfileStore.PreferenceKey, previous); else PlayerPrefs.DeleteKey(CompanionProfileStore.PreferenceKey);
                PlayerPrefs.Save();
            }
        }
        public static void StorageAndRegression()
        {
            Storage();
            PrototypePlayModeChecks.Run();
        }
        public static void BuildWindows()
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64))
                throw new InvalidOperationException("Windows build support is unavailable.");
            var graphics = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset")[0]);
            var shaders = graphics.FindProperty("m_AlwaysIncludedShaders");
            foreach (string name in new[] { "VRM10/Universal Render Pipeline/MToon10", "UniGLTF/UniUnlit" })
            {
                var shader = Shader.Find(name);
                if (shader == null) throw new InvalidOperationException("Required runtime shader is missing: " + name);
                bool present = false;
                for (int i = 0; i < shaders.arraySize; i++) if (shaders.GetArrayElementAtIndex(i).objectReferenceValue == shader) present = true;
                if (!present) { int index = shaders.arraySize++; shaders.GetArrayElementAtIndex(index).objectReferenceValue = shader; }
            }
            graphics.ApplyModifiedPropertiesWithoutUndo(); AssetDatabase.SaveAssets();
            string output = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Builds/F2Windows/Isekaid.exe");
            var result = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { CharacterCreationController.ScenePath, CharacterCreationController.GameScenePath },
                locationPathName = output, target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.Development
            });
            Debug.Log("ISEKAID_F2_WINDOWS_BUILD " + result.summary.result + " bytes=" + result.summary.totalSize);
            if (result.summary.result != BuildResult.Succeeded) throw new InvalidOperationException("Windows build failed.");
            string notices = Path.Combine(Path.GetDirectoryName(output), "ThirdPartyNotices");
            Directory.CreateDirectory(notices);
            File.Copy(Path.Combine(Application.dataPath, "_Project/AvatarCatalog.LICENSE.txt"), Path.Combine(notices, "Adventurer.txt"), true);
            string licenses = Path.GetFullPath(Path.Combine(Application.dataPath, "../../Documentation/Audits/VRM-F2/licenses"));
            foreach (string license in Directory.GetFiles(licenses, "*.txt")) File.Copy(license, Path.Combine(notices, Path.GetFileName(license)), true);
        }
    }
}
