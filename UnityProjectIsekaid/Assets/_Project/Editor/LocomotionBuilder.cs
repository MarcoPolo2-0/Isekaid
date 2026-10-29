using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Isekaid.Editor
{
    public static class LocomotionBuilder
    {
        public const string Root = "Assets/_Project/Locomotion";
        public const string ControllerPath = Root + "/Locomotion.controller";
        public static void Run()
        {
            Directory.CreateDirectory(Root + "/Sources");
            var model = AssetDatabase.LoadAssetAtPath<GameObject>(CompanionAssetBuilder.ModelPath);
            var description = Description(model);
            var clips = new List<AnimationClip>();
            foreach (string name in new[] { "Idle", "Walk", "Run" })
            {
                string path = Root + "/Sources/" + name + ".fbx";
                if (!File.Exists(path)) File.Copy(CompanionAssetBuilder.Root + "/Animations/Unarmed_" + name + ".fbx", path);
                AssetDatabase.ImportAsset(path);
                var importer = (ModelImporter)AssetImporter.GetAtPath(path);
                importer.animationType = ModelImporterAnimationType.Human;
                importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
                importer.humanDescription = description;
                var settings = importer.defaultClipAnimations;
                foreach (var setting in settings)
                {
                    setting.name = name;
                    setting.loopTime = true; setting.loopPose = true;
                    setting.lockRootRotation = true; setting.lockRootHeightY = true; setting.lockRootPositionXZ = true;
                    setting.keepOriginalOrientation = true; setting.keepOriginalPositionY = true;
                }
                importer.clipAnimations = settings;
                importer.SaveAndReimport();
                var clip = AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>().First(c => !c.name.StartsWith("__preview__"));
                if (!clip.humanMotion || !clip.isLooping) throw new InvalidOperationException("Invalid Humanoid loop: " + path);
                clips.Add(clip);
            }
            var instance = UnityEngine.Object.Instantiate(model);
            var avatar = AvatarBuilder.BuildHumanAvatar(instance, description);
            if (!avatar.isValid || !avatar.isHuman) throw new InvalidOperationException("Catalog Humanoid mapping is invalid");
            avatar.name = "AdventurerHumanoid";
            SaveAsset(avatar, Root + "/AdventurerHumanoid.asset");
            UnityEngine.Object.DestroyImmediate(instance);
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
            if (controller == null) controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
            foreach (var child in controller.layers[0].stateMachine.states) controller.layers[0].stateMachine.RemoveState(child.state);
            foreach (var oldTree in AssetDatabase.LoadAllAssetsAtPath(ControllerPath).OfType<BlendTree>()) UnityEngine.Object.DestroyImmediate(oldTree, true);
            controller.parameters = new[] { new AnimatorControllerParameter { name = "Speed", type = AnimatorControllerParameterType.Float }, new AnimatorControllerParameter { name = "Cadence", type = AnimatorControllerParameterType.Float, defaultFloat = 1 } };
            var tree = new BlendTree { name = "GroundSpeed", blendType = BlendTreeType.Simple1D, blendParameter = "Speed", useAutomaticThresholds = false };
            AssetDatabase.AddObjectToAsset(tree, controller);
            tree.AddChild(clips[1], 1.4f); tree.AddChild(clips[2], 3.8f);
            var children = tree.children;
            // Measured stance velocities are 0.87 and 5.44 m/s at the catalog's display scale.
            children[0].timeScale = 1.61f; children[1].timeScale = .70f; tree.children = children;
            var state = controller.layers[0].stateMachine.AddState("Locomotion"); state.motion = tree;
            state.iKOnFeet = true;
            state.speedParameter = "Cadence"; state.speedParameterActive = true;
            var idle = controller.layers[0].stateMachine.AddState("Idle"); idle.motion = clips[0];
            idle.iKOnFeet = true;
            controller.layers[0].stateMachine.defaultState = idle;
            var start = idle.AddTransition(state); start.hasExitTime = false; start.hasFixedDuration = true; start.duration = .15f;
            start.AddCondition(AnimatorConditionMode.Greater, .12f, "Speed");
            var stop = state.AddTransition(idle); stop.hasExitTime = false; stop.hasFixedDuration = true; stop.duration = .15f;
            stop.AddCondition(AnimatorConditionMode.Less, .06f, "Speed");
            string prefabPath = "Assets/_Project/AvatarCatalog/Adventurer.prefab";
            var prefab = PrefabUtility.LoadPrefabContents(prefabPath);
            try
            {
                var animator = prefab.GetComponentInChildren<Animator>();
                animator.avatar = AssetDatabase.LoadAssetAtPath<Avatar>(Root + "/AdventurerHumanoid.asset");
                animator.runtimeAnimatorController = controller;
                var position = animator.transform.localPosition; position.y = 0; animator.transform.localPosition = position;
                PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
            }
            finally { PrefabUtility.UnloadPrefabContents(prefab); }
            foreach (string path in new[] { CharacterCreationController.ScenePath, CharacterCreationController.GameScenePath })
            {
                var scene = EditorSceneManager.OpenScene(path);
                var serialized = new SerializedObject(UnityEngine.Object.FindAnyObjectByType<CompanionAppearance>());
                serialized.FindProperty("vrmAnimations").objectReferenceValue = controller;
                serialized.ApplyModifiedPropertiesWithoutUndo();
                EditorSceneManager.SaveScene(scene);
            }
            AssetDatabase.SaveAssets();
            Debug.Log("ISEKAID_LOCOMOTION_BUILT");
        }
        public static void BalanceLighting()
        {
            foreach (string path in new[] { CharacterCreationController.ScenePath, CharacterCreationController.GameScenePath })
            {
                var scene = EditorSceneManager.OpenScene(path);
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
                RenderSettings.ambientLight = new Color(.4f, .4f, .4f);
                foreach (var light in UnityEngine.Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
                    if (light.type == LightType.Directional) { light.intensity = .65f; light.color = Color.white; }
                EditorSceneManager.SaveScene(scene);
            }
            Debug.Log("ISEKAID_BALANCED_LIGHTING_SAVED");
        }
        private static void SaveAsset(Avatar avatar, string path)
        {
            var existing = AssetDatabase.LoadAssetAtPath<Avatar>(path);
            if (existing == null) AssetDatabase.CreateAsset(avatar, path);
            else { EditorUtility.CopySerialized(avatar, existing); UnityEngine.Object.DestroyImmediate(avatar); }
        }
        private static HumanDescription Description(GameObject model)
        {
            var map = new Dictionary<string, string>
            {
                ["Hips"] = "root.x", ["Spine"] = "spine_01.x", ["Chest"] = "spine_02.x", ["UpperChest"] = "spine_03.x", ["Neck"] = "neck.x", ["Head"] = "head.x"
            };
            foreach (var side in new[] { ("Left", "l"), ("Right", "r") })
            {
                foreach (var bone in new[] { ("Shoulder", "shoulder"), ("UpperArm", "arm_stretch"), ("LowerArm", "forearm_stretch"), ("Hand", "hand"), ("UpperLeg", "thigh_stretch"), ("LowerLeg", "leg_stretch"), ("Foot", "foot"), ("Toes", "toes_01") })
                    map[side.Item1 + bone.Item1] = bone.Item2 + "." + side.Item2;
                foreach (var finger in new[] { ("Index", "index"), ("Middle", "middle"), ("Ring", "ring"), ("Little", "pinky"), ("Thumb", "thumb") })
                    for (int i = 0; i < 3; i++) map[side.Item1 + finger.Item1 + new[] { "Proximal", "Intermediate", "Distal" }[i]] = "c_" + finger.Item2 + (i + 1) + "." + side.Item2;
            }
            return new HumanDescription
            {
                human = map.Select(p => new HumanBone { humanName = p.Key, boneName = p.Value, limit = new HumanLimit { useDefaultValues = true } }).ToArray(),
                skeleton = model.GetComponentsInChildren<Transform>().Select(t => new SkeletonBone { name = t.name, position = t.localPosition, rotation = t.localRotation, scale = t.localScale }).ToArray(),
                upperArmTwist = .5f, lowerArmTwist = .5f, upperLegTwist = .5f, lowerLegTwist = .5f, armStretch = .05f, legStretch = .05f, feetSpacing = 0, hasTranslationDoF = false
            };
        }
    }
}
