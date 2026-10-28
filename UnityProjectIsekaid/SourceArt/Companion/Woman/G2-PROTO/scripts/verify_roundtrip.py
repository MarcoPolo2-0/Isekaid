import bpy
import hashlib
import json
from pathlib import Path
from mathutils import Vector

OUTPUT = Path(__file__).resolve().parents[1]
ROOT = OUTPUT
source = OUTPUT / 'input' / 'woman.fbx'
checks = {'blender': bpy.app.version_string, 'source_sha256': hashlib.sha256(source.read_bytes()).hexdigest()}
bpy.ops.wm.open_mainfile(filepath=str(OUTPUT / 'woman_g2_body.blend'))
body = bpy.data.objects['G2_PROTO_WomanBody']
checks['native_reopen'] = {'body_vertices': len(body.data.vertices), 'body_shape_keys': list(body.data.shape_keys.key_blocks.keys()),
    'armature_bones': len(bpy.data.objects['G2_PROTO_AutoWeightRig'].data.bones)}
assert checks['native_reopen']['body_vertices'] == 11022
assert len(checks['native_reopen']['body_shape_keys']) == 7
assert checks['native_reopen']['armature_bones'] == 19
camera = bpy.context.scene.camera
target = Vector((0, 0, .9471))
camera.location = target + Vector((4, 0, 0))
camera.rotation_euler = (target - camera.location).to_track_quat('-Z', 'Y').to_euler()
camera.data.ortho_scale = 2.22
scene = bpy.context.scene
scene.render.resolution_x = 640
scene.render.resolution_y = 800
for value in (-1, 0, 1):
    for obj in [obj for obj in scene.objects if obj.type == 'MESH']:
        for name in ('ChestMinus', 'ChestPlus'):
            obj.data.shape_keys.key_blocks[name].value = max(0, value if name.endswith('Plus') else -value)
    bpy.context.view_layer.update()
    scene.render.filepath = str(OUTPUT / 'evidence' / ('14-chest-profile-' + str(value + 1) + '.png'))
    bpy.ops.render.render(write_still=True)
bpy.ops.wm.read_factory_settings(use_empty=True)
bpy.ops.import_scene.fbx(filepath=str(OUTPUT / 'woman_g2_body.fbx'), use_anim=False)
checks['fbx_reimport'] = []
for obj in bpy.context.scene.objects:
    if obj.type != 'MESH':
        continue
    checks['fbx_reimport'].append({'name': obj.name, 'vertices': len(obj.data.vertices),
        'shape_keys': list(obj.data.shape_keys.key_blocks.keys()) if obj.data.shape_keys else [],
        'vertex_groups': len(obj.vertex_groups)})
    assert obj.data.shape_keys is not None and len(obj.data.shape_keys.key_blocks) == 7
checks['fbx_reimport_armature_bones'] = [len(obj.data.bones) for obj in bpy.context.scene.objects if obj.type == 'ARMATURE']
assert checks['fbx_reimport_armature_bones'] == [19]
checks['source_original_unchanged'] = hashlib.sha256(source.read_bytes()).hexdigest() == checks['source_sha256']
(OUTPUT / 'roundtrip-results.json').write_text(json.dumps(checks, indent=2), encoding='utf8')
print('ISEK_001_NATIVE_REOPEN_AND_FBX_ROUNDTRIP_PASSED')
