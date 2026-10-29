using System;
using System.Collections.Generic;

namespace Isekaid
{
    public sealed class DialogueDirector
    {
        private readonly Func<CompanionState> state;
        private readonly Func<bool> following;
        private readonly HashSet<string> consumed = new HashSet<string>(StringComparer.Ordinal);
        private readonly List<DialogueOption> choices = new List<DialogueOption>();
        private bool afterRevival;

        public DialogueDirector(Func<CompanionState> state, Func<bool> following)
        { this.state = state; this.following = following; }

        public DialogueEntry Current { get; private set; }
        public IReadOnlyList<DialogueOption> Choices => choices;
        public bool Active => Current != null;
        public bool Suspended { get; private set; }
        public float ActiveSeconds { get; private set; }
        public int ConsumedCount => consumed.Count;
        public event Action Changed;

        private string NextTopic()
        {
            if (state().LifeState != CompanionLifeState.Active || !state().RemembersFirstRescue) return null;
            if (afterRevival && !consumed.Contains("talk.recovered")) return "talk.recovered";
            if (!consumed.Contains("talk.first")) return "talk.first";
            string command = following() ? "talk.following" : "talk.waiting";
            if (!consumed.Contains(command)) return command;
            return !consumed.Contains("talk.again") ? "talk.again" : null;
        }

        public bool HasTopics => NextTopic() != null;
        public bool Begin()
        {
            if (Active || Suspended) return false;
            string id = NextTopic();
            if (id == null) return false;
            Present(id);
            return true;
        }

        private void Present(string id)
        {
            Current = DialogueContent.Current.Get(id);
            consumed.Add(id);
            choices.Clear();
            if (Current.options != null)
                foreach (var option in Current.options)
                    if (!consumed.Contains(option.target)) choices.Add(option);
            Changed?.Invoke();
        }

        public bool Choose(string optionId)
        {
            if (!Active || Suspended || state().LifeState != CompanionLifeState.Active) return false;
            foreach (var option in choices)
                if (option.id == optionId) { Present(option.target); return true; }
            return false;
        }

        public void Continue()
        {
            if (Active && !Suspended && choices.Count == 0) Close();
        }

        public void Close()
        {
            if (!Active) return;
            Current = null;
            choices.Clear();
            Changed?.Invoke();
        }

        public void Interrupt(DialoguePriority priority)
        {
            if (priority >= DialoguePriority.Important) Close();
        }

        public void SetSuspended(bool value)
        {
            if (Suspended == value) return;
            Suspended = value;
            Changed?.Invoke();
        }

        public void Revived() { afterRevival = true; Interrupt(DialoguePriority.Survival); }
        public void Tick(float seconds)
        {
            if (state().LifeState != CompanionLifeState.Active) Interrupt(DialoguePriority.Survival);
            if (Active && !Suspended && seconds > 0 && !float.IsNaN(seconds) && !float.IsInfinity(seconds)) ActiveSeconds += seconds;
        }

        public void ResetEncounter()
        {
            Close();
            consumed.Clear();
            afterRevival = false;
            ActiveSeconds = 0;
        }
    }
}
