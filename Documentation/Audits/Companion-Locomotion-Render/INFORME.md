# Corrección de locomoción y presentación F2/F3A

Fecha: 29/10/2026. Rama `fix/isek-companion-locomotion-vrm-render`, creada desde `main` `f657d78390202c2414a629d81401d8d801636bb1`. Solicitud: prueba manual parcial de Dirección, prioridad animación y después renderizado. No autoriza cambios narrativos ni la aceptación artística de F2/F3A.

**Entrega técnica verificada en Editor y Windows; revisión visual pendiente.** Implementación `11358db57d90bac3ba62dfc0edd130a2fcd62d73`. No fusionada.

## Causas reproducidas antes del cambio

El Animator de Adventurer usaba tres clips **Generic sin repetición**: Idle 2.4583 s, Walk 1 s y Run 0.5833 s. Su tiempo normalizado seguía creciendo, pero la pose de las piernas quedaba inmóvil después del final del clip. No era suficiente comprobar únicamente ese contador: la rotación de piernas medida al final permanecía en cero grados.

El VRM utilizaba los clips sintéticos del POC, con Run a 2.2 veces su velocidad. Esos clips sí repetían, pero no eran animaciones artísticas apropiadas. La postura encogida provenía de la animación diagnóstica. La inicialización de UniVRM ya generaba correctamente su Control Rig normalizado antes de asignar el controlador; no se encontró un Animator enlazado deliberadamente al rig crudo.

`CompanionAppearance.Update()` escogía Idle/Walk/Run con umbrales instantáneos de 0.1 y 3 m/s. `VrmAppearance.SetMotion()` hacía CrossFade sin comprobar el destino. En un barrido 2.96/3.04 m/s de cinco segundos se registraron 74/75 solicitudes de cambio y 222/203 muestras en transición, para catálogo/VRM respectivamente. Algunas transiciones se interrumpían antes de convertir su destino en estado actual: contar solamente cambios de estado ocultaba el problema.

Datos previos: [baseline-results.json](baseline-results.json). `passed=true` en ese archivo significa que terminó la adquisición del baseline; **no** que sus animaciones defectuosas aprobaran la prueba. [calibration-before-results.json](calibration-before-results.json) conserva la medición antes de ajustar cadencia y apoyo.

## Cambio aplicado

- Controlador nuevo `Assets/_Project/Locomotion/Locomotion.controller`: reposo y un BlendTree Walk/Run, parámetro Speed suavizado en 0.15 s, entrada a locomoción sobre 0.12 m/s y salida bajo 0.06 m/s. Transiciones de duración fija 0.15 s. Cambiar entre caminar y correr conserva la fase del mismo estado; el reposo usa su propio ritmo.
- Tres copias binarias de los clips CC0 existentes, importadas como Humanoid y cíclicas, con root motion horneado en pose para que NavMesh conserve la autoridad sobre el desplazamiento. Los originales y sus `.meta`, los clips POC, TrialAnimations.controller y el controlador histórico Adventurer.controller se conservan completos.
- Avatar Humanoid propio para el catálogo, con correspondencias explícitas de huesos AutoRigPro. Se mantiene el GUID del prefab. Se elimina su antiguo desplazamiento vertical de 0.0522 m, innecesario con la nueva importación Humanoid.
- Cadencias Walk 1.61 y Run 0.70, derivadas de velocidades de apoyo medidas de aproximadamente 0.87 y 5.44 m/s a la escala del catálogo. Cadence reduce el ritmo bajo 1.4 m/s. Se adapta también la velocidad del Animator a la escala Humanoid efectiva (incluye la escala de los padres); referencia medida 0.976 m. Ajuste Humanoid de pies activado en ambos estados.
- El presentador VRM conserva importación transaccional, cancelación, validación y liberación. Su Animator se guarda al publicar el candidato y se limpia al descargarlo. Los controles del creador y del juego solicitan la misma locomoción. El POC histórico mantiene una ruta compatible que evita volver a solicitar el estado actual o su destino ya en transición.
- Los constructores de recursos/escenas apuntan a los recursos nuevos y conservan el ajuste de iluminación. No se cambian la personalidad, el director de diálogo, el perfil persistido, los controles del jugador ni la arquitectura de comportamiento.

Procedencia de clips: [Dawn to Dusk Games](https://dawn-to-dusk-games.itch.io/3d-anime-female-adventurer), CC0-1.0 según el recibo y licencia del catálogo. Las tres copias tienen el mismo SHA-256 binario que sus fuentes. No se incorporan clips de procedencia desconocida, servicios de pago ni dependencias nuevas de runtime.

## Renderizado: comparación independiente

Se cargó el mismo VRM 1.0 privado, SHA-256 `37eadcbaa4e240b691601daabcfa4ad4d5319793dff0cc0d128eff55f4de8736`, en un visor local independiente: three 0.180.0 y [@pixiv/three-vrm 3.4.2](https://github.com/pixiv/three-vrm), MIT. Se conservaron T-pose, texturas y materiales. El navegador recibe todos los recursos del disco mediante interceptación local y rechaza otros orígenes; el modelo no se sube a un visor remoto.

La referencia conserva sombreado de piel y costuras de la ropa. En Unity, los materiales emplean `VRM10/Universal Render Pipeline/MToon10`; las texturas base, normales y emisión están asignadas. Los factores blancos del material multiplican las texturas, no indican por sí mismos una textura ausente. Las emisiones de algunos materiales del cabello pertenecen al archivo y se preservan. No se reemplazan shaders, texturas, transparencia, normales ni condiciones de licencia.

La escena usaba luz direccional cálida a 1.7 (creador 1.6), más ambiente intenso, en un proyecto Linear. La suma de luz directa e indirecta del shader MToon excedía el rango de salida y recortaba las zonas claras. La comparación controlada en T-pose y cámara fija de 960×1200 lo reproduce sin ninguna animación: 109,078 píxeles con los tres canales >=250/255 bajo la iluminación previa aproximada; 16,153 con luz blanca 1.0/ambiente 0.2; cero con la configuración equilibrada. No es una prueba de equivalencia píxel a píxel entre renderizadores.

Se reduce la direccional a 0.65 blanca y se utiliza ambiente uniforme 0.4 en ambas escenas. Un primer relleno 0.15 eliminaba saturación, pero oscurecía demasiado el reverso de Adventurer; se corrigió antes de la entrega. No se añade exposición ni un shader que disimule texturas perdidas. El ambiente nuevo también afecta al entorno y queda sujeto a revisión visual.

La piel y la ropa del fixture son claras también en el visor independiente. No se concluye que el archivo esté defectuoso; se distingue ese aspecto original del recorte causado por la escena. El diagnóstico de postura es independiente: se reemplaza la animación POC por los clips Humanoid del catálogo, conservando el archivo privado.

Datos intermedios: [render-diagnosis-results.json](render-diagnosis-results.json). Su configuración balanced era el primer relleno 0.15; los resultados finales registran la configuración 0.4.

## Validación final

| Prueba ejecutada | Resultado |
|---|---|
| Locomoción Play Mode | PASS: seis muestras de clips Humanoid cíclicos, cuatro seguimientos y cuatro mediciones de zancada; catálogo y VRM |
| Locomoción Windows Player | PASS: mismos escenarios, sin reinicios de tiempo dentro del estado ni alternancias rápidas |
| Ruido de velocidad 2.96/3.04 m/s | 72–74 cruces; únicamente dos cambios de estado por inicio/final del desplazamiento, cero cambios rápidos y cero reinicios, para ambos avatares |
| F3A Editor y Windows | PASS: 285 comprobaciones y 18 recorridos en cada ejecución, expresiones reales/alternativas seguras, continuidad, seguimiento y doble incapacitación/reanimación |
| F2 Editor | PASS: 43 comprobaciones, creador, importación, retorno, cancelación/fallos transaccionales, copia administrada, recursos y persistencia |
| F2 Windows y segundo proceso | PASS: 36 + 2 comprobaciones; reinicio desde copia administrada sin el archivo fuente |
| Guardas y almacenamiento | PASS: nueve rechazos + dos límites de archivo, 11 comprobaciones de almacenamiento y 35 de metadatos |
| Prototipo original de dos escenas | Marcadores `ISEKAID_PROTOTYPE_CHECKS_PASSED` y `ISEKAID_TWO_SCENE_PLAYMODE_CHECKS_PASSED` presentes |
| Compilación/build | Unity 6000.6.7f1, Windows x64 Development Mono, Succeeded; tamaño informado 196,472,292 bytes |
| Integridad estática | Estructura unificada, 715 originales protegidos y biblioteca canónica de 60 entradas/18 rutas conservados |

Las regresiones F2 utilizan el fixture privado **preexistente** de metadatos grandes (`e51cc3f6f3c610c2cc8707fb6478ccbfd1b3a33ff5bf6fb672682e244376b6c0`); locomoción, render y F3A utilizan el VRM original autorizado (`37eadc…8736`). No se altera ni publica ninguno.

La velocidad de NavMesh medida en la prueba recta fue 1.400 y aproximadamente 3.795 m/s. La velocidad retrospectiva de apoyo en la pose, después de calibrar, fue:

| Avatar | Walk | Run |
|---|---:|---:|
| Adventurer | 1.352 m/s | 3.753 m/s |
| VRM autorizado | 1.275 m/s | 3.752 m/s |

Las velocidades anteriores se estiman con el pie bajo y desplazándose hacia atrás; no son contactos físicos instrumentados. La velocidad horizontal bruta del pie cercano al suelo durante desplazamiento, que también incluye salida/entrada de apoyo, fue 1.331/0.714 m/s para catálogo Walk/Run y 1.342/0.751 para VRM. Se conserva esa medición en los JSON y las secuencias para no confundir calibración de cadencia con un sistema de pies perfectamente bloqueados. La malla de reposo queda aproximadamente a 0.001 m del origen para catálogo y a 0.014 m para el VRM con ajuste Humanoid de pies.

En el baseline, `requestedStateChanges` observa por reflexión el antiguo campo `motion`; ese campo fue eliminado y el contador deja de ser aplicable al candidato. La comprobación nueva usa estados reales, tiempo normalizado, transiciones y cambios rápidos; no interpreta el cero del contador antiguo como prueba adicional.

Los cuatro logs de Windows no contienen excepciones, errores o aserciones de runtime. El Editor sí emitió incidencias internas de `UnityEditor.Search.SearchDatabase` y, al reconstruir su ventana Hierarchy, `manager != NULL`; los informes y los marcadores de Play Mode completaron después. No se presenta la Consola del Editor como completamente limpia. Se retiraron exclusivamente cambios de serialización/prefiltros URP generados por la build, tras inspeccionar su diff; no son parte de la corrección.

La primera ejecución Windows con ventana oculta aprobó las comprobaciones, pero ScreenCapture devolvió imágenes negras. Se descartaron como evidencia visual y se repitieron presentación, F3A, F2 y reinicio con **la misma build en ventana visible**: todos PASS. Los JSON Windows y las capturas publicados corresponden a esta repetición; se comprobó que las imágenes tienen contenido y se inspeccionaron creador y diálogo. Las primeras capturas negras permanecen solo en el diagnóstico privado, sin presentarse como prueba de renderizado.

Resultados: [Editor](editor-results.json), [Windows](windows-results.json), [F3A Editor](f3a-editor-results.json), [F3A Windows](f3a-windows-results.json), [F2 Editor](f2-editor-results.json), [F2 Windows](f2-windows-results.json), [reinicio](f2-restart-results.json), [build y hashes](build-results.json), [estado por criterio](criteria-status.json). [Reproducción](PRUEBAS.md).

Evidencia pública exclusivamente CC0: [antes de la carrera](evidence/before-catalog-Run.png), [después](evidence/after-catalog-Run.png), [muestras caminando](evidence/catalog-stride-1.4.gif), [muestras corriendo](evidence/catalog-stride-3.8.gif). Los GIF reúnen frames reales muestreados y no sustituyen el registro temporal CSV. Capturas del creador y diálogo de Windows se incluyen en `evidence/` solo cuando muestran el catálogo.

## Evidencia y límites

Todas las capturas y secuencias del VRM, incluso la referencia independiente, se conservan exclusivamente en `UnityProjectIsekaid/LocalVrmEvidence/Presentation/`, excluido de Git. No se publican el archivo privado, sus derivados, capturas, metadatos extensos o builds que lo incluyan. El informe puede incluir mediciones y hashes; no son una autorización de redistribución.

Las secuencias de zancada son renders reales de Unity con cámara de inspección temporal; las capturas de F2/F3A muestran la UI real. La velocidad horizontal de un hueso cercano al suelo es una medida diagnóstica: incluye parte de despegue/aterrizaje y no prueba por sí sola contacto físico o ausencia de deslizamiento. No hay un sistema de bloqueo de pies al terreno ni giros artísticos específicos. La calidad de pisadas, rodillas, cabello y ropa requiere revisión visual de Dirección y no convierte estos clips reutilizados en arte final.

La corrección se prueba con Adventurer y un VRM autorizado; no certifica todos los avatares VRM, terrenos irregulares, escalas extremas o equipos Windows. Se mantienen fuera de alcance las diferencias narrativas de personalidad y la aprobación completa de F2/F3A. No fusionar automáticamente.

## Archivos afectados

Runtime: `CompanionAppearance.cs`, `VrmAppearance.cs` y el nuevo `AvatarLocomotion.cs`. Recursos: nuevo directorio `Assets/_Project/Locomotion/` con copias FBX/metas, Avatar y controlador; referencias del prefab propio `AvatarCatalog/Adventurer.prefab` y de `CharacterCreation.unity`/`Game.unity`, más sus valores de iluminación. No se cambian los GUIDs externos de escenas o prefab.

Preparación: `AvatarIntegrationBuilder.cs`, `PrototypeSceneBuilder.cs`, `TwoSceneBuilder.cs` y nuevo `LocomotionBuilder.cs`. Validación: `PresentationAudit.cs`, `PresentationChecks.cs` y sus metas, además del visor de herramientas `tools/vrm-reference/`. Documentación: este directorio de auditoría y `readme.txt`; se actualiza también el registro central solicitado fuera del checkout. El manifiesto enumera archivos y hashes locales. No hay referencias nuevas que conectar manualmente en Inspector: los recursos y referencias ya están guardados.
