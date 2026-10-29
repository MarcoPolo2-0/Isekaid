# F3A — Criterios de aceptación narrativa y funcional

**Versión:** 0.1 · **Estado:** diseño en revisión, ejecución Unity pendiente.  
[Especificación](F3A_Especificacion_v0.1.md) · [biblioteca](F3A_Biblioteca_v0.1.json) · [árboles](F3A_Arboles_v0.1.md) · [eventos](F3A_Contrato_Eventos_v0.1.md).

## 1. Comprobaciones de la entrega documental

| ID | Comprobación | Evidencia esperada |
|---|---|---|
| F3A-D01 | Al menos 30 entradas distintas; entrega 60, 20 por personalidad; A–F cubiertas | Lectura JSON y conteo de IDs/situaciones; todos los spokenText no vacíos y distintos |
| F3A-D02 | Tres árboles, cada personalidad tiene raíz y dos hojas; sin ciclos ni referencias rotas | Recorrido del grafo: 27 nodos alcanzables y 18 opciones válidas; todas las hojas cierran |
| F3A-D03 | Toda entrada tiene disparador/contexto, personalidad, texto, emoción/intención, expresión/alternativa, prioridad, repetición, opciones/cierre y memoria | Validación estructural de campos, enums y referencias; strings no se evalúan como código |
| F3A-D04 | Ninguna entrada ni opción escribe recuerdos, clase, movimiento o confianza | memory.writes=[], trustDelta=0; opciones sin efectos ocultos y primer efecto de rescate propiedad del gameplay |
| F3A-D05 | Matriz de siete emociones y alternativas seguras | Siete claves válidas; ninguna entrada pide avatar/nombre/material como personalidad; fallback explícito |
| F3A-D06 | Estados implementados separados de adaptadores/futuro | Comparación de fuentes base; peligro general/promesas no declarados implementados |
| F3A-D07 | Referencias y originales conservados | Enlaces locales válidos; GDD, decisiones, F2 y fuentes Unity sin cambios en PR; readme append-only |
| F3A-D08 | Lectura editorial por Dirección/usuario | Personajes reconocibles, tono de desconocidos, español natural y urgencias breves; criterio humano, no aprobado por pasar JSON |

D01–D07 combinan revisión estática/estructural y lectura de contrato; no prueban un motor de conversación. D08 requiere revisión editorial humana posterior. El informe final del PR indica cuáles se verificaron realmente.

## 2. Pruebas funcionales para Desarrollo — todas pendientes

| ID | Caso reproducible | Resultado exigido |
|---|---|---|
| F3A-R01 | Trapped→rescate confirmado en tres fixtures con cada personalidad; repetir interacción | A.RESCUE correspondiente una vez, RemembersFirstRescue/Trust solo por regla existente, sin romance ni otro efecto por texto |
| F3A-R02 | Cada árbol, personalidad y alternativa de elección (18 recorridos) | Respuesta correcta, cierre legible y regreso a temas; no recorrer dos hojas como si el jugador hubiera elegido ambas |
| F3A-R03 | Tres temas completados, Talk repetido y tres variantes casuales con reloj activo | No repetir intro ni variante consumida; enfriamiento de grupo, silencio/no tema nuevo al agotar; Trust sin crecimiento |
| F3A-R04 | Cambios reales Follow/Wait y repetición de comando sin cambio | Una respuesta apropiada por cambio, opinión compatible con orden real, sin aceptación de órdenes ignoradas ni frases dobles |
| F3A-R05 | Active→Downed→reanimar, dos episodios separados | Una petición breve por caída y una gratitud por reanimación; sin segundo primer rescate ni Trust=1 reaplicado |
| F3A-R06 | Charla en curso, Downed o pérdida de elegibilidad; señal de peligro solo en fixture rotulado | Priorizar supervivencia, detener gesto social, conservar cursor válido y retomar sin repetir raíz; no afirmar detector real por inyección |
| F3A-R07 | Raíz presentada y opción pendiente → creador F2 → cambiar avatar → volver | Misma personalidad/identidad/recuerdo/consumo y opciones; visual nuevo sin reinicio de diálogo |
| F3A-R08 | Avatar con canal preferido, sin ese canal y sin gesto compatible | Misma intención/texto/efectos; alternativa segura o solo texto, sin doble ojo ni índice ajeno |
| F3A-R09 | Muestras de creador, activación happy y cancelación repetida | No eventos de rescate, confianza, promesas ni consumo de conversación real |
| F3A-R10 | Tres personalidades, mismo estado/avatares/clase y elecciones; evaluar ocultando cara | Diferencias por vocabulario, ritmo, opinión y postura; ninguna personalidad reducida a repetir frases equivalentes |
| F3A-R11 | Opción ajena al nodo, doble clic, callback de visual anterior y orden cambiada antes de presentar | Rechazo/no-op seguro; no dos hojas ni texto obsoleto; no recuperar A.RESCUE tras una reanimación |
| F3A-R12 | Salir voluntariamente, pausar y retomar; cerrar encuentro explícitamente | No castigo por silencio/pausa, reloj activo detenido en pausa, cursor de sesión conservado; reinicio de ensayo distinto de cambio de apariencia |

No exigir ni declarar aquí guardado de campaña, promesas o sistema completo de confianza. Si se ensaya reinicio de proceso, informar que la aventura completa aún no está persistida y separar ese límite de continuidad dentro del encuentro. La aceptación manual F2 sigue pendiente, aunque puedan ejecutarse fixtures aislados de F3A.

## 3. Revisión editorial de cada personalidad

- Tsundere reconoce ayuda y formula desacuerdo sin insulto constante. Su orgullo no borra vulnerabilidad ni bloquea cooperación.
- Yandere presta atención, pregunta por intenciones y pone límites, sin amor inmediato, amenazas o exclusividad obligatoria.
- Tímida usa prudencia y silencios, pero decide, pregunta y alerta con claridad cuando hace falta.
- Primer encuentro conserva cautela. Ninguna conversación revela una biografía, enemigo presente o misión que no se haya aprobado.
- La última opción de un árbol realmente cierra/retorna; no deja al jugador atrapado en una entrevista.
- Alivio y alegría no son “amor ganado”; neutral no es sonrisa permanente.
- Las frases de urgencia no se alargan para exhibir arquetipo ni retrasan reanimación.

La evaluación humana puede pedir cambios editoriales antes de fusionar. IDs estables y versión de biblioteca permiten corregir texto sin romper ramas.

## 4. Evidencia de ejecución y cierre de F3A

Desarrollo debe entregar build/commit y fixture, identidad de encuentro, personalidad, evento/estado previo/posterior, ID de entrada y opción, gesto/expresión aplicada o fallback y consumos/Trust antes/después. Capturas autorizadas y logs sin datos privados. No publicar avatares/capturas restringidos.

Pruebas documentales, revisión editorial, Play Mode, pruebas de persistencia y build son evidencias diferentes. Pasar D01–D07 no supera R01–R12. No cerrar Fase 3 completa ni la revisión manual F2 porque la biblioteca exista.

**Estado de esta entrega:** no se ejecutó Unity, conversación runtime, gestos ni builds. Los textos y datos son una base para revisión e integración de Chat 3, no evidencia de comportamiento implementado.
