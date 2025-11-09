using CampusNet.Models;
using CampusNet.Views;

namespace CampusNet.Controllers
{
    public class CampusController
    {
        private readonly Graph _graph;

        public CampusController(Graph graph)
        {
            _graph = graph;
        }

        public void Run()
        {
            BuildInitialGraph();

            ConsoleView.PrintAdjacency(_graph);

            DoTraversals();

            DoSocialQueries();

            DoCRUDOperations();

            ConsoleView.PrintAdjacency(_graph);
        }

        private void BuildInitialGraph()
        {
            ConsoleView.PrintHeader("Construcción del grafo inicial");

            var users = new List<Vertex>()
                {
                    new Vertex("U1","Jhonathan Salazar", Role.Estudiante),
                    new Vertex("U2","Camila Hernández", Role.Profesor),
                    new Vertex("U3","Felipe Torres", Role.Egresado),
                    new Vertex("U4","Daniela Ospina", Role.Estudiante),
                    new Vertex("U5","Santiago López", Role.Profesor),
                    new Vertex("U6","Valentina García", Role.Egresado),
                    new Vertex("U7","Andrés Pérez", Role.Estudiante),
                    new Vertex("U8","Mariana Gómez", Role.Egresado),
                    new Vertex("U9","Laura Martínez", Role.Profesor),
                    new Vertex("U10","David Rincón", Role.Estudiante),
                    new Vertex("U11","Sara Mejía", Role.Egresado),
                    new Vertex("U12","Mateo Cárdenas", Role.Estudiante)
                };

            foreach (var u in users) _graph.AddVertex(u);
            var edges = new List<(string from, string to)>
                {
                    // Camila (U2, profesora) muy activa
                    ("U2","U1"),
                    ("U2","U3"),
                    ("U2","U4"),
                    ("U2","U5"),
                    ("U2","U6"),

                    // Santiago (U5, profesor) también muy activo
                    ("U5","U1"),
                    ("U5","U4"),
                    ("U5","U7"),
                    ("U5","U8"),
                    ("U5","U9"),

                    // Ciclo dirigido entre Felipe, Daniela y Valentina
                    ("U3","U4"),
                    ("U4","U6"),
                    ("U6","U3"),

                    // Conexiones adicionales
                    ("U1","U7"),
                    ("U7","U10"),
                    ("U8","U9"),
                    ("U9","U1"),
                    ("U10","U1")
                    // U11 y U12 permanecen sin seguidores (in-degree 0)
                };

            int added = 0;
            foreach (var e in edges.Where(e => _graph.AddEdge(e.from, e.to)))
            {
                added++;
            }
            ConsoleView.PrintList("Aristas añadidas (count)", new[] { added.ToString() });
        }


        private void DoTraversals()
        {
            ConsoleView.PrintHeader("Recorridos - 3 BFS y 1 DFS completo");

            var bfsStarts = new[] { "U2", "U5", "U1" };
            foreach (var s in bfsStarts)
            {
                var (order, _) = _graph.BFS(s);
                ConsoleView.PrintTraversal(s, order);
            }

            var (discoveryOrder, backEdges) = _graph.DFSFull();
            ConsoleView.PrintDFS(discoveryOrder, backEdges);
        }

        private void DoSocialQueries()
        {
            ConsoleView.PrintHeader("Consultas sociales");

            var noFollowers = _graph.GetUsersWithoutFollowers();
            ConsoleView.PrintList("Usuarios sin seguidores (grado entrada 0)", noFollowers);

            // Top 3 Influencers con grado
            var influencers = _graph.GetMostInfluentialWithDegree(3);
            Console.WriteLine("Usuarios influyentes (top 3 por grado entrada):");
            foreach (var (id, grado) in influencers)
            {
                var user = _graph.GetVertex(id);
                if (user != null)
                {
                    Console.WriteLine($"{user.Name} ({user.Role}) – {grado} seguidores");
                }
            }
            Console.WriteLine();

            // Top 3 Activos con grado
            var active = _graph.GetMostActiveWithDegree(3);
            Console.WriteLine("Usuarios más activos (top 3 por grado salida):");
            foreach (var (id, grado) in active)
            {
                var user = _graph.GetVertex(id);
                if (user != null)
                {
                    Console.WriteLine($"{user.Name} ({user.Role}) – {grado} seguidos");
                }
            }
            Console.WriteLine();

            // Alcanzabilidad ejemplo: U2 -> U6 ?
            string a = "U2", b = "U6";
            bool can = _graph.IsReachable(a, b);
            var userA = _graph.GetVertex(a);
            var userB = _graph.GetVertex(b);
            if (userA != null && userB != null)
            {
                Console.WriteLine($"¿{userA.Name} puede llegar a {userB.Name}?: {(can ? "Sí" : "No")}");
            }
        }



        private void DoCRUDOperations()
        {
            ConsoleView.PrintHeader("Operaciones CRUD demostradas");

            var uNew = new Vertex("U13", "Esteban R", Role.Estudiante);
            bool added = _graph.AddVertex(uNew);
            ConsoleView.PrintOperationResult(added, $"Usuario {uNew.Id} agregado.");

            bool e1 = _graph.AddEdge("U13", "U1");
            bool e2 = _graph.AddEdge("U13", "U2");
            ConsoleView.PrintOperationResult(e1 && e2, "Relaciones de U13 agregadas.");

            bool updated = _graph.UpdateVertex("U1", newName: "Ana P. Gomez", newRole: Role.Egresado);
            ConsoleView.PrintOperationResult(updated, "Usuario U1 actualizado.");

            bool removedEdge = _graph.RemoveEdge("U2", "U6");
            ConsoleView.PrintOperationResult(removedEdge, "Arista U2->U6 eliminada.");

            bool removed = _graph.RemoveVertex("U12");
            ConsoleView.PrintOperationResult(removed, "Usuario U12 eliminado.");
        }
    }
}
