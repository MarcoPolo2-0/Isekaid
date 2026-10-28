# Isekaid — Registro de decisiones

**Autoridad coordinadora:** Chat 1 (Dirección).  
**Aprobación final:** usuario.  
**Actualizado:** 2026-10-27.
**Repositorio vigente:** `MarcoPolo2-0/Isekaid`.
**Documentación:** `Documentation/`.

Este documento contiene decisiones de diseño para que los Chats 2 y 3 trabajen con el mismo contexto. Una decisión aprobada de diseño **no** equivale a validación técnica ni a implementación terminada.

## DEC-001 — Base corporal personalizable para la compañera

- **Vinculada a:** `ISEK-001`.
- **Estado:** Dirección de diseño aprobada; **viabilidad técnica pendiente de comprobar**.
- **Origen:** propuesta del usuario ratificada por Chat 1.
- **Asset seleccionado como candidato principal:** [Woman, de Fly_jkey (Sketchfab)](https://sketchfab.com/3d-models/woman-b303b7606140480f8335cda121b9607c).
- **Archivo fuente descargado por el usuario:** `assets/woman.fbx` en el repositorio principal. La estructura interna de la malla y su viabilidad deben verificarse en Blender.
- **Licencia anunciada en Sketchfab:** Creative Commons Attribution (CC BY). Se permite modificación y uso comercial con atribución. Antes de publicar, conservar autor, URL del modelo, enlace a la licencia exacta, indicación de modificaciones y cualquier aviso incluido con la descarga.

### Decisión de diseño

Aprovechar el maniquí editable como punto de partida, no como personaje terminado. La apariencia neutral del editor tendrá proporciones equilibradas y el jugador podrá adaptar atributos corporales, inicialmente:

1. Pecho.
2. Caderas.
3. Cintura (sujeto a evaluación de topología y animación).

Usar preferentemente Shape Keys en Blender / Blend Shapes en Unity cuando las pruebas demuestren que son adecuados. Los controles deben ser independientes, guardar sus valores y modificar también la ropa de forma coherente. Personalidad y apariencia no están relacionadas.

El resultado final debe tener rostro anime, ojos detallados, cabello, vestimenta, materiales anime/cel shading, expresiones y rig/animaciones compatibles con Unity. La versión inicial será un personaje femenino adulto con top y pantalón sencillos.

### Asignación al Chat 2 — Diseño

Preparar `Documentation/Specifications/ISEK-001.md` con referencias, silueta neutral, rangos visuales razonables, controles, riesgos de clipping, animaciones mínimas y criterios verificables de aceptación. Distinguir propuesta de decisión ya aprobada.

### Asignación al Chat 3 — Desarrollo

Antes de sustituir el maniquí actual, validar el FBX real en Blender: topología, existencia o ausencia de rig y Shape Keys, edición, exportación, importación a Unity, deformaciones y compatibilidad con ropa/poses. Entregar informe y capturas reales. No reemplazar el recurso por otro modelo sin autorización.

### Puertas de aceptación

- **Primera puerta:** comprobar que la malla es una base viable y editable.
- **Segunda puerta:** demostrar ajustes independientes de proporciones sin defectos graves.
- **Tercera puerta:** comprobar deformación con rig, movimiento y ropa.
- **Cuarta puerta:** integrar personaje anime 3D con personalización funcional en Unity.

**No está aprobado afirmar que ISEK-001 está completada.**

## Principio de alcance del prototipo

Antes de ampliar el mundo o la historia, priorizar una compañera anime 3D creíble y personalizable, expresiones y reacciones contextuales. Reutilizar sistemas del prototipo técnico cuando sean válidos, sin convertir geometría provisional en arte definitivo.

---

Para nuevas decisiones: `DEC-002`, `DEC-003`, etc. Referenciar tareas `ISEK-###`, registrar fecha, estado, motivo y consecuencias.

## Documentación de diseño existente

- `Documentation/Especificacion_Companera_v0.1.md` está en estado **PROPUESTA** y requiere revisión del Chat 1.
- `Documentation/GameDesignDocument_v0.6_borrador.md` está en estado **PROPUESTA**; no reemplaza todavía a v0.5 como GDD aprobado.
- `Documentation/Workflow_Desarrollo_v1.0.md` regula la coordinación de los tres chats.

La decisión DEC-001 **no aprueba automáticamente** la totalidad de la especificación v0.1 ni el GDD v0.6.

## Organización técnica aprobada — dos repositorios

- **Principal:** `MarcoPolo2-0/Isekaid` (público) para `Documentation/`, recursos originales, decisiones e Issues.
- **Unity:** `MarcoPolo2-0/Isekaid-2026-10-27_13-09-18` (privado) para el proyecto Unity 6 real.
- **Vínculo:** submódulo `Isekaidunity`, declarado en `.gitmodules` del repositorio principal. Referencia a un commit fijo; el principal se actualiza manualmente cuando se apruebe una versión del código Unity.
- Consultar `Documentation/Workflow_Desarrollo_v1.0.md` para procedimientos de actualización y trabajo.
