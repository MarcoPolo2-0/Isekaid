# F3A-N1 — Presentación final para Dirección

**Presentación histórica v0.2:** sus dudas C1–C5 se resuelven como recomendación en la [hoja posterior](11_Hoja_Decisiones_C1_C5_v0.1.md) y el [parche v0.3](12_Parche_Editorial_v0.3_PROPUESTA.json). Esta tabla anterior se conserva y no debe confundirse con la revisión activa.

**Fecha:** 29/10/2026 · **Estado:** REVISIÓN CREATIVA, no aprobación o integración.  
**Entrega:** [PR #14](https://github.com/MarcoPolo2-0/Isekaid/pull/14), anterior a cualquier autorización de Desarrollo.  
**GDD:** Dirección ha aprobado documentalmente v0.7; su publicación está en el [PR independiente #15](https://github.com/MarcoPolo2-0/Isekaid/pull/15). Esa aprobación no autoriza estas fichas, diálogos o actuación N2.

[Índice de N1](08_Compatibilidad_Entrega_v0.1.md) · [Revisión individual de los 33](04_Revision_33_v0.1.md) · [Parche revisable](05_Parche_Editorial_v0.2_PROPUESTA.json) · [Siete escenas y actuación](06_Matriz_Reacciones_Actuacion_v0.1.md) · [Protocolo ciego](07_Prueba_Ciega_Aceptacion_v0.1.md).

## 1. Tres personas candidatas, tres conflictos distintos

Los nombres son provisionales. Trasfondos, proyectos, gustos y hábitos nuevos permanecen PROPUESTOS. No se insertan como recuerdos o misiones ni se asocian a una apariencia o clase. El protagonista y la compañera comienzan como desconocidos.

### Nera — Tsundere

[Biblia completa](01_Tsundere_v0.1_PROPUESTA.md).

Quiere recuperar capacidad de actuar y conservar criterio propio. Su proyecto candidato es un taller: algo que pueda sostener con su trabajo, además de la relación con el jugador. Es determinada, franca y atenta a lo práctico; su defecto es intentar demostrar demasiado pronto que puede seguir sola.

La contradicción es que ayuda con facilidad, pero recibir ayuda le parece una pérdida de independencia. El rescate la obliga a reconocer una necesidad real. Su cuidado puede convertirse en precaución o cooperación cuando exista ese contexto; no se inventan acciones de protección ya implementadas. Debe poder agradecer sin insultar o negarlo todo, y equivocarse sin dejar de ser firme.

### Liora — Yandere

[Biblia completa](02_Yandere_v0.1_PROPUESTA.md).

Quiere entender qué significa un encuentro para cada uno. Su proyecto candidato es componer una canción propia; el protagonista no es su único interés. Sus virtudes son atención específica, constancia y franqueza emocional. Su defecto es dar significado demasiado pronto y tardar en soltar una interpretación.

Su contradicción es que valora la sinceridad y puede sentirse herida cuando una respuesta sincera frustra lo que deseaba. Aquí reside la intensidad: un gesto concreto le ocupa más espacio emocional de lo esperable, y necesita decidir qué lugar darle. No es solo amabilidad ni una sucesión de elogios. Tampoco le concede derecho a exigir cercanía, amor o respuestas.

**Dictamen creativo:** es una base viable de intensidad inicial entre desconocidos. A.RESCUE concede peso inmediato a la ayuda; C.LATER_1 vuelve sobre la decisión de quien la rescató; ROAD.UNCERTAIN delimita expectativas. La lectura debe permitir notar esa concentración sin convertirla en devoción. Dirección debe ratificar ese grado de intensidad (C2); no afirmar que ya se perciba así en Unity. La actuación, la evolución por experiencias y la prueba ciega siguen pendientes. No se promete violencia o romance como destino obligatorio del arquetipo.

### Sira — Dandere

[Biblia completa](03_Dandere_v0.1_PROPUESTA.md).

Quiere distinguir lo que observa, lo que interpreta y lo que desea antes de hablar. Su proyecto candidato es un atlas personal de rincones que le guste recordar, sin mapa/objeto funcional añadido. Sus virtudes son reflexión, paciencia y valentía concreta; su defecto es esperar una respuesta perfecta hasta perder su turno.

Su contradicción es que el silencio protege su espacio y a la vez puede permitir que otros decidan por ella. La evolución propuesta consiste en hablar antes de tenerlo todo resuelto, conservando reserva y gusto por observar. No debe volverse extrovertida como premio por confiar.

**Dictamen creativo:** no depende del tartamudeo. En FEEL.LATER dice un no breve; en C.LATER_1 revisa una interpretación del camino; en E.REVIVED reconoce haber pedido ayuda aun con miedo. Esas decisiones se sostienen al eliminar puntos suspensivos. Queda por decidir cuánto análisis verbal cabe tan pronto después del rescate (C3). Se conservan Shy=2, shy e IDs F3A.SHY.*; no se modifica la etiqueta del creador ni la serialización.

## 2. Reacciones ante las mismas circunstancias

| Situación común | Nera | Liora | Sira |
|---|---|---|---|
| Primer rescate | Admite que necesitaba ayuda y le cuesta recibirla | Atribuye peso al acto de alguien que todavía desconoce | Ordena lo ocurrido y decide cuándo quiere hablar |
| Preguntar cómo está | Comprueba capacidad física y reserva qué contar | Se abre, pero limita lo que el otro puede interpretar como confianza | Identifica que necesita pensar y elige responder con calma |
| El jugador no sabe si se quedará en la aldea | Conserva destino propio y admite una coincidencia temporal | Define qué puede esperar para no imaginar planes compartidos | Separa la decisión de llegar de lo que todavía no decidió |
| Esperar, manteniendo la orden | Prefiere avanzar y resolver el tramo | Desea compañía y conserva derecho a expresar incomodidad | Expresa inquietud de forma firme y sin disculpa añadida |
| Reanimación | Reconoce límite y quiere comprobar cómo se sostiene | Prioriza reconocer el significado de otra ayuda | Reconoce que pudo pedir ayuda aun asustada |
| Silencio/cierre del jugador | Cede turno y conserva su objetivo | Debe tolerar no saber sin exigir confirmación afectiva | Puede dejar un silencio sin sentirse obligada a disculparse |

La última fila es dirección de personaje, no tres eventos de reacción nuevos. Hoy las tres callan al cerrar: no hay detector de abandono, penalización o frase automática. En N2, solo capacidades compatibles podrán expresar presencia corporal distinta. La comparación completa sobre los mismos IDs/opciones está en la matriz; no se altera un hecho para hacer más reconocible una personalidad.

## 3. Los 30 cambios narrativos recomendados

Son el subconjunto candidato de 33 revisiones: **7 recomendaciones ACEPTAR y 23 MEJORAR**. ACEPTAR significa adoptar la propuesta de PR #11 para ese ID, si Dirección lo autoriza; no significa aprobado. Los tres E.DOWNED se recomiendan DESCARTAR como reformulación y conservar canónicos. No se borra ningún nodo.

Los textos de la tabla son exactamente los del parche actual: esta presentación no los vuelve a reescribir. Todos requieren autorización por ID; C1–C5 señalan decisiones prioritarias, no aprobación implícita de las demás filas.

| Nº | ID existente | Recomendación | Texto candidato | Atención creativa |
|---|---|---|---|---|
| 1 | F3A.TSU.A.RESCUE | MEJORAR | Ya estoy en pie. Gracias... Me fastidia haber necesitado ayuda, pero no voy a fingir que salí sola. | C1 |
| 2 | F3A.TSU.B.FEEL.OPEN | MEJORAR | Puedo apoyar la pierna. Voy a probar despacio; si necesito parar, te lo diré. Lo de la cuerda todavía no quiero contarlo. | Revisión por ID |
| 3 | F3A.TSU.B.FEEL.BREATH | ACEPTAR | Sí. Un momento, y vuelvo a intentarlo. Gracias por darme margen. | Revisión por ID |
| 4 | F3A.TSU.B.FEEL.LATER | MEJORAR | Después. Ahora quiero recuperar el pulso y saber si puedo seguir. De lo otro hablaré con más calma. | Revisión por ID |
| 5 | F3A.TSU.B.ROAD.UNCERTAIN | MEJORAR | Yo sí quiero llegar a la aldea. Podemos compartir este tramo; lo que hagas después lo decides tú. | Revisión por ID |
| 6 | F3A.TSU.C.LATER_1 | ACEPTAR | Lo del lazo… gracias otra vez. Me sale mejor fijarme por dónde piso que hablar de estas cosas. | Revisión por ID |
| 7 | F3A.TSU.D.WAIT | MEJORAR | De acuerdo. Espero aquí. Prefiero retomar el camino en cuanto podamos. | C4 |
| 8 | F3A.TSU.D.WAIT_OPINION | MEJORAR | Puedo esperar. Aun así, prefiero que avancemos pronto; quiero dejar atrás este tramo. | C4 |
| 9 | F3A.TSU.E.REVIVED | MEJORAR | Ya puedo moverme. No iba a salir sola de esa... Gracias. Quiero comprobar cómo me sostengo antes de seguir. | Revisión por ID |
| 10 | F3A.TSU.F.RESUME | MEJORAR | Podemos retomar lo de antes. Veamos dónde lo dejamos. | Revisión por ID |
| 11 | F3A.YAN.A.RESCUE | MEJORAR | La cuerda ya no tira. Gracias por ayudarme... Acabo de conocerte y ya hay algo de ti que quiero tener presente. | C2 |
| 12 | F3A.YAN.B.FEEL.OPEN | MEJORAR | Todavía estoy asustada. Puedo decírtelo, pero no significa que ya confíe en ti para todo. Quiero ir sabiendo con quién hablo. | C2 |
| 13 | F3A.YAN.B.FEEL.BREATH | MEJORAR | Sí. Me vendrá bien quedarme un momento sin tener que fingir que ya estoy bien. Gracias por dejarme ese espacio. | Revisión por ID |
| 14 | F3A.YAN.B.FEEL.LATER | MEJORAR | Después. Quiero escucharte de verdad, y ahora sigo pendiente de lo que acaba de pasar. | Revisión por ID |
| 15 | F3A.YAN.B.ROAD.UNCERTAIN | MEJORAR | Entonces cuento con este tramo. Después cada uno decidirá. Prefiero saber a qué atenerme. | Revisión por ID |
| 16 | F3A.YAN.C.LATER_1 | MEJORAR | Te detuviste sin conocerme. Eso sigue dándome vueltas. Quiero conocer a la persona que tomó esa decisión. | C2 |
| 17 | F3A.YAN.D.WAIT | ACEPTAR | Te espero aquí. Me gustaría que me avisaras antes de seguir; así retomamos el camino juntos. | Revisión por ID |
| 18 | F3A.YAN.D.WAIT_OPINION | MEJORAR | Te espero. Preferiría seguir contigo, pero también quiero poder decirte cuándo una decisión me incomoda. | Revisión por ID |
| 19 | F3A.YAN.E.REVIVED | MEJORAR | Quiero darte las gracias antes de seguir. Me has ayudado otra vez y no quiero pasar por encima de eso. | C5 |
| 20 | F3A.YAN.F.RESUME | MEJORAR | Sigo pensando en lo que hablábamos. Podemos retomarlo, si todavía quieres. | Revisión por ID |
| 21 | F3A.SHY.A.RESCUE | MEJORAR | Gracias. Necesito ordenar lo que acaba de pasar. Primero quiero apartarme de la cuerda y luego hablar. | Revisión por ID |
| 22 | F3A.SHY.B.FEEL.OPEN | MEJORAR | Necesito pensarlo un momento. Puedo moverme, pero todavía me sobresalto. Prefiero responderte con calma. | Revisión por ID |
| 23 | F3A.SHY.B.FEEL.BREATH | ACEPTAR | Sí… Me vendrá bien. Voy a quedarme callada un momento. | Revisión por ID |
| 24 | F3A.SHY.B.FEEL.LATER | ACEPTAR | Después, por favor. Ahora no quiero hablar de ello. | Revisión por ID |
| 25 | F3A.SHY.B.ROAD.UNCERTAIN | MEJORAR | Yo sí quiero llegar a la aldea. Lo que haré después todavía prefiero pensarlo. | Revisión por ID |
| 26 | F3A.SHY.C.LATER_1 | MEJORAR | He estado pensando en el lazo. Quiero entender qué pasó antes de decidir que todo este camino es peligroso. | C3 |
| 27 | F3A.SHY.D.WAIT | ACEPTAR | Sí. Me quedo aquí. | Revisión por ID |
| 28 | F3A.SHY.D.WAIT_OPINION | ACEPTAR | Me quedo. Pero preferiría continuar pronto; este sitio todavía me inquieta. | Revisión por ID |
| 29 | F3A.SHY.E.REVIVED | MEJORAR | Gracias. Me asusté, pero pude pedir ayuda. Ahora necesito un momento antes de seguir. | Revisión por ID |
| 30 | F3A.SHY.F.RESUME | MEJORAR | Podemos volver a lo de antes. Quiero retomarlo con calma. | Revisión por ID |

No se modifican opciones del jugador, rutas, eventos, condiciones, emociones, efectos, prioridades o consumo. La etiqueta intention sigue siendo un dato editorial; no ejecuta conducta. F.RESUME se mantiene válido para raíz con opciones u hoja realmente conservada por interrupción, sin inventar otra pregunta.

## 4. Decisiones creativas que necesita Dirección

| Clave | IDs de atención prioritaria | Decisión y recomendación |
|---|---|---|
| C1 | TSU.A.RESCUE | ¿La admisión “me fastidia haber necesitado ayuda” comunica orgullo vulnerable o suena ingrata? Recomiendo admitir esa incomodidad, pero validar que el agradecimiento se perciba sincero; si falla, pedir ajuste de esa línea antes de autorizarla. |
| C2 | YAN.A.RESCUE, YAN.B.FEEL.OPEN, YAN.C.LATER_1 | Ratificar una intensidad centrada en el peso del encuentro y sus expectativas. Recomiendo esta base cauta para desconocidos; si se busca mayor intensidad desde el primer encuentro, encargar una revisión localizada, sin introducir amor, posesividad o violencia como atajo. |
| C3 | SHY.C.LATER_1 | ¿La reflexión sobre el camino es natural incluso si el jugador pulsa Charlar pronto? Recomiendo revisar su oralidad en ese caso. Una corrección sería editorial por ID; no añadir una condición temporal o memoria nueva para justificarla. |
| C4 | TSU.D.WAIT y TSU.D.WAIT_OPINION | Ambas variantes expresan preferencia por avanzar. Dirección debe decidir si quiere una primera confirmación más neutra y reservar la opinión a WAIT_OPINION. Recomiendo reservar ahí la opinión si la alternancia se siente repetitiva; el parche actual sigue intacto hasta esa decisión. |
| C5 | YAN.E.REVIVED | “No quiero pasar por encima de eso” debe expresar su necesidad de reconocer la ayuda, sin hacer responsable al jugador de su emoción. Recomiendo mantener el foco en lo que ella elige decir y revisar cualquier lectura de deuda afectiva. |

También requieren decisión los nombres Nera/Liora/Sira, los proyectos personales y los trasfondos candidatos. Pueden aprobarse por separado de los diálogos: ningún texto de los 30 revela esos nombres o proyectos. El caso C4 puede terminar en mantener el D.WAIT canónico; en ese supuesto habría 29 cambios autorizables de este conjunto, sin alterar los otros IDs. Esta presentación no toma esa decisión en lugar de Dirección.

## 5. Alcance del dictamen final

Recomiendo revisar las tres fichas como bases originales y decidir los candidatos por ID, con especial atención a C1–C5. Liora tiene una contradicción emocional propia; Sira tiene una forma de razonar y afirmar voluntad. Esa es una valoración editorial, no el resultado de un ensayo humano de identificación.

La prueba ciega, naturalidad oral y actuación N2 siguen pendientes. No cerrar F3A-R10 por el conteo de frases o por estas fichas. La oficialización documental de v0.7 no sustituye esas decisiones. El PR #14 permanece abierto y sin integración al runtime; no se fusiona automáticamente.
