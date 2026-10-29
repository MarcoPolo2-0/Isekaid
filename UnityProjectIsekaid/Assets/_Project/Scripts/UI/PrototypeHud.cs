using UnityEngine;
using UnityEngine.UIElements;

namespace Isekaid
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class PrototypeHud : MonoBehaviour
    {
        [SerializeField] private CompanionPrototype prototype;
        [SerializeField] private StyleSheet styleSheet;

        private VisualElement root;
        private VisualElement menu;
        private Label status;
        private Label objective;
        private Label prompt;
        private Label dialogue;
        private Label timer;
        private Label appearanceNotice;
        private Button resume;
        private Button incapacitate;
        private float nextHudUpdate;
        private float dialogueUntil;
        private DialoguePresenter conversation;

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            root.Clear();
            root.styleSheets.Add(styleSheet);
            root.pickingMode = PickingMode.Ignore;

            var header = new VisualElement { name = "header", pickingMode = PickingMode.Ignore };
            root.Add(header);
            AddLabel(header, "ISEKAID", "brand");
            AddLabel(header, "EL PRIMER ENCUENTRO", "eyebrow");
            objective = AddLabel(header, "", "objective");
            status = AddLabel(header, "", "status");
            timer = AddLabel(header, "", "timer");
            appearanceNotice = AddLabel(header, "", "hint");
            AddLabel(root, "+", "crosshair");
            prompt = AddLabel(root, "", "prompt");
            dialogue = AddLabel(root, "", "dialogue");
            AddLabel(root, "WASD  Moverse     Ratón  Mirar     Shift  Correr     Espacio  Saltar     Tab / Esc  Pausa", "controls");
            conversation = new DialoguePresenter(root, prototype.Companion.Dialogue, () => prototype.Companion.State.Personality);

            menu = new ScrollView { name = "menu" };
            root.Add(menu);
            AddLabel(menu, "ISEKAID · AVENTURA", "eyebrow");
            AddLabel(menu, "Un primer paso\njuntos", "menu-title");
            AddLabel(menu, "El sendero bloqueado · juego en pausa", "subtitle");
            AddLabel(menu, "Acércate a la compañera y mantén E para liberarla. Después puedes conversar y usar F para alternar entre seguirte y esperar.", "body");
            resume = AddButton(menu, "Entrar al claro", () => prototype.SetPaused(false));
            resume.AddToClassList("primary");

            AddLabel(menu, "ENSAYO DE SUPERVIVENCIA", "section-title");
            AddLabel(menu, "Después de liberarla puedes simular una incapacitación. Tendrás 30 segundos de juego activo para volver a ayudarla. La pausa detiene la cuenta.", "body");
            incapacitate = AddButton(menu, "Simular incapacitación", prototype.SimulateIncapacitation);
            AddButton(menu, "Reiniciar encuentro", Restart);
            AddButton(menu, "Volver al creador de personaje", prototype.OpenCharacterCreation);
            AddLabel(menu, "El creador conserva este encuentro en pausa y permite cambiar apariencia y personalidad. No está disponible durante una incapacitación. El guardado de la aventura entre sesiones sigue pendiente.", "hint");

            prototype.Changed += Refresh;
            prototype.Companion.Spoke += ShowDialogue;
            Refresh();
        }

        private void OnDisable()
        {
            prototype.Changed -= Refresh;
            prototype.Companion.Spoke -= ShowDialogue;
            conversation?.Dispose();
        }

        private static Label AddLabel(VisualElement parent, string text, string className)
        {
            var label = new Label(text) { pickingMode = PickingMode.Ignore };
            label.AddToClassList(className);
            parent.Add(label);
            return label;
        }

        private static Button AddButton(VisualElement parent, string text, System.Action action)
        {
            var button = new Button(action) { text = text };
            parent.Add(button);
            return button;
        }

        private void ShowDialogue(string line)
        {
            if (prototype.Companion.Dialogue.Active) { dialogue.text = ""; return; }
            dialogue.text = "COMPAÑERA\n" + line;
            dialogueUntil = Time.time + 9f;
            dialogue.style.display = DisplayStyle.Flex;
        }

        private void Restart()
        {
            dialogue.text = string.Empty;
            dialogueUntil = 0f;
            prototype.Restart();
        }

        private void Refresh()
        {
            var companion = prototype.Companion;
            var state = companion.State;
            menu.style.display = prototype.IsPaused ? DisplayStyle.Flex : DisplayStyle.None;
            incapacitate.SetEnabled(state.LifeState == CompanionLifeState.Active);
            resume.SetEnabled(state.LifeState != CompanionLifeState.Dead);
            resume.text = state.LifeState == CompanionLifeState.Dead ? "Ensayo terminado" : "Continuar en el claro";
            switch (state.LifeState)
            {
                case CompanionLifeState.Trapped:
                    objective.text = "Una desconocida necesita ayuda.\nAcércate a la trampa de los bandidos.";
                    status.text = "Atrapada · sin cuenta de rescate";
                    break;
                case CompanionLifeState.Active:
                    objective.text = "El primer rescate ya es un recuerdo.\nHabla con ella y recorre el claro.";
                    status.text = companion.IsFollowing ? "Te acompaña · confianza inicial" : "Espera aquí · confianza inicial";
                    break;
                case CompanionLifeState.Downed:
                    objective.text = "La compañera está incapacitada.\nAcércate y mantén E para reanimarla.";
                    status.text = "Necesita ayuda";
                    break;
                default:
                    objective.text = "No llegaste a tiempo.\nReinicia el encuentro para repetir el ensayo.";
                    status.text = "Game Over · ensayo de supervivencia";
                    break;
            }
        }

        private void Update()
        {
            if (Time.unscaledTime < nextHudUpdate)
                return;
            nextHudUpdate = Time.unscaledTime + 0.1f;

            var state = prototype.Companion.State;
            var appearance = prototype.Companion.GetComponent<CompanionAppearance>();
            appearanceNotice.text = appearance.RecoveredFallback ? appearance.Status : "";
            timer.text = state.LifeState == CompanionLifeState.Downed ? $"RESCATE  {Mathf.CeilToInt(state.RescueSecondsRemaining):00} s" : "";
            prompt.text = "";
            if (!prototype.IsPaused && !prototype.Companion.Dialogue.Active && prototype.CanInteract())
                prompt.text = state.LifeState == CompanionLifeState.Active
                    ? (prototype.Companion.Dialogue.HasTopics ? "Mantén E · Conversar     F · Sígueme / Espera aquí" : "Sin temas nuevos · F · Sígueme / Espera aquí")
                    : state.LifeState == CompanionLifeState.Trapped ? "Mantén E · Desactivar la trampa"
                    : state.LifeState == CompanionLifeState.Downed ? "Mantén E · Reanimar" : "";

            dialogue.style.display = dialogue.text.Length > 0 && Time.time < dialogueUntil && !prototype.IsPaused && !prototype.Companion.Dialogue.Active
                ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
