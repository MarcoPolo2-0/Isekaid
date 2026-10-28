# VRM — Fase 2: contrato funcional del creador y persistencia

**Versión:** 0.1 · **Fecha:** 28/10/2026. **Responsable:** Chat 2.  
**Estado:** F2 AUTORIZADA por [DEC-007](../DECISIONS.md); este detalle documental se entrega en PR para revisión. Casos sin ejecutar aquí.  
**Base ya fusionada:** [VRM v0.2](Sistema_Companera_VRM_v0.2.md), PR #3; [POC](../Audits/VRM-POC/INFORME.md), PR #4.  
**Seguimiento:** [Issue F2 #5](https://github.com/MarcoPolo2-0/Isekaid/issues/5). [Matriz completa](VRM_F2_F3_Matriz_Trazabilidad_v0.1.md).

## 1. Alcance y coordinación

Entregar el flujo funcional de apariencia: una compañera predeterminada incluida y una importada desde archivo VRM 1.0 local, vista previa, selección independiente de tres personalidades, confirmación, guardado y recuperación. Conservar clase/identidad/recuerdos/relación/progreso al cambiar únicamente el visual. Funcionar offline sin instalar VRoid Studio.

Chat 3 ya puede implementar desde v0.2; este PR no es un bloqueo. Las precisiones F2-P01–P04 del §12 son PROPUESTAS: si alteran reglas importantes respecto de su implementación, informar a Dirección antes de aplicarlas. No implican construir gameplay F3 o catálogo F4. No modificar Woman/G2 ni usar sus controles corporales en el nuevo creador.

**Cierre funcional F2:** el jugador entra por catálogo o importación, confirma, inicia la aventura, vuelve/cierra y recupera la misma compañera; puede cambiar su visual en momento seguro sin perder datos. Requiere evidencia real y revisión. Cambiar malla en una escena experimental no satisface este contrato.

## 2. Estado actual observado y dependencias

Inspección de fuentes fusionadas en main 480a8e99aef9c3c82db8bf78429ab4c57e32a4b7:

- CompanionProfile v1: version=1, personality, hair, eyes, top, trousers. Colores 0–3; enum Tsundere=0, Yandere=1, Shy=2. CompanionProfileStore utiliza isekaid.companion.profile.v1 y devuelve un perfil predeterminado ante datos inválidos. Eso no equivale a una migración que preserve el original.
- CompanionState mantiene estado vital, un recuerdo booleano del primer rescate, Trust y contador de incapacitación. No hay en ese perfil persistencia completa de recuerdos/relación/clase; F2 no debe inventar que ya existe.
- POC: reemplazo visual transaccional y limpieza comprobados en ensayos acotados; cancelación del propietario descarta/libera el resultado al concluir el SDK. No interrumpe inmediatamente la decodificación ni prueba todos los fallos internos.
- VRM privado de prueba y capturas no autorizados como catálogo distribuible. El catálogo requiere un recurso con procedencia/derechos revisados. Ese recurso no se selecciona mediante esta especificación.

Desarrollo debe inspeccionar consumidores y guardado actual antes de integrar. Reutilizar raíces de actor, navegación y estado; solo sustituir representación validada. No imponer nombres de managers nuevos ni rehacer arquitectura por este contrato.

## 3. Pantallas y estados observables

| Paso | Contenido y acciones | Salida |
|---|---|---|
| Apariencia | Tarjeta de apariencia incluida; botones “Usar apariencia”, “Importar VRM”, “Volver” | Selección válida pasa a vista previa; cancelar selector no cambia borrador |
| Preparación | “Leyendo archivo”, “Comprobando compatibilidad”, “Preparando vista previa”; “Cancelar” | Candidato listo o error recuperable; anterior sigue confirmado |
| Vista previa | Cuerpo/rostro, rotación y restablecer vista; “Reposo”, “Caminar”, “Alegría”, “Molestia”, “Preocupación”; capacidades y avisos | “Continuar” si compatible y uso resuelto; “Elegir otra” conserva identidad y descarta candidato |
| Personalidad | Tsundere, Yandere y Tímida, explicación y muestras contextuales sobre mismo avatar | “Continuar”, “Volver”; ninguna tarjeta visual cambia la plantilla |
| Resumen | Apariencia/capacidades, personalidad, clase existente o “Clase aún no elegida”; créditos/uso | “Comenzar aventura” en creación o “Guardar apariencia” en cambio |
| Guardado | “Guardando compañera…”; sin doble envío ni navegación que genere otro guardado | Éxito inicia/reanuda; fallo recupera datos anteriores y permite reintentar |
| Recuperación | Avatar faltante/dañado; “Volver a importar”, “Elegir apariencia incluida”, “Volver” | No recrear compañera, no eliminar archivo de partida |

Clase no se elige por ropa o personalidad. Conservar la ya asignada; una clase todavía inexistente queda sin asignación y pendiente de su flujo aprobado, sin crear una nueva pantalla de combate en F2. Nombre del archivo/autor no cambia el nombre narrativo.

**Máquina de carga:** Selección → Lectura → Validación → Preparación → Previsualización → Confirmación → Guardado → Confirmado. Lectura/validación/preparación pueden terminar en Cancelando o Error. “Cancelando” puede durar hasta que el SDK libere el candidato; mostrar “Estamos descartando el avatar…”. No informar liberación inmediata si aún quedan recursos procesándose.

Una sola importación activa. Mientras prepara/cancela, bloquear nueva selección/importación y explicar el estado. Tras terminar, elegir un segundo candidato es posible. Resultado tardío de una operación cancelada o pantalla cerrada se libera y nunca reemplaza la selección actual. Esta serialización precisa la regla v0.2 de último candidato confirmable; no requiere importaciones concurrentes.

En nueva creación no hay compañera persistente antes de confirmar. En cambio de apariencia, el actor/partida y su visual anterior siguen válidos durante preparación. Borrador y confirmado nunca se confunden.

## 4. Selección, previsualización y compatibilidad

### Catálogo inicial

Una entrada visible y utilizable offline, incluida en la instalación. Datos: identificador y versión de apariencia, nombre de tarjeta, recurso local, autor/procedencia, licencia/condiciones revisadas, créditos, miniatura local, capacidades expresivas verificadas y validación de rig/materiales/equipo de las clases habilitadas. No colocar un botón “próximamente” como si satisficiera el catálogo.

Dirección visual: compañera adulta anime, identidad facial legible, silueta estilizada con carácter (DEC-005), cabello/vestuario compatibles con la dirección aprobada. La apariencia incluida debe demostrar normal atenta, alegría, molestia/preocupación, parpadeo y gesto de prueba, o declarar explícitamente la carencia pendiente de aprobación. La cobertura cómica completa de las tres entradas sigue siendo objetivo F4; una limitación inicial no se declara cobertura terminada.

Antes de distribución: revisión humana de derechos del modelo y partes, permiso de inclusión en juego/paquetes y usos previstos, créditos accesibles. No convertir el avatar privado del POC en predeterminado ni publicar sus capturas por conveniencia.

### Archivo importado

Selector local de la plataforma aprobada, filtro VRM sin tratar extensión como validación. No soportar 0.x por conversión silenciosa. Preservar bytes fuente; guardar copia gestionada solo cuando se permita el uso y se confirme la operación.

La ficha distingue tres ejes:
1. **Técnica:** compatible / con límites / incompatible, con funciones y razón.
2. **Permisos declarados:** autor, condiciones/ausencias y texto relevante del archivo; sin afirmar titularidad.
3. **Revisión de uso humana:** uso resuelto / pendiente / prohibición aplicable detectada; declaración del jugador no es certificación legal.

Bloqueo técnico impide confirmar. Una carencia no esencial permite continuar tras aviso y demostración de alternativa. Uso pendiente o prohibido no se omite con un botón genérico de aceptar. El equipo revisa los derechos de los recursos incluidos; los documentos privados aportados por un jugador no se envían a un servidor.

### Vista previa

Cámaras de cuerpo completo y rostro, misma iluminación para comparar, giro controlado sin afectar al actor, botón “Restablecer vista”. No cortar rostro/pies por asumir una altura fija. Aceptar/rechazar proporciones extremas conforme a pruebas; no modificar hitbox, salud, velocidad o alcance según tamaño visual.

Muestras reflejan capacidades reales: “Alegría — expresión del avatar” o “Alegría — gesto alternativo”. Ausencia de happy no cambia la emoción del sistema. Neutral implica limpiar valores temporales; no aplicar índices de morph del avatar anterior al nuevo. Un canal binario o exclusión de mirada/parpadeo/boca se respeta.

No es un editor de texturas, cuerpo o ropa. No recolorear materiales desconocidos ni inventar atlas de ojos. Armas decorativas del archivo no son equipo funcional; clase/armas del juego conservan sus reglas.

## 5. Personalidad: tres elecciones y muestras

| Plantilla | Explicación en UI | Ejemplo de rescate, solo demostración |
|---|---|---|
| Tsundere | “Reservada al agradecer y directa al opinar. Aprende a confiar sin perder su carácter.” | “Ya buscaba cómo salir… Pero gracias. ¿Vas a la aldea?” |
| Yandere | “Muy atenta a los vínculos. La confianza y los límites se construyen con experiencias.” | “Te detuviste a ayudarme. Gracias. No esperaba encontrar a alguien aquí.” |
| Tímida | “Prudente y de gestos contenidos. Puede tomar la iniciativa y hacerse valiente sin dejar de ser reservada.” | “Gracias… Creí que nadie me oiría. ¿Me ayudas a volver al camino?” |

Textos son ejemplos de presentación propuestos, no biografías definitivas. Ninguno promete romance ni obediencia instantáneos. Habrá gratitud, desacuerdo, preocupación y alegría para las tres.

Nueva partida permite elegir las tres sin alterar el avatar. Cambiar apariencia de una partida no reabre selección de personalidad ni reinicia rasgos evolucionados. Las muestras del creador no registran rescate, promesas, confianza o recuerdos reales. La conducta contextual completa está en [F3 borrador](VRM_Fase3_Personalidad_Contextual_v0.1_BORRADOR.md), sin nueva autorización de implementación.

## 6. Contrato persistente y migración

### Datos e invariantes

Separar perfil persistente de estado vivo y recursos visuales. Propuesta de esquema lógico v2, no código obligatorio:

| Dato lógico | Regla |
|---|---|
| version | Nueva versión explícita del formato; no falsificar version=1 para datos incompatibles |
| companionId | Estable y local, creado una vez al confirmar nueva compañera o migrar; no deriva del archivo, su hash o personalidad |
| personality | Preservar valores actuales 0/1/2 y significado; no remapear Shy por orden visual de tarjetas |
| appearance | Discriminante catálogo/importado, identificador local y versión; hash de copia importada, ruta relativa gestionada y registro de capacidades/uso aplicable |
| identidad/estado existente | Referencia o datos ya disponibles de nombre, clase, memoria, relación, progresión; conservarlos sin que appearance los posea |
| legado v1 | Colores hair/eyes/top/trousers y copia íntegra previa, conservados como historia; no se convierten a índices de materiales/morphs arbitrarios |
| datos no reconocidos | Conservar original/respaldo y no destruir contenido de una versión futura mediante deserialización parcial |

La representación puede reemplazarse sin crear otro companionId. El estado vital, actividad, órdenes, temporizadores, misiones y preferencias no cambian al abrir/confirmar apariencia. No mezclar archivos visuales con memoria narrativa.

F2 prueba conservación de los datos existentes. Para campos aún no implementados se usan fixtures identificados que demuestren no pérdida, sin inventar progreso real ni declarar memoria/romance completos. La evolución funcional se prueba en F3 y siguientes hitos.

### Matriz de migración v1

| Entrada | Comportamiento requerido |
|---|---|
| No hay clave v1 ni partida | Borrador nuevo; identidad se confirma una sola vez |
| v1 válido | Copia íntegra y conservada de isekaid.companion.profile.v1; conservar personality y cuatro colores, crear identidad solo si no existe, proponer apariencia incluida autorizada mediante revisión visible |
| v1 inválido/JSON dañado | No guardar automáticamente valores por defecto sobre original. “No pudimos recuperar el perfil anterior”; permitir conservarlo/volver o iniciar uno nuevo mediante acción explícita con respaldo |
| Versión desconocida | Informar incompatibilidad, preservar archivo/clave y última versión válida; no simular migración |
| Perfil nuevo ya migrado | Reabrir idempotentemente: mismo companionId; sin segundo rescate, duplicados de relación ni generación de otra copia |
| Error al escribir nuevo | Original intacto; no informar éxito ni eliminar respaldo; volver a estado anterior |

No cargar directamente con el fallback silencioso actual si eso impide distinguir perfil faltante de corrupto. Inspeccionar la clave original sin borrar ni renombrar datos antes de preparar recuperación. El backup es un requisito de resultado; su ubicación/método es decisión técnica documentada por Desarrollo.

### Cambio y persistencia consistente

Preparar candidato y validar recursos → copiar a ubicación gestionada y comprobar hash → preparar/validar activación mientras el anterior es recuperable → confirmar perfil/referencia consistente y publicar visual → liberar anterior solo cuando la confirmación haya concluido.

El orden interno de persistencia/activación debe proporcionar rollback y recuperación tras cierre entre etapas; el POC visual por sí solo no resuelve dos archivos de partida. Conservar una generación válida reconocible de perfil + referencia + copia. Si se interrumpe antes de completar la nueva, al reiniciar usar la anterior o abrir recuperación, nunca un perfil que apunte a una copia incompleta.

Si guardar falla, el visual previo y la partida siguen disponibles; candidato/copia temporal pueden limpiarse cuando ya no estén referenciados. No borrar una copia usada por otra partida/versión. El éxito se muestra después de la confirmación persistente, no al terminar el importador.

Reimportar un nombre igual con bytes distintos requiere nueva validación/versión; no reemplaza silenciosamente la copia. Mover/borrar el archivo fuente no afecta a una copia gestionada válida. Si la copia falla o falta, no generar otra compañera: ofrecer catálogo/reimportar con el mismo companionId y estado.

## 7. Cambios permitidos y errores

Cambio visual solo desde un momento seguro/pausa autorizada, no durante combate, secuestro, incapacitación o secuencia narrativa crítica. No reiniciar el plazo de 30 segundos ni avanzar el tiempo de simulación por abrir el menú. Mantener las reglas actuales de pausa; no ratificar aquí cambios de balance.

| Caso | Texto español | Acción y conservación |
|---|---|---|
| Cancelar selector | Sin mensaje de fallo | Volver con borrador/confirmado intactos |
| Formato/0.x | “Esta versión de VRM no es compatible. Usa un archivo VRM 1.0.” | “Elegir otro archivo” / “Usar apariencia incluida” |
| Archivo dañado | “No pudimos leer este avatar. Puede estar incompleto o dañado.” | Reintentar otro; anterior válido |
| Recursos fuera de límites | “Este avatar supera el límite de [recurso]: [valor] / [límite].” | No activar ni corregirlo deformando el cuerpo |
| Expresiones ausentes | “No incluye [funciones]. Usaremos [alternativas comprobadas].” | Demostrar alternativa antes de confirmar |
| Uso pendiente | “Hay condiciones de uso pendientes de revisar.” | Ver condiciones / apariencia incluida |
| Prohibición aplicable | “Las condiciones indicadas no permiten este uso.” | Otro recurso, sin ignorar |
| Cancelación en curso | “Estamos descartando el avatar…” | Esperar limpieza; no iniciar otro import simultáneo |
| Guardado fallido | “No pudimos guardar el cambio. Se conserva la apariencia anterior.” | “Reintentar” / “Volver” |
| Copia faltante | “Falta la apariencia guardada. El progreso de tu compañera se conserva.” | Reimportar / catálogo / volver sin sobrescribir |
| Momento inseguro | “Podrás cambiar su apariencia en un momento seguro.” | Volver; no alterar estado vital |
| Confirmado | “Compañera guardada” / “Apariencia actualizada” | Iniciar/reanudar |

## 8. UI responsive y accesibilidad

Requisitos de entrega: 1920×1080 y 1280×720, con pruebas adicionales 1600×900 y 1366×768. No basta escalar una captura; ejecutar cada tamaño o ventana redimensionable equivalente.

**Propuesta F2-P02 de composición:** a 1280×720 reservar margen exterior 24 px; encabezado y pie fijo con acciones; área central de unos 360–400 px para selección/detalles y resto para viewport. A 1920×1080 ampliar viewport y espacio, no solo tipografía. Los tamaños son referencias de diseño a revisar, no valores del Inspector autorizados. Texto largo/condiciones en panel desplazable sin sacar “Volver” y acción principal de pantalla. A tamaños intermedios, resolver altura y ancho disponible sin solapar controles ni ocultar avisos.

- Botones con etiquetas completas y área de acción propuesta mínima 44×44 px a la escala final. Información larga debe poder ampliarse.
- Texto normal con contraste objetivo 4.5:1; elementos grandes/foco al menos 3:1 como criterio propuesto de QA, no certificación de accesibilidad universal.
- Tab/Shift+Tab y flechas recorren orden visual; Enter/Espacio activan; Escape vuelve o inicia cancelación visible. Selector del sistema respeta sus propias teclas. Si build admite mando, misma secuencia funcional.
- Selección y foco diferentes: borde/indicador + texto/icono, no solo color. Recuperar foco al control que abrió un diálogo; no perderlo al terminar importación.
- Rotar por arrastre en viewport y mediante controles accesibles; restablecer encuadre. No capturar el teclado al escribir una ruta o texto.
- “Guardar/Continuar” deshabilitado con motivo visible cuando falta selección, uso o preparación. Evitar ventanas modales anidadas y bloqueos sin salida.
- Catálogo, condiciones almacenadas y errores funcionan offline. Enlaces externos solo por acción expresa, sin bloquear el flujo de catálogo.

## 9. Seguridad, rendimiento y recursos

El POC fija un perfil diagnóstico: archivo 32 MiB, JSON 4 MiB/profundidad 64, PNG incorporadas hasta 4096 por lado y 64 megapíxeles totales, sin URI externas/data URI ni JPEG, VRM 1.0 y límites de nodos/mallas/materiales/buffers/springs. Consultar valores completos en el informe; **no son presupuestos finales de producción**.

F2-P03 propone conservar los límites comprobados como guardas iniciales hasta revisar el perfil técnico, sin ampliarlos por conveniencia. El SDK sigue procesando en el proceso Unity; nueve rechazos y seis ciclos no certifican archivos hostiles, todas las fugas, VRAM o builds.

No ejecutar código del archivo ni permitir recursos externos o lectura fuera de las rutas gestionadas. Nombres/miniaturas son datos no confiables; no se interpretan como código o rich text arbitrario. Una importación simultánea. Documentar latencia y memoria de coexistencia entre avatar anterior/candidato y recursos retenidos tras cancelación. Si se necesita endurecimiento que cambie compatibilidad o arquitectura, presentarlo con evidencia; no declararlo resuelto en este contrato.

## 10. Criterios F2 de seguimiento

Todos PENDIENTES DE EJECUCIÓN/ACEPTACIÓN en esta entrega documental. Se vinculan a los 11 criterios F2 originales y guardas compartidas sin reemplazar sus IDs.

| Caso | Prueba / condición observable |
|---|---|
| F2-AC01 | Una apariencia incluida offline disponible; elegir/volver conserva borrador y no crea identidad todavía |
| F2-AC02 | VRM 1.0 válido se prepara; 0.x, falsa extensión y archivo truncado muestran motivo y conservan anterior |
| F2-AC03 | Cuerpo/rostro encuadrados a tamaños objetivo; muestras etiquetan capacidad real/alternativa, sin afirmar funciones ausentes |
| F2-AC04 | Tsundere/yandere/tímida seleccionables sobre ambas vías; no cambia apariencia ni clase; reapertura conserva selección |
| F2-AC05 | Confirmar nueva compañera dos veces no duplica identidad; muestras no registran rescate/confianza |
| F2-AC06 | Catálogo→VRM→catálogo conserva identidad/estado/rasgos/memorias/clase/relación; fixtures ausentes marcados como tales |
| F2-AC07 | Migrar cada personalidad v1 y colores válidos, reabrir y repetir: datos/backup idénticos, mismo ID |
| F2-AC08 | v1 dañada/desconocida y migración fallida: original preservado, recuperación visible; no guardado silencioso de predeterminados |
| F2-AC09 | Mover/borrar fuente no afecta copia; copia gestionada dañada abre recuperación sin reiniciar compañera |
| F2-AC10 | Fallos al copiar, guardar, activar y cerrar entre etapas: recuperar generación anterior; sin éxito falso ni copia válida eliminada |
| F2-AC11 | Recursos externos, datos inválidos y límites: rechazo útil, sin red ni ejecución suministrada por archivo |
| F2-AC12 | Creador→Game→cierre/reinicio offline conserva perfil/apariencia; no simularlo solo en escena experimental |
| F2-AC13 | Cancelar/abandonar durante carga; intentar segundo import: no resultado tardío publicado, limpieza al concluir, foco recuperado |
| F2-AC14 | Técnica válida con permiso/uso pendiente permanece pendiente; ficha no confunde datos declarados y aprobación humana |
| F2-AC15 | Apariencia incluida tiene procedencia, créditos, derechos de distribución y capacidades revisados; no usa prueba privada no autorizada |
| F2-AC16 | Repetir seis ciclos y fallos por etapa comparables con POC; reportar recursos/latencias/memoria, sin atribuir estabilidad ilimitada |
| F2-AC17 | 1920×1080, 1280×720, 1600×900, 1366×768: sin recorte/solape; español, foco, teclado y contraste comprobados |
| F2-AC18 | Intento de cambio en peligro/Downed/captura: diferido, sin reiniciar temporizador ni cambiar reglas de pausa |

Para F2-AC10 incluir primera importación sin VRM anterior y reemplazo con avatar previo/expresión activa; comparar estado antes/después. Para F2-AC07/08 guardar backup y evidencia sin publicar datos privados. Pruebas de catálogo requieren acceso al recurso autorizado; un fixture privado permite ensayo local, no distribución.

## 11. Evidencia y cierre

Desarrollo entrega PR/commit, archivos, versiones, plataforma/render pipeline, resultados por caso, errores conocidos, imágenes reales autorizadas, perfil/rollback y migración, monitor offline y medición de recursos. Registrar qué fue automatizado y qué revisó una persona. Mantener capturas restringidas locales con hash; no publicar cuerpos/rostros sin permiso.

No aceptar F2 por compilación o por los resultados del POC. Build ejecutada se identifica por plataforma y resultado; si no se ejecuta se indica pendiente. Confirmación final de F2 y siguiente alcance F3 corresponde a Dirección, no al documento ni al desarrollador.

## 12. Aclaraciones nuevas que requieren revisión si cambian la implementación

| Propuesta | Detalle | Mientras no se revise/fusione |
|---|---|---|
| F2-P01 | Esquema lógico v2, ID estable, generaciones recuperables y detalle de migración/copia | Conservar invariantes v0.2; Chat 3 documenta su solución y consulta diferencias funcionales importantes |
| F2-P02 | Composición UI, tamaños mínimos, contrastes y pantallas intermedias | Aplicar responsive/accesibilidad ya solicitados; cifras/patrón concreto no son una orden de rehacer UI |
| F2-P03 | Mantener perfil restrictivo del POC como guarda inicial, sin JPEG/0.x ni ampliación de límites | No anunciar compatibilidad no probada; proponer ajustes con evidencia, sin bloquear la tarea autorizada |
| F2-P04 | Serialización de selección durante cancelación, recuperación explícita y ejemplos de personalidad/UI | Seguir cancelación segura/v0.2; si cambia interacción ya construida, presentarlo a Dirección |

Estos detalles no conceden nuevas mecánicas de clase, conversación o relación. El [borrador F3](VRM_Fase3_Personalidad_Contextual_v0.1_BORRADOR.md) prepara el siguiente hito cuando visual/persistencia estén estables.

**Registro 0.1:** contrato documental sobre DEC-007 y fuentes main; sin ejecutar ni modificar Unity, sin seleccionar/publicar recursos nuevos.
