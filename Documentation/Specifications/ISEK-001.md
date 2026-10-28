# ISEK-001 — Compañera anime personalizable a partir de woman.fbx

**Versión:** 0.1  
**Fecha:** 28/10/2026, Guatemala.  
**Estado de la especificación:** PROPUESTA, pendiente de revisión de Dirección.  
**Responsable:** Chat 2 — Game Design.  
**Decisión vinculante:** [DEC-001](../DECISIONS.md).  
**Base aprobada:** [GDD v0.5](../GameDesignDocument_v0.5.md).  
**Referencia de aceptación general:** [Compañera v0.1](../Especificacion_Companera_v0.1.md), todavía PROPUESTA.  
**Workflow vigente:** [Desarrollo v1.1](../Workflow_Desarrollo_v1.1.md). El v1.0 se conserva como historial.

**Estado posterior — DEC-003:** G2 requiere correcciones; solo G2-CORR está autorizada. A es referencia de trabajo, no arte final aceptado. La [revisión artística v0.2](ISEK-001_Direccion_Artistica_v0.2.md) localiza correcciones y revisa AV01–04 sobre la evidencia real de G2-PROTO; sus precisiones adicionales permanecen como propuestas. G3/G4 e integración siguen pendientes de Dirección.

**Revisión vigente — DEC-002, 28/10/2026:** Dirección aceptó la evaluación técnica G1, la factibilidad experimental de tres ejes y este marco de cuatro puertas/16 criterios; autorizó únicamente G2-PROTO. La apariencia neutral, los extremos y los detalles propuestos siguen pendientes de aceptación artística. Los estados y pruebas pendientes descritos abajo registran la entrega original; consultar [DEC-002](../DECISIONS.md) y la [auditoría técnica](../Audits/ISEK-001/INFORME.md) para los resultados posteriores. La [propuesta artística v0.1](ISEK-001_Direccion_Artistica_v0.1.md) reúne alternativas y referencias para la nueva revisión. Este aviso no aprueba la especificación completa ni autoriza G3/G4.

La elección de la base corporal está aprobada en DEC-001. Esta versión concreta su dirección visual, controles, pruebas y entregables; no declara aprobados sus detalles nuevos ni completada la tarea. Dirección revisa la especificación y Desarrollo informa la viabilidad técnica antes de sustituir al personaje actual.

## 1. Objetivo y resultado esperado

Convertir la base `woman.fbx` en una compañera femenina adulta de estilo anime 3D que pueda personalizarse, vestirse y moverse de forma coherente dentro del creador y la aventura existentes. Aprovechar la malla para reducir trabajo inicial sin convertir su forma actual en un límite permanente del diseño.

El resultado debe conservar una identidad facial reconocible en diferentes proporciones y personalidades. El cuerpo base tendrá proporciones equilibradas; cabello largo suelto, top y pantalón sencillos; colores independientes de cabello, iris, top y pantalón; ajustes independientes de pecho y caderas. Cintura se añade únicamente si supera la evaluación prevista en DEC-001.

Esta tarea cubre la base visual y sus pruebas de expresión/movimiento. No implementa conversación, memoria, autonomía completa, clases, misiones, hogar ni romance. Se conservan los requisitos de esos sistemas en el GDD; las pruebas expresivas de ISEK-001 preparan su integración posterior.

## 2. Fuente y evidencia disponible

| Elemento | Estado comprobado en esta revisión |
|---|---|
| Archivo fuente | [assets/woman.fbx](../../assets/woman.fbx), en el repositorio principal |
| Tamaño | 639 772 bytes |
| Cabecera | FBX binario, versión 7400; esto identifica el formato, no su compatibilidad efectiva |
| SHA-256 | `674C07FF612577A1A5B7E7EE124202722FA5EB0F68BDC01C820A875B5BEF1D87` |
| Procedencia registrada | DEC-001 identifica [Woman de Fly_jkey en Sketchfab](https://sketchfab.com/3d-models/woman-b303b7606140480f8335cda121b9607c) como candidato principal |
| Licencia | DEC-001 registra CC BY. La página devolvió HTTP 403 al intentar verificarla; no se ha confirmado aquí la versión exacta ni se presenta como resuelta la atribución |
| Malla, rig, UV, materiales y Shape Keys | No inspeccionados en Blender durante esta revisión; no se asume su presencia, calidad o ausencia |
| Integración en Unity | No realizada ni probada por esta entrega de diseño |

Conservar el FBX original sin alterarlo. Desarrollo debe registrar su hash antes de trabajar y producir una fuente editable derivada. Si la malla no resulta viable, presentar el problema y las alternativas a Dirección; no reemplazarla automáticamente por alguno de los otros paquetes disponibles.

## 3. Acuerdos y propuestas

| Estado | Requisito |
|---|---|
| Aprobado, DEC-001 | Usar `woman.fbx` como candidato principal y verificar su viabilidad antes de reemplazar el maniquí |
| Aprobado, GDD/DEC-001 | Personaje adulto anime, rostro y ojos detallados, cabello largo suelto, top y pantalón, rig y animación compatibles |
| Aprobado, GDD/DEC-001 | Pecho y caderas independientes; ropa coherente con sus variaciones; guardar y recuperar valores |
| Condicionado, DEC-001 | Cintura independiente, sujeta a topología y animación; su omisión definitiva requiere decisión explícita |
| Aprobado, GDD | Apariencia, personalidad y clase independientes; rostro normal y cómico; funcionamiento sin conexión |
| Propuesto aquí | Silueta neutral, extremos visuales, estructura del editor, matriz de poses y primera batería de expresiones descritas abajo |

Los criterios verifican acuerdos y propuestas; superarlos no convierte por sí solo una propuesta en decisión aprobada. La aceptación artística y la autorización de sustitución corresponden a Dirección, con aprobación final del usuario.

## 4. Dirección visual y referencias

### 4.1 Silueta neutral propuesta

Mujer claramente adulta, de constitución promedio y postura relajada. Mantener equilibrio entre cabeza, torso, pelvis y piernas; volumen moderado en pecho y caderas y una transición de cintura natural. No depender de una postura arqueada, un giro de pelvis o una perspectiva favorable para lograr la silueta.

Propuesta de lenguaje anime: cabeza ligeramente estilizada, ojos expresivos, nariz y boca simplificadas, planos limpios y sombreado por zonas. La proporción de cabeza respecto del cuerpo se decide en las vistas de referencia, sin imponer una escala física o un porcentaje de remodelado antes de ver la malla.

Conservar del FBX solo lo que sirva después de inspeccionarlo: conectividad, volúmenes, simetría y zonas reutilizables. El resultado no tiene que mantener un rostro incompatible con la estética anime. Rediseñar cara, cabello o ropa dentro del personaje derivado es distinto de sustituir la base por otro recurso.

### 4.2 Láminas y vistas requeridas

| Referencia | Función | Estado |
|---|---|---|
| [Lámina v01](../references/companion-concept-v01.png) | Orientación de cabello, ropa sencilla y contraste entre expresión normal y cómica | Candidata; no fija todavía rostro, silueta o paleta final |
| Turnaround del FBX original | Mostrar qué geometría existe realmente antes de modificarla | Pendiente de Desarrollo; frontal, perfil y espalda, misma escala, vista ortográfica |
| Turnaround de la propuesta neutral | Aprobar la dirección del cuerpo y rostro derivados | Pendiente; mismas cámaras que el original y tres cuartos adicional |
| Matriz de siluetas | Comparar extremos de pecho/caderas y cintura si es viable | Pendiente; etiquetar cada combinación y mantener cámara/pose |
| Capturas en Unity | Comprobar apariencia y movimiento reales en creador y aventura | Pendiente; un render de Blender no las sustituye |

La lámina v01 y las imágenes `candidate-01.png`/`candidate-02.png` existentes no son evidencia de la estructura interna de `woman.fbx`. La evidencia de esta tarea debe identificar el archivo y la versión del modelo evaluado.

### 4.3 Cabello, ropa y materiales

**Propuesta:** cabello largo hasta la zona lumbar, suelto y organizado en mechones legibles; frente y cejas visibles para leer expresiones. Evitar volumen de cabello que tape permanentemente ojos o boca al girar. El movimiento secundario puede ser sencillo, siempre que no atraviese de forma persistente hombros y espalda.

Top sencillo con mangas cortas y pantalón largo de movilidad práctica; el corte final sigue pendiente de aprobación visual. Mantener grandes zonas de color y pocos adornos para facilitar personalización. La ropa deberá tener volumen, grosor o tratamiento equivalente suficiente para evitar huecos y superficies que desaparezcan al rotar.

Materiales anime coherentes entre piel, cabello, iris y tela. Los tintes no reemplazan pupila, reflejos, costuras ni sombras necesarias para reconocer la forma. La paleta de la lámina es provisional; ningún color asigna una personalidad.

## 5. Personalización corporal

### 5.1 Controles y límites propuestos

Usar valores normalizados de 0 a 1 con neutral en 0.5, compatibles conceptualmente con la propuesta general. Son valores del perfil, no centímetros ni un porcentaje uniforme de escala. El mínimo y el máximo corresponden a formas creadas y revisadas; el valor intermedio debe interpolarse sin saltos visibles.

| Control | Extremo bajo propuesto | Neutral propuesto | Extremo alto propuesto | Invariantes |
|---|---|---|---|---|
| Pecho | Menor volumen y proyección, conservando continuidad del torso | Volumen moderado integrado en el tórax | Mayor volumen dentro de una silueta adulta equilibrada | No cambia altura, hombros, longitud de brazos, cintura ni postura de columna |
| Caderas | Menor anchura lateral de pelvis y transición superior del muslo | Anchura media, transición suave | Mayor anchura lateral con transición continua al muslo | No estira las piernas, desplaza pies ni ensancha todo el torso |
| Cintura, condicionada | Zona media más estrecha, sin pinzamiento | Transición natural entre tórax y pelvis | Zona media más ancha, sin aplanar el torso | No modifica los valores de pecho/caderas ni desplaza el esqueleto completo |

No se aprueban extremos exagerados ni medidas absolutas sin revisión de las siluetas. Los dos extremos deben ser perceptiblemente distintos y útiles; un slider que apenas cambia la imagen no cumple. Dirección decide el rango final con la matriz visible, no solo con números.

Los controles son independientes en datos y en intención visual. Pueden necesitar correcciones compartidas en las zonas de unión para evitar defectos; esto no debe alterar silenciosamente el valor de otro control ni desplazar una parte distante del cuerpo.

### 5.2 Ropa y deformación

Preferir Shape Keys/Blend Shapes cuando la evaluación técnica demuestre que funcionan. No resolver la personalización escalando todo el personaje ni deformando solo el cuerpo debajo de ropa inmóvil. El método final de ajuste de ropa lo elige Desarrollo; el resultado exigido es que top, pantalón y puntos de unión acompañen cada combinación admitida.

Riesgos a revisar: borde del top sobre cintura/caderas; axilas con brazos arriba; entrepierna al sentarse o correr; rodillas al flexionar; zona lumbar con giro; cuello y hombros bajo cabello. Si hay una incompatibilidad, corregirla o presentar un cambio de alcance; no ocultar un extremo del editor sin comunicarlo.

### 5.3 Matriz corporal

Probar 0, 0.5 y 1 por eje. Con pecho y caderas hay **9 combinaciones**. Si cintura se habilita, hay **27 combinaciones**. Además, recorrer cada slider continuamente manteniendo los otros controles en neutral y en una combinación extrema.

Revisar cada combinación en frontal, perfil y espalda con pose neutral, y en seis poses críticas: brazos elevados, brazos hacia delante, giro de torso, sentada, flexión profunda de piernas y zancada amplia. Son 54 o 162 configuraciones de pose, respectivamente. Pueden capturarse como secuencias ordenadas; no se exige editar a mano una imagen por configuración.

Las pruebas estáticas no sustituyen caminar/correr con las combinaciones más exigentes. Registrar exactamente los controles que producen cada defecto para poder reproducirlo.

## 6. Creador y datos del perfil

### Comportamiento propuesto

1. Abrir el creador con el perfil guardado o con valores neutrales si es una creación nueva.
2. Mostrar cuerpo completo, giro y una vista de rostro. Los controles no deben cubrir las zonas que modifican.
3. Separar visualmente color, proporciones y personalidad. Describir cada ajuste con nombre y estado, sin depender solo de una muestra de color.
4. Previsualizar cada cambio de forma inmediata. “Restablecer proporciones” vuelve al neutral del borrador, conservando colores y personalidad.
5. Guardar antes de entrar en la aventura. Si guardar falla, conservar el borrador y mostrar el error; no afirmar que se guardó.
6. Al volver al creador, conservar la apariencia. No usar esta operación para borrar recuerdos, relación o progreso cuando dichos sistemas estén disponibles.

Cintura se muestra únicamente cuando exista una implementación viable y aprobada. Si todavía está pendiente, no ofrecer un control sin efecto. Debe constar como pendiente en el informe y permanecer abierta su aceptación condicionada.

### Datos mínimos

| Dato | Regla |
|---|---|
| Versión del perfil | Identifica formato y migración; no confundir con versión del asset |
| Identidad/versión del modelo derivado | Permite interpretar los rangos y detectar incompatibilidad |
| Pecho y caderas | Valores finitos dentro del rango soportado; neutral definido por la versión |
| Cintura | Valor solo cuando el recurso la soporte; no inventar soporte por un campo serializado |
| Colores | Cabello, iris, top y pantalón independientes |
| Personalidad | Dato separado; cambiarlo no altera proporciones ni materiales |
| Estado de edición | Perfil confirmado, borrador y resultado de guardado |

El perfil actual v1 carece de ajustes corporales. **Propuesta de migración:** mantener sus colores y personalidad y asignar neutral a los nuevos ejes soportados. Datos corruptos o versiones desconocidas no deben sobrescribir automáticamente el último perfil válido. La persistencia de recuerdos está fuera de ISEK-001, pero esta tarea no debe impedirla ni borrar sus datos.

## 7. Rostro, personalidad y animaciones mínimas

### Rostro

Mantener el requisito de sistema híbrido de la v0.5. Propuesta mínima para esta tarea: neutral atenta, alegría/alivio, molestia, preocupación y vergüenza; más sorpresa cómica y una segunda reacción cómica de irritación o vergüenza. Esta batería no elimina las expresiones adicionales previstas en la especificación general.

La expresión debe leerse frontalmente y a tres cuartos, con parpadeo y mirada. Una expresión cómica sustituye las partes normales necesarias; no añade un segundo par de ojos ni deja marcas al terminar. Una señal de peligro debe cancelar la comedia y mostrar una reacción compatible. Si la señal es de ensayo, describirla como tal: no demuestra percepción ni comportamiento autónomo.

No se presupone que el FBX traiga ojos separados, boca interior, rig facial o Shape Keys. Su existencia y la solución necesaria pertenecen al informe técnico de la primera puerta.

### Animación y lenguaje corporal

| Recurso mínimo propuesto | Qué demostrar |
|---|---|
| Reposo, caminar y correr | Deformación coherente, pies estables y continuidad entre velocidades |
| Inicio/parada y giro | Recuperar reposo sin saltos de pose ni deslizamiento persistente |
| Sentarse y levantarse | Compatibilidad de caderas, cintura, top y pantalón en cambios amplios de postura |
| Mirar al interlocutor y apartar la mirada | Cara legible, cuello y cabello sin deformación grave |
| Reacción breve de agradecimiento y de preocupación | Sincronía entre expresión, postura y motivo de la reacción |
| Prueba de postura incapacitada | Compatibilidad con suelo y rig; no representa por sí sola animación de caída ni combate terminado |

Para personalidad, usar la misma apariencia en tres muestras: tsundere con reserva y reconocimiento breve; yandere con atención sostenida sin devoción instantánea; tímida con contacto visual intermitente pero intención clara. Son propuestas de dirección corporal, no personalidades físicamente distintas. El movimiento de supervivencia debe poder interrumpir un gesto social.

## 8. Puertas técnicas y decisiones de continuidad

| Puerta de DEC-001 | Comprobación de Desarrollo | Evidencia esperada | Condición para continuar |
|---|---|---|---|
| G1 — Base viable | Importación del FBX; geometría editable; escala/orientación; topología de cara, hombros, caderas y rodillas; UV/materiales; existencia o ausencia de rig/Shape Keys | Turnaround real, vista de geometría, inventario y archivo editable de prueba | Informe declara qué sirve, qué debe corregirse y qué impide continuar; Dirección revisa la viabilidad |
| G2 — Ajustes independientes | Neutral y extremos de pecho/caderas; cintura si es viable; barridos y combinaciones | Matriz de 9/27 formas con parámetros y defectos identificados | Rango visible aceptado y sin defectos graves; cintura se acepta, se corrige o queda pendiente mediante decisión explícita |
| G3 — Rig, ropa y movimiento | Pesos, retargeting compatible, poses críticas, transiciones y ropa ajustable | Clips, matriz de poses y fuente editable reproducible | Sin clipping persistente ni deformaciones que impidan uso; fallos reproducibles corregidos |
| G4 — Personaje anime integrado | Creador y aventura con mismo modelo, perfil, materiales, rostro y animación | Capturas y recorrido real en Unity; pruebas de guardado, migración y cambios de escena | Cumple criterios funcionales y recibe aceptación artística; no basta importar un modelo |

Si falta rig o Shape Keys, no se deduce que la base sea inviable: el informe debe estimar el trabajo necesario. Si el personaje necesita reconstrucción sustancial, no afirmar que el uso del FBX ya ahorró tiempo. Si una puerta falla, corregirla o plantear a Dirección la alternativa con evidencia; no cambiar de modelo por iniciativa propia.

## 9. Criterios de aceptación

Todos los casos están **PENDIENTES DE EJECUCIÓN** para esta especificación. “Base seleccionada”, “archivo existente” y “tarea terminada” son estados distintos.

| ID | Prueba y resultado esperado | Puerta |
|---|---|---|
| ISEK-001-AC01 | Importar el original sin modificarlo y producir una fuente editable que se pueda cerrar y volver a abrir conservando geometría | G1 |
| ISEK-001-AC02 | Inventariar mallas, rig, Shape Keys, UV y materiales con capturas reales; declarar ausencias y necesidades sin inferirlas del nombre del archivo | G1 |
| ISEK-001-AC03 | Presentar neutral frontal/perfil/espalda/tres cuartos; Dirección reconoce adultez, equilibrio y dirección anime de la propuesta | G2 |
| ISEK-001-AC04 | Recorrer pecho manteniendo caderas/cintura neutrales; cambio visible de volumen sin cambiar altura, extremidades o valores ajenos | G2 |
| ISEK-001-AC05 | Recorrer caderas manteniendo el resto neutral; cambio localizado con transición al muslo, sin estirar piernas ni desplazar pies | G2 |
| ISEK-001-AC06 | Si cintura es viable, demostrar ajuste localizado e independiente. Si no lo es, adjuntar evidencia y decisión pendiente; no marcar este caso como aprobado | G2 |
| ISEK-001-AC07 | Completar matriz de 9/27 formas y barridos sin saltos, huecos ni deformaciones graves; parámetros de cada muestra identificables | G2 |
| ISEK-001-AC08 | Repetir las seis poses críticas con cada combinación; top y pantalón cubren de forma coherente, sin penetración persistente visible | G3 |
| ISEK-001-AC09 | Caminar, correr, parar, girar, sentarse y levantarse con neutral y extremos exigentes; preservar volumen y contacto de pies | G3 |
| ISEK-001-AC10 | Usar el personaje en creador y aventura con iguales proporciones y materiales; sin recursos faltantes ni referencias rotas | G4 |
| ISEK-001-AC11 | Cambiar cada color afecta solo a su zona; iris conserva pupila y detalle; personalidad no cambia apariencia | G4 |
| ISEK-001-AC12 | Guardar, cerrar, abrir y transitar entre escenas recupera controles y aspecto; fallo de escritura no se informa como éxito | G4 |
| ISEK-001-AC13 | Migrar un perfil v1 válido conservando sus selecciones e introducir neutral; datos inválidos no destruyen el último perfil válido | G4 |
| ISEK-001-AC14 | Mostrar batería facial normal/cómica con mirada y parpadeo; cancelar comedia por señal de peligro y al cambiar de escena sin residuos | G4 |
| ISEK-001-AC15 | Comparar tres muestras de personalidad con la misma cara, cuerpo y ropa: diferencias de mirada/gesto legibles sin asignar anatomías distintas | G4 |
| ISEK-001-AC16 | Entregar fuente editable, exportación, recursos y procedimiento reproducible; declarar pendientes, procedencia y condiciones de uso verificadas | G4 |

Para ISEK-001-AC08/09, evaluar a tamaño de juego y en primer plano de las uniones críticas. Un defecto visible de forma repetida o sostenida bloquea la aceptación; no se oculta mediante iluminación, cámara o reducción silenciosa de rango. El rostro y la silueta requieren aprobación visual humana aunque todos los datos sean válidos.

Cada resultado debe identificar versión, controles, pose/clip, cámara, iluminación, equipo, resultado observado y archivo de evidencia. Una captura aislada no acredita animación. Compilación e importación no acreditan apariencia ni deformación correctas.

## 10. Entrega y revisión

Desarrollo entregará un informe de ISEK-001 con estado por puerta y por criterio, archivos creados/modificados, pruebas realizadas, capturas/clips, defectos y límites. Conservar el original en `assets/` y el recurso editable/importado y el código en `UnityProjectIsekaid/` del mismo repositorio, según el workflow v1.1. Identificar el estado experimental, verificar licencias y evitar integrar en escenas hasta aprobación.

Las fuentes editables y exportaciones deben indicar su versión y correspondencia. La evidencia compartida se ubicará según su contenido y permiso de publicación; no asumir que todos los recursos de terceros pueden copiarse al repositorio público. La atribución exacta de la base se resuelve antes de distribuir el derivado.

Dirección puede aceptar una puerta de viabilidad sin declarar completo el personaje. El reemplazo en la escena requiere alcance autorizado y evidencia suficiente; conservar una versión recuperable del prototipo. No cambiar el puntero del submódulo para presentar un experimento como versión aceptada.

### Pendientes para Dirección

| Punto | Decisión solicitada | Qué se conserva de los acuerdos |
|---|---|---|
| ISEK-001-P01 | Aprobar o ajustar la dirección de silueta y extremos visuales de §§4–5; su aceptación artística final necesita vistas reales del derivado | Adultez, apariencia neutral equilibrada y ajustes independientes de pecho/caderas |
| ISEK-001-P02 | Aprobar los controles normalizados, neutral 0.5, comportamiento del creador y migración propuestos en §§5–6 | Persistencia, independencia de apariencia/personalidad y ropa compatible |
| ISEK-001-P03 | Aprobar o ajustar la batería de expresiones, lenguaje corporal y animaciones mínimas de §7 | Estética anime, sistema facial híbrido y diferencias expresivas por personalidad |
| ISEK-001-P04 | Aprobar el alcance de las cuatro puertas y los 16 criterios, o autorizar únicamente una puerta identificada | No sustituir el maniquí ni declarar la tarea completada sin evidencia suficiente |

**Dependencias por resolver:** evaluar cintura con el informe técnico, sin eliminarla por omisión; fijar plataforma y equipo objetivo antes de establecer presupuesto final de malla, texturas y rendimiento; completar la verificación de procedencia y atribución. Cintura sigue siendo una decisión condicionada de DEC-001, no una nueva aprobación técnica concedida por esta especificación.

Registrar la revisión contra **ISEK-001 v0.1** y los puntos anteriores. Aprobar una puerta o un punto no aprueba automáticamente la especificación general ni el GDD v0.6. Hasta recibir esa revisión, esta entrega conserva el estado **PROPUESTA**.

### Registro

**0.1 — 28/10/2026:** preparada la especificación de ISEK-001 conforme a DEC-001 y al archivo `woman.fbx` añadido por el usuario. Define silueta, controles, ropa, datos, animaciones, cuatro puertas y 16 criterios. Se verificaron presencia, tamaño, cabecera y hash del archivo; no se importó, modeló ni validó en Blender/Unity. No se han modificado decisiones aprobadas.
