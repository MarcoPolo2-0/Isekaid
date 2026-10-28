using UnityEngine;

namespace Isekaid
{
    public static class CompanionPalette
    {
        public static readonly string[] PersonalityNames = { "Tsundere", "Yandere", "Tímida" };
        public static readonly string[] Parts = { "CABELLO", "OJOS", "TOP", "PANTALÓN" };
        public static readonly string[][] Names =
        {
            new[] { "Castaño", "Negro", "Plata", "Cobre" },
            new[] { "Verde", "Azul", "Ámbar", "Violeta" },
            new[] { "Marfil", "Bosque", "Vino", "Azul" },
            new[] { "Pizarra", "Tierra", "Oliva", "Noche" }
        };
        public static readonly Color[][] Colors =
        {
            new[] { new Color(0.19f, 0.10f, 0.08f), new Color(0.055f, 0.065f, 0.09f), new Color(0.72f, 0.76f, 0.82f), new Color(0.65f, 0.23f, 0.10f) },
            new[] { new Color(0.13f, 0.55f, 0.31f), new Color(0.13f, 0.43f, 0.85f), new Color(0.86f, 0.49f, 0.08f), new Color(0.52f, 0.22f, 0.78f) },
            new[] { new Color(0.89f, 0.83f, 0.67f), new Color(0.16f, 0.35f, 0.26f), new Color(0.47f, 0.12f, 0.20f), new Color(0.18f, 0.32f, 0.55f) },
            new[] { new Color(0.22f, 0.27f, 0.32f), new Color(0.38f, 0.25f, 0.15f), new Color(0.31f, 0.34f, 0.19f), new Color(0.07f, 0.10f, 0.15f) }
        };
    }
}
