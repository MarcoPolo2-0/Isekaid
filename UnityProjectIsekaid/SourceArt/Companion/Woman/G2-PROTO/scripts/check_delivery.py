import hashlib, json
from pathlib import Path
p=Path(__file__).resolve().parents[1]
r=json.loads((p/'deformation-results.json').read_text());u=json.loads((p/'evidence/unity/unity-import-results.json').read_text())
assert hashlib.sha256((p/'input/woman.fbx').read_bytes()).hexdigest()=='674c07ff612577a1a5b7e7ee124202722fa5eb0f68bdc01c820a875b5bef1d87'
assert len(r['combinations'])==27 and len(r['poses'])==18
assert all(c['finite_vertices'] and c['collapsed_triangles_below_1e-12_m2']==0 for c in r['combinations']+r['poses'])
assert all(a['negative_signed_clearance_below_minus_0_5mm']==0 for c in r['combinations'] for a in c['cloth_vertex_signed_clearance'])
assert u['avatarValid'] and u['avatarHuman'] and u['verifiedBodyCombinations']==27
legs=next(m for m in u['meshes'] if 'LegShell' in m['name'])
assert legs['deformationMaxDistances'][:2]==[0.0,0.0]
assert len(list((p/'evidence/combinations').glob('*.png')))==108
assert hashlib.sha256((p/'woman_g2_body.fbx').read_bytes()).digest()==hashlib.sha256((p.parents[3]/'Assets/G2BodyPrototype/woman_g2_body.fbx').read_bytes()).digest()
assert not json.loads((p/'protected-files-check.json').read_text())['changed']
print('G2_DELIVERY_CHECKS_PASSED; visual acceptance and pose clothing corrections pending')
