# ISEKAID — Workflow de desarrollo v1.2 (VRM y coordinación documental)

**Vigente desde:** 28/10/2026 · **Autoridad:** [DEC-006 y DEC-007](DECISIONS.md) · **Seguimiento:** [F2 / #5](https://github.com/MarcoPolo2-0/Isekaid/issues/5), [ISEK-001](https://github.com/MarcoPolo2-0/Isekaid/issues/1)

## Repositorio y proyecto

| Elemento | Ubicación / uso |
|---|---|
| GitHub único | [MarcoPolo2-0/Isekaid](https://github.com/MarcoPolo2-0/Isekaid) |
| Carpeta que abre Unity Hub | `UnityProjectIsekaid/` |
| Versionado Unity | `UnityProjectIsekaid/Assets/`, `Packages/`, `ProjectSettings/`, `SourceArt/` |
| Documentos compartidos | `Documentation/` |
| Modelo original conservado | `assets/woman.fbx` |
| Escenas del prototipo | `UnityProjectIsekaid/Assets/_Project/Scenes/` |
| Escenas de build | `UnityProjectIsekaid/ProjectSettings/EditorBuildSettings.asset` |
| Motor | Unity 6000.6.7f1 |

No clonar ni actualizar submódulos; `Isekaidunity` y el antiguo repositorio Unity son **historial**, no lugares vigentes para trabajar. No abrir la raíz de Git como proyecto Unity: se debe seleccionar `UnityProjectIsekaid/`.

## Responsabilidades de los tres chats

- **Chat 1 — Dirección:** prioriza, evalúa resultados visuales/técnicos, aprueba cambios de alcance, registra `DEC-###` y actualiza Issues.
- **Chat 2 — Game Design:** trabaja en `Documentation/`, referencias y criterios; publica propuestas pero no modifica código ni declara aprobada una propuesta.
- **Chat 3 — Desarrollo Unity:** trabaja en `UnityProjectIsekaid/`, conserva escenas y `.meta`, crea ramas y PR; informa importación, compilación, pruebas, muestras visuales y fallos.
- Antes de trabajar: consultar `Documentation/DECISIONS.md`, este workflow, Issue activo y especificación. Evitar editar el mismo archivo simultáneamente.

## Ramas y entregas

1. `main`: línea estable y revisada; no usarla para experimentos de arte o código.
2. Rama corta por hito: `feature/isek-001-descripcion` o `docs/isek-001-descripcion`, creada desde `main`. Para una corrección posterior, trabajar sobre el PR relacionado si procede.
3. Commits pequeños con identificador `ISEK-###`; abrir PR hacia `main`, enlazar Issue, describir rutas, dependencias, riesgos, qué pruebas se ejecutaron realmente y qué falta.
4. **No fusionar sin revisión de Dirección y del usuario cuando haya aprobación artística.** Los artefactos de laboratorio no se consideran funcionalidad terminada.
5. Si hay cambios locales: revisar `git status` antes de actualizar; no usar `git clean`, `reset --hard` o `push --force` para resolver diferencias sin respaldo.
6. La raíz tiene `.gitignore` para omitir Library, Temp, Logs, archivos de IDE y builds. Conservar archivos `.meta` relevantes. Los recursos binarios grandes se gestionan por Git LFS. No registrar secretos, contraseñas, tokens ni cachés.

## Estado actual y siguiente puerta

- GDD maestro aprobado documentalmente: [v0.7](GameDesignDocument_v0.7.md), según DEC-008. v0.5, v0.6 y borrador v0.7 se conservan íntegros como historial; los contenidos PROPUESTOS/PENDIENTES no cambian de estado por la aprobación del maestro.
- Prototipo existente y rescatado: `CharacterCreation.unity`, `Game.unity`, `CompanionPrototype.unity` y `Assets/_Project/`. Mantenerlos funcionales mientras evoluciona la compañera.
- ISEK-001 permanece abierta; Woman/G2 quedan históricos y pausados por DEC-007. VRM 1.0 + catálogo local es la dirección principal. La pausa no acepta ni elimina los resultados anteriores.
- Siguiente puerta: mantener la aceptación de F2/F3A por criterio y revisar creativamente F3A-N1 en el PR #14. La aprobación documental de v0.7 no autoriza aplicar los diálogos N1, incorporar Deredere o implementar sistemas futuros. Dirección decide IDs y alcances antes de la integración. Woman/G2 permanecen pausados hasta nueva autorización.

## Verificación sin ejecutar Unity

En la raíz de Git:

```bash
python tools/verify_unified_layout.py
```

GitHub Actions ejecuta la misma comprobación en push y PR. Solo valida archivos y configuración estática. Para declarar compilación o Play Mode **deben ejecutarse pruebas en Unity** y adjuntar resultados.

## Compatibilidad histórica

`Workflow_Desarrollo_v1.0.md` y las decisiones previas se conservan como documentación de la fase con dos repositorios. Los enlaces a `Isekaidunity` en documentos antiguos no definen la ubicación actual del proyecto. Esta v1.2 prevalece para coordinación y siguiente puerta; conserva las rutas unificadas de v1.1 y las decisiones históricas.

## Coordinación F2 y cambios de alcance

- Chat 2 formaliza decisiones recibidas; no aprueba unilateralmente requisitos nuevos. Señalar propuestas F2-P y borrador F3 en cada PR. Chat 3 usa requisitos fusionados y autorizaciones directas; una aclaración importante no fusionada no se aplica automáticamente.
- Reportar estados por caso: no ejecutado, parcial, fallo, evidencia entregada y aceptado por Dirección. POC, revisión estática, Play Mode y build son evidencias diferentes; no cerrar una fase por tener archivos.
- Mantener .vrm y capturas restringidas fuera de publicaciones hasta revisión humana de derechos. Reutilizar recursos aprobados, sin seleccionar un catálogo por su mera presencia local.
- Antes de integrar readme.txt, leer la versión remota y conservar tanto la entrada de Desarrollo como la de Diseño. Si hay conflicto, unir entradas completas con fechas y contexto; no sustituir el archivo por una copia anterior.
- Mantener #1 abierto y #5 como seguimiento F2. No marcar #2 completo por la fusión del POC; sus criterios tienen evaluación independiente.
- Estado y contrato: [F2](Specifications/VRM_Fase2_Contrato_Funcional_v0.1.md), [F3 borrador](Specifications/VRM_Fase3_Personalidad_Contextual_v0.1_BORRADOR.md), [matriz](Specifications/VRM_F2_F3_Matriz_Trazabilidad_v0.1.md).

**Registro v1.2 — 28/10/2026:** DEC-007, coordinación sin bloquear F2, propuestas explícitas, pausa Woman/G2 y preservación de entradas concurrentes. v1.0/v1.1 intactos.

**Enmienda administrativa — 29/10/2026:** actualizada la referencia al GDD maestro v0.7 y su aprobación documental DEC-008; separadas revisión creativa N1 e integración posterior. No cambian los contratos, criterios o límites de autorización de implementación.
