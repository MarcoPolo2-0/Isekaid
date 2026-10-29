using System;
using UnityEngine;
using UnityEngine.AI;

namespace Isekaid
{
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class CompanionController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private Transform visual;
        [SerializeField] private GameObject trap;

        private NavMeshAgent agent;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private float nextPathTime;
        private DialogueDirector dialogue;
        private CompanionReactionPresenter reactions;
        private CompanionAppearance appearance;
        private object reactionAvatar;
        private DialogueEntry reactingTo;
        private bool dialogueVisible;

        public CompanionState State { get; private set; } = new CompanionState();
        public bool IsFollowing { get; private set; }
        public DialogueDirector Dialogue => dialogue ??= new DialogueDirector(() => State, () => IsFollowing);
        public CompanionReactionPresenter Reactions => reactions;
        public event Action Changed;
        public event Action<string> Spoke;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            var profile = CompanionProfileStore.Load();
            State.Personality = profile.personality;
            appearance = GetComponent<CompanionAppearance>();
            appearance.Apply(profile);
            reactions = new CompanionReactionPresenter(appearance.PreviewExpression);
            Dialogue.Changed += OnDialogueChanged;
            Dialogue.Presented += Speak;
        }

        private void Update()
        {
            var previousState = State.LifeState;
            State.Tick(Time.deltaTime);
            Dialogue.Tick(Time.deltaTime);
            if (appearance.Ready)
            {
                object currentAvatar = appearance.Importer.HasAvatar ? (object)appearance.Importer.LoadedBytes : appearance.Catalog;
                if (!ReferenceEquals(reactionAvatar, currentAvatar)) { reactionAvatar = currentAvatar; reactions.Reapply(); }
                reactions.Tick(Time.deltaTime);
            }
            if (State.LifeState != previousState)
            {
                reactions.Clear();
                Changed?.Invoke();
            }

            if (Time.timeScale == 0f || !agent.isOnNavMesh)
                return;

            bool shouldMove = State.LifeState == CompanionLifeState.Active && IsFollowing;
            agent.isStopped = !shouldMove;
            if (shouldMove && Time.time >= nextPathTime)
            {
                nextPathTime = Time.time + 0.25f;
                // Only accept reachable destinations; never warp toward a lost target.
                if (NavMesh.SamplePosition(followTarget.position, out var hit, 2f, agent.areaMask))
                    agent.SetDestination(hit.position);
            }
        }

        public bool Interact()
        {
            bool fromTrap = State.LifeState == CompanionLifeState.Trapped;
            if (State.Rescue())
            {
                IsFollowing = true;
                trap.SetActive(false);
                visual.localRotation = Quaternion.identity;
                if (fromTrap) Dialogue.Rescued(); else Dialogue.Revived();
                Changed?.Invoke();
                return true;
            }

            if (State.LifeState != CompanionLifeState.Active)
                return false;

            return Dialogue.Begin();
        }

        public void SetFollowing(bool follow)
        {
            if (State.LifeState != CompanionLifeState.Active || IsFollowing == follow)
                return;

            IsFollowing = follow;
            nextPathTime = 0f;
            if (agent.isOnNavMesh)
            {
                agent.isStopped = !follow;
                if (!follow)
                {
                    agent.ResetPath();
                    agent.velocity = Vector3.zero;
                }
            }
            Dialogue.FollowingChanged();
            Changed?.Invoke();
        }

        public void SetPersonality(CompanionPersonality value)
        {
            if (State.Personality == value) return;
            State.Personality = value;
            Dialogue.RebindPersonality();
            Changed?.Invoke();
        }

        public bool Incapacitate()
        {
            if (!State.Incapacitate())
                return false;

            IsFollowing = false;
            if (agent.isOnNavMesh)
            {
                agent.ResetPath();
                agent.isStopped = true;
            }
            visual.localRotation = Quaternion.Euler(0f, 0f, 65f);
            Dialogue.Downed();
            Changed?.Invoke();
            return true;
        }

        public void ResetScenario()
        {
            var selectedPersonality = State.Personality;
            State = new CompanionState { Personality = selectedPersonality };
            IsFollowing = false;
            Dialogue.ResetEncounter();
            reactions.Clear();
            agent.ResetPath();
            agent.Warp(initialPosition);
            transform.rotation = initialRotation;
            visual.localRotation = Quaternion.identity;
            trap.SetActive(true);
            Changed?.Invoke();
        }

        private void Speak(DialogueEntry entry)
        {
            reactions.Show(entry.Emotion, Dialogue.Active && ReferenceEquals(Dialogue.Current, entry) ? -1f : 6f,
                entry.preferredExpression, entry.fallbackExpression);
            Spoke?.Invoke(entry.Text(State.Personality));
        }

        private void OnDialogueChanged()
        {
            bool visible = Dialogue.Active && !Dialogue.Suspended && Dialogue.Current != null;
            if (visible && (!dialogueVisible || !ReferenceEquals(reactingTo, Dialogue.Current)))
                reactions.Show(Dialogue.Current.Emotion, -1f, Dialogue.Current.preferredExpression, Dialogue.Current.fallbackExpression);
            else if (!visible && dialogueVisible) reactions.Clear();
            dialogueVisible = visible;
            reactingTo = Dialogue.Current;
        }

        private void OnDisable()
        {
            dialogue?.Close();
            if (appearance != null && appearance.Ready) reactions?.Clear();
        }

        private void OnDestroy()
        {
            if (dialogue != null) { dialogue.Changed -= OnDialogueChanged; dialogue.Presented -= Speak; }
        }
    }
}
