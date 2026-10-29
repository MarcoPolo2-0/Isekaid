# F3A — Matriz emoción × personalidad × reacción

**Versión:** 0.1 · **Estado:** diseño en revisión. [Especificación](F3A_Especificacion_v0.1.md), [biblioteca canónica](F3A_Biblioteca_v0.1.json).

## 1. Emoción y carácter son dimensiones distintas

Una compañera puede estar alegre, preocupada o enfadada sin convertirse en otro arquetipo. Esta matriz describe presentación/intención; no demuestra un sistema emocional persistente ya implementado. La decisión de agradecer o disentir se toma por contexto y hechos; activar happy nunca genera gratitud o confianza.

Mismo avatar, clase y situación para comparar:

| Emoción | Tsundere | Yandere | Tímida |
|---|---|---|---|
| Neutral | Frase práctica, postura firme y mirada breve; deja clara su decisión | Escucha orientada al interlocutor; pregunta concreta, sin invadir su espacio | Reserva con voz clara y pausas breves; puede iniciar una pregunta |
| Alegría | Sonrisa breve y reconocimiento directo tras una pequeña vacilación; humor seco, no burla | Reconoce el detalle concreto que le alegra y presta atención a la respuesta; no declaración romántica | Sonrisa y contacto visual algo mayor; se anima a participar sin volverse súbitamente extrovertida |
| Tristeza | Menos palabras, tono contenido, mirada baja antes de volver a decidir; no sustituirla siempre por insultos | Expresa por qué le afecta el hecho y qué necesita; no amenaza o culpa automáticamente al jugador | Pausa para ordenar la frase y pide espacio si lo necesita; conserva capacidad de decisión |
| Preocupación | Señala el problema y propone cautela con pocas palabras | Pregunta por planes/seguridad y expresa un límite propio | Identifica la inquietud y puede alertar con claridad, aunque socialmente sea reservada |
| Enfado | Objeción firme y razonada; no maltrato permanente | Explica qué le incomoda y busca entender, sin posesión, violencia ni sabotaje | Desacuerdo breve pero inequívoco; no convertir enfado en silencio obediente |
| Sorpresa | Pausa corta y observación directa, algo de humor cuando es seguro | Atención concentrada en lo ocurrido y pregunta concreta; no dramatizar toda novedad | Interjección breve y después frase completa; sin tartamudeo sistemático |
| Alivio | Baja tensión y admite ayuda sin perder orgullo | Agradece y reconoce presencia/ayuda, manteniendo cautela del primer encuentro | Recupera respiración/voz y expresa gratitud con menos dificultad |

No todos los cruces exigen una línea nueva en esta primera biblioteca. Tristeza/enfado se usan solo cuando el contexto de la línea lo justifica; no forzar un tour de siete emociones durante el primer rescate.

## 2. Presentación compatible con VRM

| Emoción | Salida preferida, solo si se validó | Alternativa sin expresión | Cuidado |
|---|---|---|---|
| Neutral | reset: limpiar pesos transitorios y volver al reposo válido | Postura atenta o sin gesto | No exigir preset neutral ni fijar sonrisa |
| Alegría | happy | Gesto breve de reconocimiento y texto | Diferenciar por boca/mejillas/mirada si existen, no solo etiqueta |
| Tristeza | sad | Pausa y mirada baja segura; texto | No usar tristeza como única cara para toda preocupación |
| Preocupación | sad suave si su prueba comunica inquietud | Atención/tensión corporal; petición clara | sad_soft_if_validated es clave semántica de adaptación, no nombre de preset |
| Enfado | angry suave validado | Mirada firme o gesto de objeción | angry_soft_if_validated no es un canal VRM inventado; no imponer cara agresiva |
| Sorpresa | surprised | Pausa breve/cambio de atención y texto | No equivale a sorpresa cómica ni detector de enemigos |
| Alivio | relaxed validado | Soltar tensión y agradecer; sin gesto si no es seguro | relaxed_if_validated no garantiza respiración animada o clip disponible |

Las claves de emotionPresentation del JSON describen intenciones; el adaptador debe traducirlas a capacidades reales o fallback. reset nunca se envía como nombre de morph: es una acción del presentador. Si falta también el gesto, conservar texto/intención y omitir animación. No crear otros ojos o retocar texturas de un avatar arbitrario.

Las intensidades concretas no están aprobadas. Las expresiones binarias y exclusiones de mirada/parpadeo/boca deben respetarse. La petición de ayuda Downed es corta y prioritaria, aunque el arquetipo normalmente hable con pausas.

## 3. Comprobación narrativa

Comparar tres grabaciones/recorridos con los mismos hechos y avatar: ¿se distingue la plantilla por ritmo, opinión y decisiones expresivas al ocultar la cara? Comparar después dos avatares con capacidades distintas: ¿la intención y la continuidad siguen siendo iguales?

La biblioteca cubre contenido, no assets de gesto, voz grabada ni sincronía labial probada. No atribuir a falta de morph un cambio de recuerdo, confianza, clase o personalidad.
