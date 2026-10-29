using UnityEngine;

namespace Isekaid.ExperimentalVrm
{
    public static class AvatarLocomotion
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Cadence = Animator.StringToHash("Cadence");
        public static float PreviewSpeed(string name) => name == "Run" ? 3.8f : name == "Walk" ? 1.4f : 0f;
        public static void SetSpeed(Animator animator, float speed, float deltaTime)
        {
            if (animator == null || animator.runtimeAnimatorController == null) return;
            animator.SetFloat(Speed, Mathf.Max(0, speed), .15f, deltaTime);
            animator.SetFloat(Cadence, Mathf.Clamp(animator.GetFloat(Speed) / 1.4f, .05f, 1f));
            ApplyStrideScale(animator);
        }
        public static void Preview(Animator animator, string name)
        {
            if (animator == null || animator.runtimeAnimatorController == null) return;
            foreach (var parameter in animator.parameters)
                if (parameter.nameHash == Speed && parameter.type == AnimatorControllerParameterType.Float)
                { animator.SetFloat(Speed, PreviewSpeed(name)); animator.SetFloat(Cadence, 1f); ApplyStrideScale(animator); return; }
            int state = Animator.StringToHash(name);
            if (!animator.HasState(0, state) || animator.GetCurrentAnimatorStateInfo(0).shortNameHash == state ||
                (animator.IsInTransition(0) && animator.GetNextAnimatorStateInfo(0).shortNameHash == state)) return;
            animator.CrossFadeInFixedTime(state, .15f);
        }
        private static void ApplyStrideScale(Animator animator)
        {
            // The CC0 source stride was measured with a 0.976 m effective Humanoid scale.
            animator.speed = .976f / Mathf.Max(.01f, animator.humanScale * animator.transform.lossyScale.y);
        }
    }
}
