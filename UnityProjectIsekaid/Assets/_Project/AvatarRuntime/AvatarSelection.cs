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
        public const int MaximumMetadataBytes = 32 * 1024;
        private static readonly UTF8Encoding MetadataEncoding = new UTF8Encoding(false, true);
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
                metadataJson = SanitizeMetadata(root["extensions"]["VRMC_vrm"]["meta"]),
                localUseAcknowledged = acknowledged
            };
        }

        public AvatarSelection SanitizedCopy()
        {
            if (metadataJson != null && MetadataEncoding.GetByteCount(metadataJson) > 4 * 1024 * 1024)
                throw new InvalidDataException("Los metadatos del avatar superan el límite de lectura.");
            return new AvatarSelection
            {
                kind = kind, catalogId = catalogId, hash = hash, vrmVersion = vrmVersion,
                localUseAcknowledged = localUseAcknowledged,
                metadataJson = kind == "vrm" ? SanitizeMetadata(JsonParser.Parse(metadataJson ?? "")) : ""
            };
        }

        private static string SanitizeMetadata(JsonNode meta)
        {
            if (!meta.IsMap()) throw new InvalidDataException("Los metadatos del avatar deben ser un objeto JSON.");
            var json = new JsonFormatter();
            json.BeginMap();
            foreach (var field in new[] {
                ("name", 256), ("version", 64), ("copyrightInformation", 8192), ("contactInformation", 512),
                ("thirdPartyLicenses", 8192), ("avatarPermission", 64), ("commercialUsage", 64),
                ("creditNotation", 64), ("modification", 64), ("otherLicenseUrl", 2048) })
            {
                if (!meta.ContainsKey(field.Item1)) continue;
                json.Key(field.Item1);
                json.Value(BoundedText(meta[field.Item1], field.Item1, field.Item2));
            }
            foreach (var field in new[] { ("authors", 16, 256), ("references", 8, 2048) })
            {
                if (!meta.ContainsKey(field.Item1)) continue;
                var values = meta[field.Item1];
                if (!values.IsArray() || values.GetArrayCount() > field.Item2)
                    throw new InvalidDataException("Demasiadas entradas o formato inválido en " + field.Item1 + ".");
                json.Key(field.Item1); json.BeginList();
                foreach (var value in values.ArrayItems()) json.Value(BoundedText(value, field.Item1, field.Item3));
                json.EndList();
            }
            foreach (string field in new[] { "allowExcessivelyViolentUsage", "allowExcessivelySexualUsage",
                "allowPoliticalOrReligiousUsage", "allowAntisocialOrHateUsage", "allowRedistribution" })
            {
                if (!meta.ContainsKey(field)) continue;
                if (!meta[field].IsBoolean()) throw new InvalidDataException("Permiso de avatar inválido: " + field + ".");
                json.Key(field); json.Value(meta[field].GetBoolean());
            }
            json.EndMap();
            string result = json.ToString();
            if (MetadataEncoding.GetByteCount(result) > MaximumMetadataBytes)
                throw new InvalidDataException("Los metadatos permitidos del avatar superan 32 KiB. No se han recortado sus condiciones de uso.");
            return result;
        }

        private static string BoundedText(JsonNode value, string field, int maximumBytes)
        {
            if (!value.IsString()) throw new InvalidDataException("Texto de metadatos inválido: " + field + ".");
            string text = value.GetString();
            if (MetadataEncoding.GetByteCount(text) > maximumBytes)
                throw new InvalidDataException("El campo " + field + " supera el límite de metadatos. No se ha recortado su contenido.");
            foreach (char c in text)
                if (char.IsControl(c) && c != '\n' && c != '\r' && c != '\t')
                    throw new InvalidDataException("El campo " + field + " contiene caracteres de control no admitidos.");
            return text;
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

        public static void AtomicWrite(string path, byte[] bytes, Func<string, bool> validate = null)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string temporary = path + ".tmp-" + Guid.NewGuid().ToString("N");
            try
            {
                using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                { stream.Write(bytes, 0, bytes.Length); stream.Flush(true); }
                if (validate != null && !validate(temporary))
                    throw new InvalidDataException("El archivo preparado no supera la lectura de verificación; se conserva el guardado anterior.");
                if (File.Exists(path)) File.Replace(temporary, path, path + ".bak");
                else File.Move(temporary, path);
            }
            finally { if (File.Exists(temporary)) File.Delete(temporary); }
        }
    }
}
