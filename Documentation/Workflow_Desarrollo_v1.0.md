> **ARCHIVO HISTÓRICO:** este workflow v1.0 describía el esquema de dos repositorios y submódulo, ya descontinuado según DEC-006. Para toda operación nueva, usar [Workflow v1.1](Workflow_Desarrollo_v1.1.md). Se conserva el texto original por trazabilidad.

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

## Repositorios, submódulo y fuente de verdad

### Repositorio principal (público)
- [`MarcoPolo2-0/Isekaid`](https://github.com/MarcoPolo2-0/Isekaid) — rama `main`.
- Contiene `Documentation/` (GDD, especificaciones, decisiones y workflow), `assets/` (recursos originales), Issues `ISEK-###` y la referencia al submódulo `Isekaidunity/`.
- Chat 1 registra decisiones y gestiona Issues; Chat 2 actualiza documentación y remite propuestas para aprobación.
- **Antes de trabajar**, leer `Documentation/DECISIONS.md`, este workflow y la especificación aplicable.
- `GameDesignDocument_v0.5.md` es la base aprobada; `GameDesignDocument_v0.6_borrador.md` y `Especificacion_Companera_v0.1.md` siguen en estado PROPUESTA hasta aprobación explícita del Chat 1.

### Repositorio Unity (privado)
- [`MarcoPolo2-0/Isekaid-2026-10-27_13-09-18`](https://github.com/MarcoPolo2-0/Isekaid-2026-10-27_13-09-18) — rama `main`.
- Contiene el proyecto Unity real: `Assets/`, `Packages/`, `ProjectSettings/`.
- **Chat 3 desarrolla aquí**; debe usar commits/ramas/PR y entregar evidencia de pruebas. No subir por duplicado toda la carpeta Unity al repositorio principal.
- **No es un repositorio eliminado ni de documentación obsoleta**: es la fuente de verdad del código Unity.

### Conexión entre repositorios
- El principal registra el submódulo `Isekaidunity` en el archivo `.gitmodules`, con URL HTTPS del repositorio Unity privado y rama sugerida `main`.
- El submódulo **apunta a un commit fijo** del repositorio Unity; hacer push en Unity **no** actualiza automáticamente ese puntero en el principal. Actualizar el puntero mediante un commit del repositorio principal solo cuando se quiera incorporar una nueva versión del juego.
- Para inicializar una copia local desde cero: `git clone --recurse-submodules https://github.com/MarcoPolo2-0/Isekaid.git` (requiere acceso al repositorio privado).
- Para una copia existente: primero comprobar `git status`; después `git pull origin main`, `git submodule sync --recursive` y `git submodule update --init --recursive`, **solo si no existen cambios locales del submódulo que deban conservarse**.
- Trabajar normalmente en el código Unity desde su propio repositorio. Si el submódulo está en `detached HEAD` por estar fijado a un commit, cambiar a una rama de trabajo explícita antes de crear commits; no hacer cambios sin verificar estado, remoto y rama.
- Si el Chat 3 actualiza la versión Unity que consumirá el principal, registrar su commit SHA, actualizar la referencia `Isekaidunity` y documentar la versión compatible.
- El repositorio principal **es público**, mientras Unity es **privado**. Los usuarios sin acceso al privado no podrán inicializar el submódulo. No publicar secretos, assets privados, datos sensibles ni el código del juego en el repositorio principal.

### Seguimiento y sincronización
- Seguimiento global: GitHub Issues `ISEK-###` **en el repositorio principal**.
- Documentación compartida: `Documentation/` del repositorio principal.
- Entrega técnica: commits o PR en el repositorio Unity privado, enlazados al Issue del principal.
- Ningún chat recibe automáticamente los mensajes de otro: consultar las versiones más recientes en GitHub al iniciar.
- Los cambios publicados en GitHub no aparecen por sí solos en una carpeta local: actualizar desde Git después de revisar archivos sin confirmar.
