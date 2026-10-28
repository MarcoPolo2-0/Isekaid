import bpy
import bmesh
import hashlib
import itertools
import json
import math
from pathlib import Path
from mathutils import Matrix, Vector, Quaternion
from mathutils.bvhtree import BVHTree
from mathutils.kdtree import KDTree

OUTPUT = Path(__file__).resolve().parents[1]
ROOT = Path(__file__).resolve().parents[4]
SOURCE = ROOT / 'assets' / 'woman.fbx'
SOURCE_HASH = hashlib.sha256(SOURCE.read_bytes()).hexdigest()
OUTPUT.joinpath('evidence', 'combinations').mkdir(parents=True, exist_ok=True)
OUTPUT.joinpath('evidence', 'poses').mkdir(exist_ok=True)
bpy.ops.wm.open_mainfile(filepath=str(OUTPUT / 'woman_original_inspection.blend'))
scene = bpy.context.scene
body = next(obj for obj in scene.objects if obj.type == 'MESH')
body.name = 'AUDIT_ONLY_WomanBody'
old_points = [body.matrix_world @ vertex.co for vertex in body.data.vertices]
ground = min(point.z for point in old_points)
body.data.transform(Matrix.Translation((0, 0, -ground * 0.01)) @ Matrix.Scale(0.01, 4) @ body.matrix_world)
body.matrix_world = Matrix.Identity(4)
body.data.update()
base = [vertex.co.copy() for vertex in body.data.vertices]
height = max(vertex.z for vertex in base)
report = {
    'source_sha256': SOURCE_HASH, 'blender': bpy.app.version_string,
    'scope': 'Temporary diagnostic shapes, shell samples and automatic-weight rig. Not production assets or approved art ranges.',
    'normalization': {'uniform_factor': 0.01, 'height_m': height, 'ground_z_m': 0},
    'shape_probes': {}, 'combinations': [], 'poses': [],
}

kd = KDTree(len(base))
for i, vertex in enumerate(base):
    kd.insert(vertex, i)
kd.balance()
symmetry_errors = [kd.find(Vector((-vertex.x, vertex.y, vertex.z)))[2] for vertex in base]
report['mirror_symmetry'] = {'maximum_distance_m': max(symmetry_errors), 'mean_distance_m': sum(symmetry_errors) / len(base)}

def bell(value):
    if abs(value) >= 1:
        return 0.0
    return (1 - value * value) ** 2

def smooth(value):
    value = max(0.0, min(1.0, value))
    return value * value * (3 - 2 * value)

def delta(vertex, axis):
    x, y, z = vertex
    sign_x = 1 if x >= 0 else -1
    if axis == 'Chest':
        weight = bell((z - 1.43) / 0.17) * bell(x / 0.31) * smooth((-y - 0.01) / 0.07)
        return Vector((x * 0.08 * weight, -0.035 * weight, 0))
    if axis == 'Hips':
        weight = bell((z - 1.00) / 0.24) * smooth((abs(x) - 0.04) / 0.10)
        return Vector((sign_x * 0.035 * weight, y * 0.10 * weight, 0))
    weight = bell((z - 1.23) / 0.13) * bell(x / 0.34)
    return Vector((x * 0.16 * weight, y * 0.12 * weight, 0))

axes = ('Chest', 'Hips', 'Waist')
body.shape_key_add(name='Basis')
for axis in axes:
    displacement = [delta(vertex, axis) for vertex in base]
    affected = [i for i, movement in enumerate(displacement) if movement.length > 1e-9]
    report['shape_probes'][axis] = {
        'affected_vertices': len(affected), 'maximum_displacement_m': max(movement.length for movement in displacement),
        'affected_z_range_m': [min(base[i].z for i in affected), max(base[i].z for i in affected)],
        'protected_head_hands_feet_unchanged': all(movement.length < 1e-9 for vertex, movement in zip(base, displacement)
                                                  if vertex.z > 1.65 or vertex.z < 0.4 or abs(vertex.x) > 0.34),
    }
    for suffix, sign in (('Minus', -1), ('Plus', 1)):
        key = body.shape_key_add(name=axis + suffix)
        for item, vertex, movement in zip(key.data, base, displacement):
            item.co = vertex + movement * sign

def sample_cloth(name, low, high, color):
    face_indices = [list(face.vertices) for face in body.data.polygons
                    if all(low <= base[index].z <= high and abs(base[index].x) < 0.38 for index in face.vertices)]
    source_ids = sorted(set(index for face in face_indices for index in face))
    mapping = {source: target for target, source in enumerate(source_ids)}
    vertices = [base[index] + body.data.vertices[index].normal * 0.009 for index in source_ids]
    mesh = bpy.data.meshes.new(name)
    mesh.from_pydata(vertices, [], [[mapping[index] for index in face] for face in face_indices])
    mesh.update()
    obj = bpy.data.objects.new(name, mesh)
    scene.collection.objects.link(obj)
    material = bpy.data.materials.new(name + '_Clay')
    material.diffuse_color = (*color, 1)
    obj.data.materials.append(material)
    for polygon in mesh.polygons:
        polygon.use_smooth = True
    obj.shape_key_add(name='Basis')
    for axis in axes:
        for suffix, sign in (('Minus', -1), ('Plus', 1)):
            key = obj.shape_key_add(name=axis + suffix)
            for target, source_index in enumerate(source_ids):
                key.data[target].co = vertices[target] + delta(base[source_index], axis) * sign
    return obj, source_ids

top, top_ids = sample_cloth('AUDIT_ONLY_TorsoShell', 1.14, 1.53, (0.21, 0.41, 0.55))
pants, pants_ids = sample_cloth('AUDIT_ONLY_LegShell', 0.13, 1.14, (0.25, 0.33, 0.42))
clothes = [top, pants]
report['cloth_samples'] = [
    {'name': obj.name, 'vertices': len(ids), 'faces': len(obj.data.polygons), 'method': 'Body-correspondent surface, 9 mm rest normal offset; intentionally open hems; no garment design or thickness.'}
    for obj, ids in ((top, top_ids), (pants, pants_ids))
]

def set_shapes(values):
    for obj in (body, top, pants):
        for axis, value in zip(axes, values):
            obj.data.shape_keys.key_blocks[axis + 'Minus'].value = max(0, -value)
            obj.data.shape_keys.key_blocks[axis + 'Plus'].value = max(0, value)
    bpy.context.view_layer.update()

def evaluated_geometry(obj):
    evaluated = obj.evaluated_get(bpy.context.evaluated_depsgraph_get())
    mesh = evaluated.to_mesh()
    vertices = [evaluated.matrix_world @ vertex.co for vertex in mesh.vertices]
    faces = [list(face.vertices) for face in mesh.polygons]
    mesh.calc_loop_triangles()
    areas = [triangle.area for triangle in mesh.loop_triangles]
    evaluated.to_mesh_clear()
    return vertices, faces, areas

def geometry_metrics():
    vertices, faces, areas = evaluated_geometry(body)
    bvh = BVHTree.FromPolygons(vertices, faces, all_triangles=False)
    clothing_metrics = []
    for obj in clothes:
        points, _, _ = evaluated_geometry(obj)
        distances = []
        for point in points:
            location, normal, _, distance = bvh.find_nearest(point)
            distances.append((point - location).dot(normal))
        clothing_metrics.append({'name': obj.name, 'sampled_vertices': len(points),
                                 'negative_signed_clearance_below_minus_0_5mm': sum(d < -0.0005 for d in distances),
                                 'minimum_signed_clearance_m': min(distances)})
    stretch = [(vertices[edge.vertices[0]] - vertices[edge.vertices[1]]).length /
               max((base[edge.vertices[0]] - base[edge.vertices[1]]).length, 1e-12) for edge in body.data.edges]
    stretch.sort()
    return {'finite_vertices': all(math.isfinite(component) for point in vertices for component in point),
            'collapsed_triangles_below_1e-12_m2': sum(area < 1e-12 for area in areas),
            'edge_stretch_max': stretch[-1], 'edge_stretch_p99': stretch[int(len(stretch) * .99)],
            'cloth_vertex_signed_clearance': clothing_metrics}

camera = scene.camera
scene.display.shading.color_type = 'MATERIAL'
body.data.materials[0].diffuse_color = (0.62, 0.65, 0.69, 1)

def render(name, direction=(0, -1, 0), target=None, scale=None, resolution=(640, 800)):
    target = Vector((0, 0, height / 2)) if target is None else Vector(target)
    camera.location = target + Vector(direction).normalized() * 5
    camera.rotation_euler = (target - camera.location).to_track_quat('-Z', 'Y').to_euler()
    camera.data.ortho_scale = scale or height * 1.17
    scene.render.resolution_x, scene.render.resolution_y = resolution
    scene.render.filepath = str(OUTPUT / 'evidence' / (name + '.png'))
    bpy.ops.render.render(write_still=True)

for values in itertools.product((-1, 0, 1), repeat=3):
    set_shapes(values)
    report['combinations'].append({'values_minus1_to_plus1': list(values), **geometry_metrics()})
    code = '_'.join(str(value + 1) for value in values)
    render('combinations/' + code, resolution=(384, 480))
set_shapes((0, 0, 0))
render('07-neutral-cloth-shells')
render('08-neutral-profile-shells', (1, 0, 0))

armature = bpy.data.armatures.new('AUDIT_ONLY_AutoWeightRig')
rig = bpy.data.objects.new('AUDIT_ONLY_AutoWeightRig', armature)
scene.collection.objects.link(rig)
bpy.context.view_layer.objects.active = rig
rig.select_set(True)
bpy.ops.object.mode_set(mode='EDIT')
definitions = [
    ('Hips', (0, 0, 1.00), (0, 0, 1.14), None),
    ('Spine', (0, 0, 1.14), (0, 0, 1.34), 'Hips'),
    ('Chest', (0, 0, 1.34), (0, 0, 1.53), 'Spine'),
    ('Neck', (0, 0, 1.53), (0, 0, 1.65), 'Chest'),
    ('Head', (0, 0, 1.65), (0, 0, 1.85), 'Neck'),
]
for side, sign in (('Left', 1), ('Right', -1)):
    definitions += [
        (side + 'Shoulder', (0, 0, 1.53), (sign * .21, 0, 1.52), 'Chest'),
        (side + 'UpperArm', (sign * .21, 0, 1.52), (sign * .48, 0, 1.49), side + 'Shoulder'),
        (side + 'LowerArm', (sign * .48, 0, 1.49), (sign * .73, 0, 1.49), side + 'UpperArm'),
        (side + 'Hand', (sign * .73, 0, 1.49), (sign * .85, 0, 1.49), side + 'LowerArm'),
        (side + 'UpperLeg', (sign * .115, 0, 1.02), (sign * .105, -.005, .56), 'Hips'),
        (side + 'LowerLeg', (sign * .105, -.005, .56), (sign * .10, .01, .14), side + 'UpperLeg'),
        (side + 'Foot', (sign * .10, .01, .14), (sign * .10, -.16, .055), side + 'LowerLeg'),
    ]
for name, head, tail, parent in definitions:
    bone = armature.edit_bones.new(name)
    bone.head, bone.tail = head, tail
    if parent:
        bone.parent = armature.edit_bones[parent]
bpy.ops.object.mode_set(mode='OBJECT')
bpy.ops.object.select_all(action='DESELECT')
body.select_set(True)
rig.select_set(True)
bpy.context.view_layer.objects.active = rig
try:
    bpy.ops.object.parent_set(type='ARMATURE_AUTO')
    report['automatic_weights'] = {'operator_completed': True}
except RuntimeError as error:
    report['automatic_weights'] = {'operator_completed': False, 'error': str(error)}
weight_totals = [sum(group.weight for group in vertex.groups) for vertex in body.data.vertices]
report['automatic_weights'].update({'bones': len(rig.data.bones), 'vertex_groups': len(body.vertex_groups),
    'unweighted_vertices': sum(total < 1e-6 for total in weight_totals), 'minimum_total_weight': min(weight_totals)})

for obj, ids in ((top, top_ids), (pants, pants_ids)):
    for group in body.vertex_groups:
        obj.vertex_groups.new(name=group.name)
    for target_index, source_index in enumerate(ids):
        for source_group in body.data.vertices[source_index].groups:
            obj.vertex_groups[source_group.group].add([target_index], source_group.weight, 'REPLACE')
    obj.parent = rig
    modifier = obj.modifiers.new('AuditArmature', 'ARMATURE')
    modifier.object = rig

def reset_pose():
    for bone in rig.pose.bones:
        bone.rotation_mode = 'QUATERNION'
        bone.rotation_quaternion = Quaternion()
    bpy.context.view_layer.update()

def rotate(name, world_axis, degrees):
    bone = rig.pose.bones[name]
    basis = bone.bone.matrix_local.to_quaternion()
    bone.rotation_quaternion = basis.inverted() @ Quaternion(Vector(world_axis), math.radians(degrees)) @ basis

poses = {
    'arms_up': [('LeftUpperArm', (0, 1, 0), -65), ('RightUpperArm', (0, 1, 0), 65)],
    'arms_forward': [('LeftUpperArm', (0, 0, 1), -75), ('RightUpperArm', (0, 0, 1), 75)],
    'torso_twist': [('Spine', (0, 0, 1), 35)],
    'seated': [('LeftUpperLeg', (1, 0, 0), -80), ('RightUpperLeg', (1, 0, 0), -80),
               ('LeftLowerLeg', (1, 0, 0), 85), ('RightLowerLeg', (1, 0, 0), 85)],
    'deep_knee': [('LeftUpperLeg', (1, 0, 0), -65), ('RightUpperLeg', (1, 0, 0), -65),
                  ('LeftLowerLeg', (1, 0, 0), 105), ('RightLowerLeg', (1, 0, 0), 105)],
    'stride': [('LeftUpperLeg', (1, 0, 0), -35), ('RightUpperLeg', (1, 0, 0), 35)],
}
for values in ((0, 0, 0), (-1, -1, -1), (1, 1, 1)):
    set_shapes(values)
    for name, rotations in poses.items():
        reset_pose()
        for bone, axis, degrees in rotations:
            rotate(bone, axis, degrees)
        bpy.context.view_layer.update()
        code = name + '_' + str(values[0] + 1)
        report['poses'].append({'pose': name, 'values_minus1_to_plus1': list(values), **geometry_metrics()})
        render('poses/' + code, (1, -2, .10), target=(0, 0, 1.10), scale=2.75)
reset_pose()
set_shapes((0, 0, 0))
render('09-probe-rest-rig-clothes')
top.hide_render = pants.hide_render = True
wire = body.copy()
wire.data = body.data.copy()
scene.collection.objects.link(wire)
wire.data.materials.clear()
ink = bpy.data.materials.new('AuditTopologyInk')
ink.diffuse_color = (.02, .025, .03, 1)
wire.data.materials.append(ink)
for polygon in wire.data.polygons:
    polygon.material_index = 0
modifier = wire.modifiers.new('AuditTopologyEdges', 'WIREFRAME')
modifier.thickness = .0007
modifier.offset = 1
render('10-face-topology', target=(0, -.015, 1.75), scale=.42, resolution=(1000, 1000))
render('11-shoulder-topology', target=(0, 0, 1.46), scale=.65, resolution=(1000, 1000))
render('12-pelvis-knee-topology', target=(0, 0, .93), scale=1.05, resolution=(1000, 1000))
bpy.data.objects.remove(wire, do_unlink=True)
top.hide_render = pants.hide_render = False
scene.render.filepath = ''
bpy.ops.wm.save_as_mainfile(filepath=str(OUTPUT / 'woman_deformation_probe.blend'))
bpy.ops.object.select_all(action='DESELECT')
for obj in (body, top, pants, rig):
    obj.select_set(True)
bpy.context.view_layer.objects.active = rig
bpy.ops.export_scene.fbx(filepath=str(OUTPUT / 'woman_deformation_probe.fbx'), use_selection=True,
    object_types={'MESH', 'ARMATURE'}, use_mesh_modifiers=False, add_leaf_bones=False,
    bake_anim=False, axis_forward='-Z', axis_up='Y', apply_unit_scale=True)
report['original_hash_preserved'] = hashlib.sha256(SOURCE.read_bytes()).hexdigest() == SOURCE_HASH
report['export'] = {'file': 'woman_deformation_probe.fbx', 'sha256': hashlib.sha256((OUTPUT / 'woman_deformation_probe.fbx').read_bytes()).hexdigest()}
(OUTPUT / 'deformation-results.json').write_text(json.dumps(report, indent=2), encoding='utf-8')
assert report['original_hash_preserved']
assert all(case['finite_vertices'] and case['collapsed_triangles_below_1e-12_m2'] == 0 for case in report['combinations'])
print('ISEK_001_DEFORMATION_PROBE_COMPLETE')
