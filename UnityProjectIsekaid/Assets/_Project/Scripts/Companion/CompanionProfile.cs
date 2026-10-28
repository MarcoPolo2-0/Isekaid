using System;

namespace Isekaid
{
    [Serializable]
    public sealed class CompanionProfile
    {
        public int version = 1;
        public CompanionPersonality personality = CompanionPersonality.Shy;
        public int hair;
        public int eyes;
        public int top;
        public int trousers;

        public bool IsValid => version == 1 && (int)personality >= 0 && (int)personality < 3 &&
            hair >= 0 && hair < 4 && eyes >= 0 && eyes < 4 && top >= 0 && top < 4 && trousers >= 0 && trousers < 4;

        public int GetColorIndex(int part)
        {
            switch (part)
            {
                case 0: return hair;
                case 1: return eyes;
                case 2: return top;
                case 3: return trousers;
                default: throw new ArgumentOutOfRangeException(nameof(part));
            }
        }

        public void SetColorIndex(int part, int index)
        {
            if (index < 0 || index >= 4)
                throw new ArgumentOutOfRangeException(nameof(index));
            switch (part)
            {
                case 0: hair = index; break;
                case 1: eyes = index; break;
                case 2: top = index; break;
                case 3: trousers = index; break;
                default: throw new ArgumentOutOfRangeException(nameof(part));
            }
        }
    }
}
