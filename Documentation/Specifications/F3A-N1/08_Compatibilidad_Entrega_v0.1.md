# F3A-N1 — Entrega, compatibilidad y límites

**Versión:** 0.1 · **Fecha:** 29/10/2026, Guatemala.  
**Estado:** DISEÑO AUTORIZADO; contenido y recomendaciones PENDIENTES DE DIRECCIÓN. Ninguna integración Unity autorizada por este documento.  
**Rama:** docs/isek-f3a-n1-character-voices, creada desde main `f835d3b` y actualizada sin commit de mezcla a `24cad113be5edc8ca1d44db1857a3f23209a2dd5` al incorporarse PR #13. La fuente canónica y el runtime no cambiaron.  
**Seguimiento:** [F3A #8](https://github.com/MarcoPolo2-0/Isekaid/issues/8) · [ISEK-001 #1](https://github.com/MarcoPolo2-0/Isekaid/issues/1).

**Actualización posterior para revisión final:** Dirección aprobó documentalmente el GDD v0.7; su publicación oficial se prepara en el [PR independiente #15](https://github.com/MarcoPolo2-0/Isekaid/pull/15). Esa aprobación no autoriza los candidatos N1. [Presentación final de biblias y 30 textos](09_Presentacion_Final_Direccion_v0.1.md), con decisiones creativas prioritarias; fichas y parche permanecen sin cambios.

## 1. Índice de entregables

| Documento | Propósito |
|---|---|
| [Biblia Tsundere](01_Tsundere_v0.1_PROPUESTA.md) | Nera, nombre de trabajo: admitir ayuda conservando independencia y criterio |
| [Biblia Yandere](02_Yandere_v0.1_PROPUESTA.md) | Liora, nombre de trabajo: intensidad en el significado del encuentro y límites recíprocos |
| [Biblia Dandere](03_Dandere_v0.1_PROPUESTA.md) | Sira, nombre de trabajo: observar, pensar y afirmar voluntad propia |
| [Revisión de los 33 ajustes](04_Revision_33_v0.1.md) | Actual/PR11/final, evaluación individual y motivo |
| [Parche editorial v0.2](05_Parche_Editorial_v0.2_PROPUESTA.json) | 33 recomendaciones revisables, sin aplicar ni sustituir biblioteca |
| [Comparación y actuación](06_Matriz_Reacciones_Actuacion_v0.1.md) | Siete contextos iguales, siete emociones y propuesta N2 con alternativas |
| [Protocolo y aceptación](07_Prueba_Ciega_Aceptacion_v0.1.md) | Medidas, criterios y clave para administración |
| [Hoja ciega separada](07b_Muestras_Ciegas_v0.1.md) | 21 muestras sin personalidad/nombre/ID de fuente, para participantes |
| [Presentación final para Dirección](09_Presentacion_Final_Direccion_v0.1.md) | Motivaciones/contradicciones, comparación y tabla de 30 candidatos con decisiones C1–C5 |
| Este informe | Dependencias, preservación del contrato, comprobaciones y decisiones pendientes |

## 2. Fuentes consultadas y estado de referencia

- [GDD v0.5](../../GameDesignDocument_v0.5.md), maestro histórico, sin modificaciones.
- [GDD v0.7 en PR #13, corte 0180b8b](https://github.com/MarcoPolo2-0/Isekaid/blob/0180b8b35f59a0a5f4357bb14549066b33a03ddc/Documentation/GameDesignDocument_v0.7_borrador.md): referencia histórica del borrador con revisiones editorial/técnica, anterior a la aprobación documental de Dirección. PR #13 se fusionó durante la preparación original de N1; la promoción oficial posterior se entrega por separado en PR #15, sin sobrescribirlo desde N1.
- [DECISIONS](../../DECISIONS.md), [workflow v1.2](../../Workflow_Desarrollo_v1.2.md), [especificación F3A](../F3A_Especificacion_v0.1.md), [eventos](../F3A_Contrato_Eventos_v0.1.md), [aceptación](../F3A_Aceptacion_v0.1.md) y [matriz emocional](../F3A_Matriz_Emocional_v0.1.md).
- [Biblioteca canónica](../F3A_Biblioteca_v0.1.json), [dirección de diferenciación](../F3A_Diferenciacion_Personalidades_v0.1_PROPUESTA.md), [revisión anterior de 60 entradas](../F3A_Diferenciacion_Revision_60_v0.1.md) y [parche PR #11](../F3A_Diferenciacion_Editorial_v0.1_PROPUESTA.json).
- Lectura del adaptador [build_f3a_runtime_content.py](../../../tools/build_f3a_runtime_content.py), [datos runtime](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/DialogueContent.cs), [director](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/DialogueDirector.cs), [presentador emocional](../../../UnityProjectIsekaid/Assets/_Project/Scripts/Companion/CompanionReactionPresenter.cs) y [UI](../../../UnityProjectIsekaid/Assets/_Project/Scripts/UI/DialoguePresenter.cs). [Auditoría técnica F3A](../../Audits/VRM-F3A/review-1/INFORME.md) y [locomoción/render](../../Audits/Companion-Locomotion-Render/INFORME.md) como evidencia previa, no repetida aquí.

Se revisa el paquete PR #11 sin convertir su merge documental en autorización de integración. El hash de la fuente vigente es `32a90c2d0d980b84d52d6a3c4e1eb18770b7ab9fbf86082562c8ced2833835ef`. Sus 33 textos propuestos aún no se aplicaron. Las tres fichas nuevas son propuestas originales, sin copiar personajes de otras obras ni afirmar que se inspeccionaron tarjetas privadas.

## 3. Resultado editorial recomendado

33 revisiones: **7 ACEPTAR, 23 MEJORAR y 3 DESCARTAR**, todas recomendaciones y no aprobaciones. Son 30 cambios candidatos y tres alertas urgentes canónicas que se conservan. Si se aprobara el candidato completo, 30 de las 60 entradas cambiarían y las otras 30 seguirían idénticas.

La diferencia se apoya en criterios, no solo en tono:

- Tsundere quiere resolver y conservar agencia, pero necesita admitir una ayuda real. Su orgullo puede dificultar recibir, no justificar insultos.
- Yandere asigna peso a lo ocurrido y busca saber qué significa para el otro, con una expectativa que debe contrastar. No es cariño genérico, romance inicial o derecho a cercanía.
- Dandere separa observación, interpretación y decisión; se reserva tiempo y puede dar una negativa clara. La valentía no requiere volverse extrovertida.

La revisión detecta un límite real en F.RESUME: puede existir un cursor de raíz o de hoja, así que no se presupone una pregunta todavía sin respuesta. También rechaza tres cambios meramente cosméticos de E.DOWNED. Mantener claridad de emergencia es más importante que forzar diferencias en cada alarma.

## 4. Matriz de compatibilidad

| Contrato / dato | Tratamiento N1 | Límite |
|---|---|---|
| Identidad y apariencia | Nombres de trabajo solo en fichas; no cambian identity, avatar o perfil | No se crea otra compañera al cambiar aspecto |
| Personalidad | Tsundere=0, Yandere=1, Shy=2; clave shy y prefijo F3A.SHY.* conservados | Dandere es etiqueta visible acordada, sin cambio de serialización/enum/UI en este PR |
| 60 IDs y orden | Se conservan todos; se revisa el conjunto exacto de 33 de PR #11 | Ningún ID se reutiliza para otro evento |
| 18 opciones / tres árboles | Textos del jugador, IDs, destinos y cierres íntegros | No se añaden nombres, preguntas sin respuesta ni rutas nuevas |
| Eventos y condiciones | Sin cambios: rescate, temas/opciones, charla, orden, incapacidad, reanimación y retorno | No hay evento de abandono, rechazo personal o percepción general |
| Memoria / Trust | writes vacío, trustDelta=0 y efectos existentes preservados | Mencionar un deseo o pensar en el rescate no implementa memoria nueva |
| Emoción / expresión | emotion, preferredExpression y fallbackExpression intactos | La nueva intención no fuerza un morph ni un canal inexistente |
| Prioridad / repetición | Valores, ámbito, grupo, consumo y enfriamiento intactos | No se repite una entrada por ser un hábito propuesto |
| Cursor / cierre | F.RESUME válido para cursor real, no inventado; raíz con opciones puede sobrevivir al cierre | Una hoja terminal cerrada se libera; no se serializan cursor, foco o episodios entre procesos |
| Navegación / supervivencia | Ninguna frase llama SetFollowing, Rescue o modifica el reloj | “Quiero apartarme” es preferencia verbal, no nueva orden o destino |
| Biblioteca/runtime | Solo copia temporal para validación; herramienta actual conserva esquema canónico 1 y runtime 2 | El archivo de parche usa patchSchemaVersion=2; no es un recurso para DialogueContent |
| Deredere y sistemas futuros | No se incorporan | Siguen fuera de esta entrega: combate, misión, promesas, memoria avanzada, romance y autonomía completa |

## 5. Cómo debe integrarse si Dirección aprueba IDs

1. Dirección decide fichas/rasgos y recomendaciones por separado. Aceptar el método o fusionar este PR no aprueba todos sus contenidos.
2. Seleccionar explícitamente IDs autorizados del parche. DESCARTAR significa mantener canónico, no borrar el nodo o aplicar primero la versión de PR #11.
3. Comprobar hash de base y valores expected. Si otra rama ya cambió fuente o contenido, detener y reconciliar con esa versión; no sobreescribirla desde esta copia.
4. Aplicar únicamente recommended.spokenText/intention de los IDs autorizados y documentar la revisión de biblioteca/libraryId según acuerdo. No alterar opciones, condicionantes, emoción o efectos aprovechando el parche.
5. Desarrollo regenera el recurso con el adaptador existente y ejecuta sus pruebas de integración y continuidad. La verificación estática de esta entrega no las sustituye.
6. Actuación N2 se tramita por separado, tras inventario real de capacidades y revisión visual. intention se transporta como dato; no hay un consumidor actual que produzca esos gestos/pausas.

Los hechos de la ficha no se vuelcan a memoria o a la biblioteca por defecto. Antes de escribir una nueva línea sobre un hábito, nombre o pasado, Dirección debe aprobar el detalle y el contexto donde puede revelarse. La biografía no concede conocimientos sobre el protagonista o su mundo.

## 6. Comprobación documental prevista

- JSON, hash de fuente y propuesta anterior; 33 IDs exactos y cuatro evaluaciones por ID.
- Correspondencia actual/PR11/final con 7/23/3 recomendaciones y 30 diferencias efectivas.
- Simulación aislada del candidato y adaptación por la herramienta vigente: 60 entradas, tres temas y 18 rutas, sin mutar fuentes.
- Comparación estructural completa, salvo texto/intención en IDs recomendados; 30 entradas completas idénticas.
- Paridad de revisión/matriz/21 tarjetas, clave separada y ausencia de nombres/etiquetas/IDs fuente en la hoja ciega.
- Campos de las tres fichas, siete contextos/emociones, enlaces relativos y archivos protegidos; historial de readme conservado.

**Resultado estático obtenido:** 33 revisiones con 7/23/3 recomendaciones; 30 cambios efectivos y 30 entradas completas idénticas en la simulación. El adaptador vigente aceptó una copia temporal y produjo 60 entradas, tres temas, 27 nodos de árbol y 18 rutas intactas. La paridad de las 21 tarjetas y su clave pasó; 47 enlaces locales válidos y nueve archivos fuente protegidos, incluido GDD v0.7, comparados con el commit base. El historial de readme se conserva. No se generó o sustituyó el recurso del juego.

Las pruebas humanas, N2 corporal, Play Mode y build quedan PENDIENTES. Estos resultados solo comprueban datos/documentación, no que los personajes ya se perciban diferentes en juego. La confirmación remota de publicación se entrega al cerrar el PR.

## 7. Decisiones que necesita Dirección

1. Aprobar/revisar la identidad propuesta de las tres fichas; decidir si nombres Nera/Liora/Sira, proyectos y trasfondos sirven como base o requieren cambios. No son nombres/objetos del juego todavía.
2. Autorizar por ID qué 7 recomendaciones acepta, qué 23 versiones mejoradas adopta y si conserva las tres alertas actuales. Puede aprobar solo un subconjunto.
3. Ratificar el protocolo y umbrales de identificación, y ejecutar lectura editorial/ciega antes de dar por lograda la diferenciación.
4. Definir la actuación mínima N2 que se encargará a Desarrollo con capacidades comprobables. No cerrar F3A-R10 por frases distintas.
5. Mantener los sistemas posteriores y Deredere fuera de alcance hasta su autorización propia. El GDD v0.7 tiene aprobación documental posterior; la publicación de su archivo oficial está en PR #15 y no autoriza N1 automáticamente.

No se pide aprobar una historia completa ni fijar todos los detalles futuros para evaluar esta entrega. La prioridad es una relación inicial entre desconocidos, con tres maneras coherentes de pensar y decidir.
