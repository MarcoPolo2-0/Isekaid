#!/usr/bin/env python3
"""Verificación estática del repositorio único Isekaid; no ejecuta Unity."""

from pathlib import Path
import subprocess
import sys

ROOT = Path(__file__).resolve().parents[1]
UNITY = ROOT / "UnityProjectIsekaid"
issues = []


def require(rel: str) -> None:
    if not (ROOT / rel).is_file():
        issues.append(f"Falta archivo requerido: {rel}")


for forbidden in (
    "Isekaidunity",
    ".gitmodules",
    "Packages",
    "ProjectSettings",
):
    if (ROOT / forbidden).exists():
        issues.append(f"Ruta obsoleta en la raíz: {forbidden}")

for folder in ("Assets", "Packages", "ProjectSettings", "SourceArt"):
    if not (UNITY / folder).is_dir():
        issues.append(f"Falta directorio Unity: UnityProjectIsekaid/{folder}")

required = (
    "UnityProjectIsekaid/ProjectSettings/ProjectVersion.txt",
    "UnityProjectIsekaid/ProjectSettings/EditorBuildSettings.asset",
    "UnityProjectIsekaid/Packages/manifest.json",
    "UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionController.cs",
    "UnityProjectIsekaid/Assets/_Project/Scripts/UI/CharacterCreationController.cs",
    "UnityProjectIsekaid/Assets/_Project/Scenes/CharacterCreation.unity",
    "UnityProjectIsekaid/Assets/_Project/Scenes/Game.unity",
    "UnityProjectIsekaid/Assets/_Project/Scenes/CompanionPrototype.unity",
    "UnityProjectIsekaid/Assets/_Project/Prefabs/Companion.prefab",
    "UnityProjectIsekaid/Assets/_Project/Prefabs/CompanionPreview.prefab",
    "UnityProjectIsekaid/Assets/_Project/Prefabs/FirstPersonPlayer.prefab",
    "Documentation/DECISIONS.md",
    "Documentation/Workflow_Desarrollo_v1.1.md",
    "Documentation/GameDesignDocument_v0.5.md",
)
for path in required:
    require(path)

for scene in ("CharacterCreation", "Game", "CompanionPrototype"):
    stem = f"Assets/_Project/Scenes/{scene}.unity"
    require(f"UnityProjectIsekaid/{stem}.meta")
    settings_path = UNITY / "ProjectSettings/EditorBuildSettings.asset"
    if settings_path.is_file() and f"path: {stem}" not in settings_path.read_text(encoding="utf-8"):
        issues.append(f"EditorBuildSettings no registra {stem}")

settings = UNITY / "ProjectSettings/ProjectVersion.txt"
if settings.is_file() and "6000.6.7f1" not in settings.read_text(encoding="utf-8"):
    issues.append("Revisar versión de Unity: esperada 6000.6.7f1")

try:
    tracked = subprocess.check_output(
        ["git", "-C", str(ROOT), "ls-files", "-z"], stderr=subprocess.DEVNULL
    ).decode("utf-8", errors="replace").split("\0")
    banned_prefixes = (
        "UnityProjectIsekaid/Library/",
        "UnityProjectIsekaid/Temp/",
        "UnityProjectIsekaid/Logs/",
        "UnityProjectIsekaid/UserSettings/",
        "UnityProjectIsekaid/Obj/",
        "UnityProjectIsekaid/Build/",
        "UnityProjectIsekaid/Builds/",
    )
    for p in tracked:
        if p.startswith(banned_prefixes):
            issues.append(f"Archivo generado versionado: {p}")
except (OSError, subprocess.CalledProcessError):
    issues.append("No se pudo consultar git ls-files")

if issues:
    print("ISEKAID_STRUCTURE_FAILED")
    for issue in issues:
        print(f"- {issue}")
    sys.exit(1)

print("ISEKAID_STRUCTURE_OK — archivos, rutas, escenas y exclusiones básicas")
print("NOTA: compilación, importación, Play Mode y builds requieren Unity")
