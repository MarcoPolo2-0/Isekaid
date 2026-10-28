using System;
using UnityEngine;

namespace Isekaid
{
    public static class CompanionProfileStore
    {
        public const string PreferenceKey = "isekaid.companion.profile.v1";

        public static CompanionProfile Load()
        {
            if (!PlayerPrefs.HasKey(PreferenceKey))
                return new CompanionProfile();
            try
            {
                var profile = JsonUtility.FromJson<CompanionProfile>(PlayerPrefs.GetString(PreferenceKey));
                if (profile != null && profile.IsValid)
                    return profile;
            }
            catch (ArgumentException) { }
            return new CompanionProfile();
        }

        public static bool TrySave(CompanionProfile profile, out string error)
        {
            if (profile == null || !profile.IsValid)
            {
                error = "Revisa las opciones del personaje antes de continuar.";
                return false;
            }
            try
            {
                PlayerPrefs.SetString(PreferenceKey, JsonUtility.ToJson(profile));
                PlayerPrefs.Save();
                error = string.Empty;
                return true;
            }
            catch (PlayerPrefsException)
            {
                error = "No se pudo guardar el personaje. Inténtalo de nuevo.";
                return false;
            }
        }
    }
}
