#if UNITY_EDITOR || DEBUG
using System;
using System.IO;
using System.Linq;
using System.Text;
using Isekaid.ExperimentalVrm;
using UnityEngine;

namespace Isekaid.Validation
{
    public static class F2MetadataChecks
    {
        public static void RunStorage()
        {
            string directory = F2RuntimeChecks.Argument("-isekTestData");
            if (string.IsNullOrEmpty(directory) || !Path.IsPathRooted(directory) || File.Exists(CompanionProfileStore.SavePath))
                throw new InvalidOperationException("A fresh isolated data directory is required.");
            int count = 0;
            void Check(bool condition, string name)
            { if (!condition) throw new InvalidOperationException(name); count++; }
            var record = new CompanionProfileStore.Record
            {
                profile = new CompanionProfile { identity = "metadata-roundtrip-fixture", personality = CompanionPersonality.Yandere, hair = 2 },
                legacyProfileJson = "{\"memoryFixture\":{\"firstRescue\":true,\"trust\":7},\"classFixture\":\"archer\"}"
            };
            Check(CompanionProfileStore.TrySave(record, out _), "Baseline profile saves");
            Check(CompanionProfileStore.TrySave(record, out _), "Baseline backup saves");
            byte[] valid = VrmFileGuard.ReadLocal(F2RuntimeChecks.Argument("-isekVrmFixture"));
            var source = AvatarSelection.FromBytes(valid, true);
            AvatarFiles.Admit(source, valid);
            var bounded = source.SanitizedCopy();
            source.metadataJson = bounded.metadataJson.Insert(1, "\"unusedPayload\":\"" + new string('x', 1300000) + "\",");
            Check(Encoding.UTF8.GetByteCount(source.metadataJson) > CompanionProfileStore.MaximumProfileBytes, "Adversarial metadata exceeds the original reader budget");
            string previous = File.ReadAllText(CompanionProfileStore.SavePath);
            record.appearance = source;
            Check(CompanionProfileStore.TrySave(record, out var saveError), "Whitelisted large metadata saves: " + saveError);
            var loaded = CompanionProfileStore.LoadRecord();
            Check(CompanionProfileStore.LastWarning == "" && loaded.appearance.hash == source.hash && loaded.profile.identity == record.profile.identity &&
                loaded.profile.personality == record.profile.personality && loaded.legacyProfileJson == record.legacyProfileJson,
                "Real disk roundtrip preserves identity, personality, legacy data and avatar reference without backup fallback");
            Check(!loaded.appearance.metadataJson.Contains("unusedPayload") && Encoding.UTF8.GetByteCount(loaded.appearance.metadataJson) <= AvatarSelection.MaximumMetadataBytes,
                "Only bounded known metadata persists");
            Check(File.ReadAllText(CompanionProfileStore.SavePath + ".bak") == previous, "Successful save backs up the previous readable profile");
            Check(source.metadataJson.Contains("unusedPayload"), "Sanitization does not mutate the caller's source metadata");

            void Reject(string json, string name)
            {
                var candidate = bounded.SanitizedCopy(); candidate.metadataJson = json; record.appearance = candidate;
                string primary = File.ReadAllText(CompanionProfileStore.SavePath), backup = File.ReadAllText(CompanionProfileStore.SavePath + ".bak");
                Check(!CompanionProfileStore.TrySave(record, out _), name);
                Check(primary == File.ReadAllText(CompanionProfileStore.SavePath) && backup == File.ReadAllText(CompanionProfileStore.SavePath + ".bak"),
                    name + " preserves primary and backup byte-for-byte");
            }
            Reject("{\"name\":\"" + new string('x', 257) + "\"}", "Oversized name rejected");
            Reject("{\"name\":\"" + new string('界', 86) + "\"}", "UTF-8 field budget uses bytes");
            Reject("{\"thirdPartyLicenses\":\"" + new string('x', 8193) + "\"}", "License text is rejected instead of truncated");
            Reject("{\"authors\":[" + string.Join(",", Enumerable.Repeat("\"author\"", 17)) + "]}", "Author count budget enforced");
            Reject("{\"references\":[" + string.Join(",", Enumerable.Repeat("\"reference\"", 9)) + "]}", "Reference count budget enforced");
            Reject("{\"name\":\"hidden\\u0000name\"}", "Unsafe control characters rejected");
            Reject("{\"allowRedistribution\":\"yes\"}", "Permission types are validated");
            Reject("{\"authors\":42}", "Invalid author container rejected");
            Reject("{\"copyrightInformation\":\"" + new string('c', 8192) + "\",\"thirdPartyLicenses\":\"" + new string('l', 8192) +
                "\",\"references\":[" + string.Join(",", Enumerable.Repeat("\"" + new string('r', 2048) + "\"", 8)) + "]}", "Combined metadata budget enforced");
            Reject("{broken", "Malformed metadata rejected");

            record.appearance = new AvatarSelection();
            string stable = File.ReadAllText(CompanionProfileStore.SavePath), stableBackup = File.ReadAllText(CompanionProfileStore.SavePath + ".bak");
            record.legacyProfileJson = new string('\\', CompanionProfileStore.MaximumProfileBytes / 4);
            Check(!CompanionProfileStore.TrySave(record, out _), "Escaped envelope size is checked before replacing the profile");
            Check(stable == File.ReadAllText(CompanionProfileStore.SavePath) && stableBackup == File.ReadAllText(CompanionProfileStore.SavePath + ".bak"),
                "Oversized complete profile preserves both existing files");
            record.legacyProfileJson = "schema-fixture"; record.formatVersion = 99;
            Check(!CompanionProfileStore.TrySave(record, out _), "The actual temporary-file reader rejects unsupported record versions");
            Check(stable == File.ReadAllText(CompanionProfileStore.SavePath) && stableBackup == File.ReadAllText(CompanionProfileStore.SavePath + ".bak"),
                "Temporary read failure does not sacrifice the backup");
            Check(Directory.GetFiles(directory, "profile-v2.json.tmp-*").Length == 0, "Rejected temporary profile is cleaned up");
            record.formatVersion = 2;
            record.legacyProfileJson = new string('x', CompanionProfileStore.MaximumProfileBytes - 4096);
            Check(CompanionProfileStore.TrySave(record, out _), "A complete profile near the maximum still saves");
            loaded = CompanionProfileStore.LoadRecord();
            Check(CompanionProfileStore.LastWarning == "" && loaded.legacyProfileJson == record.legacyProfileJson && new FileInfo(CompanionProfileStore.SavePath).Length <= CompanionProfileStore.MaximumProfileBytes,
                "Near-budget persisted profile is read in full without fallback");
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "metadata-results.json"), "{\"passed\":true,\"assertions\":" + count + ",\"metadataBudgetBytes\":" + AvatarSelection.MaximumMetadataBytes +
                ",\"profileBudgetBytes\":" + CompanionProfileStore.MaximumProfileBytes + "}");
            Debug.Log("ISEKAID_F2_METADATA_CHECKS_PASSED " + count);
        }
    }
}
#endif
