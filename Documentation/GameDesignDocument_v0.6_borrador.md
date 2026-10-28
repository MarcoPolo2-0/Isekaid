# Isekaid — Documento maestro de diseño

**Versión:** 0.6 — borrador de revisión.  
**Fecha:** 27/10/2026, Guatemala.  
**Última actualización:** 28/10/2026 — trazabilidad de DEC-001 e ISEK-001.  
**Estado:** PROPUESTA — pendiente de revisión del Chat 1, Dirección del proyecto (Cerebro); acuerdos previos conservados.  
**Workflow:** [Desarrollo v1.0](Workflow_Desarrollo_v1.0.md).  
**Responsable de esta revisión:** Chat 2 — Game Design.  
**Tarea vinculada:** [ISEK-001](Specifications/ISEK-001.md), base corporal y visual conforme a [DEC-001](DECISIONS.md). La aprobación de esa dirección no aprueba este borrador completo.  
**Motor:** Unity 6; proyecto local inspeccionado: 6000.6.7f1.  
**Dirección artística:** anime 3D con cel shading.  
**Documento base:** [GameDesignDocument_v0.5.md](GameDesignDocument_v0.5.md).  
**Especificación de aceptación:** [Compañera v0.1](Especificacion_Companera_v0.1.md).  
**Ubicación documental vigente:** `C:\Users\PC\Documents\Game Idea\Isekaid\Documentation`.

Esta revisión conserva literalmente el cuerpo de las secciones 1–16 y el registro de la v0.5. Las secciones 17–21 añaden estado actual, trazabilidad, prioridades y control de decisiones. Los detalles marcados como propuesta en la v0.5 mantienen ese estado. La versión 0.6 no aprueba por sí sola sus ampliaciones.

**Actualización de dirección, 28/10/2026:** DEC-001 establece el modelo **Woman de Fly_jkey**, disponible como `assets/woman.fbx`, como base candidata principal para ISEK-001, con apariencia neutral equilibrada. Pecho y caderas son independientes; cintura está condicionada a evaluación técnica. La especificación [ISEK-001 v0.1](Specifications/ISEK-001.md) define cuatro puertas y 16 criterios, en estado PROPUESTA. La documentación técnica de la sección 18 describe la revisión del 27/10; no es una auditoría nueva del proyecto reubicado. La lámina v01 sigue siendo candidata y no representa una aprobación del modelo final.

La copia de v0.5 se recuperó del texto íntegro de una lectura local anterior del 27/10/2026: su archivo de Downloads ya no estaba disponible al realizar esta revisión. Se conserva ese contenido, incluida su nota histórica de entrega. No se afirma identidad binaria con el archivo original desaparecido ni que dicha nota describa la ubicación actual.

## 1. Visión general

Juego **isekai** de acción y exploración en tiempo real, en primera persona, ambientado en un mundo de fantasía medieval de estructura semiabierta. El protagonista llega desde otro mundo y el jugador ve principalmente sus brazos. La principal característica del juego es una compañera adulta totalmente central para la aventura: el jugador la conoce al inicio como una desconocida, la protege, convive con ella y observa cómo las experiencias compartidas desarrollan su amistad, identidad y capacidades. El romance es una ruta opcional.

**Principios:**
- La compañera no es una misión de escolta pasiva: toma decisiones, ayuda, recuerda y tiene intereses propios.
- El primer rescate es el punto de partida de la confianza, no una amistad ni romance automáticos. El juego debe sentirse como conocer a una persona nueva.
- Su supervivencia importa, pero la IA no debe provocar derrotas injustas por fallos de navegación o cambios impredecibles.
- La personalidad afecta las decisiones y la expresividad, pero no determina la apariencia física.
- Las interacciones y los diálogos funcionan sin conexión, sin dependencia de una API de IA o tokens de pago.
- Romance opcional; la ruta de amistad debe tener contenido propio.

## 2. Protagonista

- Cámara en primera persona; se muestran solo brazos durante la jugabilidad normal.
- Combate **en tiempo real**. Propuesta de controles básicos: ataque ligero, ataque fuerte, bloqueo, esquiva, interacción y órdenes a la compañera.
- Prioridades del jugador: explorar, enfrentarse a enemigos y evitar que la compañera resulte incapacitada.
- Detalles del arsenal, resistencia y controles definitivos: **pendientes**.

## 3. Compañera: identidad y apariencia

- Personaje femenino adulto, modelo compartido de estilo anime 3D.
- Proporciones iniciales promedio; ajuste posterior de pecho y caderas.
- Cabello inicial largo y suelto; rostro neutro.
- Vestuario inicial: top sencillo y pantalón.
- Colores de cabello, ojos, top y pantalón personalizables; los colores iniciales son provisionales.
- Apariencia y personalidad son independientes.
- Plan técnico: rig humanoide, ropa deformable y Shape Keys/Blend Shapes para variaciones compatibles; verificar deformaciones de ropa y cuerpo en poses reales.

### Expresiones y lenguaje corporal

- Rostro anime detallado para situaciones normales, con expresiones cómicas 2D exageradas cuando corresponda, inspirado en el contraste visto en las referencias visuales del usuario.
- Sistema facial híbrido: controles faciales sencillos y recursos de textura/atlas para ojos, boca, sonrojo y signos cómicos. La implementación final se decidirá después de pruebas de modelado.
- Misma geometría base con perfiles expresivos diferentes según personalidad: postura, mirada, ritmo de gestos y variaciones de expresión.
- Animaciones compartidas: reposo, caminar, correr, sentarse, levantarse, interactuar y reaccionar; animaciones de clase para combate.
- Prioridad de animación: supervivencia > combate > interacción importante > movimiento > gestos sociales > actividades ambientales.

## 4. Personalidad, relación y evolución

**Arquetipos de inicio:** tsundere, yandere y tímida. Son plantillas de rasgos, no identidades visuales ni comportamientos absolutos.

**Capas de personaje:**
- Personalidad base y rasgos: tendencias relativamente estables.
- Estado emocional: miedo, alegría, enfado, cansancio y otras emociones temporales.
- Relación: confianza, cercanía, respeto y vínculo; romance solo por decisiones recíprocas, nunca por acumulación automática de puntos.
- Memoria: eventos importantes estructurados como rescates, promesas y experiencias compartidas.
- Preferencias y hábitos: afinidad por actividades, comidas, lugares y personas; pueden desarrollarse con el tiempo.
- Progresión de combate: destreza con su clase y mejores decisiones tácticas, independiente de la evolución emocional.

Una compañera tímida puede hacerse competente y valiente en combate sin dejar de ser reservada socialmente. Misiones personales e iniciativas propias pueden fortalecer el vínculo y desbloquear nuevas interacciones.

## 5. Clases de la compañera

- **Espadachina:** defensa cercana, bloqueos e intercepción; más exposición al daño.
- **Maga:** ofensiva y apoyo, barreras y posibles curaciones; maná y vulnerabilidad a corta distancia.
- **Arquera:** ataque a distancia, reconocimiento y reposicionamiento; vulnerable al ser rodeada.

Las clases son independientes de la personalidad. El jugador puede elegir una clase; cambiarla mediante entrenamiento se considera una **propuesta** para etapas posteriores. En el primer prototipo puede implementarse solo una clase para verificar el sistema.

## 6. Interacciones y diálogos sin costos recurrentes

- Biblioteca local de diálogos preparados, variantes por intención y selección contextual según personalidad, memoria, emociones y relación.
- Las conversaciones importantes, eventos de vínculo y misiones personales se escriben y revisan durante el desarrollo.
- La compañera puede iniciar conversaciones breves o proponer misiones personales, pero solo cuando el contexto lo permita.
- Un director de conversación bloquea charlas no urgentes durante peligro, combate, secuencias importantes o concentración del jugador.
- Algunas propuestas se guardan pendientes hasta una oportunidad segura; los comentarios que pierdan relevancia se descartan.
- Avisos de peligro son una excepción: pueden interrumpir cuando son útiles.
- Evitar repeticiones mediante estado de conversación, frecuencias, tiempos de espera y priorización de contenido no escuchado.
- No depender de modelos de lenguaje externos en tiempo de ejecución. Un modelo local sería opcional y no forma parte del MVP.

## 7. IA y supervivencia de la compañera

- Sigue al protagonista, evita obstáculos, mantiene una distancia funcional y busca posiciones de acuerdo con su clase.
- Órdenes previstas: **Sígueme**, **Espera aquí**, **Apóyame** y **Retírate**; las órdenes definitivas se ajustarán tras probarlas.
- Las decisiones evalúan peligro, salud, distancia, rol táctico, órdenes, personalidad y experiencia.
- Ante peligro grave prevalece la supervivencia sobre el deseo de mostrarse valiente o agradar al jugador.
- **Acordado:** al llegar a 0 de salud, la compañera queda incapacitada y comienza una ventana de **30 segundos de tiempo de juego activo** para rescatarla. Si no se salva antes de agotarse ese tiempo, muere, ocurre **Game Over** y se vuelve al último checkpoint. Que la cuenta solo avance durante el juego activo (y no en pausa) es una regla de implementación recomendada, todavía por validar en pruebas.
- **Acordado:** ciertos enemigos también pueden **secuestrar** a la compañera. Este estado es distinto de la muerte y conduce a una persecución o misión de rescate, sin Game Over inmediato. La mecánica exacta se desarrolla en la sección 11.
- Las órdenes de supervivencia y la IA deben minimizar muertes atribuibles a errores de navegación o decisiones incomprensibles.

## 8. Enemigos — ACORDADO

### A. Animales: prioridad por proximidad

**Ejemplos:** lobos, jabalíes y bestias salvajes.

- Atacan preferentemente al objetivo detectable más cercano, ya sea protagonista o compañera.
- Pueden cambiar de objetivo si cambia la proximidad o reciben estímulos importantes.
- Su comportamiento hace que separarse de la compañera tenga consecuencias.

### B. Hordas: prioridad por vulnerabilidad

**Ejemplos:** goblins, grupos numerosos de criaturas o bandidos.

- Aparecen en grupos y evalúan objetivos heridos, aislados o con menos capacidad inmediata de defensa.
- No deben conocer mágicamente puntos exactos de salud ocultos: inferir vulnerabilidad de señales observables, posición, comportamiento o heridas visibles.
- Pueden rodear y distribuir amenazas entre jugador y compañera.
- Propuesta de equilibrio: limitar atacantes cuerpo a cuerpo simultáneos, especialmente alrededor de la compañera, y telegrafiar los ataques.

### C. Enemigos grandes: prioridad por el protagonista

**Ejemplos:** ogros, trolls y golems.

- Prefieren atacar al jugador y hacen mayor daño con golpes más lentos y reconocibles.
- La preferencia **no es exclusividad**: también atacan a la compañera si está cerca, les causa daño, se vuelve un blanco accesible o hay varios enemigos grandes.
- Ataques amplios pueden alcanzar a ambos sin importar su objetivo actual.
- Dos enemigos grandes pueden elegir blancos distintos: no hay reparto forzado ni inmunidad de la compañera.

### Reglas transversales

- Selección de objetivo basada en puntuaciones variables por arquetipo: cercanía, vulnerabilidad observada, amenaza recibida y preferencia de especie.
- Evitar oscilaciones rápidas entre objetivos con umbrales y tiempos mínimos de compromiso.
- Señales de intención legibles: orientación corporal, preparación de ataques, animación y sonido.
- El jugador debe disponer de herramientas para intervenir antes de un daño grave.

### Alerta de la compañera — ACORDADO

- La compañera puede advertir al jugador de enemigos detectados fuera de su campo de visión: «¡Cuidado, vienen por la derecha!».
- La advertencia depende de una detección válida (visión, audición u otra señal del juego), con dirección relativa al jugador.
- No debe hablar constantemente: prioridad elevada para amenazas nuevas, cercanas y relevantes; evitar duplicados.
- Sus habilidades de reconocimiento y autonomía pueden mejorar mediante experiencia.
- La alerta se permite incluso si las conversaciones sociales están bloqueadas por combate o peligro.

## 9. Protección activa del jugador — PROPUESTAS PARA DISEÑAR

Aún no quedan cerradas estas mecánicas; son candidatas para el siguiente bloque de planificación:

1. **Interponerse y bloquear:** escudo o arma bloquean ataques dentro de un cono frontal y con consumo de resistencia; no cubren automáticamente a la compañera si está detrás de otra trayectoria.
2. **Interrumpir:** un golpe bien sincronizado o una habilidad específica puede cancelar ataques enemigos telegrafiados, con limitaciones frente a enemigos grandes.
3. **Atraer atención:** atacar, bloquear o provocar a un enemigo puede subir la amenaza que atribuye al jugador, pero nunca garantizar que todos ignoren a la compañera.
4. **Orden defensiva contextual:** «Retírate» o «A cubierto» activa búsqueda de una zona segura y evita que la compañera se quede quieta en la trayectoria de ataques.
5. **Rescate bajo presión:** reanimación dentro de los **30 segundos acordados**; duración de la interacción, posibilidad de interrupción y salud recuperada todavía deben ajustarse con pruebas.
6. **Claridad en pantalla:** avisos discretos de amenaza sobre la compañera y sus peticiones de ayuda; evitar llenar la pantalla de indicadores.

**Objetivo de diseño:** protegerla requiere intervenir y decidir, sin convertirla en un NPC indefenso ni hacer que la IA cause fracasos arbitrarios.

## 10. Inicio de la relación y convivencia en el hogar — ACORDADO / PROPUESTAS

### Relación entre desconocidos — ACORDADO

- El protagonista y la compañera **se conocen por primera vez durante la historia**. No comienzan como mejores amigos ni como pareja.
- La amistad se construye mediante aventuras, conversaciones, ayuda mutua, misiones personales y convivencia cotidiana.
- La evolución debe observarse en diálogos, iniciativa, expresiones, gestos, preferencias y decisiones, no solo en barras numéricas.
- El romance es opcional: no se activa automáticamente al alcanzar un nivel alto de amistad.

### Hogar compartido y salidas en solitario — ACORDADO

- Ambos disponen de una **casa/hogar que funciona como lugar estable de descanso y convivencia**.
- El jugador puede salir **sin la compañera**, dejándola en casa. No es obligatoria su presencia en todas las aventuras.
- La reacción ante una salida depende de personalidad, relación actual, necesidades, planes y frecuencia con que el jugador decide dejarla en casa.
- Si el jugador la deja atrás repetidamente de una forma que ella percibe como desatención, **puede molestarse y dejar de ganar amistad mediante ese patrón de interacción**. Las consecuencias concretas se diseñarán evitando castigos automáticos por cada salida legítima.
- La compañera tendrá actividades propias en casa: descansar, practicar, leer, cocinar u otras relacionadas con sus intereses, dentro del alcance de producción disponible.

### Reglas de equilibrio propuestas (no cerradas)

- Una salida corta, justificada o avisada no debería causar una penalización constante; la combinación de promesas incumplidas, ausencia reiterada y preferencias podría generar malestar.
- El jugador debe poder explicar de forma breve por qué quiere salir solo (por ejemplo, peligro, sigilo, gestión personal) y hacer planes para realizar actividades juntos después.
- La reacción puede ser distinta: una tímida quizá aprecie un descanso; una tsundere podría disimular molestia; una yandere podría preferir acompañarlo. No convertir arquetipos en obligaciones inflexibles.
- La compañera conserva autonomía y preferencias, pero el jugador **siempre tiene la opción práctica de salir solo**. Debe existir una forma de recuperar la relación mediante tiempo compartido y cumplimiento de promesas.
- Evitar deterioro del vínculo por tiempo real fuera del juego. Evaluar solo acontecimientos de la partida que el jugador pueda comprender.

## 11. Secuestro, persecución y rescate — ACORDADO / PROPUESTAS

### Regla principal — ACORDADO

- No todos los enemigos buscan matar: **algunos pueden capturar o secuestrar a la compañera**.
- Ante un secuestro, el jugador debe poder **perseguir a los captores y rescatarla**.
- Ser secuestrada no equivale a morir y, por tanto, **no provoca Game Over inmediato**.

### Diseño inicial propuesto

- Dar la capacidad de captura a tipos apropiados de enemigos, por ejemplo ciertos bandidos o unidades de horda; los animales ordinarios no necesitan esta mecánica.
- La captura debe tener una señal visual y una ventana de intervención clara (agarrarla, inmovilizarla, cargarla y huir); el jugador puede interrumpirla o perseguir a los captores.
- Si escapan del área de combate, se activa una **misión de rescate** con información recuperable (rastro, pista o campamento identificado), para evitar pérdidas irrecuperables por quedarse atascado o perderlos de vista.
- La compañera puede intentar resistirse, huir o dejar indicios, según clase, estado y experiencia.
- El secuestro se maneja como un estado exclusivo de la incapacitación mortal: no se mantienen dos relojes contradictorios. Sus reglas exactas de activación y fracaso quedan pendientes.
- **No generar secuestros aleatorios fuera de pantalla cuando ella está a salvo en casa en el MVP**; cualquier amenaza en el hogar necesitaría un evento explícito y jugable en una etapa posterior.

### Estados de supervivencia previstos

`Activa → Herida → Incapacitada (30 s) → Rescatada`  
`Incapacitada (30 s) → Muerta → Game Over → Último checkpoint`  
`Activa/Herida → Captura visible → Secuestrada → Persecución o búsqueda → Rescatada`

### Persistencia y justicia

- Los checkpoints deben restaurar un estado coherente de ambos personajes, recuerdos y misiones. Un rescate incompleto no debe dejar la partida bloqueada al cargar.
- Comprobar que cualquier secuestro tenga pistas, un destino válido y condiciones posibles de rescate.
- Los detalles de tiempo de captura, enemigos permitidos, derrota del jugador durante persecución y estado tras el rescate siguen **pendientes de diseño**.

## 12. Prólogo isekai, primera misión y primer encuentro — CONCEPTO ACORDADO

### Premisa — ACORDADO

- El protagonista **viaja desde su mundo a un entorno medieval de fantasía** (isekai). El mecanismo exacto del traslado —invocación, accidente, portal u otro— queda pendiente de decidir.
- El protagonista **no conoce de antemano** las reglas del nuevo mundo ni a la compañera. Se aprovecha ese desconocimiento para introducir de manera natural las mecánicas y el contexto sin largos tutoriales.
- Durante la **primera misión**, descubre a la futura compañera **atrapada en una trampa o situación equivalente** y puede rescatarla. Este acto es el primer antecedente positivo de confianza entre dos desconocidos.
- El rescate **no establece automáticamente** amistad profunda, obediencia, amor ni deuda eterna. La compañera conserva iniciativa, cautela y capacidad de decidir qué hacer.

### Secuencia jugable de apertura — PROPUESTA

1. **Llegada:** breve secuencia en primera persona: el protagonista aparece desorientado en un camino o claro próximo a una pequeña población. Se introduce movimiento y observación sin explicarlo todo de golpe.
2. **Primer contacto:** una persona del lugar ofrece una tarea modesta (por ejemplo, recuperar un paquete o recolectar materiales en un sendero). Sirve como primera misión y presenta interacción e inventario básico.
3. **Señales:** en el bosque se escuchan ruidos y se encuentran huellas o indicios de trampas. El jugador puede examinar el entorno y acercarse con precaución.
4. **Encuentro:** la futura compañera está inmovilizada por una **trampa colocada por bandidos**. **No está incapacitada por combate**, por lo que todavía no se activa el temporizador de rescate de 30 segundos.
5. **Rescate:** interacción breve para desactivar el mecanismo, sin convertir el momento en una secuencia de fallo irremediable. Puede haber una reacción distinta según su arquetipo.
6. **Presentación:** diálogo corto en el que ambos se identifican y expresan precaución o agradecimiento. La compañera puede explicar cómo llegó allí, sin revelar necesariamente toda su historia.
7. **Primer peligro compartido:** un encuentro sencillo (animal o pequeño grupo de enemigos) sirve para introducir protección y cooperación. La compañera participa según sus capacidades, en vez de actuar como objeto pasivo de escolta.
8. **Decisión de viajar juntos:** surge una motivación **mutua** y concreta para acompañarse temporalmente (por ejemplo, llegar al mismo asentamiento o investigar el origen de las trampas). Ella acepta por interés propio. El vínculo duradero se gana durante las siguientes misiones.
9. **Regreso y checkpoint:** la primera misión concluye en un lugar seguro. Se guarda el estado de la partida y se habilitan las primeras conversaciones y actividades conjuntas.

### Ejemplo de la primera reacción según personalidad — PROPUESTA

- **Tsundere:** «¡Podía salir sola! ...Bueno, gracias. ¿Tú quién eres?»; mezcla orgullo con gratitud.
- **Tímida:** «G-gracias... Pensé que nadie iba a encontrarme»; se muestra cautelosa, pero aliviada.
- **Yandere:** «Gracias por ayudarme. No esperaba que alguien se detuviera»; manifiesta interés y atención, **sin asumir romance ni obsesión inmediata**.

El arquetipo afecta tono, gestos, mirada y variantes de diálogo, **no** la apariencia ni los hechos de la escena. El nivel inicial de confianza debe ser bajo/moderado y el recuerdo del rescate se registra como un evento concreto; su efecto no debe permitir explotar el progreso de amistad.

### Relación con otros sistemas

- La trampa de **bandidos** del primer encuentro es un **evento narrativo de exploración**, diferente de los estados de captura enemiga y de la incapacitación por 0 HP.
- El jugador todavía no conoce necesariamente las clases de combate; sus tres posibilidades (espadachina, maga, arquera) se mantienen abiertas. Se decidirá si su clase viene definida desde el inicio o se elige mediante una escena de entrenamiento posterior.
- **La casa no se entrega gratuitamente durante el rescate:** se propone obtener o habilitar un hogar seguro tras varias misiones, cuando ya existe una razón de convivencia entre ambos.
- Primeras misiones sugeridas: completar el encargo inicial, ayudar a investigar las trampas, viajar al asentamiento y decidir si prolongan la alianza. Las misiones personales y rutas de amistad/romance se abren después, a ritmo gradual.
- Para el prototipo jugable basta un camino de llegada, una pequeña zona de bosque, la trampa y un encuentro sencillo. El mundo semiabierto puede construirse después.

### Pendiente de concretar

- Causa del traslado isekai y si el protagonista podrá regresar a su mundo.
- Motivación personal definitiva de la compañera y detalles de su pasado (independientes de la personalidad elegida).
- Nombre de la aldea, PNJ que encarga la primera misión e identidad del grupo de bandidos.
- Motivo compartido para la alianza, ritmo con el que se obtiene la vivienda y momento de la elección de clase.
- Tono del prólogo: misterio, aventura ligera, comedia o mezcla de estilos.

### Primera trampa colocada por bandidos — DECISIÓN ACORDADA (v0.5)

La futura compañera **cayó en una trampa preparada por bandidos**, no en una trampa natural ni de cazadores. El rescate se produce durante la primera misión del protagonista. La trampa no determina que ella sea indefensa: fue una emboscada diseñada para capturar viajeros y cualquiera podría haber caído.

**Propuesta narrativa para convertirlo en una misión jugable (aún por validar):**

**Misión provisional: «El sendero bloqueado»**

1. Al llegar a la aldea, el protagonista acepta revisar un sendero comercial porque algunos viajeros se han retrasado. Esto introduce una misión simple y un objetivo visible en el mapa.
2. En el bosque encuentra indicios de actividad humana: cuerda cortada, huellas, carga abandonada o marcas recientes en árboles. Puede descubrir un ejemplo desactivado de trampa, para enseñar a observar el entorno.
3. Escucha una voz pidiendo ayuda y descubre a la compañera suspendida o inmovilizada por una trampa de cuerda y contrapeso oculta junto al camino. Se evita una herida grave que le impida participar en la escena siguiente.
4. El jugador se acerca, examina el mecanismo y lo desactiva. La secuencia presenta **interacción contextual**. No se utiliza el temporizador de 30 segundos: ese plazo pertenece exclusivamente a una incapacitación por salud cero.
5. Ella responde de acuerdo con la personalidad seleccionada, mantiene cautela y revela que también necesitaba atravesar el sendero. Como motivación provisional, estaba buscando una ruta segura para llegar a la aldea; su trasfondo personal queda abierto.
6. Una patrulla pequeña de bandidos se acerca a comprobar la trampa. La compañera advierte al protagonista, introduciendo su sistema de alertas, y ambos superan una primera amenaza controlada.
7. Después del encuentro, ella propone acompañarse **solo hasta la aldea**, porque el camino continúa siendo peligroso y ambos tienen un destino inmediato en común. La alianza es voluntaria y temporal; el rescate no supone devoción, obediencia ni romance automáticos.
8. En la aldea informan sobre los bandidos. Concluye la primera misión, se registra un **checkpoint** y se habilitan las primeras charlas y encargos de investigación. El hogar compartido se consigue bastante después, mediante progreso jugable.

**Variación expresiva:** la trama, la trampa y el resultado del rescate son comunes a los tres arquetipos; cambian la reacción inmediata, el tono y el lenguaje corporal. Una tsundere puede ocultar su gratitud con orgullo; una tímida pedir ayuda con nerviosismo; una yandere ser muy atenta al rescate **sin enamorarse instantáneamente**.

**Decisiones de diseño para que funcione bien:**

- Dar una pista visual y una pista auditiva antes del encuentro: el jugador descubre el incidente, no aparece la compañera teletransportada a su lado.
- La secuencia no permite dejarla atrapada de manera irreversible. Si el jugador se aleja, se conserva el objetivo de ayuda y la misión no se rompe.
- No convertir la primera trampa en un secuestro completo: el sistema de secuestros posterior puede presentarse como una escalada lógica de las tácticas de los bandidos.
- Introducir la alerta de la compañera antes de exigir que el jugador coordine su combate; evitar una muerte injusta en el tutorial.
- Registrar un recuerdo único `FirstRescue_BanditTrap` y un indicador de confianza inicial moderado. No incrementar el vínculo cada vez que se recargue un checkpoint o se repita la escena.
- Mantener su identidad independiente: no queda obligada a seguir al protagonista por deuda. Una conversación posterior deberá dar una razón convincente para prolongar la compañía.

**Pruebas previstas para esta secuencia:** descubrir la trampa, rescatarla, ver una reacción por cada personalidad, recibir una alerta coherente, completar el encuentro, llegar a la aldea y cargar el checkpoint sin duplicar recuerdos.

## 13. Mundo y primer prototipo

**Mundo final:** mundo isekai de fantasía medieval con estructura semiabierta, con rutas, asentamientos, zonas exteriores, una casa/hogar estable y puntos de descanso. Extensión y contenido definitivos: pendientes.

**Primer prototipo acotado (propuesta):**
- Prólogo en un camino y bosque pequeño: llegada isekai, encargo simple, encuentro en la trampa y primer rescate; no el mundo abierto completo.
- Una arena o claro de bosque de prueba para combate, no el mundo abierto completo.
- Protagonista con arma y defensa básicas.
- Compañera con seguimiento, salud, alertas, al menos una clase funcional y tres perfiles de personalidad.
- Un animal, un pequeño grupo de enemigos de horda y un enemigo grande.
- Pruebas de cambio de objetivo, supervivencia, retirada, protección y rescate con ventana de 30 s.
- Una escena doméstica o espacio sencillo donde la compañera pueda quedarse mientras el jugador sale solo; registrar su reacción según arquetipo y repetición.
- Un secuestro guionizado de prueba con posibilidad de impedir la captura, perseguir y recuperar a la compañera (alcance a validar).
- Sin conexión y sin uso de API por tokens.

## 14. Arquitectura provisional de Unity

- `CompanionBrain`, `CompanionMotor`, `CompanionCombatBrain`, `CompanionPersonality`, `CompanionNeeds`, `CompanionRelationship`, `CompanionMemory`, `CompanionPreferences`, `CompanionHabits`, `CompanionProgression`, `CompanionAppearance`, `CompanionFaceController`, `CompanionConversationDirector`.
- `EnemyPerception`, `EnemyTargetSelector`, `EnemyBehaviorProfile`, `EnemyCombatController`.
- `PlayerCombatController`, `PlayerDefenseController`, `CompanionCommandController`, `RescueInteraction`.
- Candidatos nuevos: `CompanionLifeState`, `DownedRescueTimer`, `KidnapController`, `RescueQuestState`, `HomePresenceController`, `SoloOutingTracker`, `CompanionAbsenceReaction` y `CheckpointManager`. Evitar crear componentes separados si pueden integrarse de forma más sencilla al implementar.
- ScriptableObjects para configuraciones, estados de partida serializables para progreso y recuerdos.
- Componentes y nombres sujetos a refinamiento antes de escribir código.

## 15. Herramientas y producción

- Unity 6 y C# para el juego.
- Blender + Contour3D para geometría editable, rig y deformaciones.
- Build 3D Game Rooms para escenarios cuando corresponda.
- Game Development Studio para validación, preparación y organización de recursos, sujeto a disponibilidad de herramientas locales.
- Game Studio está especializado en navegadores, por lo que no reemplaza el desarrollo nativo en Unity.
- Cualquier servicio externo de pago requiere autorización específica previa; no es una dependencia del juego publicado.

## 16. Decisiones pendientes

- Mecánicas exactas del protagonista para defender, provocar, interrumpir y rescatar; duración y condiciones de la reanimación dentro de los 30 segundos.
- Niveles de dificultad, daño amigo y límites de atacantes simultáneos.
- Comportamiento detallado de jefes y enemigos especiales.
- Una clase funcional a priorizar en el primer prototipo.
- Mecánica concreta de captura/secuestradores, pistas de persecución, posibles consecuencias después del rescate.
- Reglas de convivencia y evaluación de salidas en solitario sin penalizaciones injustas.
- Estructura del mundo, interfaz, guardado/checkpoints, controles y especificación final de animaciones.
- Modelado efectivo, pruebas del rig y pipeline final de importación a Unity.
- Causa del viaje isekai, identidad de los bandidos y motivo mutuo de largo plazo para continuar viajando juntos.

---

**Registro de cambios v0.4:** integrado el concepto isekai, el rescate de la compañera atrapada durante la primera misión, una propuesta de prólogo jugable que introduce movimiento, interacción y cooperación, respuestas iniciales distintas según personalidad y la transición gradual hacia la convivencia. El contenido de v0.3 se conserva. Se distinguen conceptos acordados de detalles narrativos pendientes de aprobación.

**Registro de cambios v0.5:** confirmado que la primera trampa fue colocada por bandidos. Añadida la propuesta jugable «El sendero bloqueado», consecuencias del rescate, continuidad narrativa con el sistema posterior de secuestros y pruebas de aceptación del prólogo. Las opciones de encargo, motivación previa y patrulla son propuestas, no decisiones cerradas.

## 17. Revisión de visión y acuerdos — 27/10/2026

### 17.1 Experiencia que debemos proteger

El centro de Isekaid es conocer a una desconocida y desarrollar una relación mediante experiencias mutuas. La compañera tiene intereses y decisiones propios. Su valor no depende de que el jugador elija romance. El comportamiento y la presentación deben expresar el mismo estado: no basta que la programación registre una emoción si la cara, el cuerpo y las palabras la contradicen.

El prototipo técnico es una herramienta de comprobación. Sus primitivas, frases limitadas, paletas iniciales y reinicio de encuentro no definen el techo de producción ni sustituyen la visión completa. La entrega se puede dividir en hitos sin eliminar requisitos.

### 17.2 Registro de decisiones preservadas

| ID | Decisión aprobada o reafirmada | Procedencia | Comprobación prevista |
|---|---|---|---|
| A-01 | Acción y aventura isekai, primera persona, fantasía medieval semiabierta | v0.5 §§1–2, 12–13; solicitud actual | Experiencia de aventura y continuidad del mundo en hitos posteriores |
| A-02 | La relación empieza entre desconocidos y evoluciona por experiencias | v0.5 §§1, 10, 12; solicitud actual | Secuencia integrada y MEM-03 |
| A-03 | Mujer adulta anime 3D, proporciones base promedio, cabello largo suelto, top y pantalón | v0.5 §3; solicitud actual | VIS-01 a VIS-04 |
| A-04 | Colores independientes de cabello, ojos y ropa; pecho y caderas ajustables | v0.5 §3; prioridad inmediata actual | PER-01 a PER-05 |
| A-05 | Apariencia, personalidad y clase son independientes | v0.5 §§3–5 | PER-05, FAC-04 |
| A-06 | Tsundere, yandere y tímida con diferencias de conducta y expresión | v0.5 §4; solicitud actual | COR-01 a COR-04, DIA-01 |
| A-07 | Rostro normal anime y expresiones cómicas mediante sistema híbrido | v0.5 §3; solicitud actual | FAC-01 a FAC-04 |
| A-08 | Emociones, recuerdos, preferencias, hábitos, desarrollo personal e iniciativa | v0.5 §§4, 6, 10; solicitud actual | DIA, MEM y AUT |
| A-09 | Clases espadachina, maga y arquera | v0.5 §5; solicitud actual | Hito de combate; primera clase pendiente |
| A-10 | Amistad completa, romance opcional y misiones personales | v0.5 §§1, 4, 10; solicitud actual | MEM-06 y especificación narrativa posterior |
| A-11 | Juego sin conexión y sin dependencia de APIs generativas de pago | v0.5 §§1, 6, 13; solicitud actual | DIA-04 y prueba de ejecución integral |
| A-12 | Primer encuentro en una trampa de bandidos, distinto de 0 HP y de un secuestro posterior | v0.5 §12 y decisión v0.5 | AUT-06 y regresión del primer rescate |
| A-13 | 0 HP → incapacitación; rescate dentro de 30 s; fracaso → muerte, Game Over y checkpoint | v0.5 §7 | Regresión vital y futuro checkpoint real |
| A-14 | Secuestro diferente de muerte y sin Game Over inmediato; hogar y salidas en solitario | v0.5 §§10–11 | Hitos posteriores, no eliminados |
| A-15 | Diseño mantiene GDD y especificaciones; desarrollo modifica Unity | Solicitud actual | Entregas documentales separadas y trazables |

**Matiz conservado de A-13:** la v0.5 denomina acordados los 30 segundos, pero marca la pausa del contador como recomendación pendiente de validar. El prototipo implementa tiempo activo y el registro de desarrollo informa una prueba de pausa. Ese resultado técnico no cambia silenciosamente el estado de aprobación de la redacción original. Se recomienda ratificar el tiempo activo al cerrar la regla de supervivencia; no se modifica su comportamiento en esta entrega.

**Precisión de A-04:** la v0.5 situaba los ajustes corporales como trabajo posterior; la solicitud actual los lleva expresamente a la prioridad inmediata. Los extremos anatómicos concretos siguen pendientes de aprobación.

### 17.3 Propuestas que siguen abiertas

Nombres, biografía definitiva, mecanismo del traslado isekai, aldea, identidad de bandidos, patrulla del prólogo y motivo de alianza duradera no se fijan por esta revisión. “El sendero bloqueado” sigue siendo una propuesta. Tampoco se aprueba automáticamente la lámina v01, sus colores, los umbrales numéricos de la especificación o una clase de combate inicial.

Las limitaciones de producción se registran como pendientes con dependencia y evidencia necesaria. Un recorte de visión requiere describir qué se elimina, por qué y qué experiencia se pierde, y consultarlo antes de reemplazar el acuerdo.

## 18. Estado del proyecto y brecha respecto de la visión

### 18.1 Evidencia utilizada

- Inspección directa de `ProjectSettings/ProjectVersion.txt` y de los archivos propios enumerados abajo.
- [Registro de cambios](../readme.txt), con entradas de los prototipos 0.1 y 0.2.
- Resultado del chat de desarrollo “Define el proyecto según la guía”, con entrega 0.2: creador y aventura separados.
- Revisión visual de [REF-01](references/companion-concept-v01.png) como imagen de concepto, no como captura del juego.
- GDD v0.5 recuperado y solicitud de dirección del 27/10/2026.

No se ejecutó Unity durante esta revisión de diseño. Las pruebas de compilación, Play Mode y Game View de la versión 0.2 son resultados informados por desarrollo y su registro; no se presentan como pruebas repetidas aquí.

### 18.2 Hallazgos verificables y consecuencias

| Sistema | Evidencia actual | Brecha de diseño | Qué conservar |
|---|---|---|---|
| Flujo de creación | `CharacterCreation.unity` y `Game.unity`; entrega 0.2 registrada | Flujo útil, presentación aún provisional | Separación de escenas y continuidad del perfil |
| Modelo | `PrototypeSceneBuilder.cs` crea cabeza, cuerpo, cabello y ropa mediante primitivas | No demuestra modelo anime, rig, deformación ni animación facial | Control e interacción alrededor del personaje, sujetos a integración |
| Personalización | `CompanionProfile.cs` v1: personalidad y cuatro índices; `CompanionPalette.cs`: cuatro opciones por zona | No hay parámetros de pecho/caderas en el perfil inspeccionado | Paletas iniciales, validación y persistencia compatible |
| Materiales | `CompanionAppearance.cs` aplica `_BaseColor` por zonas con MaterialPropertyBlock | Cambiar tinte de primitivas no demuestra calidad de ojos, UV ni sombreado del modelo final | Separación de zonas y aplicación de apariencia |
| Guardado | `CompanionProfileStore.cs` guarda perfil en PlayerPrefs; vuelve a valores base cuando la carga no es válida | No guarda aventura, recuerdos ni relación; requiere migración al ampliar perfil | Perfil válido existente y recuperación sin pérdida al evolucionar |
| Personalidad | `CompanionState.TalkLine` alterna dos frases por perfil; rescate tiene variación propia | Falta selección por contexto/emoción y lenguaje corporal | Primeras intenciones, separación de estados y ausencia de romance instantáneo |
| Memoria | `RemembersFirstRescue` y `Trust`; reinicio crea un estado nuevo | Un hecho en sesión, sin memoria persistente ni preferencias | Regla de rescate único dentro del estado actual |
| Autonomía | `CompanionController` sigue mediante NavMesh, recibe esperar/seguir y consulta posición navegable | No demuestra evaluación completa de percepción, ruta inválida, decisiones sociales o tácticas | Navegación y órdenes como base para casos de recuperación |
| Supervivencia | Estado Downed, contador 30 s y postura inclinada; pruebas de regresión existentes | Ensayo de incapacidad, no combate, reanimación animada ni checkpoint de aventura completo | Separación trampa/incapacidad y temporizador ya comprobado por desarrollo |

Rutas de contraste: [CompanionState](../Isekaidunity/Assets/_Project/Scripts/Companion/CompanionState.cs), [CompanionProfile](../Isekaidunity/Assets/_Project/Scripts/Companion/CompanionProfile.cs), [CompanionController](../Isekaidunity/Assets/_Project/Scripts/Companion/CompanionController.cs), [CompanionAppearance](../Isekaidunity/Assets/_Project/Scripts/Companion/CompanionAppearance.cs), [CompanionProfileStore](../Isekaidunity/Assets/_Project/Scripts/Companion/CompanionProfileStore.cs), [CompanionPalette](../Isekaidunity/Assets/_Project/Scripts/Companion/CompanionPalette.cs), [constructor del prototipo](../Isekaidunity/Assets/_Project/Editor/PrototypeSceneBuilder.cs) y [pruebas existentes](../Isekaidunity/Assets/_Project/Editor/PrototypePlayModeChecks.cs).

Estas observaciones están acotadas al código propio revisado. No califican todos los paquetes de terceros ni afirman que los ZIP externos de personajes estén listos para integrarse.

### 18.3 Veredicto de diseño

Existe una base técnica reutilizable, pero no se ha demostrado la aceptación visual y social de la compañera definida en el GDD. Conviene priorizar presencia, expresión y coherencia de interacción antes de ampliar el mapa. Esa prioridad organiza el trabajo; no elimina combate, clases, convivencia ni progresión.

## 19. Especificación de la siguiente mejora

La [especificación de compañera v0.1](Especificacion_Companera_v0.1.md) contiene siete sistemas con propósito, datos, respuestas situacionales y 34 criterios de aceptación:

| Sistema | Resultado buscado | Casos |
|---|---|---|
| VIS | Modelo anime 3D real e integración visual comprobable | 4 |
| PER | Apariencia y cuerpo editables, ropa compatible y perfil conservado | 5 |
| FAC | Expresiones normales/cómicas coherentes e interrumpibles | 4 |
| COR | Personalidades legibles por postura, gestos y mirada | 4 |
| DIA | Conversación local pertinente, sin repetición mecánica ni charla en peligro | 5 |
| MEM | Emoción causal, recuerdos, gustos y cambios observables | 6 |
| AUT | Seguimiento comprensible, iniciativa segura y recuperación ante bloqueos | 6 |

La suma es 34. Las cifras, catálogos y diálogos nuevos están identificados como propuestas. Ningún caso se declara aprobado ni ejecutado por aparecer en la lista.

### Hitos propuestos

1. **H1: presencia visual y personalización.** Modelo real, rig, ropa, color, cuerpo ajustable, locomoción y rostro híbrido en creador y aventura. Prueba de los tres perfiles expresivos con la misma apariencia.
2. **H2: identidad social observable.** Conversación contextual, emoción, memoria, preferencia de prueba, iniciativa y navegación robusta. La demostración debe producir un acontecimiento y reflejarlo después; los presets sirven para inspección, no como evidencia de progresión conseguida.
3. **H3: cooperación bajo peligro real.** Una clase por seleccionar, percepción, enemigo, daño, rescate derivado del combate y checkpoint coherente. Sin sustituirlo por un botón que incapacita.
4. **Después:** ampliar clases, misiones personales, hogar, secuestros y relaciones según la v0.5, con especificaciones propias y sin escribir toda la trama antes de probar los sistemas.

No se propone reiniciar el proyecto ni imponer todos los nombres de componentes de la sección 14. Desarrollo conserva y adapta lo que sirva; la documentación exige comportamientos y evidencia.

## 20. Revisión, aprobación y entrega a desarrollo

Se aplica el [Workflow de desarrollo v1.0](Workflow_Desarrollo_v1.0.md) proporcionado por el usuario. Dirección asigna tareas, establece prioridades, aprueba especificaciones y acepta o rechaza entregas. Game Design prepara y mantiene las especificaciones; Unity Development implementa únicamente versiones aprobadas. H1/H2/H3 son una propuesta de secuencia, no IDs de tareas ni una autorización para desarrollar.

### 20.1 Regla de versiones

- `GameDesignDocument_v0.5.md`: base histórica preservada.
- `GameDesignDocument_v0.6_borrador.md`: revisión actual; no reemplaza silenciosamente los acuerdos de la base.
- `Especificacion_Companera_v0.1.md`: aceptación detallada propuesta para el siguiente ciclo.
- `references/companion-concept-v01.png`: candidata visual, sin condición de arte final aprobado.

Dirección emite la aprobación y debe identificar tarea, versión y alcance: por ejemplo, P-02 sobre extremos corporales o P-04 sobre gestos. Registrar fecha, alcance y cualquier ajuste en la siguiente revisión y en el registro de cambios. Los estados de especificación son PROPUESTA, APROBADA y REQUIERE CAMBIOS; los resultados de validación de entrega son ACEPTADO, REQUIERE CORRECCIONES, BLOQUEADO y REDISEÑO NECESARIO. No interpretar el silencio ni la aprobación de un aspecto como aceptación de todos los demás. Corregir una errata sin alterar sentido puede hacerse con una nota de revisión; cambiar una mecánica aprobada requiere consulta y registro de cambio de alcance.

### 20.2 Encargo propuesto para revisión de Dirección

**Objetivo propuesto:** acercar el prototipo a la visión mediante una compañera anime 3D real, personalizable y expresiva. Se propone comenzar por H1, conservando el flujo creador/aventura y los perfiles existentes; Dirección decide la tarea siguiente y autoriza su inicio.

**Entrada:** GDD v0.5, esta revisión y los criterios VIS/PER/FAC/COR de la especificación. Comprobar qué propuestas cuentan con aprobación antes de fijarlas como requisitos finales.

**Entregables:** fuente editable y exportación del personaje, integración en ambas escenas, perfil compatible, matriz corporal, clips de locomoción/expresiones, resultados por ID y limitaciones explícitas. Cuando exista un bloqueo, mostrar su evidencia y el impacto en el criterio afectado.

**Cierre:** no basta compilar. Mostrar que la compañera mantiene identidad y ropa al personalizarse, que se mueve, que cambia el rostro según contexto y que las personalidades se distinguen manteniendo la misma apariencia. La aceptación artística permanece pendiente hasta revisión humana.

Este encargo queda preparado en los archivos. No se ha enviado un mensaje al chat de desarrollo ni se le ha ordenado implementar automáticamente las propuestas.

### 20.3 Pendientes concretos para revisión

- Identidad visual final y asociación de las referencias anime originales.
- Extremos de pecho y caderas y límites de deformación aceptables.
- Catálogo facial, gestos por personalidad, tono y tiempos propuestos.
- Cadencia de iniciativa, distancias de seguimiento y recuperación de bloqueos.
- Alcance mínimo del guardado de recuerdos durante H2.
- Plataforma/hardware objetivo y primera clase de H3.
- Ratificación formal de la regla de tiempo activo de supervivencia, conservada como está mientras se revisa.

La preparación documental no depende de resolverlos todos ahora. Cada punto bloquea únicamente la aceptación o producción que necesite esa decisión.

## 21. Registro de esta revisión

**0.6 borrador, 27/10/2026:** recuperado y preservado el texto de la v0.5; añadida trazabilidad de acuerdos; actualizado el diagnóstico con la versión técnica 0.2 y lectura actual del código propio; elaborados criterios de aceptación de modelo, personalización, expresiones, cuerpo, conversación, memoria y autonomía; definidos hitos y evidencia; identificada la lámina v01 como candidata.

**Revisión administrativa 1, 27/10/2026:** incorporado el Workflow de desarrollo v1.0. Estado formal PROPUESTA; revisión a cargo de Dirección y mantenimiento a cargo de Game Design. Tarea pendiente de asignación. No cambian los acuerdos de diseño, los 34 criterios ni el cuerpo heredado de la v0.5.

**Revisión de trazabilidad 2, 28/10/2026:** vinculadas DEC-001 e ISEK-001; actualizado el directorio documental tras la reorganización en repositorio principal y submódulo Unity. Creada la propuesta específica para `woman.fbx`. Se mantienen sin cambios el cuerpo aprobado de la v0.5 y las propuestas generales fuera de esta tarea.

**Revisión de trazabilidad 3, 28/10/2026:** [DEC-002](DECISIONS.md) acepta la evaluación técnica G1 y autoriza G2-PROTO acotado; G3/G4 y aceptación artística siguen pendientes. La [dirección artística ISEK-001 v0.1](Specifications/ISEK-001_Direccion_Artistica_v0.1.md) añade cuatro láminas conceptuales, alternativas y criterios visuales en PROPUESTA. Las observaciones técnicas anteriores conservan su fecha; los resultados posteriores están en la [auditoría Woman](Audits/ISEK-001/INFORME.md). Este GDD continúa en BORRADOR.

**Validación documental:** integridad del cuerpo heredado, enlaces locales, IDs de criterios y coherencia entre estados aprobados/propuestos. La comprobación final de estos puntos se registra en `readme.txt` al publicar el paquete.

**Revisión de trazabilidad 4, 28/10/2026:** [DEC-003](DECISIONS.md) mantiene G2 en correcciones y limita la autorización a G2-CORR. La [dirección artística v0.2](Specifications/ISEK-001_Direccion_Artistica_v0.2.md) compara renders reales y objetivos conceptuales, localiza correcciones corporales, revisa AV01–04 y corrige referencias de prendas/expresión. Las precisiones nuevas son propuestas; este GDD permanece en BORRADOR.

**Sin ejecución de producto en esta revisión:** no se modifica código, escenas, prefabs, configuración, datos de partida ni assets dentro de Unity. No se realiza nueva compilación, Play Mode, medición de rendimiento ni build. No se afirma validación visual del juego a partir de la lámina.
