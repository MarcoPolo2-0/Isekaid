# ISEKAID — Workflow de desarrollo

**Versión:** 1.0  
**Proyecto:** Isekaid  
**Motor:** Unity 6  
**Documento base:** GameDesignDocument_v0.5.md

## Objetivo

Coordinar tres chats especializados para desarrollar Isekaid manteniendo una única visión creativa, arquitectura limpia y entregas verificables.

## Roles

### Chat 1 — Dirección del proyecto (Cerebro)

Es la autoridad para establecer prioridades, aprobar especificaciones y aceptar o rechazar entregas.

Responsabilidades:

- Recibir nuevas ideas del usuario.
- Consultar el GDD y las decisiones existentes.
- Determinar qué se debe diseñar o implementar.
- Asignar identificadores a las tareas.
- Revisar las especificaciones.
- Aprobar el inicio del desarrollo.
- Evaluar resultados y establecer correcciones.

### Chat 2 — Game Design

Transforma los requisitos aprobados en especificaciones técnicas y creativas.

Responsabilidades:

- Mantener el GDD.
- Diseñar sistemas y comportamientos.
- Preparar referencias visuales.
- Definir criterios de aceptación.
- Registrar decisiones aprobadas.
- Entregar especificaciones versionadas.

No modifica código fuente.

### Chat 3 — Unity Development

Implementa las especificaciones aprobadas.

Responsabilidades:

- Mantener el proyecto Unity 6.
- Crear e integrar assets.
- Escribir código C#.
- Mantener arquitectura modular.
- Ejecutar pruebas.
- Producir builds y evidencias visuales.
- Registrar resultados técnicos.
- Realizar commits y push a GitHub cuando corresponda.

No modifica unilateralmente requisitos de diseño.

## Flujo de trabajo

**Paso 1: Solicitud**

El usuario presenta una idea o problema al chat principal.

El chat principal crea una tarea con:

- ID.
- Objetivo.
- Alcance.
- Prioridad.
- Restricciones.
- Resultado esperado.

**Paso 2: Diseño**

El chat de diseño elabora una especificación basada en la tarea.

Debe incluir:

- Comportamiento esperado.
- Requisitos visuales cuando corresponda.
- Dependencias.
- Casos especiales.
- Criterios verificables de aceptación.

**Paso 3: Aprobación**

El chat principal revisa la especificación.

Estados posibles:

- PROPUESTA.
- APROBADA.
- REQUIERE CAMBIOS.

Solo las especificaciones aprobadas pasan a desarrollo.

**Paso 4: Implementación**

El chat de desarrollo utiliza la especificación aprobada.

Debe conservar los sistemas existentes que sean reutilizables y trabajar por hitos pequeños.

**Paso 5: Pruebas**

El desarrollador entrega un reporte con:

- Funciones implementadas.
- Archivos modificados.
- Pruebas ejecutadas y resultados.
- Evidencia visual real cuando corresponda.
- Problemas conocidos.
- Commit de Git asociado, si existe.

**Paso 6: Validación**

El chat principal compara los resultados con los criterios de aceptación.

Resultados:

- ACEPTADO.
- REQUIERE CORRECCIONES.
- BLOQUEADO.
- REDISEÑO NECESARIO.

**Paso 7: Cierre**

Una tarea aceptada se documenta y se marca como completada.

Si una nueva idea modifica un requisito aprobado, debe registrarse como cambio de alcance.

## Reglas generales

1. Nunca sustituir la visión aprobada por una versión simplificada sin consentimiento.
2. Los assets provisionales deben identificarse como tales.
3. No marcar como terminada una funcionalidad que no haya sido verificada.
4. No implementar múltiples sistemas grandes simultáneamente.
5. No introducir dependencias de APIs generativas de pago en el juego.
6. Mantener Unity compilable y una versión estable del proyecto.
7. Evitar que dos chats modifiquen el mismo archivo simultáneamente.
8. Priorizar la compañera, su personalización y sus interacciones.
9. Los documentos aprobados y los informes de implementación son la fuente de verdad entre chats.
10. El chat principal decide qué tarea se realiza después.

## Repositorio y fuente de verdad

- **Repositorio oficial vigente:** [MarcoPolo2-0/Isekaid](https://github.com/MarcoPolo2-0/Isekaid), rama `main`. No abrir nuevas tareas en el repositorio anterior `Isekaid-2026-10-27_13-09-18`.
- **Carpeta documental:** `Documentation/`. Antes de trabajar, ambos chats deben leer `Documentation/DECISIONS.md`, este workflow y la especificación pertinente.
- **Estado documental:** `GameDesignDocument_v0.5.md` es la base aprobada; `GameDesignDocument_v0.6_borrador.md` y `Especificacion_Companera_v0.1.md` son propuestas hasta revisión explícita del Chat 1.
- **Seguimiento:** GitHub Issues `ISEK-###`. Los cambios técnicos deben incluir evidencia y commits o PR cuando corresponda.
- **Sincronización:** cada chat debe consultar GitHub antes de iniciar. La carpeta local no recibe cambios automáticamente: actualizar con `git pull` tras verificar `git status`.
- **Auditoría estructural pendiente:** la entrada `Isekaidunity` aparece en GitHub como submódulo Git sin URL configurada; se debe verificar que el proyecto Unity real tenga sus archivos `Assets/`, `Packages/`, `ProjectSettings/` versionados correctamente, antes de afirmar que el código está respaldado en el repositorio oficial.
