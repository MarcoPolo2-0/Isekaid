from pathlib import Path
import json,hashlib
p=Path(__file__).resolve().parents[1]
r=json.loads((p/'deformation-results.json').read_text());u=json.loads((p/'evidence/unity/unity-import-results.json').read_text())
def stat(cases):
 return {'negative':sum(a['negative_signed_clearance_below_minus_0_5mm'] for c in cases for a in c['cloth_vertex_signed_clearance']), 'failed':sum(any(a['negative_signed_clearance_below_minus_0_5mm'] for a in c['cloth_vertex_signed_clearance']) for c in cases), 'min_mm':1000*min(a['minimum_signed_clearance_m'] for c in cases for a in c['cloth_vertex_signed_clearance']), 'stretch':max(c['edge_stretch_max'] for c in cases)}
a=stat(r['combinations']);b=stat(r['poses'])
x=json.loads((p/'surface-crossings.json').read_text());old=json.loads((p/'surface-crossings-baseline.json').read_text())
def crossings(data):return sum(a['edge_triangle_crossings'] for c in data['cases'] for a in c['cloth'])
text=f"""# ISEK-001 / G2-CORR — primera entrega para revisión

28/10/2026, Guatemala. Autoridad: DEC-003. Rama `feature/isek-001-g2-body`, PR privado #2, borrador. **G2 no aceptada**. No inicia G3/G4.

## Resultado y límites

Neutral candidata corregida localmente sobre Woman: secciones de anchura medidas en la malla original, redistribución tórax/cintura/pelvis, menor proyección esférica del pecho y relajación restringida. Se conservaron los 11022 vértices, 11020 quads, orden y conectividad; no se sustituyó el modelo ni se usó remesh. La silueta cambia claramente respecto de G2-PROTO. El tórax/pecho aún puede percibirse demasiado plano y quedan transiciones que necesitan criterio artístico; la nueva neutral tampoco se declara aprobada. La altura 1.8942 m continúa como normalización técnica.

[Comparación antes/después en cuatro vistas](evidence/before-after.jpg). Misma cámara ortográfica, escala, pose e iluminación; la fila anterior procede de la entrega revisada `afcdd61`. Las cuatro imágenes nuevas están en `evidence/neutral-body-*.png`. La dirección A de Diseño es referencia de intención, sin copiar medidas de la ilustración ni implementar rostro/cabello/prendas finales.

Original intacto: `../G2-PROTO/input/woman.fbx`, SHA-256 `674c07ff612577a1a5b7e7ee124202722fa5eb0f68bdc01c820a875b5bef1d87`. Fuente: Woman, Fly_jkey, https://sketchfab.com/3d-models/woman-b303b7606140480f8335cda121b9607c . Variante exacta CC BY/atribución pendiente antes de distribución. La versión anterior completa sigue recuperable en `../G2-PROTO/` y commit `afcdd61649bac3cabc833605d61037006eaf6509`; no se modificó. Editable actual: `woman_g2_body.blend`; FBX de esta revisión: `woman_g2_body.fbx`, copia en `Assets/G2BodyPrototype/` con el mismo GUID y sin referencias desde escenas.

## Controles e independencia

Los seis extremos se recalcularon desde la nueva Basis. Se mantienen las amplitudes máximas anteriores: pecho 24.93 mm, caderas 26.99 mm, cintura 18.49 mm; no se redujo el rango para ocultar defectos. Se cambió su distribución espacial: pecho z 1.30–1.60 m, frente del tórax; caderas z 0.82–1.12 m; cintura z 1.12–1.30 m. Los soportes se anulan en los límites; las transiciones locales cambian dentro de esas regiones. Parámetros exactos, anchuras y muestras: `scripts/build_corr.py` y `deformation-results.json`.

Se ejecutaron 15 barridos: tres ejes, cada uno con los otros dos en (0,0), (-1,-1), (+1,+1), (-1,+1), (+1,-1). Cada barrido recorre -1 a +1 con 101 muestras: **1515 estados**, sin coordenadas inválidas. Máximo desplazamiento en los vértices fuera de la región permitida: **{max(c['max_forbidden_displacement_m'] for c in r['sweeps']):.9f} m**, tolerancia 1e-7 m. Cabeza, manos y pies permanecen estables en los controles. Son barridos muestreados, no detección continua de colisiones entre muestras.

Evidencia: 108 vistas de 27 combinaciones, cuatro matrices `evidence/matrix-27-*.jpg`; 315 frames de barridos y tres GIF `evidence/sweep-Chest.gif`, `sweep-Hips.gif`, `sweep-Waist.gif`. Cada GIF muestra cinco contextos simultáneos; las etiquetas de los otros ejes siguen el orden listado en `sweeps[].other_axes`. P=pecho, C=caderas, W=cintura. Las láminas y GIF organizan renders reales, no conceptos ilustrados.

## Diagnóstico de pesos y ropa

Se sustituyó la fuga de pesos automáticos por mezclas explícitas locales en rodillas, pelvis y hombros, con transición central hacia Hips para evitar saltar directamente entre piernas. Máximo dos influencias, cero vértices sin peso, sumas normalizadas. Las muestras comparten pesos por correspondencia exacta con la malla fuente. Esto es un rig diagnóstico, no rig final validado para locomoción.

Holgura inicial normal de 16 mm, normales por extremo y corrección de vértices que fallan; ajuste local en las mismas 18 poses y reposo. Correcciones devueltas a espacio de reposo mediante la inversa de la mezcla de huesos, máximo 8 mm por iteración, seis iteraciones. No se escala globalmente la ropa. Se repitieron después todos los resultados en reposo y poses sobre las muestras finales. La base de exportación se sincroniza con Basis para evitar el fallo FBX anterior.

| Medida comparable | G2-PROTO | G2-CORR |
|---|---:|---:|
| Casos negativos en reposo | 0/27 | {a['failed']}/27 |
| Separación mínima en reposo | +1.68 mm | {a['min_mm']:+.2f} mm |
| Casos negativos en poses | 9/18 | {b['failed']}/18 |
| Vértices negativos acumulados en poses | 782 | {b['negative']} |
| Peor penetración firmada en poses | 13.70 mm | {abs(min(0,b['min_mm'])):.2f} mm |
| Máximo estiramiento de arista, reposo | 1.478 | {a['stretch']:.3f} |
| Máximo estiramiento de arista, poses | 3.578 | {b['stretch']:.3f} |
| Pares de triángulos que cruzan, 45 casos | {crossings(old)} | {crossings(x)} |

**El estiramiento máximo no se considera resuelto aunque el clipping mejore**; comparar el valor final de la tabla, sin extrapolar mejora de todas las articulaciones.

Cero triángulos colapsados bajo 1e-12 m² y posiciones finitas en los 27+18 casos. Se repitieron exactamente los seis tipos de pose con neutral/mínimos/máximos globales: brazos arriba, brazos delante, torsión, sentado, flexión profunda y zancada. No son las 162 configuraciones de AC08 ni animación continua. Las geometrías y pesos cambian: la tabla compara resultado conjunto, no causalidad aislada.

`surface-crossings.json` añade lo que el muestreo de vértices no cubre: candidatos BVH triangulados seguidos de intersección segmento/triángulo en ambas direcciones. `surface-crossings-baseline.json` repite el mismo método sobre la versión anterior intacta. Los conteos son pares de triángulos, no penetraciones independientes ni profundidades. Excluye coplanares y contactos solo en extremos; no prueba colisiones continuas ni autointersecciones del cuerpo. Persisten cruces en poses: AC08 continúa FAIL; las muestras siguen abiertas, con dobladillos escalonados, sin espesor ni diseño final.

## Blender, FBX y Unity

Reapertura nativa y reimportación Blender 5.2.3 LTS: tres mallas, seis extremos y Basis, cero diferencia entre la base de malla y Basis. Se conserva la conectividad corporal. Unity 6000.6.7f1 aislado: Avatar válido/humanoide, tres SkinnedMeshRenderer y seis Blend Shapes por malla. Se comprobaron **27 combinaciones sincronizadas y 1515 muestras sincronizadas**, con **{u['synchronizedMeshBakes']} horneados de malla** incluyendo retorno neutral. Cada renderer recibe los mismos valores y se verifica su estado y coordenadas finitas. Se comprueba además deformación efectiva de torso ante pecho y piernas ante caderas, y que pecho no desplaza piernas. No se verifican contactos entre superficies dentro de Unity; esos cruces se midieron en Blender.

SHA-256 del FBX realmente probado en Unity: `{u['sourceSha256']}`. El informe Unity registra este hash. Copia de prueba e importable de Assets coinciden por hash. Material Standard de inspección y cámara aislada: no es Play Mode ni integración URP final.

## Estado por criterio

Ver `criteria-status.json`: AC01–02 PASS de continuidad técnica; AC03–07 UNKNOWN en aceptación completa (pruebas numéricas de aislamiento y barridos verificadas, evaluación visual/autointersecciones aún pendientes); AC08 FAIL; AC09–15 NOT_CHECKED por alcance; AC16 UNKNOWN por licencia/paquete final. AV01, AV03–04 UNKNOWN hasta revisión visual; AV02 PASS de comparabilidad técnica; AV05–10 NOT_CHECKED, sin aprobación implícita de sus propuestas. La existencia de métricas o fuentes no sustituye la decisión de Dirección.

## Reproducir y revisar

1. Desde esta carpeta ejecutar Blender con `--background --factory-startup --disable-autoexec --python-exit-code 1 --python scripts/build_corr.py`. Se apoya en el original preservado en G2-PROTO/input. Sobrescribe solo salidas de esta corrección; guardar ediciones manuales aparte.
2. Ejecutar `scripts/verify_corr.py`; repetir con `-- --baseline` para la versión previa. Esperar la finalización real de Blender y comprobar JSON nuevos, no solo retorno del alias de Windows.
3. En Unity aislado vacío 6000.6.7f1, copiar FBX a `Assets/woman_g2_body.fbx` y `scripts/WomanImportAudit.cs` a `Assets/Editor`. Usar módulos animation, physics, jsonserialize e imageconversion. Ejecutar `-batchmode -force-d3d11 -executeMethod WomanImportAudit.Run -auditOutput RUTA_SALIDAS -quit`. El test abre escena vacía; no ejecutarlo sobre escenas de producción.
4. `python scripts/package_corr.py` genera matrices/GIF y manifiesto; `python scripts/check_delivery.py` verifica los resultados guardados, copias, amplitudes, invariantes y hashes. Este último no repite Blender/Unity.

Se conservaron los archivos protegidos y el puntero de submódulo. No se editaron escenas, perfil, UI, prefabs ni arquitectura runtime. Pendientes: aprobación visual de neutral/extremos, cruces residuales, pérdida de volumen en articulaciones, licencia exacta y autorización posterior para rostro, cabello, prendas finales y G3/G4. No se ejecutaron Play Mode, locomoción, builds ni dispositivos.
"""
(p/'INFORME.md').write_text(text.rstrip()+chr(10),encoding='utf8')
