# Fase 3A — Diálogos y reacciones contextuales

**Versión:** 0.1 · **Fecha:** 28/10/2026. **Estado:** SOLO DISEÑO Y PREPARACIÓN AUTORIZADOS por instrucción de Chat 1; contenido y parámetros pendientes de revisión.  
**Rama:** docs/isek-f3a-dialogue-design. **Base comprobada:** main 4bd685718d670006569841e93f6c38282323db01.  
**Seguimiento narrativo:** [Issue F3A #8](https://github.com/MarcoPolo2-0/Isekaid/issues/8), vinculado a [ISEK-001 #1](https://github.com/MarcoPolo2-0/Isekaid/issues/1) y al borrador F3.  
**Fuentes:** [GDD v0.5](../GameDesignDocument_v0.5.md), [borrador F3](VRM_Fase3_Personalidad_Contextual_v0.1_BORRADOR.md), [contrato F2](VRM_Fase2_Contrato_Funcional_v0.1.md), [DEC-007](../DECISIONS.md), [workflow v1.2](../Workflow_Desarrollo_v1.2.md).

## 1. Objetivo y alcance

Una compañera que acaba de conocer al jugador puede agradecer, callar, opinar y cooperar sin perder su carácter. Tsundere, yandere y tímida deben diferenciarse por decisiones expresivas, vocabulario, ritmo e iniciativa, no por un filtro facial.

PR #6 y #7 confirmados fusionados mediante GitHub; la aceptación manual de F2 por el usuario sigue pendiente según la instrucción actual. Fusionado no equivale a validación humana de toda la apariencia o a fin de Fase 3. Chat 3 trabaja en infraestructura en paralelo: estos datos no bloquean su trabajo ni autorizan otras mecánicas.

Se entregan 60 entradas, 20 por personalidad; tres árboles con dos elecciones cada uno; matriz de siete emociones; contrato de eventos y pruebas. No se modifica Unity, GDD o decisiones históricas ni se elige un asset.

## 2. Documentos y fuente de verdad

| Recurso | Uso |
|---|---|
| [Biblioteca JSON](F3A_Biblioteca_v0.1.json) | Contenido canónico, condiciones, intención/emoción, presentación, repetición, opciones y efectos |
| [Árboles completos](F3A_Arboles_v0.1.md) | Lectura humana de las tres conversaciones y sus ramas por personalidad |
| [Matriz emocional](F3A_Matriz_Emocional_v0.1.md) | Neutral, alegría, tristeza, preocupación, enfado, sorpresa y alivio |
| [Contrato de eventos](F3A_Contrato_Eventos_v0.1.md) | Disparadores, hechos existentes, adaptadores propuestos y ausencia de efectos narrativos por un morph |
| [Aceptación](F3A_Aceptacion_v0.1.md) | Comprobaciones editoriales/estructurales y pruebas runtime pendientes |
| [Futuro separado](F3A_Propuestas_Futuras_v0.1.md) | Promesas, relación y persistencia narrativa ampliadas; no declaradas implementadas |

El JSON es material local trasladable a ScriptableObjects/JSON u otro formato de Desarrollo. Su esquema es de diseño, no un deserializador ejecutable o una API runtime existente. Los nombres de condiciones son datos, no expresiones de código a evaluar.

## 3. Estado realmente comprobable

En el commit base, CompanionState contiene Personality, LifeState (Trapped/Active/Downed/Dead), RemembersFirstRescue, Trust y RescueSecondsRemaining. Rescue() desde Trapped establece un primer recuerdo y Trust=1 una sola vez; desde Downed reactiva sin nuevo primer rescate. No modificar esas reglas desde la biblioteca.

CompanionController.Interact() distingue rescate y charla; SetFollowing cambia seguir/esperar estando Active. Spoke es una salida de texto ya existente, Changed un aviso genérico; no son un bus narrativo con todos los eventos aquí nombrados. Los árboles con elecciones y sus adaptadores no se presentan como UI ya implementada.

F2 conserva el mismo encuentro al abrir/cerrar creador y el perfil v2 mantiene identity/personality. El guardado completo de la aventura entre procesos no se acredita: recuerdos de rescate/Trust del estado vivo no se convierten aquí en una memoria narrativa persistente. El reporte de F2 distingue fixtures de conservación de datos de sistemas de gameplay reales.

No se ha comprobado detector general de peligro, encuentro de enemigos, misión jugable de aldea, historial de promesas o economía de relación. Los textos usan hechos del rescate y la intención narrativa de llegar a la aldea; no afirman que esos sistemas posteriores estén presentes.

## 4. Cobertura del contenido

| Situación | Entradas | Intención |
|---|---:|---|
| A. Primer rescate | 3 | Alivio, cautela y gratitud; solo observar el hecho existente |
| B. Primera conversación | 27 | Tres temas × tres personalidades × raíz y dos hojas |
| C. Conversaciones posteriores | 9 | Tres variantes por personalidad: rescate, sendero y apertura cauta |
| D. Seguir/esperar | 9 | Seguir, esperar y alternativa de espera con opinión; sin desobediencia arbitraria |
| E. Incapacitación/reanimación | 6 | Una petición urgente y una gratitud posterior por personalidad |
| F. Retorno elegible | 6 | Comentario después de recuperarse o puente a un cursor suspendido |
| Total | 60 | 20 por personalidad; 27 nodos de árboles y 33 reacciones/variantes |

Una ruta de juego no pronuncia las 60 entradas de golpe. Las hojas alternativas son caminos distintos, no respuestas sucesivas. El menú ofrece también «Charlar un momento» y «Terminar conversación»; las variantes casuales no exigen agotar los tres temas. No iniciar una casual mientras hay un árbol activo.

## 5. Dirección de las tres personalidades

- **Tsundere:** vocabulario directo y práctico, orgullo, una pausa al agradecer. Da las gracias sin insultos constantes; puede reconocer su necesidad de ayuda. Su opinión no cancela por sí sola una orden válida.
- **Yandere:** intensa en atención y significado del vínculo; quiere saber qué espera la otra persona y expresa límites propios. Interés no implica romance, obediencia o celos automáticos. No pide exclusividad ni amenaza.
- **Tímida:** prudencia y dificultad inicial para formular, pero frases completas al decidir o disentir. No usar tartamudeo en cada intervención ni tratar reserva como incompetencia.

El mismo personaje conserva postura/ritmo entre una respuesta y la siguiente. Se permiten alivio y humor breve; en Downed desaparecen adornos que resten urgencia. No comparar siluetas, ropa, color u ojos para asignar plantilla. Shy=2 corresponde a Tímida; conservar enum vigente 0/1/2.

## 6. Contrato mínimo de lectura de la biblioteca

Cada entrada declara ID, situación A–F, personalidad, hecho disparador/estado de su adaptador, condiciones de elegibilidad, texto, emoción e intención; expresión preferida/alternativa, gesto/ritmo/postura; prioridad; repetición; opciones/siguiente nodo, cierre y política de memoria.

- IDs F3A.TSU/YAN/SHY... son estables. No reutilizar un ID para otro hecho; una corrección editorial mantiene ID y cambia versión de biblioteca.
- events/condiciones son claves semánticas documentadas. El adaptador los traduce a fuentes existentes y nuevos controles de conversación, no a nombres de avatar.
- playerOptions tiene texto del jugador y nextNode inequívoco. nextNode de la entrada es null porque se cierra o espera elección; no avanzar por inferencia.
- memory.writes es siempre vacío y trustDelta siempre 0. read_existing_only puede consultar RemembersFirstRescue; react_only no escribe recuerdo. El primer efecto de rescate pertenece exclusivamente al gameplay.
- La prioridad numérica solo ordena solicitudes de diálogo. No reemplaza la prioridad de animación/supervivencia del GDD.
- Las claves de gesto describen intención; no acreditan clips disponibles. Si falta un gesto seguro, se omite.

## 7. Repetición, cursor y silencio

**Parámetros menores propuestos de esta versión:** conversación casual con espera compartida de 20 segundos activos; una presentación de cada variante por encuentro. Es un punto inicial de prueba, no umbral definitivo del GDD. No exigir esperar 20 s para elegir una opción del árbol o pedir ayuda.

maxPresentationsPerScope limita la entrada individual. group comparte selección/enfriamiento, no convierte tres variantes en una sola. Por ejemplo, casual.later puede presentar las tres variantes distintas, una cada vez, respetando ese enfriamiento. No seleccionar una frase ya consumida si existe una variante elegible sin consumir.

scope=encounter usa la instancia de encuentro, no el avatar. event_instance identifica un cambio real de seguir/esperar; downed_episode/recovery_episode identifican cada caída/reanimación. D.WAIT y D.WAIT_OPINION son alternativas mutuamente excluyentes para el mismo evento; su selector determinista no cambia gameplay ni necesita un sistema emocional nuevo.

Marcar consumo cuando se presenta el texto completo en UI, antes de que otra pulsación pueda repetirlo. No se requiere voz grabada ni efecto de escritura progresiva. Si se implementan, el cursor distingue presentación iniciada/terminada para poder continuar la misma intervención, sin seleccionarla de nuevo como otro hecho.

Cerrar/reabrir charla mantiene cursor y temas consumidos dentro del encuentro. Una raíz ya presentada no vuelve a saludar: se muestran sus opciones pendientes o el historial, sin otro Spoke. La hoja termina cuando está presentada y el jugador vuelve a temas/termina. “Terminar conversación” respeta que no se quiera hablar y no penaliza.

Al agotarse temas/variantes, UI: “No hay temas nuevos por ahora”. No es una línea de compañera ni se incluye artificialmente para alcanzar el mínimo de entradas. No crear confianza por pulsar Hablar. Creador/muestras faciales/cambio visual no producen eventos de rescate ni consumo de conversación.

Reinicio explícito del encuentro de ensayo es distinto de un cambio visual y puede crear otro encuentro inicial. La persistencia completa entre ejecuciones y checkpoints requiere el sistema futuro especificado aparte; no declarar memoria duradera si solo vive en sesión.

## 8. Urgencia, recuperación y seguridad de contexto

DownedEntered tiene prioridad 100: una petición corta por episodio, sin opciones ni gestos que impidan reanimar. Una orden aplicada tiene 80, rescate/reanimación 70, decisiones de charla 60, retorno 40 y casual 30. Estas cifras son prioridad de presentación propuesta; no retrasan efectos del juego.

Pausa o salida de pantalla no avanzan el contador de juego activo. Si cambia a Downed/Dead o deja de ser elegible una conversación, suspender/cancelar su presentación según contrato; nunca una línea de agradecimiento sobre una petición de ayuda. El hecho real ya sucedido conserva su efecto aunque no llegue a pronunciarse.

Retorno seguro no se infiere solo de Active o del fin de una animación. Mientras no exista detector de peligro, F se prueba por una petición explícita de hablar después de reanimar y una ventana elegible controlada por el escenario/adaptador de ensayo; etiquetar esa limitación. Una inyección de peligro para probar interrupción no demuestra enemigos/percepción implementados.

Al retomar, preferir cursor suspendido válido. Si no hay cursor, F.SAFE_RETURN puede comentar la recuperación una vez. Si la charla se cerró voluntariamente o su contexto ya no sirve, no forzarla ni inventar de qué hablaba. Un callback tardío de un avatar anterior no publica una frase sobre el visual nuevo.

## 9. Conservación de visión y siguiente revisión

La aprobación solicitada corresponde al contenido y contrato de F3A. No autoriza toda Fase 3 ni reabre Woman/G2. Prólogo de bandidos, autonomía, combate, convivencia, misiones personales, clases, aprendizaje, amistad y romance opcional permanecen en el GDD.

Chat 3 puede continuar su infraestructura autorizada. Si necesita cambiar un disparador, campo o condición, coordinar el contrato y versión; no reescribir contenido ni introducir mecánicas importantes sin revisión. F2 sigue pendiente de revisión manual por el usuario, sin que el PR narrativo la declare aceptada.

**Entrega:** documentación y datos locales, Issue F3A, readme y PR de revisión sin fusión automática. No se ejecutaron Unity, Play Mode, ramas de conversación en juego o builds; validación editorial/estructural separada en el documento de aceptación.
