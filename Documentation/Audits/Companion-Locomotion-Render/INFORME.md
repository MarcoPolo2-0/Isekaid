# Corrección de locomoción y presentación F2/F3A

Fecha: 29/10/2026. Rama `fix/isek-companion-locomotion-vrm-render`, creada desde `main` `f657d78390202c2414a629d81401d8d801636bb1`. Solicitud: prueba manual parcial de Dirección, prioridad animación y después renderizado. No autoriza cambios narrativos ni la aceptación artística de F2/F3A.

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

En ejecución; se incorporarán resultados del mismo código antes de publicar la entrega. Ningún resultado de compilación sustituye Play Mode o ejecución de la build.

## Evidencia y límites

Todas las capturas y secuencias del VRM, incluso la referencia independiente, se conservan exclusivamente en `UnityProjectIsekaid/LocalVrmEvidence/Presentation/`, excluido de Git. No se publican el archivo privado, sus derivados, capturas, metadatos extensos o builds que lo incluyan. El informe puede incluir mediciones y hashes; no son una autorización de redistribución.

Las secuencias de zancada son renders reales de Unity con cámara de inspección temporal; las capturas de F2/F3A muestran la UI real. La velocidad horizontal de un hueso cercano al suelo es una medida diagnóstica: incluye parte de despegue/aterrizaje y no prueba por sí sola contacto físico o ausencia de deslizamiento. No hay un sistema de bloqueo de pies al terreno ni giros artísticos específicos. La calidad de pisadas, rodillas, cabello y ropa requiere revisión visual de Dirección y no convierte estos clips reutilizados en arte final.

La corrección se prueba con Adventurer y un VRM autorizado; no certifica todos los avatares VRM, terrenos irregulares, escalas extremas o equipos Windows. Se mantienen fuera de alcance las diferencias narrativas de personalidad y la aprobación completa de F2/F3A. No fusionar automáticamente.
