using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Isekaid.ExperimentalVrm;

namespace Isekaid.ExperimentalVrm.Editor
{
    public static class VrmGuardChecks
    {
        public static int Run(byte[] valid)
        {
            int count = 0;
            uint jsonLength = BitConverter.ToUInt32(valid, 12);
            string json = Encoding.UTF8.GetString(valid, 20, (int)jsonLength).TrimEnd();
            byte[] ReplaceJson(string replacement)
            {
                var content = Encoding.UTF8.GetBytes(replacement);
                int padded = (content.Length + 3) & ~3;
                using var stream = new MemoryStream(); using var writer = new BinaryWriter(stream);
                writer.Write(0x46546c67u); writer.Write(2u); writer.Write((uint)(20 + padded + valid.Length - 20 - jsonLength));
                writer.Write((uint)padded); writer.Write(0x4e4f534au); writer.Write(content);
                for (int i = content.Length; i < padded; i++) writer.Write((byte)32);
                writer.Write(valid, 20 + (int)jsonLength, valid.Length - 20 - (int)jsonLength);
                return stream.ToArray();
            }
            void Rejected(byte[] bytes)
            {
                bool rejected = false;
                try { VrmFileGuard.Validate(bytes); } catch (Exception) { rejected = true; }
                if (!rejected) throw new InvalidOperationException("Unsafe or unsupported VRM fixture was accepted.");
                count++;
            }
            VrmFileGuard.Validate(valid);
            Rejected(Array.Empty<byte>()); Rejected(new byte[28]); Rejected(new byte[VrmFileGuard.MaximumBytes + 1]);
            Rejected(ReplaceJson("{\"uri\":\"file:///not-permitted.png\"," + json.Substring(1)));
            Rejected(ReplaceJson("{\"asset\":{}," + json.Substring(1)));
            Rejected(ReplaceJson("{\"deep\":" + new string('[', 65) + "0" + new string(']', 65) + "," + json.Substring(1)));
            int views = json.IndexOf("\"bufferViews\"", StringComparison.Ordinal);
            var offsetPattern = new Regex("\"byteOffset\"\\s*:\\s*\\d+");
            Rejected(ReplaceJson(json.Substring(0, views) + offsetPattern.Replace(json.Substring(views), "\"byteOffset\":9223372036854775806", 1)));
            var badVersion = (byte[])valid.Clone(); badVersion[4] = 1; Rejected(badVersion);
            var truncated = new byte[valid.Length - 4]; Array.Copy(valid, truncated, truncated.Length); Rejected(truncated);
            return count;
        }
    }
}
