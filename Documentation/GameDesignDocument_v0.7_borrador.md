# Isekaid — Documento maestro de diseño (revisión consolidada)

**Versión:** 0.7 — BORRADOR para validación de Dirección.  
**Fecha:** 29/10/2026, Guatemala.  
**Estado:** propuestas y resultados diferenciados; **NO aprobado como nuevo maestro definitivo**, ni sustituye el historial v0.5/v0.6.  
**Motor de proyecto:** Unity 6000.6.7f1 · C# · URP.  
**Dirección artística:** anime 3D con cel shading.  
**Origen:** [GDD v0.5 acordado](GameDesignDocument_v0.5.md) + [borrador v0.6](GameDesignDocument_v0.6_borrador.md), DEC-007, PR #7/#9/#10/#11/#12 y prueba manual del usuario.  
**Ramas funcionales:** el documento no añade funcionalidades al juego. Requiere PR, revisión y aceptación para volverse maestro.

> **Regla de lectura:** «Acordado» corresponde a decisiones explícitas; «Implementado» a funciones presentes en el prototipo dentro del alcance probado; «Validado manualmente» recoge observaciones del jugador, no certificación artística universal; «Propuesta» no autoriza implementación. Las ideas heredadas de v0.5 mantienen su carácter previo salvo actualización expresa aquí.

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

## 3. Compañera: identidad, apariencia y expresividad

**Dirección adoptada (DEC-007):** compañera femenina adulta en estilo anime 3D; la apariencia procede de un catálogo local o de una importación autorizada de **VRM 1.0**. Se pausa el editor corporal propio (Woman/G2 se conservan como historial, sin nuevas iteraciones). El juego funciona sin Internet y no necesita servicios de generación de diálogo.

- **Independencia:** identidad, plantilla de personalidad, estado emocional, memoria, vínculo, clase, progresión y apariencia son datos separados. Cambiar VRM no crea una persona nueva ni modifica el rescate, confianza o clase.
- **Estado funcional comprobado:** un avatar incluido de catálogo (Adventurer, arte provisional) y un VRM local autorizado; vista previa, selección, copia privada administrada, carga/recuperación y perfil versionado. Soporte inicial probado en Windows; otros sistemas y VRM 0.x no están prometidos.
- **Expresiones:** la intención emocional se produce desde el sistema narrativo; un adaptador elige los canales faciales compatibles de cada avatar o vuelve a neutral. La ausencia de Blend Shapes no altera decisiones o recuerdos.
- **Animaciones:** reposo, caminar y correr con clips Humanoid cíclicos, mezclado continuo y velocidad suavizada; el NavMeshAgent conserva autoridad de desplazamiento. El movimiento continuo y el renderizado fueron reportados como mejorados en la prueba manual del 29/10/2026.
- **Calidad pendiente:** contacto de pies, retargeting para avatares no ensayados, calidad final del arte, gestos, mirada, expresión cómica y rendimiento extensivo necesitan más pruebas. Un archivo VRM privado no concede derechos de redistribución.
- **Catálogo futuro:** tres apariencias incluidas siguen siendo objetivo de fase posterior y requieren licencias, estilo visual y expresividad revisados.

La personalización de forma corporal y ropa se realiza, por ahora, fuera del juego. No se garantiza modificación universal de colores, mallas o prendas en VRM arbitrarios. Las reglas visuales no reemplazan las de identidad o relación.

## 4. Personalidad, relación y evolución

### Arquetipos y alcance

**Tres plantillas actuales:** Tsundere, Yandere y **Dandere** (etiqueta de producto para el antiguo nombre «Tímida»). El identificador serializado histórico `Shy` debe conservar compatibilidad con perfiles guardados; no cambiarlo sin una migración comprobada.

**Cuarta plantilla, propuesta NO APROBADA:** Deredere. Referencias para investigación: Marin Kitagawa y Ochaco Uraraka; no está implementada ni autorizada como opción de menú hasta que Dirección lo decida expresamente.

Las etiquetas son un punto de partida, no diagnósticos psicológicos ni reglas absolutas. Se desarrollarán **personajes originales**, sin copiar biografías, diálogos, diseños o escenas protegidas de Taiga Aisaka, Rin Tohsaka, Yuno Gasai, Hitori Gotoh, Shoko Komi, Hinata Hyuga, Marin Kitagawa u Ochaco Uraraka.

### Perfil profundo por personalidad (dirección de diseño)

Tomar de nuestras tarjetas de SillyTavern una metodología de autoría, **sin importar su motor al juego**. La ficha de cada compañera debe incluir:

- Identidad narrativa; rasgos, virtudes, defectos, contradicciones y límites propios.
- Valores, motivaciones, objetivos a corto y largo plazo, preferencias, hábitos y miedos.
- Desencadenantes emocionales y respuesta al estrés; cómo recupera su equilibrio.
- Voz, vocabulario, longitud de frases, humor, pausas, iniciativa y modo de expresar desacuerdo.
- Lenguaje corporal, postura, mirada, gestos compatibles con el avatar y alternativas accesibles.
- Reacciones ejemplificadas sobre los **mismos hechos**: rescate, conversación, espera, peligro, reanimación y momento seguro.
- Conocimientos y recuerdos que posee; nunca atribuirle sucesos no experimentados ni decidir por el protagonista.

**Tendencias iniciales propuestas para diferenciación:**

| Plantilla | Impulso principal | Contradicción útil | Cómo expresa cuidado |
|---|---|---|---|
| Tsundere | Recuperar agencia y resolver | Orgullo ante la vulnerabilidad | Ayuda práctica y cautela; gratitud poco ostentosa |
| Yandere | Comprender el vínculo y la cercanía elegida | Deseo de cercanía frente a límites e inseguridad | Atención sostenida y preocupación intensa, sin romance instantáneo |
| Dandere | Observar, pensar y afirmar su voluntad | Reserva social frente a valentía necesaria | Gestos discretos, escucha y decisiones claras |
| Deredere — **propuesta** | Compartir entusiasmo y calidez | Optimismo frente a inquietudes y responsabilidades | Iniciativa amistosa y afecto espontáneo, sin ingenuidad obligatoria |

### Capas independientes

Personalidad estable ≠ emoción temporal ≠ relación/confianza ≠ memoria de hechos ≠ intención ≠ forma de expresarla. Clase y progreso táctico son independientes. La compañera puede evolucionar en confianza, valentía y capacidades sin cambiar automáticamente de arquetipo. Romance opcional y recíproco; amistad con contenido propio.

**Estado observado:** el usuario comprobó que los textos se alternan entre plantillas, pero las tres todavía **se sienten demasiado iguales**. La revisión editorial del PR #11 propone 33 ajustes de texto/intención y perfiles de actuación; **aún no se implementan**. Requieren aprobación editorial, pruebas comparativas y adaptación visual segura. Ver [propuesta de diferenciación](Specifications/F3A_Diferenciacion_Personalidades_v0.1_PROPUESTA.md).

## 5. Clases de la compañera

- **Espadachina:** defensa cercana, bloqueos e intercepción; más exposición al daño.
- **Maga:** ofensiva y apoyo, barreras y posibles curaciones; maná y vulnerabilidad a corta distancia.
- **Arquera:** ataque a distancia, reconocimiento y reposicionamiento; vulnerable al ser rodeada.

Las clases son independientes de la personalidad. El jugador puede elegir una clase; cambiarla mediante entrenamiento se considera una **propuesta** para etapas posteriores. En el primer prototipo puede implementarse solo una clase para verificar el sistema.

## 6. Interacciones y diálogos sin costos recurrentes

**Dirección permanente:** diálogos locales, offline, sin API de IA ni tokens de pago. La compañera no habla por una expresión facial y no inventa información que desconozca.

**F3A integrada como prototipo:** biblioteca canónica local de 60 entradas (20 por personalidad anterior a renombrar Dandere), tres temas TRAP/FEEL/ROAD, nueve raíces y 18 alternativas. Existen elecciones, continuidad del cursor al cerrar/visitar el creador, control de repetición, reanimaciones por episodio y expresiones VRM con fallback. La prueba manual reportó conversaciones e incapacitación funcionando.

**Límites:** esto no significa diálogo libre ni memoria narrativa persistente de campaña. El sistema conserva estado de encuentro y hechos ya existentes; no implementa todavía promesas verificables, iniciativa espontánea general, percepciones de enemigos, conversaciones procedurales ni romance. Las diferencias de voz y presencia todavía están en revisión artística.

**Prioridad de respuesta:** supervivencia > combate/peligro real > interacción importante > movimiento > gestos sociales > actividades ambientales. Los comentarios seguros deben esperar su oportunidad; señales urgentes pueden interrumpir. Cambiar avatar no consume contenido ni modifica confianza.

**Próximo hito narrativo propuesto (N1/N2):** revisar y aplicar selectivamente los 33 cambios editoriales del PR #11, después añadir una presentación mínima de ritmo, mirada y gesto semántico mediante capacidades disponibles, sin forzar clips inexistentes ni bloquear botones. Comparar escenas equivalentes a ciegas con tres personalidades; conservar los IDs y rutas existentes.

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
- La reacción puede ser distinta: una dandere quizá aprecie un descanso; una tsundere podría disimular molestia; una yandere podría preferir acompañarlo. No convertir arquetipos en obligaciones inflexibles.
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
- **Dandere (antes Tímida):** «G-gracias... Pensé que nadie iba a encontrarme»; se muestra cautelosa, pero aliviada.
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

**Variación expresiva:** la trama, la trampa y el resultado del rescate son comunes a los tres arquetipos; cambian la reacción inmediata, el tono y el lenguaje corporal. Una tsundere puede ocultar su gratitud con orgullo; una dandere pedir ayuda con nerviosismo; una yandere ser muy atenta al rescate **sin enamorarse instantáneamente**.

**Decisiones de diseño para que funcione bien:**

- Dar una pista visual y una pista auditiva antes del encuentro: el jugador descubre el incidente, no aparece la compañera teletransportada a su lado.
- La secuencia no permite dejarla atrapada de manera irreversible. Si el jugador se aleja, se conserva el objetivo de ayuda y la misión no se rompe.
- No convertir la primera trampa en un secuestro completo: el sistema de secuestros posterior puede presentarse como una escalada lógica de las tácticas de los bandidos.
- Introducir la alerta de la compañera antes de exigir que el jugador coordine su combate; evitar una muerte injusta en el tutorial.
- Registrar un recuerdo único `FirstRescue_BanditTrap` y un indicador de confianza inicial moderado. No incrementar el vínculo cada vez que se recargue un checkpoint o se repita la escena.
- Mantener su identidad independiente: no queda obligada a seguir al protagonista por deuda. Una conversación posterior deberá dar una razón convincente para prolongar la compañía.

**Pruebas previstas para esta secuencia:** descubrir la trampa, rescatarla, ver una reacción por cada personalidad, recibir una alerta coherente, completar el encuentro, llegar a la aldea y cargar el checkpoint sin duplicar recuerdos.

## 13. Mundo y primer prototipo

**Visión final:** mundo isekai medieval de estructura semiabierta con caminos, asentamientos, hogar compartido, descanso, exploración y combate; amplitud y contenido por decidir.

**Prototipo jugable integrado y probado parcialmente (29/10/2026):**
- Creador de compañera con catálogo provisional o importación VRM 1.0 local y tres plantillas.
- Escena Game con trampa, rescate inicial, seguimiento/espera, estado de incapacidad y reanimación.
- Conversaciones ramificadas F3A con tres temas y respuestas según personalidad.
- Animación Humanoid de seguimiento continuo y mejora visible de luz/materiales; la persona que probó reportó la mejora.
- Persistencia de selección de apariencia y personalidad; **no equivale a guardado completo de campaña**.

**Próximos sistemas del prototipo — propuestas o pendientes:** combate básico del jugador y una clase funcional de la compañera, un animal/grupo/enemigo grande para probar prioridades, detección real de peligro, convivencia en hogar, salida en solitario, secuestro guionizado y checkpoints.

Las pruebas F2/F3A y locomoción acreditan el alcance ensayado, no la finalización del juego, todos los dispositivos o el carácter definitivo de los modelos y animaciones.

## 14. Arquitectura de Unity — estado actual y dirección

**Unity 6000.6.7f1, C#, URP.** La implementación distingue al menos:

- `CompanionController` y `CompanionState`: estados vitales, seguimiento, órdenes y hechos actuales.
- `CompanionAppearance`, `VrmAppearance` y `AvatarLocomotion`: representación/carga de avatar, locomoción y capacidades visuales; desacoplados del significado narrativo.
- `CompanionProfileStore`: persistencia y recuperación versionadas para apariencia y plantilla, con respaldo.
- `DialogueContent`, `DialogueDirector`, `DialoguePresenter` y `CompanionReactionPresenter`: datos locales, elección de respuesta, UI y salida emocional.

**Regla arquitectónica:** eventos de juego → estado narrativo/contexto → decisión e intención → diálogo/gesto/expresión. Ni la apariencia ni la UI escriben por sí mismas memoria, relación o confianza.

El perfil profundo de personaje será **dato de diseño/autoría** y podrá alimentar perfiles configurables. No conectar SillyTavern, LoreBary, JanitorAI ni un LLM al runtime; no añadir managers duplicados por arquetipo. El motor debe ser ampliable a una cuarta plantilla sin codificar reglas narrativas por cada modelo 3D.

Sistemas futuros mencionados en documentos anteriores (IA de combate, necesidades, memoria persistente, hábitos, navegación táctica, protección, secuestro y hogar) son candidatos de diseño, no componentes implementados por aparecer aquí. Mantener escenas, GUIDs, pruebas, licencias, organización responsiva y límites de compatibilidad.

## 15. Herramientas y producción

- Unity 6 y C# para el juego.
- Blender + Contour3D para geometría editable, rig y deformaciones.
- Build 3D Game Rooms para escenarios cuando corresponda.
- Game Development Studio para validación, preparación y organización de recursos, sujeto a disponibilidad de herramientas locales.
- Game Studio está especializado en navegadores, por lo que no reemplaza el desarrollo nativo en Unity.
- Cualquier servicio externo de pago requiere autorización específica previa; no es una dependencia del juego publicado.

## 16. Decisiones y validaciones pendientes

- **Próxima revisión editorial:** aceptar o ajustar las 33 correcciones del PR #11, validar biblias individuales y criterios de diferenciación observable N1/N2. La actuación corporal y la voz no se certifican por contar frases distintas.
- **Deredere:** decidir si pasa de cuarta plantilla propuesta a alcance aprobado; definir política de ampliación y migración antes de modificar UI o enum.
- **Dandere:** mostrar ese nombre al jugador preservando compatibilidad con el identificador histórico `Shy`; implementación del cambio de etiqueta aún pendiente de verificación.
- **Arte y locomoción:** aprobación artística de pisadas, materiales, gestos, expresiones, escala y otros VRM en Windows; revisar casos extremos y terrenos.
- **F2/F3A:** terminar criterios manuales y pruebas que siguen PARTIAL; no concluir soporte universal por el éxito de un fixture.
- **Gameplay:** combate, una clase funcional, percepción/peligro, convivencia, salidas independientes, secuestros, checkpoint y guardado completo.
- **Narrativa:** causa del viaje isekai, identidad/motivación de bandidos y objetivos compartidos; promesas y relaciones persistentes con consentimiento recíproco.
- **Producción:** plataformas, presupuesto de rendimiento, interfaz accesible/responsiva, licencias del catálogo y pruebas de duración.

---

## 17. Estados de decisión y resultados de prueba — 29/10/2026

| Elemento | Estado | Fundamento / frontera |
|---|---|---|
| Isekai medieval, rescate de trampa de bandidos, amistad gradual, romance opcional, compañera autónoma | **Acordado (visión)** | GDD v0.5; mecánicas de campaña aún pendientes |
| VRM 1.0 local, catálogo y separación de apariencia e identidad | **Acordado (DEC-007)** | PR #3/#4 y F2; Woman/G2 pausados |
| Creador VRM y perfil versionado | **Implementado en prototipo** | PR #7; requiere aceptación manual más amplia |
| Conversaciones locales F3A | **Implementado en prototipo** | PR #9/#10; 60 entradas / 18 rutas; profundidad pendiente |
| Locomoción/renderizado revisados | **Mejora observada manualmente** | PR #12; contacto de pies/arte final aún parciales |
| Tres arquetipos y cambio de etiqueta a Dandere | **Nombre acordado / migración pendiente** | Mantener valor `Shy`; no inventar cuarta opción en build |
| Tarjetas de personaje profundas como método editorial | **Dirección metodológica** | Adaptar capas de SillyTavern; no copiar biografías ni crear dependencia runtime |
| Parche de 33 diálogos y perfiles gestuales | **Propuesta sin integrar** | PR #11; revisión editorial antes de aplicar |
| Deredere como cuarto arquetipo | **Propuesta pendiente de autorización** | Referencias de calidez/entusiasmo, no funcionalidad implementada |
| Memoria persistente, iniciativa compleja, promesas, combate, hogar, secuestro | **Visión / futura especificación** | No declarar implementado |

**Lectura de las pruebas:** el usuario informó que las conversaciones, incapacitación/reanimación y la continuidad de movimiento funcionan, y que los modelos se ven mejor tras la corrección; las tres personalidades aún se sienten similares. Los tests automatizados documentados en los PR no sustituyen esta revisión subjetiva. No hay aprobación de arte final ni del sistema F3 completo.

## 18. Método de autoría inspirado en tarjetas de personaje

Para redactar y revisar una personalidad original:

1. **Ficha permanente:** núcleo de carácter, valores, defecto, deseo, miedo, contradicción, límites, voz, hábitos y objetivos.
2. **Estado situacional:** hecho conocido, emoción, relación, seguridad actual y opciones del jugador realmente elegidas.
3. **Intervención:** intención y prioridad que le corresponde, incluso si decide callar o disentir.
4. **Presentación:** texto localizado, pausa/ritmo, gesto y expresión disponibles, con fallback seguro.
5. **Prueba contrastada:** misma escena, avatar y selección con tres plantillas; evaluación ciega del texto y del lenguaje corporal por separado.
6. **Continuidad:** un ejemplo de tarjeta **no** representa memoria ocurrida; no escribir decisiones del jugador, no inventar hechos o vínculos y no copiar personajes existentes.

Referencias de estudio por arquetipo (no contenido para reproducir): Taiga Aisaka/Rin Tohsaka para tsundere; Yuno Gasai para yandere; Hitori Gotoh/Shoko Komi/Hinata Hyuga para dandere; Marin Kitagawa/Ochaco Uraraka para deredere propuesto. Las obras y las tarjetas de terceros no se incorporan a la biblioteca sin licencia.

## 19. Trazabilidad y control de versiones

- Documento histórico aprobado: [GDD v0.5](GameDesignDocument_v0.5.md).
- Borrador histórico/enmiendas previas: [GDD v0.6](GameDesignDocument_v0.6_borrador.md).
- [Decisiones DEC-001 a DEC-007](DECISIONS.md); DEC-007 prevalece sobre el editor corporal de DEC-001–005 para las siguientes iteraciones.
- [Contrato F2](Specifications/VRM_Fase2_Contrato_Funcional_v0.1.md); [F3 borrador](Specifications/VRM_Fase3_Personalidad_Contextual_v0.1_BORRADOR.md).
- [Biblioteca F3A](Specifications/F3A_Biblioteca_v0.1.json), [diferenciación propuesta](Specifications/F3A_Diferenciacion_Personalidades_v0.1_PROPUESTA.md) y [parche editorial](Specifications/F3A_Diferenciacion_Editorial_v0.1_PROPUESTA.json).
- [Locomoción/render: auditoría](Audits/Companion-Locomotion-Render/INFORME.md); PR #11 y PR #12 documentan los cambios recientes.
- Trabajo de Chat 2: GDD/contratos/editorial; Chat 3: Unity y pruebas; Chat 1: dirección y revisión. Cambios de estado de propuesta a acordado requieren decisión explícita y revisión de PR.
- Cada cambio debe registrarse también en el `readme.txt` raíz, con commits, push, PR y pruebas. El GDD maestro no sustituye los informes técnicos.

**Registro v0.7 (borrador):** consolidación del GDD v0.5 y las enmiendas posteriores con la dirección VRM de DEC-007, el estado F2/F3A y la corrección de locomoción/render, nombre Dandere, metodología de perfiles profundos, referencias de estudio y distinción explícita entre funciones probadas y propuestas. **No se aprueba automáticamente Deredere, las 33 revisiones editoriales ni nuevas mecánicas.**

