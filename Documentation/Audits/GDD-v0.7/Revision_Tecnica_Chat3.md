# GDD v0.7 — Auditoría técnica de Chat 3

**Fecha:** 29/10/2026, Guatemala. **PR:** [#13](https://github.com/MarcoPolo2-0/Isekaid/pull/13).

**Documento de entrada:** `448c6f9bf31ec2be23256fc92bfcb40cdf3db194`, [GDD v0.7 borrador](../../GameDesignDocument_v0.7_borrador.md).

**Código de contraste:** `main` `f835d3ba665b6d63cf94894bfaccd9a94ce783ce`.

**Dictamen:** **APTO TÉCNICAMENTE COMO CANDIDATO A DOCUMENTO MAESTRO**, con las precisiones documentales de esta revisión y los pendientes expresos. Dirección decide su promoción. No se declara aprobado, no se renombra el borrador y no se fusiona el PR.

## 1. Alcance y procedencia de la comprobación

Se inspeccionaron código propio, configuración serializada, biblioteca canónica/runtime y resultados F2/F3A/locomoción. Se consultaron DEC-007, workflow v1.2 y las revisiones de Dirección en [PR #12](https://github.com/MarcoPolo2-0/Isekaid/pull/12#pullrequestreview-5702979966) y [PR #13](https://github.com/MarcoPolo2-0/Isekaid/pull/13#pullrequestreview-5704117961). La auditoría editorial de Chat 2 se conserva intacta.

El árbol Git completo `UnityProjectIsekaid/` es **idéntico** en main, el commit de entrada del PR y la implementación ensayada de locomoción `11358db57d90bac3ba62dfc0edd130a2fcd62d73`: `5bbd2d86fd8c017a705c0a85044689cb7f2e1773`. La rama documental usa checkout disperso; los archivos ausentes en disco se leyeron de los objetos de main con `git show`, sin añadirlos o cambiarlos. Las verificaciones existentes que necesitan todos los assets se ejecutaron en el checkout completo de Desarrollo, cuyo árbol Unity es el mismo.

**En esta tarea no se ejecutaron Unity, compilación, Play Mode, builds o pruebas manuales nuevas.** Se volvió a comprobar la correspondencia estática de fuente/runtime y se contrastaron las evidencias históricas con el código actual. No se atribuyen sus ensayos a este PR documental. No se abrió ni modificó el VRM privado y no se publicaron capturas restringidas.

## 2. Afirmaciones verificadas

PASS significa que la afirmación documental coincide con el código/evidencia en el alcance indicado. PARTIAL conserva un límite de aceptación del producto; no significa que un sistema futuro deba implementarse para aprobar un GDD que lo identifica correctamente como pendiente.

| ID | Afirmación y secciones | Contraste técnico | Resultado y límite |
|---|---|---|---|
| GDD-T01 | VRM 1.0 local, §§3/14 | VrmFileGuard + VrmAppearance: GLB incorporado, versión estable 1.0, guardas y UniVRM 0.131.4; `canLoadVrm0X:false` | PASS acotado. Windows ensayado; VRM 0.x, otros sistemas y compatibilidad universal no acreditados |
| GDD-T02 | Catálogo e independencia, §§3/4 | Un catálogo `adventurer-02`; selección separada del perfil y del CompanionState vivo; cambio transaccional de avatar | PASS. Clase, evolución y memoria de campaña todavía no tienen sistema funcional; tres avatares de catálogo siguen futuros |
| GDD-T03 | Locomoción y presentación, §§3/13/17 | Controlador Humanoid con Idle + BlendTree Walk/Run, Speed amortiguado, Cadence, histéresis y agente como autoridad; luz corregida con materiales/texturas intactos | PASS técnico; **PARTIAL artístico**, LOC-05/REN-04. Sin bloqueo físico de pies, aceptación universal o arte final |
| GDD-T04 | Persistencia de perfil, §§13/14 | CompanionProfileStore.Record guarda perfil, apariencia y legado; no incluye CompanionState, DialogueDirector o mundo | PASS tras precisar alcance y presupuestos. No es una partida completa |
| GDD-T05 | Recuperación, §14 | Temporal validado antes del reemplazo, relectura del destino, respaldo, original ilegible conservado y VRM por hash; fallback anunciado | PASS en ensayos acotados. Sin prueba de corte eléctrico real o toda condición de disco |
| GDD-T06 | 60 entradas/18 rutas, §§6/17 | Fuente y runtime actuales: 20 por personalidad, tres temas, nueve raíces y 18 hojas/destinos alternativos; efectos de memoria/confianza vacíos/cero | PASS estático y en evidencias de 18 recorridos. Son alternativas en encuentros independientes, no 18 elecciones de una sola partida |
| GDD-T07 | Continuidad, §§6/13 | Close conserva raíz con opciones; terminal sin opciones se libera; creador aditivo preserva instancia/cursor/foco; pausa e incapacidad suspenden; muerte/reinicio invalidan | PASS **en memoria del encuentro**. No hay restauración de ese cursor tras cerrar el proceso |
| GDD-T08 | Incapacidad, 30 s y pausa, §7 | CompanionState usa Trapped/Active/Downed/Dead y `RescueWindowSeconds=30f`; Tick recibe tiempo escalado, ignora valores inválidos y solo descuenta en Downed | PASS técnico. Cero HP/daño de combate y ratificación formal de balance siguen pendientes |
| GDD-T09 | Reinicio frente a checkpoint, §§7/11/12/13 | Restart → ResetScenario + ResetPosition: estado nuevo, trampa/posiciones iniciales y limpieza del diálogo; no lector de checkpoint | PASS como distinción documental. Campaña/checkpoints aún no implementados |
| GDD-T10 | Shy/2/shy/SHY frente a Dandere, §§4/16/17 | Enum con tres valores, Shy implícitamente 2; perfil válido solo 0–2; conversor `shy:2`; IDs `F3A.SHY.*` y opciones conservados; UI dice Tímida | PASS. Cambiar solo texto visible es compatible sin migrar datos. No se cambia ninguna etiqueta en esta tarea |
| GDD-T11 | Funciones presentes frente a propuestas, §§4/5/6/10–18 | 33 pares expected/set siguen en expected tanto en fuente como runtime; perfiles de actuación no consumidos; Deredere ausente de código propio/contenido runtime | PASS: **0/33 aplicados**, sin cuarta opción. Combate, clases, hogar, secuestro, promesas, aprendizaje y romance siguen futuros |
| GDD-T12 | Prueba manual y aceptación, §§3/13/17 | Reporte manual de Dirección recogido en readme y auditoría editorial; matrices F2/F3A/locomoción conservan PARTIAL | **PARTIAL de aceptación**, correctamente documentado. La observación de mejora no es aceptación total ni una nueva prueba presencial de Chat 3 |

## 3. Contratos concretos y trazas de código

### Importación, catálogo y presentación

Proyecto Unity 6000.6.7f1, URP 17.6.0, UniVRM/UniGLTF 0.131.4 en `ProjectVersion.txt` y `Packages/manifest.json`. [VrmFileGuard](../../../UnityProjectIsekaid/Assets/_Project/AvatarRuntime/VrmFileGuard.cs) limita archivo a 32 MiB y JSON a 4 MiB, rechaza URI externas y data URI, admite un BIN incorporado y PNG; impone además 512 nodos, 32 mallas, 64 materiales/imágenes, 250,000 vértices acumulados y presupuestos de accessors, morphs, jerarquía y Spring Bones. Cada PNG queda limitado a 4096×4096 y el total a 67,108,864 píxeles. Son límites de esta implementación, no una promesa de aceptar cualquier VRM válido según el estándar.

[VrmAppearance.LoadLocal](../../../UnityProjectIsekaid/Assets/_Project/AvatarRuntime/VrmAppearance.cs) prepara y valida candidato, Humanoid, control rig y materiales; activa antes de retirar el avatar anterior y conserva el previo ante fallo/cancelación. El controlador asignado desde ambas escenas es `Locomotion.controller`, no TrialAnimations.controller del POC. [AvatarLocomotion](../../../UnityProjectIsekaid/Assets/_Project/AvatarRuntime/AvatarLocomotion.cs) amortigua Speed en 0.15 s y adapta cadencia a escala efectiva; [LocomotionBuilder](../../../UnityProjectIsekaid/Assets/_Project/Editor/LocomotionBuilder.cs) configura entrada >0.12 y salida <0.06 m/s, mezcla Walk/Run y bucles Humanoid. El POC histórico se conserva.

La corrección actual modifica luz direccional a 0.65 blanca y ambiente uniforme 0.4 en ambas escenas. No cambia el archivo VRM, sus texturas o sus shaders. Las imágenes y secuencias privadas del ensayo siguen fuera de Git. Los perfiles de actuación propuestos no se confunden con esta locomoción.

[CompanionReactionPresenter](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionReactionPresenter.cs) conserva emoción y pide expresiones compatibles; los preferidos `_if_validated` vuelven al fallback neutral hasta certificación. `intention` es un campo transportado de contenido, sin consumidor de gestos/mirada/postura. El contenido puede seguir funcionando aunque falte una cara compatible.

### Perfil y recuperación

[CompanionProfileStore.Record/TrySave](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionProfileStore.cs#L19) contiene formato 2, CompanionProfile, AvatarSelection y JSON legacy. El perfil incluye identidad, enum de personalidad y cuatro índices históricos de color; no demuestra recoloración universal de VRM. Apariencia guarda catálogo o hash/versión/metadatos/confirmación de uso local. El legado se conserva como datos, no se convierte por sí solo en clase/misión funcional.

| Límite/operación | Implementación verificada |
|---|---|
| Perfil | Sobre final UTF-8 de hasta **1,048,576 bytes**; excederlo rechaza antes de reemplazar |
| Metadatos persistibles | Hasta **32,768 bytes** UTF-8 saneados; lista de campos conocidos; desconocidos omitidos solo del perfil |
| Autores/referencias | Hasta 16 autores de 256 bytes y ocho referencias de 2048 bytes; más límites por campo |
| Condiciones de uso conocidas | Tipos, controles y tamaños se validan; no se truncan textos legales para entrar en presupuesto |
| Copia VRM | Bytes originales completos en `Companion/Avatars/<hash>.vrm`, independientes del sobre del perfil |
| Escritura | Flush del temporal, mismo lector y payload esperado antes de Replace/Move; relectura del destino antes de éxito |
| Perfil ilegible | Se intenta respaldo válido y se avisa. Si ambos archivos existentes son ilegibles, guardar requiere confirmación; originales se conservan |
| Avatar perdido/dañado | Se anuncia catálogo de recuperación; perfil/referencia no equivalen a campaña recuperada |

Fuentes: [AvatarSelection y AvatarFiles](../../../UnityProjectIsekaid/Assets/_Project/AvatarRuntime/AvatarSelection.cs), [store](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionProfileStore.cs#L86), [corrección de metadatos](../VRM-F2/metadata-review/INFORME.md). El límite de 1 MiB del perfil no limita a 1 MiB el archivo VRM ni su JSON: son presupuestos distintos.

### Diálogo, tiempo y reinicio

[DialogueDirector](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/DialogueDirector.cs#L125) mantiene Current, opciones, consumo, temas terminados, episodios y foco como campos en memoria. Close no reinicia la raíz con opciones pendientes; un turno terminal no queda prometido como cursor recuperable. ResetEncounter limpia todo; muerte invalida la conversación. [CompanionPrototype](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Prototype/CompanionPrototype.cs#L149) mantiene el Game vivo al visitar el creador aditivo y vuelve a la misma instancia. No hay un archivo de conversación guardada entre procesos.

[CompanionState](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionState.cs#L11) fija 30 s. [CompanionController.Update](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionController.cs) llama Tick con Time.deltaTime; [SetPaused](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Prototype/CompanionPrototype.cs#L119) cambia Time.timeScale y suspende el director. Perder foco pausa; abrir diálogo solo bloquea entrada, sin poner por sí mismo escala cero. Downed/Dead bloquean apertura del creador. Trapped no descuenta y Dead no permite Rescue. El botón de ensayo provoca Downed sin un sistema actual de salud/daño.

[Restart](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Prototype/CompanionPrototype.cs#L142) crea un estado nuevo mediante ResetScenario, borra RemembersFirstRescue/Trust/cursor/consumo/episodios, restablece trampa/posiciones y conserva personalidad seleccionada y apariencia. El primer rescate puede volver a dar Trust=1 en **ese nuevo encuentro**, sin ser una recompensa acumulable de campaña. Esta semántica es distinta de volver del creador y de cargar un checkpoint, aún futuro.

### Compatibilidad de personalidad y propuesta editorial

La correspondencia actual es `Tsundere=0`, `Yandere=1`, `Shy=2`; `CompanionProfile.IsValid` admite únicamente ese rango. `CompanionPalette.PersonalityNames[2]` sigue siendo Tímida. [Conversor canónico](../../../tools/build_f3a_runtime_content.py) usa `shy:2`; DialogueContent.Rebind usa prefijo SHY, conservando sufijos y destinos. Una futura etiqueta Dandere puede cambiar sin renombrar enum, campos serializados, clave de contenido o IDs. Esta auditoría no hace ese cambio ni autoriza ampliar a cuatro opciones.

Se compararon los **33 edits** del [parche propuesto](../../Specifications/F3A_Diferenciacion_Editorial_v0.1_PROPUESTA.json) contra fuente y runtime: 33 siguen coincidiendo con expected, ninguno con set. IDs, opciones y rutas coinciden entre ambas bibliotecas. La propuesta declarada de actuación no tiene consumidor actual. No se ejecutó el generador sin `--check` ni se aplicó el parche.

## 4. Evidencia histórica y pendientes que permanecen

| Evidencia entregada antes de esta auditoría | Resultado registrado, no reejecutado aquí |
|---|---|
| [Locomoción Editor](../Companion-Locomotion-Render/editor-results.json) y [Windows visible](../Companion-Locomotion-Render/windows-results.json) | Dos avatares, seis muestras de clips, cuatro seguimientos; cero reinicios internos/cambios rápidos y comparación de iluminación |
| [F3A Editor](../Companion-Locomotion-Render/f3a-editor-results.json) y [Windows](../Companion-Locomotion-Render/f3a-windows-results.json) | 285 comprobaciones y 18 recorridos por plataforma |
| [F2 Editor](../Companion-Locomotion-Render/f2-editor-results.json), [Windows](../Companion-Locomotion-Render/f2-windows-results.json), [reinicio](../Companion-Locomotion-Render/f2-restart-results.json) | 43, 36 y 2 comprobaciones respectivamente |
| Guardas/almacenamiento/metadatos del mismo directorio | Nueve rechazos + dos límites, 11 y 35 comprobaciones |
| [Build y hashes](../Companion-Locomotion-Render/build-results.json) | Windows x64 Development Mono, 196,472,292 bytes; no acredita release, Android u otras plataformas |

Para aceptar criterios completos prevalecen las matrices de revisión, no un PASS de cada fixture del runner:

- **F2 VRM-AC01:** falta prueba física con red desconectada. Runtime local sin servicios obligatorios no sustituye ese ensayo.
- **F2 VRM-AC12:** faltan clase, misiones y rasgos evolucionados funcionales para probar su conservación.
- **F2 VRM-AC22:** no hay eliminación de biblioteca ni acreditación de formatos futuros de partida.
- **F2 VRM-AC24:** combate, secuestro y secuencias importantes requieren sistemas/pruebas posteriores.
- **F3A-D08:** aceptación editorial de voz/contenido pendiente; revisar un GDD no acepta automáticamente la biblioteca ni los 33 cambios.
- **F3A-R10:** ritmo/postura/gesto y comparación humana de personalidad pendientes.
- **F3A-R11:** opciones ajenas/dobles/obsoletas probadas; callbacks narrativos asíncronos y cola diferida aún no implementados ni probados.
- **LOC-05/REN-04:** calidad de apoyo/deslizamiento, piel, ropa, postura y luz pendientes de aceptación artística. El movimiento residual está documentado y no hay bloqueo físico de pies.

También continúan revisión humana de derechos, otros VRM/escalas/terrenos, presupuesto de producción, sesiones prolongadas, interacción física/captura del cursor y limitaciones del ensayo a 60 FPS. Los errores internos de Search/Hierarchy del Editor documentados no se ocultan ni se confunden con excepciones de gameplay. El primer ScreenCapture Windows oculto daba negro y fue sustituido por una repetición visible de la misma build.

La observación manual de Dirección se conserva como **reporte humano atribuido** en [readme](../../../readme.txt) y [revisión editorial](Revision_Editorial_Chat2.md); no se repite ni se convierte en cierre de las matrices anteriores. La revisión de #12 autorizó fusión técnica experimental, conservando pendientes de validación presencial/artística. Aprobar v0.7 tampoco declara terminadas F2, F3A o F3 completa.

## 5. Correcciones documentales realizadas y motivo

| Hallazgo | Precisión en el GDD |
|---|---|
| T-C01 — Capas futuras descritas como datos actuales | §3 identifica la separación completa como regla de diseño y limita lo implementado a perfil/estado existente |
| T-C02 — Guardado sin contrato explícito | §14 incorpora campos, 1 MiB/32 KiB, copia por hash, recuperación y límites VRM, sin prometer partida completa |
| T-C03 — Continuidad demasiado general | §§6/13 distinguen cursor pendiente en memoria, turno terminal, creador aditivo, reinicio y nuevo proceso |
| T-C04 — Pausa y reloj con alcance implícito | §7 explicita tiempo escalado, Trapped/Downed/Dead, pérdida de foco, conversación y bloqueo del creador |
| T-C05 — Mejora de materiales podía sugerir alteración del modelo | §§3/13 precisan iluminación corregida con materiales/shaders/texturas conservados; intención no equivale a actuación |
| T-C06 — Puerta técnica y criterios aún genéricos | §17 enumera parciales F2/F3A/locomoción y vincula este informe; §§inicio/19 registran el corte técnico sin aprobar el maestro |

No se cambian textos del juego, arquetipos, reglas de combate, escenas, prefabs, bibliotecas ni los GDD históricos. La nomenclatura Shy/Dandere ya era técnicamente correcta en `448c6f9`; se verifica y conserva. La revisión editorial previa permanece íntegra.

## 6. Comprobaciones de esta revisión y dictamen

Resultados reproducibles en [Revision_Tecnica_Chat3.resultados.json](Revision_Tecnica_Chat3.resultados.json): igualdad del árbol Unity con main/implementación ensayada, archivos históricos/decisiones/editorial/canónica/runtime protegidos, 60 entradas/18 rutas y 0/33 ajustes aplicados, contrato Shy/2, enlaces locales contra rutas versionadas y readme conservado como prefijo. `tools/build_f3a_runtime_content.py --check` PASS. Las verificaciones existentes de estructura y 715 originales PASS en el checkout completo equivalente, sin abrir Unity.

**Recomendación técnica:** el contenido corregido es apto para que Dirección decida convertirlo en documento maestro. No quedan contradicciones técnicas bloqueantes dentro del alcance auditado; permanecen las decisiones de diseño y aceptación de producto enumeradas. No se requiere implementar los sistemas pendientes para que un GDD los describa como visión futura, ni su eventual promoción autoriza implementarlos automáticamente.

**Esta entrega no fusiona #13, no declara v0.7 APROBADO y no autoriza los 33 ajustes, Deredere o nuevas mecánicas.**
