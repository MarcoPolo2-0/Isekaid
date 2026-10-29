# F2 — Pruebas reproducibles

## Preparación

Usar Unity 6000.6.7f1 y una copia local autorizada del fixture VRM 1.0. El fixture no forma parte del repositorio ni de la build. Cada recorrido `full` necesita un directorio de datos nuevo y privado; el mismo directorio se reutiliza solo para `restart`.

Ejemplo PowerShell; adaptar las rutas:

```powershell
$repo = 'C:\Repos\Isekaid'
$project = Join-Path $repo 'UnityProjectIsekaid'
$unity = 'C:\Program Files\Unity\Hub\Editor\6000.6.7f1\Editor\Unity.exe'
$fixture = 'D:\PrivateFixtures\companion.vrm'
$privateChecks = 'D:\PrivateTests\IsekaidF2'
New-Item -ItemType Directory -Force $privateChecks | Out-Null
```

## Rechazos, almacenamiento y regresión

```powershell
& $unity -batchmode -force-d3d11 -projectPath $project `
  -executeMethod Isekaid.ExperimentalVrm.Editor.VrmGuardChecks.ValidateF2AndRegression `
  -isekTestData "$privateChecks\regression-fresh" -isekVrmFixture $fixture `
  -logFile "$privateChecks\regression.log"
```

Este proceso sale por sí mismo: no añadir `-quit` a los ensayos de Play Mode. Esperar salida 0 y los marcadores `ISEKAID_F2_GUARD_CHECKS_PASSED`, `ISEKAID_F2_STORAGE_CHECKS_PASSED`, `ISEKAID_PROTOTYPE_CHECKS_PASSED` e `ISEKAID_TWO_SCENE_PLAYMODE_CHECKS_PASSED`. El índice de búsqueda del Editor puede registrar la excepción conocida; la exclusión del test se limita a su stack.

Se generan `guard-results.json` y `storage-results.json` en el directorio privado. Los rechazos incluyen nueve fixtures de bytes heredados y archivos locales de más de 32 MiB y versión VRM 9.9. El almacenamiento comprueba migración, respaldo, error inyectado y reemplazo real de un archivo bloqueado.

## Recorrido integrado en Editor

```powershell
$env:UNITY_JOBS_NATIVE_LEAK_DETECTION_MODE = '2'
& $unity -batchmode -force-d3d11 -projectPath $project `
  -executeMethod Isekaid.Editor.F2IntegrationChecks.Run `
  -isekTestData "$privateChecks\editor-fresh" -isekVrmFixture $fixture `
  -isekCaptureDir "$project\LocalVrmEvidence\F2\editor" `
  -logFile "$privateChecks\editor.log"
```

Esperar salida 0, `ISEKAID_F2_RUNTIME_CHECKS_PASSED` y `results.json` con `passed: true`. Este recorrido incluye cinco fallos de preparación/activación, fallo al activar en el encuentro conservado, cancelación en curso, guardado fallido, perfiles, expresiones, tres ciclos de liberación y recuperación de la copia gestionada.

## Build Windows y ejecutable

```powershell
& $unity -batchmode -quit -projectPath $project `
  -executeMethod Isekaid.Editor.F2IntegrationChecks.BuildWindows `
  -logFile "$privateChecks\build.log"
```

Salida: `UnityProjectIsekaid/Builds/F2Windows/Isekaid.exe`. Conservar la carpeta completa y sus avisos de terceros. Es una build de desarrollo para revisión, no una distribución final.

Para repetir los diálogos reales, ejecutar desde una consola PowerShell sin cambiar el foco durante la automatización:

```powershell
$exe = Join-Path $project 'Builds\F2Windows\Isekaid.exe'
$data = Join-Path $privateChecks 'windows-fresh'
$captures = Join-Path $project 'LocalVrmEvidence\F2\windows'
$playerArgs = "-force-d3d11 -screen-fullscreen 0 -screen-width 1920 -screen-height 1080 -isekF2Test full -isekTestData `"$data`" -isekVrmFixture `"$fixture`" -isekCaptureDir `"$captures`" -isekPickerExpected yes -logFile `"$privateChecks\windows.log`""
$player = Start-Process $exe -ArgumentList $playerArgs -WindowStyle Hidden -PassThru
& "$repo\tools\test_vrm_windows_picker.ps1" -PlayerId $player.Id -AvatarPath "$data\source-copy.vrm" -TimeoutSeconds 60
$player.WaitForExit()
Copy-Item "$data\results.json" "$data\full-results.json"
```

El helper solo busca ventanas del proceso indicado y verifica el foco antes de escribir. El harness abre el primer selector en la carpeta privada del ensayo. Se selecciona la copia de prueba y se cancela un segundo diálogo. La automatización no concede permisos de uso a modelos ajenos; el fixture debe estar previamente autorizado.

El recorrido completo borra únicamente `source-copy.vrm`, después de confirmar la copia gestionada. No elimina el fixture original.

### Reinicio real

Conservar `full-results.json` antes del reinicio porque el harness vuelve a escribir `results.json`:

```powershell
$restartArgs = "-force-d3d11 -screen-fullscreen 0 -isekF2Test restart -isekTestData `"$data`" -isekCaptureDir `"$captures\restart`" -logFile `"$privateChecks\restart.log`""
$player = Start-Process $exe -ArgumentList $restartArgs -WindowStyle Hidden -PassThru
$player.WaitForExit()
```

Debe recuperar el VRM desde `Avatars/<hash>.vrm`, sin fuente, y permitir entrar a Game. Este ensayo comprueba apariencia y personalidad persistidas; no acredita guardado global de la aventura.

## Revisión manual de Dirección

1. Abrir CharacterCreation o el ejecutable sin argumentos de ensayo. Elegir Adventurer y cada personalidad; comprobar giro, reposo, caminar, correr y expresiones disponibles. Guardar y comenzar.
2. Volver desde una aventura segura al creador. Importar un archivo local autorizado, revisar sus metadatos y declarar permiso de uso local antes de preparar la vista previa. Cancelar y repetir con otro candidato.
3. Guardar y regresar. Verificar mismo encuentro, rescate, confianza, personalidad, posición e interfaz; comprobar pausa y continuación. Intentar abrir el creador durante incapacitación: debe mantenerse el estado y su temporizador.
4. Reiniciar la aplicación y comprobar apariencia y personalidad. Revisar recuperación mediante catálogo/reimportación si falta la copia gestionada, conservando el perfil y sus respaldos.
5. Revisar las capturas a 1920×1080, 1280×720 y 1600×900. La primera captura del arranque del Player puede estar incompleta; utilizar las imágenes estabilizadas.
6. Repetir la primera ejecución con la red realmente desconectada. Este paso y el monitoreo de tráfico permanecen pendientes de comprobación independiente.

## Verificación de entrega

Desde la raíz del repositorio:

```powershell
python tools/verify_unified_layout.py
python tools/verify_vrm_f2_delivery.py
```

Estos scripts no ejecutan Unity ni sustituyen los ensayos anteriores. Comprueban estructura, archivos originales protegidos y exclusión de modelos, builds y capturas privados.
