using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Isekaid
{
    [DefaultExecutionOrder(-200)]
    public sealed class CompanionPrototype : MonoBehaviour
    {
        [SerializeField] private FirstPersonController player;
        [SerializeField] private CompanionController companion;
        [SerializeField] private NavMeshData navigation;

        private NavMeshDataInstance navMeshInstance;
        private float previousTimeScale;
        private bool changingScene;
        public static CompanionPrototype Editing { get; private set; }
        private readonly Dictionary<Behaviour, bool> suspendedViews = new Dictionary<Behaviour, bool>();
        private readonly Dictionary<UIDocument, StyleEnum<DisplayStyle>> suspendedDocuments = new Dictionary<UIDocument, StyleEnum<DisplayStyle>>();
        private bool pausedBeforeCreation;

        public FirstPersonController Player => player;
        public CompanionController Companion => companion;
        public bool IsPaused { get; private set; }
        public event Action Changed;

        private void Awake()
        {
            previousTimeScale = Time.timeScale;
            navMeshInstance = NavMesh.AddNavMeshData(navigation);
        }

        private void OnEnable() => companion.Changed += OnCompanionChanged;
        private void Start() => SetPaused(false);

        private void OnDisable()
        {
            companion.Changed -= OnCompanionChanged;
            player.InputEnabled = false;
            Time.timeScale = previousTimeScale;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnDestroy()
        {
            if (Editing == this) Editing = null;
            if (navMeshInstance.valid)
                navMeshInstance.Remove();
        }

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard != null && (keyboard.tabKey.wasPressedThisFrame || keyboard.escapeKey.wasPressedThisFrame))
                SetPaused(!IsPaused);

            if (IsPaused)
                return;

            bool interact = player.InteractPressed;
            bool command = keyboard != null && keyboard.fKey.wasPressedThisFrame;
            if ((!interact && !command) || !CanInteract())
                return;
            if (interact)
                companion.Interact();
            if (command)
                companion.SetFollowing(!companion.IsFollowing);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                SetPaused(true);
        }

        private void OnCompanionChanged()
        {
            if (companion.State.LifeState == CompanionLifeState.Dead)
                SetPaused(true);
            Changed?.Invoke();
        }

        public bool CanInteract()
        {
            var cameraTransform = player.ViewCamera.transform;
            return Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, 3.2f,
                Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) &&
                hit.collider.GetComponentInParent<CompanionController>() == companion;
        }

        public void SetPaused(bool paused)
        {
            IsPaused = paused || companion.State.LifeState == CompanionLifeState.Dead;
            Time.timeScale = IsPaused ? 0f : 1f;
            player.InputEnabled = !IsPaused;
            Cursor.lockState = IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = IsPaused;
            Changed?.Invoke();
        }

        public void SimulateIncapacitation()
        {
            if (companion.Incapacitate())
                SetPaused(false);
        }

        public void Restart()
        {
            companion.ResetScenario();
            player.ResetPosition();
            SetPaused(false);
        }

        public void OpenCharacterCreation()
        {
            if (changingScene || !Application.CanStreamedLevelBeLoaded(CharacterCreationController.ScenePath))
                return;
            if (companion.State.LifeState == CompanionLifeState.Downed || companion.State.LifeState == CompanionLifeState.Dead)
                return;
            changingScene = true;
            pausedBeforeCreation = IsPaused;
            SetPaused(true);
            Editing = this;
            suspendedViews.Clear();
            foreach (var root in gameObject.scene.GetRootGameObjects())
            {
                foreach (var document in root.GetComponentsInChildren<UIDocument>(true))
                {
                    suspendedDocuments.Add(document, document.rootVisualElement.style.display);
                    document.rootVisualElement.style.display = DisplayStyle.None;
                }
                foreach (var view in root.GetComponentsInChildren<Behaviour>(true))
                    if (view is Camera || view is AudioListener || view is Light)
                    { suspendedViews.Add(view, view.enabled); view.enabled = false; }
            }
            var operation = SceneManager.LoadSceneAsync(CharacterCreationController.ScenePath, LoadSceneMode.Additive);
            operation.completed += _ => SceneManager.SetActiveScene(SceneManager.GetSceneByPath(CharacterCreationController.ScenePath));
        }

        public async Task ResumeFromCreation(CompanionProfile profile)
        {
            var presentation = companion.GetComponent<CompanionAppearance>();
            await presentation.LoadSaved(preserveOnFailure: true);
            if (presentation.RecoveredFallback) throw new InvalidOperationException("La apariencia guardada requiere recuperación.");
            presentation.Apply(profile);
            companion.SetPersonality(profile.personality);
            var creator = SceneManager.GetSceneByPath(CharacterCreationController.ScenePath);
            SceneManager.SetActiveScene(gameObject.scene);
            if (creator.IsValid()) _ = SceneManager.UnloadSceneAsync(creator);
            foreach (var pair in suspendedViews) if (pair.Key != null) pair.Key.enabled = pair.Value;
            foreach (var pair in suspendedDocuments) if (pair.Key != null) pair.Key.rootVisualElement.style.display = pair.Value;
            suspendedViews.Clear();
            suspendedDocuments.Clear();
            Editing = null;
            changingScene = false;
            SetPaused(pausedBeforeCreation);
        }
    }
}
