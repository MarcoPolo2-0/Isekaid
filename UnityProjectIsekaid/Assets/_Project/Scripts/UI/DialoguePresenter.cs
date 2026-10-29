using System;
using UnityEngine.UIElements;

namespace Isekaid
{
    public sealed class DialoguePresenter : IDisposable
    {
        private readonly DialogueDirector director;
        private readonly Func<CompanionPersonality> personality;
        private readonly VisualElement panel, options;
        private readonly Label text;
        private readonly Button advance, casual, close;
        public DialoguePresenter(VisualElement root, DialogueDirector director, Func<CompanionPersonality> personality)
        {
            this.director = director;
            this.personality = personality;
            panel = new VisualElement { name = "conversation" };
            root.Add(panel);
            panel.Add(new Label("CONVERSACIÓN ACTIVA") { name = "conversation-speaker" });
            text = new Label { name = "conversation-text" };
            panel.Add(text);
            options = new VisualElement { name = "conversation-options" };
            panel.Add(options);
            advance = new Button(director.Continue) { name = "conversation-continue", text = "Continuar · Intro" };
            panel.Add(advance);
            casual = new Button(() => director.ChatCasually()) { name = "conversation-casual", text = "Charlar un momento" };
            panel.Add(casual);
            close = new Button(director.Close) { name = "conversation-close", text = "Terminar conversación · Esc" };
            panel.Add(close);
            var note = new Label("Biblioteca F3A en revisión · 1–3 Elegir · Tab Pausa");
            note.AddToClassList("hint"); panel.Add(note);
            director.Changed += Refresh;
            Refresh();
        }

        private void Refresh()
        {
            panel.style.display = director.Active && !director.Suspended ? DisplayStyle.Flex : DisplayStyle.None;
            panel.Q<Label>("conversation-speaker").text = director.Current == null ? "CONVERSACIÓN · TEMAS" :
                director.ShowingHistory ? "COMPAÑERA · OPCIONES PENDIENTES" : "COMPAÑERA · CONVERSACIÓN ACTIVA";
            text.text = director.Current?.Text(personality()) ?? (director.Choices.Count > 0 ? "Elige un tema o charlemos un momento." : "No hay temas nuevos por ahora");
            options.Clear();
            int revision = director.Revision;
            Button focused = null;
            for (int index = 0; index < director.Choices.Count; index++)
            {
                var option = director.Choices[index];
                var button = new Button(() => director.Choose(option.id, revision)) { name = option.id, text = (index + 1) + " · " + option.text };
                button.RegisterCallback<FocusInEvent>(_ => director.FocusedOptionId = option.id);
                options.Add(button);
                if (option.id == director.FocusedOptionId) focused = button;
            }
            advance.style.display = director.Current != null && director.Choices.Count == 0 ? DisplayStyle.Flex : DisplayStyle.None;
            advance.text = director.Current?.eventType == "ResumeConversationRequested" ? "Retomar diálogo · Intro" : "Volver a temas · Intro";
            casual.style.display = director.Current == null ? DisplayStyle.Flex : DisplayStyle.None;
            casual.SetEnabled(director.CanChat);
            if (director.Active && !director.Suspended)
            {
                if (options.childCount > 0) (focused != null ? focused : options[0]).Focus();
                else if (director.Current != null) advance.Focus();
                else if (director.CanChat) casual.Focus();
                else close.Focus();
            }
        }

        public void Dispose()
        {
            director.Changed -= Refresh;
            director.Close();
            panel.RemoveFromHierarchy();
        }
    }
}
