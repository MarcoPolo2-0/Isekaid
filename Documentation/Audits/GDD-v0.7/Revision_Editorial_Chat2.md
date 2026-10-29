# GDD v0.7 — Auditoría editorial, narrativa y de consistencia

**Fecha:** 29/10/2026, Guatemala. **Responsable:** Chat 2 — Game Design.  
**Objeto:** [PR #13](https://github.com/MarcoPolo2-0/Isekaid/pull/13), rama docs/isek-gdd-v07-consolidated.  
**Entrada revisada:** `d94d67109920ce0883f3a23b406c52d55e86b593`; main de contraste `f835d3b`.  
**Resultado editorial:** PREPARADO PARA REVISIÓN FINAL DE DIRECCIÓN, con comprobación técnica de Chat 3 y aprobación formal pendientes. No se declara maestro oficial ni se fusiona el PR.

## 1. Fuentes y alcance

Se compararon el [GDD v0.7](../../GameDesignDocument_v0.7_borrador.md), [v0.5](../../GameDesignDocument_v0.5.md), [v0.6](../../GameDesignDocument_v0.6_borrador.md), [DEC-001–007](../../DECISIONS.md), [contrato F2](../../Specifications/VRM_Fase2_Contrato_Funcional_v0.1.md), [F3 borrador](../../Specifications/VRM_Fase3_Personalidad_Contextual_v0.1_BORRADOR.md), [especificación F3A](../../Specifications/F3A_Especificacion_v0.1.md), [aceptación F3A](../../Specifications/F3A_Aceptacion_v0.1.md) y los tres artefactos del PR #11: [dirección](../../Specifications/F3A_Diferenciacion_Personalidades_v0.1_PROPUESTA.md), [revisión de los 60 IDs](../../Specifications/F3A_Diferenciacion_Revision_60_v0.1.md) y [parche de 33 ajustes](../../Specifications/F3A_Diferenciacion_Editorial_v0.1_PROPUESTA.json).

El estado técnico se contrastó con [F2](../VRM-F2/INFORME.md), [F3A revisión 1](../VRM-F3A/review-1/INFORME.md), [locomoción/render](../Companion-Locomotion-Render/INFORME.md) y lectura estática de CompanionState, CompanionProfile, CompanionPalette, CompanionPrototype y PrototypeHud. Los resultados manuales se atribuyen a la instrucción directa de Dirección: conversaciones, incapacitación/reanimación y seguimiento continuo funcionan; presentación VRM mejorada; personalidades todavía demasiado similares. Esta auditoría no repite esas pruebas.

Las fichas concretas del proyecto SillyTavern no se proporcionaron ni inspeccionaron. Se adopta la metodología solicitada y se citan fuentes públicas, sin afirmar una comparación con tarjetas privadas o importar su contenido.

## 2. Cobertura de la visión heredada

La comparación abarca todas las secciones principales, no solo las modificadas. «Conservado» significa que el requisito o la propuesta mantiene visibilidad y estado, no que esté implementado o aprobado por esta revisión.

| Fuente | Decisión, contenido o propuesta preservada | Destino en v0.7 y tratamiento |
|---|---|---|
| v0.5 §1 | Isekai medieval semiabierto, primera persona, compañera adulta central, amistad y romance opcional, offline | §§1–4, 6 y 13; visión ACORDADA, sin reducirla al prototipo |
| v0.5 §2 | Brazos del protagonista, acción en tiempo real, controles y arsenal por decidir | §2 y pendientes §16; combate no declarado implementado |
| v0.5 §3 | Identidad visual anime, cabello/top/pantalón de referencia, silueta, expresión normal/cómica, animaciones y prioridades | §3; editor propio pausado por DEC-007, referencia artística conservada y capacidades visuales pendientes |
| v0.5 §4 | Rasgos, emociones, recuerdos, gustos, hábitos, aprendizaje, misiones personales, relación y progreso táctico | §4 y método §18; profundidad restaurada explícitamente sin inventar sistemas completos |
| v0.5 §5 | Espadachina, maga y arquera; clase independiente, elección y entrenamiento propuesto | §5 y §12; flujo de elección pendiente, preasignación inicial solo como alternativa a conciliar |
| v0.5 §6 | Biblioteca local, iniciativas/alertas oportunas, no repetición, comentarios caducados y opción local fuera del MVP | §6; F3A presente separado de visión futura y opción local PROPUESTA |
| v0.5 §7 | Seguir/esperar y órdenes futuras, prioridad de supervivencia, incapacidad 30 s, muerte/checkpoint y secuestro distinto | §7; ensayo vital separado de combate 0 HP/checkpoint, ratificación temporal pendiente |
| v0.5 §8 | Animales por proximidad, hordas por vulnerabilidad observable, grandes con preferencia no exclusiva por jugador y alertas válidas | §8 completo; diseño ACORDADO, implementación PENDIENTE |
| v0.5 §9 | Bloqueo, interrupción, amenaza, retirada, rescate bajo presión y claridad visual | §9; propuestas de protección conservadas, sin promoverlas a mecánicas presentes |
| v0.5 §10 | Desconocidos, vínculo gradual, hogar, actividades propias, salida en solitario, posibles molestias y recuperación sin castigo automático | §10; acuerdos y reglas de equilibrio PROPUESTAS siguen separados |
| v0.5 §11 | Captura/secuestro, intervención/persecución/pistas/rescate, ausencia de Game Over inmediato y justicia de guardado | §11; reglas principales ACORDADAS, detalles PROPUESTOS e implementación PENDIENTE |
| v0.5 §12 | Llegada isekai, trampa de bandidos sin reloj de Downed, alianza voluntaria, hogar posterior y sendero bloqueado propuesto | §12; origen de la trampa precisado, ejemplos depurados, misión/patrulla/checkpoint no presentados como implementados |
| v0.5 §13 | Mundo final y ampliación del prototipo a combate, enemigos, hogar y secuestro | §13; prototipo presente y objetivos posteriores por separado |
| v0.5 §14 | Modularidad, estado serializable, separación de sistemas y nombres provisionales | §14; nombres presentes contrastados y sistemas futuros conservados sin imponer componentes nuevos |
| v0.5 §15 | Herramientas de producción y autorización de servicios externos pagados | §15 conservado; no se convierten en dependencias del juego |
| v0.5 §16 | Arsenal, defensa, reanimación, dificultad/daño amigo, atacantes, jefes, clase, captura, hogar, mundo/guardado y narrativa pendientes | §16; detalles omitidos en la primera consolidación recuperados como pendientes |
| v0.6 §17 | Registro A-01–A-15 y protección de visión; propuestas/limitaciones no recortan acuerdos | §§1–19; A-03/A-04 matizados por DEC-007; regla temporal mantiene su matiz de aprobación |
| v0.6 §18 | Diagnóstico fechado del antiguo prototipo, separación de evidencia de programación y arte | §§3, 6, 13, 14 y 17; historial intacto, diagnóstico actual basado en entregas posteriores |
| v0.6 §19 | 34 criterios propuestos VIS/PER/FAC/COR/DIA/MEM/AUT e hitos H1–H3 | Referencia histórica en §19; no aprobar toda la propuesta ni reactivar cuerpo por copiar esos hitos |
| v0.6 §20 | Funciones de tres chats, versiones, aprobación parcial y cambios de alcance | §19 y workflow v1.2; puerta explícita de promoción |
| v0.6 §21 | Historial de revisión y evidencia artística/conceptual diferenciada | Referencias preservadas; registros originales no reescritos |
| v0.6 §22 | DEC-007, catálogo/VRM 1.0, sustitución del editor propio, independencia de capas y fases pendientes | §§3, 4, 6, 13 y 17; dirección adoptada sin aceptar automáticamente F2/F3A/F4 |

Los registros v0.4/v0.5 y todas las revisiones de v0.6 permanecen en sus archivos. Las decisiones DEC-001–005 no se borran: DEC-006 reemplaza su organización anterior de repositorios y DEC-007 pausa cuerpo/Woman/G2. No se declaran recursos inviables o pruebas antiguas superadas por el cambio de estrategia.

## 3. Hallazgos y correcciones explícitas

| ID | Problema encontrado | Corrección y motivo |
|---|---|---|
| GDD-R01 | La definición de implementado incluía «alcance probado», mezclando existencia y validación | Leyenda de cinco estados y matriz con decisión/producto por separado; una prueba no aprueba una decisión |
| GDD-R02 | §7 decía que pausa/tiempo activo seguían sin validar, pese a ensayos existentes | Se reconoce implementación y prueba técnica; se conserva la ratificación formal pendiente de v0.6 sin cambiar balance |
| GDD-R03 | Supervivencia y órdenes podían leerse como combate/percepción/checkpoints actuales | Fronteras explícitas en §§2, 5, 7, 8, 10–12: Downed de ensayo y Game Over/reinicio no son 0 HP de combate y guardado de campaña |
| GDD-R04 | «Migración pendiente» para Dandere sugería renombrar datos al cambiar una etiqueta | Se registra nombre visible ACORDADO e implementación PENDIENTE; preservar Shy/2, shy e IDs SHY sin migración por etiqueta |
| GDD-R05 | Ejemplos heredados de prólogo reforzaban negación automática de ayuda y tartamudeo, y podían competir con la biblioteca vigente | Se remite a A.RESCUE canónica y a focos editoriales propuestos del PR #11, preservando ejemplos antiguos en v0.5/v0.6 |
| GDD-R06 | Aprendizaje, gustos, hábitos y referencia artística normal/cómica quedaban demasiado resumidos | Se explicitan objetivos completos; catálogo/alternativas y sistema híbrido propuesto separados de soporte universal o arte final |
| GDD-R07 | FirstRescue_BanditTrap y confianza «moderada» podían interpretarse como evento y nivel ya implementados | Se distinguen bandera/Trust=1 existentes de evento de campaña propuesto y se exige futura reconciliación sin premio duplicado |
| GDD-R08 | §5 permitía elegir clase, mientras §12 dejaba abierta una clase ya definida sin explicar la relación | Se conserva elección como objetivo; flujo pendiente y preasignación narrativa como alternativa PROPUESTA que requiere conciliación |
| GDD-R09 | §16 omitía pendientes heredados sobre dificultad, daño amigo, jefes, atacantes y detalles de rescate | Se restauran explícitamente como decisiones abiertas, sin ampliar implementación |
| GDD-R10 | «El juego funciona sin Internet» parecía una certificación; la prohibición genérica de LLM borraba la opción local fuera del MVP de v0.5 | Offline queda como requisito acordado con prueba física pendiente; opción local permanece PROPUESTA sin autorizar integración |
| GDD-R11 | Fichas profundas eran un listado general sin criterio de contenido o evolución | Tabla de nueve áreas, límites de autoría, contraste en seis situaciones y referencias por arquetipo; valores concretos/fichas pendientes |
| GDD-R12 | La matriz podía dar por implementadas las revisiones del PR #11 o aceptar fases por observación manual | Se comprueba 0/33 aplicados; tres plantillas y etiquetas distinguidas, criterios PARTIAL enlazados y alcance manual acotado |
| GDD-R13 | «Trampa o situación equivalente» debilitaba el origen aprobado; promoción de maestro y autoridad de fuentes eran poco explícitas | Premisa fija trampa de bandidos; se preservan detalles de misión como propuesta y se explicita revisión técnica + decisión final de Dirección |

Se añadieron referencias relativas de F3A, aceptación, siete emociones, propuesta general histórica y auditorías. No se encontró un motivo para reescribir los documentos fuente: se corrige el consolidado y se conserva la cronología. Los enlaces se comprueban sobre el árbol versionado del PR, incluidas las nuevas referencias de esta revisión.

## 4. Personalidades y fichas: compatibilidad de la revisión

- **ACORDADO en esta instrucción:** metodología de fichas profundas y nombre visible Dandere. Los datos/biografías individuales y actuaciones quedan pendientes de autoría y revisión. No se inventa un nuevo número DEC.
- **Tsundere:** resolver y recuperar capacidad de actuar sigue siendo compatible con estudiar orgullo/vulnerabilidad; no implica hostilidad o negación automática de gratitud.
- **Yandere:** intensidad y contradicciones se diseñan con voluntad y límites propios. Un rescate confirmado puede importar mucho sin crear un historial de intimidad, romance automático, violencia o dependencia extrema.
- **Dandere:** observar y afirmar una decisión permanece compatible con Shy y los 11 ajustes propuestos de ese grupo. Reserva no equivale a tartamudeo o pasividad obligatorios.
- **Deredere:** solo estudio PROPUESTO; ninguna cuarta opción, enum o dato de juego se añade. Aprobación del GDD no la autoriza por omisión.

El parche PR #11 conserva 33 ajustes de texto/intención sobre IDs existentes, 27 textos retenidos y perfiles de actuación propuestos. La comparación por ID verifica que ninguno de los 33 textos nuevos coincide todavía con su entrada canónica; tampoco coincide el recurso runtime con esos textos propuestos. No se aplica ese parche para corregir el GDD. Un deseo, hábito o detonante nuevo de ficha deberá proponerse sin alterar silenciosamente las rutas, recuerdos o efectos existentes.

## 5. Comprobaciones y evidencia

La verificación de publicación debe confirmar:

1. GDD v0.5, v0.6, DECISIONS, biblioteca canónica y recurso runtime iguales al encabezado de entrada del PR; ningún archivo Unity modificado.
2. Los 33 pares expected/set siguen siendo propuesta: 0 aplicados en fuente y runtime, con 60 IDs y 18 opciones conservados.
3. Enlaces relativos y anclas internas de los dos documentos revisados válidos contra archivos versionados; numeración de 19 secciones principales única y ordenada.
4. Historial previo completo de readme conservado como prefijo, con registro añadido por hallazgo.
5. Diff limitado a GDD v0.7, esta auditoría y readme; commit/push en la rama del PR #13 y PR abierto/sin fusionar.

**Resultado estático obtenido:** 19 secciones principales en orden; 22 secciones históricas trazadas; 13 hallazgos; 39 enlaces locales válidos; cinco archivos protegidos comparados con la entrada del PR; historial de readme conservado y 0/33 ajustes aplicados en fuente/runtime. El comprobador existente `tools/build_f3a_runtime_content.py --check` informa 60 entradas y 18 rutas con recurso runtime idéntico a su fuente. La confirmación remota del punto 5 se entrega al publicar.

Estas son comprobaciones documentales/estáticas. No se ejecutan Unity, compilación, Play Mode, builds, pruebas de render ni mediciones de personalidad. La observación manual del usuario se documenta como tal; no se presenta como un ensayo repetido por Chat 2. Los resultados efectivos de publicación se registran en readme y en la entrega final.

## 6. Decisiones abiertas para Dirección

1. Resolver, tras la revisión técnica de Chat 3, si el contenido de v0.7 puede promocionarse a maestro y con qué identificación de versión. Hasta entonces v0.7 sigue BORRADOR y el PR no se fusiona.
2. Ratificar formalmente la precisión de tiempo activo para la regla de 30 s, sin modificar entretanto el comportamiento probado.
3. Aprobar/revisar por ID los 33 ajustes y el siguiente trabajo N1/N2. La fusión documental de #11 no autoriza aplicarlos.
4. Elaborar/revisar fichas originales completas; definir deseos, hábitos, detonantes y evolución particulares sin convertir ejemplos en recuerdos de campaña.
5. Resolver Deredere por separado. Clase inicial/flujo de elección, combate, daño amigo/dificultad, secuestro, hogar, checkpoints y detalles de narrativa conservan sus pendientes; no es necesario cerrarlos todos para aprobar un GDD que los identifica como tales.
6. Mantener aceptación F2/F3A y visual por criterio, con límites técnicos, derechos, rendimiento y plataformas propios; la aprobación editorial del maestro no la sustituye.

## 7. Recomendación

La consolidación corregida es apta para revisión final como candidato a nuevo maestro: preserva la visión y distingue avances, propuestas y pendientes. No recomiendo marcarla todavía oficialmente APROBADA desde Chat 2 ni usar su aprobación futura como autorización general para implementar todo lo descrito. La revisión de exactitud técnica corresponde a Chat 3 y la aprobación final a Dirección/usuario.

Las correcciones editoriales resuelven contradicciones y omisiones de visibilidad. No eliminan características por falta de implementación ni aprueban mecánicas nuevas. El único cambio de nomenclatura acordado es el visible Dandere, conservando el contrato técnico existente.
