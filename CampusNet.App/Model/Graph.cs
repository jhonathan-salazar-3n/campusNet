namespace CampusNet.Models
{
    public class Graph
    {
        private readonly Dictionary<string, Vertex> _vertices;
        private readonly Dictionary<string, List<string>> _adj;

        public Graph()
        {
            _vertices = new Dictionary<string, Vertex>();
            _adj = new Dictionary<string, List<string>>();
        }

        // Vértices
        public bool AddVertex(Vertex v)
        {
            if (v == null || _vertices.ContainsKey(v.Id)) return false;
            _vertices[v.Id] = v;
            _adj[v.Id] = new List<string>();
            return true;
        }

        public bool RemoveVertex(string id)
        {
            if (!_vertices.ContainsKey(id)) return false;
            _adj.Remove(id);

            foreach (var key in _adj.Keys.ToList())
            {
                _adj[key].RemoveAll(x => x == id);
            }

            _vertices.Remove(id);
            return true;
        }

        public bool UpdateVertex(string id, string? newName = null, Role? newRole = null)
        {
            if (!_vertices.TryGetValue(id, out var v)) return false;
            if (!string.IsNullOrWhiteSpace(newName)) v.Name = newName!;
            if (newRole.HasValue) v.Role = newRole.Value;
            return true;
        }

        // Aristas
        public bool AddEdge(string fromId, string toId)
        {
            if (fromId == toId) return false;
            if (!_vertices.ContainsKey(fromId) || !_vertices.ContainsKey(toId)) return false;

            var list = _adj[fromId];
            if (list.Contains(toId)) return false;
            list.Add(toId);
            return true;
        }

        public bool RemoveEdge(string fromId, string toId)
        {
            if (!_adj.ContainsKey(fromId)) return false;
            return _adj[fromId].Remove(toId);
        }

        // Consultas
        public IReadOnlyDictionary<string, List<string>> Adjacency => _adj;

        public IEnumerable<Vertex> GetAllVertices() => _vertices.Values;

        public int OutDegree(string id) => _adj.ContainsKey(id) ? _adj[id].Count : 0;

        public int InDegree(string id)
        {
            if (!_vertices.ContainsKey(id)) return -1;
            return _adj.Values.Count(list => list.Contains(id));
        }

        public List<string> GetUsersWithoutFollowers()
        {
            return _vertices.Keys.Where(id => InDegree(id) == 0).ToList();
        }

        public List<string> GetMostInfluential(int top = 3)
        {
            return _vertices.Keys
                .OrderByDescending(id => InDegree(id))
                .ThenBy(id => id)
                .Take(top)
                .ToList();
        }

        public List<string> GetMostActive(int top = 3)
        {
            return _vertices.Keys
                .OrderByDescending(id => OutDegree(id))
                .ThenBy(id => id)
                .Take(top)
                .ToList();
        }

        public (List<string> order, int reached) BFS(string origin)
        {
            var order = new List<string>();
            if (!_vertices.ContainsKey(origin)) return (order, 0);

            var visited = new HashSet<string>();
            var q = new Queue<string>();
            visited.Add(origin);
            q.Enqueue(origin);

            while (q.Count > 0)
            {
                var u = q.Dequeue();
                order.Add(u);
                foreach (var v in _adj[u].Where(v => !visited.Contains(v)))
                {
                    visited.Add(v);
                    q.Enqueue(v);
                }
            }

            return (order, order.Count);
        }

        public (List<string> discoveryOrder, List<(string, string)> backEdges) DFSFull()
        {
            var discovered = new HashSet<string>();
            var finished = new HashSet<string>();
            var order = new List<string>();
            var backEdges = new List<(string, string)>();

            void DfsVisit(string u)
            {
                discovered.Add(u);
                order.Add(u);
                foreach (var v in _adj[u])
                {
                    if (!discovered.Contains(v))
                    {
                        DfsVisit(v);
                    }
                    else if (!finished.Contains(v))
                    {
                        backEdges.Add((u, v));
                    }
                }
                finished.Add(u);
            }

            foreach (var id in _vertices.Keys.Where(id => !discovered.Contains(id)))
            {
                DfsVisit(id);
            }

            return (order, backEdges);
        }

        public bool IsReachable(string from, string to)
        {
            if (!_vertices.ContainsKey(from) || !_vertices.ContainsKey(to)) return false;
            var (order, _) = BFS(from);
            return order.Contains(to);
        }

        public Vertex? GetVertex(string id)
        {
            if (_vertices.TryGetValue(id, out var v))
                return v;
            return null;
        }


        public List<(string Id, int Grado)> GetMostInfluentialWithDegree(int top = 3)
        {
            return _vertices.Keys
                .Select(id => (Id: id, Grado: InDegree(id)))
                .OrderByDescending(x => x.Grado)
                .ThenBy(x => x.Id)
                .Take(top)
                .ToList();
        }

        public List<(string Id, int Grado)> GetMostActiveWithDegree(int top = 3)
        {
            return _vertices.Keys
                .Select(id => (Id: id, Grado: OutDegree(id)))
                .OrderByDescending(x => x.Grado)
                .ThenBy(x => x.Id)
                .Take(top)
                .ToList();
        }

    }
}
