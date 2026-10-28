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

## DEC-002 — Revisión de entregas ISEK-001 y autorización de prototipo corporal controlado

**Fecha:** 28/10/2026.  
**Estado:** APROBADA COMO AUTORIZACIÓN DE TRABAJO ACOTADO. **NO** implica aceptación artística ni cierre de ISEK-001.  
**Tarea:** [ISEK-001](https://github.com/MarcoPolo2-0/Isekaid/issues/1).

### Evidencia revisada

- Diseño: [Especificación ISEK-001 v0.1](Specifications/ISEK-001.md), entrega en commit `1874c868f5730765914abe82d99ab8e6e693218a`.
- Técnica: [Auditoría ISEK-001](Audits/ISEK-001/INFORME.md), entrega en commit `62b613e0b470c7c8f510dcfb87e0c0057ba3a0ca`.
- El original `assets/woman.fbx` conserva el hash registrado por ambos informes.

### Resolución de Dirección — Chat 1

1. **Se acepta la evaluación técnica de la puerta G1**: la malla Woman se considera viable para continuar el prototipado, sin declarar terminada su adaptación artística, su licencia detallada ni una integración de producción.
2. **G2: factibilidad demostrada, resultado artístico pendiente.** Los ensayos de Blender generaron controles independientes de pecho, caderas y cintura y 27 combinaciones; el FBX diagnóstico se importó en un proyecto Unity aislado con Avatar Humanoid válido y Blend Shapes. Esto autoriza seguir explorando los tres ejes, **no** aprobar extremos, aspecto neutral definitivo ni declarar acabada cintura.
3. **G3 y G4 continúan pendientes.** Se observaron penetraciones en prendas de prueba, deformaciones de pesos automáticos y ausencia de rostro anime, cabello, animaciones finales y prueba en las escenas del juego. No presentar estas partes como completas.
4. **Se aprueba la especificación v0.1 solo como marco de trabajo de las puertas G1–G4 y de sus 16 casos de aceptación.** Las cifras, tiempos, amplitudes estéticas y elementos etiquetados como propuestas siguen sujetos a revisión visual y funcional. El GDD v0.6 continúa en BORRADOR.
5. **Se autoriza a Chat 3 una etapa limitada G2-PROTO** en rama separada del repositorio Unity privado: preparar una **versión neutral corporal editable** y **tres controles experimentales** de pecho/caderas/cintura; mejorar transiciones y evaluar pesos/ropa con pruebas reproducibles. La ropa de ensayo y el esqueleto diagnóstico **no** son recursos finales. No sustituir el maniquí de las escenas actuales ni fusionar con `main` antes de revisión del Chat 1.
6. **Se solicita a Chat 2 revisión visual enfocada:** comparar turnaround neutral con dirección anime, documentar rangos y referencias faciales/cabello, preparar criterios de aprobación visual que no dependan solo de métricas. Las versiones nuevas de requisitos se entregan como propuestas y se revisan aquí antes de ampliar el alcance de Chat 3.

### Condiciones de entrega de G2-PROTO

- Archivo Blender derivado editable y FBX exportable en la rama de trabajo de Unity, sin alterar el original del repositorio principal; registrar procedencia, versión y hash.
- Turnaround frontal, perfil, tres cuartos y espalda del cuerpo neutral; vistas comparables de los extremos y matriz de 27 combinaciones.
- Informe de fallos visibles de ropa/skin y cómo se corrigen; no ocultar clipping, ni declarar completa la aceptación G3.
- Evidencia de reimportación correcta en Unity; preferentemente ensayos en entorno separado y sin tocar escenas de producción.
- Informe al Issue `ISEK-001`, commit/PR y evidencias; advertir explícitamente qué no está validado.
- **La autorización termina en la presentación de G2-PROTO**: rostro anime completo, ropa final, locomoción integrada y reemplazo en creador/aventura requieren revisión de la siguiente puerta.

### Riesgos y obligaciones abiertas

- Verificar variante exacta CC BY, atribución, enlaces de licencia y modificaciones antes de distribuir derivados.
- No fijar presupuesto de rendimiento/plataforma sin mediciones y decisión de alcance.
- No actualizar la referencia del submódulo del repositorio principal por una prueba todavía no aceptada.

**Próxima revisión de Chat 1:** aceptación artística del cuerpo neutral y rangos, seguida de autorización específica para G3/G4.
