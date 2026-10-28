# Isekaid — VRM local, prueba experimental

## Revisión de estabilidad del PR #4 — vigente

28/10/2026. Alcance exclusivo: reemplazo transaccional, fallos por etapas, cargas/descargas repetidas y actualización con main. **No fusionado; integración definitiva pendiente de Chat 1.** La investigación inicial se conserva debajo como antecedente, no se repitió la batería facial ni se generaron nuevas capturas.

### Reemplazo y descarga

`VrmAppearance.LoadLocal` mantiene Avatar, diccionario de expresiones y flags del anterior durante la preparación. El candidato se importa oculto, valida Humanoid/mallas/materiales, configura Animator, prepara expresiones y recursos y se activa. Solo después de validar su activación se oculta el anterior y se publican las nuevas referencias; su destrucción se programa al finalizar el commit. Todo el tramo de activación hasta ese punto es síncrono, sin un await intermedio que exponga una transición parcial.

Si falla, se restauran exactamente los flags anteriores, se conserva su runtime de expresiones y se desactiva/destruye el candidato. Las referencias se preparan en variables locales antes de publicarse. `Unload()` es idempotente, limpia el API visual y restaura la visibilidad original del maniquí en la escena experimental. Se rechazan una segunda carga o una descarga mientras hay una importación en curso. Al destruir el presentador durante el await, el SDK termina su carga y el resultado se descarta y libera: es cancelación lógica, no interrupción inmediata de la decodificación.

Los hooks de fallo están disponibles únicamente en Editor y sus llamadas se eliminan del Player por Conditional. No permiten que el archivo VRM inyecte código; son un mecanismo local de la prueba.

### Pruebas ejecutadas en Play Mode, Unity 6000.6.7f1

| Caso | Resultado |
|---|---|
| Primera importación falla después de ocultar presentación previa | PASS: vuelve el maniquí y Avatar sigue vacío |
| `ValidatedBytes`, `Imported`, `Prepared`, `Activated`, `PreviousHidden` | PASS: cinco fallos forzados conservan identidad del anterior, renderers, expresiones y peso happy 0.7 |
| Activación previa a publicación | PASS: nuevo activo y anterior aún visible en Activated; API todavía apunta al anterior |
| Seis ciclos de carga, reemplazo y descarga | PASS: nuevo publicado visible, anterior destruido, descarga doble sin error |
| Segunda carga/descarga durante una carga pendiente | PASS: rechazadas sin sustituir ni perder la instancia válida |
| Destruir presentador mientras el SDK importa | PASS: tarea termina con error/cancelación, ningún root VRM ni nuevos assets transitorios retenidos |
| Recursos propiedad del SDK y avatares nativos inspeccionados | PASS: 1138 referencias comprobadas destruidas tras la limpieza |

`stability-results.json` contiene los resultados nuevos. Se inspeccionan recursos reales de RuntimeGltfInstance, Avatar y root, usando la nulidad nativa de Unity después de Destroy/frames; no basta que el wrapper C# pierda una referencia. Para el caso de destrucción del propietario se compara además el conjunto de Mesh/Texture/Material/Avatar transitorios antes y después.

Los fallos se inyectan en los cinco límites indicados, no dentro de cada función del SDK ni del decodificador PNG. Hay pruebas tanto con apariencia anterior válida como con la primera importación sin VRM previo. No se vuelve a presentar la prueba antigua de ruta inexistente como si cubriera todas esas etapas.

### Memoria y alcance de la medición

Se mide tras calentamiento y, en cada descarga, después de frames, Resources.UnloadUnusedAssets y GC. La tolerancia diagnóstica declarada es 16 MiB de crecimiento de memoria asignada respecto del baseline caliente; **no es un presupuesto de producción**. Mayor diferencia observada, incluido destruir el propietario: **0.250 MiB**. Pico muestreado por el Editor durante las operaciones: **770.54 MiB**; puede omitir picos síncronos entre muestras.

| Muestra descargada | Asignada MiB | Reservada MiB | Managed MiB | Mesh/Texture/Material/Avatar globales |
|---|---:|---:|---:|---|
| warm-baseline | 495.354 | 1753.38 | 21.457 | 7/384/71/0 |
| cycle-0 | 495.358 | 1753.38 | 21.527 | 7/384/71/0 |
| cycle-1 | 495.394 | 1753.38 | 21.543 | 7/384/71/0 |
| cycle-2 | 495.401 | 1753.38 | 21.555 | 7/384/71/0 |
| cycle-3 | 495.405 | 1753.38 | 21.539 | 7/384/71/0 |
| cycle-4 | 495.472 | 1753.38 | 21.547 | 7/384/71/0 |
| cycle-5 | 495.476 | 1753.38 | 21.543 | 7/384/71/0 |
| owner-destroyed | 495.603 | 1753.38 | 21.500 | 7/384/71/0 |

Los conteos globales se mantienen; las referencias de recursos inspeccionadas se destruyen. La memoria reservada por Unity permanece en 1753.38 MiB: no se confunde esa reserva del allocator con objetos de avatar retenidos ni se promete devolución inmediata de RAM al sistema operativo. Resultado: sin retención detectada dentro de estos seis ciclos y fallos acotados. No demuestra ausencia de toda fuga en sesiones largas, VRAM o todas las plataformas; no se ejecutaron build, perfilado GPU ni pruebas de larga duración. El reemplazo requiere coexistencia transitoria de dos avatares y debe presupuestarse antes de producción.

### Main y conservación

Se incorporó main `83553e9f7e74567d60aab06f0b701caee8d715ec` mediante merge `55d3684a0756d611c524b430afcccb2d4b443027`, sin rebase destructivo ni descartar cambios locales. Los tres archivos locales preexistentes se respaldaron y sus hashes coincidieron después del merge; los borradores documentales del otro chat permanecen fuera de este commit. La entrada propia de readme se registra aparte.

`python tools/verify_unified_layout.py`: **PASS**, código 0, `ISEKAID_STRUCTURE_OK`. La ausencia de ese script en la primera entrega queda resuelta por el main incorporado. Los tests originales `PrototypeChecks` y `PrototypePlayModeChecks` se ejecutaron otra vez después del merge/corrección y registraron ambos PASSED. La comprobación estática no sustituye esos ensayos de Play Mode ni completa por sí sola ISEK-002; no se repitió por separado su escena histórica CompanionPrototype.

Se recalcularon los hashes de **803 archivos protegidos**: cero cambios. Se conservan CharacterCreation, Game, prefabs originales, Woman, G2-PROTO, G2-CORR y el VRM local. El merge elimina rutas antiguas de Unity de la raíz como establece main; no altera las escenas/prefabs de UnityProjectIsekaid. Avatar, capturas y logs privados permanecen excluidos de Git. No hay nuevo creador, catálogo ni activación en producción.

### Reproducir esta corrección

Con el proyecto correcto y el archivo privado autorizado, ejecutar Unity con `-batchmode -force-d3d11 -projectPath RUTA_UNITYPROJECT -executeMethod Isekaid.ExperimentalVrm.Editor.VrmStabilityChecks.Run -vrmTestPath RUTA_ABSOLUTA_MODELO.vrm -vrmOutput CARPETA_PRIVADA -logFile LOG_PRIVADO`, sin `-quit`. Usa la escena experimental guardada; no llama a constructores ni guarda escenas. Comprobar `ISEKAID_VRM_STABILITY_PASSED` y el JSON nuevo.

Ejecutar `python tools/verify_unified_layout.py` y `python Documentation/Audits/VRM-POC/scripts/check_delivery.py`. El segundo solo comprueba evidencias guardadas y hashes; no repite Unity. No se requiere publicar el modelo o las imágenes para ejecutar los tests localmente.

---

## Antecedente: primera prueba, commit 73e89ed

Lo siguiente documenta la entrega inicial. Sus límites de medición de descarga y su antigua ausencia del script de layout quedan actualizados por la revisión anterior.

28/10/2026, Guatemala. Rama `experiment/vrm-companion-import`. **Viable como prueba técnica en Play Mode; no aprobada como función de producción.** Repositorio unificado `MarcoPolo2-0/Isekaid`, proyecto `UnityProjectIsekaid/`. No sustituye Woman ni reabre G2. No implementa el nuevo creador, catálogo, persistencia de avatares ni sistema completo de personalidad.

## Estado del prototipo / ISEK-002

Se ejecutaron los tests existentes antes de añadir UniVRM y después de la integración: `PrototypeChecks.Run` y `PrototypePlayModeChecks.Run`. Los registros contienen `ISEKAID_PROTOTYPE_CHECKS_PASSED` y `ISEKAID_TWO_SCENE_PLAYMODE_CHECKS_PASSED`. Se prueban creador, colores/perfil, transición a Game, rescate, memoria única, personalidad, órdenes/seguimiento, incapacitación, pausa y retorno. Esto es ejecución automatizada real en Play Mode, no solo inspección estática. Ver `baseline-results.json`.

Fallo documentado en la entrega inicial de ISEK-002: faltaba `tools/verify_unified_layout.py`; no se pudo ejecutar y no se declara completada ISEK-002. Hay advertencia preexistente UGameStudio UAC0005 y avisos de macros en `.gitattributes` anidado tras la unificación. No se corrigieron por no pertenecer a este experimento. No se reconstruyeron escenas de producción. CompanionPrototype.unity no se volvió a recorrer de forma independiente; no se declara ISEK-002 completa.

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
