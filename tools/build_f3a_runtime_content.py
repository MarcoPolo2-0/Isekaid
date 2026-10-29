"""Validate the reviewed local library and build its bounded Unity representation."""
import hashlib
import json
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / "Documentation/Specifications/F3A_Biblioteca_v0.1.json"
TARGET = ROOT / "UnityProjectIsekaid/Assets/_Project/Resources/Dialogue/F3A.es.json"
PERSONALITIES = {"tsundere": 0, "yandere": 1, "shy": 2}
EMOTIONS = dict(neutral="Neutral", alegria="Joy", tristeza="Sadness", preocupacion="Concern",
                enfado="Anger", sorpresa="Surprise", alivio="Relief")
EVENTS = {"FirstRescueCompleted", "TopicSelected", "PlayerOptionSelected", "TalkRequested", "FollowingChanged",
          "DownedEntered", "ReanimationCompleted", "SafeReturnTalkRequested", "ResumeConversationRequested"}
CONDITIONS = {"lifeState", "remembersFirstRescue", "interactionWindow", "selectedTopic", "selectedOption",
              "conversationMode", "hasActiveTree", "isFollowing", "orderVariant", "recoveryPending", "hasSuspendedConversation"}
SCOPES = {"encounter", "event_instance", "downed_episode", "recovery_episode"}
CLOSURES = {"offer_topics_when_eligible", "await_player_choice", "complete_topic_and_return_to_topics",
            "end_and_return_when_safe", "urgent_no_choices", "end_no_first_rescue_effect", "resume_saved_cursor_or_close_if_invalid"}
EXPRESSIONS = {"reset", "happy", "sad", "surprised", "sad_soft_if_validated", "angry_soft_if_validated", "relaxed_if_validated"}


def build():
    raw = SOURCE.read_text(encoding="utf-8").encode("utf-8")
    data = json.loads(raw)
    assert data["schemaVersion"] == 1 and data["language"] == "es"
    entries = data["entries"]
    ids = {entry["id"] for entry in entries}
    assert len(ids) == len(entries) == 60
    assert all(sum(entry["personality"] == key for entry in entries) == 20 for key in PERSONALITIES)
    assert len({entry["spokenText"] for entry in entries}) == 60
    output = []
    for entry in entries:
        trigger = entry["trigger"]
        conditions = trigger["conditions"]
        event = trigger["event"]
        assert event in EVENTS and set(conditions) <= CONDITIONS, entry["id"]
        assert conditions["lifeState"] == ("Downed" if event == "DownedEntered" else "Active")
        assert conditions["remembersFirstRescue"] is True
        assert conditions["interactionWindow"] == ("urgent" if event == "DownedEntered" else "provided_eligible")
        assert entry["nextNode"] is None and entry["closure"] in CLOSURES
        assert entry["memory"]["writes"] == [] and entry["memory"]["trustDelta"] == 0
        assert entry["memory"]["firstRescueEffectOwner"] == "existing_gameplay_only"
        repetition = entry["repetition"]
        assert repetition["scope"] in SCOPES and repetition["maxPresentationsPerScope"] == 1
        assert repetition["markConsumedOn"] == "presentation_started"
        assert 0 <= repetition["cooldownActiveSeconds"] <= 20
        presentation = entry["presentation"]
        assert presentation["preferredExpression"] in EXPRESSIONS and presentation["fallbackExpression"] == "reset"
        options = []
        for option in entry["playerOptions"]:
            assert option["nextNode"] in ids
            assert option["effects"] == dict(memoryWrites=[], trustDelta=0, followingChange=None, classChange=None)
            options.append(dict(id=option["id"], text=option["playerText"], target=option["nextNode"]))
        assert len(options) <= 3 and len({o["id"] for o in options}) == len(options)
        if event == "TopicSelected":
            assert conditions["selectedTopic"] == entry["tree"] and len(options) == 2
        if event == "PlayerOptionSelected":
            assert conditions["selectedOption"] in {o["id"] for root in entries for o in root["playerOptions"]}
        if event == "TalkRequested":
            assert conditions["conversationMode"] == "casual" and conditions["hasActiveTree"] is False
        if event == "FollowingChanged":
            assert type(conditions["isFollowing"]) is bool
            assert conditions.get("orderVariant", "") in ("", "plain", "opinion")
        if event in ("SafeReturnTalkRequested", "ResumeConversationRequested"):
            assert conditions["recoveryPending"] is True
            assert conditions["hasSuspendedConversation"] == (event == "ResumeConversationRequested")
        output.append(dict(id=entry["id"], personality=PERSONALITIES[entry["personality"]], text=entry["spokenText"],
                           emotion=EMOTIONS[entry["emotion"]], intention=entry["intention"], eventType=event,
                           lifeState=conditions["lifeState"], topic=entry.get("tree", ""),
                           orderVariant=conditions.get("orderVariant", ""), following=conditions.get("isFollowing", False),
                           group=repetition["group"], scope=repetition["scope"], cooldown=repetition["cooldownActiveSeconds"],
                           priority=entry["priority"]["value"], closure=entry["closure"], options=options,
                           preferredExpression=presentation["preferredExpression"], fallbackExpression=presentation["fallbackExpression"]))
    by_id = {entry["id"]: entry for entry in output}
    topics = []
    for tree in data["trees"]:
        roots = [tree["entryByPersonality"][key] for key in PERSONALITIES]
        assert all(by_id[key]["topic"] == tree["topic"] and by_id[key]["eventType"] == "TopicSelected" for key in roots)
        topics.append(dict(id=tree["id"], text=tree["playerPrompt"], roots=roots))
    assert len(topics) == 3 and len({t["id"] for t in topics}) == 3
    for entry in output:
        for option in entry["options"]:
            target = by_id[option["target"]]
            assert target["personality"] == entry["personality"] and target["topic"] == entry["topic"]
            assert target["eventType"] == "PlayerOptionSelected" and not target["options"]
            assert target["closure"] == "complete_topic_and_return_to_topics"
    return dict(formatVersion=2, provisional=True, libraryId=data["libraryId"], sourceSha256=hashlib.sha256(raw).hexdigest(),
                topics=topics, entries=output)


if __name__ == "__main__":
    expected = json.dumps(build(), ensure_ascii=False, indent=2) + "\n"
    if sys.argv[1:] == ["--check"]:
        assert TARGET.read_text(encoding="utf-8") == expected, "Runtime library is stale or edited outside its canonical source"
    else:
        assert not sys.argv[1:], "Use no arguments to build or --check to verify"
        TARGET.write_text(expected, encoding="utf-8")
    print("ISEKAID_F3A_LIBRARY_OK entries=60 paths=18")
