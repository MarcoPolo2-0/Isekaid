# F3A-N1 — Comparación de reacciones y dirección de actuación

**Versión:** 0.1 · **Estado:** PROPUESTA N1/N2; textos recomendados y actuación deseada, sin ejecución Unity.  
[Parche N1](05_Parche_Editorial_v0.2_PROPUESTA.json) · [Prueba ciega](07_Prueba_Ciega_Aceptacion_v0.1.md) · [Compatibilidad](08_Compatibilidad_Entrega_v0.1.md).

## 1. Qué compara esta matriz

Misma compañera adulta en apariencia/clase/estado, distinto arquetipo de autoría. Los turnos marcados por IDs son una lectura del candidato: 30 cambios recomendados más los textos vigentes que se conservan. No es una grabación de Unity ni una nueva biblioteca consumida por el juego. Los IDs y el orden de cada recorrido siguen siendo los existentes.

La prueba de primer encuentro no concede conocimiento del nombre del otro. No se presupone una respuesta del jugador que no figure en las opciones. TRAP conserva sus seis intervenciones de raíz/SPACE por personalidad: es un bloque canónico sin cambios y permite detectar qué no resuelve todavía N1.

## 2. Primer rescate de la trampa de bandidos

**Estado/recorrido:** Trapped → Rescue confirmado; sin temporizador de Downed.  
**Acción común del jugador:** El jugador libera a la compañera; no se presupone otra conversación.

| Personalidad | Turnos comparables | Prioridad o interpretación |
|---|---|---|
| Tsundere | `F3A.TSU.A.RESCUE`: «Ya estoy en pie. Gracias... Me fastidia haber necesitado ayuda, pero no voy a fingir que salí sola.» | Aceptar necesidad sin renunciar a autonomía. |
| Yandere | `F3A.YAN.A.RESCUE`: «La cuerda ya no tira. Gracias por ayudarme... Acabo de conocerte y ya hay algo de ti que quiero tener presente.» | Decidir qué peso dar a un acto de un desconocido. |
| Dandere | `F3A.SHY.A.RESCUE`: «Gracias. Necesito ordenar lo que acaba de pasar. Primero quiero apartarme de la cuerda y luego hablar.» | Ordenar lo ocurrido y elegir cuándo hablar. |

## 3. Primera conversación con un desconocido

**Estado/recorrido:** Active con RemembersFirstRescue; elegir TRAP y después SPACE.  
**Acción común del jugador:** «¿Qué ocurrió con la trampa?» → «No tienes que explicarlo ahora».

| Personalidad | Turnos comparables | Prioridad o interpretación |
|---|---|---|
| Tsundere | `F3A.TSU.B.TRAP.OPEN`: «Pisé una cuerda y el lazo me levantó antes de que pudiera reaccionar. Menuda manera de recibir a quien pasa por aquí.»<br><br>`F3A.TSU.B.TRAP.SPACE`: «Bien. Hay cosas más urgentes que escucharme protestar. Primero salgamos de junto a la cuerda.» | Relata un problema y prioriza dejar la situación. |
| Yandere | `F3A.YAN.B.TRAP.OPEN`: «El lazo se cerró al pisar. Después solo podía oír el sendero sin ver bien quién pasaba. No quiero volver a sentirme así.»<br><br>`F3A.YAN.B.TRAP.SPACE`: «Gracias por dejarlo ahí. Quiero decidir cuánto cuento, aunque me hayas ayudado.» | Explica una experiencia de incertidumbre y conserva control de lo que cuenta. |
| Dandere | `F3A.SHY.B.TRAP.OPEN`: «Noté algo en el pie y, de pronto, estaba suspendida. Intenté soltar el nudo, pero no alcanzaba bien.»<br><br>`F3A.SHY.B.TRAP.SPACE`: «Sí… Gracias. Prefiero dejar esa parte por ahora. Puedo contarte algo más cuando me tranquilice.» | Describe lo observado y acepta espacio para tranquilizarse. |

## 4. Preguntar cómo se siente y ofrecer un momento

**Estado/recorrido:** Elegir FEEL y después BREATH, en un encuentro nuevo por personalidad.  
**Acción común del jugador:** «¿Cómo te encuentras?» → «Puedes tomarte un momento».

| Personalidad | Turnos comparables | Prioridad o interpretación |
|---|---|---|
| Tsundere | `F3A.TSU.B.FEEL.OPEN`: «Puedo apoyar la pierna. Voy a probar despacio; si necesito parar, te lo diré. Lo de la cuerda todavía no quiero contarlo.»<br><br>`F3A.TSU.B.FEEL.BREATH`: «Sí. Un momento, y vuelvo a intentarlo. Gracias por darme margen.» | Comprueba capacidad y elige cuánto exponer. |
| Yandere | `F3A.YAN.B.FEEL.OPEN`: «Todavía estoy asustada. Puedo decírtelo, pero no significa que ya confíe en ti para todo. Quiero ir sabiendo con quién hablo.»<br><br>`F3A.YAN.B.FEEL.BREATH`: «Sí. Me vendrá bien quedarme un momento sin tener que fingir que ya estoy bien. Gracias por dejarme ese espacio.» | Admite miedo y pone límites a una confianza que aún no existe. |
| Dandere | `F3A.SHY.B.FEEL.OPEN`: «Necesito pensarlo un momento. Puedo moverme, pero todavía me sobresalto. Prefiero responderte con calma.»<br><br>`F3A.SHY.B.FEEL.BREATH`: «Sí… Me vendrá bien. Voy a quedarme callada un momento.» | Reconoce una incertidumbre y elige tiempo para responder. |

## 5. Decisión de viajar hacia la aldea

**Estado/recorrido:** Elegir ROAD y después UNCERTAIN.  
**Acción común del jugador:** «¿Qué piensas hacer ahora?» → «Aún no sé si me quedaré allí».

| Personalidad | Turnos comparables | Prioridad o interpretación |
|---|---|---|
| Tsundere | `F3A.TSU.B.ROAD.OPEN`: «Iba hacia la aldea. Y pienso seguir; esta trampa no va a decidir mi camino.»<br><br>`F3A.TSU.B.ROAD.UNCERTAIN`: «Yo sí quiero llegar a la aldea. Podemos compartir este tramo; lo que hagas después lo decides tú.» | Su destino propio organiza el acuerdo temporal. |
| Yandere | `F3A.YAN.B.ROAD.OPEN`: «Quiero llegar a la aldea. Me gustaría saber si tú vas en la misma dirección, antes de seguir a alguien que acabo de conocer.»<br><br>`F3A.YAN.B.ROAD.UNCERTAIN`: «Entonces cuento con este tramo. Después cada uno decidirá. Prefiero saber a qué atenerme.» | Delimita expectativas para no inventar un futuro compartido. |
| Dandere | `F3A.SHY.B.ROAD.OPEN`: «Iba a la aldea. Me da un poco de apuro pedirlo, pero preferiría no recorrer este tramo sin compañía.»<br><br>`F3A.SHY.B.ROAD.UNCERTAIN`: «Yo sí quiero llegar a la aldea. Lo que haré después todavía prefiero pensarlo.» | Distingue una decisión tomada de otra que necesita pensar. |

## 6. Seguir y esperar

**Estado/recorrido:** Tras rescate: Wait → Follow → Wait; la segunda espera produce opinion. Comparar extractos FOLLOW y WAIT_OPINION.  
**Acción común del jugador:** Los mismos cambios reales de orden, sin llamadas no-op ni supuesta amenaza.

| Personalidad | Turnos comparables | Prioridad o interpretación |
|---|---|---|
| Tsundere | `F3A.TSU.D.FOLLOW`: «Voy. Pero si ves otra cuerda, me avisas antes de acercarte.»<br><br>`F3A.TSU.D.WAIT_OPINION`: «Puedo esperar. Aun así, prefiero que avancemos pronto; quiero dejar atrás este tramo.» | Acepta y expresa preferencia por avanzar. |
| Yandere | `F3A.YAN.D.FOLLOW`: «Voy contigo. Si cambias de dirección, dímelo; prefiero decidir sabiendo adónde vamos.»<br><br>`F3A.YAN.D.WAIT_OPINION`: «Te espero. Preferiría seguir contigo, pero también quiero poder decirte cuándo una decisión me incomoda.» | Acepta compañía y conserva derecho a decir que una decisión incomoda. |
| Dandere | `F3A.SHY.D.FOLLOW`: «Sí, voy contigo. Si necesito que bajemos el paso, te lo diré.»<br><br>`F3A.SHY.D.WAIT_OPINION`: «Me quedo. Pero preferiría continuar pronto; este sitio todavía me inquieta.» | Acepta y expresa inquietud con claridad, sin pedir perdón. |

FOLLOW conserva su texto canónico. La primera espera usa WAIT, la segunda WAIT_OPINION; esos contadores y la navegación no dependen de esta opinión. No atribuir a la voz una evaluación de peligro que todavía no existe.

## 7. Incapacitación y reanimación

**Estado/recorrido:** Control de incapacitación de ensayo → Downed → interacción de reanimación; mismo episodio.  
**Acción común del jugador:** El jugador reanima. No se añade enemigo, combate o rescate anterior al inicial.

| Personalidad | Turnos comparables | Prioridad o interpretación |
|---|---|---|
| Tsundere | `F3A.TSU.E.DOWNED`: «¡Ayúdame! No consigo levantarme.»<br><br>`F3A.TSU.E.REVIVED`: «Ya puedo moverme. No iba a salir sola de esa... Gracias. Quiero comprobar cómo me sostengo antes de seguir.» | Necesidad inmediata; después recupera acción y reconoce límite. |
| Yandere | `F3A.YAN.E.DOWNED`: «¡Necesito ayuda! No puedo incorporarme.»<br><br>`F3A.YAN.E.REVIVED`: «Quiero darte las gracias antes de seguir. Me has ayudado otra vez y no quiero pasar por encima de eso.» | Necesidad inmediata; después reconoce el peso de otra ayuda. |
| Dandere | `F3A.SHY.E.DOWNED`: «¡Ayuda, por favor! No puedo levantarme.»<br><br>`F3A.SHY.E.REVIVED`: «Gracias. Me asusté, pero pude pedir ayuda. Ahora necesito un momento antes de seguir.» | Necesidad inmediata; después reconoce que fue capaz de pedirla. |

Las tres E.DOWNED conservan la alarma vigente. La lectura del personaje se busca en E.REVIVED. Volver a pedir ayuda no duplica RemembersFirstRescue o Trust. La derrota y el plazo no cambian para facilitar una actuación.

## 8. Silencio, desacuerdo y límites personales

**Estado/recorrido:** FEEL → LATER → cerrar; diez segundos activos sin nueva interacción.  
**Acción común del jugador:** «¿Prefieres que hablemos después?»; el jugador cierra. Ninguna frase automática al callar.

| Personalidad | Turnos comparables | Prioridad o interpretación |
|---|---|---|
| Tsundere | `F3A.TSU.B.FEEL.LATER`: «Después. Ahora quiero recuperar el pulso y saber si puedo seguir. De lo otro hablaré con más calma.» | Pospone la exposición para recuperar capacidad de actuar. |
| Yandere | `F3A.YAN.B.FEEL.LATER`: «Después. Quiero escucharte de verdad, y ahora sigo pendiente de lo que acaba de pasar.» | Pospone para poder escuchar con atención real. |
| Dandere | `F3A.SHY.B.FEEL.LATER`: «Después, por favor. Ahora no quiero hablar de ello.» | Formula una negativa breve y firme. |

Después del cierre las tres callan: eso es respeto común al jugador, no una carencia que debamos rellenar con tres despedidas. No se reconoce una emoción de rechazo ni se aplica consecuencia. La diferencia reside en qué límite acaban de expresar y, en N2 si hay capacidades, cómo vuelven al reposo. Un rechazo personal o romántico necesita opciones/eventos futuros y no forma parte del parche.

## 9. Mirada, postura, pausa y gesto: propuestas N2

| Canal | Tsundere | Yandere | Dandere |
|---|---|---|---|
| Orientación | Base estable y torso disponible para la siguiente acción | Torso claramente orientado al intercambio, sin invadir distancia | Orientación moderada; se estabiliza al expresar una decisión |
| Mirada | Directa al concretar o disentir; breve desvío al admitir necesidad | Atención sostenida con descansos naturales; busca al interlocutor al reconocer significado | Contacto intermitente; mira de frente al poner un límite o pedir ayuda |
| Pausa significativa | Entre el plan y admitir ayuda, no delante de cada frase | Antes de decir por qué algo le importa; deja espacio después | Antes de formular una conclusión; la conclusión llega clara |
| Gesto discreto | Gesto corto hacia una acción/objeto que sí existe; manos quietas al reconocer ayuda | Mano abierta contenida o pequeña detención del gesto al cambiar de significado | Manos próximas; deja de moverlas para afirmar algo importante |
| Evitar | Brazos cruzados, bufidos o desvío de mirada en bucle | Mirada fija, acercamiento invasivo, agarrar o vigilar al jugador | Encogimiento constante, balanceo infantil o tartamudeo como animación |

La orientación deseada afecta a una actuación visual compatible, no autoriza girar el agente contra su navegación o añadir un destino. El texto y las opciones deben estar disponibles de inmediato. Una pausa de actuación puede ensayarse sin bloquear lectura/entrada; no se fija un retraso obligatorio por personalidad. Downed omite pausas y gestos sociales. No hay voces grabadas ni control de prosodia acreditados por este documento.

## 10. Emoción e intención a través de tres cuerpos

La emoción se conserva independiente de personalidad. Las siguientes acotaciones son deseos de actuación y no un sistema emocional persistente.

| Emoción | Tsundere | Yandere | Dandere |
|---|---|---|---|
| Neutralidad | Quiere situar el siguiente paso; postura estable, mirada directa, ritmo resuelto | Quiere entender el intercambio; orientación presente y atención con descansos | Quiere observar sin quedar excluida; manos próximas, contacto intermitente, conclusión clara |
| Alegría | Reconoce un resultado bueno; gesto pequeño y humor seco, mirada que vuelve | Da espacio al momento compartido; gesto abierto contenido y atención cálida, sin exigir reciprocidad | Comparte un detalle por elección; hombros más sueltos y un contacto algo más sostenido |
| Tristeza | Le cuesta aceptar un límite que no puede reparar; manos quietas, menos prisa al reconocerlo | Algo compartido o esperado le pesa; mantiene atención suave sin pedir consuelo obligatorio | Necesita entender lo que siente; mirada baja que recupera para decir algo concreto |
| Preocupación | Quiere reducir un problema; gesto breve y recomendación concreta si el hecho existe | Quiere claridad y coordinación; atención concentrada y necesidad expresada, sin apropiarse del otro | Quiere separar observación e interpretación; se estabiliza y formula una petición concreta |
| Enojo / enfado | Defiende criterio o responsabilidad; postura firme, mirada directa y frases específicas | Defiende un límite o pide aclarar una expectativa, sin amenaza; gesto contenido y atención precisa | Sostiene una negativa; levanta mirada y completa la frase sin disculpa añadida |
| Sorpresa | Se detiene para reevaluar una acción y recuperar plan | Reconoce un cambio de significado y permite no saber qué implica aún | Se toma una pausa para comprender y después expresa qué sabe |
| Alivio | Recupera acción y puede admitir necesidad; se suelta la tensión sin celebrar como victoria | Reconoce a quien ayudó y el peso del momento; gesto contenido y vuelta a contacto normal | Reconoce que sobrevivió al miedo y pudo hablar; hombros descansan y vuelve la mirada |

No inventar pérdida para representar tristeza ni traición para representar enojo. En escenas actuales solo se usa la emoción de la entrada válida. Una ficha de temor o hábito no añade un detector ni justifica que el personaje sepa algo del jugador.

## 11. Capacidades disponibles y alternativas

| Canal de fuente vigente | Estado de integración observado | Actuación deseada / alternativa |
|---|---|---|
| reset | Se traduce a neutral/limpieza | Mantener lectura de texto, sin sonrisa permanente |
| happy, sad, surprised | Se solicitan cuando el avatar los admite; soporte no universal | Si falta canal, fallback reset; texto/elecciones y significado permanecen |
| sad_soft_if_validated, angry_soft_if_validated, relaxed_if_validated | Hoy usan fallback neutral por no estar certificados | No sustituir por un morph intenso o por un nombre supuesto; N2 requiere prueba y aprobación visual |
| Gesto, postura, mirada y ritmo editoriales | No trasladados a DialogueEntry ni ejecutados por intention | N2 requiere adaptador/capacidad aprobados; si no existen, omitir el efecto |
| Idle/Walk/Run | Locomoción Humanoid acotada integrada, independiente del carácter | No equivale a inclinar cabeza, cruzar brazos, respirar o señalar |

N1 no cambia emotion ni preferredExpression/fallbackExpression de ninguna entrada. Ante una capacidad ausente, texto completo y opciones siguen funcionando; no reducir autonomía ni contenido. No tomar índices de otro VRM. Si se dispone de gesto compatible sin cara, puede sostener intención; si tampoco existe, usar solo texto y declarar pendiente la diferenciación corporal.

Un gesto nuevo se vincularía a una presentación realmente nueva, no a cada Changed. Al suspender/entrar al creador debe limpiarse de forma segura y restaurar solo una presencia válida al volver, sin repetir Spoke o consumir contenido. Close puede conservar una raíz con opciones; no prometer que conserve una hoja terminal. Validar prioridad, clips, mirada y restauración en N2, sin presentarlos como implementados por esta ficha.
