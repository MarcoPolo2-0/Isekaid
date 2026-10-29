# Reproducción de la corrección

Usar Unity 6000.6.7f1 y abrir `UnityProjectIsekaid/`. Todos los datos de prueba deben ir a carpetas nuevas fuera de Assets. Mantener modelo, copias administradas, dumps de materiales y capturas VRM en ubicaciones privadas. No reutilizar una carpeta de F2 full que ya tenga un perfil VRM; restart sí utiliza la carpeta de su full anterior.

## Preparar recursos

Los recursos generados ya están incluidos. Para regenerarlos, sin cambiar sus GUIDs externos:

```powershell
& $unity -batchmode -quit -projectPath "$repo\UnityProjectIsekaid" -executeMethod Isekaid.Editor.LocomotionBuilder.Run -logFile "$data\builder.log"
& $unity -batchmode -quit -projectPath "$repo\UnityProjectIsekaid" -executeMethod Isekaid.Editor.LocomotionBuilder.BalanceLighting -logFile "$data\lighting.log"
```

`$unity` es la ruta a Unity.exe; `$repo`, la raíz del repositorio; `$data`, una carpeta de resultados aislada; `$vrm`, el archivo privado autorizado. Run copia las tres fuentes CC0 solo si faltan y regenera sus importaciones Humanoid, Avatar y controlador. No sustituye los FBX/metas originales ni modifica el VRM. BalanceLighting cambia únicamente iluminación de las dos escenas. El baseline almacenado se adquirió desde main f657d78, antes de estas correcciones.

## Play Mode

```powershell
& $unity -projectPath "$repo\UnityProjectIsekaid" -executeMethod Isekaid.Editor.PresentationAudit.Run -isekPresentationTest candidate -isekVrmFixture "$vrm" -isekTestData "$data\presentation-editor" -isekCaptureDir "$repo\UnityProjectIsekaid\LocalVrmEvidence\Presentation\manual-editor" -logFile "$data\presentation-editor.log"
```

La entrada y salida de Play Mode las controla el método: no añadir `-quit`. El escenario pasa de creador a Game, inspecciona Idle/Walk/Run, mide variación real de piernas, seguimiento continuo/frenada/reanudación/dirección, ruido alrededor de 3 m/s, zancada recta 1.4/3.8 y una comparación T-pose de iluminación. Lo repite con Adventurer y el VRM. Emite JSON, CSV y secuencias PNG reales; solo los resultados numéricos son publicables para el VRM. El modo `calibration` solo mide poses/zancada estacionaria y **no** equivale a la batería completa.

Para F3A y F2, repetir la misma forma de ejecución, con directorios nuevos y estas entradas:

| Método | Resultado esperado |
|---|---|
| `Isekaid.Editor.F3AIntegrationChecks.Run` | `f3a-results.json`: 285 comprobaciones/18 rutas |
| `Isekaid.Editor.F2IntegrationChecks.Run` | `results.json`: 43 comprobaciones con fixture de metadatos grande preexistente |
| `Isekaid.ExperimentalVrm.Editor.VrmGuardChecks.ValidateF2AndRegression` | guard-results/storage-results y marcador TWO_SCENE_PLAYMODE_CHECKS_PASSED |
| `Isekaid.Editor.F2IntegrationChecks.MetadataStorage` | metadata-results: 35 comprobaciones; añadir `-batchmode -quit` |

## Windows

```powershell
& $unity -batchmode -quit -projectPath "$repo\UnityProjectIsekaid" -executeMethod Isekaid.Editor.F2IntegrationChecks.BuildWindows -logFile "$data\build.log"
$player = "$repo\UnityProjectIsekaid\Builds\F2Windows\Isekaid.exe"
& $player -screen-fullscreen 0 -screen-width 1920 -screen-height 1080 -isekPresentationTest candidate -isekVrmFixture "$vrm" -isekTestData "$data\presentation-windows" -isekCaptureDir "$repo\UnityProjectIsekaid\LocalVrmEvidence\Presentation\manual-windows" -logFile "$data\presentation-windows.log"
```

Para las regresiones, cambiar `-isekPresentationTest candidate` por `-isekF3ATest full`, `-isekF2Test full` o `-isekF2Test restart`, con carpetas separadas. Full F2 usa el fixture de metadatos grandes previo. Restart reutiliza sus datos, **sin** `-isekVrmFixture`; full ya elimina su copia fuente. Verificar las dos escenas, continuar y frenar varias veces, girar, abrir/cerrar diálogo, cambiar personalidad, volver al creador, cambiar catálogo/VRM, incapacitar/reanimar y reiniciar el proceso. Revisar también los tamaños de UI que ejercen F2/F3A.

Los ensayos de presentación y F3A se acotan a 60 FPS para evitar el caso previo de pasos de entrada submilimétricos a frecuencias extremas fuera de foco. No se cambia el jugador original para ocultar ese límite. No se declara probada la ventana nativa del selector de archivos en esta revisión.

## Referencia independiente y privacidad

Instalar Node y Playwright en un entorno de herramientas externo al juego, con Microsoft Edge disponible. Ninguno se añade como dependencia de Isekaid. Ejecutar:

```powershell
node tools/vrm-reference/capture.cjs "$vrm"
```

El script obtiene las dependencias MIT fijadas en `sources.json`, verifica sus SHA-256 y guarda sus licencias en `.utmp/vrm-reference-verified`. Con la caché completa funciona sin descargar nada. El avatar se sirve al navegador desde memoria local, sin servidor público ni subidas; se rechazan otros orígenes de la página. La captura e inventario quedan únicamente en `UnityProjectIsekaid/LocalVrmEvidence/Presentation/reference`. Se comparan con `vrm-tpose-*` de Unity; no se utiliza la T-pose para aprobar el retargeting animado.

## Comprobaciones estáticas

```powershell
python tools/verify_unified_layout.py
python tools/verify_vrm_f2_delivery.py
python tools/build_f3a_runtime_content.py --check
```

Estas comprobaciones no sustituyen Play Mode, Windows ni la aceptación visual. Los resultados finales de esta entrega están en los JSON de este directorio y sus hashes en `artifact-manifest.json`.

Para evidencia de pantalla en Windows, ejecutar la build con ventana visible. Ocultarla produjo capturas negras en el primer ensayo, aunque los controles lógicos y los renders directos de cámara terminaban correctamente. Comprobar contenido de las imágenes antes de usarlas como evidencia. Los resultados finales de Windows provienen de una repetición visible de la misma build.
