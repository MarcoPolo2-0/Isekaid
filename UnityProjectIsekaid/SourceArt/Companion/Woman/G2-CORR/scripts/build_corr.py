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
ROOT = OUTPUT
SOURCE = OUTPUT.parent / 'G2-PROTO' / 'input' / 'woman.fbx'
SOURCE_HASH = hashlib.sha256(SOURCE.read_bytes()).hexdigest()
OUTPUT.joinpath('evidence', 'combinations').mkdir(parents=True, exist_ok=True)
OUTPUT.joinpath('evidence', 'poses').mkdir(exist_ok=True)
bpy.ops.wm.open_mainfile(filepath=str(OUTPUT.parent / 'G2-PROTO' / 'input' / 'woman_original_inspection.blend'))
scene = bpy.context.scene
body = next(obj for obj in scene.objects if obj.type == 'MESH')
body.name = 'G2_PROTO_WomanBody'
old_points = [body.matrix_world @ vertex.co for vertex in body.data.vertices]
ground = min(point.z for point in old_points)
body.data.transform(Matrix.Translation((0, 0, -ground * 0.01)) @ Matrix.Scale(0.01, 4) @ body.matrix_world)
body.matrix_world = Matrix.Identity(4)
body.data.update()
original_normalized = [vertex.co.copy() for vertex in body.data.vertices]
def neutral_bell(value):
    return max(0, 1 - value * value) ** 3 if abs(value) < 1 else 0
def ramp(t):
    t=max(0,min(1,t))
    return t*t*(3-2*t)
width_sections=[(.70,.15),(.82,.175),(.95,.195),(1.04,.185),(1.14,.155),(1.24,.140),(1.34,.160),(1.43,.187)]
def section_width(z):
    for i in range(len(width_sections)-1):
        a,wa=width_sections[i];b,wb=width_sections[i+1]
        if a<=z<=b:
            t=(z-a)/(b-a)
            m0=(wb-width_sections[max(0,i-1)][1])/(b-width_sections[max(0,i-1)][0])
            m1=(width_sections[min(len(width_sections)-1,i+2)][1]-wa)/(width_sections[min(len(width_sections)-1,i+2)][0]-a)
            return (2*t**3-3*t*t+1)*wa+(t**3-2*t*t+t)*(b-a)*m0+(-2*t**3+3*t*t)*wb+(t**3-t*t)*(b-a)*m1
    return width_sections[0 if z<.70 else -1][1]
# Measure actual cross-sections before redistributing width on the existing vertices.
source_widths=[]
for j in range(147):
    z=.70+j*.005
    cuts=[]
    for edge in body.data.edges:
        a,b=(original_normalized[i] for i in edge.vertices)
        if min(a.z,b.z)<=z<=max(a.z,b.z) and abs(b.z-a.z)>1e-8:
            x=a.x+(b.x-a.x)*(z-a.z)/(b.z-a.z)
            if abs(x)<.34: cuts.append(abs(x))
    source_widths.append(max(cuts) if cuts else .2)
for vertex in body.data.vertices:
    x,y,z=vertex.co
    front=ramp((-y-.015)/.065)
    chest=ramp((z-1.20)/.14)*ramp((1.64-z)/.12)*front
    chest_target_y=-.045+(y+.045)*.35
    vertex.co.y=y*(1-chest)+chest_target_y*chest
    vertex.co.z=z
    if .70<z<1.43:
        f=(z-.70)/.005;i=min(145,int(f));t=f-i
        measured=source_widths[i]*(1-t)+source_widths[i+1]*t
        blend=ramp((z-.70)/.10)*ramp((1.43-z)/.06)
        vertex.co.x=x*((1-blend)+blend*section_width(z)/measured)
        vertex.co.y*=1-.15*neutral_bell((z-1.00)/.31)+.12*neutral_bell((z-1.24)/.18)
    if .90<z<1.64:
        envelope=ramp((z-.90)/.16)*ramp((1.64-z)/.13)*ramp((-y-.005)/.035)
        half_width=section_width(min(z,1.43))
        wall=-.070*math.sqrt(max(.08,1-(vertex.co.x/(half_width*1.1))**2))
        breast=.048*math.exp(-((abs(vertex.co.x)-.080)/.070)**2-((z-1.435)/.083)**2)
        vertex.co.y=vertex.co.y*(1-envelope)+(wall-breast)*envelope
# Smooth only the edited torso and pelvis transition, preserving all vertex IDs.
adjacency=[[] for _ in body.data.vertices]
for edge in body.data.edges:
    a,b=edge.vertices
    adjacency[a].append(b);adjacency[b].append(a)
for iteration in range(80):
    positions=[v.co.copy() for v in body.data.vertices]
    for v in body.data.vertices:
        x,y,z=original_normalized[v.index]
        weight=.48*ramp((z-.78)/.22)*ramp((1.64-z)/.13)*ramp((.34-abs(x))/.09)
        if iteration>=20 and z<1.15: weight=0
        if weight and adjacency[v.index]:
            average=sum((positions[j] for j in adjacency[v.index]),Vector())/len(adjacency[v.index])
            v.co=positions[v.index].lerp(average,weight)
for vertex in body.data.vertices:
    x,y,z=vertex.co
    front=ramp((-y-.005)/.025)
    vertex.co.y-=.035*math.exp(-((abs(x)-.075)/.080)**2-((z-1.435)/.090)**2)*front
for vertex,original in zip(body.data.vertices,original_normalized):
    if original.z>1.65 or original.z<.4 or abs(original.x)>.34: vertex.co=original
bm = bmesh.new()
bm.from_mesh(body.data)
bmesh.ops.recalc_face_normals(bm, faces=bm.faces)
bm.to_mesh(body.data)
bm.free()
body.data.normals_split_custom_set([(0,0,0)] * len(body.data.loops))
body.data.update()
base = [vertex.co.copy() for vertex in body.data.vertices]
height = max(vertex.z for vertex in base)
report = {
    'source_sha256': SOURCE_HASH, 'blender': bpy.app.version_string,
    'scope': 'G2-CORR DEC-003; neutral and ranges pending artistic approval; diagnostic rig and shells only.',
    'normalization': {'uniform_factor': 0.01, 'height_m': height, 'ground_z_m': 0},
    'shape_probes': {}, 'combinations': [], 'poses': [], 'neutral_width_sections_m': width_sections,
}

kd = KDTree(len(base))
for i, vertex in enumerate(base):
    kd.insert(vertex, i)
kd.balance()
symmetry_errors = [kd.find(Vector((-vertex.x, vertex.y, vertex.z)))[2] for vertex in base]
report['neutral_max_movement_m'] = max((a-b).length for a,b in zip(base, original_normalized))
report['mirror_symmetry'] = {'maximum_distance_m': max(symmetry_errors), 'mean_distance_m': sum(symmetry_errors) / len(base)}

def bell(value):
    if abs(value) >= 1:
        return 0.0
    return (1 - value * value) ** 3

def smooth(value):
    value = max(0.0, min(1.0, value))
    return value * value * (3 - 2 * value)

def raw_delta(vertex, axis):
    x,y,z=vertex
    if axis=='Chest':
        w=bell((z-1.45)/.15)*bell(x/.31)*smooth((-y-.005)/.05)
        return Vector((x*.06*w,-.025*w,0))
    if axis=='Hips':
        w=bell((z-.97)/.15)*smooth((abs(x)-.035)/.09)
        return Vector((x*.12*w,y*.08*w,0))
    w=bell((z-1.21)/.09)*bell(x/.34)
    return Vector((x*.12*w,y*.10*w,0))
# Retain the previous maximum amplitudes while recalculating their spatial support.
limits={'Chest':.0249258750681,'Hips':.0269859563323,'Waist':.0184852977410}
report['axis_amplitudes_m']=limits
factors={axis:limits[axis]/max(raw_delta(v,axis).length for v in base) for axis in limits}
def delta(vertex, axis):
    return raw_delta(vertex,axis)*factors[axis]

axes = ('Chest', 'Hips', 'Waist')
body.shape_key_add(name='Basis')
preview_camera=scene.camera
for view,direction in {'front':(0,-1,0),'side':(1,0,0),'back':(0,1,0),'threequarter':(1,-2,.1)}.items():
    target=Vector((0,0,height/2))
    preview_camera.location=target+Vector(direction).normalized()*5
    preview_camera.rotation_euler=(target-preview_camera.location).to_track_quat('-Z','Y').to_euler()
    preview_camera.data.ortho_scale=height*1.17
    scene.render.resolution_x=640;scene.render.resolution_y=800
    scene.render.filepath=str(OUTPUT/'evidence'/('preview-'+view+'.png'))
    bpy.ops.render.render(write_still=True)
if '--preview-only' in __import__('sys').argv:
    bpy.ops.wm.save_as_mainfile(filepath=str(OUTPUT/'neutral_preview.blend'))
    raise SystemExit(0)
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
    neutral_mesh = bpy.data.meshes.new('NeutralNormals')
    neutral_mesh.from_pydata(base, [], [list(face.vertices) for face in body.data.polygons])
    neutral_mesh.update()
    vertices = [base[index] + neutral_mesh.vertices[index].normal * .016 for index in source_ids]
    bpy.data.meshes.remove(neutral_mesh)
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
            shaped = [point + delta(point, axis) * sign for point in base]
            temp = bpy.data.meshes.new('NormalProbe')
            temp.from_pydata(shaped, [], [list(face.vertices) for face in body.data.polygons])
            temp.update()
            for target, source_index in enumerate(source_ids):
                key.data[target].co = shaped[source_index] + temp.vertices[source_index].normal * .016
            bpy.data.meshes.remove(temp)
    return obj, source_ids

top, top_ids = sample_cloth('G2_PROTO_TorsoShell', 1.14, 1.53, (0.21, 0.41, 0.55))
pants, pants_ids = sample_cloth('G2_PROTO_LegShell', 0.13, 1.14, (0.25, 0.33, 0.42))
clothes = [top, pants]
report['cloth_samples'] = [
    {'name': obj.name, 'vertices': len(ids), 'faces': len(obj.data.polygons), 'method': 'Body-correspondent surface, 16 mm recalculated per-shape normal offset; intentionally open hems; no garment design or thickness.'}
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
        points, cloth_faces, _ = evaluated_geometry(obj)
        distances = []
        for point in points:
            location, normal, _, distance = bvh.find_nearest(point)
            distances.append((point - location).dot(normal))
        clothing_metrics.append({'name': obj.name, 'sampled_vertices': len(points),
                                 'negative_signed_clearance_below_minus_0_5mm': sum(d < -0.0005 for d in distances),
                                 'minimum_signed_clearance_m': min(distances),
                                 'surface_overlap_face_pairs': len(bvh.overlap(BVHTree.FromPolygons(points,cloth_faces)))})
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
    if '--diagnostic-only' in __import__('sys').argv and not name.startswith(('poses/','neutral-clothes-')): return
    target = Vector((0, 0, height / 2)) if target is None else Vector(target)
    camera.location = target + Vector(direction).normalized() * 5
    camera.rotation_euler = (target - camera.location).to_track_quat('-Z', 'Y').to_euler()
    camera.data.ortho_scale = scale or height * 1.17
    scene.render.resolution_x, scene.render.resolution_y = resolution
    scene.render.filepath = str(OUTPUT / 'evidence' / (name + '.png'))
    bpy.ops.render.render(write_still=True)

# Keep garment samples outside the evaluated body over all additive rest combinations.
report['cloth_rest_fit_iterations'] = []
for iteration in range(4):
    corrections = {obj.name: [Vector() for _ in obj.data.vertices] for obj in clothes}
    worst = {obj.name: [0.0 for _ in obj.data.vertices] for obj in clothes}
    for values in itertools.product((-1, 0, 1), repeat=3):
        set_shapes(values)
        points, faces, _ = evaluated_geometry(body)
        surface = BVHTree.FromPolygons(points, faces)
        for obj in clothes:
            cloth_points, _, _ = evaluated_geometry(obj)
            for i, point in enumerate(cloth_points):
                location, normal, _, distance = surface.find_nearest(point)
                gap = (point - location).dot(normal)
                deficit = .004 - gap
                if deficit > worst[obj.name][i]:
                    worst[obj.name][i] = deficit
                    corrections[obj.name][i] = normal * deficit
    report['cloth_rest_fit_iterations'].append(max(max(items) for items in worst.values()))
    set_shapes((0,0,0))
    for obj in clothes:
        for key in obj.data.shape_keys.key_blocks:
            for item, movement in zip(key.data, corrections[obj.name]):
                item.co += movement
    bpy.context.view_layer.update()

for values in itertools.product((-1, 0, 1), repeat=3):
    set_shapes(values)
    report['combinations'].append({'values_minus1_to_plus1': list(values), **geometry_metrics()})
    code = '_'.join(str(value + 1) for value in values)
    for view, direction in {'front': (0,-1,0), 'side': (1,0,0), 'back': (0,1,0), 'threequarter': (1,-2,.1)}.items():
        top.hide_render = pants.hide_render = True
        render('combinations/' + code + '_' + view, direction, resolution=(480, 600))
        top.hide_render = pants.hide_render = False
set_shapes((0, 0, 0))
render('07-neutral-cloth-shells')
render('08-neutral-profile-shells', (1, 0, 0))

armature = bpy.data.armatures.new('G2_PROTO_AutoWeightRig')
rig = bpy.data.objects.new('G2_PROTO_AutoWeightRig', armature)
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
# Replace heat leakage across adjacent limbs with explicit diagnostic joint zones.
for vertex in body.data.vertices:
    x,y,z=base[vertex.index];side='Left' if x>=0 else 'Right'
    weights={}
    def blend(a,b,t):
        t=smooth(t)
        return {a:1-t,b:t}
    if z<1.12:
        if z<.23: weights=blend(side+'Foot',side+'LowerLeg',(z-.13)/.10)
        elif z<.70: weights=blend(side+'LowerLeg',side+'UpperLeg',(z-.49)/.21)
        else: weights=blend(side+'UpperLeg','Hips',(z-.93)/.19)
    elif abs(x)>.205 and z>1.30:
        if abs(x)<.30: weights=blend('Chest',side+'UpperArm',(abs(x)-.205)/.095)
        elif abs(x)<.56: weights=blend(side+'UpperArm',side+'LowerArm',(abs(x)-.43)/.13)
        else: weights=blend(side+'LowerArm',side+'Hand',(abs(x)-.70)/.08)
    elif z<1.29: weights=blend('Hips','Spine',(z-1.12)/.17)
    elif z<1.51: weights=blend('Spine','Chest',(z-1.29)/.16)
    elif z<1.64: weights=blend('Chest','Neck',(z-1.51)/.13)
    else: weights=blend('Neck','Head',(z-1.64)/.08)
    if .70<z<1.12:
        leg=side+'UpperLeg'
        if leg in weights:
            weights[leg]*=1-smooth((z-.80)/.14)*(1-smooth(abs(x)/.05))
            weights['Hips']=1-weights[leg]
    for group in body.vertex_groups: group.remove([vertex.index])
    for name,weight in weights.items():
        if weight>1e-8: body.vertex_groups[name].add([vertex.index],weight,'REPLACE')
report['weight_method']='Explicit side-isolated joint blends; identical source-correspondent weights on shells; max two influences.'
weight_totals = [sum(group.weight for group in vertex.groups) for vertex in body.data.vertices]
report['automatic_weights'].update({'bones': len(rig.data.bones), 'vertex_groups': len(body.vertex_groups),
    'unweighted_vertices': sum(total < 1e-6 for total in weight_totals), 'minimum_total_weight': min(weight_totals), 'maximum_total_weight': max(weight_totals), 'maximum_influences': max(len(v.groups) for v in body.data.vertices)})

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
# Fit only failing garment vertices across the comparable diagnostic poses.
report['local_pose_fit_iterations']=[]
for iteration in range(6):
    corrections={o.name:[Vector() for _ in o.data.vertices] for o in clothes}
    worst={o.name:[0.0 for _ in o.data.vertices] for o in clothes}
    for values in ((0,0,0),(-1,-1,-1),(1,1,1)):
        set_shapes(values)
        for rotations in [()] + list(poses.values()):
            reset_pose()
            for bone,axis,degrees in rotations: rotate(bone,axis,degrees)
            bpy.context.view_layer.update()
            points,faces,_=evaluated_geometry(body);surface=BVHTree.FromPolygons(points,faces)
            for obj in clothes:
                cp,_,_=evaluated_geometry(obj)
                for i,point in enumerate(cp):
                    location,normal,_,distance=surface.find_nearest(point)
                    deficit=.002-(point-location).dot(normal)
                    if deficit>worst[obj.name][i]:
                        matrix=Matrix(((0,0,0),(0,0,0),(0,0,0)))
                        for g in obj.data.vertices[i].groups:
                            bone=rig.pose.bones[obj.vertex_groups[g.group].name]
                            matrix += (bone.matrix @ bone.bone.matrix_local.inverted()).to_3x3()*g.weight
                        movement=matrix.inverted_safe() @ (normal*deficit)
                        if movement.length>.008: movement*=.008/movement.length
                        corrections[obj.name][i]=movement
                        worst[obj.name][i]=deficit
    report['local_pose_fit_iterations'].append(max(max(w) for w in worst.values()))
    reset_pose();set_shapes((0,0,0))
    for obj in clothes:
        for key in obj.data.shape_keys.key_blocks:
            for vertex,movement in zip(key.data,corrections[obj.name]): vertex.co+=movement
    bpy.context.view_layer.update()
# Recompute all rest metrics after local pose fitting; earlier views show body only.
report['combinations']=[]
for values in itertools.product((-1,0,1),repeat=3):
    set_shapes(values)
    report['combinations'].append({'values_minus1_to_plus1':list(values),**geometry_metrics()})
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
report['sweeps']=[]
(OUTPUT/'evidence/sweeps').mkdir(exist_ok=True)
for axis_index,axis in enumerate(axes):
    others=[i for i in range(3) if i!=axis_index]
    for context_index,context in enumerate(((0,0),(-1,-1),(1,1),(-1,1),(1,-1))):
        values=[0,0,0]
        for i,v in zip(others,context): values[i]=v
        values[axis_index]=0;set_shapes(values)
        reference=[v.copy() for v in evaluated_geometry(body)[0]]
        forbidden=[]
        for i,v in enumerate(base):
            allowed={'Chest':1.30<v.z<1.60 and v.y<-.005 and abs(v.x)<.31,
                     'Hips':.82<v.z<1.12 and abs(v.x)>.035,
                     'Waist':1.12<v.z<1.30 and abs(v.x)<.34}[axis]
            if not allowed: forbidden.append(i)
        max_leak=0;max_step=0;previous=None
        for step in range(101):
            values[axis_index]=-1+step*.02;set_shapes(values)
            points=evaluated_geometry(body)[0]
            assert all(math.isfinite(c) for v in points for c in v)
            max_leak=max(max_leak,max((points[i]-reference[i]).length for i in forbidden))
            if previous: max_step=max(max_step,max((a-b).length for a,b in zip(points,previous)))
            previous=points
            if step%5==0:
                top.hide_render=pants.hide_render=True
                render('sweeps/'+axis+'_'+str(context_index)+'_'+str(step).zfill(3),(1,-2,.1),resolution=(240,300))
                top.hide_render=pants.hide_render=False
        report['sweeps'].append({'axis':axis,'other_axes':others,'other_values':context,'samples':101,
            'forbidden_vertices':len(forbidden),'max_forbidden_displacement_m':max_leak,'max_step_m':max_step})
        assert max_leak<1e-7
reset_pose()
set_shapes((0, 0, 0))
for view, direction in {'front': (0,-1,0), 'side': (1,0,0), 'back': (0,1,0), 'threequarter': (1,-2,.1)}.items():
    render('neutral-clothes-' + view, direction)
    top.hide_render = pants.hide_render = True
    render('neutral-body-' + view, direction, resolution=(960,1200))
    for vertex, point in zip(body.data.shape_keys.key_blocks['Basis'].data, original_normalized):
        vertex.co = point
    bpy.context.view_layer.update()
    render('original-body-' + view, direction, resolution=(960,1200))
    for vertex, point in zip(body.data.shape_keys.key_blocks['Basis'].data, base):
        vertex.co = point
    bpy.context.view_layer.update()
    top.hide_render = pants.hide_render = False
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
for obj in (body, top, pants):
    for vertex, basis in zip(obj.data.vertices, obj.data.shape_keys.key_blocks['Basis'].data):
        vertex.co = basis.co
    obj.data.update()
scene.render.filepath = ''
bpy.ops.wm.save_as_mainfile(filepath=str(OUTPUT / 'woman_g2_body.blend'))
bpy.ops.object.select_all(action='DESELECT')
for obj in (body, top, pants, rig):
    obj.select_set(True)
bpy.context.view_layer.objects.active = rig
bpy.ops.export_scene.fbx(filepath=str(OUTPUT / 'woman_g2_body.fbx'), use_selection=True,
    object_types={'MESH', 'ARMATURE'}, use_mesh_modifiers=False, add_leaf_bones=False,
    bake_anim=False, axis_forward='-Z', axis_up='Y', apply_unit_scale=True)
report['original_hash_preserved'] = hashlib.sha256(SOURCE.read_bytes()).hexdigest() == SOURCE_HASH
report['export'] = {'file': 'woman_g2_body.fbx', 'sha256': hashlib.sha256((OUTPUT / 'woman_g2_body.fbx').read_bytes()).hexdigest()}
(OUTPUT / 'deformation-results.json').write_text(json.dumps(report, indent=2), encoding='utf-8')
assert len(body.data.vertices) == 11022
assert all(len(f.vertices) == 4 for f in body.data.polygons)
assert report['original_hash_preserved']
assert all(case['finite_vertices'] and case['collapsed_triangles_below_1e-12_m2'] == 0 for case in report['combinations'])
print('ISEK_001_DEFORMATION_PROBE_COMPLETE')
