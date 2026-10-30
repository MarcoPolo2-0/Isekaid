# F3A-N1 — Microrevisión trazable C2/C5

**Versión:** 0.1 · **Fecha:** 29/10/2026, Guatemala.
**Estado:** PROPUESTA DE TEXTOS, pendiente de selección de IDs por Dirección.
**Origen:** ajustes finales solicitados por Dirección tras revisar el commit `01f3522`.
**Candidato activo:** [parche editorial v0.5](17_Parche_Editorial_v0.5_PROPUESTA.json).
**Antecedente íntegro:** [parche v0.4](15_Parche_Editorial_v0.4_PROPUESTA.json) y [comparación anterior](14_C2_C5_Dramatica_v0.1.md).
**Dirección dramática:** DEC-009 satisfactoria; [biblia candidata](13_Yandere_Dramatica_v0.2_PROPUESTA.md). Esta microrevisión no fija nombre, trasfondo o detalles biográficos.

## 1. Comparación de textos

### F3A.YAN.A.RESCUE

| Versión | Texto exacto |
|---|---|
| Propuesta v0.4, sin aplicar | No sé cómo te llamas todavía... pero no quiero olvidar tu cara. Gracias por sacarme de ahí. |
| Recomendación v0.5, sin aplicar | No sé cómo te llamas todavía... pero no quiero olvidar ese gesto tuyo. Gracias por soltar el lazo. |

**Intención conservada:** `n1_yan_a_rescue`.

**Motivo:** La atención intensa se fija en el gesto de rescate conocido; elimina la dependencia de un rostro del protagonista que el prefab de primera persona no garantiza.

### F3A.YAN.B.FEEL.OPEN

| Versión | Texto exacto |
|---|---|
| Propuesta v0.4, sin aplicar | Estoy bien... No, mentira. Me asusté. Y aun así, ahora mismo tengo más curiosidad por ti que por salir de este sitio. |
| Recomendación v0.5, sin aplicar | Estoy bien... No, mentira. Me asusté. Necesito alejarme del lazo, pero me distrae lo mucho que quiero saber de ti. |

**Intención conservada:** `n1_yan_b_feel_open`.

**Motivo:** Mantiene la autocorrección y su necesidad de alejarse; la curiosidad intensa interfiere con su atención al peligro sin convertir quedarse junto a la trampa en su objetivo.

### F3A.YAN.C.LATER_1

| Versión | Texto exacto |
|---|---|
| Propuesta v0.4, sin aplicar | Sigo pensando en el lazo... No, en cuando lo soltaste. Lo que más se me queda es ese gesto tuyo. |
| Recomendación v0.5, sin aplicar | Sigo pensando en el lazo... No, más bien en cómo lo soltaste. Lo que más se me queda es ese gesto tuyo. |

**Intención conservada:** `n1_yan_c_later_1`.

**Motivo:** Sustituye la construcción antinatural «en cuando» por «más bien en cómo»; la autocorrección desplaza el foco de la trampa a la acción del otro y conserva la fijación incipiente.

### F3A.YAN.E.REVIVED

| Versión | Texto exacto |
|---|---|
| Propuesta v0.4, sin aplicar | Gracias... Me alivia poder moverme. Lo extraño es cuánto me alivia que sigas aquí. |
| Recomendación v0.5, sin aplicar | Gracias... Me alivia poder moverme. Lo extraño es cuánto me alivia que sigas aquí. |

**Intención conservada:** `n1_yan_e_revived`.

**Sin cambios.** Dirección lo considera candidato preferido por su alivio desproporcionado ante la presencia del protagonista. Preferencia no equivale a autorización final o integración.

## 2. Evidencia y límites del ajuste

**Representación del protagonista:** inspección estática del [prefab FirstPersonPlayer](../../../UnityProjectIsekaid/Assets/_Project/Prefabs/FirstPersonPlayer.prefab), en el [corte 01f3522](https://github.com/MarcoPolo2-0/Isekaid/blob/01f3522a2bebbad929c16ac1e59f963e53f14520/UnityProjectIsekaid/Assets/_Project/Prefabs/FirstPersonPlayer.prefab). Contiene controlador de personaje y cámara de primera persona; no declara un renderizador o malla facial. Esto no demuestra ausencia de toda representación creada en ejecución o añadida por una escena, pero no garantiza un rostro visible para esta frase. Se elimina esa dependencia y se usa el gesto conocido de liberar el lazo. No se modifica el prefab ni se realiza una comprobación en Play Mode.

**Peligro y curiosidad:** FEEL.OPEN mantiene «Estoy bien... No, mentira» y reconoce que necesita alejarse del lazo. «Me distrae» describe la competencia emocional entre seguridad y curiosidad, no una acción de caminar, fallo de atención simulado, bloqueo de controles o pausa nueva. El texto no exige quedarse junto a la trampa y las opciones BREATH/LATER siguen intactas.

**Naturalidad:** C.LATER_1 corrige «en cuando» por «más bien en cómo». Conserva la autocorrección y cambia el foco desde la trampa hacia lo que hizo el desconocido. No exige tiempo transcurrido nuevo ni crea una memoria.

**Reanimación:** E.REVIVED mantiene exactamente texto, intención y el resto de la revisión v0.4. No hay amor instantáneo, exclusividad, promesa de quedarse o deuda emocional. Sigue candidato preferido, sin aplicación autorizada.

## 3. Alcance y trazabilidad del parche

- v0.5 es un manifiesto completo con los mismos 33 IDs revisados: 7 ACEPTAR, 23 MEJORAR y 3 DESCARTAR. Son dictámenes editoriales, no aprobaciones.
- Solo tres spokenText cambian frente a v0.4. Sus evaluaciones y motivos se actualizan y conservan la recomendación anterior con versión de origen. Las otras 30 revisiones del manifiesto permanecen idénticas, incluidas E.REVIVED y las tres E.DOWNED que se recomienda conservar canónicas.
- Siguen siendo 30 cambios candidatos frente a la canónica. **Los otros 26 candidatos permanecen sin autorización definitiva.** Tampoco quedan autorizados estos tres textos nuevos o E.REVIVED por mantenerlo preferido.
- Todas las intenciones recomendadas permanecen iguales. No hay una justificación que exija cambiarlas.
- IDs, orden, condiciones, rutas/opciones, disparadores, emociones, expresiones, prioridad, repetición, efectos y memoria se conservan. Shy=2, shy y F3A.SHY.* no cambian.
- Se preserva el esquema de hashes verificados: SHA-256 UTF-8 sin BOM, normalizando únicamente CRLF/CR a LF; commit/path/blob y expected siguen siendo controles de base. El hash del manifiesto v0.4 se registra en supersedes y se comprueba contra el archivo y su blob del commit revisado.
- Revisar/aplicar en el futuro solo los IDs/campos autorizados contra la canónica vigente. No aplicar primero v0.4 y luego v0.5. Un cambio de base requiere reconciliación, sin omitir hashes o expected.

## 4. Prueba ciega, pendiente hasta cierre de textos

No se regeneran todavía [muestras históricas v0.2](07b_Muestras_Ciegas_v0.1.md) o su clave. Dirección ha pedido actualizarlas **cuando los textos estén cerrados**, y esta entrega todavía necesita revisión. Las muestras anteriores no sirven para puntuar v0.5.

Tras cierre explícito: preparar una versión nueva separada de las muestras/clave a partir de los IDs autorizados, registrar commit/hash y reemplazar los estímulos afectados desde v0.2, sin mezclar versiones. Conservar anonimato, contextos, elecciones y umbrales del [protocolo](07_Prueba_Ciega_Aceptacion_v0.1.md); revisar la descripción Q según la propuesta de la hoja dramática. Los lectores no reciben esta comparación etiquetada. No hay resultados humanos y la validez del JSON no demuestra diferenciación percibida.

## 5. Criterios para esta entrega

1. Las cuatro filas v0.4/v0.5 coinciden exactamente con sus manifiestos; únicamente tres textos cambian.
2. E.REVIVED y los otros 26 candidatos son idénticos a v0.4 y siguen sin autorización de integración.
3. Candidato simulado: 60 entradas, tres temas y 18 rutas; solo texto/intención candidatos difieren frente a canónica. El adaptador se ejecuta sobre una copia temporal, sin escribir el recurso real.
4. v0.4 e históricos, GDD v0.7, canónica/runtime, informes técnicos y muestras/clave se conservan. readme.txt añade el registro sin borrar historial.
5. PR #14 recibe commit/push y queda abierto, sin fusión. Dirección revisa y selecciona IDs antes de pedir N1 a Chat 3.

La verificación efectuada se registra en readme.txt. Sin cambios a Unity, sin ensayo humano y sin declarar cerrados los textos o completada F3A.
