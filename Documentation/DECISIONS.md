# Isekaid — Registro de decisiones

**Autoridad coordinadora:** Chat 1 (Dirección).  
**Aprobación final:** usuario.  
**Actualizado:** 2026-10-29.
**Repositorio vigente:** `MarcoPolo2-0/Isekaid`.
**Documentación:** `Documentation/`.

Este documento contiene decisiones de diseño para que los Chats 2 y 3 trabajen con el mismo contexto. Una decisión aprobada de diseño **no** equivale a validación técnica ni a implementación terminada.

**Maestro documental vigente — DEC-008:** [GDD v0.7](GameDesignDocument_v0.7.md), aprobado documentalmente por instrucción directa de Dirección del 29/10/2026. Las propuestas y validaciones pendientes conservan su estado. Las referencias al maestro anterior en decisiones fechadas son históricas.

**Dirección visual vigente — DEC-007:** VRM 1.0 + catálogo local adoptados; F2 autorizada y pendiente de entrega real. Woman/G2 y el editor corporal quedan pausados e históricos. La continuidad de correcciones y las restricciones de repositorios de decisiones anteriores se leen con DEC-006/007; no son órdenes vigentes de reanudación. [Contrato F2](Specifications/VRM_Fase2_Contrato_Funcional_v0.1.md), [workflow v1.2](Workflow_Desarrollo_v1.2.md).

## DEC-001 — Base corporal personalizable para la compañera

- **Vinculada a:** `ISEK-001`.
- **Estado:** Dirección de diseño aprobada; **viabilidad técnica pendiente de comprobar**.
- **Origen:** propuesta del usuario ratificada por Chat 1.
- **Asset seleccionado como candidato principal:** [Woman, de Fly_jkey (Sketchfab)](https://sketchfab.com/3d-models/woman-b303b7606140480f8335cda121b9607c).
- **Archivo fuente descargado por el usuario:** `assets/woman.fbx` en el repositorio principal. La estructura interna de la malla y su viabilidad deben verificarse en Blender.
- **Licencia anunciada en Sketchfab:** Creative Commons Attribution (CC BY). Se permite modificación y uso comercial con atribución. Antes de publicar, conservar autor, URL del modelo, enlace a la licencia exacta, indicación de modificaciones y cualquier aviso incluido con la descarga.

### Decisión de diseño

Aprovechar el maniquí editable como punto de partida, no como personaje terminado. La apariencia neutral del editor tendrá proporciones equilibradas y el jugador podrá adaptar atributos corporales, inicialmente:

1. Pecho.
2. Caderas.
3. Cintura (sujeto a evaluación de topología y animación).

Usar preferentemente Shape Keys en Blender / Blend Shapes en Unity cuando las pruebas demuestren que son adecuados. Los controles deben ser independientes, guardar sus valores y modificar también la ropa de forma coherente. Personalidad y apariencia no están relacionadas.

El resultado final debe tener rostro anime, ojos detallados, cabello, vestimenta, materiales anime/cel shading, expresiones y rig/animaciones compatibles con Unity. La versión inicial será un personaje femenino adulto con top y pantalón sencillos.

### Asignación al Chat 2 — Diseño

Preparar `Documentation/Specifications/ISEK-001.md` con referencias, silueta neutral, rangos visuales razonables, controles, riesgos de clipping, animaciones mínimas y criterios verificables de aceptación. Distinguir propuesta de decisión ya aprobada.

### Asignación al Chat 3 — Desarrollo

Antes de sustituir el maniquí actual, validar el FBX real en Blender: topología, existencia o ausencia de rig y Shape Keys, edición, exportación, importación a Unity, deformaciones y compatibilidad con ropa/poses. Entregar informe y capturas reales. No reemplazar el recurso por otro modelo sin autorización.

### Puertas de aceptación

- **Primera puerta:** comprobar que la malla es una base viable y editable.
- **Segunda puerta:** demostrar ajustes independientes de proporciones sin defectos graves.
- **Tercera puerta:** comprobar deformación con rig, movimiento y ropa.
- **Cuarta puerta:** integrar personaje anime 3D con personalización funcional en Unity.

**No está aprobado afirmar que ISEK-001 está completada.**

## Principio de alcance del prototipo

Antes de ampliar el mundo o la historia, priorizar una compañera anime 3D creíble y personalizable, expresiones y reacciones contextuales. Reutilizar sistemas del prototipo técnico cuando sean válidos, sin convertir geometría provisional en arte definitivo.

---

Para nuevas decisiones: `DEC-002`, `DEC-003`, etc. Referenciar tareas `ISEK-###`, registrar fecha, estado, motivo y consecuencias.

## Documentación de diseño existente

- `Documentation/Especificacion_Companera_v0.1.md` está en estado **PROPUESTA** y requiere revisión del Chat 1.
- `Documentation/GameDesignDocument_v0.6_borrador.md` está en estado **PROPUESTA**; no reemplaza todavía a v0.5 como GDD aprobado.
- `Documentation/Workflow_Desarrollo_v1.0.md` regula la coordinación de los tres chats.

La decisión DEC-001 **no aprueba automáticamente** la totalidad de la especificación v0.1 ni el GDD v0.6.

## Organización técnica aprobada — dos repositorios

- **Principal:** `MarcoPolo2-0/Isekaid` (público) para `Documentation/`, recursos originales, decisiones e Issues.
- **Unity:** `MarcoPolo2-0/Isekaid-2026-10-27_13-09-18` (privado) para el proyecto Unity 6 real.
- **Vínculo:** submódulo `Isekaidunity`, declarado en `.gitmodules` del repositorio principal. Referencia a un commit fijo; el principal se actualiza manualmente cuando se apruebe una versión del código Unity.
- Consultar `Documentation/Workflow_Desarrollo_v1.0.md` para procedimientos de actualización y trabajo.

## DEC-002 — Revisión de entregas ISEK-001 y autorización de prototipo corporal controlado

**Fecha:** 28/10/2026.  
**Estado:** APROBADA COMO AUTORIZACIÓN DE TRABAJO ACOTADO. **NO** implica aceptación artística ni cierre de ISEK-001.  
**Tarea:** [ISEK-001](https://github.com/MarcoPolo2-0/Isekaid/issues/1).

### Evidencia revisada

- Diseño: [Especificación ISEK-001 v0.1](Specifications/ISEK-001.md), entrega en commit `1874c868f5730765914abe82d99ab8e6e693218a`.
- Técnica: [Auditoría ISEK-001](Audits/ISEK-001/INFORME.md), entrega en commit `62b613e0b470c7c8f510dcfb87e0c0057ba3a0ca`.
- El original `assets/woman.fbx` conserva el hash registrado por ambos informes.

### Resolución de Dirección — Chat 1

1. **Se acepta la evaluación técnica de la puerta G1**: la malla Woman se considera viable para continuar el prototipado, sin declarar terminada su adaptación artística, su licencia detallada ni una integración de producción.
2. **G2: factibilidad demostrada, resultado artístico pendiente.** Los ensayos de Blender generaron controles independientes de pecho, caderas y cintura y 27 combinaciones; el FBX diagnóstico se importó en un proyecto Unity aislado con Avatar Humanoid válido y Blend Shapes. Esto autoriza seguir explorando los tres ejes, **no** aprobar extremos, aspecto neutral definitivo ni declarar acabada cintura.
3. **G3 y G4 continúan pendientes.** Se observaron penetraciones en prendas de prueba, deformaciones de pesos automáticos y ausencia de rostro anime, cabello, animaciones finales y prueba en las escenas del juego. No presentar estas partes como completas.
4. **Se aprueba la especificación v0.1 solo como marco de trabajo de las puertas G1–G4 y de sus 16 casos de aceptación.** Las cifras, tiempos, amplitudes estéticas y elementos etiquetados como propuestas siguen sujetos a revisión visual y funcional. El GDD v0.6 continúa en BORRADOR.
5. **Se autoriza a Chat 3 una etapa limitada G2-PROTO** en rama separada del repositorio Unity privado: preparar una **versión neutral corporal editable** y **tres controles experimentales** de pecho/caderas/cintura; mejorar transiciones y evaluar pesos/ropa con pruebas reproducibles. La ropa de ensayo y el esqueleto diagnóstico **no** son recursos finales. No sustituir el maniquí de las escenas actuales ni fusionar con `main` antes de revisión del Chat 1.
6. **Se solicita a Chat 2 revisión visual enfocada:** comparar turnaround neutral con dirección anime, documentar rangos y referencias faciales/cabello, preparar criterios de aprobación visual que no dependan solo de métricas. Las versiones nuevas de requisitos se entregan como propuestas y se revisan aquí antes de ampliar el alcance de Chat 3.

### Condiciones de entrega de G2-PROTO

- Archivo Blender derivado editable y FBX exportable en la rama de trabajo de Unity, sin alterar el original del repositorio principal; registrar procedencia, versión y hash.
- Turnaround frontal, perfil, tres cuartos y espalda del cuerpo neutral; vistas comparables de los extremos y matriz de 27 combinaciones.
- Informe de fallos visibles de ropa/skin y cómo se corrigen; no ocultar clipping, ni declarar completa la aceptación G3.
- Evidencia de reimportación correcta en Unity; preferentemente ensayos en entorno separado y sin tocar escenas de producción.
- Informe al Issue `ISEK-001`, commit/PR y evidencias; advertir explícitamente qué no está validado.
- **La autorización termina en la presentación de G2-PROTO**: rostro anime completo, ropa final, locomoción integrada y reemplazo en creador/aventura requieren revisión de la siguiente puerta.

### Riesgos y obligaciones abiertas

- Verificar variante exacta CC BY, atribución, enlaces de licencia y modificaciones antes de distribuir derivados.
- No fijar presupuesto de rendimiento/plataforma sin mediciones y decisión de alcance.
- No actualizar la referencia del submódulo del repositorio principal por una prueba todavía no aceptada.

**Próxima revisión de Chat 1:** aceptación artística del cuerpo neutral y rangos, seguida de autorización específica para G3/G4.

## DEC-003 — Revisión de G2-PROTO y continuidad limitada de correcciones

**Fecha:** 28/10/2026, Guatemala.

**Estado de la entrega:** RECIBIDA COMO EVIDENCIA TÉCNICA; **G2 REQUIERE CORRECCIONES**.

**Autorización:** únicamente **G2-CORR**, iteración del prototipo existente. G3/G4 no quedan autorizadas.

**Tarea:** [ISEK-001](https://github.com/MarcoPolo2-0/Isekaid/issues/1), abierta. La aprobación artística final del usuario permanece pendiente.

### Versiones y comprobaciones de esta revisión

- Chat 2: [dirección artística v0.1](https://github.com/MarcoPolo2-0/Isekaid/blob/4610d8a6997d8dc1f66650b4a69a3379ecedcace/Documentation/Specifications/ISEK-001_Direccion_Artistica_v0.1.md), commit `4610d8a6997d8dc1f66650b4a69a3379ecedcace`. Revisadas las cuatro láminas, alternativas, limitaciones y modificaciones documentales. Son conceptos ilustrados; no acreditan geometría ni funcionamiento en Unity.
- Chat 3: [PR privado #2](https://github.com/MarcoPolo2-0/Isekaid-2026-10-27_13-09-18/pull/2), borrador abierto, rama `feature/isek-001-g2-body`, commit `afcdd61649bac3cabc833605d61037006eaf6509`. [Informe versionado](https://github.com/MarcoPolo2-0/Isekaid-2026-10-27_13-09-18/blob/afcdd61649bac3cabc833605d61037006eaf6509/SourceArt/Companion/Woman/G2-PROTO/INFORME.md). Los enlaces requieren acceso al repositorio privado.
- GitHub confirmó 169 archivos añadidos en el PR: 166 dentro de `SourceArt/Companion/Woman/G2-PROTO/` y tres correspondientes a la carpeta de importación `Assets/G2BodyPrototype` y sus metadatos. No cambia código runtime, escenas ni ProjectSettings.
- La copia revisada coincide con los commits publicados en los directorios examinados. Se recalcularon los 165 hashes del manifiesto: todos coinciden. Se ejecutó `scripts/check_delivery.py`: `G2_DELIVERY_CHECKS_PASSED`. Este chequeo comprueba archivos y resultados guardados; no vuelve a ejecutar Blender o Unity.
- Se recalcularon los hashes de los 58 archivos protegidos: coinciden con la referencia de la entrega. Esto conserva los cambios locales anteriores; no significa que esos archivos estén todos publicados en `main`.
- Se inspeccionaron visualmente la comparación original/neutral, las cuatro matrices de 27 combinaciones, una muestra de flexión profunda y el render aislado de Unity. No se revisó individualmente cada PNG de poses ni se reprodujo animación continua.

### Estado de los criterios

| Criterios | Resolución y evidencia efectiva |
|---|---|
| AC01–02 / G1 | Se mantiene la aceptación técnica de DEC-002. Hay original preservado, inventario y registros de reapertura/exportación; esta revisión no repitió la apertura en Blender. |
| AC03 / AV01–02 | Cuatro vistas y comparación original/neutral entregadas. **Neutral no aceptada**: persisten separación y forma muy esférica del pecho, cintura pinzada y transiciones marcadas de pelvis/muslo. La modificación existe, pero todavía no alcanza el equilibrio solicitado. |
| AC04–06 / AV03 | **Parciales**. Seis Shape Keys representan tres ejes; los resultados muestran desplazamientos y protección de cabeza, manos y pies. Unity registra los seis controles por malla y piernas inmóviles ante pecho. Faltan barridos continuos y comprobación de todos los invariantes anatómicos entre ejes; no basta que existan parámetros separados. Cintura conserva estado experimental. |
| AC07 / AV04 | **Parcial**. 27 combinaciones estáticas, 108 renders y resultados sin coordenadas inválidas ni triángulos colapsados bajo el umbral del ensayo. No hay evidencia de barridos continuos ni comprobación completa de autointersecciones/cruces de triángulos. Rangos artísticos pendientes. |
| AC08 / G3 | **No superado e incompleto**. Se probaron seis poses con tres combinaciones globales: 18 de las 162 configuraciones previstas para tres ejes. Nueve casos conservan penetraciones; la peor profundidad pasa de 7,20 a 13,70 mm. Estiramiento máximo de arista 3,578, sin mejora. |
| AC09 / G3 | Sin prueba de locomoción, transiciones, sentarse/levantarse o contacto de pies en movimiento continuo. Avatar Humanoid válido no equivale a retargeting o animación aceptados. |
| AC10–15 / G4 y AV05–10 | No demostrados por esta entrega: creador/aventura, colores, persistencia/migración, rostro/expresiones, cabello y pruebas de personalidad. Permanecen fuera del alcance autorizado. |
| AC16 | **Parcial**: fuentes editables, exportación, scripts, hashes y límites entregados; licencia exacta/atribución y paquete final de producción pendientes. No se declara completado por existir un `.blend`. |

La mejora de ropa en reposo es válida dentro del ensayo: cero penetraciones de vértices detectadas en 27 combinaciones, con separación mínima registrada de aproximadamente 1,68 mm. El muestreo de vértices no certifica ausencia de intersecciones entre superficies. La ropa sigue siendo una muestra abierta de diagnóstico. Las 18 poses son evidencia exploratoria útil, no cumplimiento de AC08; no se exige convertir G2 en una entrega completa de G3.

La evidencia Unity corresponde a importación aislada en 6000.6.7f1: Avatar válido/humanoide, tres mallas con seis Blend Shapes y 27 combinaciones corporales horneadas. El script recorre las combinaciones sobre el cuerpo; no valida conjuntamente ropa/cuerpo en las 27 combinaciones dentro de Unity. No acredita Play Mode, materiales URP finales, rendimiento, dispositivo ni build.

### Resolución artística limitada

1. **ART01:** seleccionar **A — Anime equilibrado** como referencia de trabajo. No es aprobación del rostro 3D ni aceptación artística final del usuario; B queda archivada como alternativa.
2. **ART02:** aprobar la intención de neutral adulta moderada y transición natural tórax/cintura/pelvis como objetivo de corrección. No aprobar la neutral actual ni fijar altura o medidas por los píxeles del concepto.
3. **ART03:** adoptar extremos moderados como intención; amplitudes exactas pendientes de vistas reales y barridos. No reducir rangos silenciosamente para esconder defectos ni convertirlos aún en contrato del perfil.
4. **ART04:** aceptar como referencia documental el cabello largo suelto, top de cuello redondo con holgura y pantalón de tela sencilla. Las láminas requieren corregir escote, costuras similares a jeans y ajuste excesivo. Esto no autoriza construir ropa/cabello finales.
5. **ART05:** conservar el lenguaje facial híbrido como dirección y usar AV01–04 para la revisión corporal. AV05–10 quedan como criterios propuestos para hitos posteriores; no autorizan sistema facial ni integración. La proximidad visual neutral/alegría debe corregirse en la siguiente propuesta.

### Siguiente instrucción — Chat 2

Preparar revisión artística v0.2 basada en los renders reales del commit `afcdd61`, con observaciones localizadas de pecho/tórax, cintura y pelvis/muslo, y comparación con A. Entregar vistas comparables sin cabello que oculte el cuerpo, instrucciones claras para neutral/extremos y correcciones del top/pantalón conceptual. Separar ilustración, anotación sobre evidencia y resultado 3D. Revisar AV01–04 por punto; no marcar los diez criterios como aprobados ni modificar las evidencias originales. Mantener apariencia y personalidad independientes. Publicar documentación en el principal y enlazarla al Issue; no tocar código ni recursos Unity. Cualquier requisito adicional será propuesta pendiente de Dirección.

### Siguiente instrucción — Chat 3 / G2-CORR

Continuar en la misma rama y PR, mediante entregas pequeñas:

1. Corregir localmente neutral y transiciones conforme a ART02 y la dirección A. Mantener Woman y el original; preservar conectividad cuando sea viable y justificar cualquier modificación necesaria. Conservar una versión recuperable de la entrega revisada.
2. Recalcular extremos desde la neutral corregida; presentar cuatro vistas con cámaras/escala/luz iguales, comparación antes/después, 27 combinaciones y barridos continuos de cada eje con los otros neutrales y en extremos mixtos. Registrar parámetros, archivo y regiones que deben permanecer estables. No cambiar perfil, UI ni reglas del juego.
3. Dentro del diagnóstico ya autorizado, corregir pesos/holguras locales o correctivos necesarios para reducir los defectos detectados en pelvis, rodillas y hombros, y resolver la regresión de clipping. Repetir al menos los mismos 18 casos comparables y registrar cruces de superficies que el muestreo actual no detecte. No inflar globalmente la ropa ni presentar las muestras como prendas finales. La batería completa de 162 configuraciones y las animaciones siguen pendientes de una autorización G3.
4. Reabrir/exportar/reimportar el derivado y repetir las comprobaciones en Unity aislado, aplicando los controles de forma sincronizada a cuerpo y muestras de ropa. Actualizar informe, hashes, evidencia y matriz AC/AV. Identificar claramente qué se ejecutó y qué sigue pendiente.

La primera entrega de esta iteración debe permitir revisar neutral/rangos antes de ampliar trabajo de rig o ropa. Las observaciones de Chat 2 que excedan esta decisión no se implementan automáticamente.

### Límites y próxima puerta

- **No aprobar G2 todavía ni iniciar G3/G4 completas.** No crear rostro final, cabello, ropa final, locomoción integrada ni nuevas capas/sistemas runtime en esta iteración.
- PR #2 permanece en borrador y sin fusionar. No sustituir el maniquí, modificar escenas/prefabs de producción ni actualizar el submódulo, que sigue fijado a `362a56673373c88b0e4168f19501db8cd52e4b40`.
- Mantener fuentes y herramientas de ensayo separadas del runtime; no es necesario cambiar arquitectura para esta corrección de recursos.
- Confirmar variante exacta de licencia y atribución antes de distribuir derivados. El GDD v0.6 y la especificación general no quedan aprobados por esta decisión.
- Próxima revisión: neutral/rangos corregidos, barridos e invariantes de AC03–07/AV01–04, y reporte explícito de las regresiones diagnósticas. Solo entonces decidir aceptación de G2 y alcance específico de G3.

**Límites de la revisión:** no se recompiló ni se ejecutaron Blender, Unity, Play Mode o builds. Los resultados de importación/deformación son evidencia publicada por Desarrollo, contrastada con scripts, archivos, hashes e imágenes. La consulta de checks de GitHub devolvió 403 por permisos; no se afirma que CI haya pasado.


## DEC-004 — Revisión documental de la primera corrección G2-CORR

**Fecha:** 28/10/2026.  
**Estado:** ENTREGA RECIBIDA, AVANCE TÉCNICO RECONOCIDO; **G2 AÚN NO APROBADA**.  
**Referencia vigente:** DEC-003. **Alcance permitido:** únicamente correcciones G2-CORR en la rama de trabajo. **G3/G4 no autorizadas**.  
**Tarea:** [ISEK-001](https://github.com/MarcoPolo2-0/Isekaid/issues/1).

### Evidencia revisada

- [PR privado #2 de Unity](https://github.com/MarcoPolo2-0/Isekaid-2026-10-27_13-09-18/pull/2), rama `feature/isek-001-g2-body`, encabezado de entrega `b329a3e510068ffef61c951143921cb3d9777d3b`. Continúa como **draft**, sin fusionar.
- [Informe G2-CORR](https://github.com/MarcoPolo2-0/Isekaid-2026-10-27_13-09-18/blob/feature/isek-001-g2-body/SourceArt/Companion/Woman/G2-CORR/INFORME.md) y `criteria-status.json`: métricas y pruebas declaradas por Chat 3. Los datos aquí se atribuyen a esa entrega; no se han repetido las ejecuciones de Blender/Unity en esta revisión.
- [Dirección artística v0.2](Specifications/ISEK-001_Direccion_Artistica_v0.2.md), commit `5e97e26ce362081fbc76bac95db8b9a0484017ba`: revisión sobre renders **G2-PROTO**, anterior a G2-CORR; por tanto, no constituye una evaluación visual de la nueva neutral.
- Los archivos gráficos de evidencia del PR privado se encuentran almacenados mediante **Git LFS**; la lectura disponible devolvió punteros LFS, no los píxeles completos. Esta Dirección no afirma haber inspeccionado las nuevas imágenes ni aprobado la silueta.

### Resultado de la segunda entrega

1. **Avance documental/técnico aceptado como evidencia de trabajo, no como cierre de G2.** Chat 3 declara 11,022 vértices preservados, seis Shape Keys para tres ejes, 27 combinaciones, 15 barridos de 101 muestras, Avatar Humanoid válido y controles sincronizados en un proyecto Unity aislado. AC03–07/AV01,03–04 permanecen pendientes de aceptación visual y verificación completa. AV02 registra comparabilidad técnica.
2. El diagnóstico de ropa mejora respecto de G2-PROTO: máxima penetración de **13.70 a 4.03 mm**, poses con penetración de **9/18 a 6/18**, vértices negativos acumulados **782 a 36**, pares de triángulos cruzados en 45 casos **3,927 a 2,596**. **No cumple todavía AC08**; el estiramiento máximo pasa de **3.578 a 3.584**, sin mejora.
3. La nueva neutral también requiere inspección artística: el propio informe advierte que el pecho puede percibirse demasiado plano y que persisten transiciones susceptibles de corrección. No se aprueban aún las formas neutras, los extremos ni el rango de cintura como aspecto de producción.
4. La importación aislada de Unity **no** acredita Play Mode, materiales finales, locomoción, prendas finales, persistencia o integración en escenas. AC09–15/AV05–10 siguen sin ejecutar y fuera de alcance.
5. **Decisión:** conservar abierto el PR #2, no fusionar a `main`, no actualizar el puntero del submódulo y no sustituir el maniquí de producción. **No iniciar G3 ni G4**.

### Instrucciones siguientes

**Chat 2 — Revisión visual de la versión G2-CORR (no de G2-PROTO).**

- Contrastar las cuatro vistas `G2-CORR/evidence/neutral-body-*.png`, `before-after.jpg`, los extremos y matrices actuales con la dirección A y R01–R03 de v0.2.
- Entregar **dictamen por AV01–AV04** sobre la neutral y amplitudes reales: diferencias visibles, fallos localizados, correcciones concretas y referencias a los archivos exactos.
- Si las vistas privadas no pueden evaluarse en el chat, solicitar al usuario que aporte una comparación exportada y accesible antes de decidir sobre la apariencia. No sustituir evidencia real por conceptos generados.
- Registrar como revisión propuesta v0.3 o informe acotado, sin aprobar arte ni modificar Unity.

**Chat 3 — Continuidad correctiva condicionada al dictamen visual.**

- Mantener el estado recuperable de G2-PROTO y G2-CORR; no reescribir originales.
- Antes de nuevas modificaciones anatómicas mayores, presentar las cuatro vistas y comparaciones actuales a Dirección/Chat 2. Una vez localizados defectos, corregir solo regiones concretas en la misma rama y PR.
- Seguir reduciendo las intersecciones en el diagnóstico de pesos/ropa sin inflar globalmente las prendas ni reducir silenciosamente rangos. Registrar tendencias y límites de pruebas.
- No implementar rostro, cabello, prendas finales, animaciones de producción, nuevos sistemas runtime o cambios de escenas, UI y perfil sin nueva autorización.

**Próxima puerta:** validación visual humana de neutral/extremos G2-CORR, cruzada con barridos y reporte de cruces residuales. La aprobación del usuario de la apariencia final continúa siendo indispensable.


## DEC-005 — Rechazo visual de G2-CORR por pérdida de silueta

**Fecha:** 28/10/2026. **Origen:** evaluación directa del usuario. **Estado:** G2-CORR NO aprobada artísticamente.

El usuario prefiere la silueta estilizada de G2-PROTO a la versión G2-CORR. La corrección redujo en exceso el volumen y contraste de formas del personaje. Se mantiene G2-PROTO como referencia de estilo, sin aprobar sus defectos geométricos anteriores. El objetivo es combinar la expresividad de la silueta previa con las mejoras de continuidad y deformación alcanzadas en G2-CORR. «Neutral» significa valor base de los controles, no simplificación de la silueta.

**Chat 2:** actualizar la guía artística con comparación de las cuatro vistas y una propuesta intermedia, identificada como concepto cuando corresponda. Presentarla para validación visual del usuario.

**Chat 3:** conservar los dos prototipos y la fuente original; elaborar una nueva neutral de cuatro vistas antes de ampliar el trabajo de skinning y ropa. Mantener el mismo PR sin fusionar y sin alterar escenas de producción. Recalcular controles y pruebas tras aprobación visual.

**Estado de puertas:** G2 requiere correcciones; G3/G4 permanecen sin autorización. La aprobación técnica parcial no sustituye la aceptación artística del usuario.


## DEC-006 — Repositorio único y limpieza de raíz Unity

**Fecha:** 28/10/2026. **Estado:** APROBADA — organización técnica vigente. Esta decisión sustituye exclusivamente la organización de repositorios de DEC-001/002/003/004 y del workflow v1.0; sus decisiones artísticas y técnicas históricas permanecen registradas.

- **Repositorio único:** `MarcoPolo2-0/Isekaid`, rama estable `main`. **Raíz real de Unity:** `UnityProjectIsekaid/` (contiene `Assets/`, `Packages/`, `ProjectSettings/`, `SourceArt/`). **No hay submódulos activos.** Las referencias al antiguo `Isekaidunity` y al repositorio privado son históricas, no instrucciones vigentes.
- Se retiran del estado actual del repositorio `Packages/`, `ProjectSettings/` y `.vsconfig` en la **raíz general**, por duplicar una instalación Unity anterior. Su contenido sigue recuperable en los commits previos. No se toca el contenido equivalente bajo `UnityProjectIsekaid/`.
- Se conserva en `UnityProjectIsekaid/Assets/_Project/` el prototipo original, sus tres escenas `CharacterCreation`, `Game` y `CompanionPrototype`, scripts, prefabs y archivos `.meta`. La lista de escenas de compilación vigente se encuentra en `UnityProjectIsekaid/ProjectSettings/EditorBuildSettings.asset`.
- Los recursos `G2-PROTO` y `G2-CORR` ya están versionados en el nuevo repositorio como **evidencia experimental**, no como aceptación artística ni autorización de reemplazar recursos del juego. **DEC-005 sigue vigente: G2-CORR rechazada visualmente, G2 en correcciones, G3/G4 sin autorización.** La presencia de estos archivos en `main` tras la migración no implica aprobación ni integración a escenas.
- Proceso vigente de tres chats, Git, ramas y PR: [Workflow de desarrollo v1.1](Workflow_Desarrollo_v1.1.md). Las entregas futuras usan este único repositorio, nombran rutas relativas a su raíz y enlazan al Issue ISEK-001.
- Se incorpora verificación automatizada **estática** de estructura, escena inicial y archivos esenciales; no certifica importación, Play Mode, compilación de C#, materiales ni builds en Unity. La validación en Unity 6000.6.7f1 sigue pendiente y debe informarse por separado.
- **Advertencia de distribución:** el repositorio actualmente es público y contiene código, imágenes y recursos de terceros; revisar licencias, atribución y credenciales antes de publicar nuevas dependencias o distribuir derivados.

**Próxima tarea:** abrir en Unity Hub `UnityProjectIsekaid/`, comprobar compilación/Play Mode del creador y aventura, preservar el prototipo y luego continuar la iteración artística G2 bajo DEC-005.

## DEC-007 — VRM 1.0 + catálogo local; Fase 2 autorizada

**Fecha:** 28/10/2026, Guatemala. **Autoridad:** instrucción directa del usuario como Chat 1 — Dirección.  
**Estado de diseño:** ADOPTADO como dirección principal. **Implementación F2:** AUTORIZADA, PENDIENTE DE ENTREGA Y ACEPTACIÓN.  
**Seguimiento:** [Fase 2 / Issue #5](https://github.com/MarcoPolo2-0/Isekaid/issues/5), [ISEK-001 / Issue #1](https://github.com/MarcoPolo2-0/Isekaid/issues/1).

### Motivos y decisión

Adoptar VRM 1.0 con importación desde archivo local y catálogo incluido para concentrar producción en una compañera viva y reutilizar herramientas externas de creación visual. Se deja de desarrollar por ahora el editor corporal propio. Esto reduce el alcance del editor que debemos producir, pero no acredita un ahorro medido ni elimina los costes de importación, compatibilidad, seguridad y soporte.

Identidad, personalidad, memoria, relación, clase, progreso y representación visual son independientes. Cambiar apariencia conserva los datos de la misma compañera. Se mantienen tsundere, yandere y tímida, con emociones, preferencias, autonomía, memoria, iniciativas y desacuerdos; el formato visual no sustituye el cerebro ni reduce la narrativa a expresiones.

Una apariencia predeterminada y una importada bastan para la primera entrega funcional. Tres apariencias incluidas son el objetivo posterior de catálogo. El juego funciona offline, sin necesitar VRoid Studio instalado, cuentas, MCP, modelos de lenguaje o APIs de pago. VRM 0.x queda pendiente y no se anuncia compatible.

### Evidencia de los PR fusionados

- [PR #3](https://github.com/MarcoPolo2-0/Isekaid/pull/3): especificación [VRM v0.2](Specifications/Sistema_Companera_VRM_v0.2.md), commit documental `bcb9adf8ea7423ec42e9895d444e8f5cb8457fb5`, fusión `c9531708e77a9684739b5d00b9e32b63d2be9d20`. Aprueba conceptualmente la dirección con ajustes; conserva los 26 criterios por fases, no resultados de ejecución.
- [PR #4](https://github.com/MarcoPolo2-0/Isekaid/pull/4): prueba VRM y corrección transaccional `9947c0c53c7e85bc9b95678902737b5a9a9dbd33`; encabezado `db8ec8c83a625053f8f80bc4e7572f7e7209c8eb`, fusión `480a8e99aef9c3c82db8bf78429ab4c57e32a4b7`. Confirmado fusionado mediante API; el informe conserva frases anteriores a la fusión como contexto histórico.
- [Informe VRM-POC](Audits/VRM-POC/INFORME.md): declara Play Mode en Unity 6000.6.7f1/URP 17.6.0/D3D11, UniVRM 0.131.4; carga local VRM 1.0, Humanoid, 14 presets faciales probados, seguimiento y clips sintéticos. Reemplazo transaccional, cinco fallos por etapa, seis ciclos, descarga idempotente y destrucción del propietario comprobados en el alcance descrito.
- [Estabilidad](Audits/VRM-POC/stability-results.json): 1138 referencias inspeccionadas liberadas; mayor crecimiento asentado informado de unos 0.250 MiB. La tolerancia de ensayo de 16 MiB no es presupuesto de producción. Persisten coexistencia transitoria de dos avatares y coste de decodificación; no se ha probado toda fuga, VRAM o sesiones prolongadas.

Esta Dirección adopta la viabilidad acotada documentada. La redacción de DEC-007 no vuelve a ejecutar esos ensayos ni convierte el POC en un creador funcional. La fusión conserva las pruebas experimentales, no acredita integración final, seguridad total o builds validadas.

### Autorización acotada Fase 2

Chat 3 puede continuar desde la v0.2 fusionada: selección local de apariencia predeterminada/importada, previsualización de cuerpo/rostro, tres personalidades independientes, carga/error/cancelación, copia gestionada, cambio visual seguro, conservación de identidad/estado y migración del perfil v1. Entregar evidencia real de guardado, recuperación, offline, 1920×1080/1280×720 e intermedias, controles accesibles y pruebas de regresión.

El [contrato funcional F2](Specifications/VRM_Fase2_Contrato_Funcional_v0.1.md) y la [matriz](Specifications/VRM_F2_F3_Matriz_Trazabilidad_v0.1.md) detallan la entrega. Las precisiones nuevas identificadas F2-P01–P04 son propuestas en revisión; no bloquean trabajo autorizado ni se aplican por estar en un PR sin fusionar. La preparación del [borrador F3](Specifications/VRM_Fase3_Personalidad_Contextual_v0.1_BORRADOR.md) es documental; no autoriza implementar mecánicas nuevas o F3 completa.

### Conservación y pendientes

- Woman, G2-PROTO y G2-CORR se conservan como recursos históricos. Nuevas correcciones corporales quedan pausadas. Esta decisión sustituye la continuación activa del editor y correcciones de DEC-001–005, no reescribe sus resultados ni aprueba sus defectos.
- [ISEK-001 permanece abierta](Specifications/ISEK-001_Estado_DEC007.md): el alcance corporal está pausado y su objetivo visual está siendo sustituido por VRM. No cerrar casos antiguos como superados por cambiar de estrategia.
- Se conserva la organización unificada de DEC-006. El [workflow v1.2](Workflow_Desarrollo_v1.2.md) actualiza la siguiente puerta y responsabilidades; no altera el historial v1.0/v1.1.
- Separar restricciones técnicas, permisos declarados y derechos sujetos a revisión humana. El modelo privado del POC tiene condiciones que no acreditan su redistribución o uso comercial; ni modelo ni capturas se publican como catálogo por pasar pruebas. El catálogo necesita recursos con procedencia y permisos comprobados por el equipo.
- Pendientes: integración funcional; guardado/migración y fallos reales; cobertura de datos hostiles y cancelación lógica; presupuesto por plataforma, rendimiento sostenido y memoria en reemplazo; validación de builds; derechos de cada recurso y aprobación visual humana del catálogo.
- GDD v0.5 permanece intacto como histórico aprobado. El borrador v0.6 incorpora enmiendas de personalización claramente diferenciadas; no se aprueba todo el documento.

**Próxima aceptación:** informe y evidencias de F2 por Chat 3; revisión de Dirección antes de declarar terminada la fase. Ni la autorización ni este PR completan F2, ISEK-001 o ISEK-002. No fusionar la entrega documental automáticamente.

## DEC-008 — Aprobación documental del GDD v0.7

**Fecha:** 29/10/2026, Guatemala. **Autoridad:** instrucción directa de Dirección/usuario, conforme con las auditorías editorial de Chat 2 y técnica de Chat 3.  
**Estado:** APROBACIÓN DOCUMENTAL; publicación del maestro mediante PR independiente. No se ordena fusión automática.

- Se adopta [GameDesignDocument_v0.7.md](GameDesignDocument_v0.7.md) como documento maestro oficial. Parte del borrador revisado de PR #13, corte `0180b8b35f59a0a5f4357bb14549066b33a03ddc`, incorporado a main por `24cad11`.
- Se conservan íntegros [v0.5](GameDesignDocument_v0.5.md), [v0.6](GameDesignDocument_v0.6_borrador.md) y [borrador v0.7](GameDesignDocument_v0.7_borrador.md). No se renombra, elimina o sobrescribe el borrador; se crea un archivo oficial nuevo.
- La promoción cambia portada, vigencia, trazabilidad y registro administrativo. Las secciones de diseño §§1–18 se preservan; las notas previas mantienen su carácter histórico.
- La aprobación del documento **no aprueba automáticamente** contenidos PROPUESTOS/PENDIENTES: Deredere, nuevas mecánicas, datos biográficos, integración N1, actuación N2 o parámetros de balance no ratificados. No convierte evidencia técnica o manual parcial en aceptación completa de F2/F3A, catálogo o arte final.
- [PR #14](https://github.com/MarcoPolo2-0/Isekaid/pull/14) sigue en revisión creativa: nombres Nera/Liora/Sira provisionales y 30 cambios recomendados sobre el conjunto de 33 revisiones. Dirección seleccionará los IDs y rasgos que autorice antes de solicitar a Desarrollo integración; no se aplica el parche ni se fusiona #14 desde esta decisión.
- Se actualizan README y la referencia operativa del workflow v1.2. No se modifican Unity, biblioteca canónica, recurso runtime, serialización o pruebas originales. Los cambios se añaden a readme sin borrar historial.

**Límite:** esta decisión oficializa el documento; no marca ISEK-001/F3A completados ni amplía la autorización de implementación. La incorporación física a main se entrega mediante PR para revisión.
