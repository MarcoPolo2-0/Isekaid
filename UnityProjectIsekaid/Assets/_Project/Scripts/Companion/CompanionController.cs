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
        private int conversationIndex;

        public CompanionState State { get; private set; } = new CompanionState();
        public bool IsFollowing { get; private set; }
        public event Action Changed;
        public event Action<string> Spoke;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            var profile = CompanionProfileStore.Load();
            State.Personality = profile.personality;
            GetComponent<CompanionAppearance>().Apply(profile);
        }

        private void Update()
        {
            var previousState = State.LifeState;
            State.Tick(Time.deltaTime);
            if (State.LifeState != previousState)
                Changed?.Invoke();

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
                Spoke?.Invoke(State.RescueLine(fromTrap));
                Changed?.Invoke();
                return true;
            }

            if (State.LifeState != CompanionLifeState.Active)
                return false;

            Spoke?.Invoke(State.TalkLine(conversationIndex++));
            return true;
        }

        public void SetFollowing(bool follow)
        {
            if (State.LifeState != CompanionLifeState.Active)
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
            Spoke?.Invoke(follow ? "Vamos. Mantengamos los ojos abiertos." : "Te espero aquí. Avísame cuando continuemos.");
            Changed?.Invoke();
        }

        public void SetPersonality(CompanionPersonality value)
        {
            State.Personality = value;
            conversationIndex = 0;
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
            Spoke?.Invoke("¡Necesito ayuda! Acércate para reanimarme.");
            Changed?.Invoke();
            return true;
        }

        public void ResetScenario()
        {
            var selectedPersonality = State.Personality;
            State = new CompanionState { Personality = selectedPersonality };
            IsFollowing = false;
            conversationIndex = 0;
            agent.ResetPath();
            agent.Warp(initialPosition);
            transform.rotation = initialRotation;
            visual.localRotation = Quaternion.identity;
            trap.SetActive(true);
            Changed?.Invoke();
        }
    }
}
