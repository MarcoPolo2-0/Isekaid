using System;
using System.Threading;
using System.Threading.Tasks;
using Isekaid.ExperimentalVrm;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Isekaid
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class CharacterCreationController : MonoBehaviour
    {
        public const string ScenePath = "Assets/_Project/Scenes/CharacterCreation.unity";
        public const string GameScenePath = "Assets/_Project/Scenes/Game.unity";
        [SerializeField] private CompanionAppearance appearance;
        [SerializeField] private Transform preview;
        [SerializeField] private StyleSheet styleSheet;
        private CompanionProfileStore.Record record;
        private CompanionProfileStore.Record previousSavedRecord;
        private Label description, feedback, permissions;
        private Button start, prepare, cancel;
        private Toggle acknowledgment;
        private Foldout localOptions;
        private string pendingPath;
        private string pendingHash;
        private bool loading, recoveryConfirmed;
        private CancellationTokenSource importCancellation;
        public CompanionProfile Profile => record.profile;
        public CompanionAppearance Appearance => appearance;
        public string Feedback => feedback?.text;

        private async void OnEnable()
        {
            Time.timeScale = CompanionPrototype.Editing == null ? 1f : 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            record = CompanionProfileStore.LoadRecord();
            string warning = CompanionProfileStore.LastWarning;
            appearance.Apply(Profile);
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.Clear();
            root.styleSheets.Add(styleSheet);
            root.pickingMode = PickingMode.Ignore;
            var header = new VisualElement { name = "creation-header", pickingMode = PickingMode.Ignore };
            root.Add(header);
            AddLabel(header, "ISEKAID", "brand");
            AddLabel(header, "TU COMPAÑERA", "eyebrow");
            AddLabel(header, "Una identidad propia", "creation-title");
            AddLabel(header, "Apariencia y personalidad independientes", "hint");
            var controls = new VisualElement { name = "preview-controls" };
            root.Add(controls);
            var rotation = new Slider("Girar", -180, 180);
            rotation.RegisterValueChangedCallback(evt => preview.localRotation = Quaternion.Euler(0, 180 + evt.newValue, 0));
            controls.Add(rotation);
            var motions = new VisualElement(); motions.AddToClassList("palette"); controls.Add(motions);
            foreach (var name in new[] { "Idle", "Walk", "Run" })
            { var key = name; motions.Add(new Button(() => appearance.PreviewMotion(key)) { text = key == "Idle" ? "Reposo" : key == "Walk" ? "Caminar" : "Correr" }); }
            var expression = new DropdownField("Expresión", new System.Collections.Generic.List<string> { "neutral", "happy", "angry", "sad", "relaxed", "surprised", "blink", "aa" }, 0);
            expression.RegisterValueChangedCallback(evt =>
            {
                if (!appearance.PreviewExpression(evt.newValue)) feedback.text = "Este avatar no ofrece ese canal; se mantiene una expresión neutral.";
            });
            controls.Add(expression);
            AddLabel(controls, "Muestras visuales: no generan recuerdos ni confianza.", "hint");
            var panel = new VisualElement { name = "creation-menu" }; root.Add(panel);
            var menu = new ScrollView { name = "creation-content" }; panel.Add(menu);
            menu.style.flexGrow = 1; menu.style.flexShrink = 1; menu.style.minHeight = 0;
            AddLabel(menu, "01 / APARIENCIA", "eyebrow");
            menu.Add(new Button(SelectCatalog) { text = "Adventurer · apariencia incluida" });
            AddLabel(menu, "Dawn to Dusk Games · CC0-1.0 · arte provisional. Disponible sin conexión.", "hint");
            menu.Add(new Button(PickAvatar) { text = "Importar VRM 1.0 desde este equipo…" });
            AddLabel(menu, "Windows · hasta 32 MiB · texturas PNG incorporadas. El archivo permanece privado en este equipo.", "hint");
            localOptions = new Foldout { text = "Archivo local y permisos", value = false }; menu.Add(localOptions);
            permissions = AddLabel(localOptions, "", "body");
            AddLabel(localOptions, "Declaro ser autor o disponer de permiso separado para usar y copiar localmente este avatar.", "hint");
            acknowledgment = new Toggle("Confirmar permiso de uso local");
            acknowledgment.style.whiteSpace = WhiteSpace.Normal;
            localOptions.Add(acknowledgment);
            AddLabel(localOptions, "Esta declaración no verifica licencias ni autoriza redistribución. Revisa también los recursos incorporados.", "hint");
            prepare = new Button(PreparePending) { text = "Preparar vista previa del archivo" }; localOptions.Add(prepare);
            cancel = new Button(CancelImport) { text = "Cancelar selección / importación" }; localOptions.Add(cancel);
            AddLabel(menu, "02 / PERSONALIDAD", "section-title");
            var personalities = new DropdownField(new System.Collections.Generic.List<string>(CompanionPalette.PersonalityNames), (int)Profile.personality);
            personalities.RegisterValueChangedCallback(evt => SetPersonality((CompanionPersonality)personalities.index)); menu.Add(personalities);
            description = AddLabel(menu, "", "body"); RefreshDescription();
            var colors = new Foldout { text = "Colores históricos del prototipo", value = false }; menu.Add(colors);
            AddLabel(colors, "Se conservan para compatibilidad. No modifican las texturas ni los canales del VRM o Adventurer.", "hint");
            for (int part = 0; part < CompanionPalette.Parts.Length; part++) AddPalette(colors, part);
            feedback = AddLabel(panel, warning, "feedback");
            start = new Button(() => BeginGame()) { text = CompanionPrototype.Editing == null ? "Guardar y comenzar aventura" : "Guardar y volver a la aventura" };
            start.AddToClassList("primary"); panel.Add(start);
            AddLabel(panel, "Cambiar apariencia conserva la identidad. Durante una aventura se vuelve al mismo encuentro.", "hint");
            RefreshControls();
            try
            {
                importCancellation = new CancellationTokenSource();
                await appearance.LoadSaved(importCancellation.Token);
                if (this == null) return;
                if (record.appearance.kind == "vrm") permissions.text = AvatarPermissions.Describe(record.appearance.metadataJson);
                feedback.text = warning + (warning.Length > 0 ? "\n" : "") + appearance.Status;
            }
            catch (OperationCanceledException) { }
            catch (Exception) { if (this != null) feedback.text = "No se pudo preparar la vista previa. Selecciona la apariencia incluida o reimporta."; }
            if (this != null) RefreshControls();
        }

        private void RefreshControls()
        {
            if (start == null) return;
            start.SetEnabled(!loading && !appearance.Busy && appearance.Ready && !appearance.RecoveredFallback && pendingPath == null);
            prepare.SetEnabled(!loading && !appearance.Busy && pendingPath != null);
            cancel.SetEnabled(!loading && (appearance.Busy || pendingPath != null));
        }

        public void SelectCatalog()
        {
            if (appearance.Busy) { CancelImport(); feedback.text = "Espera a que finalice la cancelación y vuelve a elegir Adventurer."; return; }
            pendingPath = null;
            acknowledgment.value = false;
            appearance.SelectCatalog();
            recoveryConfirmed = true;
            permissions.text = "Adventurer · CC0-1.0. Fuente: dawn-to-dusk-games.itch.io/3d-anime-female-adventurer";
            feedback.text = appearance.Status;
            RefreshControls();
        }

        private void PickAvatar()
        {
            try
            {
                string path = WindowsAvatarPicker.Pick();
                if (path == null) { feedback.text = "Selector cancelado; se conserva la apariencia preparada."; return; }
                CancelImport();
                var selection = AvatarSelection.FromBytes(VrmFileGuard.ReadLocal(path), false);
                pendingPath = path;
                pendingHash = selection.hash;
                localOptions.value = true;
                permissions.text = AvatarPermissions.Describe(selection.metadataJson);
                acknowledgment.value = false;
                feedback.text = "Archivo compatible con la validación inicial. Revisa sus permisos antes de prepararlo.";
            }
            catch (Exception error) { feedback.text = "No se pudo seleccionar el archivo. " + error.Message; }
            RefreshControls();
        }

        private async void PreparePending()
        {
            try
            {
                var selection = AvatarSelection.FromBytes(VrmFileGuard.ReadLocal(pendingPath), false);
                if (selection.hash != pendingHash)
                {
                    pendingHash = selection.hash; acknowledgment.value = false;
                    permissions.text = AvatarPermissions.Describe(selection.metadataJson);
                    feedback.text = "El archivo cambió. Revisa sus permisos y confirma de nuevo.";
                    return;
                }
                await ImportAvatarAsync(pendingPath, acknowledgment.value);
            }
            catch (Exception) { }
        }

        public async Task ImportAvatarAsync(string path, bool acknowledged)
        {
            if (loading) throw new InvalidOperationException("Se está guardando la selección.");
            if (appearance.Busy) throw new InvalidOperationException("Espera a que finalice la importación cancelada.");
            importCancellation?.Cancel();
            importCancellation?.Dispose();
            importCancellation = new CancellationTokenSource();
            try
            {
                var metadata = AvatarSelection.FromBytes(VrmFileGuard.ReadLocal(path), acknowledged);
                permissions.text = AvatarPermissions.Describe(metadata.metadataJson);
                acknowledgment.SetValueWithoutNotify(acknowledged);
                localOptions.value = true;
                feedback.text = "Preparando el avatar. Puedes cancelar; la apariencia anterior se conserva.";
                var operation = appearance.LoadVrm(path, acknowledged, importCancellation.Token, metadata.hash);
                RefreshControls();
                await operation;
                if (this == null) return;
                pendingPath = null;
                recoveryConfirmed = true;
                feedback.text = appearance.Status + $" Importación: {appearance.Importer.ImportMilliseconds:F0} ms.";
            }
            catch (OperationCanceledException)
            { if (this != null) feedback.text = "Importación cancelada; se conserva la apariencia anterior."; throw; }
            catch (Exception error)
            { if (this != null) feedback.text = "No se pudo importar. " + error.Message; throw; }
            finally { if (this != null) RefreshControls(); }
        }

        public void CancelImport()
        {
            importCancellation?.Cancel();
            pendingPath = null;
            if (feedback != null) feedback.text = "Selección cancelada; se conserva la apariencia preparada.";
            RefreshControls();
        }

        public void SetPersonality(CompanionPersonality value)
        {
            if ((int)value < 0 || (int)value >= CompanionPalette.PersonalityNames.Length) return;
            Profile.personality = value;
            RefreshDescription();
        }
        public void SetColor(int part, int index)
        {
            Profile.SetColorIndex(part, index);
            appearance.SetColor(part, CompanionPalette.Colors[part][index]);
        }

        public bool BeginGame()
        {
            if (loading || appearance.Busy || !appearance.Ready || appearance.RecoveredFallback || pendingPath != null) return false;
            if (!Application.CanStreamedLevelBeLoaded(GameScenePath)) { feedback.text = "La escena de aventura no está disponible."; return false; }
            try
            {
                if (appearance.Selection.kind == "vrm") AvatarFiles.Admit(appearance.Selection, appearance.Importer.LoadedBytes);
                previousSavedRecord = CompanionProfileStore.LoadRecord();
                record.appearance = appearance.Selection;
                if (!CompanionProfileStore.TrySave(record, out var error, recoveryConfirmed)) { feedback.text = error; return false; }
            }
            catch (Exception error) { feedback.text = "No se pudo guardar. Se conserva la selección anterior. " + error.Message; return false; }
            loading = true;
            RefreshControls();
            feedback.text = "Preparando la aventura…";
            if (CompanionPrototype.Editing != null) ResumeGame();
            else SceneManager.LoadSceneAsync(GameScenePath);
            return true;
        }

        private async void ResumeGame()
        {
            try { await CompanionPrototype.Editing.ResumeFromCreation(Profile); }
            catch (Exception)
            {
                if (this == null) return;
                bool restored = CompanionProfileStore.TrySave(previousSavedRecord, out _);
                loading = false;
                feedback.text = "La aventura anterior se conserva en pausa. No se pudo actualizar su vista; vuelve a intentarlo." +
                    (restored ? " Se recuperó la selección guardada anterior." : " El respaldo del perfil sigue disponible para recuperación.");
                RefreshControls();
            }
        }

        private void OnDisable() { importCancellation?.Cancel(); }
        private void OnDestroy() { importCancellation?.Dispose(); }
        private void AddPalette(VisualElement parent, int part)
        {
            AddLabel(parent, CompanionPalette.Parts[part], "section-title");
            var row = new VisualElement(); row.AddToClassList("palette"); parent.Add(row);
            for (int i = 0; i < CompanionPalette.Colors[part].Length; i++)
            {
                int index = i;
                var button = new Button(() => SetColor(part, index)) { text = CompanionPalette.Names[part][i] };
                button.style.borderBottomWidth = 5; button.style.borderBottomColor = CompanionPalette.Colors[part][i]; row.Add(button);
            }
        }
        private void RefreshDescription()
        {
            if (description == null) return;
            switch (Profile.personality)
            {
                case CompanionPersonality.Tsundere: description.text = "Reservada con su gratitud, directa al hablar. Necesita tiempo para mostrar confianza."; break;
                case CompanionPersonality.Yandere: description.text = "Atenta y de emociones intensas. El vínculo nace de experiencias compartidas, sin romance automático."; break;
                default: description.text = "Cauta y de voz tranquila. Ganar seguridad no cambia su forma reservada de relacionarse."; break;
            }
        }
        private static Label AddLabel(VisualElement parent, string text, string className)
        {
            var label = new Label(text) { pickingMode = PickingMode.Ignore }; label.AddToClassList(className); parent.Add(label); return label;
        }
    }
}
