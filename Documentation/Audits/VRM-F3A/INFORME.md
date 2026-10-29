# ISEKAID — F3A: diálogos y reacciones en Game

**Revisión vigente:** [correcciones de Dirección, biblioteca canónica PR #9 y continuidad del cursor](review-1/INFORME.md). El contenido de este documento conserva la entrega inicial de 13 entradas.

**Fecha:** 29/10/2026, Guatemala. **Unity:** 6000.6.7f1.
**Rama:** `feature/isek-f3a-dialogue-runtime`. **Base:** `main` `4bd685718d670006569841e93f6c38282323db01`, con PR #6 y #7 fusionados.
**Implementación probada:** `5f3da89dd0e1853f8b24d8ac8c291723260bf4cb`. **Estado:** entrega técnica experimental para revisión de Dirección. No fusionar; no declara F3 completa ni acepta F2.

## Autorización y resultado

La autorización directa de Chat 1 permite esta secuencia limitada: rescatar, conversar, elegir una pregunta, responder según personalidad, volver a conversar con otro contexto, seguir/esperar, incapacitar y reanimar. Se consultaron DECISIONS.md, Workflow v1.2, el borrador F3 y los informes/regresiones F2. El borrador general no se convierte en autorización de promesas, romance, combate, iniciativas o memoria persistente.

El recorrido funciona en la escena **Game existente**, con Adventurer y con el VRM privado de ensayo, en Play Mode y Windows Development. El contenido es provisional y reemplazable: 13 entradas, 39 variantes de personalidad y tres preguntas con IDs estables. No hay llamadas narrativas a servicios externos.

**Controles:** mantener E para rescatar/conversar; 1–3 o botones para elegir; Continuar/Intro para terminar una respuesta; Cerrar/Esc para saltar; Tab para pausa; F para seguir/esperar. La conversación libera el cursor y bloquea movimiento/mirada mediante `FirstPersonController.InputEnabled`; el mundo sigue activo. El menú de pausa conserva el diálogo y detiene su tiempo. Al cerrar o interrumpir se devuelve la propiedad de los controles al prototipo, respetando pausa, muerte y creador.

## Cambios y límites arquitectónicos

| Pieza | Responsabilidad |
|---|---|
| `Scripts/Companion/DialogueContent.cs` y `Resources/Dialogue/F3A.es.json` | Datos locales, IDs para futura localización, variantes, emoción y hasta tres opciones; valida IDs, textos y destinos |
| `Scripts/Companion/DialogueDirector.cs` | Consulta el estado real por referencia, selecciona contexto, registra contenido consumido y mantiene una conversación por encuentro; no toca confianza ni apariencia |
| `Scripts/UI/DialoguePresenter.cs` | UI Toolkit, hablante, frase, elecciones y cierre; llama al director |
| `Scripts/Companion/CompanionReactionPresenter.cs` | Traduce siete intenciones semánticas mediante la API existente de expresiones; neutral seguro y limpieza |
| `CompanionController.cs` | Conserva rescate, NavMesh, seguimiento, estados y Spoke; conecta hechos reales a contenido y reacciones |
| `CompanionPrototype.cs` | Coordina entrada, pausa, cierre e interrupción; impide atajos de Game detrás del creador aditivo |
| `PrototypeHud.cs` / `Prototype.uss` | Mantienen el estilo y avisos transitorios, incorporan el panel de conversación |
| `Validation/F3ARuntimeChecks.cs` / `Editor/F3AIntegrationChecks.cs` | Recorrido real reproducible, entrada simulada, activación de botones, capacidades y disposición visual; el ensamblado de validación añade referencia a Input System |

No se modificaron escenas, GUID existentes, prefabs, `CompanionState`, perfiles, migración, importador, paquetes, Woman ni G2. No hay campos nuevos de Inspector ni wiring manual. Las APIs históricas RescueLine/TalkLine permanecen compatibles; la selección nueva se hace con datos fuera del controlador.

El director vive con el controlador ya conservado por F2 al abrir el creador. Cambiar apariencia interrumpe la presentación, conserva los contenidos consumidos y no repite el rescate. Reiniciar encuentro limpia ese historial junto con el estado que ya reiniciaba el prototipo. No se crea un segundo guardado; los consumos no sobreviven al cierre del proceso.

La interfaz `Interrupt(DialoguePriority)` está disponible para eventos futuros. Esta entrega solo utiliza hechos existentes: cambio de estado vital, órdenes, creador y reinicio. No se inventa un detector de peligro o un sistema de combate. Pausar/reanudar no vuelve a emitir Spoke para la misma entrada.

## Pruebas ejecutadas

| Prueba | Resultado técnico | Evidencia |
|---|---|---|
| F3A en Unity Play Mode | PASS, 123 comprobaciones, sin errores runtime registrados | `editor-results.json` |
| F3A desde ejecutable Windows x64 Development Mono | PASS, 123 comprobaciones, sin errores runtime registrados | `windows-results.json` |
| Mismo rescate y pregunta, tres personalidades | Tres respuestas distintas en el mismo catálogo; recuerdo inicial único | Variantes exactas en ambos resultados F3A |
| Elecciones, destino, opción inválida, continuar, cerrar y Esc | PASS; teclado virtual y eventos de activación UI Toolkit | Resultados F3A |
| Movimiento después de cerrar | PASS mediante pulsación W y desplazamiento medido | Resultados F3A |
| Repetición y reinicio | Intro no se reitera, respuestas consumidas dejan de ofrecerse, agotamiento sin bucle; reinicio limpia encuentro | Resultados F3A |
| Pausa/reanudación | Tiempo narrativo y cuenta de rescate detenidos en frames reales; misma entrada y sin Spoke duplicado | Resultados F3A |
| Incapacitación, reanimación y muerte | Prioridad de ayuda, alivio al revivir, confianza = 1, sin charla ordinaria al morir | Resultados F3A |
| Catálogo → VRM → catálogo durante el mismo encuentro | Mismo CompanionState, confianza, recuerdo e historial; copia gestionada guardada | Resultados F3A |
| Expresiones y fallback | Pesos reales happy, limpieza neutral y alternativa segura con capacidades ausentes | Resultados F3A y tabla siguiente |
| 1920×1080, 1600×900, 1280×720 | Capturas reales y límites/textos/opciones sin desborde en el contenido provisional | `evidence/`, resultados F3A |
| Regresión F2 completa en Editor, metadatos grandes | PASS, 43 comprobaciones y 195 recursos liberados | `f2-regression-results.json` |
| Regresión F2 en la nueva build Windows | PASS, 36 comprobaciones | `f2-windows-results.json` |
| Segundo proceso Windows sin archivo fuente | PASS, dos comprobaciones sobre copia gestionada y entrada a Game | `f2-restart-results.json` |
| Metadatos hostiles, lector real y respaldo | PASS, 35 aserciones; límites 32 KiB / 1 MiB conservados | `metadata-results.json` |
| Guardas de importación y almacenamiento anterior | PASS: nueve rechazos + dos límites, once aserciones de almacenamiento | `guard-results.json`, `regression-storage-results.json` |
| Prototipo original de dos escenas | PASS, marcadores originales de estática y Play Mode | `prototype-results.json` |
| Build Windows | Succeeded; 196,505,711 bytes informados por Unity | `build-results.json` |
| Estructura unificada y recursos protegidos F2 | PASS, 715 archivos protegidos sin alteración | `structure-results.json`, `artifact-manifest.json` |

Los PASS son evidencia de ejecución, no aprobación de contenido, arte o cierre de fase. El error conocido de `UnityEditor.Search.SearchDatabase` apareció en el Editor; se excluye solo ese stack del registro de errores de gameplay, como en F2. Las comprobaciones iniciales del ensayo detectaron ajustes necesarios en su referencia de entrada y en conservación de InputSettings; el cierre funcionaba, pero una aserción de captura OS no era aplicable a una ventana batch/sin foco. Los resultados publicados corresponden a la ejecución corregida, sin silenciar errores runtime.

## Emociones comprobadas

| Intención | Adventurer | VRM privado probado |
|---|---|---|
| Neutral | neutral | neutral |
| Alegría | happy | happy |
| Tristeza | sad | sad |
| Preocupación | sad | sad |
| Enfado | sad, alternativa porque no dispone de angry | angry |
| Sorpresa | surprised | surprised |
| Alivio | happy, alternativa porque no dispone de relaxed | relaxed |

Las correspondencias se registran por avatar en los resultados. No se presupone el mismo nombre de morph entre modelos: se utiliza la abstracción de expresiones existente. Un presentador sin canales emocionales se probó con las siete intenciones y devolvió neutral sin perder el texto. Activar la expresión no modifica memoria, personalidad o confianza. Los avisos temporales expiran con tiempo activo y el cierre/interrupción limpia las caras. La preocupación y tristeza comparten canal en esta muestra; su diferenciación visual final queda pendiente.

## Evidencia visual real y privacidad

Revisión visual realizada sobre capturas de Game a las tres resoluciones y sobre las tres respuestas a la misma pregunta. Son imágenes del catálogo Adventurer incluido, con atribución/licencia conservada en `Assets/_Project/AvatarCatalog.LICENSE.txt` y en los avisos de la build. No son arte final.

- [Elecciones 1920×1080](evidence/dialogue-choices-1920.png)
- [Elecciones 1600×900](evidence/dialogue-choices-1600.png)
- [Elecciones 1280×720](evidence/dialogue-choices-1280.png)
- [Respuesta tsundere](evidence/dialogue-answer-Tsundere.png)
- [Respuesta yandere](evidence/dialogue-answer-Yandere.png)
- [Respuesta tímida](evidence/dialogue-answer-Shy.png)

El VRM original y sus capturas siguen fuera de Git, bajo la exclusión `LocalVrmEvidence/`. Se reutilizó el fixture privado de F2 sin modificarlo. Su compatibilidad no acredita derechos de redistribución. El hash original y el fixture de metadatos grandes están en los resultados correspondientes. La build no incorpora ese avatar.

## Criterios de entrega y pendientes

| ID | Criterio F3A autorizado | Estado |
|---|---|---|
| F3A-01 | Recorrido vertical en Game, no demo separada | EVIDENCIA TÉCNICA ENTREGADA |
| F3A-02 | Tres personalidades, hechos reales y rescate único | EVIDENCIA TÉCNICA ENTREGADA |
| F3A-03 | Opciones correctas, cierre e historial del encuentro | EVIDENCIA TÉCNICA ENTREGADA |
| F3A-04 | Expresiones disponibles, fallback y limpieza | EVIDENCIA TÉCNICA ENTREGADA; lectura artística pendiente |
| F3A-05 | Pausa, ayuda, reanimación, muerte y reinicio | EVIDENCIA TÉCNICA ENTREGADA |
| F3A-06 | Controles sin bloqueo tras interrupción | Movimiento y eventos UI comprobados; captura física del cursor y recorrido manual con ratón PENDIENTES |
| F3A-07 | UI en tres resoluciones | EVIDENCIA TÉCNICA Y CAPTURAS ENTREGADAS; aprobación de legibilidad pendiente |
| F3A-08 | Creador, cambio de apariencia y persistencia F2 | REGRESIÓN TÉCNICA PASS; F2 conserva sus pendientes humanos y cuatro criterios parciales |
| F3A-09 | Entrega, documentación, hashes y rama aislada | ENTREGADO PARA REVISIÓN; no fusionar |

**Pendientes:** biblioteca definitiva de Chat 2; prueba manual de F2/F3A con teclado y ratón en ventana enfocada; validación de contenido y expresiones; revisión humana de derechos. No se probó desconexión física de red, Android, release no Development, hardware mínimo o sesión prolongada. No se repitió el selector nativo de archivo; se conservan sus evidencias anteriores. No se implementan memoria entre sesiones, promesas, voz, lip sync, romance, misiones, combate ni F3 completa.

La prueba Windows es una build real con automatización, no una sesión manual del usuario. Los controles UI se activaron con teclado virtual y eventos de navegación sobre botones reales; esto no acredita cada clic físico. Ver la [guía reproducible y recorrido manual](PRUEBAS.md).
