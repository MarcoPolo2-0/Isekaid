# PR #7 — Corrección del guardado de metadatos

**Fecha:** 28/10/2026, Guatemala. **Estado:** corrección técnica entregada para revisión; no fusionar `main` ni iniciar F3/F4.

## Solicitud y causa

[Revisión de Dirección](https://github.com/MarcoPolo2-0/Isekaid/pull/7#pullrequestreview-5692435703): el GLB admite hasta 4 MiB de JSON, mientras el lector del perfil admite 1 MiB. La primera implementación persistía `VRMC_vrm.meta` completo y podía declarar un guardado exitoso que no pudiera leerse después.

## Corrección

1. Lista explícita de campos VRM 1.0 persistibles. Conserva identidad, créditos, copyright, licencias de terceros, referencias y permisos; omite propiedades desconocidas e índices de miniatura ligados al archivo. El VRM gestionado conserva sus bytes originales, incluidos todos los metadatos.
2. Presupuesto total de metadatos: **32 KiB UTF-8** después de serializar. Límites por campo y cantidad: nombre 256 bytes, versión/permisos enumerados 64, contacto 512, URL 2048, copyright/licencias de terceros 8192 cada uno; hasta 16 autores de 256 bytes y ocho referencias de 2048 bytes.
3. Los campos conocidos con tipo incorrecto, controles no admitidos o exceso de tamaño se rechazan. **No se recortan textos legales.** Se preservan saltos de línea y tabulaciones. La copia saneada no modifica el objeto de entrada.
4. Presupuesto del perfil completo sin cambios: **1 MiB**. Se calcula sobre los bytes finales del sobre, incluidos escapes y copia histórica; un exceso se rechaza antes de reemplazar archivos.
5. El archivo temporal se vacía y se prueba con el mismo lector del perfil y el payload esperado **antes** del reemplazo atómico. Un fallo deja intactos perfil y respaldo válidos. Después del reemplazo se vuelve a leer el destino y solo entonces se devuelve éxito.
6. También se corrigió el retorno booleano ante `InvalidDataException`: la API comunica un rechazo recuperable en lugar de propagar esa excepción.

## Pruebas ejecutadas

| Ensayo | Resultado | Evidencia |
|---|---|---|
| 35 aserciones de metadatos/guardado | PASS | `storage-results.json` |
| JSON crudo >1 MiB, campos desconocidos, UTF-8, autores, referencias, controles, permisos mal tipados, licencias demasiado largas | PASS | Mismo ensayo; rechazos conservan ambos archivos byte a byte |
| Tamaño total acumulado, escapes, esquema inválido, limpieza del temporal y perfil cercano al máximo | PASS | Lectura real del archivo y respaldo |
| VRM privado con JSON de 1,427,620 bytes y propiedad desconocida de 1,300,000 caracteres | PASS | `editor-results.json`: 43 comprobaciones integradas |
| Build Windows Development Mono y recorrido completo con ese VRM | PASS | `windows-results.json`, `build-results.json` |
| Segundo proceso, sin archivo fuente, copia gestionada → creador → Game | PASS | `restart-results.json` |
| Regresión del prototipo, almacenamiento previo y guardas | PASS | `regression-results.json`, `regression-storage-results.json`, `guard-results.json` |

El perfil producido por el caso grande ocupa **1,088 bytes**, con **370 bytes** de metadatos persistidos. Se conserva la referencia por hash, personalidad e identidad; no se usa el respaldo como sustituto silencioso de un guardado nuevo ilegible. La propiedad desconocida no aparece en el perfil. El archivo VRM completo y las capturas permanecen privados.

Los resultados anteriores de F2 siguen disponibles en el directorio padre y corresponden a la entrega inicial. Los nuevos resultados de esta carpeta identifican la revisión de metadatos; los hashes del binario nuevo están separados para no atribuir pruebas antiguas a la build corregida.

## Integración del PR #6

El PR #6 seguía abierto: se incorporó su encabezado `6261706b2ec3378cc4c8405225b540d7d89e44db` mediante merge `c863817`, dentro de la rama del PR #7. No se fusionó ninguno con `main`. La resolución de `readme.txt` conservó las 593 líneas existentes y las 26 líneas entrantes, además del registro propio de la integración. Los detalles F2-P01–P04 y F3 mantienen su estado documental.

## Reproducción

Usar rutas privadas nuevas y el fixture local autorizado. Desde la raíz:

```powershell
python tools/create_vrm_metadata_fixture.py D:\PrivateFixtures\companion.vrm D:\PrivateTests\large-metadata.vrm
```

Prueba adversarial de persistencia en Unity 6000.6.7f1:

```powershell
& $unity -batchmode -quit -force-d3d11 -projectPath $project `
  -executeMethod Isekaid.Editor.F2IntegrationChecks.MetadataStorage `
  -isekTestData 'D:\PrivateTests\metadata-storage-fresh' -isekVrmFixture $fixture `
  -logFile 'D:\PrivateTests\metadata-storage.log'
```

Debe registrar `ISEKAID_F2_METADATA_CHECKS_PASSED 35`. Para el recorrido integrado, usar el fixture grande en los comandos Editor/Windows de [PRUEBAS.md](../PRUEBAS.md), con directorios nuevos. Copiar `results.json` antes del reinicio, como indica ese documento. No publicar el fixture derivado ni sus imágenes.

## Límites conservados

La revisión no acepta F2 completa ni convierte los ensayos en garantía de seguridad o de todos los avatares. Permanecen cuatro criterios PARTIAL, la falta de prueba con red físicamente desconectada, los casos dependientes de gameplay futuro y la revisión humana de derechos/calidad visual. No se amplían presupuestos del importador, no se modifica Woman/G2 ni se activa F3/F4.
