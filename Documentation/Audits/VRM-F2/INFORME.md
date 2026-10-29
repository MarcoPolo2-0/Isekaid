# ISEKAID â€” F2: creador de compaÃ±era y apariencia VRM

**Fecha:** 28/10/2026, Guatemala. **Unity:** 6000.6.7f1. **Rama:** `feature/isek-vrm-f2-character-creator`.
**Base:** `main` 480a8e9, con PR #3 y #4 incorporados. **Estado:** entrega experimental para revisiÃ³n de DirecciÃ³n; no fusionar ni iniciar F3.

**Revisión posterior de Dirección:** [corrección del presupuesto de metadatos, roundtrip y pruebas](metadata-review/INFORME.md). Incluye la documentación del PR #6, sin fusionar main. Los resultados del cuerpo de este informe conservan la entrega inicial.

## 1. Resultado y alcance

El creador permite elegir Adventurer, importar un VRM 1.0 local, girar la vista previa, probar animaciones y expresiones, seleccionar una de las tres personalidades, guardar y entrar a Game. La apariencia se carga mediante el importador transaccional existente; se conservan su validaciÃ³n, sus GUID y el controlador de compaÃ±era del prototipo.

Al volver al creador desde Game se conserva el mismo encuentro en pausa. Se mantienen la instancia de `CompanionController`, su `CompanionState`, confianza, recuerdo del primer rescate, posiciÃ³n y reglas de movimiento. Al regresar se restauran cÃ¡maras, luces, audio y HUD. El cambio se bloquea mientras la compaÃ±era estÃ¡ incapacitada o muerta. No existe todavÃ­a un sistema completo de combate, secuestro, misiones o memoria narrativa sobre el cual acreditar los casos futuros.

No se modifican Woman, G2-PROTO, G2-CORR ni los prefabs originales. Se verifican 715 archivos protegidos sin cambios de contenido; tambiÃ©n se conservan los archivos `.meta` de las escenas. Las dos escenas autorizadas reciben referencias al catÃ¡logo y al controlador visual; CharacterCreation ajusta cÃ¡mara y capa de vista previa. `CompanionPrototype.unity` permanece como escena histÃ³rica.

## 2. ImplementaciÃ³n

- `AvatarRuntime/`: se traslada el importador de PR #4 conservando ensamblado, espacio de nombres y GUID. Se aÃ±aden cancelaciÃ³n, selecciÃ³n por hash, validaciÃ³n de metadatos y selector nativo Windows. No se aÃ±ade otra dependencia de importaciÃ³n ni un segundo controlador de personalidad.
- `CompanionAppearance`: conserva las referencias de colores antiguas y coordina catÃ¡logo, VRM y muestras visuales. Los tintes histÃ³ricos no se aplican a texturas o Ã­ndices de morphs ajenos. Los colliders del VRM se desactivan; navegaciÃ³n e interacciÃ³n siguen usando el actor original.
- `CharacterCreationController`: mantiene UI Toolkit y el estilo del prototipo. El botÃ³n de continuar permanece visible; permisos y colores histÃ³ricos se despliegan aparte. Una cancelaciÃ³n o error conserva la apariencia vÃ¡lida previa.
- `CompanionPrototype`: abre el creador de forma aditiva en pausa y vuelve al mismo encuentro. Oculta el documento del HUD sin destruir su Ã¡rbol visual.
- `GraphicsSettings`: conserva MToon URP y UniUnlit en la build para que la carga dinÃ¡mica no dependa de shaders ausentes. Unity aÃ±ade sus campos de serializaciÃ³n actuales al guardar estos ajustes.

### Persistencia y recuperaciÃ³n

El perfil v2 separa identidad y personalidad de `appearance`. El archivo contiene versiÃ³n, checksum y respaldo; se escribe mediante temporal, vaciado y reemplazo del archivo. El perfil v1 de PlayerPrefs permanece intacto y su JSON original se conserva, incluidos campos desconocidos. El fixture estructurado de migraciÃ³n incluye memoria y clase; esto prueba conservaciÃ³n de datos, no implementaciÃ³n de esos sistemas.

Los VRM confirmados se copian a `Application.persistentDataPath/Companion/Avatars/<sha256>.vrm`. La selecciÃ³n conserva hash, versiÃ³n VRM, metadatos y declaraciÃ³n de uso local; no guarda la ruta original ni depende de ella. Antes de cargar una copia se comprueba su hash. Si falta o estÃ¡ daÃ±ada, se muestra Adventurer con aviso y se conserva la referencia guardada para reimportaciÃ³n o confirmaciÃ³n explÃ­cita de recuperaciÃ³n.

No hay borrado automÃ¡tico de la biblioteca ni limpieza de copias sin referencia: evita eliminar recursos de otros perfiles o respaldos, pero puede dejar archivos huÃ©rfanos tras fallos de guardado. El encuentro conserva su memoria durante la ediciÃ³n; **el guardado completo de la aventura entre ejecuciones no forma parte de esta entrega** y no existÃ­a en el prototipo.

### Recursos y cancelaciÃ³n

Se identificÃ³ una fuga nativa de buffers de colas en el combinador compartido de Spring Bones al retirar avatares. Se utiliza `Vrm10FastSpringboneRuntimeStandalone`, disponible en el SDK fijado, sin parchear el paquete. La repeticiÃ³n posterior con detecciÃ³n de fugas activada no produjo ese aviso.

La carga mantiene oculto el candidato mientras se prepara y se evalÃºa el rig; su contenedor visual permite ajustar altura y apoyo sin alterar el actor. Se valida y activa antes de retirar la apariencia anterior. El token llega al SDK y se comprueba antes de publicar el candidato. Esto no permite interrumpir arbitrariamente una decodificaciÃ³n nativa sÃ­ncrona.

El ensayo fuera del Editor detectÃ³ un desbordamiento de pila al transferir un StringBuilder grande al selector Win32. Se sustituyÃ³ por un buffer en memoria no administrada, liberado en `finally`. Las pruebas de seleccionar y cancelar utilizan diÃ¡logos reales del ejecutable; el helper solo actÃºa sobre el proceso de prueba indicado.

## 3. Licencias y privacidad

**CatÃ¡logo incluido:** Adventurer Outfit 02, Dawn to Dusk Games, versiÃ³n local 1.2. La [pÃ¡gina del autor](https://dawn-to-dusk-games.itch.io/3d-anime-female-adventurer) publica CC0-1.0. Se reutilizan malla, texturas y clips Unarmed Idle/Walk/Run ya presentes; no se reconstruye el personaje. La procedencia y hashes originales estÃ¡n en `SourceReceipt.json`. El arte del catÃ¡logo es provisional y no constituye aprobaciÃ³n artÃ­stica de F4.

**SDK:** UniVRM/UniGLTF 0.131.4, mismas dependencias fijadas en PR #4. Se conservan los avisos MIT de [UniVRM 0.131.4](https://github.com/vrm-c/UniVRM/blob/v0.131.4/LICENSE.txt) y UniGLTF, tambiÃ©n en el paquete Windows. No se contrata ni invoca un servicio generativo o de pago.

**Fixture privado:** el hash utilizado estÃ¡ en los resultados, sin publicar el archivo ni sus imÃ¡genes. Declara `onlyAuthor`, `personalNonProfit`, crÃ©ditos requeridos, modificaciÃ³n prohibida y redistribuciÃ³n no permitida. La UI muestra estos datos por separado de la declaraciÃ³n humana de autorÃ­a o permiso adicional. Compatibilidad tÃ©cnica y marcar una casilla no verifican derechos sobre los recursos incorporados ni autorizan redistribuciÃ³n.

Las capturas del VRM permanecen en `UnityProjectIsekaid/LocalVrmEvidence/F2/`, excluidas de Git. Las imÃ¡genes pÃºblicas de este informe muestran Ãºnicamente el catÃ¡logo CC0. No se incorporan modelos privados a la build.

## 4. Pruebas y evidencia

Antes de modificar se respaldaron las dos escenas y se tomaron hashes de 797 archivos. La comprobaciÃ³n estÃ¡tica inicial del prototipo pasÃ³. El primer Play Mode se interrumpiÃ³ por `UnityEditor.Search.SearchDatabase`; se registrÃ³ su stack y se excluye Ãºnicamente ese fallo del Ã­ndice del Editor en la regresiÃ³n. No se excluyen errores del gameplay.

Los resultados finales y su alcance se registran en los JSON adjuntos. Una prueba automatizada que manipula APIs del creador no acredita por sÃ­ sola cada clic de UI; el selector nativo se comprueba ademÃ¡s mediante sus diÃ¡logos reales.

| ComprobaciÃ³n | Evidencia |
|---|---|
| CatÃ¡logo y VRM: creador â†’ Game; tres personalidades independientes | `editor-results.json`, `windows-results.json` |
| Misma compaÃ±era, confianza y primer rescate durante cambios visuales | Aserciones sobre la misma instancia de estado y controlador |
| CancelaciÃ³n antes y durante carga; solicitudes consecutivas | Aserciones de conservaciÃ³n del avatar anterior |
| Fallos en cinco etapas: bytes, importaciÃ³n, preparaciÃ³n, activaciÃ³n y ocultaciÃ³n anterior | InyecciÃ³n en Editor; rollback verificado |
| Guardado fallido, migraciÃ³n v1 y recuperaciÃ³n de respaldo | `storage-results.json` |
| Copia gestionada daÃ±ada/perdida y archivo fuente borrado | Recorrido integrado; referencia y estado conservados |
| Cargas y descargas repetidas | Tres ciclos; 195 recursos propios destruidos comprobados por ejecuciÃ³n |
| Selector nativo: seleccionar y cancelar | Ejecutable Windows y helper limitado al proceso de pruebas |
| Reinicio real sin archivo fuente | Segundo proceso; `restart-results.json` |
| Rechazos y regresiÃ³n del prototipo | `guard-results.json` (nueve fixtures y dos archivos locales), `regression-results.json` |
| Rutas, exclusiones y originales protegidos | `tools/verify_unified_layout.py`, `tools/verify_vrm_f2_delivery.py` |
| UI 1920Ã—1080, 1280Ã—720 y 1600Ã—900 | Capturas reales del creador; revisiÃ³n visual local |

![Creador con catÃ¡logo CC0, 1280Ã—720](evidence/creator-catalog-1280.png)

![Game con catÃ¡logo CC0](evidence/game-catalog.png)

### Expresiones y animaciones

El fixture conserva los canales comprobados en la auditorÃ­a previa: `happy`, `angry`, `sad`, `relaxed`, `surprised`, `aa`, `ih`, `ou`, `ee`, `oh`, `blink`, `blinkLeft`, `blinkRight`, `lookUp`, `lookDown`, `lookLeft`, `lookRight`, `neutral`. F2 vuelve a solicitar los canales disponibles y comprueba una peticiÃ³n inexistente sin error. Esto no acredita sincronizaciÃ³n de labios ni conducta narrativa profunda.

Adventurer tiene 16 Shape Keys inspeccionadas. Las muestras se mapean por nombre a sonrisa, tristeza, sorpresa, parpadeo y apertura de boca; angry/relaxed vuelven a neutral por falta de un canal adecuado. Se reutilizan sus clips Generic. El VRM usa las animaciones Humanoid diagnÃ³sticas de PR #4; no se presentan como locomociÃ³n artÃ­stica final.

### Rendimiento

Build Windows: 196,441,782 bytes segÃºn BuildPipeline, Development Mono, D3D11; equipo de prueba con NVIDIA GeForce RTX 5070 Ti. Editor: 638.2 ms de importador y 1.78 ms de intervalo medio. Windows: 518.4 ms y 1.61 ms; memoria asignada de 209.89 a 204.34 MiB tras los ciclos. El tiempo del importador no incluye todas las lecturas previas del creador. Las ventanas de ensayo no representan una sesiÃ³n de usuario ni permiten deducir un objetivo de FPS. Se mide tiempo de importaciÃ³n, intervalo de 60 frames y memoria asignada por Unity antes/despuÃ©s de los ciclos. No es un perfil GPU completo ni una garantÃ­a para hardware mÃ­nimo. La primera captura del ejecutable durante arranque puede preceder al dibujo completo; no se usa como aprobaciÃ³n visual del catÃ¡logo.

## 5. Criterios F2 de la especificaciÃ³n v0.2

| Criterio | Estado | Alcance comprobado / pendiente |
|---|---|---|
| VRM-AC01 | PARTIAL | CatÃ¡logo Ãºnico incorporado y ejecutable local sin cuenta ni descarga. No se desconectÃ³ fÃ­sicamente la red durante este ensayo. |
| VRM-AC04 | PASS | CancelaciÃ³n, peticiones consecutivas y diÃ¡logo nativo; el candidato cancelado no sustituye al anterior. |
| VRM-AC12 | PARTIAL | Identidad, personalidad, datos legacy y estado real existente conservados. Clase, misiones y rasgos evolucionados aÃºn no existen funcionalmente; no se acredita F3. |
| VRM-AC13 | PASS | Error de guardado inyectado y fallo real de reemplazo de archivo bloqueado; original y respaldo recuperables. No se ensayÃ³ corte elÃ©ctrico real. |
| VRM-AC14 | PASS | Copia gestionada comprobada tras borrar el archivo fuente y reiniciar el ejecutable. |
| VRM-AC15 | PASS | Copia daÃ±ada/perdida: fallback anunciado, referencia y memoria del encuentro conservadas; reimportaciÃ³n recupera el recurso. |
| VRM-AC16 | PASS | MigraciÃ³n v1 con colores, personalidad y JSON desconocido preservados; respaldo y error de guardado comprobados. |
| VRM-AC17 | PASS | Restricciones reales y fixture con crÃ©ditos mostrados por separado de la declaraciÃ³n humana. RevisiÃ³n de derechos pendiente; no equivale a permiso legal validado. |
| VRM-AC22 | PARTIAL | Copias por hash y respaldos preservados. No se implementa eliminaciÃ³n de biblioteca ni se acredita compatibilidad con formatos futuros de partida. |
| VRM-AC24 | PARTIAL | EdiciÃ³n bloqueada en incapacitaciÃ³n/muerte y encuentro pausado conservado. Combate, secuestro y secuencias importantes requieren sistemas y pruebas posteriores. |
| VRM-AC26 | PASS | Muestras sin controlador de compaÃ±era propio; no modifican confianza, rescates ni estado del encuentro conservado. |

Estos estados quedan sujetos a los resultados finales adjuntos y revisiÃ³n de DirecciÃ³n. No se declara aprobaciÃ³n artÃ­stica ni cierre de F3/F4.

Los archivos `native-picker-results.json` conservan el recorrido con diálogos de la build previa; el selector no cambió en la build final. `windows-results.json` y `restart-results.json` corresponden al recorrido y reinicio del código final. El cambio posterior de empaquetado solo añade avisos de licencia y se comprobó con un nuevo reinicio.

## 6. Limitaciones

- Plataforma verificada: Windows; selector en Windows Editor/Player. No se promete soporte mÃ³vil, macOS, Linux o WebGL.
- LÃ­mites heredados: 32 MiB, VRM 1.0, GLB con recursos incorporados, PNG hasta 4096 por lado y presupuesto total de pÃ­xeles; lÃ­mites adicionales de nodos, geometrÃ­a y Spring Bones. JPEG, VRM 0.x y recursos externos se rechazan en este prototipo.
- La importaciÃ³n ocurre en el proceso de Unity. Los lÃ­mites y nueve rechazos no constituyen aislamiento de seguridad ni defensa completa frente a archivos hostiles. No se ejecuta cÃ³digo del avatar ni se aceptan URI externas.
- No se realizÃ³ captura de trÃ¡fico que acredite cero solicitudes de red del ejecutable. La build de desarrollo incluye la infraestructura de diagnÃ³stico de Unity. VRM-AC18 no se acepta por inferencia del cÃ³digo.
- No hay presupuesto de rendimiento/hardware ni intervalos artÃ­sticos aprobados para aceptar completamente VRM-AC19/20. El ajuste visual mantiene el actor y su hitbox, pero no acredita todos los rigs, materiales o accesorios posibles.
- La iluminaciÃ³n MToon del fixture muestra zonas claras muy intensas. Requiere revisiÃ³n artÃ­stica; no se han alterado sus texturas ni se certifica fidelidad a VRoid.
- No se acredita guardado global de aventura, personalidad evolutiva, combate, clases, misiones, secuestro, lip sync ni catÃ¡logo final de tres personajes.
- El Ã­ndice de bÃºsqueda del Editor falla en este entorno; el primer importado tambiÃ©n registrÃ³ un fallo de TMP de UGameStudio ya presente antes de la integraciÃ³n. No se corrigen dependencias ajenas como parte de F2.

## 7. ReproducciÃ³n y revisiÃ³n

Consultar `PRUEBAS.md` para comandos y pasos manuales. Los datos y capturas de pruebas deben ir a directorios privados fuera de Git; el fixture se proporciona localmente, nunca como recurso del proyecto. No ejecutar builders histÃ³ricos sobre las escenas aceptadas: el ajuste de F2 es `AvatarIntegrationBuilder.Run` y requiere conservar versiones recuperables.

DirecciÃ³n debe revisar la calidad visual, la experiencia del creador, las restricciones documentadas y los criterios parciales antes de decidir una integraciÃ³n estable. Esta rama no se fusiona automÃ¡ticamente.
