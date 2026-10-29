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
        public string id, text, emotion, intention, eventType, lifeState, topic, orderVariant;
        public string group, scope, closure, preferredExpression, fallbackExpression;
        public int personality, priority;
        public float cooldown;
        public bool following;
        public DialogueOption[] options;
        public CompanionEmotion Emotion => Enum.Parse<CompanionEmotion>(emotion);
        public string Text(CompanionPersonality value) => (int)value == personality ? text :
            throw new InvalidOperationException("El cursor de diálogo pertenece a otra personalidad.");
    }

    [Serializable]
    public sealed class DialogueTopic
    {
        public string id, text;
        public string[] roots;
    }

    [Serializable]
    public sealed class DialogueContent
    {
        public bool provisional;
        public int formatVersion;
        public string libraryId, sourceSha256;
        public DialogueTopic[] topics;
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

        public DialogueEntry Reaction(string eventType, CompanionPersonality personality, bool following = false, string variant = "")
        {
            foreach (var entry in entries)
                if (entry.eventType == eventType && entry.personality == (int)personality &&
                    (eventType != "FollowingChanged" || entry.following == following && entry.orderVariant == variant)) return entry;
            throw new InvalidOperationException("No hay respuesta canónica para " + eventType);
        }

        public DialogueEntry Rebind(DialogueEntry entry, CompanionPersonality personality)
        {
            if (entry == null) return null;
            string tag = personality == CompanionPersonality.Tsundere ? "TSU" : personality == CompanionPersonality.Yandere ? "YAN" : "SHY";
            return Get("F3A." + tag + entry.id.Substring("F3A.TSU".Length));
        }

        public void Validate()
        {
            byId = new Dictionary<string, DialogueEntry>(StringComparer.Ordinal);
            if (formatVersion != 2 || string.IsNullOrWhiteSpace(libraryId) || sourceSha256?.Length != 64 ||
                entries == null || entries.Length == 0 || topics == null || topics.Length != 3)
                throw new InvalidOperationException("Formato de biblioteca de diálogo inválido.");
            foreach (var entry in entries)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.id) || string.IsNullOrWhiteSpace(entry.text) ||
                    entry.personality < 0 || entry.personality > 2 || entry.priority < 0 || entry.priority > 100 ||
                    entry.cooldown < 0 || float.IsNaN(entry.cooldown) || float.IsInfinity(entry.cooldown) ||
                    !Enum.TryParse<CompanionEmotion>(entry.emotion, out var emotion) || !Enum.IsDefined(typeof(CompanionEmotion), emotion))
                    throw new InvalidOperationException("Entrada de diálogo inválida.");
                if (!byId.TryAdd(entry.id, entry)) throw new InvalidOperationException("ID de diálogo duplicado: " + entry.id);
                if (entry.scope != "encounter" && entry.scope != "event_instance" && entry.scope != "downed_episode" && entry.scope != "recovery_episode")
                    throw new InvalidOperationException("Alcance de consumo inválido: " + entry.id);
            }
            foreach (var entry in entries)
            {
                if (entry.options == null) continue;
                if (entry.options.Length > 3) throw new InvalidOperationException("F3A permite hasta tres opciones.");
                var ids = new HashSet<string>(StringComparer.Ordinal);
                foreach (var option in entry.options)
                    if (option == null || string.IsNullOrWhiteSpace(option.id) || string.IsNullOrWhiteSpace(option.text) ||
                        !ids.Add(option.id) || !byId.TryGetValue(option.target ?? "", out var target) ||
                        target.personality != entry.personality || target.topic != entry.topic || target.eventType != "PlayerOptionSelected" ||
                        target.options?.Length > 0 || target.closure != "complete_topic_and_return_to_topics")
                        throw new InvalidOperationException("Opción de diálogo inválida: " + entry.id);
            }
            var topicIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var topic in topics)
            {
                if (topic == null || string.IsNullOrWhiteSpace(topic.id) || string.IsNullOrWhiteSpace(topic.text) ||
                    !topicIds.Add(topic.id) || topic.roots?.Length != 3) throw new InvalidOperationException("Tema inválido.");
                for (int personality = 0; personality < 3; personality++)
                    if (!byId.TryGetValue(topic.roots[personality], out var root) || root.personality != personality ||
                        root.eventType != "TopicSelected" || topic.id != "F3A.TREE." + root.topic || root.options?.Length != 2)
                        throw new InvalidOperationException("Raíz inválida: " + topic.id);
            }
        }
    }
}
