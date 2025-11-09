using CampusNet.Models;

namespace CampusNet.Views
{
    public static class ConsoleView
    {
        public static void PrintHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine("==== " + title + " ====");
        }

        public static void PrintAdjacency(Graph graph)
        {
             Console.WriteLine("\n==== LISTA DE ADYACENCIA ====");

            var vertices = graph.GetAllVertices().ToDictionary(v => v.Id, v => v);

            foreach (var v in vertices.Values)
            {
                if (!graph.Adjacency.ContainsKey(v.Id) || graph.Adjacency[v.Id].Count == 0)
                {
                    Console.WriteLine($"{v.Id} ({v.Name}, {v.Role}): (sin conexiones)");
                }
                else
                {
                    var destinos = graph.Adjacency[v.Id]
                        .Select(id => $"{id} ({vertices[id].Name})");
                    Console.WriteLine($"{v.Id} ({v.Name}, {v.Role}): {string.Join(", ", destinos)}");
                }
            }
        }

        public static void PrintVertices(IEnumerable<Vertex> vertices)
        {
            foreach (var v in vertices) Console.WriteLine(v);
        }

        public static void PrintTraversal(string origin, List<string> order)
        {
            Console.WriteLine($"BFS desde {origin} -> orden: {string.Join(" -> ", order)} (alcanzados: {order.Count})");
        }

        public static void PrintDFS(List<string> discoveryOrder, List<(string, string)> backEdges)
        {
            Console.WriteLine($"DFS orden de descubrimiento: {string.Join(" -> ", discoveryOrder)}");
            if (backEdges.Any())
            {
                Console.WriteLine("Posibles ciclos detectados por back-edges:");
                foreach (var e in backEdges) Console.WriteLine($"{e.Item1} -> {e.Item2}");
            }
            else
            {
                Console.WriteLine("No se detectaron ciclos (no se encontraron back-edges).");
            }
        }

        public static void PrintList(string title, IEnumerable<string> items)
        {
            Console.WriteLine($"{title}: {string.Join(", ", items)}");
        }

        public static void PrintOperationResult(bool ok, string successMsg, string failMsg = "Operación fallida")
        {
            Console.WriteLine(ok ? successMsg : failMsg);
        }
    }
}
