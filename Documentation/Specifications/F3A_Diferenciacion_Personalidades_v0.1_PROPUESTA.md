# F3A — Diferenciación observable de personalidades

**Versión:** 0.1 · **Fecha:** 29/10/2026, Guatemala.  
**Estado:** PROPUESTA PARA DIRECCIÓN. Autoriza esta entrega documental la solicitud de revisión narrativa del usuario; no aprueba su contenido ni cambios de runtime.  
**Base inspeccionada:** main `f657d78390202c2414a629d81401d8d801636bb1`, con PR #9 y #10 fusionados.  
**Seguimiento:** [F3A #8](https://github.com/MarcoPolo2-0/Isekaid/issues/8) · [ISEK-001 #1](https://github.com/MarcoPolo2-0/Isekaid/issues/1).

[Revisión de las 60 entradas](F3A_Diferenciacion_Revision_60_v0.1.md) · [Datos editoriales propuestos](F3A_Diferenciacion_Editorial_v0.1_PROPUESTA.json) · [Biblioteca canónica vigente](F3A_Biblioteca_v0.1.json) · [GDD v0.5, intacto](../GameDesignDocument_v0.5.md).

## 1. Problema y evidencia

Dirección informa que las tres compañeras se sienten prácticamente iguales en juego. Esa observación humana guía la revisión; este documento no afirma repetir su sesión en Unity.

La lectura del main actual explica parte de la sensación:

| Fuente inspeccionada | Comportamiento actual | Implicación |
|---|---|---|
| [Adaptador canónico](../../tools/build_f3a_runtime_content.py), función build | Copia texto, emoción, intención y expresión; omite ritmo, postura y gesto de la fuente | Los metadatos corporales anteriores no llegan a DialogueEntry |
| [DialogueContent](../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/DialogueContent.cs), DialogueEntry | Conserva intention como texto, sin selector de gesto por intención | Una intención distinta no ejecuta una acción distinta |
| [DialogueDirector](../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/DialogueDirector.cs), Begin/NextCasual/FollowingChanged | Menú común; casuales en orden de la biblioteca; espera alterna plain/opinion igual en las tres | Las decisiones de iniciar, esperar o ceder turno son iguales |
| [DialoguePresenter](../../UnityProjectIsekaid/Assets/_Project/Scripts/UI/DialoguePresenter.cs), Refresh | Presenta el texto completo y las opciones; no hay actuación temporal diferenciada | Los puntos suspensivos no producen una pausa corporal |
| [CompanionController](../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionController.cs), Speak | Misma duración de reacción transitoria, seis segundos; el nodo activo mantiene su expresión | Ritmos y duración expresiva no distinguen personalidades |
| [ReactionPresenter](../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionReactionPresenter.cs), Reapply | Las claves terminadas en _if_validated van a fallback; no acreditan un morph validado | Preocupación, enfado y alivio canónicos pueden acabar en neutral |

El [informe técnico de F3A, revisión 1](../Audits/VRM-F3A/review-1/INFORME.md) ya deja R10 parcial por ritmo/postura y evaluación humana. Sus pruebas no certifican diferenciación artística. Es un registro anterior a la fusión; se conserva su redacción histórica.

La revisión editorial detecta otra causa: agradecer → poner un límite → seguir aparece con demasiada frecuencia en las tres. Cambiar sinónimos conserva la misma función social. Se proponen 33 ajustes puntuales y 27 conservaciones, sin sustituir indiscriminadamente la biblioteca ni sus rutas.

## 2. Matriz de carácter: prioridades que producen diferencias

Son tendencias editoriales. No son diagnósticos, porcentajes de IA, niveles persistentes ni reglas absolutas. Una tímida puede tomar la iniciativa y una tsundere puede agradecer con claridad.

| Dimensión | Tsundere | Yandere | Tímida |
|---|---|---|---|
| Primera atención | Qué puede resolver o comprobar | Qué significa el intercambio para ambas personas | Qué observa y qué necesita para intervenir |
| Necesidad propia | Recuperar control sobre su situación | Construir una cercanía que pueda elegir | Tener espacio y conservar su voz |
| Contradicción útil | Le cuesta admitir ayuda; la reconoce mediante acciones prácticas | Desea cercanía; mantiene límites y no da por conocida a la otra persona | Evita exponerse; afirma una decisión cuando importa |
| Agradecer | Reconoce ayuda brevemente y concreta su siguiente paso | Reconoce al interlocutor y vuelve sobre el rescate compartido | Se permite una pausa y formula una gratitud sencilla |
| Preocupación | Sugiere una precaución concreta; disimula parte del miedo | Expresa cómo le afecta la situación y busca coordinación recíproca | Nombra lo observado; pide una cosa concreta |
| Desacuerdo | Afirmación directa y razón práctica | Expone su preferencia y el límite de la cercanía | Empieza prudente y termina con una decisión clara |
| Iniciar un intercambio | Trae un asunto o una propuesta útil | Retoma algo de la interacción y muestra interés por el otro | Observa primero y aporta un detalle o petición breve |
| Silencio del jugador | Cede turno; evita otra explicación | Mantiene atención disponible y luego la relaja | Descansa sin sentirse obligada a disculparse |
| Aplazamiento explícito | Cierre breve; conserva su objetivo | Reconoce el aplazamiento sin buscar garantías afectivas | Acepta el cierre sin autoacusarse |
| Peligro confirmado: Downed | Pide ayuda para actuar; admite que no puede sola | Dirige la petición al interlocutor, sin exigir sacrificio o exclusividad | La urgencia vence la reserva; alerta con claridad |
| Cautela después del rescate | Decide qué puede hacer; no promete confianza total | El rescate importa, pero no equivale a conocer al jugador | Expresa una certeza pequeña y un límite concreto |
| Alegría | Satisfacción breve y humor seco | Atención cálida al momento compartido | Apertura breve que ella misma elige mantener |
| Enfado | Se vuelve concisa y específica | Afirma un límite sin amenaza o chantaje | Mantiene una negativa clara; no se vuelve incapaz de hablar |
| Gesto base compatible | Gesto corto hacia una acción, sin señalar siempre | Mano abierta contenida hacia el intercambio | Manos próximas al cuerpo; se estabiliza al afirmar un límite |
| Mirada compatible | Directa al opinar; breve desvío al agradecer | Atención sostenida con descansos naturales | Intermitente; contacto claro en una decisión o alarma |
| Riesgo de caricatura que revisar | Hostilidad o negación automática de todo afecto | Vigilancia invasiva, romance instantáneo, dependencia extrema | Tartamudeo continuo, encogimiento perpetuo o sumisión |

La confianza observable de esta iteración es una disposición conversacional, no un nivel nuevo. Solo se consulta el primer rescate existente. No afirmar que distingue varios grados de confianza ni que conserva recuerdos entre procesos.

## 3. Tres escenas del prototipo, con elecciones idénticas

Son recorridos reproducibles sobre controles existentes y conversaciones canónicas; las líneas y acotaciones propuestas aún no se han ejecutado. Usar tres encuentros nuevos con el mismo avatar y parámetros. Cambiar únicamente personalidad. No cambiar la personalidad a mitad de una comparación, pues RebindPersonality conserva el cursor.

### S1. Rescate → trampa → permitir que no lo explique

**Preparación:** Trapped → interacción de rescate → abrir conversación → tema TRAP → «No tienes que explicarlo ahora». Se recorre A.RESCUE y B.TRAP.OPEN→SPACE de cada personalidad.

| Personalidad | A.RESCUE propuesta | Hoja SPACE vigente, conservada | Acción comunicativa y puesta en escena propuesta |
|---|---|---|---|
| Tsundere | Ya está. Primero, fuera del lazo. Puedo sostenerme… Gracias. Me has sacado de un buen lío. | Bien. Hay cosas más urgentes que escucharme protestar. Primero salgamos de junto a la cuerda. | Organiza el siguiente paso y acepta dejar de explicarse. Recupera base estable; agradece con un desvío breve de mirada. |
| Yandere | Gracias por detenerte. No sabía quién venía por el sendero; ahora sé quién me sacó del lazo. Me gustaría conocerte un poco, cuando pueda respirar. | Gracias por dejarlo ahí. Quiero decidir cuánto cuento, aunque me hayas ayudado. | Reconoce a la persona que se detuvo y decide cuánto contar. Se orienta al intercambio, reconoce el límite y cede turno. |
| Tímida | Gracias… Dame un momento. Ya estoy en el suelo. Prefiero apartarme de la cuerda antes de seguir hablando. | Sí… Gracias. Prefiero dejar esa parte por ahora. Puedo contarte algo más cuando me tranquilice. | Ordena primero su reacción y acepta el espacio ofrecido. Hace una pausa corporal corta y termina de hablar sin otra disculpa. |

Las tres respetan la misma elección. No se crea una promesa ni se mueve el NavMeshAgent desde una frase. «Fuera del lazo» expresa una preferencia; la liberación ya la realizó Rescue. El gesto no certifica un desplazamiento.

### S2. Preguntar cómo está → aplazar → silencio y cierre

**Preparación:** rescate → FEEL → «¿Prefieres que hablemos después?» → leer hoja LATER → Terminar conversación. Dejar pasar diez segundos activos sin hablar. Volver a abrir, sin reiniciar encuentro.

| Personalidad | Raíz FEEL.OPEN propuesta | Hoja LATER propuesta | Diferencia observable propuesta |
|---|---|---|---|
| Tsundere | Puedo apoyar la pierna. Voy a probar despacio; si necesito parar, lo diré. Lo de la cuerda… todavía me cuesta hablarlo. | Después. Prefiero recuperar el pulso antes de explicarme. Podemos dejarlo aquí. | Comprueba capacidad física y pospone la explicación. Cierra el asunto sin buscar permiso afectivo. |
| Yandere | Sigo sobresaltada. Me ayuda que quieras saber cómo estoy. Quiero hablar contigo, pero despacio; aún nos estamos conociendo. | Sí, después. Quiero prestar atención a lo que me cuentes; ahora solo consigo pensar en lo que acaba de pasar. Lo dejamos por hoy. | Explica el valor de escucharse y acepta que aún no es el momento. Desactiva su gesto de atención sin perseguir respuesta. |
| Tímida | Déjame pensarlo… Me tiembla la voz, aunque ya puedo moverme. Ahora mismo prefiero escuchar un poco. | Después, por favor. Ahora no quiero hablar de ello. | Se toma tiempo para observar y después formula una negativa breve. Sostiene la decisión sin disculpas añadidas. |

**Lo comprobable hoy:** la hoja completa FEEL; Close conserva los consumos, deja otros temas disponibles y no produce una nueva frase. El director no detecta “el jugador me rechaza”. Diez segundos de silencio no restan confianza ni disparan diálogo. Una raíz cerrada antes de elegir, en otro ensayo, conserva sus opciones y se reabre sin otro Spoke.

**Diferenciación corporal pendiente:** al cerrar, volver al reposo de personalidad con el canal compatible; no reabrir conversación ni añadir una frase de despedida. Si no hay animación, omitir el gesto. La aceptación del silencio sigue siendo común; se diferencia su presencia, no el respeto al jugador.

Una negativa personal, hostil o romántica no existe en las elecciones actuales. Su reacción necesita opciones o un evento explícito futuros; no se deduce de inactividad, salir del juego o abrir el creador.

### S3. Acompañarse → esperar → incapacidad y reanimación

**Preparación:** rescate → ROAD → «Compartamos el camino hasta la aldea» → cerrar → cambiar a esperar → seguir → esperar de nuevo para alcanzar WAIT_OPINION. Usar el control de incapacitación del prototipo; reanimar y pedir hablar. No introducir un enemigo o encuentro nuevo.

La opción ROAD.TOGETHER conserva su texto e ID: acuerdo práctico para tsundere, posibilidad de expresar necesidades para yandere y participación en las decisiones para tímida. La opción no ejecuta una orden. El rescate ya establece seguimiento; SetFollowing pertenece a sus controles reales.

| Personalidad | D.WAIT_OPINION propuesta | E.DOWNED propuesta | E.REVIVED propuesta |
|---|---|---|---|
| Tsundere | Espero aquí. Aun así, preferiría avanzar en cuanto podamos; quiero salir de este tramo. | ¡Ayúdame a levantarme! No puedo sola. | Ya puedo moverme. Esta vez no podía sola… Gracias. Voy a comprobar cómo me sostengo. |
| Yandere | Te espero. Prefiero seguir acompañada, aunque también necesito decidir por dónde voy. Avísame cuando quieras continuar. | ¡Necesito ayuda! Por favor, acércate. | Has vuelto a ayudarme. Gracias. Quiero decírtelo mirándote, ahora que puedo: me alegra verte aquí. |
| Tímida | Me quedo. Pero preferiría continuar pronto; este sitio todavía me inquieta. | ¡No puedo levantarme! ¡Ayuda! | Gracias… Me asusté. Necesito un momento antes de intentar seguir. |

**Resultado esperado:** tsundere busca recuperar capacidad de actuar, yandere reconoce la ayuda como un momento compartido y tímida nombra el miedo sin perder claridad. Sus opiniones no cancelan la espera. Downed interrumpe a las tres inmediatamente; ninguna pausa social retrasa la petición de ayuda ni los 30 segundos activos de rescate. La reanimación no registra otro primer rescate. Sin cursor pendiente, pedir hablar presenta F.SAFE_RETURN; con cursor suspendido válido, F.RESUME, sin inventar otra conversación.

Los gestos propuestos solo se añaden después de la transición real y con prioridad menor a supervivencia. No exigir caminar, incorporarse o proteger al jugador si no hay animación o mecánica acreditadas.

## 4. Intención emocional → expresión, cuerpo, mirada y ritmo

La emoción es común; cambia la manera de expresarla. Las expresiones preferidas siguientes son las que ya admite la fuente. Se conserva fallback reset. El actual runtime traduce reset a neutral; las tres claves condicionales siguen sin certificación visual, por lo que usa fallback. No sustituirlas por happy o angry intensos para fingir diferenciación.

| Emoción | Expresión de fuente / alternativa | Tsundere: intención y cuerpo | Yandere: intención y cuerpo | Tímida: intención y cuerpo |
|---|---|---|---|---|
| Neutral | reset / reset | Ordenar el asunto; torso estable, mirada directa, frase resuelta | Atender al intercambio; torso orientado, mirada con descansos, turno deliberado | Observar y decidir; manos próximas, mirada intermitente, cierre claro |
| Alegría | happy / reset | Reconocer que algo salió bien; gesto pequeño, mirada que vuelve, humor breve | Valorar el momento compartido; gesto abierto contenido, atención cálida, deja espacio al otro | Permitir una apertura; hombros más sueltos, contacto algo más sostenido, frase breve completa |
| Tristeza | sad / reset | Admitir un límite sin resolverlo todo; manos quietas, breve desvío, menos prisa | Dar importancia a lo que pasó; atención suave sin invadir, pausa deliberada, sin pedir consuelo obligatorio | Nombrar una pérdida o miedo concreto; baja la mirada y la recupera, silencio permitido |
| Preocupación | sad_soft_if_validated / reset | Proponer precaución concreta; gesto corto sin desplazar, mirada al interlocutor, concisa | Pedir coordinación; manos contenidas, atención presente, expresa necesidad sin exigir cercanía | Señalar lo observado; se estabiliza, mirada clara al advertir, petición específica |
| Enfado | angry_soft_if_validated / reset | Marcar desacuerdo práctico; postura firme, mirada directa, menos rodeos | Defender un límite; torso estable sin acercarse, mirada atenta con descanso, voz escrita precisa | Sostener una negativa; levanta la mirada, manos quietas, final sin vacilar |
| Sorpresa | surprised / reset | Reevaluar qué hacer; interrupción breve del gesto, mirada que vuelve, respuesta concreta | Reconocer el cambio en el intercambio; manos detenidas, pausa, sin atribuir intención al jugador | Detenerse a comprender; pausa corporal breve, mirada al interlocutor, luego claridad |
| Alivio | relaxed_if_validated / reset | Soltar tensión y comprobar capacidad; exhalación solo si hay clip, gratitud breve | Reconocer a quien ayudó; gesto contenido, mirada al interlocutor, agradecimiento recíproco | Recuperar calma; hombros descansan, mirada que vuelve, admite lo ocurrido sin otra disculpa |

No usar esta tabla como un repertorio de hechos automáticos: tristeza no presupone una pérdida nueva; enfado no presupone traición. El texto/evento válido justifica la intención. Mirar alrededor o señalar un riesgo no acredita percepción del mundo; no apuntar a un enemigo inexistente.

**Parámetros de ensayo propuestos, no medidos:** texto y opciones completos visibles de inmediato. Un gesto social opcional puede empezar a 0 ms/150 ms/350 ms tras presentar la entrada (tsundere/yandere/tímida), con duración orientativa 450–800/800–1200/600–1000 ms. Son rangos de actuación inicial, no tiempos obligatorios de lectura ni perfiles de capacidad mental. Downed usa cero retraso y omite gestos sociales. Las pausas no bloquean elegir, cerrar, seguir o rescatar.

Conservar reposo compatible al terminar el gesto; no repetirlo mientras una hoja sigue visible. Changed o un historial reabierto no vuelve a ejecutar el primer gesto. Una restauración visual puede recuperar la pose sostenida válida sin reemitir Spoke ni consumir otro nodo.

## 5. Iniciativa y respuesta contextual: frontera entre presente y futuro

| Situación | Hecho existente | Siguiente iteración propuesta | Posterior, pendiente de alcance |
|---|---|---|---|
| Agradecer y comentar al rescatar | Rescued emite A.RESCUE | Diferenciar el objetivo de la intervención y su gesto; mismo evento | Nuevos recuerdos del rescate y evolución por campaña |
| Abrir conversación | Begin abre menú común | Mantener menú accesible y aplicar presencia corporal compatible; no insertar otra charla obligatoria | Elegir iniciativa espontánea por preferencias/contexto |
| Charla casual | Botón explícito; tres variantes en orden y 20 s activos de enfriamiento | Mantener consumo/orden actual; hacer que cada intervención aporte una acción comunicativa propia | Selección por recuerdos, intereses o percepción nueva |
| Silencio con opción pendiente | Cursor espera; no hay detector de intención | Mantener espera, variar reposo compatible, ninguna línea nueva | Reconocer silencio a través de un evento definido y opcional |
| Aplazar un tema | SPACE/LATER/UNCERTAIN y Close | Respuestas diferenciadas con IDs y efectos existentes | Rechazo personal, reparación o reconciliación |
| Opinar al esperar | FollowingChanged alterna plain/opinion por número de esperas | Conservar orden y variantes; distinta preferencia y actuación | Evaluar desacuerdo táctico o cambiar una orden por peligro real |
| Peligro confirmado | Downed y reanimación | Aviso inmediato y recuperación diferenciada | Sensores, combate, protección y advertencias sobre amenazas reales |
| Cercanía o confianza | RemembersFirstRescue y Trust existentes; sin premios de biblioteca | Mostrar cautela mediante diálogo y presencia, sin escribir valores | Memoria ampliada, niveles de relación, promesas y romance opcional |

La yandere puede valorar intensamente el único rescate confirmado sin inventar un historial de intimidad. La tímida puede disentir al esperar sin una barra de valentía. La tsundere puede reconocer ayuda sin necesitar una mecánica de afecto oculto.

## 6. Datos y compatibilidad con el diálogo integrado

El JSON de propuesta es un **parche editorial para revisión**, no otro recurso runtime ni un archivo para cargar mediante DialogueContent. Contiene revisión de los 60 IDs, 33 pares expected/set y tres perfiles de presentación pendientes. No mantiene otra biblioteca completa ni duplica los árboles.

Solo propone modificar spokenText e intention en IDs existentes. El parche exige el hash de fuente y los valores anteriores exactos. Si cambia la base, detener la aplicación y revisar entradas afectadas. No sobrescribir cambios de Desarrollo con una copia anterior.

Tras aprobación, Desarrollo puede incorporar esos campos a la fuente canónica y regenerar F3A.es.json con el adaptador existente. El esquema canónico sigue siendo 1 y la salida runtime 2. El hash de origen debe renovarse por el propio adaptador. Acordar un nuevo libraryId de revisión al integrar; no cambiar el nombre de archivo de fuente sin actualizar su consumidor. La rama documental actual no modifica fuente canónica, recurso Unity ni herramientas.

La compatibilidad tiene dos niveles:

- **Editorial:** texto/intención pasan por el formato vigente; IDs, opciones, destinos, eventos, condiciones, prioridad, consumo, enfriamientos y efectos se conservan. intention continúa siendo dato, no un comando ejecutable.
- **Actuación propuesta:** presentationProposal no tiene consumidor actual. Desarrollo necesita un cambio acotado en el presentador y su adaptador de datos para traducir intención/perfil a capacidades compatibles. No esperar que el loader ignore campos nuevos y por ello aparezcan gestos.

No evaluar cadenas como código ni buscar clips por el nombre del avatar. Un perfil depende de personalidad y contexto. Una capacidad depende del rig/animaciones aprobadas; son fuentes separadas.

## 7. Recomendación para Desarrollo: una iteración acotada

**Solicitar a Dirección N1 + N2 antes de ampliar contenido narrativo.** No se encarga su implementación desde este PR.

**N1 — Ajuste de contenido:** aplicar únicamente las 33 correcciones aprobadas; adaptar la revisión canónica con la herramienta actual. Mantener las 27 líneas conservadas y todas las rutas. Repetir validación canónica y los 18 recorridos funcionales para comprobar que el cambio editorial no rompe continuidad.

**N2 — Una primera actuación por personalidad:** escoger un avatar de prueba con capacidades conocidas. Inventariar clips/rig antes de afirmar soporte. Conectar la presentación de una entrada nueva a un solo gesto breve y a un reposo/mirada compatibles de cada perfil, usando los metadatos editoriales pertinentes; conservar UI de texto inmediato. Priorizar S1 agradecimiento, S2 desacuerdo/cierre y S3 recuperación. No construir un motor de conducta nuevo. Si faltan animaciones, producir o elegir tres actuaciones mínimas para revisión de Dirección, con procedencia verificada; no simular que ya existen.

Conectar efectos expresivos a Presented y al contexto visual controlado, preservando que Changed solo refresca/restaura. Limpiar el gesto al interrumpir, cerrar o entrar al creador; conservar cursor/consumo. Al volver del creador, restaurar presencia compatible sin otro saludo. Ningún gesto escribe Trust, mueve el agente, cambia clase o reduce la ventana de rescate.

**Puerta posterior N3:** iniciativa espontánea, reconocimiento del silencio y rechazo personal. Requieren eventos explícitos, criterios de seguridad contextual, frecuencia y consentimiento de conversación. Memoria de campaña, promesas o evolución no forman parte de N1/N2.

El resultado mínimo útil de N1/N2 no es “33 líneas cargadas”: Dirección debe poder observar tres prioridades comunicativas y tres formas de presencia dentro de los mismos recorridos. Si solo se cambia texto, informar mejora editorial parcial y mantener R10 abierto.

## 8. Aceptación propuesta y prueba sin cara

La aceptación humana y las pruebas runtime están pendientes. Esta entrega comprueba la estructura de DIF-01 y la consistencia documental; no ejecuta los recorridos ni las actuaciones.

| ID | Caso | Resultado exigido |
|---|---|---|
| DIF-01 | Inventario y aplicación en copia temporal | 60 IDs y 18 opciones/destinos idénticos; 33 cambios de texto/intención, 27 textos conservados; sin efectos nuevos |
| DIF-02 | Recorridos S1/S2/S3 con las mismas elecciones y estado | Diferencias en foco, iniciativa dentro del turno y cierre; registrar ID presentado y acotación realmente ejecutada |
| DIF-03 | Clasificación ciega A, solo texto sin etiquetas, avatar o ID | Reconocer personalidades y justificar por intención/forma de ceder o tomar turno; no basta citar una muletilla |
| DIF-04 | Clasificación ciega B, vídeos de cuerpo con cara oculta y sin subtítulos/voz | Diferenciar perfiles por postura, mirada de cabeza y ritmo compatibles, sin cambios de avatar, cámara o encuadre |
| DIF-05 | Silencio 10 s, Close, visita al creador y reapertura | Ninguna acusación, pérdida de confianza o apertura automática; cursor/consumo permanecen; no repetir la actuación de saludo |
| DIF-06 | LATER/SPACE y UNCERTAIN de cada personalidad | Respeta aplazamiento/destino propio; sin amor automático, insulto obligatorio, culpa o castigo |
| DIF-07 | Downed durante un gesto social y dos reanimaciones | Aviso inmediato, gesto social cancelado, ayuda accesible y ningún segundo primer rescate ni alteración del reloj |
| DIF-08 | Avatar sin expresión, sin gesto y cambio de avatar durante cursor | Texto/opciones intactos; fallback real u omisión segura; no atribuir fallo técnico a personalidad |
| DIF-09 | Una entrada visible, varios Changed y cierre/reapertura | No bucle de gesto, raíz reemitida ni consumo extra; reposo recuperable |
| DIF-10 | Revisión de siete emociones por personalidad | Alegría distinta de alivio/neutral; tristeza, enfado y urgencia no borran autonomía o claridad; sin aprobar una cara por un nombre de preset |
| DIF-11 | Regresión F3A | Mantener 18 recorridos, seguimiento/espera, pausa, continuidad y cambio visual; volver a medir en la implementación modificada |

**Protocolo ciego propuesto:** nueve muestras por ronda: S1/S2/S3 × tres personalidades, en orden aleatorio y sin TSU/YAN/SHY, nombres de creador o colores distintos. Dar una descripción breve de las tres tendencias sin mostrar frases del ensayo. Cinco evaluadores generan 45 clasificaciones por ronda. Umbral inicial para Dirección: al menos 36/45 aciertos y 11/15 por personalidad, con matriz de confusión y razones. A y B se evalúan por separado; no compensar un cuerpo indiferenciado con aciertos de texto. Los umbrales son una propuesta de prueba, no evidencia estadística de calidad definitiva. Con un evaluador puede hacerse un piloto; no declarar superado el protocolo completo.

En la ronda A se registra si reconocen resolver, atender al intercambio u observar/afirmar, además de la etiqueta. En B se mantiene cámara/cuerpo/iluminación y emoción/contexto equivalentes; no se oculta la orientación de cabeza si se usa como mirada. Cada clip muestra entrada, reacción y vuelta al reposo. Si no existe actuación compatible, DIF-04 queda pendiente: el fallback garantiza funcionamiento, no diferenciación corporal. Una prueba sin cara y con subtítulos combina después ambos canales; tampoco sustituye estas dos comprobaciones.

La revisión humana puede rechazar una actuación demasiado caricaturesca aunque supere el conteo. No cerrar F3A-R10, Fase 3, ISEK-001 o la aceptación manual F2 con este PR.

## 9. Referencias de RP: qué se tomó y qué se excluyó

Investigación consultada el 29/10/2026. Se usan recursos de autoría como referencia metodológica; las personalidades y textos de Isekaid son una propuesta original para el prototipo.

- [LoreBary — Personality & Past](https://lorebary.com/academy/characters/character-personality): organiza valores, motivaciones, contradicciones y respuesta bajo presión, además de rasgos. Aplicación propia: convertir cada tendencia en una prioridad observable; no copiar escalas como nuevos sistemas de confianza.
- [LoreBary — Scene & Opening](https://lorebary.com/academy/characters/character-scene): utiliza aperturas y ejemplos situados para mostrar voz/acción dejando decisiones al jugador. Aplicación propia: comparar las tres sobre el mismo evento y elección; un ejemplo editorial no se convierte en un recuerdo ocurrido.
- [LoreBary — Preview and Test](https://lorebary.com/academy/characters/preview-and-test): separa presentación de tarjeta y prueba de respuesta, y propone ensayar situaciones que desafían valores. Aplicación propia: revisión de datos, prueba ciega y regresión son evaluaciones distintas.
- [SillyTavern — Character Design](https://docs.sillytavern.app/usage/core-concepts/characterdesign/): separa descripción, escenario, primera intervención y ejemplos de diálogo. Aplicación propia: conservar reglas estables y ejemplos concretos de reacción, en contenido local preparado.

Se exploró JanitorAI a través del [perfil del creador Iorveths](https://ioverse.uwu.ai/). El enlace externo de guía ofrecido por ese perfil devolvió contenido ajeno a la guía al consultarlo; no se usa para fundamentar esta especificación. No se presenta una copia de terceros o un dominio parecido como documentación oficial. No se extrajeron tarjetas privadas ni se copió la biografía o el saludo de un personaje de esos catálogos.

Estos recursos orientan la escritura; no demuestran que un arquetipo psicológico sea universal ni que el diseño ya funcione en Unity. Se mantiene contenido local offline, sin introducir servicios de RP, importación de tarjetas o dependencia generativa en ejecución.

## 10. Comprobaciones de esta entrega documental

El 29/10/2026 se comprobó el hash de la fuente base, la revisión de los 60 IDs, 33 ajustes (11 por personalidad), 27 conservaciones y los valores expected. Se aplicó la propuesta solo a una copia temporal: el adaptador existente aceptó los datos y produjo formato runtime 2 con 60 entradas, tres temas y 18 opciones. La estructura ajena a texto/intención resultó idéntica. También se comprobó paridad de textos con la revisión editorial y con las tablas de escenas, y 14 enlaces locales.

La fuente canónica, el recurso runtime y GDD v0.5 se compararon con HEAD y permanecen intactos. El historial de readme se conserva como prefijo completo. Esto es validación estática y de transformación de datos: no acredita compilación, Play Mode, diferenciación humana, animaciones ni una nueva build.

## 11. Decisiones que necesita Dirección

1. Revisar las tres prioridades de carácter y las 33 correcciones, aceptables por grupos o por ID.
2. Aprobar o corregir N1/N2 como siguiente iteración acotada y las actuaciones de referencia antes de generalizarlas.
3. Ajustar el protocolo ciego y sus umbrales. Mantener N3 y la evolución narrativa ampliada como propuestas futuras.

GDD v0.5, decisiones anteriores, biblioteca canónica y resultados técnicos originales se conservan. Este documento no los declara obsoletos ni transforma pruebas anteriores en aceptación artística.
