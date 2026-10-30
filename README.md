# Isekaid

Juego 3D en Unity 6 · aventura isekai en primera persona con compañera anime personalizable.

**Repositorio unificado:** código Unity, prototipo jugable, documentación y fuentes artísticas viven en este mismo GitHub. La carpeta principal de Git **no es** el proyecto Unity.

## Abrir el juego

1. Clonar el repositorio y preparar Git LFS (`git lfs install`, `git lfs pull`) para recuperar modelos y binarios versionados.
2. En **Unity Hub → Add project from disk**, elegir **`UnityProjectIsekaid/`**, no la raíz del repositorio.
3. Abrir con **Unity 6000.6.7f1**. La escena de inicio configurada es `Assets/_Project/Scenes/CharacterCreation.unity` y la siguiente es `Game.unity`.
4. Verificar la ventana Console y el flujo de creación/aventura. La existencia de archivos en GitHub no sustituye pruebas locales de compilación ni Play Mode.

## Estructura

```text
Isekaid/
├── UnityProjectIsekaid/
│   ├── Assets/            # Scripts, escenas, prefabs y recursos Unity
│   ├── Packages/          # Dependencias Unity
│   ├── ProjectSettings/   # Ajustes Unity y escenas de build
│   └── SourceArt/         # Fuentes Blender y prototipos experimentales
├── Documentation/         # GDD, especificaciones, criterios y decisiones
├── assets/                # Fuente original woman.fbx
├── tools/                 # Comprobaciones estáticas de estructura
├── .github/workflows/     # Validación automática
├── .gitignore
└── .gitattributes
```

## Continuar desarrollo

- [Workflow vigente v1.2](Documentation/Workflow_Desarrollo_v1.2.md)
- [Decisiones de Dirección](Documentation/DECISIONS.md) — consultar las más recientes; DEC-006 establece la nueva estructura.
- [Issue ISEK-001](https://github.com/MarcoPolo2-0/Isekaid/issues/1) — seguimiento de la compañera y sus criterios.
- [GDD maestro oficial v0.7](Documentation/GameDesignDocument_v0.7.md) — aprobación documental de Dirección; no autoriza automáticamente propuestas o integraciones.
- Historial íntegro: [v0.5](Documentation/GameDesignDocument_v0.5.md), [v0.6](Documentation/GameDesignDocument_v0.6_borrador.md) y [borrador v0.7](Documentation/GameDesignDocument_v0.7_borrador.md).

El prototipo anterior y las evidencias G2-PROTO/G2-CORR se conservan. Woman/G2 y el editor corporal están pausados por DEC-007; sus resultados no se declaran aceptados por adoptar VRM. La revisión de F3A-N1 sigue en el [PR #14](https://github.com/MarcoPolo2-0/Isekaid/pull/14), sin autorización automática para aplicar diálogos. Trabajar en ramas/PR por hito y someter las entregas a Dirección antes de fusionar o reemplazar modelos de producción.

Este repositorio es **público**. Revisar licencias de recursos externos y no publicar secretos o datos privados.
