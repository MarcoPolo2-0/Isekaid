using UnityEngine;
using UnityEngine.AI;

namespace Isekaid.ExperimentalVrm
{
    public sealed class VrmCompanionAdapter : MonoBehaviour
    {
        public CompanionController Behaviour;
        public VrmAppearance Appearance;
        private NavMeshAgent agent;
        private string motion;
        private void Awake() { if (Behaviour != null) agent = Behaviour.GetComponent<NavMeshAgent>(); }
        private void OnEnable() { if (Behaviour != null) Behaviour.Changed += Refresh; }
        private void OnDisable() { if (Behaviour != null) Behaviour.Changed -= Refresh; }
        private void Update()
        {
            if (Appearance == null || Appearance.PreviewingMotion) { motion = null; return; }
            FollowAgentMotion();
        }
        private void Refresh()
        {
            ICompanionExpressions expressions = Appearance;
            expressions?.RequestExpression(Behaviour.State.LifeState == CompanionLifeState.Active ? "relaxed" : "sad", 1);
        }
        public void FollowAgentMotion()
        {
            if (agent != null && Appearance != null)
            {
                string next = agent.velocity.magnitude > 3f ? "Run" : agent.velocity.magnitude > .1f ? "Walk" : "Idle";
                if (motion != next) { motion = next; Appearance.SetMotion(next); }
            }
        }
    }
}
