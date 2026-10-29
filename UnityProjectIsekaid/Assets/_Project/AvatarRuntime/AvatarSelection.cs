using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UniJSON;
using UnityEngine;

namespace Isekaid.ExperimentalVrm
{
    [Serializable]
    public sealed class AvatarSelection
    {
        public string kind = "catalog";
        public string catalogId = "adventurer-02";
        public string hash = "";
        public string vrmVersion = "";
        public string metadataJson = "";
        public bool localUseAcknowledged;

        public bool IsValid => kind == "catalog" ? catalogId == "adventurer-02" :
            kind == "vrm" && vrmVersion == "1.0" && localUseAcknowledged && IsHash(hash);

        private static bool IsHash(string value)
        {
            if (value == null || value.Length != 64) return false;
            foreach (char c in value) if (!(c >= '0' && c <= '9') && !(c >= 'a' && c <= 'f')) return false;
            return true;
        }

        public static AvatarSelection FromBytes(byte[] bytes, bool acknowledged)
        {
            VrmFileGuard.Validate(bytes);
            var root = JsonParser.Parse(Encoding.UTF8.GetString(bytes, 20, BitConverter.ToInt32(bytes, 12)));
            return new AvatarSelection
            {
                kind = "vrm", catalogId = "", vrmVersion = "1.0", hash = AvatarFiles.Hash(bytes),
                metadataJson = root["extensions"]["VRMC_vrm"]["meta"].ToString("  "),
                localUseAcknowledged = acknowledged
            };
        }
    }

    [Serializable]
    public sealed class AvatarPermissions
    {
        public string name;
        public string[] authors;
        public string avatarPermission;
        public string commercialUsage;
        public string creditNotation;
        public string modification;
        public bool allowRedistribution;
        public string otherLicenseUrl;

        public static string Describe(string json)
        {
            var meta = JsonUtility.FromJson<AvatarPermissions>(json);
            if (meta == null) return "Metadatos ausentes. Permisos pendientes de revisión.";
            string Value(string value) => string.IsNullOrEmpty(value) ? "no declarado" : value;
            return $"Modelo: {Value(meta.name)}\nAutoría: {Value(meta.authors == null ? null : string.Join(", ", meta.authors))}\n" +
                $"Usuarios: {Value(meta.avatarPermission)} · Uso comercial: {Value(meta.commercialUsage)}\n" +
                $"Créditos: {Value(meta.creditNotation)} · Modificación: {Value(meta.modification)}\n" +
                $"Redistribución declarada: {meta.allowRedistribution}\nLicencia adicional: {Value(meta.otherLicenseUrl)}\n" +
                "Esto describe el archivo; no verifica derechos sobre cabello, ropa u otros recursos. No se publicará el modelo.";
        }
    }

    public static class AvatarFiles
    {
        public static string Root
        {
            get
            {
#if UNITY_EDITOR || DEBUG
                var args = Environment.GetCommandLineArgs();
                for (int i = 0; i + 1 < args.Length; i++)
                    if (args[i] == "-isekTestData" && Path.IsPathRooted(args[i + 1])) return args[i + 1];
#endif
                return Path.Combine(Application.persistentDataPath, "Companion");
            }
        }

        public static string Hash(byte[] bytes)
        {
            using var sha = SHA256.Create();
            return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
        }

        public static string ManagedPath(AvatarSelection selection)
        {
            if (selection == null || !selection.IsValid || selection.kind != "vrm") throw new InvalidDataException("Referencia de avatar inválida.");
            return Path.Combine(Root, "Avatars", selection.hash + ".vrm");
        }

        public static byte[] ReadManaged(AvatarSelection selection)
        {
            var bytes = VrmFileGuard.ReadLocal(ManagedPath(selection));
            if (Hash(bytes) != selection.hash) throw new InvalidDataException("La copia privada del avatar está dañada. Reimporta el archivo o elige la apariencia incluida.");
            return bytes;
        }

        public static void Admit(AvatarSelection selection, byte[] bytes)
        {
            if (bytes == null || Hash(bytes) != selection.hash) throw new InvalidDataException("La selección ya no corresponde al modelo preparado.");
            VrmFileGuard.Validate(bytes);
            string path = ManagedPath(selection);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            if (File.Exists(path))
            {
                // Preserve an existing cache entry until its replacement has been flushed.
                if (Hash(File.ReadAllBytes(path)) == selection.hash) return;
            }
            AtomicWrite(path, bytes);
        }

        public static void AtomicWrite(string path, byte[] bytes)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string temporary = path + ".tmp-" + Guid.NewGuid().ToString("N");
            try
            {
                using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                { stream.Write(bytes, 0, bytes.Length); stream.Flush(true); }
                if (File.Exists(path)) File.Replace(temporary, path, path + ".bak");
                else File.Move(temporary, path);
            }
            finally { if (File.Exists(temporary)) File.Delete(temporary); }
        }
    }
}
