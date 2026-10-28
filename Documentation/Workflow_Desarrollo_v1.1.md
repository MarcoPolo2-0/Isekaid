# ISEKAID — Workflow de desarrollo v1.1 (repositorio único)

**Vigente desde:** 28/10/2026 · **Autoridad:** [DEC-006](DECISIONS.md) · **Seguimiento:** [ISEK-001](https://github.com/MarcoPolo2-0/Isekaid/issues/1)

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

- GDD aprobado: `Documentation/GameDesignDocument_v0.5.md`. El documento v0.6 continúa **BORRADOR**.
- Prototipo existente y rescatado: `CharacterCreation.unity`, `Game.unity`, `CompanionPrototype.unity` y `Assets/_Project/`. Mantenerlos funcionales mientras evoluciona la compañera.
- ISEK-001: G1 técnico aceptado; G2-PROTO viable, G2-CORR **no aceptada visualmente** según DEC-005; G3/G4 pendientes. Los assets de G2 bajo `SourceArt/` son experimentales y no deben sustituir automáticamente el personaje.
- Siguiente revisión técnica: abrir en Unity, detectar errores de consola, probar creador → escena Game, persistencia de perfil y regreso a escena, adjuntar evidencia; luego reanudar iteración visual G2 separada del juego estable.

## Verificación sin ejecutar Unity

En la raíz de Git:

```bash
python tools/verify_unified_layout.py
```

GitHub Actions ejecuta la misma comprobación en push y PR. Solo valida archivos y configuración estática. Para declarar compilación o Play Mode **deben ejecutarse pruebas en Unity** y adjuntar resultados.

## Compatibilidad histórica

`Workflow_Desarrollo_v1.0.md` y las decisiones previas se conservan como documentación de la fase con dos repositorios. Los enlaces a `Isekaidunity` en documentos antiguos no definen la ubicación actual del proyecto. Esta v1.1 prevalece para operaciones Git, rutas y entregas.
