# Isekaid — VRM local, prueba experimental

28/10/2026, Guatemala. Rama `experiment/vrm-companion-import`. **Viable como prueba técnica en Play Mode; no aprobada como función de producción.** Repositorio unificado `MarcoPolo2-0/Isekaid`, proyecto `UnityProjectIsekaid/`. No sustituye Woman ni reabre G2. No implementa el nuevo creador, catálogo, persistencia de avatares ni sistema completo de personalidad.

## Estado del prototipo / ISEK-002

Se ejecutaron los tests existentes antes de añadir UniVRM y después de la integración: `PrototypeChecks.Run` y `PrototypePlayModeChecks.Run`. Los registros contienen `ISEKAID_PROTOTYPE_CHECKS_PASSED` y `ISEKAID_TWO_SCENE_PLAYMODE_CHECKS_PASSED`. Se prueban creador, colores/perfil, transición a Game, rescate, memoria única, personalidad, órdenes/seguimiento, incapacitación, pausa y retorno. Esto es ejecución automatizada real en Play Mode, no solo inspección estática. Ver `baseline-results.json`.

Fallo documental actual de ISEK-002: falta `tools/verify_unified_layout.py`; no se pudo ejecutar y no se declara completada ISEK-002. Hay advertencia preexistente UGameStudio UAC0005 y avisos de macros en `.gitattributes` anidado tras la unificación. No se corrigieron por no pertenecer a este experimento. No se reconstruyeron escenas de producción. CompanionPrototype.unity no se volvió a recorrer de forma independiente; no se declara ISEK-002 completa.

## Recurso, SDK e importación

El archivo privado `assets/test yandere companion.vrm`, generado por VRoid Studio 2.14.0, es VRM 1.0 / GLB 2, 15.21 MiB. SHA-256 `37eadcbaa4e240b691601daabcfa4ad4d5319793dff0cc0d128eff55f4de8736`. No fue alterado, importado como asset de producción ni añadido a Git.

UniVRM **0.131.4**, paquetes oficiales `com.vrmc.gltf` y `com.vrmc.vrm`, rutas UPM `Packages/UniGLTF` y `Packages/VRM10`. `packages-lock.json` fija commit `efd20f62f1bf0ac3fcc34de1e497324c5197d146`. La release indica Unity 2022.3 o posterior; se comprobó aquí compilación y carga con **6000.6.7f1**, URP 17.6.0 y Direct3D 11, no todas las plataformas. No se añadieron servicios de inferencia, credenciales, red de avatares ni APIs de pago.

La prueba usa `Vrm10.LoadBytesAsync`, VRM 0.x deshabilitado, generación de control rig recomendada y selección de materiales URP de UniVRM. Los bytes se leen desde una ruta local durante Play Mode y se validan antes de pasarlos al SDK. `RuntimeGltfInstance` conserva la propiedad y destrucción de recursos. Una importación fallida deja vivo el avatar anterior; se verificó esta recuperación. No se ejecutan scripts o assemblies suministrados por el archivo.

Escena: `Assets/Experiments/VrmCompanion/VrmLocalTrial.unity`. Es una copia de ensayo de Game, con los mismos componentes de comportamiento, navegación e interacciones y un presentador VRM aparte. Las escenas originales, prefabs definitivos, perfil y código existente no se modificaron. Se comprobaron hashes de **803 archivos protegidos**, incluidos Woman, ambos G2 y el VRM original: cero cambios. Build Settings permanece intacto.

## Resultado visual y animación

Avatar Humanoid válido, 3 SkinnedMeshRenderer, 21330 vértices de malla cargada, 32654 triángulos y 16 materiales, todos con `VRM10/Universal Render Pipeline/MToon10`. La suma de vértices en accessors originales es mayor por submallas/atributos compartidos; no equivale a vértices únicos importados.

Se generaron cuatro clips de ensayo por músculos Humanoid: reposo estático, caminar, correr y giro. Son ciclos sintéticos de validación, sin root motion, no animaciones finales. El giro rota la presentación mientras conserva el actor/estado. Se recorrió al menos 1.5 s por estado; variación medida del muslo: caminar 27.0°, correr 54.0°; giro de raíz 135.0°. Las poses y manos aún resultan mecánicas y requieren clips finales licenciados, ajuste de retargeting y revisión de contactos/penetraciones.

La cámara de evidencia sigue el frente real del avatar; no supone que el actor conserve orientación mundial fija. Se corrigió un ensayo inicial con rig no generado y encuadre fijo, que ocultaba el rostro. Las capturas entregadas pertenecen a la versión final con control rig. Se encuadra la geometría horneada, no una altura supuesta. Bounds finales aproximados 2.25 m de alto, incluyendo cabello/calzado, con escala de anclaje 1: revisar tamaño y collider antes de producción; no se cambió la escala artística del archivo.

Hay 25 grupos Spring Bone reales; el SDK los actualizó y se registró movimiento de sus articulaciones. La comparación de quaternions contra la pose inicial puede acercarse a 180° y no certifica estabilidad física. No se considera superada una prueba completa de cabello/ropa, colisiones o estabilidad prolongada. Se conservan los recursos y parámetros declarados por el modelo.

## Expresiones y recuperación

El archivo declara **14 presets con morph binds**: neutral, happy, angry, sad, relaxed, surprised, blink, blinkLeft, blinkRight, aa, ih, ou, ee, oh. Se aplicaron individualmente, se comprobó peso de morph y se capturaron las 14 caras. Las cinco emociones, parpadeo y vocales son visibles en las imágenes finales. UniVRM expone además lookUp/lookDown/lookLeft/lookRight como canales procedimentales; no se presentan como cuatro morphs añadidos al archivo.

`ICompanionExpressions.RequestExpression(nombre, peso)` solicita expresiones sin conocer renderer o índice de blend shape. `VrmAppearance` encapsula UniVRM, limita pesos válidos a 0–1, limpia la solicitud anterior y devuelve false/restaura pose neutra si falta el nombre. La ausencia se probó con un nombre inexistente. No hay análisis de voz ni lip sync: las vocales son solicitudes manuales de ensayo.

## Conexión con comportamiento existente

`VrmCompanionAdapter` observa `CompanionController.Changed` y velocidad de su NavMeshAgent. Solicita una reacción visual provisional por estado activo/no activo y selecciona Idle/Walk/Run sin modificar reglas de negocio. El modo de previsualización permite probar clips; “Seguir movimiento” devuelve el control al observador. El avatar cuelga del anclaje visual existente y acompaña sus transformaciones; el actor sigue controlando ruta, rescate, estado e interacciones.

El test mantiene la misma instancia visual al recorrer las tres personalidades y verifica que rescate registra una sola confianza. El actor existente se desplazó 0.157 m en el ensayo de seguimiento: prueba mínima de movimiento con el VRM montado, no certificación de rutas largas o locomoción final. El seguimiento más amplio se cubre por los tests originales, que también pasaron después de instalar UniVRM. No se vincula personalidad al nombre “yandere” del archivo.

## Rendimiento básico, no presupuesto de lanzamiento

Hardware: Intel(R) Core(TM) i7-14700F; NVIDIA GeForce RTX 5070 Ti. Importación SDK después de prevalidación: **702.7 ms**. Memoria total asignada por Unity antes/después: 447.3/632.7 MiB, diferencia **185.4 MiB**. Al final, 655.9 MiB; no se midió recuperación tras descarga/GC ni VRAM.

Intervalos de simulación del Editor batch, sin capturas ni pausas >100 ms: media 0.452 ms, p95 0.504 ms. **No son FPS de un build, GPU time ni presentación real en pantalla.** Render+readback+codificación PNG de 960×1200: mediana 41.4 ms, primera captura 236.9 ms. Estos costes incluyen diagnóstico; tampoco representan el frame del juego. Falta perfilado standalone/dispositivo y medición de picos/unload para fijar presupuestos.

## Seguridad y permisos

Límites del prototipo: 32 MiB por archivo; GLB 2 con JSON <=4 MiB, profundidad <=64, sin claves duplicadas ni URI externas/data URI; VRM 1.0 estable; hasta 512 nodos, 32 mallas, 64 materiales/imágenes, 2048 accessors y 12 millones de elementos totales, 250000 posiciones y 12 millones de posiciones×morph; PNG incorporadas <=4096 por lado y <=64 megapíxeles totales; jerarquía sin ciclos ni padres múltiples; 128 springs/2048 referencias de joints y 256 colliders. BufferViews deben caber en el BIN. Solo un import simultáneo; coordenadas finitas y materiales compatibles antes de sustituir apariencia. JPEG, recursos externos y VRM 0.x se rechazan en esta primera prueba.

Nueve rechazos reproducibles ejecutados: vacío, cabecera inválida, tamaño excesivo, URI externa, claves duplicadas, JSON demasiado profundo, versión GLB incompatible truncamiento y offset desbordado. No es fuzzing completo ni sandbox de proceso: Unity/decodificadores y el SDK procesan datos en el proceso del juego. Para importación de archivos hostiles hacen falta aislamiento, cobertura adversarial, límites de tiempo y memoria medidos y tratamiento de cancelación/unload; no afirmar seguridad total por pasar nueve casos.

Metadatos globales declarados: autor MarcoPolo, avatarPermission=onlyAuthor, commercialUsage=personalNonProfit, creditNotation=required, allowRedistribution=false, modification=prohibited; licencia enlazada https://vrm.dev/licenses/1.0/. Los flags adicionales permiten violencia/sexual/política/antisocial según el archivo, pero no se convierten en contenido ni permisos globales del juego. **Este modelo no queda autorizado para catálogo público ni uso comercial por superar la prueba técnica.**

No hay licencia o trazabilidad individual para las 26 imágenes, prendas y cabello incorporados. El generador VRoid y los nombres de materiales no prueban origen ni derechos de cada recurso. Se conserva el archivo original; falta confirmar condiciones con el autor/recursos originales antes de distribuir. Tanto el VRM como las capturas quedan locales e ignorados por Git. El repositorio solo recibe código, escenas sin avatar incrustado, resultados numéricos y hashes de evidencias.

## Evidencias y reproducción

- `runtime-results.json`, `source-inspection.json`, `baseline-results.json`, `protected-files-check.json` y `local-capture-manifest.json` acompañan este informe.
- Capturas privadas: `UnityProjectIsekaid/LocalVrmEvidence/`: cuatro estados de movimiento, 14 expresiones, seguimiento y lámina `expressions-contact-sheet.jpg`. Son renders de cámara Unity dentro de Play Mode, no una captura del escritorio ni conceptos ilustrados. No se publicaron los PNG.
- Abrir exactamente `UnityProjectIsekaid/` en Unity 6000.6.7f1 y resolver UPM. Abrir `Assets/Experiments/VrmCompanion/VrmLocalTrial.unity`, Play; Escape libera el cursor mediante la pausa existente, pegar ruta absoluta y “Cargar VRM”; reanudar y usar controles del prototipo. El archivo debe proceder del usuario autorizado; el repositorio no lo descarga ni lo incluye.
- Repetir automatización con Unity: `-batchmode -force-d3d11 -projectPath RUTA_UNITYPROJECT -executeMethod Isekaid.ExperimentalVrm.Editor.VrmProbeEditor.Run -vrmTestPath RUTA_ABSOLUTA_MODELO.vrm -vrmOutput CARPETA_PRIVADA -logFile LOG_PRIVADO`. El test genera/sobrescribe solo su escena/clips experimentales. No añadir `-quit`: el test termina tras Play Mode.
- Baseline: `-executeMethod Isekaid.Editor.PrototypePlayModeChecks.Run`, también sin `-quit`. Restaura el perfil guardado al terminar.

**Siguiente paso sujeto a Chat 1:** aprobar contrato apariencia/comportamiento, política de permisos y plataformas; después importación gestionada/persistencia, selección de archivos, escala/collider, clips finales, carga/cancelación/descarga y perfilado real. Este experimento demuestra viabilidad acotada; no inicia integración definitiva ni da por aceptado ISEK-001 o el diseño VRM propuesto.

Referencias oficiales: [UniVRM 0.131.4](https://github.com/vrm-c/UniVRM/releases/tag/v0.131.4), [API runtime Vrm10](https://vrm.dev/en/api/runtime-import/UniVRM10_Vrm10/), [fuentes fijadas del SDK](https://github.com/vrm-c/UniVRM/tree/efd20f62f1bf0ac3fcc34de1e497324c5197d146/Packages).
