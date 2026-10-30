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

2026-10-29 — Prueba manual parcial F2/F3A recibida. Nueva rama fix/isek-companion-locomotion-vrm-render desde main f657d78. Se prepara medición previa de clips, tipos de rig, tiempo normalizado, alternancias de velocidad y materiales con catálogo y VRM autorizado; todavía sin corregir locomoción/renderizado ni alterar narrativa. Archivo VRM y capturas permanecen privados.

2026-10-29 — Locomoción: baseline reproducido en Play Mode con ambos avatares. Adventurer Idle/Walk/Run son Generic no cíclicos; Run dura 0.5833 s y la pose deja de avanzar al terminar. Ruido 2.96/3.04 m/s produjo 74/75 solicitudes y 222/203 frames en transición en cinco segundos. Se conservan POC y FBX/metas históricos; copias CC0 de tres fuentes, Avatar Humanoid mapeado y nuevo BlendTree Speed compartido. Velocidad suavizada 0.15 s y sin reiniciar estados. Primera repetición: cero cambios de estado, reinicios y frames de transición; clips Humanoid cíclicos en ambos. No implica todavía aceptación visual, contacto de pies ni regresiones completas.

2026-10-29 — Render: visor independiente local @pixiv/three-vrm 3.4.2 / three 0.180.0 cargó el VRM privado desde disco sin enviarlo a un servicio. La referencia muestra texturas y detalle presentes. Se prepara comparación controlada T-pose en Unity entre iluminación vigente y dos niveles; diagnóstico separado de retargeting. Todo resultado/captura del VRM queda en LocalVrmEvidence ignorado. Se conserva su hash y no se modifica licencia ni archivo.

2026-10-29 — Corrección de presentación: luz blanca 0.65 y ambiente uniforme 0.4 en creador/Game, tras comparación independiente y barrido controlado MToon (109,078 píxeles saturados antes, cero con equilibrio). Se descarta relleno 0.15 por oscurecer Adventurer. Sin cambiar shader, texturas, emisión, normales ni VRM. Constructores de escenas conservan el ajuste. Visor reproducible local en tools/vrm-reference con dependencias MIT fijadas por hash, captura exclusivamente ignorada y bloqueo de otros orígenes.

2026-10-29 — Calibración: Walk 1.61 y Run 0.70, escala efectiva Humanoid y Cadence para baja velocidad; reposo separado del BlendTree Walk/Run con umbrales 0.12/0.06 m/s y transiciones de 0.15 s. Se activa ajuste de pies Humanoid y se retira elevación histórica 0.0522 m del catálogo (malla en reposo medida después a ~0.002 m del origen). Importador conserva candidato transaccional y guarda Animator para evitar búsquedas por frame. Diagnóstico registra barridos, poses, zancada, secuencias reales y render controlado. Nuevos clips son copias idénticas de fuentes CC0 con nuevos metas; GUIDs existentes e históricos preservados. Batería final Editor/Windows/F2/F3A en marcha, sin afirmar todavía su resultado ni aprobación artística.

2026-10-29 — Validación final: locomoción Editor/Windows PASS sin reinicios ni cambios rápidos, F3A 285 comprobaciones/18 rutas por plataforma, F2 Editor 43 y Windows 36 + reinicio 2; guardas 9+2, almacenamiento 11, metadatos 35 y prototipo de dos escenas PASS. Build Windows x64 Development Mono Succeeded, 196,472,292 bytes. Se detecta que ScreenCapture de Windows devuelve imágenes negras al ocultar la ventana; se descartan esas capturas como evidencia visual y se repite la misma build en ventana visible. Los renders de inspección y Editor son válidos. No se cambia código para esta repetición; se registran incidencias internas del Editor Search/Hierarchy y se retiran solo prefiltros/serialización URP generados por la build.

2026-10-29 — Cierre técnico de corrección: misma build repetida en ventana visible, presentación + F3A 285/18 + F2 36 + reinicio 2 PASS; capturas de creador/diálogo inspeccionadas y con imagen real. JSON definitivos Windows reemplazan la primera adquisición oculta. Informe, reproducción, estado por criterio, hashes y capturas/secuencias CC0 en Documentation/Audits/Companion-Locomotion-Render; VRM/referencia/capturas privadas siguen ignorados y SHA original 37eadc...8736 intacto. Saturación cuantificada en ensayo controlado aproximado de la iluminación anterior, no comparación píxel a píxel de las escenas. LOC-05/REN-04 permanecen PARTIAL por aceptación visual de pisadas, postura y presentación; no se declara F2/F3A aprobada. Se publica rama nueva desde main con commits y PR para Dirección; sin fusionar. Se cierran únicamente los visores de diagnóstico creados en esta tarea.

2026-10-29 — Publicación realizada: PR #12 https://github.com/MarcoPolo2-0/Isekaid/pull/12, abierto hacia main, auto-merge desactivado y sin fusionar. Implementación 11358db; informe y evidencias 79cf951. Rama fix/isek-companion-locomotion-vrm-render sincronizada. Se adjunta el PR a la entrega y se mantiene pendiente la revisión visual de Dirección.
2026-10-29 — CHAT 2 / F3A: revision narrativa por diferencias insuficientes entre personalidades. Rama docs/isek-f3a-personality-review desde main f657d78; PR #9/#10 fusionados comprobados. Se conserva el GDD v0.5, la biblioteca canonica y los archivos Unity.
- F3A_Diferenciacion_Personalidades_v0.1_PROPUESTA.md: matriz de caracter y siete emociones, tres escenas existentes con elecciones iguales, limites actuales/futuros, recomendacion N1/N2 y prueba ciega de texto/cuerpo por separado.
- F3A_Diferenciacion_Revision_60_v0.1.md: revision de 60/60 IDs, 33 correcciones propuestas (11 por personalidad) y 27 textos conservados con motivos. Se mantienen opciones, rutas y efectos.
- F3A_Diferenciacion_Editorial_v0.1_PROPUESTA.json: parche expected/set y hash de base; datos de texto/intencion compatibles con el adaptador actual. Perfiles corporales pendientes de consumidor/animaciones, sin presentarlos como implementados.
- Investigadas guias publicas de LoreBary/SillyTavern. Se extraen principios de caracterizacion, sin copiar personajes ni incorporar servicios de RP al juego. La guia externa encontrada desde Iorveths no se usa como fuente al devolver otro contenido.
- Comprobaciones estaticas: 60 IDs, 33 cambios, 27 conservaciones; adaptador existente acepta una copia temporal con 60 entradas, tres temas y 18 opciones; paridad editorial, 14 enlaces locales y originales protegidos. No se ejecutan Unity, Play Mode, actuaciones ni builds.
- Propuesta pendiente de Direccion, sin fusion automatica ni cierre de R10/F3A/ISEK-001. Iniciativa espontanea, rechazo personal y memoria ampliada permanecen como fases posteriores. Historial completo de readme conservado.

2026-10-29 — Dirección (Chat 1): nueva revisión consolidada Documentation/GameDesignDocument_v0.7_borrador.md a partir de GDD v0.5 y v0.6 y estado PR #7/#9/#10/#11/#12. Mantiene intactas versiones históricas. Actualiza dirección VRM/DEC-007, F2/F3A, locomoción y presentación observadas en prueba manual, nombre Dandere con compatibilidad Shy, método editorial inspirado en fichas de personaje de SillyTavern y referencias de estudio, distinguiendo claramente Deredere y los 33 cambios editoriales como PROPUESTAS. El documento no modifica Unity ni aprueba nuevos sistemas; pendiente revisión y aceptación de Dirección mediante PR.

2026-10-29 — CHAT 2 / Revision editorial GDD v0.7 en PR #13, rama docs/isek-gdd-v07-consolidated, entrada d94d671. Comparadas las 16 secciones de v0.5 y seis bloques adicionales de v0.6 con DEC-001–007, F2/F3A y propuestas PR #11. No se modifica Unity, biblioteca canonica ni GDD v0.5/v0.6.
- GDD-R01: leyenda de ACORDADO / IMPLEMENTADO / VALIDADO MANUALMENTE / PROPUESTO / PENDIENTE y matriz de estados separados; implementado no significa validado o aprobado.
- GDD-R02: contador activo/pausa tecnicamente probado frente a ratificacion formal heredada pendiente; no cambia balance ni comportamiento.
- GDD-R03: explicitadas fronteras de combate, salud cero, Game Over/reinicio de ensayo y checkpoint de campana; tambien enemigos, hogar y secuestro futuros.
- GDD-R04: Dandere es nombre visible acordado, aun no aplicado; conservar Shy/2, shy e IDs SHY. Cambio de etiqueta no exige migracion de datos.
- GDD-R05: depurados ejemplos de prologo que competian con contenido canonico/reforzaban caricaturas; referencia a A.RESCUE vigente y propuesta PR #11.
- GDD-R06: restaurada visibilidad de aprendizaje, gustos/habitos, expresividad normal/comica, silueta y referencia artistica; editor corporal propio sigue pausado DEC-007.
- GDD-R07: separados RemembersFirstRescue/Trust=1 existentes de FirstRescue_BanditTrap y relacion persistente propuestos, sin premio duplicado.
- GDD-R08: conciliada eleccion de clase con alternativa propuesta de preasignacion inicial; flujo sigue pendiente.
- GDD-R09: recuperados pendientes heredados de dificultad, dano amigo, atacantes, jefes, rescate, hogar y narrativa.
- GDD-R10: offline como requisito con prueba fisica pendiente; opcion de modelo local fuera del MVP conservada como propuesta, sin conectar servicios al runtime.
- GDD-R11: ampliado metodo de fichas profundas en nueve areas y contraste de seis situaciones; referencias por arquetipo como estudio, sin copiar personajes. No se inspeccionaron tarjetas privadas de SillyTavern.
- GDD-R12: comprobados 0/33 ajustes aplicados en fuente/runtime; manual de Direccion documentado sin cerrar F2/F3A ni criterios PARTIAL.
- GDD-R13: precisada trampa de bandidos y puerta de promocion del maestro; revision tecnica de Chat 3 y aprobacion final de Direccion pendientes.
- Nuevo informe Documentation/Audits/GDD-v0.7/Revision_Editorial_Chat2.md: cobertura historica, motivos por hallazgo, limites, decisiones abiertas y recomendacion de revision final.
- Comprobaciones: 19 secciones principales, 22 secciones historicas trazadas, 13 hallazgos, 39 enlaces locales validos, cinco archivos protegidos intactos y readme conservado integro. Comprobador canonico/runtime existente PASS, 60 entradas/18 rutas, sin aplicar parche narrativo.
- Solo documentacion: no se ejecutan Unity, compilacion, Play Mode, animaciones ni builds. v0.7 sigue BORRADOR; Deredere y N1/N2 no quedan autorizados. Commit/push a la misma rama del PR #13, sin fusion automatica.

2026-10-29 — CHAT 3 / Validación técnica del GDD v0.7, PR #13, rama docs/isek-gdd-v07-consolidated. Entrada editorial 448c6f9 y main f835d3b. Árbol Unity idéntico a main y a la implementación ensayada 11358db (5bbd2d86fd8c017a705c0a85044689cb7f2e1773). Auditoría exclusivamente documental: no se ejecutan Unity, compilación, Play Mode ni builds nuevos.
- T-C01: §3 distingue separación completa de capas como diseño de los datos reales de perfil/encuentro; clase, progreso y memoria de campaña siguen pendientes.
- T-C02: §14 explicita perfil de 1 MiB, metadatos conocidos de 32 KiB, copia VRM por hash, temporal/destino releídos, respaldo válido y recuperación confirmada cuando ambos archivos son ilegibles. VRM hasta 32 MiB/JSON 4 MiB y recursos PNG incorporados; no se promete partida completa o recuperación universal.
- T-C03: §§6/13 limitan continuidad a cursor con opciones pendiente en la misma instancia; cerrar terminal, volver del creador, reiniciar y abrir otro proceso son operaciones distintas. Reiniciar limpia rescate/confianza/diálogo y no carga checkpoint.
- T-C04: §7 precisa 30 s de Time.deltaTime solo en Downed, pausa global por timeScale=0, pausa por pérdida de foco, conversación sin pausa automática y creador bloqueado en Downed/Dead; no cambia balance ni añade salud/combate.
- T-C05: §§3/13 precisan iluminación corregida con materiales/texturas/shaders conservados e intención narrativa sin actuación corporal integrada; observaciones manuales siguen atribuidas a Dirección, sin certificación artística nueva.
- T-C06: §17 enumera VRM-AC01/12/22/24, F3A-D08/R10/R11 y LOC-05/REN-04 parciales; se enlaza informe técnico y corte de código sin convertirlos en aceptación.
- Verificados Shy/2, shy e IDs SHY, tres opciones actuales, 60 entradas/18 rutas y 0/33 ajustes aplicados. No se implementan Dandere visible, Deredere ni perfiles de actuación. GDD v0.5/v0.6, DECISIONS, auditoría editorial Chat 2, canónica/runtime, escenas, prefabs y todo Unity conservados.
- Nuevos Documentation/Audits/GDD-v0.7/Revision_Tecnica_Chat3.md y Revision_Tecnica_Chat3.resultados.json: matriz por afirmación, contratos, procedencia de pruebas históricas, límites, comprobaciones estáticas y dictamen APTO TÉCNICAMENTE COMO CANDIDATO. La decisión de maestro oficial corresponde a Dirección; v0.7 sigue BORRADOR y PR #13 sin fusionar. Cambios registrados para commit/push en la misma rama y comprobación de CI, sin modificar registros anteriores.

2026-10-29 — CHAT 2 / F3A-N1: profundizacion y diferenciacion de personalidades. Rama nueva docs/isek-f3a-n1-character-voices desde main f835d3b, actualizada por avance directo a 24cad11 al fusionarse PR #13 durante la preparacion. GDD v0.7 sigue BORRADOR, se consulta intacto; readme incluye integro el nuevo registro de Direccion/Chat 2/Chat 3.
- Documentation/Specifications/F3A-N1/01_Tsundere_v0.1_PROPUESTA.md: biblia original, nombre de trabajo Nera, independencia/ayuda, virtudes/defectos, proyecto propio, gustos/habitos, limites y evolucion. Biografia y rasgos nuevos sujetos a Direccion.
- 02_Yandere_v0.1_PROPUESTA.md: Liora como nombre propuesto, peso del encuentro y expectativas, intensidad con reciprocidad y limites, proyecto propio y evolucion sin romance inicial o dependencia extrema.
- 03_Dandere_v0.1_PROPUESTA.md: Sira como nombre propuesto, observacion/reflexion/voluntad y evolucion sin perder reserva. Se conservan Shy=2, shy e IDs F3A.SHY.*; no cambia UI o serializacion.
- 04_Revision_33_v0.1.md y 05_Parche_Editorial_v0.2_PROPUESTA.json: revision individual de los 33 cambios de PR #11 con actual/propuesto/final, naturalidad, personalidad, coherencia, contexto y motivo. Recomendaciones: 7 ACEPTAR, 23 MEJORAR, 3 DESCARTAR. Son 30 candidatos a cambio, ninguno aprobado o aplicado.
- Las tres E.DOWNED conservan alarma canonica; F.RESUME se corrige para raiz u hoja guardada, sin inventar una pregunta pendiente. El parche se aplica solo a IDs autorizados sobre la base canonica, nunca encima de todos los cambios PR #11.
- 06_Matriz_Reacciones_Actuacion_v0.1.md: siete contextos identicos y siete emociones, diferencias de prioridad/interpretacion y guia N2. Actuacion deseada separada de canales/animaciones disponibles, alternativas y texto inmediato sin retrasar rescate.
- 07_Prueba_Ciega_Aceptacion_v0.1.md y 07b_Muestras_Ciegas_v0.1.md: 21 tarjetas sin etiquetas/nombres/IDs fuente y clave separada para administracion; protocolo de voz escrita, naturalidad, confusion y futura ronda corporal. Sin resultados humanos ficticios.
- 08_Compatibilidad_Entrega_v0.1.md: indice, fuentes, limites del runtime y contratos, integracion condicionada, verificaciones y decisiones pendientes. Nombres, trasfondos, proyectos y fichas no crean misiones u objetos del juego.
- Validacion estatica: 33 IDs y cuatro evaluaciones por ID; 7/23/3 decisiones, 30 cambios efectivos y 30 entradas completas conservadas. Adaptador vigente acepta copia temporal: 60 entradas, tres temas, 27 nodos y 18 rutas intactas; opciones/disparadores/efectos/emociones/repeticion sin cambios.
- Paridad de revision/matriz/21 tarjetas y clave comprobada; 47 enlaces locales y nueve archivos fuente protegidos intactos. GDD v0.5/v0.6/v0.7, DECISIONS, propuestas previas, canonica/runtime y todo Unity sin modificar; historial completo de readme preservado.
- No se ejecutan Unity, compilacion, Play Mode, N2, prueba ciega humana ni builds. Deredere y sistemas futuros fuera de alcance. Se publica commit/push y PR a main para Direccion, sin fusionarlo ni dar por aprobados los 33 ajustes.

2026-10-29 — CHAT 2 / Revision creativa final de PR #14 por instruccion de Direccion, separada de la oficializacion GDD en PR #15. Sin aplicar dialogos al runtime ni fusionar PR.
- Nuevo Documentation/Specifications/F3A-N1/09_Presentacion_Final_Direccion_v0.1.md: sintesis de Nera/Tsundere, Liora/Yandere y Sira/Dandere, motivaciones, proyectos, defectos y contradicciones; nombres y trasfondos siguen provisionales.
- Comparadas reacciones ante rescate, sentimientos, aldea, espera, reanimacion y silencio bajo los mismos hechos. Intensidad de Liora centrada en peso del encuentro/expectativas, con limites; Sira distingue observacion/interpretacion/decision y puede decir no sin tartamudeo obligatorio. Dictamen editorial, sin afirmar resultado de Unity o prueba ciega.
- Presentados los 30 candidatos exactos del parche: 7 ACEPTAR y 23 MEJORAR, pendientes por ID. Se mantienen las tres alertas E.DOWNED canonicas, sin borrar nodos.
- Decisiones C1–C5: gratitud/orgullo de Nera; grado de intensidad inicial de Liora; naturalidad de reflexion temprana de Sira; solapamiento WAIT/WAIT_OPINION de Nera; agradecimiento de Liora sin deuda afectiva. No se decide por Direccion ni se modifica el parche por estas observaciones.
- 08_Compatibilidad_Entrega_v0.1.md enlaza presentacion y PR #15; distingue aprobacion documental posterior del GDD frente al corte historico original y la autorizacion de N1 aun pendiente.
- Verificada tabla de 30 IDs/textos contra JSON; validacion N1 PASS, 60 entradas/18 rutas, 57 enlaces locales y nueve fuentes protegidas. Oficializacion GDD validada por separado, 18 secciones de diseno iguales, cinco historicos/fuentes intactos y 64 enlaces/anclas. No se ejecutan Unity, compilacion, Play Mode, actuaciones, pruebas humanas o builds.
- Solo presentacion, indice y este registro cambian en esta revision. Fichas, parche, canonica/runtime y GDD intactos. Historial previo completo de readme conservado; commit/push al mismo PR #14 y sin fusion automatica. Al integrar ambos PR conservar los bloques de cada rama.

2026-10-29 — CHAT 3 / Preparación técnica N1/N2, sin implementación. Análisis del PR #14 al corte e363576 (incluye presentación final de Chat 2), main 24cad11 y árbol Unity 5bbd2d86fd8c017a705c0a85044689cb7f2e1773. No se aprueba ni aplica contenido creativo.
- Nuevo Documentation/Audits/F3A-N1-N2/Impacto_Tecnico_Chat3_v0.1.md: aplicación futura por ID/campo aprobado, 60 IDs/18 rutas/eventos/efectos preservados; 33 recomendaciones son 30 candidatos y tres E.DOWNED canónicas. C1–C5 siguen pendientes; no se añade un aplicador al runtime.
- Hallazgo previo a N1: canonicalSha256 y previousProposalSha256 del parche usan bytes CRLF; el generador/sourceSha256 runtime y los blobs Git usan LF. Contenido concordante, pero se requiere convención explícita y reconciliación de metadata antes de implementar; no se cambia el parche ni se omiten expected/hash.
- Etiqueta futura Dandere: cambio mínimo de PersonalityNames[2] tras autorización; conservar Shy=2, shy, SHY y formato de perfiles. No se cambia etiqueta ahora ni se añade Deredere.
- Inventario N2: solo Idle/Walk/Run admitidos; cabeza/manos mapeadas, ojos no declarados en Avatar Humanoid del catálogo; cara con capacidades limitadas y VRM/LookAt dependientes de archivo. Gestos sociales no integrados; límites e intensidades necesitan prueba real. Fuentes locales y documentación oficial Unity/UniVRM contrastadas sin abrir VRM privado.
- Reutilización propuesta de Controller/ReactionPresenter/Appearance/VrmAppearance, conservando asmdefs; disparo único Presented y restauración distinta de Changed. Identificados doble Show, Animator.speed global de locomoción, prioridades, cancelación y reemplazo de avatar como riesgos. Único relay IK genérico si la ruta elegida exige callback junto al Animator; no gestores por personalidad.
- Plan de regresión N1/N2: selección/contrato, etiqueta y perfiles v1/v2, F2/F3A, seguimiento, pausa, 30 s/incapacitación, creador, cambios de avatar y callbacks obsoletos; prueba humana de voz/cuerpo separada y derechos de capturas preservados.
- Simulación estática temporal: 40 selecciones válidas y siete rechazos PASS, sin escribir TARGET ni fuente real; candidato completo 30 cambios/30 íntegras, 60 entradas/18 rutas y restantes campos idénticos. Nuevo Impacto_Tecnico_Chat3.resultados.json conserva resultados y 16 blobs protegidos. No se ejecutan Unity, Play Mode, build, prueba humana o actuación N2.
- Solo análisis, resultados y este registro se publican con commit/push en PR #14; fichas, propuesta, canónica/runtime, Unity, GDD e historial previos intactos. Dirección debe aprobar creativamente y después autorizar otro PR N1 de implementación. No fusionar automáticamente.

2026-10-29 — CHAT 2 / Oficializacion documental GDD v0.7 por instruccion de Direccion, conforme con auditorias editorial y tecnica. Rama independiente docs/isek-gdd-v07-official desde main 24cad11; sin fusion automatica.
- Creado Documentation/GameDesignDocument_v0.7.md como maestro oficial con aprobacion documental. GDD v0.5, v0.6 y borrador v0.7 conservados integros; no se renombra ni sobrescribe el borrador.
- Secciones de diseno 1–18 identicas al borrador revisado. Solo portada, vigencia, trazabilidad y registro de oficializacion; las notas de elaboracion conservan su fecha/estado historico.
- DEC-008 registra autoridad, alcance y limites: no autoriza Deredere, nuevas mecanicas, parametros pendientes, integracion N1 o actuacion N2; no acepta automaticamente F2/F3A, catalogo o arte.
- README apunta al maestro v0.7 y workflow v1.2; se actualiza su referencia a ISEK-001/Woman/G2 historicos y PR #14 en revision. Workflow actualiza la referencia maestra y la puerta de revision documental, conservando contratos y limites.
- PR #14 sigue separado: Nera/Liora/Sira provisionales y 30 cambios candidatos pendientes de decision por ID. Ningun dialogo aplicado al juego.
- Validacion documental: 19 secciones, 18 secciones de diseno sin cambios, cinco archivos protegidos intactos y 64 enlaces locales/anclas validos. Historial completo de readme conservado; no se ejecutan Unity, compilacion, Play Mode o builds.
- Publicacion mediante commit/push y PR documental independiente; no se fusiona este PR ni #14 desde esta entrega.

2026-10-29 — CHAT 2 / Resolucion creativa C1–C5 en PR #14, pendiente de aprobacion personal de Direccion. Rama sincronizada con main 0ef11e5 (GDD oficial v0.7/DEC-008) mediante 9cbbb12. Conflicto de readme resuelto conservando completo el registro previo N1/Chat 3 y el bloque de oficializacion de main; documentos oficiales e historicos intactos.
- Nuevos 11_Hoja_Decisiones_C1_C5_v0.1.md y 12_Parche_Editorial_v0.3_PROPUESTA.json en Documentation/Specifications/F3A-N1/. Hoja breve con recomendaciones C1–C5, alternativas fuera del manifiesto, candidatas originales y 30 IDs/textos exactos para revision.
- C1: Nera admite que queria salir sola y no podia; gratitud directa sin reproche al jugador. C2: Liora da peso al acto de un desconocido y le inquieta su propia expectativa; intensidad y contradiccion presentes, miedo a perder un vinculo solo tras experiencias reales futuras, sin romance/violencia o abandono inventados.
- C3: Sira expresa que quiere entender el lazo antes de concluir; formulacion natural en charla inmediata sin tiempo de reflexion supuesto. C4: WAIT neutra y WAIT_OPINION conserva preferencia por avanzar. C5: Liora agradece como valoracion propia sin pedir reciprocidad.
- Siete formulaciones revisadas respecto a v0.2. Siguen 33 revisiones: 7 ACEPTAR, 23 MEJORAR, 3 DESCARTAR; 30 cambios candidatos y tres alarmas canonicas. Alternativas no se incorporan automaticamente. expected, rutas, condiciones, emocion, efectos y controles de repeticion conservados; intenciones recomendadas sin cambios respecto a v0.2.
- Ampliada ficha propuesta de Liora con contradiccion y miedo gradual; nombres Nera/Liora/Sira, biografias, proyectos, gustos y evolucion siguen propuestas. No se crea memoria de partida, relacion previa o sistema de confianza.
- Indice 08 apunta a v0.3 y GDD oficial. Revisiones/matriz/presentacion v0.2 se conservan con notas historicas. Tarjetas ciegas v0.2 y clave permanecen intactas; necesitan estimulos actualizados antes de puntuar v0.3. No se simulan resultados humanos.
- Atendido hallazgo de Chat 3 en la nueva metadata: SHA-256 UTF-8 sin BOM normalizado a LF (solo saltos CRLF/CR), anclado a commit/path/blob y valores expected. Canonic normalized e475943e...214e coincide con runtime.sourceSha256; propuesta anterior LF e6562ea5...d6a. Parche v0.2 e informe/resultados tecnicos conservados integros; no se desactivan comprobaciones.
- Validacion estatica v0.3 PASS: 33 IDs, 30 cambios, siete revisiones sobre v0.2, 60 entradas/tres temas/18 rutas, resto de campos identicos y paridad de 30 filas de la hoja. 75 enlaces locales validos, nueve fuentes de main y dos informes tecnicos intactos; ambos historiales readme preservados.
- Solo documentacion/propuestas: Unity, canonica/runtime y GDD sin modificar. No se ejecutan Unity, compilacion, Play Mode, N2 o prueba ciega humana. Commit/push al PR #14 sin fusionarlo; Direccion decide IDs/campos y fichas antes de autorizar implementacion.

2026-10-29 — CHAT 2 / Nueva direccion Yandere DRAMATICA por instruccion explicita de Direccion. DEC-009 registra aprobacion de orientacion creativa, sin aprobar automaticamente perfil detallado, nombre, biografia, escenas o dialogos. Se conserva el enfoque contenido anterior como antecedente historico.
- Nuevo Documentation/Specifications/F3A-N1/13_Yandere_Dramatica_v0.2_PROPUESTA.md: biblia candidata, contradiccion entre ser elegida libremente y controlar incertidumbre; dulzura/inquietud, celos, obsesion potencial, errores y consecuencias. Perfil ficcional sin diagnosticos; proyecto musical y gustos siguen propuestos, vida propia y amistad sin romance obligatorio.
- Cuatro etapas conceptuales A desconocidos, B vinculo, C apego/conflicto, D crisis/evolucion. No son escenarios de IDs, niveles, estados o sistemas nuevos. A no adelanta amor o pertenencia; B-D necesitan experiencias reales y diseno posterior. Se distinguen hecho, temor/interpretacion, accion y coste; el jugador conserva decisiones y no debe perdonar automaticamente.
- Estudio acotado de Yuno Gasai con ficha/introduccion oficiales enlazadas: contraste e intensidad como referencia, sin copiar trasfondo, relaciones, diseno, dialogos o acontecimientos; se separa fuente consultada de interpretacion creativa.
- Nuevo 14_C2_C5_Dramatica_v0.1.md: cuatro comparaciones canonica/v0.3/v0.4, 12 contextos de mismos hechos/elecciones con Nera y Sira, separacion de silencio actual y futuras reacciones; lista exacta de 30 textos activos para revision. C1/C3/C4 mantienen recomendaciones pendientes.
- Nuevo 15_Parche_Editorial_v0.4_PROPUESTA.json: solo cuatro spokenText de Yandere revisados frente a v0.3 (A.RESCUE, B.FEEL.OPEN, C.LATER_1, E.REVIVED). Intenciones recomendadas previas y demas campos intactos; 33 revisiones, 7 ACEPTAR/23 MEJORAR/3 DESCARTAR, 30 candidatos y tres alertas E.DOWNED canonicas conservadas. No se aplica al juego.
- C2 muestra gratitud, atencion absorbente y curiosidad desproporcionada de un primer encuentro; C5 da peso emocional a presencia/ayuda sin amor automatico, exclusividad o deuda. No se crea memoria facial, promesa, abandono ocurrido o reconocimiento tecnico. Hashes UTF-8/LF, commit/path/blob y expected se conservan como validacion de base; v0.4 sustituye recomendacion y no se aplica acumulada.
- Notas historicas anadidas a 02 y 11 sin borrar contenido; indice 08 apunta a 13/14/15 y aclara el hash CRLF historico. Parches v0.2/v0.3, fichas de Nera/Sira, matriz, tarjetas ciegas y auditoria/resultados Chat 3 preservados. La prueba v0.4 necesita actualizar estimulos y descripcion anonima Q; no se simulan resultados humanos.
- Validacion estatica v0.4 PASS: 33 IDs, cuatro revisiones YAN sobre v0.3, 30 cambios/30 entradas completas identicas en copia temporal; adaptador vigente acepta 60 entradas/tres temas/18 rutas; 30 filas y 12 contextos coinciden con JSON; 95 enlaces locales validos y diez archivos fuente/informes protegidos intactos. Historial previo de readme y DEC-001–008 conservado completo.
- No se modifica GDD maestro v0.7 o historicos, canonica/runtime, Unity, Shy=2/shy/F3A.SHY.*, contratos o efectos. No se ejecutan Unity, compilacion, Play Mode, actuacion N2, prueba humana o builds. Publicacion por commit/push al mismo PR #14, sin fusionarlo. Direccion revisara detalles e IDs antes de solicitar implementacion a Chat 3.

2026-10-29 — CHAT 2 / Microrevision final Yandere dramatica solicitada por Direccion tras revisar 01f3522. DEC-009 sigue satisfactoria; no se modifica la decision ni se autoriza ningun texto o integracion.
- Nuevos Documentation/Specifications/F3A-N1/16_Microrevision_C2_C5_v0.1.md y 17_Parche_Editorial_v0.5_PROPUESTA.json. Comparacion exacta de cuatro textos v0.4/v0.5, motivos, intenciones conservadas, limites y criterios. Parche v0.4 preservado integro; v0.5 lo referencia por commit/path/blob/SHA-256 UTF-8 normalizado LF.
- F3A.YAN.A.RESCUE cambia la fijacion en cara por el gesto de soltar el lazo. Inspeccion estatica del prefab FirstPersonPlayer al corte 01f3522: controlador y camara, sin malla/renderizador facial declarados; no garantiza rostro visible. No se afirma inspeccion visual de escenas o Play Mode.
- F3A.YAN.B.FEEL.OPEN conserva autocorreccion emocional y curiosidad intensa, pero afirma necesidad de alejarse del lazo. La distraccion es expresion narrativa, sin nueva mecanica, movimiento, bloqueo de controles o retraso en emergencia; opciones BREATH/LATER intactas.
- F3A.YAN.C.LATER_1 corrige "en cuando" por "mas bien en como" conservando autocorreccion y fijacion en el gesto. F3A.YAN.E.REVIVED mantiene exactamente texto y revision de v0.4 como candidato preferido, sin aprobacion definitiva.
- Solo tres spokenText cambian frente a v0.4; todas las intenciones previas se conservan. Los otros 26 candidatos siguen identicos y sin autorizacion definitiva. Total: 33 revisiones, 7 ACEPTAR/23 MEJORAR/3 DESCARTAR, 30 candidatos y tres alertas E.DOWNED canonicas conservadas. IDs, condiciones, rutas, emociones, efectos y repeticion intactos.
- Indice 08 actualizado a v0.5; nota historica en hoja 14 preserva todo su contenido. No se regeneran muestras ciegas ni clave: Direccion pide hacerlo cuando los textos esten cerrados. Se documenta version nueva posterior al cierre y seleccion de IDs, sin mezclar material historico o resultados.
- Validacion estatica v0.5 PASS: hashes de canonica/propuesta PR11/parche v0.4 verificados contra archivos y blobs; expected coincide. Tres revisiones sobre v0.4, E.REVIVED y otros 26 candidatos identicos; simulacion temporal conserva 60 entradas/tres temas/18 rutas y solo texto/intencion como campos candidatos. Cuatro comparaciones coinciden con JSON; 108 enlaces locales validos, nueve fuentes/informes y 14 documentos narrativos previos conservados (hoja 14 integra con nota). Todo readme previo preservado.
- GDD oficial/historicos, DEC-001–009, canonica/runtime, Unity y serializacion sin cambios. No se ejecutan Unity, compilacion, Play Mode, N2, prueba humana o builds. Commit/push al mismo PR #14 sin fusionarlo; Direccion revisara v0.5 y seleccionara IDs antes de solicitar N1.
