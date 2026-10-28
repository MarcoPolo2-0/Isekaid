import bpy, json, ast, hashlib
from pathlib import Path
from mathutils import Vector,Quaternion
from mathutils.bvhtree import BVHTree
from mathutils.geometry import intersect_ray_tri
import math
p=Path(__file__).resolve().parents[1]
import sys
baseline='--baseline' in sys.argv
asset=p.parent/'G2-PROTO' if baseline else p
bpy.ops.wm.open_mainfile(filepath=str(p.parent/'G2-PROTO'/'woman_g2_body.blend'))
old_body=bpy.data.objects['G2_PROTO_WomanBody']
old_faces={tuple(sorted(f.vertices)) for f in old_body.data.polygons}
old_points=[v.co.copy() for v in old_body.data.vertices]
bpy.ops.wm.open_mainfile(filepath=str(asset/'woman_g2_body.blend'))
new_body=bpy.data.objects['G2_PROTO_WomanBody']
assert {tuple(sorted(f.vertices)) for f in new_body.data.polygons}==old_faces
protected=[i for i,v in enumerate(old_points) if v.z>1.65 or v.z<.4 or abs(v.x)>.34]
protected_difference=max((new_body.data.vertices[i].co-old_points[i]).length for i in protected)
assert protected_difference<1e-7, str(protected_difference)
(p/'topology-protection.json').write_text(json.dumps({'same_indexed_faces':True,'body_vertices':len(new_body.data.vertices),'faces':len(old_faces),'protected_vertices':len(protected),'maximum_protected_neutral_difference_m':protected_difference},indent=2))

body=bpy.data.objects['G2_PROTO_WomanBody'];top=bpy.data.objects['G2_PROTO_TorsoShell'];pants=bpy.data.objects['G2_PROTO_LegShell'];rig=bpy.data.objects['G2_PROTO_AutoWeightRig']
axes=('Chest','Hips','Waist')
poses=next(ast.literal_eval(n.value) for n in ast.parse((p/'scripts/build_corr.py').read_text()).body if isinstance(n,ast.Assign) and any(isinstance(t,ast.Name) and t.id=='poses' for t in n.targets))
def geometry(obj):
 e=obj.evaluated_get(bpy.context.evaluated_depsgraph_get());m=e.to_mesh();m.calc_loop_triangles()
 v=[e.matrix_world@x.co for x in m.vertices];f=[tuple(t.vertices) for t in m.loop_triangles];e.to_mesh_clear();return v,f
def crosses(a,b):
 for tri,other in ((a,b),(b,a)):
  for i in range(3):
   origin=tri[i];segment=tri[(i+1)%3]-origin
   if segment.length<1e-9:continue
   hit=intersect_ray_tri(*other,segment.normalized(),origin,True)
   if hit is not None:
    t=(hit-origin).dot(segment)/segment.length_squared
    if 1e-6<t<1-1e-6:return True
 return False
checks=[]
for pose,values in [('rest',c['values_minus1_to_plus1']) for c in json.loads((p/'deformation-results.json').read_text())['combinations']]+[(c['pose'],c['values_minus1_to_plus1']) for c in json.loads((p/'deformation-results.json').read_text())['poses']]:
 for obj in (body,top,pants):
  for axis,value in zip(axes,values):
   obj.data.shape_keys.key_blocks[axis+'Minus'].value=max(0,-value);obj.data.shape_keys.key_blocks[axis+'Plus'].value=max(0,value)
 for bone in rig.pose.bones:bone.rotation_mode='QUATERNION';bone.rotation_quaternion=Quaternion()
 if pose!='rest':
  for name,axis,degrees in poses[pose]:
   bone=rig.pose.bones[name];basis=bone.bone.matrix_local.to_quaternion();bone.rotation_quaternion=basis.inverted()@Quaternion(Vector(axis),math.radians(degrees))@basis
 bpy.context.view_layer.update();bv,bf=geometry(body);bt=BVHTree.FromPolygons(bv,bf,all_triangles=True)
 row={'pose':pose,'values':values,'cloth':[]}
 for obj in (top,pants):
  cv,cf=geometry(obj);ct=BVHTree.FromPolygons(cv,cf,all_triangles=True);pairs=bt.overlap(ct);hits=[]
  for a,b in pairs:
   if crosses([bv[i] for i in bf[a]],[cv[i] for i in cf[b]]):hits.append([a,b])
  row['cloth'].append({'name':obj.name,'candidate_triangle_pairs':len(pairs),'edge_triangle_crossings':len(hits),'first_pairs':hits[:20]})
 checks.append(row)
(p/('surface-crossings-baseline.json' if baseline else 'surface-crossings.json')).write_text(json.dumps({'method':'BVH candidate pairs followed by strict segment-triangle tests in both directions; excludes coplanar and endpoint-only contact; no continuous collision proof','cases':checks},indent=2))
if baseline: raise SystemExit(0)
# Reopen/export checks include the basis synchronization that previously failed.
checks={'native':[],'fbx':[]}
for kind in ('native','fbx'):
 if kind=='fbx':
  bpy.ops.wm.read_factory_settings(use_empty=True);bpy.ops.import_scene.fbx(filepath=str(p/'woman_g2_body.fbx'),use_anim=False)
 for obj in bpy.context.scene.objects:
  if obj.type!='MESH':continue
  keys=obj.data.shape_keys.key_blocks
  difference=max((v.co-b.co).length for v,b in zip(obj.data.vertices,keys['Basis'].data))
  assert difference<1e-6
  assert len(keys)==7
  checks[kind].append({'name':obj.name,'vertices':len(obj.data.vertices),'basis_max_difference_m':difference,'keys':list(keys.keys())})
(p/'roundtrip-results.json').write_text(json.dumps(checks,indent=2))
