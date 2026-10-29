"""Verify F2 delivery boundaries; Unity runtime checks are separate."""
import hashlib
import json
from pathlib import Path
import subprocess

root = Path(__file__).resolve().parents[1]
report = root / "Documentation/Audits/VRM-F2"
manifest = json.loads((report / "protected-files.json").read_text(encoding="utf-8"))
changed = []
for name, expected in manifest["sha256"].items():
    file = root / name
    if not file.is_file() or hashlib.sha256(file.read_bytes()).hexdigest() != expected:
        changed.append(name)
assert not changed, f"Protected originals changed: {changed}"
tracked = subprocess.check_output(["git", "ls-files", "-z"], cwd=root).decode().split("\0")
assert not any(p.lower().endswith(".vrm") for p in tracked), "A private VRM is tracked"
assert not any(p.startswith("UnityProjectIsekaid/LocalVrmEvidence/") for p in tracked), "Private evidence is tracked"
assert not any(p.startswith("UnityProjectIsekaid/Builds/") for p in tracked), "Build output is tracked"
for name in ("editor-results.json", "storage-results.json", "windows-results.json", "restart-results.json",
             "native-picker-results.json", "guard-results.json", "regression-results.json", "build-results.json"):
    path = report / name
    assert path.is_file(), f"Missing run: {name}"
    result = json.loads(path.read_text(encoding="utf-8-sig"))
    assert result["passed"], f"Failed run: {name}"
print(f"ISEKAID_F2_DELIVERY_CHECKS_PASSED protected={len(manifest['sha256'])}")
