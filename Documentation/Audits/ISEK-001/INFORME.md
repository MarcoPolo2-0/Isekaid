# ISEK-001 — Auditoría técnica de Woman

**Fecha:** 28/10/2026, Guatemala.  
**Versión:** 1.0.  
**Alcance autorizado:** evaluación técnica; sin implementación definitiva.  
**Estado:** informe entregado para revisión de Dirección; ISEK-001 sigue abierta.

## 1. Dictamen

**Woman es una base corporal editable viable, con condiciones.** La conectividad permite crear deformaciones locales, conservarlas en un archivo editable y exportarlas con un esqueleto reconocido como Humanoid por Unity. No hace falta sustituir el cuerpo por primitivas ni reconstruirlo íntegramente para continuar la evaluación.

**El recurso todavía no es una compañera anime lista para producción.** El original no tiene rig, pesos, animaciones ni Shape Keys. La cabeza es una superficie sin ojos ni boca modelados. La silueta de origen tiene pecho/caderas prominentes y cintura muy estrecha; no debe aprobarse automáticamente como el cuerpo neutral promedio descrito en DEC-001. La ropa y los pesos automáticos de ensayo muestran limitaciones concretas.

Recomiendo que Dirección autorice, si acepta estos resultados, un siguiente hito acotado de **neutral corporal + rediseño local de rostro + rig/pesos y ropa ajustable**, antes de integrar el personaje en el juego. Esta auditoría no concede aceptación artística ni aprueba los rangos de los controles.

## 2. Referencias y protección del trabajo existente

- [DEC-001](../../DECISIONS.md), consultada localmente y en GitHub. Blob remoto: `1fd05fd3026726483b02bbae78b0ec36070f399a`.
- [Workflow 1.0](../../Workflow_Desarrollo_v1.0.md), contrastado con GitHub; el contenido local coincide. Blob remoto: `32e8b85bcead5202e1a90c78eac3b57697b7d729`.
- [Issue ISEK-001/#1](https://github.com/MarcoPolo2-0/Isekaid/issues/1), abierto; leído mediante API pública. Última actualización observada: `2026-10-28T02:06:52Z`.
- [Especificación ISEK-001 v0.1](../../Specifications/ISEK-001.md): PROPUESTA. Se usó para ordenar riesgos y ejemplos de prueba, sin convertir sus detalles en aprobados.
- [GDD v0.5](../../GameDesignDocument_v0.5.md), base aprobada.

Repositorio principal local: `C:\Users\PC\Documents\Game Idea\Isekaid`. Commit observado: `1874c868f5730765914abe82d99ab8e6e693218a`. El proyecto del juego está en el repositorio separado `Isekaidunity`; commit observado: `362a56673373c88b0e4168f19501db8cd52e4b40`. Se respetaron los cambios locales preexistentes.

**No se escribieron código, escenas, prefabs, configuración ni assets dentro de `Isekaidunity` durante esta auditoría.** La prueba de importación se ejecutó en otro proyecto Unity, creado para el ensayo. No se cambió el puntero del submódulo, no se sustituyó el personaje del juego y no se publicaron mensajes en otros chats ni en el issue.

## 3. Archivo y entorno comprobados

| Elemento | Resultado observado |
|---|---|
| Original | `assets/woman.fbx`, 639,772 bytes, FBX binario 7400 |
| SHA-256 original | `674c07ff612577a1a5b7e7ee124202722fa5eb0f68bdc01c820a875b5bef1d87` |
| Autor/procedencia registrados | Woman, Fly_jkey; [Sketchfab](https://sketchfab.com/3d-models/woman-b303b7606140480f8335cda121b9607c), según DEC-001 |
| Blender ejecutado | 5.2.3 LTS, instalación Microsoft Store; inicio mediante su alias `blender-launcher.exe` |
| Unity ejecutado | 6000.6.7f1, proyecto aislado de auditoría |
| Servicios de pago | Ninguno |
| GDS | No se encontró `game-dev` en PATH; no se utilizó ese pipeline ni se instaló software |

La licencia CC BY procede del registro de DEC-001; esta auditoría **no verifica de nuevo la versión exacta de la licencia**. Antes de distribuir derivados, completar el registro de autor, URL, enlace a la licencia exacta y modificaciones. El hash del original se volvió a comprobar al terminar.

El inicio directo del ejecutable protegido de Microsoft Store devolvió acceso denegado. Se resolvió usando el alias declarado por el propio paquete; no se alteraron permisos del sistema.

## 4. Malla real y topología

Inventario reproducible: [inventory.json](inventory.json).

| Dato del original | Resultado |
|---|---:|
| Objetos de malla | 1, `Mesh_0` |
| Vértices | 11,022 |
| Aristas | 22,040 |
| Caras | 11,020, todas cuadriláteros |
| Triángulos evaluados | 22,040 |
| Componentes conectados | 1 |
| Bordes abiertos / no manifold / aristas sueltas | 0 / 0 / 0 |
| Aristas manifold con orientación no contigua | 0 |
| Caras degeneradas bajo `1e-12` unidades locales² | 0 |
| Capas UV / materiales | 1 (`UVMap`) / 1 (`Material.004`) |
| Rig / grupos de pesos / animaciones / Shape Keys | Ninguno |

La prueba de simetría de la copia normalizada encontró distancia máxima y media de **0** al buscar el vértice reflejado en X. Es favorable para editar ambos lados y desarrollar las formas corporales.

La estructura cerrada y los cuadriláteros son una buena base de edición; **no prueban por sí solos** buen flujo anatómico, ausencia exhaustiva de autointersecciones, UV sin solapamientos ni deformación apta para animación. Hay concentraciones de flujo en cuello, axilas y pelvis que necesitan revisión al pintar pesos y modelar correctivos.

El importador dejó el cuerpo con unos **189.4196 de alto** y los pies por debajo del origen. Para los ensayos se aplicó exclusivamente a la copia un factor uniforme de `0.01`, se horneó la orientación y se apoyaron los pies en Z=0. La copia quedó de **1.8942 m**. Esto es una normalización técnica de prueba; no fija la altura de diseño aprobada.

El material referencia `untitled8_DefaultMaterial_Normal.png`, ausente en los archivos disponibles. Las capturas usan materiales de inspección, no las texturas finales. No se certificó el UV para texturizado de producción.

Evidencia: [frontal](evidence/01-original-front.png), [perfil](evidence/02-original-profile.png), [espalda](evidence/03-original-back.png), [tres cuartos](evidence/04-original-three-quarter.png), [aristas originales](evidence/05-original-wire.png), [topología facial](evidence/10-face-topology.png), [hombros](evidence/11-shoulder-topology.png) y [pelvis/rodillas](evidence/12-pelvis-knee-topology.png).

## 5. Shape Keys de pecho, caderas y cintura

Se añadieron a la copia seis formas diagnósticas: `ChestMinus/Plus`, `HipsMinus/Plus`, `WaistMinus/Plus`, además de Basis. Usan desplazamientos locales continuos; no escalan el objeto completo. Se preservaron los 11,022 vértices y su conectividad.

| Eje | Vértices afectados | Desplazamiento máximo de ensayo |
|---|---:|---:|
| Pecho | 1,007 | aproximadamente 35.0 mm |
| Caderas | 1,824 | aproximadamente 37.7 mm |
| Cintura | 1,207 | aproximadamente 21.7 mm |

Las formas preservan cabeza, manos y pies en las regiones protegidas declaradas por el script. Se activaron independientemente y en **27 combinaciones** de tres valores por eje (`-1, 0, +1`). En las láminas aparecen como `0/2, 1/2, 2/2`; no son valores de un editor de producto ya implementado.

Las 27 combinaciones conservaron coordenadas finitas y no generaron triángulos colapsados bajo `1e-12 m²`. La mayor razón de longitud de arista respecto del reposo fue **1.515**, y el peor percentil 99 fue **1.278**. Estos valores describen el ensayo, no un umbral de aceptación aprobado. No sustituyen un examen exhaustivo de autointersecciones o de conservación de volumen.

**Cintura resulta técnicamente posible como Shape Key local.** Sigue condicionada a corregir ropa y pesos y a aceptar visualmente el rango. Las zonas de transición de ejes pueden compartir vértices; eso no altera el valor de otro eje, pero puede requerir formas correctivas combinadas.

El cambio de pecho se aprecia más en perfil; la variación lateral frontal es deliberadamente menor en este campo de ensayo. Los extremos no deben pasar a producción sin la revisión de Dirección.

Matrices: [cintura baja](evidence/matrix-waist-0.jpg), [neutral](evidence/matrix-waist-1.jpg), [alta](evidence/matrix-waist-2.jpg). Comparación específica: [pecho en perfil](evidence/chest-profile-comparison.jpg).

## 6. Ropa: prueba, fallos y método recomendado

Se crearon **muestras abiertas de superficie**, no prendas terminadas: una del torso y otra de las piernas. Se derivaron de regiones del cuerpo, con correspondencia de vértices, separación inicial nominal de 9 mm por normal y las mismas seis formas. Los pesos se transfirieron mediante esa correspondencia. Así se comprobó el principio de sincronizar cuerpo y ropa sin introducir una simulación física ni otro personaje.

**El método ingenuo de copiar desplazamientos y pesos no basta para cerrar la ropa.** En las 27 formas en reposo y en las 18 poses apareció al menos una muestra de distancia firmada negativa inferior a -0.5 mm. La peor muestra fue aproximadamente **-4.75 mm en reposo** y **-7.20 mm en poses**. En reposo neutral se detectaron 32 muestras negativas en el torso y 12 en piernas. Son muestras de vértices respecto de la superficie más próxima; no constituyen una comprobación exhaustiva de colisión entre triángulos.

Los bordes irregulares y la separación visible en la unión torso/piernas pertenecen a las muestras recortadas y no son un diseño de top/pantalón aprobado. No se ocultaron en las capturas.

Preparación recomendada:

1. Aprobar primero cuerpo neutral y extremos; modelar top y pantalón sobre esa base, con costuras, holgura y aberturas intencionales.
2. Mantener correspondencia con el cuerpo donde sea útil; para prendas con otra topología, transferir deformación con un método de superficie y revisar el resultado.
3. Dar a cada prenda las mismas entradas de control, pero corregir sus propias formas. Igual nombre/peso no significa que la ropa deba usar exactamente el mismo delta de posición.
4. Revisar axilas, pecho, cintura del pantalón, entrepierna y rodillas; añadir correctivos de combinación/pose donde se detecte penetración.
5. Revisar grosor y normales, pintar pesos y repetir las 27 combinaciones y las poses aprobadas. La simulación de tela no debe usarse para encubrir un ajuste base incorrecto.

## 7. Rig, pesos y exportación

El original carece de rig. Para evaluar la ruta técnica se creó un **esqueleto diagnóstico de 19 huesos** con pesos automáticos de Blender. No incluye dedos ni rig facial. El operador terminó y ningún vértice corporal quedó sin peso. Eso **no certifica** la distribución ni la calidad artística del skinning.

Se ensayaron seis poses con proporciones bajas, neutrales y altas simultáneas: brazos arriba, brazos al frente, giro de torso, sentada, flexión profunda y zancada. Total: **18 configuraciones**. No se ejecutó la batería completa de 162 poses/combinaciones propuesta por Chat 2, ni animaciones continuas de caminar/correr.

Las 18 configuraciones conservaron coordenadas finitas y no colapsaron triángulos bajo el umbral declarado. Sin embargo, el peor estiramiento de arista alcanzó **3.578 veces** su longitud de reposo, y el peor percentil 99 fue **1.620**. La zancada y las flexiones requieren corregir especialmente pelvis/entrepierna y la distribución de pesos. También se deben revisar hombros y codos; la ausencia de triángulos colapsados no demuestra conservación de volumen.

Comparaciones: [proporciones bajas](evidence/poses-comparison-0.jpg), [neutrales](evidence/poses-comparison-1.jpg), [altas](evidence/poses-comparison-2.jpg). Son poses diagnósticas, no animaciones finales ni pruebas de contacto de pies con el suelo.

Se guardó y reabrió el [archivo editable derivado](woman_deformation_probe.blend). El [FBX de ensayo](woman_deformation_probe.fbx) se reimportó en Blender conservando las seis formas más Basis y el esqueleto de 19 huesos. Datos: [roundtrip-results.json](roundtrip-results.json).

En el proyecto Unity aislado, el FBX produjo **Avatar válido y `isHuman=true`**, tres mallas con pesos y seis Blend Shapes cada una. Se horneó la malla corporal para cada forma a peso 100 y se comprobaron desplazamientos no nulos, acordes con los del ensayo de Blender. La ropa de piernas conserva correctamente delta cero para pecho, que no afecta esa región.

Unity separó vértices por los atributos de importación: cuerpo 12,052, muestra torso 2,106 y piernas 4,725. No significa que se hayan añadido vértices al archivo fuente de Blender. Ver [unity-import-results.json](evidence/unity-import-results.json) y [captura real del importador aislado](evidence/13-unity-isolated-import.png).

La captura Unity usa materiales Standard del proyecto aislado. **No prueba los materiales URP definitivos, el creador del juego, Play Mode, retargeting de clips reales ni una build.**

## 8. Rostro anime y expresiones

La vista de [cabeza original](evidence/06-original-head.png) y su [malla](evidence/10-face-topology.png) muestran una superficie cerrada sin ojos, boca ni cavidad oral modelados. No se encontraron recursos faciales en el FBX.

Se pueden crear Shape Keys porque la malla conserva conectividad, pero mover esa superficie lisa no crea por sí solo párpados, labios, ojos detallados o expresiones legibles. La solución requiere trabajo artístico real:

- Rediseñar localmente la cabeza y su unión con el cuello, con flujo alrededor de párpados y labios. No hay evidencia que obligue a reconstruir todo el cuerpo.
- Añadir ojos/iris/pupilas y recursos de boca según la dirección aprobada; rehacer UV y texturas de las zonas modificadas.
- Crear y revisar formas normales de ojos, boca, cejas y parpadeo; separar mirada y habla cuando corresponda.
- Reservar el canal de texturas/atlas o recursos 2D para la parte cómica del sistema híbrido, con exclusión de ojos normales cuando proceda.

La viabilidad del enfoque es razonable, pero **no se implementó ni se validó un rostro anime** en esta auditoría. Su aceptación necesita referencias y revisión artística de Dirección.

## 9. Reutilización del prototipo Unity

Inspección estática del proyecto privado local; no se modificaron sus archivos ni se reejecutaron aquí sus pruebas de juego anteriores.

| Componente existente | Reutilización recomendada | Adaptación futura necesaria |
|---|---|---|
| `CharacterCreation.unity` / `Game.unity` | Conservar flujo, UI, rotación y navegación | Sustituir solo el visual cuando se apruebe; añadir vista de rostro y controles reales |
| `CompanionProfile`, `CompanionProfileStore` | Conservar colores y personalidad existentes | Migración explícita para proporciones/versión del modelo; preservar datos desconocidos o corruptos antes de reemplazarlos |
| `CompanionAppearance` / `CompanionPalette` | Conservar selección y aplicación de tintes | Soportar materiales por submalla e índices y Blend Shapes; un tinte global por renderer no basta para piel/iris/ropa combinados |
| `CompanionController` y NavMesh | Conservar seguimiento, espera y estado | Separar velocidad/estado visual hacia Animator; calibrar cápsula y escala del nuevo modelo |
| `CompanionState` | Conservar distinción trampa/incapacitación y temporizador activo | La pose caída actual inclina el visual rígidamente; sustituirla por animación/pose de rig en un hito aprobado |
| `FirstPersonController`, HUD y flujo de pausa | Reutilizar como infraestructura de prueba | Mantener encuadre, distancia y claridad; no acreditan combate ni experiencia final |
| Prefabs de compañera y preview | Conservar sus raíces de integración | Compartir un visual validado para evitar divergencia entre creador y aventura |

El perfil v1 no tiene pecho/caderas/cintura. La recuperación silenciosa a valores por defecto del store debe revisarse antes de introducir una nueva versión: una vista previa no debe reemplazar datos válidos sin una decisión explícita de guardado.

## 10. Puertas de decisión y siguiente alcance sugerido

| Puerta | Resultado técnico de esta auditoría | Lo que falta para aceptar |
|---|---|---|
| G1: base editable | Evidencia favorable: malla cerrada, editable, simétrica, sin degeneración detectada | Dirección evalúa silueta neutral y coste del rostro; licencia exacta pendiente antes de distribuir |
| G2: ajustes independientes | Demostrados en campos de prueba y 27 combinaciones; exportables a Unity | Rangos modelados/aceptados, barrido y validación geométrica/visual más completa |
| G3: rig, ropa y movimiento | Ruta Humanoid comprobada; muestras y pesos automáticos muestran fallos | Rig/pesos corregidos, prendas reales y animaciones continuas; sin penetraciones persistentes |
| G4: personaje anime integrado | No ejecutada por límite de alcance | Autorización de implementación, rostro/cabello/materiales y validación en creador/aventura |

**Propuesta para Dirección, no implementación autorizada:** primero aprobar la neutral corporal y el rediseño local del rostro; después definir rangos, prendas y rig corregido; finalmente integrar el visual aprobado en las raíces del prototipo, migrar el perfil y ejecutar las pruebas completas. Cintura puede continuar como candidata técnica; no recomendaría declararla terminada ni descartarla por adelantado.

## 11. Entregables y límites de la prueba

- [Inspección original editable](woman_original_inspection.blend), sin modificar el FBX de entrada.
- [Prueba editable de deformación/rig](woman_deformation_probe.blend) y [exportación FBX](woman_deformation_probe.fbx), ambas identificadas como diagnósticas.
- Inventarios JSON, resultados por combinación/pose y [resumen de métricas](metrics-summary.json).
- Vistas nativas de Blender, tres matrices de formas, tres matrices de poses y una captura de importación Unity.
- Scripts de [inspección](scripts/inspect_woman.py), [ensayo](scripts/probe_woman.py), [reapertura](scripts/verify_roundtrip.py) e [importación aislada](scripts/WomanImportAudit.cs).
- [Manifiesto de archivos y SHA-256](artifact-manifest.json).

Reproducción en este equipo: iniciar el alias `blender-launcher.exe` con `--background --factory-startup --disable-autoexec --python-exit-code 1 --python RUTA_DEL_SCRIPT`, en orden inspección, ensayo y verificación. El ensayo parte del `.blend` de inspección y conserva el FBX original. Para Unity, copiar el FBX derivado y el script de importación a un **proyecto vacío separado**, ejecutar `WomanImportAudit.Run` con `-auditOutput DIRECTORIO_DE_EVIDENCIA` y Unity 6000.6.7f1; no instalar el script en el juego definitivo.

Los renders de inspección son ortográficos, con material de estudio. Las láminas son composiciones etiquetadas de esos renders reales. No son imágenes de concepto ni sustituyen una revisión del personaje final. Las métricas de ropa son muestreadas; no se certificaron colisión exhaustiva, UV sin solapamientos, conservación de volumen, rendimiento, Play Mode, animaciones finales ni builds.

**Decisión solicitada a Chat 1:** revisar si acepta continuar con esta base y aprobar el alcance artístico/técnico siguiente junto con la especificación del Chat 2. Esta entrega no marca ISEK-001 como completada.
