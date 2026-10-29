using System;
using System.Collections.Generic;
using UnityEngine;

namespace Isekaid
{
    public enum CompanionEmotion { Neutral, Joy, Sadness, Concern, Anger, Surprise, Relief }
    public enum DialoguePriority { Casual, Important, Survival }

    [Serializable]
    public sealed class DialogueOption
    {
        public string id;
        public string text;
        public string target;
    }

    [Serializable]
    public sealed class DialogueEntry
    {
        public string id;
        public string emotion;
        public string[] texts;
        public DialogueOption[] options;
        public CompanionEmotion Emotion => Enum.Parse<CompanionEmotion>(emotion);
        public string Text(CompanionPersonality personality) => texts[(int)personality];
    }

    [Serializable]
    public sealed class DialogueContent
    {
        public bool provisional;
        public DialogueEntry[] entries;
        private Dictionary<string, DialogueEntry> byId;
        private static DialogueContent current;

        public static DialogueContent Current
        {
            get
            {
                if (current != null) return current;
                var asset = Resources.Load<TextAsset>("Dialogue/F3A.es");
                if (asset == null) throw new InvalidOperationException("Falta el contenido local de diálogo F3A.");
                current = JsonUtility.FromJson<DialogueContent>(asset.text);
                current.Validate();
                return current;
            }
        }

        public DialogueEntry Get(string id) => byId[id];

        public void Validate()
        {
            byId = new Dictionary<string, DialogueEntry>(StringComparer.Ordinal);
            if (entries == null || entries.Length == 0) throw new InvalidOperationException("No hay entradas de diálogo.");
            foreach (var entry in entries)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.id) || entry.texts == null || entry.texts.Length != 3 ||
                    !Enum.TryParse<CompanionEmotion>(entry.emotion, out var emotion) || !Enum.IsDefined(typeof(CompanionEmotion), emotion))
                    throw new InvalidOperationException("Entrada de diálogo inválida.");
                foreach (string text in entry.texts)
                    if (string.IsNullOrWhiteSpace(text)) throw new InvalidOperationException("Variante de personalidad vacía: " + entry.id);
                if (!byId.TryAdd(entry.id, entry)) throw new InvalidOperationException("ID de diálogo duplicado: " + entry.id);
            }
            foreach (var entry in entries)
            {
                if (entry.options == null) continue;
                if (entry.options.Length > 3) throw new InvalidOperationException("F3A permite hasta tres opciones.");
                var ids = new HashSet<string>(StringComparer.Ordinal);
                foreach (var option in entry.options)
                    if (option == null || string.IsNullOrWhiteSpace(option.id) || string.IsNullOrWhiteSpace(option.text) ||
                        !ids.Add(option.id) || !byId.ContainsKey(option.target ?? ""))
                        throw new InvalidOperationException("Opción de diálogo inválida: " + entry.id);
            }
        }
    }
}
