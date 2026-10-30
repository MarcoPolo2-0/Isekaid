# F3A N1/N2 — Preparación técnica de Chat 3

Fecha: 29/10/2026. Estado: **ANÁLISIS, SIN AUTORIZACIÓN DE IMPLEMENTACIÓN**.

Propuesta: [PR #14](https://github.com/MarcoPolo2-0/Isekaid/pull/14), corte `e363576c376ac466ad6ac2824da165e3ac0e8209`, incluida la presentación final de Chat 2. Código de contraste: main `24cad113be5edc8ca1d44db1857a3f23209a2dd5`. Árbol Unity común: `5bbd2d86fd8c017a705c0a85044689cb7f2e1773`.

**Conclusión:** N1 puede ser una modificación acotada de datos y de una etiqueta visible, usando el generador y los sistemas actuales. Requiere primero aprobación creativa por ID y resolver la convención de hashes descrita abajo. N2 necesita capacidades de presentación comprobadas y autorización separada: hoy no hay gestos sociales integrados. Este informe no aprueba textos, fichas, N2 ni el futuro PR de implementación.

Solo se añaden este análisis, resultados de simulación estática y readme. No se modifica Unity, biblioteca canónica/runtime, fichas, parche, GDD o decisiones. No se ejecutan Unity, Play Mode, builds, pruebas humanas o N2; no se abre el VRM privado. La oficialización documental del GDD sigue su trámite separado.

## 1. N1: aplicación selectiva y contratos que deben permanecer

El [parche v0.2](../../Specifications/F3A-N1/05_Parche_Editorial_v0.2_PROPUESTA.json) tiene **33 revisiones: 7 ACEPTAR, 23 MEJORAR y 3 DESCARTAR**. Son recomendaciones editoriales, no aprobaciones. Su candidato completo cambiaría 30 entradas y conservaría 30 completas. Las tres E.DOWNED permanecen canónicas; DESCARTAR no borra un nodo. Las decisiones C1–C5 de la [presentación final](../../Specifications/F3A-N1/09_Presentacion_Final_Direccion_v0.1.md) siguen abiertas; mantener también TSU.D.WAIT, por ejemplo, dejaría 29 cambios.

### Procedimiento para un futuro PR N1 autorizado

1. Registrar la decisión de Dirección con revisión/hash de propuesta, IDs exactos y campos/versión de texto autorizados. Lista vacía significa cero cambios; prohibir comodines. Una aprobación de ficha, del método o del merge documental no autoriza todos los diálogos. Nombres de trabajo, biografías y proyectos tampoco se vuelcan al perfil.
2. Leer la canónica vigente, comprobar la identidad de base acordada y los valores `expected`. Ante divergencia, detener y reconciliar; no sobrescribir una edición posterior. Aplicar directamente `recommended` de PR #14, nunca aplicar primero todo PR #11.
3. Cambiar exclusivamente `spokenText` e `intention` de los IDs/campos autorizados. Si Dirección aprueba solo el texto, conservar la intención previa. Una revisión creativa adicional necesita valores exactos nuevos y aprobación registrada; no se improvisa durante la integración.
4. Comparar estructuralmente antes/después: mismos 60 IDs y orden, tres temas, nueve raíces, 18 opciones/destinos, eventos/condiciones, personalidad, emoción/expresiones, prioridades, scopes, consumo/enfriamiento, cierres y efectos. Las entradas no seleccionadas quedan idénticas. Mantener writes vacío, trustDelta=0 y opciones sin cambio de clase/orden.
5. Validar el candidato en copia temporal con el [generador existente](../../../tools/build_f3a_runtime_content.py). Con autorización, actualizar la fuente y regenerar `Resources/Dialogue/F3A.es.json`; después ejecutar `--check`. El generador construye/verifica, **no aplica el parche ni concede aprobación**. `sourceSha256` cambia por el contenido; los IDs de entrada permanecen. No hace falta cambiar formatos canónico 1/runtime 2 ni el guardado del perfil.
6. Abrir el PR de implementación N1 independiente, desde main vigente y con alcance aprobado. Adjuntar diff por ID/campo, manifiesto de selección, resultados y capturas. El rollback es revertir esa revisión de fuente/recurso/etiqueta; no borrar perfiles o respaldos. Revisar primero cualquier edición posterior para no revertirla accidentalmente.

La selección explícita puede documentarse junto al informe de implementación; no se necesita un servicio o gestor de parches dentro del juego. Si se crea una herramienta puntual de aplicación, será de desarrollo, con dry-run y rechazo de bases/IDs/campos inválidos. En esta preparación no se añade esa herramienta al repositorio ni se crea un manifiesto que finja aprobación.

### Hallazgo técnico previo: hash de bytes frente a hash normalizado

La comprobación detectó una diferencia de saltos de línea, no una integración narrativa:

| Fuente | SHA-256 |
|---|---|
| Canónica local CRLF, declarada por el parche | `32a90c2d0d980b84d52d6a3c4e1eb18770b7ab9fbf86082562c8ced2833835ef` |
| Canónica normalizada UTF-8/LF, blob de main y `runtime.sourceSha256` | `e475943e9a9eaab1787277a8257d111af21c7ff8509bef5f8145b3e3fc4f214e` |
| Propuesta anterior, bytes CRLF declarados | `a0a1337dfce2d92eab9b2692f2d95bdf40751a7c62b0c2ed0df976c5cd7cbc98` |
| Misma propuesta anterior normalizada LF | `e6562ea5244bf74c59d241c94be05624ff36e348c603a819bef6b1783caa5d6a` |

**Requisito previo a N1:** fijar una convención explícita, preferiblemente SHA-256 UTF-8/LF como el generador, anclada además a commit/path/blob de base. Registrar los hashes crudos históricos por separado. Reconciliar esa metadata técnica al preparar la revisión autorizada sin alterar sus textos. No aceptar cualquier hash ni desactivar `expected` para salvar el caso CRLF/LF. El parche original se conserva intacto en esta tarea.

## 2. Etiqueta Dandere sin migración

La etiqueta del creador procede de `CompanionPalette.PersonalityNames[2]`; `CharacterCreationController` construye su Dropdown con esa lista y convierte el índice a enum. Solo se encontró una cadena visible «Tímida» en código propio. Tras autorización de implementación, bastaría cambiar esa entrada a Dandere y verificar la UI; los textos descriptivos no se reescriben aprovechando el cambio.

Conservar `CompanionPersonality { Tsundere, Yandere, Shy }`, `Shy=2`, clave `shy`, prefijo `F3A.SHY.*`, opciones/destinos y validación de perfiles 0–2. La selección persistida es el valor numérico; no se renombra el contrato ni se incrementa el formato para una etiqueta. Mantener históricos y fixtures que usan Shy. No añadir Deredere, una cuarta entrada, un alias de enum o una migración innecesaria.

Prueba específica futura: cargar perfiles v1/v2 con personalidad 2, comprobar identidad/selección/backup, mostrar Dandere, guardar/reabrir y confirmar que sigue serializado 2. Ejecutar los cambios de personalidad con conversación pendiente y los 18 recorridos, sin modificar los IDs para que coincidan con la etiqueta.

## 3. N2: inventario real y límites

Se leyó código/configuración de main y evidencia previa; presencia técnica y aceptación visual siguen separadas. No se inspeccionó una actuación nueva en esta tarea.

| Capacidad | Adventurer | VRM autorizado / VRM arbitrario | Tratamiento posterior |
|---|---|---|---|
| Locomoción | Solo Idle/Walk/Run admitidos en `Art/Companion/Adventurer/Animations`; Humanoid integrado | Mismo controlador sobre control rig normalizado; ensayos acotados existentes | Conservar capa base, fase, Speed/Cadence y autoridad NavMesh |
| Cara | Mapeos happy/sad/surprised/blink/aa; regresión existente comprueba happy/blink | POC reporta 14 presets reales del fixture; otros avatares pueden omitirlos | Solicitar por nombre/capacidad; no copiar índices ni dar por certificada intensidad |
| Expresiones condicionales | No certificadas como actuación final | `_if_validated` continúa en neutral con el código actual | No desbloquearlas por cambiar texto/intención; requiere revisión visual |
| Cabeza/cuello/manos | Mapeados en el Avatar Humanoid | Humanoid validado, huesos opcionales dependen del modelo | Posibilidad técnica; amplitud, ejes y deformaciones necesitan prueba real |
| Ojos/mirada | El mapeo de LocomotionBuilder **no declara LeftEye/RightEye** | UniVRM expone LookAt; configuración y soporte dependen del archivo | No prometer seguimiento ocular universal; cabeza/neutral o efecto omitido según capacidad |
| Gestos sociales | No hay clips sociales admitidos ni capa de gestos actual | VRM no aporta automáticamente clips de gestos; tampoco hay un catálogo social integrado | No usar Walk/Run como gesto. Nuevo clip requiere procedencia, licencia, retarget y aceptación |
| Ritmo/voz | Texto inmediato; temporización facial existente | Igual; no hay voz grabada ni prosodia implementada | Ritmo N2 será presentación visual; no supone TTS, voz o mecanografía |

Fuentes: [LocomotionBuilder](../../../UnityProjectIsekaid/Assets/_Project/Editor/LocomotionBuilder.cs), [apariencia](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionAppearance.cs), [reacciones](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionReactionPresenter.cs), [POC facial histórico](../VRM-POC/INFORME.md#expresiones-y-recuperación) y [ensayo de locomoción](../Companion-Locomotion-Render/INFORME.md). El informe de POC no certifica todos los VRM ni nuevos gestos.

### Diseño mínimo propuesto para una autorización N2 posterior

**Ritmo.** Ampliar la temporización del `CompanionReactionPresenter` para inicio, sostén y salida de una actuación aprobada, medida en tiempo activo y cancelable. Texto completo y opciones disponibles de inmediato; Downed sin demora social. No usar Time.timeScale, Animator.speed global, cadencia de caminar ni retrasar el evento narrativo para crear «personalidad». `AvatarLocomotion` ya escribe Animator.speed por escala: modificarlo para ritmo produciría interferencia con el seguimiento.

**Selección.** Datos locales de presentación por personalidad técnica e intención real, con valores acotados y señales semánticas permitidas. Admitir biblioteca parcialmente aplicada: intenciones antiguas, nuevas o desconocidas deben tener fallback neutro/omitido, sin errores ni textos alternativos inventados. No interpretar la prosa de las fichas como órdenes o código, ni inferir gestos por palabras del diálogo. El formato exacto de esos datos se resolverá en la especificación N2; no ampliar el esquema de N1 para adelantarlo.

**Gestos.** Solo con clip compatible/licenciado y máscara apropiada: capa superior o aditiva controlada desde el presentador visual, sin reemplazar ni reiniciar la locomoción. No llamar `PreviewMotion/SetMotion` con un nombre de gesto: el helper actual está diseñado para Idle/Walk/Run y una solicitud desconocida puede llevar Speed a reposo. Añadir, si se autoriza N2, una solicitud visual específica que devuelva éxito/capacidad. Al terminar, bajar el peso de la capa, sin forzar Idle. Restringir primeras pruebas a Active seguro y quieto; caminando/corriendo omitir el gesto hasta probar que no afecta locomoción, equilibrio o pies.

**Mirada.** En el puente VRM aprovechar LookAt configurado y restaurar sus valores previos al salir; no escribir índices faciales supuestos ni huesos crudos después del procesamiento del SDK. Para Adventurer, cabeza Humanoid con objetivo ya conocido por la interacción, límites y transiciones probados; seguimiento ocular omitido mientras no exista mapeo válido. No girar el agente, cambiar destino o introducir percepción nueva para atender una mirada. La pose/IK del control rig debe ocurrir antes de su transferencia, constraints, mirada, expresión y Spring Bones; no duplicar Runtime.Process por frame. [Orden oficial UniVRM](https://vrm.dev/en/api/api_update/).

Si se elige IK de Animator para la cabeza, el controlador actual tiene IK Pass desactivado y el Animator está en el modelo, separado del controlador de compañera. El callback exige Humanoid, IK Pass y el script en el objeto del Animator. En ese caso se justifica **un único relay genérico pequeño por Animator**, creado/liberado con la apariencia; no un controlador por personalidad. No asumir que agregar OnAnimatorIK al padre existente bastará. Esa necesidad y su orden se comprobarán en Unity 6000.6.7f1 antes de adoptar la ruta. [Requisitos de OnAnimatorIK](https://docs.unity.com/en-us/engine/6000.0/script-reference/unityengine/monobehaviour/onanimatorik). También se inspeccionó la implementación local fijada de UniVRM 0.131.4; su Vrm10Instance ofrece LookAtTarget/LookAtTargetType y procesa por defecto en LateUpdate.

Los matices corporales de la [matriz de Chat 2](../../Specifications/F3A-N1/06_Matriz_Reacciones_Actuacion_v0.1.md) se convierten en parámetros solo después de aprobarlos y probar capacidades. Sin capacidad, mantener texto/elección y omitir el canal; un fallback funcional no acredita diferenciación corporal.

## 4. Reutilización y ciclo de vida

| Pieza actual | Responsabilidad posterior | Límite |
|---|---|---|
| DialogueContent / generador | Datos narrativos aprobados, IDs y validación | N1 no añade decisiones, memoria, sensores o control de avatar |
| DialogueDirector | Elegibilidad, cursor, consumo, episodios y eventos | Conserva reglas; no contiene retargeting o SDK |
| CompanionController | Único enlace entre presentación realmente nueva, estado y vista | No reproducir gestos desde cada Changed |
| CompanionReactionPresenter | Emoción, temporización/cancelación de salida visual | Ampliación acotada; sin tocar perfil, Trust o órdenes |
| CompanionAppearance | Reutilizar selección/capacidades de catálogo o VRM | Solicitudes pequeñas tipo Try, con fallo seguro; no gestor nuevo paralelo |
| VrmAppearance | Detalle SDK, Animator y vida del recurso | Mantener VRM10 aislado en su asmdef; no añadir dependencia directa a Isekaid.Runtime |
| DialoguePresenter | Texto, opciones, foco y refresco | No decide actuación ni retrasa acceso a opciones |

**Riesgo concreto de doble inicio:** `DialogueDirector.Present` hace Notify/Changed antes de Emit/Presented; hoy `OnDialogueChanged` y `Speak` pueden llamar a `reactions.Show`. Es inocuo para el mapeo facial actual, pero iniciar un gesto desde Show lo lanzaría dos veces o lo reiniciaría al volver de pausa. Usar `Presented → Speak` como disparo único de actuaciones transitorias; Changed refresca/restaura presencia válida sin nuevo inicio ni consumo. No modificar ahora ese orden.

Una identidad local de presentación que avance solo al recibir Presented puede invalidar trabajos pendientes. `Dialogue.Revision` por sí sola no identifica un evento narrativo: también cambia al suspender/refrescar UI. Separar inicio único de re-aplicación de cara/pose sostenida; al volver del creador no repetir el gesto inicial ni Spoke. Este control es transitorio, no memoria de personaje ni un campo nuevo de guardado.

| Situación | Regla de limpieza/restauración N2 a probar |
|---|---|
| Pausa / pérdida de foco | Detener reloj de actuación y limpiar/suspender salidas según especificación; al volver no reiniciar inicio |
| Cerrar / reabrir raíz pendiente | Conservar opciones/consumo; limpiar objetivo y capas. Reaplicar solo presencia válida, sin otra presentación |
| Creador aditivo | Sin eventos sociales autónomos en preview; encuentro permanece suspendido. No reutilizar referencias del cuerpo anterior |
| Cambio de avatar válido / fallido | Resolver capacidades al publicar el avatar; conservar transacción/rollback. Desvincular referencias antiguas y no volver a consumir contenido |
| Cambio de personalidad | Invalidar actuación previa y rebind del cursor con IDs vigentes; no convertirlo en evento de rescate |
| Downed / Dead | Cancelar inmediatamente pausas/gestos/miradas sociales; mantener prioridad y reloj de supervivencia actuales |
| Reinicio / destrucción | Invalidar tickets/tareas, borrar referencias y capas; sin callbacks que afecten al encuentro nuevo o al modelo destruido |

No proponer nuevos PersonalityManager, NarrativeManager, MemoryManager, GestureManager o componentes por arquetipo. El relay de IK solo se admite por la ubicación del callback, si resulta necesario. No hay una nueva dependencia de paquetes, servicios generativos o APIs de pago requerida por este análisis.

## 5. Pruebas exigidas antes de aceptar la implementación

Las pruebas siguientes son un plan, no resultados de Unity actuales. Reducen riesgos y aportan evidencia; la diferenciación percibida y la calidad corporal requieren evaluación humana.

| Grupo | Ensayo automatizado futuro | Revisión manual |
|---|---|---|
| Selección N1 | Base/expected correctos; cero IDs, subconjunto, selección completa, duplicados/desconocidos/comodines/campos prohibidos rechazados; entradas no seleccionadas idénticas | Dirección compara el diff por ID con su aprobación C1–C5, sin cambios incidentales |
| Contrato narrativo | Generador --check; 60 IDs/orden, 18 rutas/eventos/condiciones/efectos/cooldown iguales; 18 recorridos en encuentros independientes | Leer voz y hechos en los mismos contextos; no introducir romance, recuerdos, decisiones del jugador o «esperas» mecánicas por redacción |
| Dandere / perfil | Perfil v1/v2=2 antes/después, identidad/selección/legacy/backup idénticos; tres valores, Shy/shy/SHY conservados; etiqueta visible correcta | Creador, cambio en encuentro y reapertura; ninguna cuarta opción |
| F2 | Repetir batería existente: Editor, Windows y segundo proceso sin fuente; carga/cancelación/fallos por etapas, copia dañada, recuperación y recursos liberados; metadatos adversariales | Selector de archivo, avatar autorizado local, retorno a misma compañera y UI a 1280/1600/1920 e intermedios |
| F3A | Repetir batería vigente (referencia 285 comprobaciones/18 rutas), sumando casos sin reducir cobertura; raíz y hoja interrumpidas, doble reanimación y revisiones de UI obsoletas | Hablar, elegir, cerrar, moverse, volver al creador y retomar sin duplicar línea/recompensa |
| Supervivencia | Trapped sin reloj; Downed 30 s de tiempo activo, pausa/foco, reanimación, expiración/muerte y reinicio; sin nuevo campo de salud/checkpoint | Urgencia inmediata y ningún gesto/pausa que dificulte rescate o congele el reloj |
| Locomoción + N2 | NormalizedTime sin reinicios, ruido 2.96/3.04 m/s, frenadas, arranque y dirección en ambos avatares; capa social vuelve a cero y no altera Speed/Cadence/Animator.speed/NavMesh | Seguimiento largo, pies, hombros/cuello, apoyos y transición de gesto a caminar/correr |
| Capacidades | Cara ausente, ojos ausentes, clip ausente, intención vieja/nueva/desconocida; éxito o no-op seguro; sin excepciones y texto/opciones presentes | Inventario real por modelo; pose, intensidades, ojo/cabeza, Spring Bones, ropa y límites sin asumir equivalencia universal |
| Ciclo N2 | Una sola puesta en marcha por Presented; pausa/creador/rebind/avatar fallido/válido/destrucción; callback tardío rechazado por identidad de presentación y de avatar | Cero gestos repetidos en bucle, mirada fija o residuos al cerrar/volver/reanimar |
| Entrega | Compilación, Play Mode, Windows visible, estructura/715 originales, hashes y logs; separar evidencia actual de histórica | Capturas antes/después, cursor/inputs físicos y comparación visual; no usar ScreenCapture negro de ventana oculta como prueba |

Los valores 43 Editor / 36 Windows / 2 reinicio F2, 285/18 F3A, guardas 9+2, almacenamiento 11 y metadatos 35 son **referencias históricas** de las baterías disponibles. Una versión ampliada puede tener más aserciones; no se considera aprobada por conservar un número si quitó cobertura. Persisten los criterios PARTIAL y límites de offline físico, derechos y plataformas previos.

Para N1 usar el [protocolo ciego de voz](../../Specifications/F3A-N1/07_Prueba_Ciega_Aceptacion_v0.1.md) tras ratificación de Dirección, con versiones/IDs y orden documentados. Para N2 evaluar cuerpo por separado, mismo avatar/cámara/escena y sin texto/voz/cara que revelen la plantilla; registrar capacidades reales y vuelta a reposo. No declarar superado F3A-R10 por JSON válido, textos distintos o fallback. Las capturas VRM privadas no se publican sin autorización de derechos.

## 6. Simulación ejecutada ahora, sin implementar

Se simuló en memoria y archivos temporales descartables, llamando al `build()` del generador sobre una copia de fuente. **No se escribió su TARGET ni se sustituyó el recurso del juego.** Resultados en [Impacto_Tecnico_Chat3.resultados.json](Impacto_Tecnico_Chat3.resultados.json):

- **40 selecciones válidas:** ninguna, las 30 recomendaciones, los tres descartes como no-op, 30 selecciones individuales, tres subconjuntos por personalidad, mezcla, supuesto C4 de 29 cambios y autorización de solo texto/intención por separado.
- **Siete rechazos:** hash distinto, ID desconocido, comodín, duplicado, emotion/playerOptions prohibidos y expected obsoleto.
- En los candidatos construidos: 60 entradas, 18 rutas, campos estructurales iguales y entradas no seleccionadas íntegras. Selección completa: 30 cambios/30 intactas. **Canónica y runtime reales: cero cambios.**
- Comprobado el hallazgo CRLF/LF contra blob de main y sourceSha256 runtime, preservando los valores originales del parche. Los 16 archivos protegidos y todo el árbol Unity permanecen idénticos al corte de entrada. No se otorga aprobación a ningún ID.

La simulación no acredita integración, calidad creativa, N2, rendimiento o prueba humana. No se crean nuevos personajes, recuerdos, romance, clases o mecanismos.

## 7. Secuencia recomendada de autorización

1. Dirección resuelve fichas, C1–C5 y candidatos/campos por ID del PR #14; puede aprobar solo una parte. Ratifica su revisión de fuente y protocolo.
2. Conciliar el contrato de hashes sin modificar inadvertidamente contenido; preparar una base actual verificable para aplicación.
3. Tras autorización explícita, Desarrollo abre **otro PR N1 de implementación**: fuente/recurso seleccionados y, si se incluye en alcance, etiqueta Dandere. Sin actuación N2, cuarta personalidad o nuevas mecánicas.
4. Ejecutar y revisar regresiones/lectura ciega; mantener claros sus pendientes.
5. Encargar N2 aparte: inventario en Unity, clips/máscaras o mirada mínima compatibles, límites y prueba corporal aprobados antes de ampliar actuación. Esta preparación no inicia esa implementación.

El PR #14 continúa como propuesta creativa/documental; añadir este análisis no aprueba ni aplica sus recomendaciones. No fusionar automáticamente.
