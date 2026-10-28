from pathlib import Path
import json,hashlib
p=Path(__file__).resolve().parents[1];repo=p.parents[2]
def read(name): return json.loads((p/name).read_text(encoding='utf8'))
r=read('runtime-results.json');source=read('source-inspection.json')
assert not r['error'] and r['humanoid'] and r['followMoved'] and r['safeFallback']
assert r['personalityIndependent'] and r['failedImportPreservedAvatar'] and r['rejectionChecks']==9
assert r['motionTests'][1]['maximumThighRotation']>15 and r['motionTests'][2]['maximumThighRotation']>30
assert r['motionTests'][3]['maximumRootRotation']>90
assert len(source['file_preset_expressions'])==14
assert all(c['available'] and c['maximumMorphWeight']>0 for c in r['faceTests'])
baseline=read('baseline-results.json');assert all(baseline['before'].values()) and all(baseline['after'].values())
assert not read('protected-files-check.json')['changed']
for name,sha in read('artifact-manifest.json').items():assert hashlib.sha256((p/name).read_bytes()).hexdigest()==sha,name
model=repo/source['file']
if model.exists():assert hashlib.sha256(model.read_bytes()).hexdigest()==source['sha256']
private=repo/read('local-capture-manifest.json')['private_directory']
if private.exists():
 for name,sha in read('local-capture-manifest.json')['captures'].items():assert hashlib.sha256((private/name).read_bytes()).hexdigest()==sha,name
print('VRM_SAVED_DELIVERY_CHECKS_PASSED; this checks recorded evidence, not a new Unity run')
