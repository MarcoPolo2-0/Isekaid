# VRM — Matriz de aceptación y trazabilidad F2/F3

**Versión:** 0.1 · **Fecha:** 28/10/2026. **Autoridad:** [DEC-007](../DECISIONS.md).  
**Estado:** matriz de seguimiento; no resultados de ejecución nuevos. [Issue F2 #5](https://github.com/MarcoPolo2-0/Isekaid/issues/5), [ISEK-001 #1](https://github.com/MarcoPolo2-0/Isekaid/issues/1).

## 1. Fuentes y lectura de estados

- [VRM v0.2](Sistema_Companera_VRM_v0.2.md): 26 criterios originales y reparto F1=5, F2=11, F3=8, F4=2; versión histórica fusionada sin editar.
- [Contrato F2](VRM_Fase2_Contrato_Funcional_v0.1.md): 18 comprobaciones detalladas para seguimiento.
- [F3 borrador](VRM_Fase3_Personalidad_Contextual_v0.1_BORRADOR.md): diez comprobaciones propuestas del siguiente hito.
- [POC](../Audits/VRM-POC/INFORME.md), [estabilidad](../Audits/VRM-POC/stability-results.json): pruebas publicadas por Desarrollo; Diseño las leyó y contrastó con PR fusionados, no reejecutó Unity.

“Evidencia parcial” describe cobertura de ensayo; “pendiente” significa que no se presenta evidencia funcional suficiente. Ninguna celda declara cumplimiento final de un caso solo por existir un archivo. DEC-007 adopta la viabilidad técnica acotada y autoriza F2; no da por aceptados todos los criterios F1 ni la seguridad de producción.

## 2. Los 26 criterios originales sin renumerar

| ID original | Fase v0.2 | Detalle o evidencia relacionada | Estado de revisión documental |
|---|---|---|---|
| VRM-AC01 | F2 | F2-AC01, F2-AC12 | Catálogo/recorrido persistente offline pendientes |
| VRM-AC02 | F1 | PR #4; F2-AC02/03 para integración | Carga local/Humanoid/materiales en un avatar de ensayo; no catálogo ni build |
| VRM-AC03 | F1 | Rechazos POC; F2-AC02/11 | Evidencia acotada de datos inválidos; no cobertura adversarial completa |
| VRM-AC04 | F2 | F2-AC13 | POC rechaza concurrencia y libera al concluir SDK; UI/cancelación del creador pendientes |
| VRM-AC05 | F4 | Matriz tres apariencias × tres personalidades × clases habilitadas | Objetivo posterior; F2-AC04 verifica solo ambas vías iniciales |
| VRM-AC06 | F3 | F3-AC01/03 | Reacciones iniciales del prototipo no acreditan toda profundidad contextual |
| VRM-AC07 | F3 | F3-AC02/09 | Promesas y persistencia contextual pendientes |
| VRM-AC08 | F3 | F3-AC07/08 | POC aplica presets; la alternativa ante falta de canal no está acreditada como gameplay |
| VRM-AC09 | F3 | F3-AC04/07 | Comedia/retorno y variantes entre avatares pendientes |
| VRM-AC10 | F3 | F3-AC04 | Director de conversación e interrupción contextual pendientes |
| VRM-AC11 | F3 | F3-AC07/08 | Rostro estático con conducta íntegra pendiente |
| VRM-AC12 | F2 | F2-AC06 | Reemplazo visual POC comprobado; conservación persistente completa pendiente |
| VRM-AC13 | F2 | F2-AC10 | Fallos visuales por etapa probados; archivos/guardado y crash recovery pendientes |
| VRM-AC14 | F2 | F2-AC09/12 | Copia gestionada independiente de fuente pendiente |
| VRM-AC15 | F2 | F2-AC09 | Recuperación de copia faltante/dañada pendiente |
| VRM-AC16 | F2 | F2-AC07/08 | Migración v1 no implementada por el POC |
| VRM-AC17 | F2 | F2-AC14/15 | Metadatos leídos no son revisión humana ni autorización de catálogo |
| VRM-AC18 | F1 | Guardas POC; F2-AC11/12 | Carga local y rechazos documentados; seguridad y monitorización del flujo integrado pendientes |
| VRM-AC19 | F1 | Límites POC; F2-AC11/16 | Seis ciclos, recursos y memoria acotados; presupuesto de producción/fixtures de frontera pendientes |
| VRM-AC20 | F1 | Humanoid POC; F2-AC02/03/06 | Un avatar compatible; intervalos de escala, rigs inválidos y ventajas por tamaño no aceptados |
| VRM-AC21 | F3 | Presets POC; F3-AC07/08 | Aplicaciones individuales no acreditan todas las mezclas/exclusiones de gameplay |
| VRM-AC22 | F2 | F2-AC09/10 | Referencias, eliminación y recuperación entre partidas/versiones pendientes |
| VRM-AC23 | F4 | Tres entradas; F2-AC15 para primera incluida | Catálogo completo pendiente; no se posponen derechos del recurso inicial hasta F4 |
| VRM-AC24 | F2 | F2-AC18 | Cambio diferido y continuidad vital de UI integrada pendientes |
| VRM-AC25 | F3 | F3-AC05/06 | Iniciativa/desacuerdo contextual pendientes de hito gameplay |
| VRM-AC26 | F2 | F2-AC05/13 | Muestras aisladas no deben afectar memoria; validar en creador real |

Los IDs originales permanecen en su fase. Comprobaciones de regresión compartidas no duplican el criterio ni trasladan su aceptación. Una F2 parcial debe declararse parcial; pruebas F3/F4 no bloquean el alcance inicial por sí mismas.

## 3. Instrucción de Dirección → entregable

| Instrucción | Documento / comprobación | Estado de aprobación |
|---|---|---|
| Adoptar VRM 1.0 + catálogo offline | DEC-007; F2 §§1–4; F2-AC01/02/12 | Decisión recibida aprobada; funcionalidad pendiente |
| Independencia de identidad/personalidad/memoria/clase/representación | F2 §§5–6; F2-AC04/06/07; F3 §§2–4 | Principio aprobado; migración/detalle en revisión |
| Una incluida y una importada | F2 §§1/4; F2-AC01/02/15 | Alcance inicial aprobado; asset distribuible pendiente |
| Tres personalidades profundas | F2 §5; matriz F3 §6; F3-AC01–10 | Se conserva visión aprobada; F3 es borrador sin implementación autorizada |
| Cancelar y recuperar sin pérdida | F2 §§3/6/7; F2-AC09/10/13 | Invariante de v0.2; precisión de interacción F2-P04 en revisión |
| Migrar v1 sin borrar datos | F2 §6; F2-AC07/08 | Requisito aprobado; esquema/detalle F2-P01 en revisión |
| Responsive y accesibilidad | F2 §8; F2-AC17 | Resoluciones requeridas; cifras/layout concretos F2-P02 propuestos |
| Permisos y catálogo | F2 §§4/9; F2-AC14/15 | Revisión humana pendiente por recurso, no suplantada por un test |
| Pausar editor propio/Woman/G2 | DEC-007; estado ISEK-001; Issue #1 | Instrucción aprobada; recursos/historia preservados |
| Mantener GDD v0.5 | Nueva enmienda §22 de borrador v0.6 | v0.5 intacto; v0.6 sigue borrador, no aprobación global |
| No bloquear Chat 3 | F2 §1/12; workflow v1.2 | V0.2 fusionada y autorización independiente siguen suficientes para continuar |

## 4. Pendientes y evidencias exigidas

- Recurso de catálogo autorizado: no seleccionado en esta entrega. El modelo del POC sigue restringido; derechos y distribución no pasan por tener Avatar válido.
- Plataformas/builds y presupuestos: recoger mediciones; límites y tolerancias diagnósticas no se convierten en garantías finales.
- Guardado/transacción: comparar bytes/estados y recuperar tras fallos/cierre entre etapas; no basta una captura de la UI.
- Seguridad/cancelación: delimitar cobertura y cancelación lógica; no declarar interrupción inmediata o aislamiento de proceso si no existen.
- Comportamiento F3: secuencias de hechos, decisiones y persistencia; no acreditarlo por screenshots faciales.
- Mantener el progreso de #2 independiente; pruebas originales repetidas en POC son evidencia para revisión, no cierre automático de aquel Issue.

## 5. Registro y siguiente revisión

F2 se acepta por resultados reales y revisión de Chat 1. F3/F4 requieren alcance propio. El PR documental no fusionado contiene detalles propuestos que no se aplican automáticamente a código; cambios funcionales relevantes se señalan en el PR.

**Registro 0.1:** trazabilidad DEC-007, 26 criterios originales, 18 comprobaciones F2, diez criterios F3. Todos los resultados nuevos están pendientes; pruebas antiguas se atribuyen a sus informes y PR.
