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
