using System;

namespace Isekaid
{
    public sealed class CompanionReactionPresenter
    {
        private readonly Func<string, bool> request;
        private float remaining;
        public CompanionEmotion Emotion { get; private set; }
        public string AppliedExpression { get; private set; } = "neutral";
        public bool UsedFallback { get; private set; }

        public CompanionReactionPresenter(Func<string, bool> request) { this.request = request; }
        public void Show(CompanionEmotion emotion, float seconds = -1f)
        {
            Emotion = emotion;
            remaining = seconds;
            Reapply();
        }

        public void Reapply()
        {
            string preferred = Emotion switch
            {
                CompanionEmotion.Joy => "happy",
                CompanionEmotion.Sadness => "sad",
                CompanionEmotion.Concern => "sad",
                CompanionEmotion.Anger => "angry",
                CompanionEmotion.Surprise => "surprised",
                CompanionEmotion.Relief => "relaxed",
                _ => "neutral"
            };
            UsedFallback = false;
            AppliedExpression = preferred;
            if (request(preferred)) return;
            UsedFallback = true;
            if (Emotion == CompanionEmotion.Relief && request("happy")) { AppliedExpression = "happy"; return; }
            if (Emotion == CompanionEmotion.Anger && request("sad")) { AppliedExpression = "sad"; return; }
            AppliedExpression = "neutral";
            request("neutral");
        }

        public void Tick(float seconds)
        {
            if (remaining <= 0 || seconds <= 0 || float.IsNaN(seconds) || float.IsInfinity(seconds)) return;
            remaining -= seconds;
            if (remaining <= 0) Clear();
        }
        public void Clear() => Show(CompanionEmotion.Neutral);
    }
}
