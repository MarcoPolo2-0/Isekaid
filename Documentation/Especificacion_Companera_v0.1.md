# Isekaid — Especificación de la compañera y aceptación del prototipo

**Versión:** 0.1 · **Fecha:** 27/10/2026 · **Estado:** PROPUESTA.

**Última actualización:** 28/10/2026 — trazabilidad de DEC-001 e ISEK-001.

**Revisión administrativa histórica:** 1 — adopción del [Workflow de desarrollo v1.0](Workflow_Desarrollo_v1.0.md).  
**Ubicación y procedimiento vigentes:** [Workflow v1.1](Workflow_Desarrollo_v1.1.md), DEC-006 (sin alterar requisitos creativos de esta propuesta).  
**Responsable:** Chat 2 — Game Design.  
**Revisión y aprobación:** Chat 1 — Dirección del proyecto (Cerebro).  
**Tareas vinculadas:** [ISEK-001](Specifications/ISEK-001.md) concreta la base corporal y visual conforme a DEC-001. Los sistemas sociales y de autonomía conservan su alcance propuesto y requieren asignación de Dirección.  
**Inicio de desarrollo:** no autorizado por esta especificación; requiere estado APROBADA para una tarea, versión y alcance identificados.

**Base:** [GDD v0.5](GameDesignDocument_v0.5.md), [revisión v0.6](GameDesignDocument_v0.6_borrador.md) y prioridades solicitadas el 27/10/2026. Este documento define resultados; no modifica código ni acredita una implementación.

**Actualización de trazabilidad, 28/10/2026:** [DEC-001](DECISIONS.md) selecciona el modelo **Woman de Fly_jkey**, disponible como `assets/woman.fbx`, como candidato principal. Establece una apariencia neutral equilibrada, ajustes independientes de pecho y caderas y cintura condicionada a viabilidad técnica. [ISEK-001 v0.1](Specifications/ISEK-001.md) especifica esta tarea; sigue en PROPUESTA. La selección del candidato no aprueba el resto de este documento. Se conserva como referencia histórica la matriz original de dos ejes; la especificación de la tarea amplía la prueba a tres ejes solo si cintura se habilita.

## 1. Propósito y alcance

La siguiente mejora debe demostrar que la compañera tiene una identidad reconocible: el jugador puede personalizarla, reconocer sus emociones, distinguir su personalidad por lo que hace y comprobar que las experiencias compartidas afectan sus respuestas. La apariencia, las animaciones, la conversación y las decisiones deben describir al mismo personaje.

El alcance inmediato comprende modelo, personalización, rostro, lenguaje corporal, conversación contextual, emoción, memoria y autonomía de exploración. Las tres clases, combate completo, misiones personales, hogar, secuestros y amistad/romance permanecen en la visión completa. Su entrega posterior no autoriza eliminarlos ni sustituirlos permanentemente por botones de prueba.

**Cómo leer los estados:**

- **A — Aprobado:** requisito confirmado en la solicitud actual o decisión explícita de la v0.5. Su alcance se conserva.
- **D — Derivado:** criterio de verificación necesario para demostrar un requisito A; no constituye una mecánica nueva ni un resultado ya conseguido.
- **P — Propuesto:** comportamiento, umbral, contenido o secuencia de entrega que aún requiere revisión. No debe presentarse como una decisión aprobada.
- **V — Verificado:** reservado para una prueba ejecutada con evidencia identificable. Ningún caso de aceptación de este documento tiene todavía este estado.

Estas marcas califican requisitos y evidencia; no sustituyen los estados formales del workflow. Que un requisito esté marcado A no aprueba automáticamente esta especificación completa ni autoriza su implementación. Dirección determina el siguiente trabajo y la aceptación de las entregas. La secuencia H1/H2/H3 sigue siendo una propuesta. Esta revisión administrativa conserva íntegros los criterios y comportamientos de la versión 0.1.

Los datos descritos son conceptos de diseño. Los nombres son identificadores documentales locales, no APIs ni clases obligatorias. Desarrollo decide su representación reutilizando la estructura existente.

## 2. Referencias y condiciones de evaluación

### Referencias visuales

| Referencia | Estado | Qué aporta | Qué no acredita |
|---|---|---|---|
| REF-01: [lámina de compañera v01](references/companion-concept-v01.png) | P, candidata | Cabello largo, top y pantalón, silueta adulta, cinco expresiones; contraste normal/cómico | Aprobación del rostro, paleta, proporciones finales, volumen real, topología, rig o fidelidad en Unity |
| REF-02: referencias anime mencionadas en la v0.5 | Pendiente de recuperar y asociar a archivos concretos | Dirección del contraste entre rostro normal y comedia | No se dispone en este paquete de las imágenes originales; no puede certificarse una coincidencia exacta con ellas |
| REF-03: capturas y clips del personaje en Unity | Pendiente de producción | Calidad real del modelo y continuidad entre creador y aventura | Una imagen de concepto o un render externo no sustituye esta evidencia |

REF-01 no fija una personalidad por color ni asigna nombre, origen, gustos o clase. Cabello castaño, ojos verdes, top marfil y pantalón oscuro siguen siendo opciones provisionales.

### Protocolo común propuesto

Evaluar una misma versión del personaje en el creador y en la aventura. Capturar frontal, perfil, tres cuartos y espalda; cuerpo completo y primer plano facial; luz neutral, sol lateral y sombra del escenario. Registrar resolución, escala de interfaz, cámara, iluminación, versión de Unity, equipo y parámetros de personalización.

Como banco inicial **P**, usar 1920×1080 y 1280×720, sin recortar manos ni pies; distancias equivalentes a aproximadamente 1, 2 y 5 metros con escala real de Unity y la cámara de juego. No son plataformas objetivo aprobadas. El primer plano verifica rostro; el plano medio, interacción; el distante, silueta y locomoción.

La plataforma de lanzamiento, equipo mínimo, tasa de imágenes objetivo y presupuestos de polígonos/texturas siguen pendientes. Desarrollo debe medir CPU, GPU y memoria en el equipo declarado y proponer presupuestos antes de cerrar la aceptación de rendimiento. No se trasladan automáticamente los requisitos Android de otros proyectos.

Una presentación se rechaza cuando solo muestra una pose favorable, oculta extremos de personalización o reemplaza la evidencia de juego por renders externos. La aceptación artística exige revisión humana explícita; compilar no la concede.

## 3. Modelo, materiales y presentación — VIS

### Qué debe hacer

**A:** representar una mujer adulta de estética anime 3D con proporciones iniciales promedio, cabello largo suelto, rostro neutro, top y pantalón sencillos. Compartir modelo entre personalidades y admitir ropa deformable, rig humanoide y ajustes corporales.

**D:** mantener su identidad desde todos los ángulos, al hablar y al moverse. El rostro debe leerse como anime mediante ojos, cejas, nariz, boca y sombreado coherentes. Los materiales del creador y de la aventura deben conservar el diseño bajo sus respectivas luces.

**P:** usar volúmenes limpios, sombras de dos o tres niveles y contorno discreto donde ayude a leer la silueta. Los límites exactos del contorno y del sombreado se deciden comparando capturas reales. No es obligatorio copiar una tecnología de shader concreta.

### Datos y entregables necesarios

| Dato o recurso | Uso | Condición de entrega |
|---|---|---|
| Modelo fuente editable y exportación | Corregir geometría, rig y deformaciones | Fuente accesible, versión y correspondencia con la exportación integrada |
| Identidad visual y silueta de referencia | Comparar cara, cabello, cuerpo y ropa | Referencia aprobada y versión; mientras no exista, revisión provisional |
| Rig, pesos y límites de deformación | Locomoción, gestos, rostro y ajustes | Mapa de huesos, poses de revisión y extremos soportados |
| Materiales, texturas, máscaras y UV | Separar piel, cabello, iris, top y pantalón | Sin texturas faltantes; tintes independientes y coherentes con las luces |
| Animaciones compatibles | Dar presencia y comunicar intención | Reposo, caminar, correr, giro y gestos del hito; lista de pendientes explícita |
| Procedencia y condiciones de uso | Poder distribuir el recurso | Registro del origen y condiciones aplicables si procede de un paquete externo |

Los archivos ZIP de personajes que existen en la carpeta de assets son candidatos sin evaluación en esta revisión. No se consideran seleccionados, integrados ni validados por su nombre.

### Comportamiento ante situaciones

| Situación | Resultado esperado |
|---|---|
| Girar 360° en el creador | Volumen consistente; sin agujeros, normales invertidas ni mechones que desaparezcan de forma imprevista |
| Pasar a la aventura | Mismo modelo, ajustes y materiales; la iluminación puede cambiar su apariencia luminosa, no sus datos |
| Caminar, correr, detenerse y girar | Pies coherentes con el suelo, cuerpo sin deslizarse continuamente, cabello y ropa sin atravesar repetidamente el torso |
| Levantar brazos, agacharse y sentarse | Hombros, codos, cadera y rodillas conservan volumen; top y pantalón acompañan el cuerpo |
| Incapacitación | Postura compatible con el suelo y lectura clara de que necesita ayuda; no basta inclinar todo el modelo rígidamente |

### Aceptación

- **VIS-01 (D):** el personaje integrado es un modelo deformable con rig; no un conjunto de primitivas presentado como asset final.
- **VIS-02 (D):** revisión comparada de rostro, cabello, silueta y ropa en cuatro ángulos y tres iluminaciones. Cualquier diferencia con la referencia se documenta y requiere aceptación artística.
- **VIS-03 (D):** reproducir la batería de poses y locomoción sin agujeros, penetraciones persistentes ni deformaciones que alteren la identidad. Registrar los defectos por pose y ajuste, no ocultarlos con la cámara.
- **VIS-04 (D):** entregar fuente editable, exportación, materiales y evidencia de su integración real. Un archivo 3D descargado sin comprobación en Unity no cierra el criterio.

## 4. Personalización y continuidad del perfil — PER

### Qué debe hacer

**A:** permitir modificar por separado cabello, ojos, top y pantalón; ajustar pecho y caderas; mantener independencia entre apariencia, personalidad y clase. El cuerpo base continúa siendo adulto y de proporciones promedio.

**D:** cada modificación corporal debe verse en el cuerpo y en la ropa; mover solo la escala del objeto completo no cumple. Los controles deben funcionar en el creador y conservar su resultado al entrar en la aventura y volver a abrir el perfil.

**P:** dos controles continuos independientes, pecho y caderas, normalizados entre 0 y 1 con base en 0.5. Sus extremos representan formas modeladas y revisadas, no centímetros ni multiplicadores arbitrarios. La forma de los extremos requiere aprobación visual antes de fijar el rango de producción.

**P:** conservar las paletas existentes como presets iniciales. No son el límite definitivo de personalización. Un selector libre de color, más peinados, altura y otros rasgos no se incluyen automáticamente en este hito.

### Datos necesarios

| Grupo | Datos mínimos | Persistencia y reglas |
|---|---|---|
| Identidad del perfil | Identificador estable, versión del esquema y versión del recurso | Persistente; distingue el personaje de una sesión de prueba |
| Apariencia | Selecciones de los cuatro colores, valores de pecho y caderas | Persistente; validación contra los rangos que admite el asset |
| Personalidad | Perfil elegido, separado de apariencia | Persistente; cambiarlo en una prueba no cambia colores ni dimensiones |
| Clase | Clase elegida cuando se habilite su elección | Independiente; momento de selección aún pendiente |
| Edición | Valores al abrir, borrador y estado de guardado | Temporal; no sobrescribir el perfil confirmado por una previsualización |

**P:** aplicar los cambios en vista previa inmediatamente; “Guardar y comenzar” confirma el perfil solo si guardar funciona. “Restablecer apariencia” modifica el borrador y no elimina relación, recuerdos ni progreso. Salir con cambios pendientes ofrece guardarlos o descartarlos. El acceso al creador durante una partida deberá preservar su identidad y progreso; reiniciar el encuentro es una limitación actual, no el comportamiento final acordado.

**D:** los perfiles v1 existentes con cuatro índices de color y personalidad deben tener una migración explícita al introducir ajustes corporales. **P:** rellenar los campos ausentes con los valores base sin cambiar la selección original. Ante una versión futura desconocida o un archivo corrupto, preservar los datos existentes y explicar el fallo antes de reemplazarlos. No convertir una recuperación silenciosa a valores por defecto en pérdida irreversible.

### Aceptación

- **PER-01 (D):** cambiar cada color afecta solo a su zona. El iris conserva pupila, reflejos y lectura; piel y ropa adyacente no reciben el tinte.
- **PER-02 (D/P):** probar las nueve combinaciones de mínimo/base/máximo de pecho y caderas, más barrido continuo; repetir poses críticas y locomoción. Cero penetraciones persistentes visibles entre cuerpo y ropa en las vistas de prueba. Si un extremo falla, corregir el asset o proponer un cambio de rango; no recortarlo silenciosamente.
- **PER-03 (D):** mismo perfil antes y después de guardar, cerrar la aplicación, abrirla, entrar en la aventura y regresar al creador. La comprobación de datos debe acompañarse de comparación visual.
- **PER-04 (D/P):** probar perfil antiguo válido, datos inválidos, versión desconocida y fallo de escritura. No perder una selección válida ni presentar un guardado fallido como éxito.
- **PER-05 (D):** repetir la misma apariencia con las tres personalidades y, cuando existan, las tres clases. No cambian cuerpo, colores, identidad ni datos de relación como efecto lateral.

## 5. Sistema facial híbrido — FAC

### Qué debe hacer

**A:** combinar expresiones normales anime y expresiones cómicas exageradas conservando el mismo personaje. La cara debe responder a la conversación, a la emoción y al peligro.

La v0.5 propone controles faciales y texturas/atlas. Se conserva esa dirección; el reparto concreto entre geometría, materiales y recursos 2D se valida con el modelo. Este documento no obliga a crear un componente por cada canal.

### Datos necesarios

| Grupo | Contenido |
|---|---|
| Catálogo de expresiones | Identificador, familia normal/cómica, intensidad, ojos, cejas, boca y sonrojo |
| Activación | Intención o emoción causante, situación permitida, prioridad y condiciones de cancelación |
| Transición | Entrada, duración máxima, salida y pose facial de retorno |
| Compatibilidad | Parpadeo, mirada, habla, máscaras y recursos que se excluyen mutuamente |
| Estado actual | Expresión activa, causa vigente y estados de cara que deben restaurarse |

### Catálogo inicial propuesto

| Tipo | Expresión | Uso y lectura |
|---|---|---|
| Normal | Neutral atenta | Reposo; disponible sin sonrisa permanente |
| Normal | Alegría/alivio | Seguridad o resultado positivo; sonrisa y mirada más abierta |
| Normal | Vergüenza | Sonrojo moderado, mirada desviada; no implica romance |
| Normal | Molestia | Cejas, tensión de boca y postura acordes con desacuerdo |
| Normal | Miedo/preocupación | Amenaza detectada o recuerdo reciente; sin convertirlo en indefensión |
| Normal | Tristeza/cansancio | Pérdida o esfuerzo; solo con causa válida |
| Cómica | Sorpresa | Ojos y boca simplificados por un hecho ligero e inesperado |
| Cómica | Irritación breve | Signo gráfico y deformación estilizada; no se confunde con alerta real |
| Cómica | Vergüenza exagerada | Reacción social breve en contexto seguro |

### Reglas situacionales

**D:** peligro, dolor o incapacitación interrumpen una reacción cómica. Los signos gráficos no ocultan una amenaza ni permanecen al cambiar de escena. Una expresión de miedo necesita una causa válida: amenaza percibida o evento recordado relevante.

**P:** el evento selecciona una intención facial; la personalidad cambia su intensidad y duración, no el hecho ocurrido. En seguridad, terminar la comedia vuelve a la emoción vigente, no necesariamente a neutral. Un recurso de ojos cómicos sustituye al normal mientras dure; no se dibujan simultáneamente dos pares de ojos. El parpadeo y los labios respetan el modo activo.

**P:** transiciones normales de 0.15–0.30 s; golpe cómico de 0.6–1.2 s más salida; cancelar la comedia dentro de 0.2 s al recibirse un evento de peligro. Son valores de ensayo pendientes de ajustar con clips, no animaciones aprobadas.

### Aceptación

- **FAC-01 (D/P):** demostrar el catálogo en vista frontal y tres cuartos, en creador y juego; el revisor reconoce la emoción sin leer la etiqueta. Protocolo propuesto: al menos 4 de 5 reconocimientos correctos por familia con observadores que no vean el nombre; informar tamaño y resultado de la muestra.
- **FAC-02 (D/P):** interrumpir cada expresión cómica con peligro, incapacidad y cambio de escena; recuperar ojos, boca, materiales y expresión apropiada, sin capas residuales.
- **FAC-03 (D):** ejecutar parpadeo, mirada lateral y habla durante expresiones normales y cómicas. Sin ojos dobles, dientes superpuestos ni sonrisas incompatibles con una alerta grave.
- **FAC-04 (D):** expresar la misma emoción con los tres perfiles sin cambiar la geometría base ni asociar un arquetipo a un rostro exclusivo.

## 6. Personalidad y lenguaje corporal — COR

### Qué debe hacer

**A:** tsundere, yandere y tímida son tendencias de personalidad. Deben distinguirse por diálogo, gestos, postura, mirada, iniciativa y reacción, manteniendo identidad, experiencia y capacidad de supervivencia. Una tímida puede ser competente; el primer rescate no produce devoción instantánea en una yandere.

**Datos necesarios:** perfil de tendencias, emoción e intensidad actuales, fase de relación, intención de interacción, atención/mirada, acción de locomoción, gesto compatible y su historial reciente. La clase determina técnica de combate; la personalidad modifica su expresión dentro de condiciones seguras.

### Dirección corporal propuesta

| Situación | Tsundere | Yandere | Tímida |
|---|---|---|---|
| Reposo seguro | Peso asimétrico, mirada al entorno; reserva sin enfado constante | Atención sostenida al interlocutor, postura orientada y relajada | Manos próximas al cuerpo, mirada intermitente; postura estable |
| Agradecimiento inicial | Desvía la mirada y luego reconoce la ayuda | Atiende al gesto recibido, pregunta con interés contenido | Exhala, mira brevemente al interlocutor y agradece |
| Conversación | Gesto breve y respuesta directa; puede suavizarse con confianza | Escucha atenta y recuerda un detalle pertinente; respeta la distancia | Pausa antes de responder y gestos pequeños; menos vacilación al conocer al jugador |
| Desacuerdo | Postura firme y razón concreta | Expresa preocupación por el plan; propone otra opción | Empieza con cautela pero completa una objeción clara |
| Peligro | Interrumpe el gesto social y actúa | Prioriza una decisión protectora viable | Puede alertar y actuar pese al miedo |
| Mayor confianza | Más contacto visual y gratitud directa | Más iniciativa mutua y conversación personal | Más iniciativa y firmeza sin perder su reserva |

Las variaciones anteriores son **P**, no nuevas biografías. Evitar loops de brazos cruzados, manos entrelazadas o mirada fija que se repitan mecánicamente. Ningún arquetipo altera la anatomía ni asigna automáticamente romance, agresión o incompetencia.

### Prioridades y continuidad

Se conserva la prioridad de la v0.5: supervivencia > combate > interacción importante > movimiento > gestos sociales > actividades ambientales. Un gesto no bloquea la salida de una zona peligrosa. **P:** los gestos de torso y cabeza compatibles pueden acompañar caminar; otros esperan a una parada. Recuperar locomoción sin reiniciar el clip desde su primer fotograma repetidamente.

### Aceptación

- **COR-01 (D/P):** misma apariencia, mismo encuentro y mismo texto informativo para tres clips sin etiqueta. Debe observarse una diferencia legible de postura, mirada o gesto; registrar qué rasgos distinguen cada perfil. Como prueba de dirección propuesta, 4 de 5 observadores identifican cada perfil sin depender del color o la voz.
- **COR-02 (D):** interrumpir reposo y gestos con caminar, correr, orden y peligro. Sin deslizamiento persistente, postura congelada ni demora peligrosa causada por la animación social.
- **COR-03 (D/P):** comparar el mismo perfil con confianza inicial y posterior mediante estados de prueba declarados. Mostrar una evolución concreta sin reemplazar su personalidad. El preset demuestra presentación, no progresión ganada en juego.
- **COR-04 (D):** desacuerdo comunicado con motivo comprensible y alternativa; no bloquear una misión por un rechazo aleatorio sin solución.

## 7. Conversación contextual — DIA

### Qué debe hacer

**A:** usar contenido escrito y disponible localmente; seleccionar respuestas por situación, personalidad, emoción, memoria y relación. Permitir iniciativa en seguridad y alertas urgentes durante peligro. Evitar repetición y comentarios fuera de contexto. No depender de una API generativa, conexión ni modelo local en este hito.

### Datos necesarios

| Registro | Contenido mínimo |
|---|---|
| Línea | ID estable, intención, texto, perfil aplicable o compartido, variante y prioridad |
| Elegibilidad | Estado vital, seguridad, interlocutor, hechos conocidos, recuerdos, fase de relación y emoción permitida |
| Presentación | Expresión, gesto, mirada, duración/subtítulo y condiciones de interrupción |
| Antirrepetición | Última reproducción, usos, grupo de variantes y plazo para volver a usarlo |
| Iniciativa pendiente | Motivo, evento de origen, caducidad y condiciones que deben seguir siendo ciertas |
| Confirmación | Marca de reproducción y efecto, si existe, aplicados solo cuando corresponda |

Una frase no demuestra que el hecho ocurrió. El estado del juego habilita el texto. No escribir «recuerdo que me salvaste» si no existe ese recuerdo; no inventar enemigos detectados a partir de una frase de prueba.

### Selección y situaciones propuestas

1. Comprobar si puede conversar: estado activo, interlocutor válido y actividad compatible. Bloquear charla social durante peligro, combate o interacción importante; las alertas siguen su canal de prioridad.
2. Filtrar líneas por hechos y recuerdos válidos antes de elegir tono. Excluir contenido romántico si la ruta no se inició mediante decisión recíproca.
3. Elegir intención pertinente; preferir una variante no oída y compatible con personalidad/emoción. Si no hay ninguna, permitir silencio o una respuesta breve neutral sin inventar información.
4. Asociar expresión y gesto a la intención. No lanzar dos conversaciones al mismo tiempo.
5. Al interrumpir, detener texto/voz/gesto de forma coherente. Revalidar el asunto antes de retomarlo; descartar alertas antiguas y comentarios caducados.

**P:** ensayo inicial con seis intenciones: agradecimiento tras rescate, cautela ante desconocido, curiosidad, reconocimiento de experiencia compartida, desacuerdo y propuesta de actividad segura. Dos variantes por intención y personalidad: 36 líneas sociales. Añadir confirmaciones de seguir/esperar y avisos de ruta bloqueada/peligro como contenido funcional separado. La cantidad es un paquete de ensayo, no una definición de conversación profunda.

**P:** máximo una iniciativa social cada 45–90 s de tiempo activo, sujeta a elegibilidad; silencio mientras se responde. No repetir la misma línea entre las últimas tres de su grupo; esperar al menos 120 s para repetir una línea. Si hay pocas variantes, permitir silencio antes que violar la regla. La conversación solicitada por el jugador no espera el cooldown de iniciativas, pero respeta contexto y evita repetir una respuesta larga.

### Ejemplo propuesto: recuerdo del rescate

Contexto: trampa liberada, lugar seguro, relación inicial; propósito compartido aún temporal.

| Perfil | Texto de ejemplo | Presentación |
|---|---|---|
| Tsundere | «Viste el mecanismo antes que yo. Esta vez me ayudaste tú; ahora vigilaré el camino.» | Mirada lateral al inicio, reconocimiento breve y orientación al sendero |
| Yandere | «Te detuviste cuando me escuchaste. Quiero saber quién eres, pero primero salgamos de aquí.» | Atención directa, alivio contenido y cautela hacia el entorno |
| Tímida | «Pensé que nadie me oiría. Gracias. Si también vas a la aldea, podemos caminar juntos.» | Exhalación y contacto visual breve; iniciativa clara pese a la reserva |

Son líneas de prueba, no diálogo final aprobado ni obligación de fijar la aldea como destino definitivo. Con desconocido, curiosidad y gratitud no equivalen a romance.

### Aceptación

- **DIA-01 (D):** misma situación con las tres personalidades produce información compatible y variación expresiva. Una emoción o memoria diferente cambia la selección cuando corresponde.
- **DIA-02 (D):** iniciar peligro en mitad de una charla: cesa la interacción social, se permite una alerta fundada y no se reproduce después una advertencia vencida.
- **DIA-03 (D/P):** sesión de diez minutos con solicitudes repetidas y pausas. Registrar líneas, contexto y tiempos; cumplir reglas de repetición, mostrar silencios válidos y no aumentar confianza por pulsar hablar.
- **DIA-04 (D):** ejecución sin conexión de la secuencia completa; todos los textos y recursos requeridos están disponibles y no aparece una dependencia de servicio de generación.
- **DIA-05 (D):** recuerdo ausente, relación inicial y romance no iniciado excluyen sus líneas respectivas. Un evento percibido falso no permite una alerta verdadera.

## 8. Emoción, memoria, preferencias y desarrollo — MEM

### Qué debe hacer

**A:** distinguir rasgos estables, emoción temporal, relación, recuerdos y aprendizaje. Los acontecimientos deben tener consecuencias observables y coherentes; una barra de confianza o una frase repetida no cumplen por sí solas. Amistad desarrollada y romance opcional son rutas distintas.

### Datos y duración

| Capa | Datos mínimos | Persistencia propuesta |
|---|---|---|
| Personalidad | Perfil y tendencias | Persistente; no se recalcula por cada diálogo |
| Emoción | Tipo, intensidad, causa, instante en tiempo de partida y condición de recuperación | Temporal, conservable al guardar una situación; no decae por tiempo real con el juego cerrado |
| Relación | Confianza, cercanía, respeto, compromisos y estado de ruta | Persistente; dimensiones distinguibles, sin fórmula de puntuación aprobada aún |
| Recuerdo | Tipo e ID de instancia del evento, participantes, lugar/contexto, desenlace, relevancia y efectos ya aplicados | Persistente en el alcance de guardado habilitado |
| Preferencia | Actividad/objeto, afinidad y origen: rasgo inicial o experiencia | Persistente; no deducir un gusto del color del cabello o del arquetipo |
| Hábito | Acción contextual y oportunidades recientes válidas | Persistente solo cuando tenga sentido; nunca ejecutar si hay peligro |
| Aprendizaje | Situación practicada, resultado y cambio conductual permitido | Persistente; progreso de destreza independiente de romance y reserva social |

No es necesario almacenar una novela ni simular todo en cada fotograma. Los recuerdos son hechos estructurados, y las frases se escriben para los casos relevantes.

### Reglas propuestas de causalidad

| Acontecimiento | Efecto inmediato | Efecto posterior comprobable |
|---|---|---|
| Primer rescate | Alivio mezclado con cautela; registro único | Reconoce la ayuda en contexto seguro, sin amor instantáneo |
| Amenaza percibida | Aumenta miedo/alerta; interrumpe charla | Recuperación gradual después de comprobar seguridad, sin perder memoria del episodio |
| Ayuda recíproca válida | Gratitud y antecedente de confianza | Otra línea elegible o mayor disposición a proponer una actividad |
| Promesa incumplida | Molestia con motivo identificable | Conversación sobre el hecho y oportunidad concreta de reparación |
| Actividad elegida que le gusta | Alegría contextual | Iniciativa ocasional relacionada con esa preferencia |
| Resolver varias veces una dificultad | Mayor familiaridad | Una acción de reconocimiento o apoyo más oportuna, manteniendo personalidad |

Los últimos cuatro ejemplos son **P** para la prueba social, no contenido ya existente. Propuesta inicial: definir una actividad de descanso/observación como preferencia de prueba, sin fijarla como gusto canónico.

**D:** aplicar un evento y sus efectos una sola vez en la misma continuidad de partida. Cargar, reentrar en escena o repetir el callback no vuelve a premiar el mismo hecho. Eventos distintos del mismo tipo pueden existir, pero no usan el primer rescate como una fuente infinita de confianza.

**P:** conservar un ID de instancia del evento y el registro de efectos aplicados junto al estado guardado. Al volver a un checkpoint anterior al evento, restaurar también relación y recuerdos anteriores: el rescate puede ocurrir de nuevo en esa línea temporal, pero no acumula beneficios de la línea descartada. Una nueva partida usa una continuidad nueva.

**P:** con varias emociones, priorizar peligro actual y relevancia de la causa; evitar oscilaciones entre feliz y aterrada en cada actualización. Tras una amenaza, probar 10–30 s de recuperación en tiempo activo cuando ya es segura. El valor final requiere pruebas. No descontar amistad por cerrar el juego.

### Aceptación

- **MEM-01 (D):** disparar dos veces el mismo rescate produce un único recuerdo y un único efecto de relación; un nuevo evento legítimo puede registrarse por separado.
- **MEM-02 (D/P):** guardar/cargar conserva el hecho y su efecto sin duplicarlos; restaurar un punto anterior revierte ambos de forma coherente. Mientras no exista guardado de aventura, usar una prueba de estado aislada y declarar que la persistencia jugable sigue pendiente.
- **MEM-03 (D/P):** mostrar la misma situación antes y después de un hecho válido: cambia una respuesta y una conducta observable. Un preset permite probar presentación, pero el cierre exige una secuencia que realmente produzca el hecho.
- **MEM-04 (D):** pausar o cerrar no avanza procesos basados en tiempo activo ni genera desatención ficticia. Las pruebas deben distinguir emoción, confianza y temporizador vital.
- **MEM-05 (D/P):** mostrar un gusto y una iniciativa contextual coherentes; rechazar la propuesta sin penalización automática por un único rechazo. La consecuencia exacta queda pendiente de balance.
- **MEM-06 (D):** alcanzar alta amistad no activa romance, celos posesivos ni contenido exclusivo de pareja sin la elección correspondiente. La amistad conserva iniciativas y contenido propio.

## 9. Autonomía, seguimiento y órdenes — AUT

### Qué debe hacer

**A:** acompañar con intención, evitar obstáculos, mantener una distancia útil, advertir de amenazas realmente percibidas y tomar iniciativas comprensibles. Las decisiones consideran peligro, salud, distancia, órdenes, personalidad, clase y experiencia. La supervivencia prevalece sobre agradar o sostener una pose.

El conjunto definitivo de órdenes continúa abierto en la v0.5. “Sígueme” y “Espera aquí” ya existen en el prototipo y deben conservarse mientras se mejora su ejecución. “Apóyame” y “Retírate” quedan vinculadas a la especificación de combate posterior.

### Datos necesarios

Estado vital; orden vigente; posición del jugador y último lugar conocido; destino autorizado; ruta completa/parcial/inválida; distancia; velocidad; bloqueo; amenazas percibidas y vigencia; seguridad de la posición; interacción actual; acción autónoma elegible; emoción y experiencia pertinentes. La percepción limita la información disponible: no conoce automáticamente lo que ocurre detrás de cualquier pared.

### Situaciones y respuesta propuesta

| Situación | Decisión | Comunicación y recuperación |
|---|---|---|
| Seguir en espacio abierto | Elegir posición próxima y lateral/trasera; caminar o correr según separación | No empujar al jugador ni ocupar de forma persistente el centro de cámara |
| Jugador se detiene | Reducir velocidad y reposar a distancia útil | Orientación natural; no alternar arrancar/frenar continuamente |
| Pasillo estrecho o puerta | Ceder paso y buscar destino navegable | No bloquear el único acceso ni avanzar a través de paredes |
| Esperar en zona segura | Conservar la orden; observar o hacer un gesto local | No reanudar seguimiento por una conversación o cambio de expresión |
| Peligro mientras espera | Priorizar supervivencia con una acción viable | Aviso breve; al estar a salvo, pedir o confirmar cómo continuar. No desaparecer ni abandonar silenciosamente |
| Ruta bloqueada o destino inalcanzable | Detenerse en posición segura y reintentar con límite | Aviso de bloqueo una vez; no penalizar vínculo por error de navegación |
| Jugador sale de zona navegable | Usar último punto alcanzable, sin cruzar geometría | Permitir regresar y recuperar el seguimiento; no teletransporte invisible durante la prueba |
| Parada prolongada y segura | Observar, ajustar postura o iniciar una charla elegible | Respetar cooldown, orden y prioridad de interacción; no inventar una necesidad urgente |
| Incapacitación | Cancelar órdenes y conversación social | Pedir ayuda; la trampa inicial mantiene su estado separado del temporizador mortal |

**P, parámetros iniciales de ensayo:** distancia deseada 1.8–2.8 m; detener seguimiento al entrar a 2 m y reanudar al superar 3 m, con espacio suficiente; considerar bloqueo tras 3 s sin progreso y avisar como máximo una vez cada 10 s mientras persista. En lugares estrechos la seguridad y la ruta válida prevalecen sobre la distancia. No son medidas aprobadas ni universales: se ajustan con escala, velocidad y cámara reales.

**P:** autonomía de exploración limitada a acciones cercanas y reversibles: mirar, observar un elemento, comprobar el camino o proponer una pausa. No recoger recursos del jugador, iniciar otra misión ni cruzar una transición de escena sin una regla de diseño específica. En el primer encuentro, la propuesta de continuar conserva cautela y motivo mutuo.

### Aceptación

- **AUT-01 (D/P):** circuito repetible con recta, giro, obstáculo, pasillo y parada; recorrerlo tres veces a pie y corriendo. Sin atravesar colisiones, empujar, atascar la puerta ni alternar destinos de forma continua.
- **AUT-02 (D):** seguir → esperar → hablar → seguir conserva orden y estado. Repetir la orden no duplica diálogos ni inicia varias acciones simultáneas.
- **AUT-03 (D/P):** bloquear ruta y sacar al jugador del área navegable; se comunica la limitación, no se castiga el vínculo y el seguimiento se recupera al volver a una situación válida. Registrar tiempos y posiciones.
- **AUT-04 (D):** una amenaza detectada cancela una acción social y permite alerta; una amenaza no percibida no produce conocimiento omnisciente. Una señal inyectada solo valida la respuesta al evento, no el sistema de percepción.
- **AUT-05 (D/P):** durante una parada segura aparece una iniciativa coherente con preferencia/emoción; no aparece durante combate, incapacitación ni interacción importante.
- **AUT-06 (D):** el control de incapacitación de prueba y el rescate inicial siguen separados. El plazo de 30 s no se activa al descubrir la trampa. Preservar la regresión de supervivencia existente.

## 10. Datos compartidos y resolución de conflictos

**P:** una sola secuencia de decisión coherente: contexto válido → intención → texto/gesto/rostro compatibles → confirmación de efectos. El rostro y el diálogo no cambian hechos de negocio por sí mismos. Los presets de ensayo se identifican para no confundirlos con recuerdos ganados.

| Conflicto | Resultado necesario |
|---|---|
| Sonrisa o comedia + amenaza grave | Prioridad de amenaza; presentación apropiada y cancelación del gesto incompatible |
| Orden de esperar + peligro inmediato | Acción de supervivencia comprensible; conservar información de la orden para resolverla después |
| Recuerdo válido + lugar inseguro | Conservar recuerdo, posponer conversación social y revalidarla luego |
| Cambio de apariencia + partida existente | No eliminar identidad, recuerdos, preferencias, relación ni misión |
| Cambio de escena + acción pendiente | Limpiar presentación y conservar únicamente datos válidos; no ejecutar callbacks sobre una escena destruida |
| Registro de rescate + guardado/carga | Un hecho y un efecto dentro de la continuidad restaurada |

## 11. Hitos propuestos y demostración integrada

### H1 — Presencia visual y personalización

Incluye VIS, PER, FAC y la primera batería COR. Entrega modelo real, fuente editable, rig, materiales, locomoción, ajustes corporales y rostro híbrido demostrables en ambas escenas. La cara no se considera final hasta revisar la referencia pendiente. No se cierra si solo hay un render, un modelo sin rig, sliders sin deformación o una expresión mostrada únicamente en un inspector.

### H2 — Identidad social observable

Incluye DIA, MEM y autonomía segura de AUT. Entrega contenido local, emoción causal, registro de eventos, variaciones corporales, iniciativa y recuperación de ruta. La persistencia de memoria se prueba en el alcance declarado; si aún no existe guardado de aventura, se mantiene esa aceptación abierta. No se necesita construir el mundo completo para probarla.

### H3 — Cooperación bajo peligro real

Añade una clase funcional por elegir, percepción real, enemigos y rescate derivado de combate, con checkpoint coherente. Reutiliza H1/H2 y conserva futuras clases, secuestro y convivencia. La señal de amenaza del banco de pruebas no sustituye este hito.

### Secuencia de aceptación transversal

| Paso | Acción | Evidencia esperada |
|---|---|---|
| 1 | Crear y guardar una apariencia; reiniciar la aplicación | Ajustes y cuerpo visibles conservados |
| 2 | Iniciar aventura y encontrarla atrapada | Reconocer el mismo personaje y una desconocida cautelosa, sin temporizador mortal |
| 3 | Rescatarla e interactuar | Un hecho registrado, alivio, línea y gesto compatibles con el perfil |
| 4 | Caminar, detenerse y ordenar esperar | Locomoción, distancia útil y continuidad de la orden |
| 5 | Provocar un momento social ligero y luego una amenaza | Comedia pertinente, interrupción y respuesta de supervivencia; identificar si la amenaza fue real o de ensayo |
| 6 | Volver a seguridad y conversar | Recuperación emocional, referencia válida a lo ocurrido y ausencia de repetición inmediata |
| 7 | Recibir y rechazar una iniciativa | Motivo comprensible y reacción acorde, sin romance ni castigo arbitrario |
| 8 | Guardar/cargar el estado habilitado | Memoria y efectos conservados sin duplicación; no ocultar guardado de aventura pendiente |
| 9 | Repetir con las otras personalidades y la misma apariencia | Mismos hechos, tres formas reconocibles de vivirlos |

No se exige aprobar H1/H2/H3 de una vez. Un hito parcial se informa como parcial y no reduce la definición de terminado de los siguientes.

## 12. Matriz de evidencia y cierre

Todos los grupos están **pendientes de ejecución para esta especificación**. Los resultados anteriores del prototipo son antecedentes de regresión, no pruebas de estos nuevos criterios.

| Grupo | Casos | Evidencia mínima | Revisión |
|---|---|---|---|
| Visual | VIS-01 a VIS-04 | Fuente, exportación, capturas y clip de poses en Unity | Arte + integración |
| Personalización | PER-01 a PER-05 | Matriz corporal, comparación de perfil y prueba de migración/fallo | Arte + funcional |
| Rostro | FAC-01 a FAC-04 | Catálogo, transiciones e interrupciones desde varios ángulos | Arte + funcional |
| Cuerpo/persona | COR-01 a COR-04 | Clips comparables de los tres perfiles y evidencia de transición | Diseño + animación |
| Diálogo | DIA-01 a DIA-05 | Contextos, líneas elegidas, tiempos, bloqueo y ejecución sin red | Diseño + funcional |
| Memoria | MEM-01 a MEM-06 | Antes/después, eventos duplicados, guardar/cargar y límites de ruta | Diseño + persistencia |
| Autonomía | AUT-01 a AUT-06 | Circuito grabado, bloqueos, recuperación y regresión de supervivencia | Diseño + navegación |

Cada resultado debe registrar: ID del caso, versión probada, estado inicial, acción, resultado esperado, resultado observado, archivo de evidencia, defectos y resolución. Estados permitidos: pendiente, pasó, falló, parcial o bloqueado por dependencia. “No probado” nunca equivale a “pasó”.

**Defectos que impiden aceptar el hito correspondiente:** pérdida de perfil/progreso, contradicción de hechos o estados, romance automático, doble recompensa del rescate, bloqueo sin recuperación, muerte atribuible a navegación sin salida, personaje todavía provisional presentado como final, deformación persistente visible, o dependencia de conexión para el contenido esencial.

**Revisión subjetiva:** la identidad, atractivo visual, naturalidad y tono requieren aprobación humana sobre capturas y clips. La lista objetiva evita defectos, pero no reemplaza ese juicio. Un cambio de decisión aprobada debe explicitar su motivo e impacto y consultarse antes de reemplazarla.

## 13. Decisiones abiertas de esta especificación

| ID | Decisión pendiente | Propuesta o trabajo preparado | Qué condiciona |
|---|---|---|---|
| P-01 | Rostro y silueta definitivos | REF-01 como candidata; recuperar referencias anime originales | Cierre artístico de H1 |
| P-02 | Extremos corporales aceptables | Dos ejes y matriz de nueve combinaciones | Producción de blend shapes y ropa |
| P-03 | Catálogo y tiempos faciales | Seis normales y tres cómicas, con tiempos de ensayo | Cantidad final de recursos y animación |
| P-04 | Gestos y tono por personalidad | Tabla COR y diálogos de ejemplo | Dirección de animación y escritura |
| P-05 | Ritmo de charla y autonomía | Frecuencias y distancias propuestas | Sensación de compañía y navegación |
| P-06 | Alcance de memoria persistente en H2 | Hechos mínimos y relación, conservados juntos | Guardado y continuidad de la demostración |
| P-07 | Plataforma, hardware y rendimiento | Medición del equipo de prueba; sin cifras inventadas | Presupuesto técnico y aceptación de rendimiento |
| P-08 | Primera clase funcional | Espadachina, maga o arquera; sin selección automática | Especificación del combate de H3 |

Las propuestas no reemplazan los acuerdos. Pueden revisarse por separado; su existencia no autoriza modificar código desde el trabajo de diseño.
