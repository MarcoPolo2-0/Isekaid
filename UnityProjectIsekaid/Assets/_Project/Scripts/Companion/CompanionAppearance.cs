using UnityEngine;

namespace Isekaid
{
    public sealed class CompanionAppearance : MonoBehaviour
    {
        [SerializeField] private Renderer[] hair;
        [SerializeField] private Renderer[] eyes;
        [SerializeField] private Renderer[] top;
        [SerializeField] private Renderer[] trousers;

        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        private MaterialPropertyBlock properties;

        public void Apply(CompanionProfile profile)
        {
            if (profile == null || !profile.IsValid)
                return;
            for (int part = 0; part < CompanionPalette.Colors.Length; part++)
                SetColor(part, CompanionPalette.Colors[part][profile.GetColorIndex(part)]);
        }

        public void SetColor(int part, Color color)
        {
            Renderer[] targets;
            switch (part)
            {
                case 0: targets = hair; break;
                case 1: targets = eyes; break;
                case 2: targets = top; break;
                case 3: targets = trousers; break;
                default: return;
            }

            if (properties == null)
                properties = new MaterialPropertyBlock();

            foreach (var target in targets)
            {
                if (target == null)
                    continue;
                target.GetPropertyBlock(properties);
                properties.SetColor(BaseColor, color);
                target.SetPropertyBlock(properties);
            }
        }
    }
}
