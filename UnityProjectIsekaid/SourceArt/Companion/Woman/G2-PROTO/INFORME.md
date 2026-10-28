# ISEK-001 / G2-PROTO — entrega técnica para revisión

Fecha: 28/10/2026. Estado: **PROTOTIPO ENTREGADO; G2 NO ACEPTADA**.
Rama: `feature/isek-001-g2-body`. Autoridad: DEC-002, Issue [ISEK-001](https://github.com/MarcoPolo2-0/Isekaid/issues/1). No incorpora como aprobada la propuesta artística v0.1 de Chat 2.

## Recurso editable y procedencia

Se deriva Woman de Fly_jkey, https://sketchfab.com/3d-models/woman-b303b7606140480f8335cda121b9607c . Atribución CC BY declarada en DEC-001; confirmar variante exacta y requisitos antes de distribuir. No se sustituyó por primitivas ni se reconstruyó la malla. `input/woman.fbx` conserva los bytes originales, SHA-256 `674c07ff612577a1a5b7e7ee124202722fa5eb0f68bdc01c820a875b5bef1d87`. `input/woman_original_inspection.blend` conserva la inspección inicial reproducible.

Editable: [woman_g2_body.blend](woman_g2_body.blend). Exportación: [woman_g2_body.fbx](woman_g2_body.fbx); copia idéntica importable en `Assets/G2BodyPrototype/woman_g2_body.fbx`, con ModelImporter Humanoid y Blend Shapes. No hay referencia desde escenas/prefabs del juego. Blender 5.2.3 LTS, Unity 6000.6.7f1.

## Qué cambió

- Neutral candidata: reducción localizada de proyección/anchura del pecho y caderas, relleno de cintura, manteniendo altura de ensayo 1.8942 m. Movimiento neutral máximo 59.99 mm. La silueta todavía conserva rasgos exagerados del original; no se presenta como neutral artística aprobada.
- Misma conectividad: 11022 vértices y 11020 cuadriláteros. Se recalcularon normales para evitar sombreado heredado de la forma original.
- Tres ejes independientes mediante seis Shape Keys: `ChestMinus/Plus`, `HipsMinus/Plus`, `WaistMinus/Plus`. Control -1..1: Minus=max(0,-valor), Plus=max(0,valor); en Unity multiplicar por 100. No activar simultáneamente ambos extremos del mismo eje. Campos con caída cúbica y soporte más amplio para transiciones. Cabeza, manos y pies protegidos en los tres controles.
- Máximos desplazamientos por eje: pecho 24.93 mm, caderas 26.99 mm, cintura 18.49 mm. Estos números son rangos experimentales.
- Rig de prueba de 19 huesos con pesos automáticos normalizados y limitados a cuatro influencias. Cero vértices sin peso; sumas entre 0.999999955 y 1.000000045. Es corrección de normalización, no rig final pintado manualmente.
- Muestras abiertas de torso y piernas derivadas del cuerpo: holgura normal de 16 mm, normales propias por Shape Key y cuatro iteraciones de ajuste sobre las 27 combinaciones. Pesos transferidos por correspondencia de vértices. Se sincronizó la malla base con Basis antes de exportar: sin esta corrección el FBX añadía desplazamientos ajenos al control.

## Resultados comprobados

| Prueba | G1 aceptada | G2-PROTO |
|---|---:|---:|
| Combinaciones en reposo con penetración de vértices >0.5 mm | 27/27 | 0/27 |
| Muestras negativas acumuladas en reposo | 1152 | 0 |
| Menor separación firmada en reposo | -4.75 mm | +1.68 mm |
| Poses con penetraciones, seis poses × neutral/mínimo/máximo | 18/18 | 9/18 |
| Muestras negativas acumuladas en poses | 1490 | 782 |
| Peor penetración en poses | 7.20 mm | **13.70 mm: regresión pendiente** |
| Máximo estiramiento de arista en reposo | 1.515 | 1.478 |
| Peor percentil 99 de estiramiento en reposo | 1.278 | 1.175 |
| Máximo estiramiento de arista en poses | 3.578 | 3.578: pendiente |
| Peor percentil 99 de estiramiento en poses | 1.620 | 1.509 |

Las geometrías, rangos y holguras cambiaron: la comparación mide el resultado conjunto, no demuestra por separado el efecto de cada corrección. Estiramiento se mide contra la base de cada ensayo; no es una validación anatómica. Todas las 27 combinaciones y 18 poses tienen coordenadas finitas y cero triángulos colapsados bajo 1e-12 m². El test de penetración es un muestreo de vértices contra BVH y normales locales; no prueba ausencia de cruces entre triángulos, autointersecciones o contactos durante animación continua.

Reapertura nativa y reimportación FBX verificadas: seis controles, tres mallas y 19 huesos. Unity aislado: avatar válido y humanoide, tres SkinnedMeshRenderer con seis Blend Shapes, 27 combinaciones corporales horneadas con valores finitos. El cuerpo importado tiene 12062 vértices por separación de normales/UV; no implica nueva topología de autoría. La muestra de piernas permanece inmóvil ante ChestMinus/Plus tras exportación. Las medidas crudas del mesh Unity están en su espacio local; las del BakeMesh incluyen la escala del renderer.

Se compararon hashes de 58 archivos de escenas, scripts existentes y ProjectSettings: cero cambios durante G2. Los cambios locales previos permanecen fuera de los commits de esta entrega. No se actualizó el puntero de submódulo del repositorio principal.

## Evidencias reales

- [Original frente a neutral, cuatro vistas](evidence/original-vs-neutral.jpg).
- 108 renders completos en `evidence/combinations/`: 27 combinaciones × frontal, lateral, trasera y tres cuartos.
- [Matriz frontal](evidence/matrix-27-front.jpg), [lateral](evidence/matrix-27-side.jpg), [trasera](evidence/matrix-27-back.jpg), [tres cuartos](evidence/matrix-27-threequarter.jpg). P=pecho, C=caderas, W=cintura; -1/0/+1.
- 18 capturas de poses en `evidence/poses/`, cuatro vistas de ropa neutral y acercamientos de topología.
- [Render Unity aislado](evidence/unity/13-unity-isolated-import.png), [resultado Unity](evidence/unity/unity-import-results.json), [geometría y ropa](deformation-results.json), [reapertura/FBX](roundtrip-results.json), [protección de escenas](protected-files-check.json).

Son renders directos de Blender y cámara Unity; las láminas solo organizan y rotulan esos renders. No son ilustraciones conceptuales. El archivo Blender queda en neutral con muestras de ropa visibles, seleccionables/ocultables desde Outliner.

## Pendientes y recomendación

1. Dirección debe revisar neutral y los tres rangos. La cintura, el pecho separado y las transiciones pelvis/muslo aún necesitan criterio artístico; no aprobar solo por las métricas.
2. No integrar todavía: persisten pliegues y pérdida de volumen en hombros/pelvis/rodillas. El peor estiramiento 3.578 no mejoró; la normalización de pesos no sustituye la pintura anatómica ni correctivos por pose.
3. Las muestras de ropa son superficies de diagnóstico con bordes escalonados, separación entre piezas y sin espesor, UV/costuras ni diseño final. El peor clipping en poses empeora pese a reducirse los casos. Rehacer el ajuste local de holgura/pesos y evaluar correctivos por pose antes de llamarlas prendas usables. No ocultar este fallo inflando globalmente la ropa.
4. Rostro anime, expresiones, dedos, cabello, materiales URP finales y animación/locomoción quedan fuera de este prototipo; la cabeza conserva la base sin rostro. La textura normal original sigue ausente.
5. No se verificaron Play Mode, escenas definitivas, build, rendimiento ni dispositivo. Unity aislado usa material Standard de inspección; no demuestra integración URP del juego.

Recomendación: revisar ahora el cambio de neutral y los extremos; conservar la rama sin fusionar. Solo después de la decisión visual, autorizar una corrección acotada de rig/ropa y pruebas de contactos más completas. Esta entrega habilita revisión y no cierra la puerta G2.

## Reproducir

Desde esta carpeta, con Blender instalado:

```powershell
& "$env:LOCALAPPDATA/Microsoft/WindowsApps/blender-launcher.exe" --background --factory-startup --disable-autoexec --python-exit-code 1 --python "$PWD/scripts/build_g2.py"
& "$env:LOCALAPPDATA/Microsoft/WindowsApps/blender-launcher.exe" --background --factory-startup --disable-autoexec --python-exit-code 1 --python "$PWD/scripts/verify_roundtrip.py"
```

Esperar la finalización real de Blender y comprobar archivos/JSON, no solo el retorno del launcher de Windows. El constructor sobrescribe únicamente salidas de G2; conservar modificaciones artísticas manuales en otra copia antes de regenerar.

Para Unity, crear un proyecto aislado vacío 6000.6.7f1 con módulos animation, imageconversion, physics y jsonserialize; copiar FBX a `Assets/woman_g2_body.fbx` y `scripts/WomanImportAudit.cs` a `Assets/Editor/`. Ejecutar Unity con `-batchmode -force-d3d11 -projectPath "RUTA_PROYECTO_AISLADO" -executeMethod WomanImportAudit.Run -auditOutput "RUTA_SALIDAS" -quit`. No ejecutar esta prueba en las escenas de producción: abre una escena temporal vacía. Las aserciones fallan si avatar, mallas, controles o combinaciones no se conservan.

Finalmente `python scripts/check_delivery.py` comprueba resultados, original, independencia y coherencia de copias. Las imágenes y láminas son evidencia de esta ejecución; regenerarlas si cambia el recurso.
