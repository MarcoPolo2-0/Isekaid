using System;

namespace Isekaid
{
    public enum CompanionPersonality { Tsundere, Yandere, Shy }
    public enum CompanionLifeState { Trapped, Active, Downed, Dead }

    [Serializable]
    public sealed class CompanionState
    {
        public const float RescueWindowSeconds = 30f;

        public CompanionPersonality Personality { get; set; }
        public CompanionLifeState LifeState { get; private set; } = CompanionLifeState.Trapped;
        public bool RemembersFirstRescue { get; private set; }
        public int Trust { get; private set; }
        public float RescueSecondsRemaining { get; private set; }

        public bool Rescue()
        {
            if (LifeState != CompanionLifeState.Trapped && LifeState != CompanionLifeState.Downed)
                return false;

            if (LifeState == CompanionLifeState.Trapped && !RemembersFirstRescue)
            {
                RemembersFirstRescue = true;
                Trust = 1;
            }

            LifeState = CompanionLifeState.Active;
            RescueSecondsRemaining = 0f;
            return true;
        }

        public bool Incapacitate()
        {
            if (LifeState != CompanionLifeState.Active)
                return false;

            LifeState = CompanionLifeState.Downed;
            RescueSecondsRemaining = RescueWindowSeconds;
            return true;
        }

        public void Tick(float activeSeconds)
        {
            if (LifeState != CompanionLifeState.Downed || activeSeconds <= 0f ||
                float.IsNaN(activeSeconds) || float.IsInfinity(activeSeconds))
                return;

            RescueSecondsRemaining = Math.Max(0f, RescueSecondsRemaining - activeSeconds);
            if (RescueSecondsRemaining == 0f)
                LifeState = CompanionLifeState.Dead;
        }

        public string RescueLine(bool fromTrap)
        {
            if (!fromTrap)
                return "Gracias por volver. Necesito recuperar el aliento antes de continuar.";

            switch (Personality)
            {
                case CompanionPersonality.Tsundere:
                    return "¡Ya estaba buscando cómo salir! ...Pero gracias. ¿Tú también vas hacia la aldea?";
                case CompanionPersonality.Yandere:
                    return "Te detuviste a ayudarme... Gracias. No conozco este camino. Podemos llegar juntos a la aldea.";
                default:
                    return "Gracias... Creí que nadie iba a escucharme. Si también vas a la aldea, ¿podemos acompañarnos?";
            }
        }

        public string TalkLine(int index)
        {
            if (LifeState != CompanionLifeState.Active)
                return string.Empty;

            switch (Personality)
            {
                case CompanionPersonality.Tsundere:
                    return index % 2 == 0
                        ? "No pienso volver a caer en una de esas trampas. Vigila el camino; yo miraré entre los árboles."
                        : "Que me hayas ayudado no significa que ya te conozca. Pero podemos cuidarnos hasta la aldea.";
                case CompanionPersonality.Yandere:
                    return index % 2 == 0
                        ? "Recuerdo que escuchaste mi voz y te acercaste. Quiero saber qué te trajo a este lugar."
                        : "Prefiero que sigamos cerca mientras haya trampas. Después decidiré por dónde continuar.";
                default:
                    return index % 2 == 0
                        ? "Todavía me asusta ese ruido entre los árboles... Voy a estar atenta."
                        : "No suelo hablar mucho con desconocidos. Pero recuerdo que me ayudaste. Gracias por escuchar.";
            }
        }
    }
}
