# F3A — Contrato de eventos para Desarrollo

**Versión:** 0.1 · **Estado:** diseño de integración propuesto dentro de F3A; ningún nombre de evento aquí acredita una API ya instalada.  
**Base inspeccionada:** main 4bd685718d670006569841e93f6c38282323db01.  
[Especificación](F3A_Especificacion_v0.1.md) · [biblioteca](F3A_Biblioteca_v0.1.json) · [pruebas](F3A_Aceptacion_v0.1.md).

## 1. Fuentes existentes y conectores nuevos

| Clave semántica de biblioteca | Hecho o entrada verificable | Estado real / adaptación requerida | Salida de contenido |
|---|---|---|---|
| FirstRescueCompleted | Interact/Rescue confirmado de Trapped a Active, bandera previa falsa y posterior verdadera | Mecánica existente; capturar hecho/resultado, no publicar al pulsar antes de confirmar | Una A.RESCUE; leer recuerdo existente, no volver a aplicarlo |
| TopicSelected | Jugador elige TRAP/FEEL/ROAD en conversación elegible | Árbol/selector de F3A propuesto; no UI ya instalada por esta documentación | B.*.OPEN correspondiente a personalidad vigente |
| PlayerOptionSelected | Opción válida del nodo que está esperando respuesta | Adaptador nuevo propuesto, validar nodo/ID/encuentro; rechazar doble entrada | Hoja indicada por nextNode; sin efecto de confianza/movimiento |
| TalkRequested | Interact estando Active | Entrada existente; selector contextual/consumo propuesto sustituye alternancia cíclica | Mostrar temas pendientes; «Charlar un momento» elige una C.LATER elegible, nunca ambas a la vez |
| FollowingChanged | SetFollowing estando Active y cambio real de IsFollowing | Orden existente; emitir después de aplicar/leer resultado. No responder aceptación a una orden ignorada | D.FOLLOW o una alternativa de D.WAIT según estado real |
| DownedEntered | State.Incapacitate confirmado, Active a Downed | Estado existente; episodio de incapacidad con ID transitorio nuevo para deduplicar | E.DOWNED una vez por episodio; sin opciones |
| ReanimationCompleted | Interact/Rescue confirmado de Downed a Active | Mecánica existente; distinguirla del primer rescate por estado anterior | E.REVIVED una vez; no FirstRescueCompleted ni nuevo Trust=1 |
| SafeReturnTalkRequested | Petición explícita de hablar tras reanimación, sin conversación suspendida y con ventana elegible | Combina hechos existentes; no prueba detector general de seguridad | F.SAFE_RETURN una vez por episodio de recuperación |
| ResumeConversationRequested | Reanudar explícitamente cursor suspendido aún válido tras recuperación | Cursor/diálogo F3A propuestos; sin percepción automática nueva | F.RESUME y continuación del cursor, sin otra introducción |
| Peligro / pérdida de elegibilidad | Estado vital, pausa o señal fiable provista por escenario de prueba | Downed/Dead/pausa comprobables; detector general DangerDetected no comprobado y queda futuro | Suspender charla/limpiar gesto; ensayo inyectado se rotula como tal, no genera enemigo |

Changed es un aviso genérico y Spoke una salida de texto. No suscribirse a Spoke para volver a producir otro Spoke con la misma causa. La integración necesita elegir un solo propietario de la respuesta: no presentar simultáneamente las líneas antiguas hardcodeadas y las nuevas. Esto no ordena reescribir CompanionController; Chat 3 acuerda el punto mínimo de conexión dentro de su infraestructura autorizada.

## 2. Sobre semántico propuesto

Cada hecho consumido por el selector debe tener:
- eventId/causationId y encounterId: estables durante el encuentro, no nombres de malla.
- companionIdentity: la identidad existente del perfil v2, no hash del VRM.
- eventType y origin: gameplay confirmado, acción de conversación o fixture de ensayo.
- previousLifeState/currentLifeState cuando aplique; estado de seguir real para órdenes.
- occurredAtActiveSeconds: valor finito de tiempo activo, sin penalizar pausa o tiempo en configuración.
- isPreview: muestra del creador nunca es una fuente narrativa.
- cursor/selectedOption solo en conversación, episodeId solo en caída/recuperación.
- eligibleWindow: señal explícita de que el flujo puede hablar; Active por sí solo no demuestra ausencia de peligro.

Son datos de frontera para adaptar, no nueva memoria universal. No se evalúan cadenas de condiciones como código. Las comparaciones de trigger.conditions del JSON se interpretan sobre esta vista de contexto documentada.

## 3. Campos derivados que necesita F3A

| Condición | Cómo se obtiene | Alcance |
|---|---|---|
| lifeState / remembersFirstRescue / isFollowing | Lectura del estado/orden existente | Nunca se escribe desde la entrada narrativa |
| selectedTopic / selectedOption | Selector de conversación propuesto | Solo sesión/cursor actual; validar que pertenece al nodo |
| conversationMode / hasActiveTree | Elección «Charlar un momento» y ausencia de árbol en curso | No exige completar los tres temas ni crea memoria emocional |
| orderVariant | Selector determinista plain/opinion para un cambio a espera | Elegir una sola entrada; no cambia la orden ni aleatoriza obediencia |
| recoveryPending | Reanimación confirmada, aún sin comentario de retorno consumido | Transitorio por episodio; no se conserva como recuerdo de campaña |
| hasSuspendedConversation | Cursor válido y no cerrado voluntariamente | Si no existe, no fingir que había conversación pendiente |
| interactionWindow=provided_eligible | Ventana habilitada por UI/escenario de ensayo, respetando pausa/estado | Sin afirmar sensor de seguridad o enemigo nuevo |
| urgent | Entrada en Downed con temporizador de juego existente | No espera disponibilidad de charla social |

La personalidad se lee de la compañera vigente, no de metadatos. Las selecciones/cursor no se alojan en el presentador VRM; sobreviven a cambiar apariencia y a la visita F2 al creador. Un reinicio explícito de encuentro de prueba puede crear nueva instancia; no es cambio de avatar.

## 4. Orden, unicidad y presentación

1. Confirmar transición real. El resultado fallido/no-op no emite una aceptación ficticia.
2. Deduplicar eventId/episodeId por identidad y encuentro antes de elegir contenido.
3. Elegir una entrada compatible con personalidad, contexto y prioridad.
4. Revalidar antes de presentar: si murió/cayó, cambió la orden o terminó el contexto, cancelar lo obsoleto. No reproducir A.RESCUE pendiente después de una caída/reanimación como si acabara de liberarse de la trampa.
5. Presentar texto/gesto compatible y consumir la variante una vez. Interrumpir no vuelve a aplicar memoria.
6. Cerrar/guardar cursor del encuentro, no registrar puntos de relación.

Si el mismo frame produce Downed y una petición de hablar, gana Downed. Si cambia Wait→Follow antes de presentar espera, descartar la aceptación de espera pendiente. Si se repite un comando que no cambia IsFollowing, no generar otra aceptación continuamente; no perder la orden real por omitir el texto.

Mientras la interfaz está en pausa/configuración, no despachar charla al mundo ni avanzar enfriamientos/contador de supervivencia. Al volver, revalidar; no reproducir una cola interminable de frases. La selección de temas tiene prioridad sobre casual; F.RESUME no se presenta a la vez que F.SAFE_RETURN ni otra C.

## 5. Efectos permitidos y prohibidos

Todas las entradas declaran memory.writes vacío y trustDelta=0. read_existing_only solo consulta RemembersFirstRescue. El gameplay existente registra el rescate; el adaptador narrativo no invoca Rescue para poder decir gracias.

El registro de consumo/cursor evita repeticiones; no pretende ser una memoria de acontecimientos ampliada. No hay escritura de promesas, misiones, romance, clase o rasgos. Las opciones “Compartamos el camino” y “Puedes tomarte un momento” no llaman SetFollowing ni crean una promesa o descanso nuevo; órdenes del jugador van por su vía real.

Cambiar apariencia o mostrar happy desde el creador no emite hechos narrativos. Una expresión ausente no se trata como emoción ausente. Las capacidades de avatar son salidas y solo afectan al presentador.

## 6. Escenarios de ensayo para coordinación

- Rescate confirmado → tema TRAP → opción → cierre → repetir Talk: no saludo de rescate ni recompensa duplicados.
- Downed → petición urgente → reanimación → gratitud → hablar en ventana elegible: retorno sin nuevo primer rescate.
- Abrir tema → ir al creador F2 → cambiar avatar → volver: mismo cursor y hechos; no forzar a la compañera a olvidar.
- Suspender por Downed o señal de peligro inyectada → recuperar elegibilidad → retomar: sin repetir raíz ni inventar sensor implementado.
- Estado cambiado entre solicitud y presentación: descartar contenido obsoleto y conservar resultado de gameplay.

Las pruebas inyectadas muestran el contrato, no detectores o enemigos reales. La infraestructura paralela de Chat 3 no queda bloqueada; cambios importantes de interfaz/propiedad se registran para revisión antes de fusionar. Persistencia de campaña, promesas y evolución amplia quedan en [propuestas futuras](F3A_Propuestas_Futuras_v0.1.md).
