# F3A — Correcciones de Dirección al PR #10

**Fecha:** 29/10/2026, Guatemala. **Estado:** evidencia técnica para revisión; no fusionar ni declarar F3 completa.

**Solicitud:** [revisión de Dirección](https://github.com/MarcoPolo2-0/Isekaid/pull/10#pullrequestreview-5699436782). Se incorpora el PR #9, encabezado `6df539da5334f76bd66d5e9be28c81c2735e5e6b`, mediante merge `9d8464c` dentro de la misma rama `feature/isek-f3a-dialogue-runtime`. Main no se modifica. Implementación de juego: `a1aebe3`; ensayo final y build: `d2f58a8`.

Los informes y capturas del directorio padre corresponden al prototipo anterior de 13 entradas. Esta revisión los conserva como historial y sustituye sus resultados vigentes de diálogo. F2 conserva sus pendientes de aceptación manual.

## Correcciones entregadas

| Anotación | Cambio | Prueba |
|---|---|---|
| Integrar biblioteca PR #9 | 60 entradas canónicas, 20 por personalidad; TRAP/FEEL/ROAD; nueve raíces y 18 alternativas, con IDs/textos originales | 18 recorridos independientes en Game; `routes` en los resultados |
| Mantener cursor durante F2 | Se conserva nodo, opciones, foco, consumo y la instancia de CompanionState; abrir el creador solo suspende presentación | Catálogo→VRM→catálogo reales con una raíz pendiente; sin otro Spoke de raíz |
| Cierre antes de elegir | La ventana cerrada conserva el cursor; consumo de raíz no equivale a completar tema. La hoja elegida completa solo ese tema | Cerrar, moverse y reabrir la misma raíz; opciones intactas y una sola presentación |
| Recuperación por episodio | Contador de episodios para incapacidad/reanimación/retorno; suspensión repetida idempotente | Dos episodios completos y un tercero para comprobar cierre voluntario; una ayuda y gratitud por episodio |
| Evidencia y regresión | Resultados nuevos, estados completos por criterio, capturas, hashes y verificación canónica en CI | Archivos de esta carpeta; evidencias F2 separadas |

La primera charla abre un menú de temas, sin inventar otra frase de la compañera. Cada raíz ofrece sus dos opciones canónicas; la hoja completa queda visible hasta «Volver a temas» o «Terminar conversación». Reabrir una raíz presentada muestra «Opciones pendientes» como historial, sin reemitir Spoke. La visita al creador no consume contenido ni vuelve a registrar rescate o confianza.

El retorno tras reanimación requiere una nueva petición explícita y elegible. Con cursor suspendido presenta F.RESUME y permite continuar las opciones; sin cursor presenta F.SAFE_RETURN por episodio. No se abre una charla automáticamente después de un cierre voluntario. La muerte invalida el cursor y el reinicio explícito limpia el encuentro.

Seguir/esperar solo emite aceptación cuando cambia la orden real. Las alternativas WAIT/WAIT_OPINION se seleccionan de forma determinista y nunca impiden aplicar la orden. Las tres variantes casuales comparten 20 segundos activos de enfriamiento, conforme a la propuesta recibida; al agotarse se usa el estado de UI «No hay temas nuevos por ahora», sin una frase NPC artificial.

## Adaptación y separación de responsabilidades

`tools/build_f3a_runtime_content.py` lee la biblioteca de Diseño sin modificarla y produce `Resources/Dialogue/F3A.es.json`, formato runtime 2. Comprueba los 60 IDs y textos distintos, enum de personalidad, destinos, dos hojas por raíz, condiciones conocidas, alcance/repetición, emociones y efectos cero. Las condiciones se traducen a conexiones explícitas, nunca se evalúan como código. La huella de fuente se calcula sobre UTF-8 con saltos de línea normalizados.

La estructura runtime conserva IDs, texto, personalidad-condición, emoción/intención, evento, estado vital, tema, variante de orden, alcance, grupo/enfriamiento, prioridad, cierre y presentación facial. Metadatos editoriales de ritmo/postura/gesto permanecen en la fuente; no se anuncian clips que no existen. El formato actual está deliberadamente acotado a esta versión de F3A.

El director mantiene un único cursor del encuentro. Ventana activa, pausa, cursor y consumo son estados distintos. `Presented` tiene un solo propietario de voz/texto en CompanionController; `Changed` refresca UI/controles sin producir otra línea. Se preservan las APIs históricas RescueLine/TalkLine sin presentar sus textos en paralelo.

Las elecciones capturan una revisión de la vista. Una opción ajena, doble elección o revisión anterior a la visita al creador no puede producir otra hoja. La validación se aplica en el director; el presentador no modifica vida, confianza, personalidad u órdenes.

No se modifican escenas, prefabs, GUID existentes, jugador, CompanionState, perfiles, migración, importador, paquetes ni Woman/G2. No hay wiring nuevo. El creador y el sistema de movimiento conservan sus clases existentes.

## Pruebas y resultados

Ver `editor-results.json` y `windows-results.json` para las comprobaciones y recorridos; `presentations` identifica fixture, entrada/evento, personalidad, estado vital, episodio, confianza, consumo y expresión aplicada. Cada alternativa del árbol se prueba en un encuentro nuevo; no se recorren dos hojas incompatibles en la misma conversación.

| Ensayo | Evidencia |
|---|---|
| Unity 6000.6.7f1 Play Mode, escenario acotado a 60 FPS | 285 comprobaciones, 18 recorridos; `editor-results.json` |
| Windows x64 Development Mono, mismo escenario | 285 comprobaciones y 18 recorridos; `windows-results.json` |
| Metadatos grandes y cambio transaccional F2 en Editor | 43 comprobaciones, 195 recursos liberados; `f2-editor-results.json` |
| Regresión F2 desde la nueva build y reinicio sin fuente | `f2-windows-results.json`, `f2-restart-results.json` |
| Guardas, almacenamiento anterior y prototipo de dos escenas | Nueve rechazos + dos límites, once aserciones; `guard-results.json`, `storage-results.json`, `prototype-results.json` |
| Metadatos hostiles y respaldo | 35 aserciones; `metadata-results.json` |
| Build nueva y huellas | `build-results.json` |
| Canonicalización, estructura, archivos protegidos y procedencia | `structure-results.json`, `artifact-manifest.json` |

El error conocido del índice `UnityEditor.Search.SearchDatabase` se excluye únicamente por su stack del registro de gameplay. No se silencian errores del juego.

### Movimiento y límite del ensayo Windows

Se conservó la aserción de desplazamiento real después del cierre. Los dos primeros intentos Windows no la superaron. El segundo registró 0.0516 m en 4,216 frames con entrada habilitada, pausa desactivada y escala 1. El prefab original declara `minMoveDistance=0.001`: a frecuencias extremas fuera de foco los pasos individuales pueden quedar por debajo del umbral. Esta explicación está respaldada por código/configuración y la frecuencia observada; no se modificó el jugador para esta revisión.

El ensayo final mantiene las teclas durante al menos tres frames, espera su liberación con timeout y acota la ejecución a 60 FPS. Restaura los ajustes al salir; no fija un presupuesto de FPS para producción. La ejecución final Windows midió 1.4591 m en 34 frames (delta 0.0166679 s); Editor midió 1.3158 m. Los resultados registran metros, frames, delta y umbral. `initial-windows-run.json` conserva el diagnóstico de los intentos fallidos. La DLL de gameplay permanece idéntica entre esas builds; solo cambió el ensayo. Este caso extremo del prototipo queda documentado, no corregido ni usado para afirmar validez a cualquier frecuencia.

## Criterios del PR #9

La evaluación completa y sus límites están en `criteria-status.json`: **17 PASS técnicos/documentales y 3 PARTIAL**. No son aceptación humana. El campo `criteria` de los resultados automáticos informa éxito de los fixtures cubiertos; para el criterio completo prevalece esta evaluación.

| Criterio | Estado | Evidencia / límite |
|---|---|---|
| D01 | PASS | 60 entradas distintas, 20 por personalidad, A–F conservadas |
| D02 | PASS | 27 nodos de árboles, nueve raíces, 18 destinos sin ciclos; todos los recorridos alternativos ejecutados |
| D03 | PASS | Campos canónicos completos y adaptación validada; metadatos editoriales no se convierten en animaciones |
| D04 | PASS | writes vacío, trustDelta=0 y opciones sin cambios de clase/orden; gameplay conserva el rescate |
| D05 | PASS | Siete emociones y fallback explícito; certificación visual no inferida |
| D06 | PASS | Hechos reales, adaptadores y futuro separados; no detector general de peligro |
| D07 | PASS | Fuentes de Diseño, GDD, decisiones, originales y ambos bloques de readme preservados |
| D08 | PARTIAL | Pendiente revisión editorial humana de contenido/voz |
| R01 | PASS | Rescate de las tres personalidades, efecto único por encuentro |
| R02 | PASS | 18 recorridos independientes, hoja correcta y vuelta a temas |
| R03 | PASS | Temas completados, tres casuales, enfriamiento activo compartido y agotamiento sin repetición |
| R04 | PASS | Cambios reales de seguir/esperar, no-op sin aceptación repetida y opinión compatible |
| R05 | PASS | Episodios separados y deduplicados; no segundo primer rescate ni recompensa |
| R06 | PASS | Downed real e interrupción explícita rotulada; cursor válido conservado y prioridad de ayuda |
| R07 | PASS | Raíz, opciones, foco, identidad/estado y consumo conservados al cambiar catálogo/VRM |
| R08 | PASS | Preferido disponible, canal ausente y fallback; intención/texto sin efectos laterales |
| R09 | PASS | Muestras/cancelación del creador sin consumo ni hechos de rescate/confianza |
| R10 | PARTIAL | Textos distintos verificados; ritmo/postura y comparación humana sin cara pendientes |
| R11 | PARTIAL | Opción ajena, doble elección y revisión UI obsoleta probadas. No se fabricó un callback narrativo asíncrono del SDK ni una cola de órdenes diferidas; sus ensayos completos quedan pendientes |
| R12 | PASS | Cierre/reapertura voluntaria, pausa y reinicio diferenciados; memoria de sesión conservada |

## Presentación y límites pendientes

La matriz de Diseño no autoriza intensidades faciales finales. `reset` se traduce a limpieza neutral. happy/sad/surprised usan el adaptador existente cuando están disponibles. sad_soft_if_validated, angry_soft_if_validated y relaxed_if_validated son intenciones condicionales, no nombres de morph: sin certificación visual se usa neutral y texto. No se inventa un gesto. La emoción semántica se conserva aunque falte una cara compatible.

No se acreditan captura física del cursor ni cada clic real con ratón; hay teclado virtual, eventos UI Toolkit, desplazamiento medido y revisión de capturas. Quedan aprobación editorial/visual, gestos/ritmo finales, revisión humana de derechos, validación manual F2 y casos asíncronos indicados. No se probaron Android, release no Development, red físicamente desconectada ni sesiones prolongadas.

No se implementan persistencia de campaña, promesas, romance, misiones, enemigos, memoria ampliada o economía de confianza. No se fusionan PR #9 ni #10 a main. Los cuatro criterios parciales anteriores de F2 permanecen abiertos.

## Capturas reales

Las seis imágenes públicas de `evidence/` muestran exclusivamente Adventurer incluido (atribución y licencia conservadas). Se revisaron disposición y texto a 1920×1080, 1600×900 y 1280×720. Las capturas del VRM privado permanecen en LocalVrmEvidence y fuera de Git.

- [Raíz con opciones pendientes 1920×1080](evidence/canonical-root-1920.png)
- [Raíz 1600×900](evidence/canonical-root-1600.png)
- [Raíz 1280×720](evidence/canonical-root-1280.png)
- [Respuesta tsundere](evidence/canonical-answer-Tsundere.png)
- [Respuesta yandere](evidence/canonical-answer-Yandere.png)
- [Respuesta tímida](evidence/canonical-answer-Shy.png)

Ver [reproducción y revisión manual](PRUEBAS.md). La build local conserva toda la carpeta Builds/F2Windows, incluidos datos y avisos; comprobar hashes de esta revisión para distinguirla de la entrega inicial.
