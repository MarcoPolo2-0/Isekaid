using System;
using System.Collections.Generic;

namespace Isekaid
{
    public sealed class DialogueDirector
    {
        private readonly Func<CompanionState> state;
        private readonly Func<bool> following;
        private readonly HashSet<string> consumed = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> completedTopics = new HashSet<string>(StringComparer.Ordinal);
        private readonly List<DialogueOption> choices = new List<DialogueOption>();
        private DialogueEntry resumeCursor;
        private string resumeFocus;
        private bool open, recoveryPending, resumeAfterRecovery, downed, rescued;
        private int episode, order, waits;
        private float nextCasual;

        public DialogueDirector(Func<CompanionState> state, Func<bool> following)
        { this.state = state; this.following = following; }

        public DialogueEntry Current { get; private set; }
        public IReadOnlyList<DialogueOption> Choices => choices;
        public bool Active => open;
        public bool Suspended { get; private set; }
        public float ActiveSeconds { get; private set; }
        public int ConsumedCount => consumed.Count;
        public int RecoveryEpisode => episode;
        public int Revision { get; private set; }
        public string FocusedOptionId { get; set; }
        public bool ShowingHistory => Current?.eventType == "TopicSelected" && consumed.Contains(Key(Current));
        public bool HasTopics => Eligible && (Current != null || completedTopics.Count < DialogueContent.Current.topics.Length || CanChat || recoveryPending);
        public bool CanChat => Eligible && Current == null && ActiveSeconds >= nextCasual && NextCasual() != null;
        private bool Eligible => state().LifeState == CompanionLifeState.Active && state().RemembersFirstRescue;
        public event Action Changed;
        public event Action<DialogueEntry> Presented;

        private void Notify() { Revision++; Changed?.Invoke(); }
        private string Key(DialogueEntry entry) => entry.id + (entry.scope == "encounter" ? "" :
            entry.scope == "event_instance" ? "@order:" + order : "@episode:" + episode);
        private void Emit(DialogueEntry entry)
        {
            if (consumed.Add(Key(entry))) Presented?.Invoke(entry);
        }

        private DialogueEntry NextCasual()
        {
            foreach (var entry in DialogueContent.Current.entries)
                if (entry.eventType == "TalkRequested" && entry.personality == (int)state().Personality && !consumed.Contains(Key(entry))) return entry;
            return null;
        }

        private void RefreshChoices()
        {
            choices.Clear();
            if (Current != null)
            {
                if (Current.options != null) choices.AddRange(Current.options);
                return;
            }
            foreach (var topic in DialogueContent.Current.topics)
            {
                var root = DialogueContent.Current.Get(topic.roots[(int)state().Personality]);
                if (!completedTopics.Contains(root.topic)) choices.Add(new DialogueOption { id = topic.id, text = topic.text, target = root.id });
            }
        }

        public bool Begin()
        {
            if (Active || Suspended || !Eligible) return false;
            open = true;
            if (recoveryPending && resumeAfterRecovery && Current != null)
            {
                resumeCursor = Current;
                resumeFocus = FocusedOptionId;
                recoveryPending = resumeAfterRecovery = false;
                Present(DialogueContent.Current.Reaction("ResumeConversationRequested", state().Personality));
            }
            else if (recoveryPending && Current == null)
            {
                recoveryPending = false;
                Present(DialogueContent.Current.Reaction("SafeReturnTalkRequested", state().Personality));
            }
            else { recoveryPending = resumeAfterRecovery = false; RefreshChoices(); Notify(); }
            return true;
        }

        private void Present(DialogueEntry entry)
        {
            Current = entry;
            FocusedOptionId = null;
            if (entry.eventType == "PlayerOptionSelected") completedTopics.Add(entry.topic);
            RefreshChoices();
            Notify();
            Emit(entry);
        }

        public bool Choose(string optionId, int expectedRevision = -1)
        {
            if (!Active || Suspended || !Eligible || expectedRevision >= 0 && expectedRevision != Revision) return false;
            foreach (var option in choices)
                if (option.id == optionId) { Present(DialogueContent.Current.Get(option.target)); return true; }
            return false;
        }

        public bool ChatCasually()
        {
            if (!Active || Suspended || !CanChat) return false;
            var entry = NextCasual();
            nextCasual = ActiveSeconds + entry.cooldown;
            Present(entry);
            return true;
        }

        public void Continue()
        {
            if (!Active || Suspended || !Eligible || choices.Count != 0) return;
            Current = resumeCursor;
            if (resumeCursor != null) FocusedOptionId = resumeFocus;
            resumeCursor = null;
            RefreshChoices();
            Notify();
        }

        public void Close()
        {
            if (!Active) return;
            if (resumeCursor != null) { Current = resumeCursor; resumeCursor = null; FocusedOptionId = resumeFocus; }
            else if (Current?.options == null || Current.options.Length == 0) Current = null;
            open = resumeAfterRecovery = false;
            RefreshChoices();
            Notify();
        }

        public void Interrupt(DialoguePriority priority)
        {
            if (priority < DialoguePriority.Important) return;
            if (priority == DialoguePriority.Survival)
            {
                if (resumeCursor != null) { Current = resumeCursor; resumeCursor = null; FocusedOptionId = resumeFocus; }
                resumeAfterRecovery |= Active && Current != null;
                open = false;
                Notify();
            }
            else Close();
        }

        public void SetSuspended(bool value)
        {
            if (Suspended == value) return;
            Suspended = value;
            Notify();
        }

        public void Rescued()
        {
            if (rescued || !Eligible) return;
            rescued = true;
            Emit(DialogueContent.Current.Reaction("FirstRescueCompleted", state().Personality));
        }
        public void FollowingChanged()
        {
            Close(); order++;
            string variant = following() ? "" : ++waits % 2 == 1 ? "plain" : "opinion";
            Emit(DialogueContent.Current.Reaction("FollowingChanged", state().Personality, following(), variant));
        }
        public void Downed()
        {
            if (downed || state().LifeState != CompanionLifeState.Downed) return;
            downed = true; episode++;
            Interrupt(DialoguePriority.Survival);
            Emit(DialogueContent.Current.Reaction("DownedEntered", state().Personality));
        }
        public void Revived()
        {
            if (!downed || !Eligible) return;
            downed = false; recoveryPending = true;
            Emit(DialogueContent.Current.Reaction("ReanimationCompleted", state().Personality));
        }

        public void RebindPersonality()
        {
            Current = DialogueContent.Current.Rebind(Current, state().Personality);
            resumeCursor = DialogueContent.Current.Rebind(resumeCursor, state().Personality);
            if (Current != null) consumed.Add(Key(Current));
            RefreshChoices();
            Notify();
        }

        public void Tick(float seconds)
        {
            if (state().LifeState == CompanionLifeState.Dead)
            {
                if (open || Current != null || resumeCursor != null)
                { open = false; Current = resumeCursor = null; choices.Clear(); Notify(); }
                return;
            }
            if (state().LifeState == CompanionLifeState.Downed && open) Interrupt(DialoguePriority.Survival);
            if (Eligible && !Suspended && seconds > 0 && !float.IsNaN(seconds) && !float.IsInfinity(seconds))
            {
                bool enableCasual = open && Current == null && ActiveSeconds < nextCasual && ActiveSeconds + seconds >= nextCasual;
                ActiveSeconds += seconds;
                if (enableCasual) Notify();
            }
        }

        public void ResetEncounter()
        {
            Current = resumeCursor = null;
            open = recoveryPending = resumeAfterRecovery = downed = rescued = false;
            consumed.Clear(); completedTopics.Clear(); choices.Clear();
            episode = order = waits = 0;
            ActiveSeconds = nextCasual = 0;
            FocusedOptionId = null;
            resumeFocus = null;
            Notify();
        }
    }
}
