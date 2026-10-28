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

        private CompanionProfile profile;
        private Label description;
        private Label feedback;
        private Button start;
        private bool loading;

        public CompanionProfile Profile => profile;

        private void OnEnable()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            profile = CompanionProfileStore.Load();
            appearance.Apply(profile);

            var root = GetComponent<UIDocument>().rootVisualElement;
            root.Clear();
            root.styleSheets.Add(styleSheet);
            root.pickingMode = PickingMode.Ignore;
            var header = new VisualElement { name = "creation-header", pickingMode = PickingMode.Ignore };
            root.Add(header);
            Label(header, "ISEKAID", "brand");
            Label(header, "ANTES DEL PRIMER ENCUENTRO", "eyebrow");
            Label(header, "Tu futura\ncompañera", "menu-title");
            Label(header, "Elige su apariencia y personalidad.\nSu historia juntos comienza en el claro.", "body");

            var viewControls = new VisualElement { name = "preview-controls" };
            root.Add(viewControls);
            Label(viewControls, "VISTA DEL PERSONAJE", "eyebrow");
            var rotation = new Slider("Girar", -180f, 180f) { value = 0f };
            rotation.RegisterValueChangedCallback(evt => preview.localRotation = Quaternion.Euler(0f, 180f + evt.newValue, 0f));
            viewControls.Add(rotation);
            Label(viewControls, "Modelo provisional · colores en tiempo real", "hint");

            var menu = new ScrollView { name = "creation-menu" };
            root.Add(menu);
            Label(menu, "01 / CREACIÓN DE PERSONAJE", "eyebrow");
            Label(menu, "Una identidad propia", "creation-title");
            Label(menu, "PERSONALIDAD", "section-title");
            var personalities = new DropdownField(new System.Collections.Generic.List<string>(CompanionPalette.PersonalityNames), (int)profile.personality);
            personalities.RegisterValueChangedCallback(evt => SetPersonality((CompanionPersonality)personalities.index));
            menu.Add(personalities);
            description = Label(menu, "", "body");
            RefreshDescription();
            for (int part = 0; part < CompanionPalette.Parts.Length; part++)
                AddPalette(menu, part);
            Label(menu, "La apariencia y la personalidad son independientes. La confianza se construye durante la aventura.", "hint");
            feedback = Label(menu, "", "feedback");
            start = new Button(() => BeginGame()) { text = "Guardar y comenzar aventura" };
            start.AddToClassList("primary");
            menu.Add(start);
            Label(menu, "La selección se guarda en este equipo. Volver al creador desde el juego inicia un nuevo encuentro al continuar.", "hint");
        }

        public void SetPersonality(CompanionPersonality value)
        {
            if ((int)value < 0 || (int)value >= CompanionPalette.PersonalityNames.Length)
                return;
            profile.personality = value;
            RefreshDescription();
        }

        public void SetColor(int part, int index)
        {
            profile.SetColorIndex(part, index);
            appearance.SetColor(part, CompanionPalette.Colors[part][index]);
        }

        public bool BeginGame()
        {
            if (loading)
                return false;
            if (!Application.CanStreamedLevelBeLoaded(GameScenePath))
            {
                feedback.text = "La escena de aventura no está disponible.";
                return false;
            }
            if (!CompanionProfileStore.TrySave(profile, out string error))
            {
                feedback.text = error;
                return false;
            }
            loading = true;
            start.SetEnabled(false);
            feedback.text = "Entrando al claro...";
            SceneManager.LoadSceneAsync(GameScenePath);
            return true;
        }

        private void AddPalette(VisualElement parent, int part)
        {
            Label(parent, CompanionPalette.Parts[part], "section-title");
            var row = new VisualElement();
            row.AddToClassList("palette");
            parent.Add(row);
            for (int i = 0; i < CompanionPalette.Colors[part].Length; i++)
            {
                int index = i;
                var button = new Button(() =>
                {
                    SetColor(part, index);
                    foreach (var sibling in row.Children())
                        sibling.RemoveFromClassList("selected");
                    row[index].AddToClassList("selected");
                }) { text = CompanionPalette.Names[part][i] };
                button.style.borderBottomWidth = 5f;
                button.style.borderBottomColor = CompanionPalette.Colors[part][i];
                button.EnableInClassList("selected", profile.GetColorIndex(part) == i);
                row.Add(button);
            }
        }

        private void RefreshDescription()
        {
            if (description == null)
                return;
            switch (profile.personality)
            {
                case CompanionPersonality.Tsundere: description.text = "Reservada con su gratitud, directa al hablar. Necesita tiempo para mostrar confianza."; break;
                case CompanionPersonality.Yandere: description.text = "Atenta y de emociones intensas. El vínculo nace de experiencias compartidas, sin romance automático."; break;
                default: description.text = "Cauta y de voz tranquila. Ganar seguridad no cambia su forma reservada de relacionarse."; break;
            }
        }

        private static Label Label(VisualElement parent, string text, string className)
        {
            var label = new Label(text) { pickingMode = PickingMode.Ignore };
            label.AddToClassList(className);
            parent.Add(label);
            return label;
        }
    }
}
