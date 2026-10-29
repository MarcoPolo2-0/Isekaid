using System;
using System.IO;
using System.Text;
using Isekaid.ExperimentalVrm;
using UnityEngine;

namespace Isekaid
{
    public static class CompanionProfileStore
    {
        public const string PreferenceKey = "isekaid.companion.profile.v1";
        public const int MaximumProfileBytes = 1024 * 1024;
        public static string SavePath => Path.Combine(AvatarFiles.Root, "profile-v2.json");
        public static string LastWarning { get; private set; } = "";
#if UNITY_EDITOR || DEBUG
        public static Action BeforeCommit;
#endif
        [Serializable]
        public sealed class Record
        {
            public int formatVersion = 2;
            public CompanionProfile profile = new CompanionProfile();
            public AvatarSelection appearance = new AvatarSelection();
            public string legacyProfileJson = "";
        }
        [Serializable]
        private sealed class Envelope { public string payload; public string sha256; }

        public static CompanionProfile Load() => LoadRecord().profile;

        public static Record LoadRecord()
        {
            LastWarning = "";
            if (TryRead(SavePath, out var record)) return record;
            if (TryRead(SavePath + ".bak", out record))
            {
                LastWarning = "Se recuperó el respaldo del perfil. El archivo anterior se conserva para diagnóstico local.";
                return record;
            }
            if (File.Exists(SavePath) || File.Exists(SavePath + ".bak"))
                LastWarning = "No se puede leer el perfil. Se conserva en disco; selecciona explícitamente una apariencia para guardar una recuperación.";
            var legacy = PlayerPrefs.GetString(PreferenceKey, "");
            if (!string.IsNullOrEmpty(legacy))
            {
                try
                {
                    var profile = JsonUtility.FromJson<CompanionProfile>(legacy);
                    if (profile != null && profile.IsValid)
                    {
                        profile.version = 2;
                        if (string.IsNullOrEmpty(profile.identity)) profile.identity = Guid.NewGuid().ToString("N");
                        return new Record { profile = profile, legacyProfileJson = legacy };
                    }
                }
                catch (ArgumentException) { }
                LastWarning = "El perfil anterior no es compatible. Su contenido original se conserva; revisa la recuperación antes de guardar.";
            }
            return new Record { profile = new CompanionProfile { identity = Guid.NewGuid().ToString("N") }, legacyProfileJson = legacy };
        }

        private static bool TryRead(string path, out Record record, string expectedPayload = null)
        {
            record = null;
            try
            {
                if (!File.Exists(path) || new FileInfo(path).Length > MaximumProfileBytes) return false;
                var envelope = JsonUtility.FromJson<Envelope>(File.ReadAllText(path));
                if (envelope == null || envelope.payload == null || AvatarFiles.Hash(Encoding.UTF8.GetBytes(envelope.payload)) != envelope.sha256) return false;
                if (expectedPayload != null && envelope.payload != expectedPayload) return false;
                var candidate = JsonUtility.FromJson<Record>(envelope.payload);
                if (candidate == null || candidate.formatVersion != 2 || candidate.profile == null || !candidate.profile.IsValid ||
                    string.IsNullOrEmpty(candidate.profile.identity) || candidate.appearance == null || !candidate.appearance.IsValid) return false;
                record = candidate;
                return true;
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException || error is ArgumentException) { return false; }
        }

        public static bool TrySave(CompanionProfile profile, out string error)
        {
            var record = LoadRecord();
            record.profile = profile;
            return TrySave(record, out error);
        }

        public static bool TrySave(Record record, out string error, bool recoveryConfirmed = false)
        {
            error = "";
            if (record?.profile == null || !record.profile.IsValid || record.appearance == null || !record.appearance.IsValid)
            { error = "Revisa las opciones del personaje antes de continuar."; return false; }
            try
            {
                bool validMain = TryRead(SavePath, out _);
                bool validBackup = TryRead(SavePath + ".bak", out _);
                if (!validMain && !validBackup && !recoveryConfirmed &&
                    (File.Exists(SavePath) || File.Exists(SavePath + ".bak")))
                    throw new InvalidDataException("Confirma una apariencia para recuperar el perfil sin borrar sus originales.");
                record.profile.version = 2;
                if (string.IsNullOrEmpty(record.profile.identity)) record.profile.identity = Guid.NewGuid().ToString("N");
                var prepared = new Record
                {
                    formatVersion = record.formatVersion, profile = record.profile,
                    appearance = record.appearance.SanitizedCopy(), legacyProfileJson = record.legacyProfileJson
                };
                if (prepared.appearance.kind == "vrm") AvatarFiles.ReadManaged(prepared.appearance);
                var payload = JsonUtility.ToJson(prepared);
                var envelope = new Envelope { payload = payload, sha256 = AvatarFiles.Hash(Encoding.UTF8.GetBytes(payload)) };
                byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(envelope, true));
                if (bytes.Length > MaximumProfileBytes)
                    throw new InvalidDataException("El perfil preparado supera 1 MiB; no se ha reemplazado el perfil ni su respaldo.");
#if UNITY_EDITOR || DEBUG
                BeforeCommit?.Invoke();
#endif
                // Never overwrite the last valid backup with an unreadable primary file.
                if (File.Exists(SavePath) && !validMain)
                    File.Move(SavePath, SavePath + ".unreadable-" + Guid.NewGuid().ToString("N"));
                AvatarFiles.AtomicWrite(SavePath, bytes, path => TryRead(path, out _, payload));
                if (!TryRead(SavePath, out _, payload))
                    throw new InvalidDataException("No se pudo confirmar la lectura del perfil guardado. El respaldo anterior sigue disponible.");
                return true;
            }
            catch (Exception failure) when (failure is IOException || failure is InvalidDataException || failure is UnauthorizedAccessException || failure is ArgumentException)
            { error = "No se pudo guardar. Se conserva la selección anterior. " + failure.Message; return false; }
        }
    }
}
