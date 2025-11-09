# CampusNet - Taller Integrador (Grafos Dirigidos) - Programación III

## Resumen

CampusNet es una simulación de una red social académica (grafo dirigido) desarrollada en C# como consola (.NET 6+). Arquitectura MVC estricta:

**Modelo-Vista-Controlador (MVC)** en **C# (.NET 6+)**.  

- Models: Vertex, Edge, Graph (sin impresión)
- Views: ConsoleView (encargada de salida en consola)
- Controllers: CampusController (casos de uso)

## Requisitos cumplidos
- Grafo dirigido con ≥12 vértices y ≥18 aristas.
- 3 BFS desde usuarios distintos.
- 1 DFS completo con detección de back-edges (posibles ciclos).
- Consultas: usuarios sin seguidores, usuarios influyentes, usuarios activos.
- Ciclo dirigido incluido (U3→U4→U6→U3).
- Al menos 2 nodos con in-degree 0 (U11, U12).
- 3 operaciones CRUD demostradas.
- `Program.cs` sólo invoca al Controller.
- No hay lógica de negocio en la View ni impresión dentro del Model.

## Estructura de repositorio

(ver estructura de carpetas en la raíz).

## Integrantes

- Jhonathan Salazar Munnoz - Arquitecto de soluciones y desarrollador

## Requisitos de compilación y ejecución

1. Tener instalado **.NET SDK 6.0 o superior**.
2. Clonar el repositorio o copiar los archivos.
3. Abrir la carpeta del proyecto en terminal y ejecutar:

```bash
cd src/CampusNet.App
dotnet build
dotnet run
```

## Ejemplo salida

Usuarios más activos (top 3 por grado salida):
Camila Hernández (Profesor) – 5 seguidos
Santiago López (Profesor) – 5 seguidos
Jhonathan Salazar (Estudiante) – 1 seguidos

¿Camila Hernández puede llegar a Valentina García?: Sí

==== Operaciones CRUD demostradas ====
Usuario U13 agregado.
Relaciones de U13 agregadas.
Usuario U1 actualizado.
Arista U2->U6 eliminada.