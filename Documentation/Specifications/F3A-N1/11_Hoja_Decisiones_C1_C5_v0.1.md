# F3A-N1 — Hoja de decisiones C1–C5

**Versión:** 0.1 · **Estado:** RECOMENDACIÓN PARA DIRECCIÓN, ninguna aprobación de textos/personajes.  
**Base sincronizada:** main `0ef11e5`, GDD oficial v0.7/DEC-008. PR #14 sigue sin fusionar.  
**Recomendación activa:** [parche v0.3](12_Parche_Editorial_v0.3_PROPUESTA.json). Sustituye la recomendación v0.2 para los mismos 33 IDs: 30 cambios candidatos y tres alarmas conservadas. No aplicar ambas propuestas sucesivamente al juego.

## C1 — Nera: orgullo vulnerable, gratitud inequívoca

**Recomiendo** reconocer que quería resolverlo sola y que necesitaba ayuda, sin enfadarse con quien la ayudó.

**A.RESCUE:** «Ya puedo sostenerme. Quería salir sola... no podía. Gracias por ayudarme.»

**Alternativa más cálida, fuera de los 30:** «Gracias. Necesitaba esa ayuda. Voy a comprobar si puedo seguir.» Pierde parte del orgullo explícito; prefiero la recomendada para mantener la contradicción.

## C2 — Liora: intensidad y miedo gradual a perder un vínculo

**Recomiendo** mostrar cuánto peso da a un acto concreto y la inquietud que le causa su propia expectativa. No es una compañera simplemente cariñosa: le cuesta dejar de volver sobre el encuentro. Quiere sinceridad, aunque una respuesta sincera pueda frustrarla. Esa contradicción debe poder incomodarla; no se corrige mágicamente por reconocerla.

- **A.RESCUE:** «No sé cómo te llamas todavía... Me has ayudado, y ya me cuesta tratar esto como un encuentro cualquiera. Gracias.»
- **B.FEEL.OPEN:** «Estoy asustada. Me alivia que estés aquí... y me inquieta lo rápido que eso empieza a importarme. Prefiero ir despacio.»
- **C.LATER_1:** «Sigo pensando en que te detuviste. Le estoy dando mucho peso... y apenas sé nada de ti. Quiero conocerte sin inventarme el resto.»

**Alternativa más contenida para A.RESCUE, fuera de los 30:** «Gracias por detenerte. Todavía no te conozco; quiero saber quién eligió ayudarme.» Prefiero las recomendadas: permiten reconocer intensidad y autocautela sin imponer afecto al jugador.

**Desarrollo propuesto:** al principio pesa el rescate, no una relación ya construida. Solo después de experiencias compartidas reales podrá temer que un vínculo significativo se pierda o signifique menos para el otro. Una distancia legítima podría dolerle y hacerle buscar una aclaración; su evolución consiste en contrastar y aceptar incertidumbre sin exigir garantías. Ese conflicto futuro no es un evento, nivel, recuerdo o penalización nuevos en F3A. El silencio actual nunca se interpreta automáticamente como abandono.

La referencia de Yuno Gasai se toma como consigna de complejidad emocional y contradicción, sin copiar escenas, historia, diálogo o diseño. No se programa un destino violento, posesivo o romántico. Ver [ampliación propuesta de la ficha de Liora](02_Yandere_v0.1_PROPUESTA.md).

## C3 — Sira: reflexión presente e iniciativa propia

**Recomiendo** una observación breve y un propósito actual; eliminar el tiempo de reflexión supuesto y la generalización sobre todo el camino.

**C.LATER_1:** «El lazo me ha asustado. Quiero entender qué pasó antes de sacar conclusiones.»

**Alternativa, fuera de los 30:** «Me ha asustado el lazo. Necesito un momento para entenderlo.» Prefiero la recomendada porque expresa qué quiere hacer con su duda. Su iniciativa está dentro del turno; no se inventa una conversación espontánea. La negativa firme de FEEL.LATER y la ayuda pedida bajo presión siguen mostrando voluntad sin tartamudeo.

## C4 — Nera al esperar: confirmar y opinar son turnos distintos

**Recomiendo** una primera confirmación neutra y reservar la preferencia para la variante opinion:

- **D.WAIT:** «De acuerdo. Espero aquí.»
- **D.WAIT_OPINION:** «Puedo esperar. Aun así, prefiero que avancemos pronto; quiero dejar atrás este tramo.»

**Alternativa neutra para WAIT, fuera de los 30:** «Entendido. Me quedo aquí.» Prefiero la recomendada por sencillez. Solo cambia la formulación: mismo evento, selección plain/opinion, orden aplicada y comportamiento.

## C5 — Liora tras reanimación: importancia sin deuda

**Recomiendo** agradecer y declarar el peso que tiene para ella, sin pedir respuesta, permanencia o reciprocidad.

**E.REVIVED:** «Gracias por ayudarme otra vez. Para mí no ha sido un gesto pequeño... Quería decírtelo.»

**Alternativa más sobria, fuera de los 30:** «Gracias por volver a ayudarme. Me alivia poder hablarte ahora.» Prefiero la recomendada por su intensidad contenida. “Otra vez” refiere rescate inicial y reanimación existentes; no registra otro primer rescate.

## Los 30 cambios que recomiendo integrar tras autorización

Las alternativas de arriba no pertenecen a esta lista. Los textos exactos están en recommended del parche v0.3. Siete formulaciones cambian respecto a v0.2; los otros 23 candidatos se conservan. ACEPTAR significa recomendación sobre PR #11, nunca aprobado.

| ID existente | Recomendación | Formulación exacta |
|---|---|---|
| F3A.TSU.A.RESCUE | MEJORAR | Ya puedo sostenerme. Quería salir sola... no podía. Gracias por ayudarme. |
| F3A.TSU.B.FEEL.OPEN | MEJORAR | Puedo apoyar la pierna. Voy a probar despacio; si necesito parar, te lo diré. Lo de la cuerda todavía no quiero contarlo. |
| F3A.TSU.B.FEEL.BREATH | ACEPTAR | Sí. Un momento, y vuelvo a intentarlo. Gracias por darme margen. |
| F3A.TSU.B.FEEL.LATER | MEJORAR | Después. Ahora quiero recuperar el pulso y saber si puedo seguir. De lo otro hablaré con más calma. |
| F3A.TSU.B.ROAD.UNCERTAIN | MEJORAR | Yo sí quiero llegar a la aldea. Podemos compartir este tramo; lo que hagas después lo decides tú. |
| F3A.TSU.C.LATER_1 | ACEPTAR | Lo del lazo… gracias otra vez. Me sale mejor fijarme por dónde piso que hablar de estas cosas. |
| F3A.TSU.D.WAIT | MEJORAR | De acuerdo. Espero aquí. |
| F3A.TSU.D.WAIT_OPINION | MEJORAR | Puedo esperar. Aun así, prefiero que avancemos pronto; quiero dejar atrás este tramo. |
| F3A.TSU.E.REVIVED | MEJORAR | Ya puedo moverme. No iba a salir sola de esa... Gracias. Quiero comprobar cómo me sostengo antes de seguir. |
| F3A.TSU.F.RESUME | MEJORAR | Podemos retomar lo de antes. Veamos dónde lo dejamos. |
| F3A.YAN.A.RESCUE | MEJORAR | No sé cómo te llamas todavía... Me has ayudado, y ya me cuesta tratar esto como un encuentro cualquiera. Gracias. |
| F3A.YAN.B.FEEL.OPEN | MEJORAR | Estoy asustada. Me alivia que estés aquí... y me inquieta lo rápido que eso empieza a importarme. Prefiero ir despacio. |
| F3A.YAN.B.FEEL.BREATH | MEJORAR | Sí. Me vendrá bien quedarme un momento sin tener que fingir que ya estoy bien. Gracias por dejarme ese espacio. |
| F3A.YAN.B.FEEL.LATER | MEJORAR | Después. Quiero escucharte de verdad, y ahora sigo pendiente de lo que acaba de pasar. |
| F3A.YAN.B.ROAD.UNCERTAIN | MEJORAR | Entonces cuento con este tramo. Después cada uno decidirá. Prefiero saber a qué atenerme. |
| F3A.YAN.C.LATER_1 | MEJORAR | Sigo pensando en que te detuviste. Le estoy dando mucho peso... y apenas sé nada de ti. Quiero conocerte sin inventarme el resto. |
| F3A.YAN.D.WAIT | ACEPTAR | Te espero aquí. Me gustaría que me avisaras antes de seguir; así retomamos el camino juntos. |
| F3A.YAN.D.WAIT_OPINION | MEJORAR | Te espero. Preferiría seguir contigo, pero también quiero poder decirte cuándo una decisión me incomoda. |
| F3A.YAN.E.REVIVED | MEJORAR | Gracias por ayudarme otra vez. Para mí no ha sido un gesto pequeño... Quería decírtelo. |
| F3A.YAN.F.RESUME | MEJORAR | Sigo pensando en lo que hablábamos. Podemos retomarlo, si todavía quieres. |
| F3A.SHY.A.RESCUE | MEJORAR | Gracias. Necesito ordenar lo que acaba de pasar. Primero quiero apartarme de la cuerda y luego hablar. |
| F3A.SHY.B.FEEL.OPEN | MEJORAR | Necesito pensarlo un momento. Puedo moverme, pero todavía me sobresalto. Prefiero responderte con calma. |
| F3A.SHY.B.FEEL.BREATH | ACEPTAR | Sí… Me vendrá bien. Voy a quedarme callada un momento. |
| F3A.SHY.B.FEEL.LATER | ACEPTAR | Después, por favor. Ahora no quiero hablar de ello. |
| F3A.SHY.B.ROAD.UNCERTAIN | MEJORAR | Yo sí quiero llegar a la aldea. Lo que haré después todavía prefiero pensarlo. |
| F3A.SHY.C.LATER_1 | MEJORAR | El lazo me ha asustado. Quiero entender qué pasó antes de sacar conclusiones. |
| F3A.SHY.D.WAIT | ACEPTAR | Sí. Me quedo aquí. |
| F3A.SHY.D.WAIT_OPINION | ACEPTAR | Me quedo. Pero preferiría continuar pronto; este sitio todavía me inquieta. |
| F3A.SHY.E.REVIVED | MEJORAR | Gracias. Me asusté, pero pude pedir ayuda. Ahora necesito un momento antes de seguir. |
| F3A.SHY.F.RESUME | MEJORAR | Podemos volver a lo de antes. Quiero retomarlo con calma. |

Se conservan las tres E.DOWNED canónicas y los 27 IDs ajenos a la revisión. En total el candidato completo modifica 30 entradas y conserva 30. No cambia rutas, opciones, condiciones, emoción, prioridad, consumo o efectos.

## Identidades originales: todas candidatas

| Candidata | Motivación y gustos propuestos | Defecto, contradicción y evolución propuesta |
|---|---|---|
| [Nera / Tsundere](01_Tsundere_v0.1_PROPUESTA.md) | Independencia y taller propio; objetos cuidados, mecanismos sencillos y humor seco | Ayuda con facilidad pero recibe con dificultad; aprende a pedir una ayuda concreta sin perder criterio |
| [Liora / Yandere](02_Yandere_v0.1_PROPUESTA.md) | Conexión con significado y canción propia; variaciones musicales, sinceridad y detalles elegidos | Da peso demasiado pronto; quiere sinceridad y le duele una expectativa frustrada. Podrá temer perder un vínculo real, aprendiendo a respetar distancia sin volverse indiferente |
| [Sira / Dandere](03_Dandere_v0.1_PROPUESTA.md) | Explorar a su ritmo y atlas personal; dibujar formas, pequeños rompecabezas y observar | Espera una respuesta perfecta y pierde su turno; aprende a hablar antes de resolverlo todo, conservando reserva |

Requieren aprobación separada nombres, trasfondos, proyectos, gustos/hábitos, defectos/detonantes concretos y trayectorias de evolución. Son orientación de autoría, no datos de partida, habilidades, objetos o misiones implementados. Regiones, familia y causa del viaje siguen abiertos. Shy=2, shy e IDs SHY permanecen intactos.

## Compatibilidad y decisión solicitada

La revisión v0.3 fija hashes **SHA-256 de texto UTF-8 sin BOM normalizado a LF**, anclados a commit/path/blob. Resuelve la discrepancia CRLF/LF señalada por [Chat 3](../../Audits/F3A-N1-N2/Impacto_Tecnico_Chat3_v0.1.md), sin desactivar expected o aceptar contenido divergente. Su informe anterior y el parche v0.2 quedan íntegros como antecedentes. El recurso runtime no se regenera.

Recomiendo a Dirección evaluar esta versión exacta, autorizar IDs/campos concretos si procede y decidir fichas por separado. No se integran nombres o biografías al aprobar un texto. La prueba ciega anterior conserva estímulos v0.2 como historial: no puntuar v0.3 con esa hoja sin actualizarla. No se ejecutan Unity, N2 o pruebas humanas, ni se fusiona #14 desde esta entrega.
