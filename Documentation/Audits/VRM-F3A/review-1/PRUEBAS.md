# Revisión PR #10 — Reproducción y recorrido manual

La [guía del directorio padre](../PRUEBAS.md) conserva los comandos Unity/Windows. Usar datos privados **nuevos** y esta revisión; no mezclar resultados con el prototipo de 13 entradas. El runner sigue siendo `Isekaid.Editor.F3AIntegrationChecks.Run` y el ejecutable usa `-isekF3ATest full` con `-isekTestData`, `-isekVrmFixture` y `-isekCaptureDir` privados. En Editor no añadir `-quit` al recorrido Play Mode.

## Datos canónicos

Desde la raíz del repositorio:

```powershell
python tools/build_f3a_runtime_content.py
python tools/build_f3a_runtime_content.py --check
python tools/verify_unified_layout.py
python tools/verify_vrm_f2_delivery.py
```

El primer comando regenera únicamente el recurso runtime de diálogo; no cambia la biblioteca canónica. El segundo valida exactitud y forma, incluidos efectos cero. CI ejecuta --check y la estructura. No editar a mano los textos del recurso generado.

## Resultado esperado

- `f3a-results.json`: passed=true, 285 comprobaciones y routes con 18 opciones distintas.
- Fixture de entrada a 60 FPS, con pulsos que cruzan frames y timeout. Se conserva una aserción de desplazamiento >0.15 m después del cierre, sin modificar el jugador.
- Nodo/elección/episodio, confianza, consumo y expresión se registran en presentations. Los ensayos de peligro son señales explícitas rotuladas, no enemigos.
- Regresión F2: Editor 43, Windows 36, reinicio sin archivo fuente 2; metadatos 35; guardas 9+2 y almacenamiento 11. Reutilizar los comandos del informe F2; conservar results.json antes del reinicio.
- Para pruebas de biblioteca alternativa, cada una de las 18 rutas reinicia el encuentro; no concatenar dos hojas incompatibles de un árbol.

## Recorrido manual pendiente de Dirección/usuario

1. Abrir la build sin argumentos de ensayo. Elegir Adventurer y personalidad; entrar a Game y rescatar con E.
2. Conversar con E, elegir TRAP/FEEL/ROAD y leer sus dos respuestas. Escoger una; comprobar su hoja completa y «Volver a temas»/«Terminar conversación».
3. Abrir una raíz, enfocar la segunda opción y cerrar **antes** de contestar. Moverse/mirar y volver a conversar: misma raíz como historial y opciones/foco pendientes, sin otra emisión de la raíz.
4. Con esa raíz pendiente, Tab → creador. Cambiar solo apariencia y volver. Reanudar Game: mismo cursor, opciones y estado. Repetir VRM→catálogo. Las capturas VRM permanecen privadas.
5. Incapacitar desde el menú con charla en curso. Comprobar ayuda inmediata, movimiento libre para reanimar y reloj detenido al pausar. Reanimar, pedir hablar y continuar F.RESUME hasta las opciones guardadas.
6. Completar/cerrar la charla y repetir otra incapacidad/reanimación. Comprobar una nueva ayuda/gratitud y retorno de ese episodio, con un solo primer rescate/confianza inicial. Una charla cerrada voluntariamente no se abre por sí sola.
7. Cambiar seguir/esperar y repetir la misma orden por la API de ensayo: no repetir aceptación del no-op. Las opiniones de espera no impiden esperar.
8. Probar «Charlar un momento» sin agotar obligatoriamente los temas; después comprobar tres variantes y su intervalo compartido de 20 segundos activos. Pausa/configuración no cuentan. Al agotar, solo estado UI sin frase NPC nueva.
9. Comparar la misma elección en tres encuentros nuevos con distintas personalidades y el mismo avatar. Revisar vocabulario, ritmo y postura por separado; no aprobarlo solo por un morph.
10. Verificar teclado/ratón, captura física del cursor en ventana enfocada y legibilidad en 1920×1080, 1600×900 y 1280×720. Comprobar muerte/reinicio y que el creador siga operativo.

No se simula prueba manual aprobada por pasar la automatización. Registrar pasos, resolución, personalidad, apariencia y evidencia autorizada. El caso extremo de miles de FPS fuera de foco y minMoveDistance del prefab original queda documentado en el informe; no se corrigió como parte de F3A.
