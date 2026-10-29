# F3A — Reproducción y revisión manual

**Revisión vigente:** [biblioteca canónica, 18 recorridos y guía manual actualizada](review-1/PRUEBAS.md). Los comandos siguen siendo válidos; usar resultados y hashes de la revisión actual.

Usar Unity 6000.6.7f1 con soporte Windows, la rama F3A y un VRM 1.0 local autorizado. Cada ejecución completa utiliza un directorio de datos nuevo fuera del repositorio. Cerrar otras instancias de Unity que usen este mismo proyecto. No publicar modelos ni imágenes privadas.

## Unity Play Mode

Variables de ejemplo que deben adaptarse a la instalación y a carpetas privadas:

```powershell
$unity = 'C:\Program Files\Unity\Hub\Editor\6000.6.7f1\Editor\Unity.exe'
$project = (Resolve-Path '.\UnityProjectIsekaid').Path
$fixture = 'D:\PrivateFixtures\companion.vrm'
& $unity -batchmode -projectPath $project `
  -executeMethod Isekaid.Editor.F3AIntegrationChecks.Run `
  -isekTestData 'D:\PrivateTests\f3a-editor-fresh' `
  -isekVrmFixture $fixture `
  -isekCaptureDir 'D:\PrivateTests\f3a-editor-captures' `
  -logFile 'D:\PrivateTests\f3a-editor.log'
```

No añadir `-quit` al recorrido Play Mode: el runner sale al terminar. Debe producir `f3a-results.json`, `passed: true`, 123 comprobaciones y el marcador `ISEKAID_F3A_PLAYMODE_CHECKS_PASSED`. Los pesos semánticos se comprueban en la instancia VRM real; el archivo no se incluye en el proyecto. Una ventana batch/sin foco no acredita captura física del cursor.

## Build Windows

Se reutiliza el constructor existente F2, sin duplicar escenas ni configuración:

```powershell
& $unity -batchmode -quit -projectPath $project `
  -executeMethod Isekaid.Editor.F2IntegrationChecks.BuildWindows `
  -logFile 'D:\PrivateTests\f3a-build.log'
```

La salida local queda en `UnityProjectIsekaid/Builds/F2Windows/` por compatibilidad con el constructor. El nombre de carpeta no significa que sea una build antigua: comprobar los hashes de `build-results.json`. Conservar toda la carpeta, incluido `Isekaid_Data` y `ThirdPartyNotices`.

```powershell
& '.\UnityProjectIsekaid\Builds\F2Windows\Isekaid.exe' `
  -screen-fullscreen 0 -isekF3ATest full `
  -isekTestData 'D:\PrivateTests\f3a-windows-fresh' `
  -isekVrmFixture $fixture `
  -isekCaptureDir 'D:\PrivateTests\f3a-windows-captures' `
  -logFile 'D:\PrivateTests\f3a-windows.log'
```

Sin argumentos de prueba, el ejecutable abre el creador normal y no ejecuta el ensayo.

## Regresión F2

Reutilizar los comandos y herramientas de [F2/PRUEBAS.md](../VRM-F2/PRUEBAS.md). En esta entrega se ejecutaron:

- `Isekaid.Editor.F2IntegrationChecks.Run`: recorrido Editor con el fixture privado grande generado por `tools/create_vrm_metadata_fixture.py`.
- `Isekaid.ExperimentalVrm.Editor.VrmGuardChecks.ValidateF2AndRegression`: guardas, almacenamiento y prototipo original Play Mode; sin `-quit`.
- `Isekaid.Editor.F2IntegrationChecks.MetadataStorage`: con `-batchmode -quit`, datos nuevos y **también** `-isekVrmFixture $fixture`.
- Ejecutable nuevo con `-isekF2Test full`; copiar `results.json` antes del siguiente proceso.
- Segundo proceso con `-isekF2Test restart`, mismo `-isekTestData`, sin argumento de fixture ni archivo fuente. El ensayo completo anterior elimina su copia fuente.

Verificación estática, separada de Play Mode:

```powershell
python tools/verify_unified_layout.py
python tools/verify_vrm_f2_delivery.py
```

## Revisión manual pendiente del usuario / Dirección

La revisión visual de las capturas publicadas se realizó. El siguiente recorrido físico sigue **PENDIENTE**, no se marca como aprobado por la automatización:

1. Abrir la build sin argumentos de prueba, elegir Adventurer y una personalidad; entrar en Game.
2. Mantener E sobre la compañera atrapada; comprobar rescate y frase única. Volver a mantener E para conversar.
3. Elegir con ratón una pregunta; comprobar texto y rostro. Usar Continuar/Intro. Moverse y mirar inmediatamente con WASD/ratón; confirmar captura del cursor en ventana enfocada.
4. Repetir con las tres personalidades desde el creador y un encuentro nuevo; comparar la misma pregunta, no situaciones distintas.
5. Conversar otra vez, saltar con Esc y comprobar contexto distinto. Usar F tanto fuera como dentro de diálogo; verificar seguir/esperar.
6. Pulsar Tab durante conversación, esperar y reanudar; comprobar misma frase sin reiteración ni consumo de tiempo. Cerrar mientras se conserva pausa mediante el flujo disponible sin reanudar indebidamente.
7. Simular incapacitación desde el menú, observar petición de ayuda; reanimar y conversar. Confirmar alivio, confianza inicial sin duplicación y prioridad vital.
8. Abrir el creador desde un encuentro seguro; importar el VRM local después de revisar permisos. Volver a Game, mantener recuerdo/confianza/historial; repetir hacia catálogo.
9. Reiniciar encuentro y dejar vencer otra incapacitación; comprobar Game Over, ausencia de charla casual y reinicio funcional.
10. Revisar todo a 1920×1080, 1600×900 y 1280×720. Registrar cualquier texto ilegible, solape, foco erróneo o expresión poco coherente.

Registrar resultado, resolución, apariencia, personalidad, pasos y captura. Las capturas de modelos restringidos permanecen privadas. La aprobación de F3A y de la estabilidad manual de F2 corresponde a Dirección; no fusionar automáticamente.
