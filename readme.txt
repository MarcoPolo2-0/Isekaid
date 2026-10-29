ISEKAID - REGISTRO DE CAMBIOS
Proyecto: C:\Users\PC\Documents\Game Idea\Isekaid
Guía de referencia: GameDesignDocument_v0.5.md

REGLA DEL PROYECTO
Desde el 27/10/2026, cada cambio del proyecto debe documentarse aquí.
Agregar entradas cronológicas sin borrar el historial. Indicar fecha,
qué cambió, archivos o áreas afectados, validaciones y pendientes.
Distinguir lo implementado de lo propuesto y no dar por realizada una
prueba que no se haya ejecutado.

======================================================================
27/10/2026 - COMPANION PROTOTYPE 0.1 - PRIMERA BASE FUNCIONAL
======================================================================

Objetivo
Comenzar en Unity 6.6 (6000.6.7f1) una escena acotada para probar el
primer encuentro y las interacciones con la compañera, según la guía v0.5.

Implementado
- Estructura Assets/_Project con código runtime y herramientas de editor
  en ensamblados separados.
- Escena CompanionPrototype: claro de prueba, obstáculos y NavMesh.
- Movimiento en primera persona con el Input System existente.
- Maniquí provisional con cabello largo, top, pantalón y ojos; colores
  editables durante la sesión.
- Personalidades tsundere, yandere y tímida con diálogos locales diferentes,
  independientes de la apariencia.
- Rescate inicial de una trampa de bandidos, sin temporizador de muerte.
- Recuerdo único del primer rescate y confianza inicial moderada, sin
  duplicarla al repetir interacciones ni activar romance automáticamente.
- Órdenes de seguir y esperar; navegación alrededor de obstáculos.
- Ensayo de incapacitación con 30 segundos de juego activo para reanimar.
  La pausa detiene la cuenta. Agotar el tiempo provoca Game Over.
- Reinicio del encuentro de prueba; no es un sistema de checkpoints.
- Panel de pausa, personalización, estado e indicaciones de interacción.
- Referencias conectadas en escena y prefabs.
- Escena añadida al inicio de EditorBuildSettings, conservando SampleScene.

Correcciones durante la construcción
- Importación del tema de UI Toolkit.
- Inicialización de la personalidad antes de crear la interfaz.
- Detención inmediata al ordenar esperar, sin desplazamiento residual.
- Collider vinculado a la postura del maniquí para que la interacción de
  reanimación coincida con el cuerpo visible cuando está caído.

Archivos y áreas principales (rutas relativas a Isekaid)
- Assets/_Project/Scenes/CompanionPrototype.unity
- Assets/_Project/Scripts/Companion/CompanionState.cs
- Assets/_Project/Scripts/Companion/CompanionController.cs
- Assets/_Project/Scripts/Companion/CompanionAppearance.cs
- Assets/_Project/Scripts/Player/FirstPersonController.cs
- Assets/_Project/Scripts/Prototype/CompanionPrototype.cs
- Assets/_Project/Scripts/UI/PrototypeHud.cs
- Assets/_Project/Scripts/Isekaid.Runtime.asmdef
- Assets/_Project/Editor/PrototypeSceneBuilder.cs
- Assets/_Project/Editor/PrototypeChecks.cs
- Assets/_Project/Editor/PrototypePlayModeChecks.cs
- Assets/_Project/Editor/Isekaid.Editor.asmdef
- Assets/_Project/Prefabs/FirstPersonPlayer.prefab
- Assets/_Project/Prefabs/Companion.prefab
- Assets/_Project/UI/Prototype.uss
- Assets/_Project/Settings/PrototypeTheme.tss
- Assets/_Project/Settings/PrototypePanelSettings.asset
- Assets/_Project/Settings/ClearingNavigation.asset
- Assets/_Project/Materials/ y archivos .meta correspondientes.
- ProjectSettings/EditorBuildSettings.asset

Validación realizada
- Compilación mediante Unity y comprobaciones de referencias de escena.
- Comprobación del recuerdo único y de los estados de supervivencia.
- Comprobación de una ruta completa alrededor del obstáculo central.
- Pruebas automatizadas en Play Mode: rescate manteniendo E, seguimiento,
  espera, pausa del temporizador, reanimación, Game Over y reinicio.
- Comprobación de interacción sobre la postura de incapacitación.
- Ejecución de las pruebas con renderizador gráfico habilitado.
Resultados registrados:
  ISEKAID_PROTOTYPE_CHECKS_PASSED
  ISEKAID_PROTOTYPE_PLAYMODE_CHECKS_PASSED
No se completó una revisión visual ni se obtuvo una captura utilizable.
La ejecución con gráficos no demuestra por sí sola la calidad visual.
No se generó un ejecutable de distribución.

Pendientes y limitaciones
- Revisión visual de escena e interfaz.
- Modelo anime definitivo, rig, deformaciones, expresiones y animaciones.
- Combate, clases funcionales, enemigos y alertas de peligro.
- Secuestros, convivencia, progresión y desarrollo de la historia.
- Guardado y checkpoints; los ajustes actuales duran esta sesión.
- Investigar el aviso de limpieza de DataSaveManager_GO, relacionado con
  UGameStudio, al cerrar la escena.

Cómo probar
Abrir Assets/_Project/Scenes/CompanionPrototype.unity y pulsar Play.
WASD: moverse. Ratón: mirar. Shift: correr. Espacio: saltar.
Mantener E: interactuar. F: alternar seguir/esperar.
Tab o Esc: pausa y ajustes.

======================================================================
27/10/2026 - REGLA DE DOCUMENTACIÓN Y REGISTRO INICIAL
======================================================================
- Inicializado el readme.txt existente, que estaba vacío.
- Registrada la primera versión, sus validaciones y sus limitaciones.
- Establecida la regla de documentar cada cambio futuro en este archivo.
- Cambio exclusivamente documental; no se modificó la jugabilidad.

======================================================================
27/10/2026 - COMPANION PROTOTYPE 0.2 - CREADOR Y AVENTURA SEPARADOS
======================================================================

Objetivo
Separar la creación del personaje y la aventura en dos escenas conectadas,
conservar la selección y reducir trabajo innecesario de interacción.

Implementado
- CharacterCreation.unity como escena inicial del proyecto.
- Vista previa 3D de cuerpo completo, plataforma e iluminación propia.
- Control de giro para inspeccionar el modelo sin iniciar la aventura.
- Selección de personalidad y colores de cabello, ojos, top y pantalón.
- Indicador visual de la opción de color seleccionada.
- Perfil local versionado con validación de personalidad e índices de color.
  Se guarda como JSON en PlayerPrefs mediante CompanionProfileStore.
- Botón Guardar y comenzar aventura: guarda el diseño y carga Game.unity
  de forma asíncrona, evitando envíos repetidos durante la transición.
- Game.unity conserva el claro, el primer rescate, las conversaciones,
  el seguimiento y el ensayo de supervivencia del prototipo anterior.
- El juego aplica automáticamente la apariencia y personalidad guardadas.
- La aventura comienza con el movimiento habilitado; Tab abre la pausa.
- Menú de pausa simplificado: continuar, ensayo de incapacitación,
  reiniciar encuentro y volver al creador.
- Volver al creador recupera el diseño. Al continuar comienza un nuevo
  encuentro; el progreso de la aventura todavía no se guarda.
- Solo CharacterCreation y Game quedan habilitadas en Build Settings.
  CompanionPrototype y SampleScene se conservan deshabilitadas.

Optimización y ajustes
- Retirada la personalización duplicada del menú de pausa.
- Paletas y nombres centralizados para no duplicar los mismos valores.
- La vista previa no ejecuta NavMeshAgent ni CompanionController.
- El controlador de juego consulta el rayo de interacción cuando hay una
  acción de interactuar u ordenar, en lugar de hacerlo cada fotograma.
  El aviso del HUD conserva su actualización cada 0.1 segundos.
- Se mantiene MaterialPropertyBlock para cambiar colores por personaje.
- Reubicado el control de giro para no tapar los pies en la vista previa.
No se midieron FPS, consumo de memoria ni una mejora porcentual de rendimiento.

Archivos nuevos (rutas relativas a Isekaid)
- Assets/_Project/Scenes/CharacterCreation.unity y su .meta
- Assets/_Project/Scenes/Game.unity y su .meta
- Assets/_Project/Prefabs/CompanionPreview.prefab y su .meta
- Assets/_Project/Scripts/Companion/CompanionProfile.cs y su .meta
- Assets/_Project/Scripts/Companion/CompanionPalette.cs y su .meta
- Assets/_Project/Scripts/Companion/CompanionProfileStore.cs y su .meta
- Assets/_Project/Scripts/UI/CharacterCreationController.cs y su .meta
- Assets/_Project/Editor/TwoSceneBuilder.cs y su .meta

Archivos modificados
- Assets/_Project/Scripts/Companion/CompanionAppearance.cs
- Assets/_Project/Scripts/Companion/CompanionController.cs
- Assets/_Project/Scripts/Prototype/CompanionPrototype.cs
- Assets/_Project/Scripts/UI/PrototypeHud.cs
- Assets/_Project/UI/Prototype.uss
- Assets/_Project/Editor/PrototypeSceneBuilder.cs
- Assets/_Project/Editor/PrototypeChecks.cs
- Assets/_Project/Editor/PrototypePlayModeChecks.cs
- ProjectSettings/EditorBuildSettings.asset
- Este readme.txt, agregando esta entrada sin borrar el historial.
Las referencias del creador, cámara, modelo y UI quedaron conectadas.

Validación realizada
- Compilación de los scripts en Unity 6000.6.7f1.
- Referencias serializadas y ausencia de scripts faltantes en ambas escenas.
- Serialización del perfil y rechazo de índices o versiones no compatibles.
- Pruebas automatizadas en Play Mode:
  creación -> juego -> regreso al creador -> segundo inicio del juego.
- Verificación de los cuatro colores y de la personalidad entre escenas,
  después de reiniciar el encuentro y al volver a comenzar.
- Regresión de rescate con E, seguimiento alrededor del obstáculo, espera,
  pausa del temporizador, reanimación, Game Over y reinicio.
- El runner automático restaura el perfil que existía antes de la prueba.
Resultados registrados:
  ISEKAID_PROTOTYPE_CHECKS_PASSED
  ISEKAID_TWO_SCENES_CREATED_AND_VALIDATED
  ISEKAID_TWO_SCENE_PLAYMODE_CHECKS_PASSED
- Revisión visual real en Game View, en formato 16:9: creador, cambio de
  cabello mediante botones, giro, entrada al juego, pausa con Tab y regreso
  al creador. Se revisó el ajuste final de posición del control de giro.

Cómo probar esta versión
Abrir Assets/_Project/Scenes/CharacterCreation.unity y pulsar Play.
Elegir personalidad y colores. Usar Girar para inspeccionar el modelo.
Pulsar Guardar y comenzar aventura.
Dentro del juego, usar Tab para abrir la pausa y volver al creador.

Pendientes y límites
- El personaje continúa siendo un maniquí; no se creó un modelo anime,
  rig, Blend Shapes ni nuevas animaciones en esta versión.
- No se ha validado la interfaz en todas las resoluciones o dispositivos.
- El guardado actual cubre la personalización, no el progreso de la aventura.
- No se generó un ejecutable de distribución ni se midió rendimiento real.
- Combate, secuestros, convivencia e historia completa siguen pendientes.

======================================================================
27/10/2026 - WORKFLOW DE DESARROLLO 1.0
======================================================================
Proyecto: Isekaid. Motor: Unity 6.
Documento base: GameDesignDocument_v0.5.md.
Workflow establecido por el usuario para coordinar tres chats especializados.

Roles
Chat 1 - Dirección del proyecto (Cerebro)
- Recibe ideas, consulta el GDD y las decisiones existentes.
- Establece prioridades, asigna IDs y define objetivo, alcance, restricciones
  y resultado esperado de cada tarea.
- Revisa especificaciones, aprueba el inicio del desarrollo y evalúa entregas.
- Acepta o rechaza resultados y determina correcciones y la siguiente tarea.

Chat 2 - Game Design
- Transforma los requisitos en especificaciones técnicas y creativas.
- Mantiene el GDD y registra las decisiones aprobadas.
- Define comportamientos, referencias visuales, dependencias, casos especiales
  y criterios verificables de aceptación.
- Entrega especificaciones versionadas. No modifica código fuente.

Chat 3 - Unity Development (función de este chat)
- Implementa las especificaciones aprobadas en Unity 6.
- Crea e integra assets y escribe código C# con arquitectura modular.
- Conserva los sistemas existentes reutilizables y avanza por hitos pequeños.
- Ejecuta pruebas y produce builds y evidencia visual cuando corresponda.
- Registra resultados técnicos, archivos afectados y problemas conocidos.
- Realiza commits y push a GitHub cuando corresponda a la tarea.
- No modifica unilateralmente los requisitos de diseño.

Flujo obligatorio
1. Solicitud: Dirección crea una tarea con ID, objetivo, alcance, prioridad,
   restricciones y resultado esperado.
2. Diseño: Game Design prepara la especificación versionada, incluyendo
   comportamiento, requisitos visuales, dependencias, casos especiales y
   criterios verificables de aceptación.
3. Aprobación: Dirección asigna PROPUESTA, APROBADA o REQUIERE CAMBIOS.
   Solo una especificación APROBADA pasa a desarrollo.
4. Implementación: Unity Development ejecuta la especificación aprobada,
   reutiliza sistemas existentes y trabaja por hitos pequeños.
5. Pruebas: entrega funciones implementadas, archivos modificados, pruebas
   y resultados, evidencia visual real cuando corresponda, problemas conocidos
   y commit asociado si existe.
6. Validación: Dirección compara la entrega con los criterios y asigna
   ACEPTADO, REQUIERE CORRECCIONES, BLOQUEADO o REDISEÑO NECESARIO.
7. Cierre: una tarea ACEPTADA se documenta y se marca como completada.
   Una idea que cambie un requisito aprobado se registra como cambio de alcance.

Reglas generales
- Mantener la visión aprobada; una simplificación necesita consentimiento.
- Identificar los assets provisionales como tales.
- No declarar terminada una funcionalidad que no haya sido verificada.
- No implementar simultáneamente múltiples sistemas grandes.
- No introducir dependencias de APIs generativas de pago dentro del juego.
- Mantener Unity compilable y una versión estable del proyecto.
- Evitar que dos chats modifiquen el mismo archivo simultáneamente.
- Priorizar la compañera, su personalización y sus interacciones.
- Los documentos aprobados y los informes de implementación son la fuente
  de verdad compartida entre chats.
- El chat principal decide qué tarea sigue.
- Se mantiene la regla de documentar cada cambio en este readme.txt,
  conservando el historial y distinguiendo validaciones de pendientes.

Resultado de esta entrada
- Workflow 1.0 registrado como regla de trabajo del proyecto.
- Este chat asume Unity Development; las nuevas implementaciones se reciben
  mediante una tarea y especificación APROBADA de Dirección.
- Actualización documental, sin cambios de código, escenas o assets.
- No se ejecutaron pruebas nuevas por esta actualización documental.

======================================================================
27/10/2026 - DIRECCION DE DISENO - GDD 0.6 BORRADOR Y ACEPTACION DE COMPANERA
======================================================================

Objetivo
Preservar la vision del GDD y definir resultados verificables para mejorar
la presencia visual, personalizacion y comportamiento de la companera.

Documentos entregados (rutas relativas a Game Idea)
- Documentation/GameDesignDocument_v0.5.md
- Documentation/GameDesignDocument_v0.6_borrador.md
- Documentation/Especificacion_Companera_v0.1.md
- Documentation/references/companion-concept-v01.png

Origen y estado
- El archivo v0.5 ya no estaba en Downloads. Se recupero su texto integro
  desde una lectura local anterior del 27/10/2026, sin reconstruir su
  contenido por suposiciones. No se certifica identidad binaria con el
  archivo original desaparecido.
- La v0.6 conserva el cuerpo de la v0.5 y agrega revision de vision,
  trazabilidad, diagnostico de la version tecnica 0.2 y prioridades.
- Las decisiones aprobadas no se sustituyen. Los nuevos rangos, tiempos,
  contenidos y criterios de direccion se distinguen como propuestas o
  verificaciones derivadas; el borrador no equivale a aprobacion.
- Especificacion de siete sistemas y 34 criterios: modelo, personalizacion,
  rostro, cuerpo/persona, dialogo, memoria y autonomia.
- Cada sistema define objetivo, datos, situaciones y evidencia de aceptacion.
- La lamina v01 se conserva como referencia candidata, no como arte final.
- Los hitos H1/H2/H3 ordenan la entrega sin eliminar clases, amistad,
  romance opcional, hogar, misiones personales ni secuestros.

Revision y validacion documental
- Leidos el GDD, el registro actual y los resultados de desarrollo 0.2.
- Inspeccion de solo lectura de codigo propio y version de Unity.
- Comprobada igualdad del cuerpo heredado de la v0.5 en la v0.6.
- Comprobados 34 identificadores unicos, referencias entre criterios y
  16 enlaces locales del paquete segun sus destinos.
- Comprobada la copia de la referencia y de los cuatro archivos mediante
  hashes entre la preparacion y su destino.
- Ninguna prueba nueva de aceptacion del producto se declara ejecutada.

Limites y pendientes
- Revisar identidad visual definitiva y referencias anime originales.
- Aprobar extremos corporales, gestos, catalogo facial y parametros de ensayo.
- Definir plataforma, hardware objetivo, guardado de recuerdos y primera clase.
- No se modificaron codigo, escenas, prefabs, ajustes ni assets de Unity.
- No se ejecutaron compilacion, Play Mode, build ni medidas de rendimiento.
- No se envio el encargo al chat de desarrollo; queda preparado en el GDD.

======================================================================
27/10/2026 - WORKFLOW 1.0 - ADOPCION EN GAME DESIGN
======================================================================

- Guardado Documentation/Workflow_Desarrollo_v1.0.md con el workflow
  proporcionado por el usuario para los tres roles.
- Este registro corresponde a Chat 2 - Game Design: mantiene GDD,
  especificaciones, referencias y criterios; no modifica codigo fuente.
- Direccion (Cerebro) asigna IDs, prioridades, aprobaciones de desarrollo,
  validacion de entregas y la siguiente tarea.
- GDD v0.6 borrador y Especificacion_Companera v0.1 permanecen PROPUESTA,
  con ID de tarea pendiente de asignacion por Direccion.
- H1/H2/H3 son propuestas de secuencia, no IDs de tarea ni autorizaciones.
- Diferenciadas las marcas de requisito/evidencia A/D/P/V de los estados
  formales de aprobacion y validacion del workflow.
- Revision administrativa 1: sin cambios en las mecanicas, el cuerpo
  heredado de la v0.5 ni los 34 criterios de aceptacion.
- Verificado el contenido heredado, la seccion de requisitos conservada,
  los 34 IDs unicos y los siete estados formales del workflow.
- Comprobados hashes previos para evitar sobrescribir cambios concurrentes
  en los documentos; copias de entrega verificadas por hash.
- No se asignaron tareas ni se enviaron instrucciones a otros chats.
- No se modificaron codigo, escenas, prefabs ni assets de Unity.
- No se ejecutaron nuevas pruebas del juego.

======================================================================
28/10/2026 - GAME DESIGN - ISEK-001 CON WOMAN.FBX
======================================================================

- Incorporadas por avance directo las decisiones y el workflow publicados
  por Direccion en el repositorio principal; sin cambiar el puntero Unity
  ni sus archivos locales pendientes.
- Creada Documentation/Specifications/ISEK-001.md, version 0.1, PROPUESTA.
- La eleccion de woman.fbx como candidato principal procede de DEC-001.
- Definidos silueta neutral, pecho y caderas independientes, cintura
  condicionada a viabilidad, ropa compatible, perfil y animaciones minimas.
- Definida matriz de 9 combinaciones, o 27 con cintura, seis poses criticas
  y barridos continuos; cuatro puertas de evaluacion y 16 criterios.
- Vinculados ISEK-001 y DEC-001 desde el GDD v0.6 y la especificacion general.
- Actualizada la ubicacion documental y ocho enlaces hacia Isekaidunity.
- Conservados los acuerdos del GDD v0.5 y el estado PROPUESTA de los borradores.

Validacion documental y del archivo recibido
- Confirmados 16 IDs unicos y 31 enlaces locales de los documentos revisados.
- Confirmado que el cuerpo heredado de la v0.5 no cambio.
- woman.fbx: 639772 bytes, cabecera FBX binario 7400.
- SHA-256: 674C07FF612577A1A5B7E7EE124202722FA5EB0F68BDC01C820A875B5BEF1D87.
- Confirmado que el FBX original conserva ese hash tras la entrega.
- La fuente Sketchfab devolvio HTTP 403; la licencia se registra como
  informacion de DEC-001, sin afirmar verificacion independiente nueva.

Limites
- No se inspeccionaron topologia, rig o Shape Keys en Blender.
- No se modificaron FBX, codigo, escenas, prefabs ni assets de Unity.
- No se ejecutaron compilacion, Play Mode, build ni pruebas de deformacion.
- Ninguna puerta tecnica ni criterio de producto se declara superado.
- La especificacion queda preparada para revision de Direccion.

======================================================================
28/10/2026 - ENTREGA FORMAL DE DISENO ISEK-001 V0.1
======================================================================

- Confirmado que el trabajo actualizado estaba guardado localmente, sin
  un commit previo ni otra rama remota que contuviera esta entrega.
- Preparada la publicacion documental de ISEK-001 v0.1 en el repositorio
  principal MarcoPolo2-0/Isekaid, conservando su estado PROPUESTA.
- Especificacion general y GDD v0.6 referencian expresamente DEC-001:
  modelo Woman de Fly_jkey, apariencia neutral equilibrada, pecho y
  caderas independientes y cintura condicionada a viabilidad tecnica.
- Identificados ISEK-001-P01 a P04 para revisar silueta y extremos,
  controles y perfil, expresiones y animaciones, y alcance de las puertas.
- Verificados 16 criterios unicos, cuatro puntos de aprobacion,
  31 enlaces locales y conservacion del cuerpo heredado de la v0.5.
- El hash de woman.fbx coincide con la referencia registrada.
- Alcance de la entrega: Documentation/Specifications/ISEK-001.md,
  Documentation/Especificacion_Companera_v0.1.md,
  Documentation/GameDesignDocument_v0.6_borrador.md y este registro.
- No se aprueba la especificacion ni se declara viable el FBX por esta
  publicacion. Revision de Direccion y validacion tecnica pendientes.
- No se modifican decisiones aprobadas, codigo ni puntero del submodulo.
- No se ejecutaron Blender, Unity, pruebas de deformacion ni builds.

======================================================================
28/10/2026 - ISEK-001 - AUDITORIA TECNICA DE WOMAN
======================================================================
Alcance autorizado por Direccion: auditar assets/woman.fbx; no iniciar
implementacion definitiva ni modificar los sistemas del proyecto Unity.

Referencias consultadas
- Documentation/DECISIONS.md, DEC-001, y workflow 1.0, contrastados con GitHub.
- Issue ISEK-001 del principal: https://github.com/MarcoPolo2-0/Isekaid/issues/1.
- Documentation/Specifications/ISEK-001.md, version 0.1 en estado PROPUESTA.

Pruebas y resultados
- Blender 5.2.3 LTS realmente ejecutado desde su alias de Microsoft Store.
- Original: una malla conectada, 11022 vertices, 11020 caras cuadrilaterales,
  22040 triangulos; sin bordes abiertos, aristas no manifold ni caras
  degeneradas detectadas con los umbrales documentados.
- El original no contiene rig, pesos, animaciones ni Shape Keys.
- Se detectaron una textura normal ausente y una escala que requiere revision.
- Copia diagnostica normalizada a 1.8942 m, sin fijar una altura de diseno.
- Creadas seis Shape Keys locales de ensayo para pecho, caderas y cintura;
  probadas 27 combinaciones en reposo, sin coordenadas invalidas o triangulos
  colapsados bajo el umbral de la prueba.
- Rig temporal de 19 huesos con pesos automaticos: cero vertices corporales
  sin peso. Ensayadas seis poses en tres combinaciones de proporciones.
- Las muestras abiertas de ropa presentan muestras de distancia negativa;
  pesos automaticos y flexiones muestran estiramientos que necesitan correccion.
  No se consideran prendas ni rig finales ni rangos artisticos aprobados.
- Reapertura del .blend y reimportacion del FBX derivado en Blender comprobadas.
- Prueba en un proyecto Unity aislado: Avatar valido y Humanoid, seis Blend
  Shapes por malla y deformacion corporal no nula verificada mediante BakeMesh.
- La importacion de prueba no equivale a Play Mode, animaciones finales,
  materiales URP definitivos ni integracion en el creador/aventura reales.
- El rostro original no tiene ojos ni boca modelados: requiere rediseno local,
  nuevos recursos faciales y trabajo de expresiones antes de ser un rostro anime.

Entrega
- Documentation/Audits/ISEK-001/INFORME.md.
- Inventario, resultados numericos por combinacion/pose y hashes de artefactos.
- .blend original de inspeccion, .blend de ensayo y exportacion FBX diagnostica.
- Vistas ortograficas y de topologia, 27 formas, 18 poses, laminas comparativas
  etiquetadas y captura real de la importacion aislada en Unity.
- Scripts reproducibles de auditoria y de la prueba de importacion.
- Este readme.txt: entrada agregada conservando su historial anterior.

Dictamen
Woman es una base corporal editable viable con condiciones. Recomendado
continuar con ella, revisar la neutral corporal y trabajar localmente el rostro,
rig/pesos y ropa. Cintura puede continuar como candidata tecnica; no se declara
aceptada. ISEK-001 sigue abierta y requiere revision de Chat 1 junto con Chat 2.

Proteccion y limites
- woman.fbx conserva SHA-256
  674C07FF612577A1A5B7E7EE124202722FA5EB0F68BDC01C820A875B5BEF1D87.
- No se modificaron archivos ni sistemas definitivos de Isekaidunity.
- No se sustituyo Woman por otro recurso, ni se contrato un servicio de pago.
- No se cambio el puntero del submodulo ni se publico en el Issue.
- Licencia CC BY registrada segun DEC-001; version exacta pendiente antes
  de distribuir derivados. Sin aceptacion artistica, build o rendimiento medido.

======================================================================
28/10/2026 - VERSIONADO DE LA AUDITORIA ISEK-001
======================================================================
- Por solicitud del usuario, preparado el lote de auditoria para git add .,
  commit y push en el repositorio principal MarcoPolo2-0/Isekaid, rama main.
- Alcance: Documentation/Audits/ISEK-001 y esta entrada de readme.txt.
- Se conservan los cambios locales previos del submodulo Isekaidunity;
  el versionado del repositorio principal no confirma sus archivos internos.
- Cambio de control de versiones, sin modificaciones nuevas del producto.
- Las verificaciones tecnicas de la auditoria constan en su informe.
  No se repiten compilacion, Play Mode ni builds por esta operacion.

======================================================================
28/10/2026 - ISEK-001 - PROPUESTA ARTISTICA V0.1 TRAS DEC-002
======================================================================

- Revisadas DEC-002 y auditoria tecnica Woman v1.0 publicadas por Direccion
  y Desarrollo; G1 aceptada tecnicamente y G2-PROTO autorizado con limites.
- Preparada Documentation/Specifications/ISEK-001_Direccion_Artistica_v0.1.md
  en PROPUESTA, sin declarar cuerpo definitivo ni ampliar G3/G4.
- Cuatro laminas conceptuales: alternativas A/B, neutral en cuatro vistas,
  extremos de tres ejes y ocho muestras faciales; recomendada alternativa A.
- Referencias guardadas en Documentation/references/ISEK-001-art-v0.1/,
  con briefs completos, origen y limites frente a las evidencias reales.
- Diez criterios visuales y cinco puntos de decision para Chat 1.
- Vinculadas la propuesta y DEC-002 desde ISEK-001 y GDD v0.6 borrador.
- Verificados 46 enlaces locales, cuatro briefs y conservacion del cuerpo
  heredado del GDD v0.5; el hash original de woman.fbx sigue coincidiendo.
- Registrados limites de las ilustraciones: costuras/escote a corregir,
  diferencias discretas de caderas y expresiones pendientes en 3D real.
- No se ejecutaron Blender, Unity, deformaciones, animaciones ni builds;
  no se modificaron codigo, FBX, evidencias de auditoria ni submodulo.


28/10/2026 - ISEK-001 / G2-PROTO - ENTREGA PARA REVISION
- Rama privada feature/isek-001-g2-body: neutral derivada, seis Shape Keys, pesos normalizados y muestras de ropa corregidas en reposo.
- Blender editable, FBX, scripts, informe y 108 vistas de 27 combinaciones en SourceArt/Companion/Woman/G2-PROTO; recurso importable aislado en Assets/G2BodyPrototype.
- Unity 6000.6.7f1 aislado: Humanoid valido, 27 combinaciones; original preservado y 58 archivos existentes sin cambios.
- Pendiente aprobacion visual de neutral/rangos; ropa en poses mantiene penetraciones hasta 13.70 mm. G2 no se declara aceptada.
- Sin sustitucion del maniqui, cambios de escenas definitivas, fusion a main ni actualizacion del puntero de submodulo.
- Publicado commit privado afcdd61649bac3cabc833605d61037006eaf6509 y PR borrador https://github.com/MarcoPolo2-0/Isekaid-2026-10-27_13-09-18/pull/2
- Informe comunicado a Direccion en https://github.com/MarcoPolo2-0/Isekaid/issues/1#issuecomment-6474245300

28/10/2026 - ISEK-001 G2-CORR / DEC-003 - PR #2
- Neutral candidata y extremos recalculados sin reducir amplitudes maximas. Topologia preservada; 4330 vertices de regiones protegidas sin cambios neutrales.
- 27 combinaciones, 108 vistas, 15 barridos de 101 muestras con otros ejes neutros/extremos mixtos; 315 frames y tres GIF de evidencia.
- Pesos locales y holguras revisados; mismos 18 casos repetidos. Clipping maximo 13.70 -> 4.03 mm, persiste en 6 casos. Cruces de triangulos medidos, no se ocultan. Estiramiento maximo 3.584 frente a 3.578: no resuelto.
- Unity aislado: Humanoid valido y controles sincronizados en cuerpo y dos muestras; 27 combinaciones + 1515 estados, 4629 horneados.
- Informe, estados AC/AV y hashes en SourceArt/Companion/Woman/G2-CORR. G2 requiere revision artistica; sin Play Mode ni builds.
- Version G2-PROTO recuperable intacta, 165 archivos protegidos sin cambios. Sin cambios de escenas/perfil/UI/runtime, fusion ni puntero de submodulo.
- Publicado G2-CORR: b329a3e510068ffef61c951143921cb3d9777d3b en el mismo PR borrador #2.
- Informe comunicado en https://github.com/MarcoPolo2-0/Isekaid/issues/1#issuecomment-6475338380

28/10/2026 - VRM-POC - PR experimental de importacion local
- UniVRM 0.131.4 fijado, VRM 1.0 local validado; escena independiente con adaptador de apariencia y expresiones.
- Baseline existente pasa en Play Mode antes y despues; falta tools/verify_unified_layout.py de ISEK-002.
- Humanoid, clips de ensayo, 14 expresiones, 25 Spring Bones y 9 rechazos de seguridad comprobados. Capturas reales privadas en LocalVrmEvidence.
- Informe y hashes: Documentation/Audits/VRM-POC. Original, Woman, G2 y 803 archivos protegidos sin cambios.
- Archivo VRM y capturas excluidos de Git por permisos; sin cambios de escenas/prefabs definitivos, perfil, gameplay o nuevo creador.
- Viabilidad tecnica acotada: requiere revision de escala/locomocion, perfilado standalone y derechos de recursos antes de integrar. Sin fusion a main.

28/10/2026 - VRM-POC / PR #4 - CORRECCION DE ESTABILIDAD
- Rama actualizada con main 83553e9 mediante merge; cambios locales anteriores respaldados y preservados.
- Reemplazo transaccional: candidato preparado y activado antes de publicar referencias y destruir el anterior; rollback de visibilidad y expresiones.
- Descarga explicita e idempotente; rechazo de operaciones simultaneas y limpieza al destruir el presentador durante la carga.
- Unity Play Mode: cinco fallos por etapa, rollback de primera carga, seis ciclos de carga/reemplazo/descarga y 1138 referencias a recursos comprobadas destruidas.
- Memoria asentada: crecimiento maximo 0.25 MiB; conteos de mallas/texturas/materiales/avatars constantes. La reserva del allocator permanece, sin afirmar ausencia absoluta de fugas ni VRAM validada.
- tools/verify_unified_layout.py y pruebas originales del creador/Game pasan. Informe, resultados y hashes actualizados en Documentation/Audits/VRM-POC.
- 803 archivos protegidos intactos; VRM y capturas siguen privados. Sin fusion a main, nuevo creador ni integracion de produccion.
28/10/2026 - ISEK-001 - SISTEMA VRM V0.2 / REVISION DE CHAT 1
- Direccion conceptual VRM + catalogo: APROBADA CON AJUSTES por instruccion
  directa de Chat 1; implementacion definitiva pendiente de Chat 3.
- Identidad, personalidad, memoria, clase y representacion independientes.
- Primera entrega funcional: un avatar incluido y uno importado VRM 1.0.
  Tres apariencias del catalogo son objetivo posterior; VRM 0.x pendiente.
- Conservados 26 criterios, distribuidos sin duplicados: F1 tecnica (5),
  F2 importacion/persistencia (11), F3 personalidad/expresiones (8),
  F4 catalogo completo (2). Los casos siguen sin ejecutar ni aceptar.
- Diferenciados limites tecnicos, permisos declarados y revision humana
  de derechos. Ningun resultado tecnico equivale a licencia validada.
- Tres personalidades y profundidad narrativa conservadas; Woman y ambos
  G2 quedan como trabajo historico, sin nuevas correcciones corporales.
- Version 0.1 conservada; version 0.2 incorpora la revision documental.
- Verificados IDs, particion de fases y cuatro enlaces locales de v0.2.
- No se cambia GDD v0.5, no se cierra ISEK-001 ni se ejecutan Unity,
  importacion, Play Mode o builds. Esperar evaluacion tecnica antes de
  solicitar aprobacion definitiva del cambio de alcance.
- Entrega mediante rama y PR documental, sin incluir el experimento Unity.

[2026-10-28] F2 VRM - implementacion experimental en curso
- Rama feature/isek-vrm-f2-character-creator creada desde main 480a8e9.
- Checkout independiente; cambios locales y documentos de Chat 2 preservados.
- Escenas originales y metadatos respaldados fuera del repositorio antes de modificar.
- Importador transaccional reutilizado en _Project/AvatarRuntime, conservando GUID.
- Catalogo local Adventurer con licencia CC0 publicada por Dawn to Dusk Games.
- Creador con importacion Windows, permisos declarados, muestras visuales y personalidad independiente.
- Perfil v2 con identidad, referencia visual separada, copia privada por SHA-256 y guardado con respaldo.
- El regreso al creador mantiene el encuentro en pausa; no reinicia memoria ni confianza existentes.
- Verificacion inicial estatica del prototipo: PASS. Play Mode base bloqueado por excepcion de UnityEditor.Search.SearchDatabase; no se atribuye al gameplay.
- Pruebas de integracion y revision visual en curso; esta entrada no declara cerrada F2.

[2026-10-28] F2 VRM - recorrido integrado y correcciones verificadas
- Pruebas en Play Mode: catalogo y VRM llegan a Game sin sustituir CompanionController.
- Conservados encuentro, instancia de estado, primer rescate, confianza, personalidad, velocidad y radio de navegacion durante el cambio visual.
- Corregidos metadatos VRM, encuadre tras evaluar Animator y HUD al volver del creador.
- Spring Bones usa el runtime independiente de UniVRM para evitar una fuga comprobada del combinador compartido en reemplazos; repeticion con detector nativo sin ese aviso.
- 195 recursos propios liberados en tres ciclos VRM/catalogo; cancelacion durante carga y fallos de preparacion/activacion/guardado comprobados.
- Nueve comprobaciones de migracion, conservacion de campos legacy y respaldo: PASS.
- PrototypeChecks y PrototypePlayModeChecks: PASS; asercion del creador actualizada a conservar el encuentro en pausa, segun F2.
- Verificados 715 archivos originales protegidos: cero cambios de contenido, incluidos Woman, SourceArt, prefabs y GUID de escenas.
- Capturas reales revisadas en 1920x1080, 1280x720 y 1600x900. Capturas con el VRM privado permanecen excluidas de Git.
- Build Windows y comprobacion fuera del Editor en curso; no se declara lista para lanzamiento ni se inicia F3.

[2026-10-28] F2 VRM - validacion de Windows y recuperacion en el encuentro
- Build Windows de desarrollo generada con Unity 6000.6.7f1 y probada fuera del Editor, sin incluir el fixture privado.
- Selector nativo real: seleccion y cancelacion verificadas; corregido el buffer Win32 para evitar un desbordamiento de pila en Mono.
- Ejecutable reiniciado en un segundo proceso sin el archivo fuente: apariencia gestionada y personalidad recuperadas; entrada a Game comprobada.
- Probado fallo de activacion al volver al encuentro: permanecen la apariencia anterior, el perfil guardado y la confianza; el creador permite reintentar.
- Regresion final: PrototypeChecks y PrototypePlayModeChecks PASS. Once aserciones de almacenamiento PASS, incluido fallo real de reemplazo por archivo bloqueado.
- Nueve rechazos heredados y dos pruebas de archivo local (tamano y version VRM no admitida) PASS.
- Avisos MIT y procedencia CC0 conservados junto al ejecutable; capturas VRM privadas excluidas de Git.
- F2 se entrega para revision con criterios parciales documentados; no se aprueba arte, no se inicia F3/F4 y no se fusiona main.

[2026-10-28] F2 VRM - informe y evidencias para revision
- Informe VRM-F2 con 11 criterios v0.2: 7 PASS y 4 PARTIAL, sin declarar cierre artistico o narrativo.
- Adjuntos resultados de Editor, Windows, selector, reinicio, guardados, rechazos y regresiones; manifiestos SHA-256 de implementacion, build, capturas locales y 715 originales protegidos.
- Capturas publicas limitadas al catalogo CC0. VRM de prueba, copias gestionadas, sus imagenes y builds permanecen fuera de Git.
- Publicados pasos reproducibles y comprobador de entrega. Main consultado de nuevo: permanece en 480a8e9; no hay nuevos cambios documentales que incorporar.
- Cambios automaticos de cache de URP durante la build retirados del checkout; se conserva el ajuste intencional de shaders para carga VRM.
- Se conserva una build Windows de revision local con avisos de terceros. Revision de Direccion, desconexion fisica de red y limitaciones futuras documentadas.


======================================================================
28/10/2026 - CHAT 2 / DEC-007 - VRM F2 Y PREPARACION DE GAMEPLAY F3
======================================================================
- Instruccion directa de Direccion formalizada: VRM 1.0 + catalogo local
  adoptados; F2 autorizada, no implementada ni aceptada por este documento.
- Confirmados PR #3 y #4 fusionados. POC leido como evidencia acotada de
  Play Mode/estabilidad, sin repetir Unity ni atribuirle persistencia/builds.
- DEC-007, enmienda explicita del GDD v0.6 y workflow nuevo v1.2 preparados.
  GDD v0.5 y versiones historicas sin cambios.
- ISEK-001 permanece abierto; Woman/G2 y editor corporal pausados y
  conservados. Seguimiento funcional F2 creado en Issue #5.
- Contrato F2: seleccion, importacion VRM, vista previa, estados, tres
  personalidades, migracion v1, copia/guardado y recuperacion; 18 casos.
- Una apariencia incluida y una importada suficientes para F2; recurso
  distribuible aun requiere derechos humanos revisados. No se elige asset.
- F3 borrador: seis capas, eventos/memoria, promesas, peligro, iniciativa,
  confianza y matriz contextual de tres personalidades; diez casos propuestos.
- Matriz mantiene 26 IDs y particion 5/11/8/2; F2-P01 a P04 y F3 son
  aclaraciones/propuestas, no bloquean ni amplian automaticamente a Chat 3.
- Verificacion documental: siete documentos, 67 enlaces locales, IDs unicos,
  particion conservada y cuerpo heredado de v0.5 preservado.
- Sin cambios de codigo, escenas, prototipos tecnicos, Woman/G2 ni recursos
  privados. No se ejecutan Unity, Play Mode, importacion, animaciones o builds.
- Rama docs/isek-vrm-f2-dec007 y PR hacia main para revision; sin fusion
  automatica. Se preservan entradas previas de readme de Desarrollo y Diseno.

[2026-10-28] PR #7 - incorporacion de la revision documental #6
- Integrados los commits del PR #6 (6261706), todavia abierto, dentro de la rama experimental F2; main no se fusiona ni se modifica.
- Conservadas todas las entradas de ambos readme.txt y los documentos historicos. Las propuestas F2-P01-P04 y F3 conservan su estado documental.

[2026-10-28] PR #7 - correccion del presupuesto de metadatos y verificacion del guardado
- VRMC_vrm.meta se reduce a una lista explicita de campos estandar: textos de identidad/derechos, autores, referencias y permisos booleanos. Se conserva completo el VRM privado original.
- Presupuesto persistido: 32 KiB UTF-8 en total; limites por campo, 16 autores y 8 referencias. Los textos legales demasiado largos se rechazan, no se recortan; se rechazan tipos incorrectos y controles no admitidos.
- El perfil completo conserva el limite de 1 MiB. Se comprueba el tamano final, incluidas codificaciones/escapes, antes de reemplazar archivos.
- El archivo temporal se lee con el mismo lector del perfil antes del commit atomico; despues se relee el destino y se verifica el payload esperado antes de devolver exito.
- 35 aserciones adversariales PASS, incluidas conservacion byte a byte del perfil/respaldo ante rechazo, esquema invalido, tamano escapado y roundtrip cerca del limite.
- VRM privado de prueba con JSON de 1,427,620 bytes: recorrido Editor PASS; perfil resultante de 1,088 bytes y metadatos de 370 bytes, sin persistir la propiedad desconocida de 1,300,000 caracteres.
- Regresion de prototipo, 11 aserciones previas de almacenamiento y guardas de archivos repetidas: PASS. Pruebas Windows/reinicio y CI se registran en el informe de revision.

[2026-10-28] PR #7 - resultados finales de la revision tecnica
- Recorrido Windows con el VRM adversarial PASS; segundo proceso sin copia fuente PASS. Perfil legible de 1,088 bytes, metadatos de 370 bytes, respaldo existente y campos desconocidos excluidos.
- Informe de revision, resultados, hashes del binario actualizado y generador reproducible de fixture privado agregados en VRM-F2/metadata-review.
- Se mantienen 7 criterios F2 PASS y 4 PARTIAL. Offline fisico, gameplay futuro, derechos y calidad visual no se declaran aprobados. No se publica el VRM derivado ni sus capturas.
- Cambios del PR #6 integrados conservando ambos registros; main permanece sin fusionar. La correccion se publica en el mismo PR #7.
28/10/2026 - CHAT 2 / PR #6 - CORRECCION DE REVISION DE DIRECCION
- Atendida revision 5692435453: retirada de workflow v1.2 la instruccion
  residual de reanudar G2, contradictoria con DEC-007.
- Siguiente puerta: revision/aceptacion de F2 y preparacion documental de
  F3; implementacion F3 requiere autorizacion posterior. Woman/G2 en pausa.
- GDD v0.5 e historial completo de readme de Desarrollo/Diseno conservados.
- Solo correccion documental; no se ejecutan Unity ni builds, ni se fusiona PR.

2026-10-29 — F3A: rama feature/isek-f3a-dialogue-runtime desde main 4bd6857 (PR #6/#7 fusionados). Autorización directa de Dirección para diálogo local limitado en Game; F2 sigue pendiente de pruebas manuales. Contenido provisional JSON con IDs estables, director por encuentro, opciones UI Toolkit y reacciones semánticas reutilizan control, avatar y pausa existentes. Sin cambios de escenas, GUID existentes, perfil o importador; no F3 completa, memoria persistente, romance ni combate. Pruebas en preparación; sin declarar funcionalidad verificada todavía.

2026-10-29 — F3A: primer recorrido real Play Mode superado en Unity 6000.6.7f1: personalidades, elecciones, rescate/reanimación, pausa, movimiento y cambios de apariencia. Capturas reales revisadas a 1920x1080, 1600x900 y 1280x720. Corrección local de entrada: Tab/Esc no reanudan Game detrás del creador aditivo. Se ajusta la presentación para no repetir Spoke al pausar/reanudar y conservar la petición de ayuda al salir del menú; pruebas adicionales y regresión F2 en curso. El Editor batch no acredita captura física del cursor.

2026-10-29 — F3A: 123 comprobaciones finales Play Mode PASS (editor-results.json); se confirma no repetición de Spoke por pausa y reacción de ayuda desde el menú. Regresión F2 con metadatos grandes PASS: 43 comprobaciones, 195 recursos liberados. Regresión original de dos escenas PASS; guardas: 9 rechazos y 2 límites de archivo; almacenamiento heredado: 11 aserciones PASS. Capturas públicas seleccionadas exclusivamente de Adventurer CC0; VRM y capturas privadas excluidos.

2026-10-29 — F3A: build Windows x64 Development Mono Succeeded (196,505,711 bytes), implementación 5f3da89. Ejecutable real: 123 comprobaciones F3A PASS; regresión F2 Windows: 36 PASS y segundo proceso sin fuente: 2 PASS. Metadatos adversariales: 35 PASS. Estructura y 715 archivos protegidos F2 sin cambios. Informes, hashes, capturas reales solo del catálogo y guía de revisión manual en Documentation/Audits/VRM-F3A/. Pendientes: juego manual en ventana enfocada/captura física del cursor, aprobación de textos/lectura emocional y estabilidad manual F2; se mantienen los cuatro criterios parciales F2 y las restricciones de derechos. Sin fusionar ni declarar F3 completa.


======================================================================
28/10/2026 - CHAT 2 / F3A - DISENO NARRATIVO Y REACCIONES CONTEXTUALES
======================================================================
- Autorizacion directa de Direccion: solo diseno/preparacion F3A.
  PR #6/#7 fusionados comprobados; F2 pendiente de revision manual humana.
- F3A_Especificacion_v0.1.md: alcance, voces, cursor, prioridades,
  repeticion, silencio y limites existentes/futuros diferenciados.
- F3A_Biblioteca_v0.1.json: 60 entradas unicas, 20 por personalidad;
  situaciones A/B/C/D/E/F = 3/27/9/9/6/6, sin efectos de confianza/memoria.
- F3A_Arboles_v0.1.md: trampa, estado personal y aldea, tres arboles;
  27 nodos alcanzables y 18 opciones/rutas, todas con cierre.
- F3A_Matriz_Emocional_v0.1.md: siete emociones x tres personalidades,
  canales preferidos y alternativas VRM/gesto, sin decidir desde morphs.
- F3A_Contrato_Eventos_v0.1.md: hechos actuales y adaptadores propuestos;
  peligro general/promesas no se declaran implementados.
- F3A_Aceptacion_v0.1.md: ocho criterios documentales/editoriales y
  doce pruebas runtime pendientes; no confundir JSON valido con gameplay.
- F3A_Propuestas_Futuras_v0.1.md: memoria persistente, promesas, confianza,
  percepcion y demas extensiones separadas de esta autorizacion.
- Verificacion estructural: IDs/textos distintos, campos y alternativas,
  grafos/rutas, efectos cero, matriz de siete emociones y 27 enlaces locales.
- Seguimiento F3A Issue #8, vinculado a ISEK-001 #1 y borrador F3.
- Rama docs/isek-f3a-dialogue-design y PR a main para revision; sin fusion.
- GDD, decisiones, documentos historicos y Unity sin modificar. No se
  ejecutan Unity, Play Mode, conversaciones en juego ni builds.
- Chat 3 continua infraestructura autorizada en paralelo; esta entrega no
  autoriza Fase 3 completa ni concede aceptacion manual de F2.

2026-10-29 — F3A revisión de Dirección: se integra la documentación y biblioteca canónica del PR #9 (6df539d) en la rama del PR #10, conservando completos ambos bloques de readme. Autorizada su adaptación local y corrección de continuidad/cursor, 18 recorridos y episodios de reanimación. Textos todavía sujetos a aprobación; sin ampliar F3, modificar confianza ni fusionar main.

2026-10-29 — F3A revisión PR #10: biblioteca runtime adaptada reproduciblemente desde las 60 entradas del PR #9; IDs, textos y 18 opciones conservados y efectos cero validados. Cursor/opciones/foco sobreviven al cierre voluntario, pausa, creador y suspensión vital; reanimación y retorno por episodio, órdenes sin respuesta a no-op, tres casuales con enfriamiento activo compartido. Se preparan pruebas de los 18 recorridos, doble incapacidad y cambios reales de avatar. Expresiones condicionales no certificadas usan neutral, sin inventar gestos o validación artística.

2026-10-29 — F3A revisión PR #10: Play Mode final PASS, 285 comprobaciones y 18 recorridos independientes. Verificados cierre/reapertura sin reemitir raíz, foco/opciones en creador catálogo→VRM→catálogo, doble reanimación por episodio, suspensión idempotente y callbacks con revisión obsoleta rechazados. Se retira el aviso transitorio antiguo al abrir el menú de temas. F3A-R10 y aprobación editorial/visual siguen parciales; regresión F2 y build revisada en preparación.

2026-10-29 — F3A revisión PR #10: CI comprobará también que el recurso runtime coincide con la biblioteca canónica mediante tools/build_f3a_runtime_content.py --check. Regresión F2 Editor PASS (43 comprobaciones y 195 recursos liberados); guardas 9+2, almacenamiento 11 y prototipo original de dos escenas PASS.

2026-10-29 — F3A revisión PR #10: primer ensayo Windows detectó ausencia de desplazamiento con un pulso de W de duración solo temporal, manteniéndose habilitada la entrada al cerrar. El ensayo ahora mantiene teclas al menos tres frames, espera liberación procesada y usa timeout; conserva la aserción de desplazamiento y registra metros/frames/entrada/pausa. Play Mode con el ensayo revisado PASS (285/18). Sin corrección adicional de gameplay para esa observación; Windows se repetirá en una nueva build.

2026-10-29 — F3A revisión PR #10: segundo ensayo Windows registró solo 0.0516 m en 4,216 frames con entrada habilitada y pausa desactivada. El prefab original tiene minMoveDistance=0.001 m: se documenta el caso de pasos submilimétricos a frecuencias extremas fuera de foco. El escenario de pruebas se acota a 60 FPS y restaura los ajustes al salir, sin modificar jugador/prefab/escenas; Play Mode final a 60 FPS PASS, 285 comprobaciones/18 recorridos y desplazamiento real 1.3158 m. Windows se repite con esa calibración.

2026-10-29 — F3A revisión PR #10: Windows final a 60 FPS PASS, 285 comprobaciones y 18 recorridos; desplazamiento W tras cierre 1.4591 m en 34 frames, entrada habilitada y sin pausa. DLL de gameplay idéntica a la primera build de esta revisión; build final 196,562,300 bytes. Regresión F2 Windows PASS (36), documentación nueva por criterio en Documentation/Audits/VRM-F3A/review-1/: 17 PASS y 3 PARTIAL (D08, R10, R11), sin convertir fixtures acotados en aprobación editorial o callbacks SDK inexistentes. Modelos/capturas VRM privados y originales preservados. Reinicio F2 y CI se comprueban al cierre.

2026-10-29 — F3A revisión PR #10: segundo proceso Windows F2 PASS (2 comprobaciones) sin copia fuente. Informe/hashes/capturas/criterios actualizados; fuente narrativa de Chat 2 intacta, ambos bloques de readme conservados y 715 originales protegidos. Se publica evidencia para revisión de Dirección, sin fusionar main ni dar por aprobada F2/F3.
