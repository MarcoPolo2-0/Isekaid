"""Create a private VRM metadata fixture without changing its binary resources."""
import argparse
import hashlib
import json
from pathlib import Path
import struct

parser = argparse.ArgumentParser()
parser.add_argument("source", type=Path)
parser.add_argument("output", type=Path)
args = parser.parse_args()
source, output = args.source.resolve(), args.output.resolve()
repo = Path(__file__).resolve().parents[1]
assert not output.is_relative_to(repo), "Keep the fixture outside the repository"
assert output.suffix.lower() == ".vrm" and not output.exists(), "Use a new private .vrm path"
assert 28 <= source.stat().st_size <= 32 * 1024 * 1024, "Source exceeds fixture limits"
raw = source.read_bytes()
magic, version, size, length, kind = struct.unpack_from("<IIIII", raw)
assert magic == 0x46546C67 and version == 2 and size == len(raw) and kind == 0x4E4F534A
assert 2 <= length <= 4 * 1024 * 1024 and 20 + length + 8 <= len(raw)
root = json.loads(raw[20:20 + length])
assert root["extensions"]["VRMC_vrm"]["specVersion"] == "1.0"
root["extensions"]["VRMC_vrm"]["meta"]["unusedPayload"] = "x" * 1300000
content = json.dumps(root, ensure_ascii=False, separators=(",", ":")).encode("utf-8")
content += b" " * (-len(content) % 4)
assert 1024 * 1024 < len(content) <= 4 * 1024 * 1024
remainder = raw[20 + length:]
result = struct.pack("<IIIII", magic, version, 20 + len(content) + len(remainder), len(content), kind) + content + remainder
assert len(result) <= 32 * 1024 * 1024
output.parent.mkdir(parents=True, exist_ok=True)
with output.open("xb") as stream:
    stream.write(result)
print(json.dumps({"bytes": len(result), "jsonBytes": len(content), "sha256": hashlib.sha256(result).hexdigest()}))
