import hashlib,json
from pathlib import Path
p=Path(__file__).resolve().parents[1]
def read(name): return json.loads((p/name).read_text())
assert (p/'INFORME.md').is_file()
r=read('deformation-results.json');u=read('evidence/unity/unity-import-results.json')
assert hashlib.sha256((p.parent/'G2-PROTO/input/woman.fbx').read_bytes()).hexdigest()=='674c07ff612577a1a5b7e7ee124202722fa5eb0f68bdc01c820a875b5bef1d87'
assert len(r['combinations'])==27 and len(r['poses'])==18 and len(r['sweeps'])==15
assert all(c['finite_vertices'] and c['collapsed_triangles_below_1e-12_m2']==0 for c in r['combinations']+r['poses'])
assert all(c['samples']==101 and c['max_forbidden_displacement_m']<1e-7 for c in r['sweeps'])
old=json.loads((p.parent/'G2-PROTO/deformation-results.json').read_text())
def minimum(cases):return min(a['minimum_signed_clearance_m'] for c in cases for a in c['cloth_vertex_signed_clearance'])
assert minimum(r['combinations'])>=-.0005
assert minimum(r['poses'])>minimum(old['poses'])
for axis,shape in r['shape_probes'].items():
 assert shape['protected_head_hands_feet_unchanged']
 assert abs(shape['maximum_displacement_m']-old['shape_probes'][axis]['maximum_displacement_m'])<1e-6
assert u['avatarValid'] and u['avatarHuman'] and u['verifiedBodyCombinations']==27
assert u['synchronizedSweepSamples']==1515 and u['synchronizedMeshBakes']==4629
assert u['sourceSha256']==hashlib.sha256((p/'woman_g2_body.fbx').read_bytes()).hexdigest()
assert len(read('surface-crossings.json')['cases'])==45
assert len(list((p/'evidence/combinations').glob('*.png')))==108
assert len(list((p/'evidence/sweeps').glob('*.png')))==315
assert not read('protected-files-check.json')['changed']
assert hashlib.sha256((p/'woman_g2_body.fbx').read_bytes()).digest()==hashlib.sha256((p.parents[3]/'Assets/G2BodyPrototype/woman_g2_body.fbx').read_bytes()).digest()
for path,expected in read('artifact-manifest.json').items():
 assert hashlib.sha256((p/path).read_bytes()).hexdigest()==expected,path
print('G2_CORR_DELIVERY_CHECKS_PASSED; artistic acceptance and pose defects remain pending')
