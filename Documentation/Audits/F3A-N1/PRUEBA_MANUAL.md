# N1 v0.5 — Repetición y revisión de Dirección

Estado: integración experimental; la aceptación narrativa dentro del juego corresponde a Dirección. N2 no está implementado.

## Preparación

Usar Unity **6000.6.7f1**, la rama del PR de implementación y `UnityProjectIsekaid/`. Abrir `Assets/_Project/Scenes/CharacterCreation.unity`. Conservar los datos habituales y probar con un directorio aislado. El avatar VRM autorizado se selecciona desde su almacenamiento privado; no publicarlo ni compartir sus capturas.

El manifiesto [manifest.json](manifest.json) enumera los 30 IDs exactos, los campos modificados y los hashes. [Parche_Autorizado_v0.5.json](Parche_Autorizado_v0.5.json) es una copia de evidencia del blob `77228337ac434b4a56a47e25f05219e6c1ceffe7` del commit aprobado `5bdfeed43994e509f48dde148beca50dc961278f`. Su estado editorial histórico no sustituye la autorización directa posterior de Dirección. No se aplicaron versiones anteriores ni se fusionó el PR #14.

## Recorrido manual

1. En el creador comprobar **Tsundere, Yandere y Dandere**, exactamente tres opciones. Elegir Adventurer y Dandere, guardar y comenzar. Cerrar el proceso y volver a abrir: debe conservar personalidad y apariencia. Repetir con un perfil anterior cuya personalidad numérica sea 2; no hace falta migrar el formato.
2. Reiniciar el encuentro para cada personalidad. Rescatar con E y revisar `A.RESCUE`. Abrir conversación; recorrer los tres temas y sus dos opciones en encuentros independientes: **3 personalidades × 3 temas × 2 alternativas = 18 rutas**. Comparar el texto mostrado con el parche autorizado, especialmente FEEL.OPEN/BREATH/LATER y ROAD.UNCERTAIN. No deben cambiar las opciones del jugador.
3. Dejar una raíz con dos opciones pendiente. Cerrar con Esc, caminar, volver a hablar y elegir una opción. Repetir entrando al creador desde pausa y volviendo a la aventura con catálogo y con VRM: misma raíz/opciones/foco, sin nuevo rescate ni confianza adicional. Cambiar personalidad en el creador y comprobar que se usa la biblioteca de la selección actual, conservando identidad y apariencia.
4. Agotar temas y solicitar charla casual hasta `C.LATER_1`, respetando el enfriamiento existente. Alternar seguimiento/espera para revisar `D.WAIT` y `D.WAIT_OPINION`. No esperar nuevos gestos, silencios detectados ni conocimientos derivados de la biografía.
5. Incapacitar con el control existente. Revisar el `E.DOWNED` original, el límite inicial de **30 segundos**, la pausa global y la reanimación. Repetir con conversación pendiente para revisar `F.RESUME`, y sin cursor pendiente para `E.REVIVED`. Reanimar no debe repetir la recompensa del primer rescate. Dejar expirar el temporizador y comprobar muerte/reinicio del encuentro; no es un checkpoint de campaña.
6. Seguir caminando/corriendo, frenar, reanudar y girar varias veces con Adventurer y con el VRM. Revisar continuidad de animación y ausencia de nuevas alteraciones de postura/materiales. N1 no cambia locomoción ni presentación del avatar.
7. Repetir creador y conversación en **1920×1080, 1600×900 y 1280×720**. Comprobar textos completos, opciones accesibles, scroll, foco con teclado, ratón físico y recuperación del cursor al cerrar. Revisar también la build Windows visible.

Registrar por personalidad los IDs leídos, coherencia con el contexto, distinción de voz y resultado **ACEPTADO / REQUIERE CORRECCIONES**. La presencia del texto exacto y las pruebas automáticas no equivalen a aceptación creativa ni corporal. Nera/Liora/Sira, sus biografías y proyectos continúan como propuestas.

## Repetición automatizada

Desde la raíz, con Python 3.10 o posterior y Git disponible:

```powershell
python tools/verify_f3a_n1.py
python tools/build_f3a_runtime_content.py --check
python tools/verify_unified_layout.py
python tools/verify_vrm_f2_delivery.py
```

El verificador necesita el commit base `0ef11e5` en el historial; no aplica cambios. Rechaza hash/base/expected incorrectos, IDs ajenos o duplicados, campos adicionales e incorporación de los DESCARTAR. Compara toda la estructura canónica con el resultado autorizado y valida el recurso generado. La convención es UTF-8 sin BOM y LF, sin normalizar otra parte del contenido; Git fija LF para los dos recursos y el generador escribe LF también en Windows.

Definir `$unity` como la ruta a Unity.exe, `$repo` como la raíz del checkout, `$data` como una carpeta **nueva** fuera de Assets y `$vrm` como el archivo privado autorizado. Ejecutar Unity de forma secuencial, sin otro Editor abierto sobre el mismo proyecto:

```powershell
function Test-Unity($name, $method, $extra = '') {
    $arguments = '-projectPath "' + $repo + '\UnityProjectIsekaid" -executeMethod ' + $method +
        ' -isekTestData "' + $data + '\' + $name + '" -isekCaptureDir "' + $data + '\' + $name + '-captures"' +
        ' -isekVrmFixture "' + $vrm + '" -logFile "' + $data + '\' + $name + '.log" ' + $extra
    $run = Start-Process -FilePath $unity -ArgumentList $arguments -WindowStyle Hidden -PassThru
    $run.WaitForExit()
    if ($run.ExitCode -ne 0) { throw "Prueba fallida: $name" }
}
Test-Unity 'f3a' 'Isekaid.Editor.F3AIntegrationChecks.Run'
Test-Unity 'f2' 'Isekaid.Editor.F2IntegrationChecks.Run'
Test-Unity 'presentation' 'Isekaid.Editor.PresentationAudit.Run' '-isekPresentationTest candidate'
Test-Unity 'guards' 'Isekaid.ExperimentalVrm.Editor.VrmGuardChecks.ValidateF2AndRegression'
Test-Unity 'metadata' 'Isekaid.Editor.F2IntegrationChecks.MetadataStorage' '-batchmode -quit'
Test-Unity 'build' 'Isekaid.Editor.F2IntegrationChecks.BuildWindows' '-batchmode -quit'
```

La espera explícita evita lanzar dos Editor sobre el mismo proyecto o dar por terminada una prueba porque volvió la consola. Crear `$data` antes de ejecutar. Los métodos de Play Mode entran/salen por sí mismos; no añadir `-quit` a F3A, F2, PresentationAudit o ValidateF2AndRegression. Este último encadena Play Mode y no debe cerrarse prematuramente.

La build queda en `UnityProjectIsekaid/Builds/F2Windows/Isekaid.exe`. Ejecutarla **visible** con `-screen-fullscreen 0 -screen-width 1920 -screen-height 1080`, los argumentos de datos/VRM/capturas anteriores, y uno de `-isekF3ATest full`, `-isekF2Test full`, `-isekPresentationTest candidate`. El reinicio usa `-isekF2Test restart`, el mismo directorio de F2 full y ningún `-isekVrmFixture`. Conservar `results.json` de full antes de restart. Para reproducir exactamente las pruebas de metadatos grandes, usar el fixture local descrito en el informe F2, sin publicarlo.

Las capturas automáticas ejercen UI real con entrada simulada; la revisión creativa, ratón físico y selector nativo requieren la prueba humana anterior. No ejecutar métodos de reconstrucción de escenas, assets o iluminación para esta entrega.
