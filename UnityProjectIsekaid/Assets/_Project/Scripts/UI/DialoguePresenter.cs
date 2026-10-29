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
        private readonly Button advance;
        public DialoguePresenter(VisualElement root, DialogueDirector director, Func<CompanionPersonality> personality)
        {
            this.director = director;
            this.personality = personality;
            panel = new VisualElement { name = "conversation" };
            root.Add(panel);
            panel.Add(new Label("COMPAÑERA · CONVERSACIÓN ACTIVA") { name = "conversation-speaker" });
            text = new Label { name = "conversation-text" };
            panel.Add(text);
            options = new VisualElement { name = "conversation-options" };
            panel.Add(options);
            advance = new Button(director.Continue) { name = "conversation-continue", text = "Continuar · Intro" };
            panel.Add(advance);
            panel.Add(new Button(director.Close) { name = "conversation-close", text = "Cerrar / saltar · Esc" });
            var note = new Label("Contenido provisional F3A · 1–3 Elegir · Tab Pausa");
            note.AddToClassList("hint"); panel.Add(note);
            director.Changed += Refresh;
            Refresh();
        }

        private void Refresh()
        {
            panel.style.display = director.Active && !director.Suspended ? DisplayStyle.Flex : DisplayStyle.None;
            text.text = director.Current?.Text(personality()) ?? "";
            options.Clear();
            for (int index = 0; index < director.Choices.Count; index++)
            {
                var option = director.Choices[index];
                options.Add(new Button(() => director.Choose(option.id)) { name = option.id, text = (index + 1) + " · " + option.text });
            }
            advance.style.display = director.Choices.Count == 0 ? DisplayStyle.Flex : DisplayStyle.None;
            if (director.Active && !director.Suspended)
            {
                if (options.childCount > 0) options[0].Focus();
                else advance.Focus();
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
