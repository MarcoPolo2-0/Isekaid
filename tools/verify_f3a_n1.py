"""Verify the authorized N1 v0.5 integration against its exact Git sources."""
import copy
import hashlib
import importlib.util
import json
from pathlib import Path
import subprocess
import sys

sys.dont_write_bytecode = True
ROOT = Path(__file__).resolve().parents[1]
BASE = "0ef11e552178ad2daf52293d7c32f4cdd1f80ea6"
APPROVED = "5bdfeed43994e509f48dde148beca50dc961278f"
PATCH_PATH = "Documentation/Specifications/F3A-N1/17_Parche_Editorial_v0.5_PROPUESTA.json"
PATCH_BLOB = "77228337ac434b4a56a47e25f05219e6c1ceffe7"
CANONICAL = "Documentation/Specifications/F3A_Biblioteca_v0.1.json"
CANONICAL_BLOB = "2931044800a500bbcefe184b15c27ae516c393e7"
SOURCE_HASH = "e475943e9a9eaab1787277a8257d111af21c7ff8509bef5f8145b3e3fc4f214e"
AUDIT = ROOT / "Documentation/Audits/F3A-N1"
SUFFIXES = ("A.RESCUE", "B.FEEL.OPEN", "B.FEEL.BREATH", "B.FEEL.LATER", "B.ROAD.UNCERTAIN",
            "C.LATER_1", "D.WAIT", "D.WAIT_OPINION", "E.REVIVED", "F.RESUME")
AUTHORIZED = [f"F3A.{personality}.{suffix}" for personality in ("TSU", "YAN", "SHY") for suffix in SUFFIXES]


def git(*args):
    return subprocess.check_output(["git", *args], cwd=ROOT)


def normalized(raw):
    return raw.decode("utf-8-sig").replace("\r\n", "\n").replace("\r", "\n").encode("utf-8")


def blob(raw):
    return hashlib.sha1(b"blob " + str(len(raw)).encode("ascii") + b"\0" + raw).hexdigest()


def candidate(source, patch):
    assert hashlib.sha256(normalized(source)).hexdigest() == SOURCE_HASH, "Canonical hash mismatch"
    assert patch["proposalVersion"] == "0.5" and patch["patchSchemaVersion"] == 2
    base = patch["base"]
    assert (base["commit"], base["canonicalPath"], base["canonicalBlob"], base["canonicalSha256"]) == (
        BASE, CANONICAL, CANONICAL_BLOB, SOURCE_HASH), "Patch provenance mismatch"
    assert patch["integration"]["allowedFields"] == ["spokenText", "intention"]
    selected = [r for r in patch["reviews"] if r["recommendation"] in ("ACEPTAR", "MEJORAR")]
    assert len(patch["reviews"]) == 33 and len({r["id"] for r in patch["reviews"]}) == 33
    assert len(selected) == 30 and {r["id"] for r in selected} == set(AUTHORIZED), "Unauthorized selection"
    result = json.loads(source)
    entries = {entry["id"]: entry for entry in result["entries"]}
    assert len(entries) == len(result["entries"]) == 60
    for review in patch["reviews"]:
        entry = entries[review["id"]]
        assert review["personality"] == entry["personality"]
        assert set(review["expected"]) == set(review["recommended"]) == {"spokenText", "intention"}
        for field, expected in review["expected"].items():
            assert entry[field] == expected, f"Stale expected: {review['id']}/{field}"
        if review["id"] in AUTHORIZED:
            entry.update(review["recommended"])
        else:
            assert review["id"].endswith(".E.DOWNED") and review["recommendation"] == "DESCARTAR"
            assert review["recommended"] == review["expected"]
    return result


def verify():
    source = git("show", f"{BASE}:{CANONICAL}")
    assert blob(source) == CANONICAL_BLOB
    patch_raw = (AUDIT / "Parche_Autorizado_v0.5.json").read_bytes()
    assert blob(normalized(patch_raw)) == PATCH_BLOB, "Approved patch bytes changed"
    patch = json.loads(patch_raw)
    previous = git("show", f"{BASE}:{patch['base']['previousProposalPath']}")
    assert blob(previous) == patch["base"]["previousProposalBlob"]
    assert hashlib.sha256(normalized(previous)).hexdigest() == patch["base"]["previousProposalSha256"]
    expected = candidate(source, patch)
    current_raw = (ROOT / CANONICAL).read_bytes()
    assert current_raw == normalized(current_raw), "Canonical must be UTF-8/LF without BOM"
    assert json.loads(current_raw) == expected, "Integration contains missing or unauthorized changes"
    original = json.loads(source)
    changed = []
    for before, after in zip(original["entries"], expected["entries"]):
        fields = [key for key in before if before[key] != after[key]]
        if fields:
            assert fields and set(fields) <= {"spokenText", "intention"}
            changed.append(dict(id=before["id"], fields=fields))
    assert [e["id"] for e in changed] == AUTHORIZED
    assert sum(len(e["playerOptions"]) for e in expected["entries"]) == 18
    assert git("show", f"{BASE}:UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionPalette.cs").decode().replace(
        '"Tímida"', '"Dandere"') == (ROOT / "UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionPalette.cs").read_text(encoding="utf-8")
    spec = importlib.util.spec_from_file_location("f3a_builder", ROOT / "tools/build_f3a_runtime_content.py")
    builder = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(builder)
    runtime = builder.TARGET.read_bytes()
    assert runtime == normalized(runtime), "Runtime must be UTF-8/LF without BOM"
    assert json.loads(runtime) == builder.build(), "Runtime is stale"
    manifest = json.loads((AUDIT / "manifest.json").read_text(encoding="utf-8"))
    assert manifest["approvedCommit"] == APPROVED and manifest["baseCommit"] == BASE
    assert manifest["sourcePath"] == PATCH_PATH and manifest["sourceBlob"] == PATCH_BLOB
    assert manifest["patchSha256"] == hashlib.sha256(normalized(patch_raw)).hexdigest()
    assert manifest["changes"] == changed and manifest["unchangedEntries"] == 30
    assert manifest["canonicalSha256After"] == hashlib.sha256(current_raw).hexdigest()
    assert manifest["runtimeSha256After"] == hashlib.sha256(runtime).hexdigest()
    # The complete structural comparison above protects every field outside the two approved fields.
    rejected = []
    for name in ("hash", "expected", "unknown_id", "duplicate_id", "extra_field", "discard", "base"):
        bad = copy.deepcopy(patch)
        raw = source
        if name == "hash": raw += b" "
        if name == "expected": bad["reviews"][0]["expected"]["spokenText"] += "!"
        if name == "unknown_id": bad["reviews"][0]["id"] = "F3A.TSU.UNKNOWN"
        if name == "duplicate_id": bad["reviews"][1]["id"] = bad["reviews"][0]["id"]
        if name == "extra_field": bad["reviews"][0]["recommended"]["emotion"] = "alegria"
        if name == "discard": next(r for r in bad["reviews"] if r["recommendation"] == "DESCARTAR")["recommendation"] = "ACEPTAR"
        if name == "base": bad["base"]["canonicalBlob"] = "0" * 40
        try:
            candidate(raw, bad)
        except (AssertionError, KeyError):
            rejected.append(name)
        else:
            raise AssertionError(f"Invalid input accepted: {name}")
    assert normalized(source) == normalized(source.replace(b"\n", b"\r\n"))
    print("ISEKAID_N1_VERIFIED changed=30 unchanged=30 entries=60 paths=18 rejected=" + str(len(rejected)))


if __name__ == "__main__":
    verify()
