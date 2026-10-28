import bpy
import bmesh
import hashlib
import json
import math
from pathlib import Path
from mathutils import Vector

ROOT = Path(__file__).resolve().parents[4]
OUTPUT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / 'assets' / 'woman.fbx'
OUTPUT.mkdir(parents=True, exist_ok=True)
(OUTPUT / 'evidence').mkdir(exist_ok=True)

bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)
bpy.ops.import_scene.fbx(filepath=str(SOURCE), use_anim=True)
imported = list(bpy.context.scene.objects)
meshes = [obj for obj in imported if obj.type == 'MESH']
report = {
    'source': str(SOURCE.relative_to(ROOT)),
    'sha256': hashlib.sha256(SOURCE.read_bytes()).hexdigest(),
    'blender': bpy.app.version_string,
    'scene_units': {'system': bpy.context.scene.unit_settings.system, 'scale_length': bpy.context.scene.unit_settings.scale_length},
    'armatures': [], 'meshes': [], 'actions': [action.name for action in bpy.data.actions],
}

for obj in imported:
    if obj.type == 'ARMATURE':
        report['armatures'].append({'name': obj.name, 'bones': [bone.name for bone in obj.data.bones]})

all_points = []
for obj in meshes:
    mesh = obj.data
    mesh.calc_loop_triangles()
    points = [obj.matrix_world @ vertex.co for vertex in mesh.vertices]
    all_points.extend(points)
    bm = bmesh.new()
    bm.from_mesh(mesh)
    bm.verts.ensure_lookup_table()
    bm.faces.ensure_lookup_table()
    visited = set()
    components = []
    for vertex in bm.verts:
        if vertex.index in visited:
            continue
        stack = [vertex]
        visited.add(vertex.index)
        size = 0
        while stack:
            current = stack.pop()
            size += 1
            for edge in current.link_edges:
                other = edge.other_vert(current)
                if other.index not in visited:
                    visited.add(other.index)
                    stack.append(other)
        components.append(size)
    item = {
        'name': obj.name, 'vertices': len(mesh.vertices), 'edges': len(mesh.edges),
        'faces': len(mesh.polygons), 'triangles': len(mesh.loop_triangles),
        'face_sides': {str(sides): sum(len(face.vertices) == sides for face in mesh.polygons) for sides in sorted(set(len(face.vertices) for face in mesh.polygons))},
        'boundary_edges': sum(edge.is_boundary for edge in bm.edges),
        'nonmanifold_edges': sum(not edge.is_manifold for edge in bm.edges),
        'wire_edges': sum(edge.is_wire for edge in bm.edges),
        'noncontiguous_edges': sum(edge.is_manifold and not edge.is_contiguous for edge in bm.edges),
        'degenerate_faces_below_1e-12_local_units_squared': sum(face.calc_area() < 1e-12 for face in bm.faces),
        'components_vertex_counts': sorted(components, reverse=True),
        'uv_layers': [layer.name for layer in mesh.uv_layers],
        'shape_keys': [key.name for key in mesh.shape_keys.key_blocks] if mesh.shape_keys else [],
        'vertex_groups': [group.name for group in obj.vertex_groups],
        'modifiers': [{'name': modifier.name, 'type': modifier.type} for modifier in obj.modifiers],
        'materials': [slot.material.name if slot.material else None for slot in obj.material_slots],
        'location': list(obj.location), 'rotation_euler': list(obj.rotation_euler), 'scale': list(obj.scale),
        'world_min': [min(point[axis] for point in points) for axis in range(3)],
        'world_max': [max(point[axis] for point in points) for axis in range(3)],
    }
    bm.free()
    report['meshes'].append(item)

report['images'] = [{'name': image.name, 'filepath': image.filepath, 'packed': bool(image.packed_file),
                     'exists': bool(image.packed_file) or Path(bpy.path.abspath(image.filepath)).is_file()} for image in bpy.data.images if image.source == 'FILE']
minimum = Vector([min(point[axis] for point in all_points) for axis in range(3)])
maximum = Vector([max(point[axis] for point in all_points) for axis in range(3)])
center = (minimum + maximum) * 0.5
dimensions = maximum - minimum
report['world_bounds'] = {'min': list(minimum), 'max': list(maximum), 'dimensions': list(dimensions)}

scene = bpy.context.scene
scene.render.engine = 'BLENDER_WORKBENCH'
scene.render.resolution_x = 960
scene.render.resolution_y = 1200
scene.render.resolution_percentage = 100
scene.render.image_settings.file_format = 'PNG'
scene.world.color = (0.06, 0.06, 0.06)
scene.display.shading.light = 'STUDIO'
scene.display.shading.studiolight_rotate_z = math.radians(25)
scene.display.shading.color_type = 'SINGLE'
scene.display.shading.single_color = (0.63, 0.66, 0.70)
scene.display.shading.show_shadows = True
scene.display.shading.show_cavity = True
scene.display.shading.cavity_type = 'BOTH'
scene.display.shading.background_type = 'WORLD'
camera_data = bpy.data.cameras.new('AuditCamera')
camera = bpy.data.objects.new('AuditCamera', camera_data)
scene.collection.objects.link(camera)
scene.camera = camera
camera_data.type = 'ORTHO'

def render(name, direction, target=center, scale=dimensions.z * 1.14, wire=False):
    camera.location = target + Vector(direction).normalized() * dimensions.z * 3
    camera.rotation_euler = (target - camera.location).to_track_quat('-Z', 'Y').to_euler()
    camera.data.ortho_scale = scale
    overlays = []
    if wire:
        for obj in meshes:
            overlay = obj.copy()
            overlay.data = obj.data.copy()
            scene.collection.objects.link(overlay)
            overlay.data.materials.clear()
            ink = bpy.data.materials.get('AuditWire') or bpy.data.materials.new('AuditWire')
            ink.diffuse_color = (0.025, 0.025, 0.025, 1)
            overlay.data.materials.append(ink)
            for polygon in overlay.data.polygons:
                polygon.material_index = 0
            modifier = overlay.modifiers.new('TopologyEdges', 'WIREFRAME')
            modifier.thickness = dimensions.z * 0.0003
            modifier.offset = 1
            modifier.use_replace = True
            overlays.append(overlay)
        scene.display.shading.color_type = 'MATERIAL'
    scene.render.filepath = str(OUTPUT / 'evidence' / (name + '.png'))
    bpy.ops.render.render(write_still=True)
    for overlay in overlays:
        bpy.data.objects.remove(overlay, do_unlink=True)
    scene.display.shading.color_type = 'SINGLE'

render('01-original-front', (0, -1, 0))
render('02-original-profile', (1, 0, 0))
render('03-original-back', (0, 1, 0))
render('04-original-three-quarter', (1, -2, 0.15))
render('05-original-wire', (0, -1, 0), wire=True)
face_center = Vector((center.x, center.y, maximum.z - dimensions.z * 0.08))
render('06-original-head', (0, -1, 0), target=face_center, scale=dimensions.z * 0.23, wire=True)
for obj in meshes:
    obj.show_wire = False
    obj.show_all_edges = False
scene.render.filepath = ''
bpy.ops.wm.save_as_mainfile(filepath=str(OUTPUT / 'woman_original_inspection.blend'))
(OUTPUT / 'inventory.json').write_text(json.dumps(report, indent=2), encoding='utf-8')
assert hashlib.sha256(SOURCE.read_bytes()).hexdigest() == report['sha256']
print('ISEK_001_BLENDER_INVENTORY_COMPLETE', json.dumps(report))
