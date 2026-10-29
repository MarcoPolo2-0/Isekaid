# F3A — Propuestas futuras separadas del diseño autorizado

**Versión:** 0.1 · **Estado:** PROPUESTAS, sin autorización de implementación por esta entrega.  
[Especificación F3A](F3A_Especificacion_v0.1.md) · [borrador amplio F3](VRM_Fase3_Personalidad_Contextual_v0.1_BORRADOR.md) · [GDD](../GameDesignDocument_v0.5.md).

## 1. Qué usa F3A ahora

Hechos existentes de primer rescate, RemembersFirstRescue, Trust, estados vitales y órdenes; entradas de conversación, cursor/consumo y adaptadores propuestos para integrar la biblioteca. El registro de qué texto se presentó no se vende como memoria emocional persistente.

Las opciones no cambian confianza, no crean una promesa y no modifican IsFollowing. El encuentro puede conservarse durante F2 sin que eso pruebe guardado narrativo entre procesos. Peligro/seguridad general se ensaya solo con señales rotuladas cuando no haya percepción fiable.

## 2. Extensiones futuras que requieren revisión de alcance

| ID | Propuesta futura | Dependencia y prueba antes de aceptarla |
|---|---|---|
| F3A-FUT01 | Memoria estructurada persistente de campaña, participantes y significado de hechos | Guardado completo de aventura, migración y deduplicación entre checkpoint/reinicio; no sobrescribir el recuerdo booleano ni premiarlo otra vez |
| F3A-FUT02 | Promesa explícita con condición acordada, cumplida/incumplida y posible reparación | Diseño de elecciones y fuente de cumplimiento fiables; pausa/silencio no causan incumplimiento. No derivar promesas del texto casual actual |
| F3A-FUT03 | Evolución gradual de confianza/rasgos con causas consultables | Reglas de relación, anti-explotación y escenas significativas; no farmear Talk ni cambiar de arquetipo por umbral |
| F3A-FUT04 | Percepción general de peligro y director conversacional en exploración/combate | Detectores/estados de gameplay verificados; prioridad de supervivencia y pruebas de señales falsas/caducadas |
| F3A-FUT05 | Iniciativas autónomas más allá de responder y proponer temas locales | Preferencias/hábitos, motivos y ventanas seguras; no inventar pistas o misiones sin fuente real |
| F3A-FUT06 | Atlas cómico o expresiones custom certificadas por entrada de catálogo | Recursos y derechos, oclusión de cara normal, exclusiones y retorno; no modificar avatares externos arbitrariamente |
| F3A-FUT07 | Voces, sincronía labial y animaciones de gesto finales | Assets licenciados, lectura temporal/interrupción y pruebas por rig. spokenText y gestureId no son archivos de audio o clips presentes |
| F3A-FUT08 | Convivencia, misiones personales, progresión táctica y romance recíproco | Hitos del GDD con narrativa propia; amistad completa. No se eliminan por acotar F3A ni se implementan con una tabla de emociones |

## 3. Decisiones menores de esta entrega, a ajustar tras revisión

IDs y esquema JSON de contenido; tres temas con dos opciones, cierre voluntario; 20 segundos activos como enfriamiento casual propuesto; prioridades numéricas solo de presentación; consumo por encuentro y por episodio, con cero premios por charla.

Estas precisiones sirven para revisar/integrar F3A y no cambian el GDD. Si una requiere nueva arquitectura, reglas de relación o altera gameplay de forma importante, Chat 3 la devuelve a Dirección antes de asumirla por estar en un PR no fusionado.

## 4. Conservación de alcance

No reabrir el editor corporal Woman/G2, no reemplazar VRM 1.0 por otro formato, no vincular avatar a personalidad/clase. Mantener el prólogo, incapacidad y rescates, autonomía, combate, hogar y progresión previstos. No crear enemigos o misiones como si ya estuvieran implementados.

La revisión manual F2 corresponde al usuario y no la concede este paquete. Chat 3 continúa su infraestructura autorizada de forma independiente. Dirección decidirá la integración concreta de F3A y cualquier extensión de Fase 3; el PR no se fusiona automáticamente.
