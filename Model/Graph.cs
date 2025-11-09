using System;
using System.Collections.Generic;
using System.Linq;

namespace CampusNet.Model
{
    public class Graph
    {
        public Dictionary<string, Vertex> Vertices { get; set; }
        public Dictionary<string, List<string>> AdjacencyList { get; set; }

        public Graph()
        {
            Vertices = new Dictionary<string, Vertex>();
            AdjacencyList = new Dictionary<string, List<string>>();
        }

        public void AddVertex(Vertex v)
        {
            if (!Vertices.ContainsKey(v.Id))
            {
                Vertices[v.Id] = v;
                AdjacencyList[v.Id] = new List<string>();
            }
        }

        public void RemoveVertex(string id)
        {
            if (Vertices.ContainsKey(id))
            {
                Vertices.Remove(id);
                AdjacencyList.Remove(id);
                foreach (var list in AdjacencyList.Values)
                    list.Remove(id);
            }
        }

        public void AddEdge(string from, string to)
        {
            if (Vertices.ContainsKey(from) && Vertices.ContainsKey(to))
            {
                if (!AdjacencyList[from].Contains(to))
                    AdjacencyList[from].Add(to);
            }
        }

        public void RemoveEdge(string from, string to)
        {
            if (AdjacencyList.ContainsKey(from))
                AdjacencyList[from].Remove(to);
        }

        public List<string> BFS(string start)
        {
            var order = new List<string>();
            if (!Vertices.ContainsKey(start) || !AdjacencyList.ContainsKey(start))
                return order;

            var visited = new HashSet<string>();
            var queue = new Queue<string>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                if (visited.Contains(current)) continue;

                visited.Add(current);
                order.Add(current);

                if (!AdjacencyList.TryGetValue(current, out var neighbors))
                    continue;

                foreach (var neighbor in neighbors)
                {
                    if (!visited.Contains(neighbor))
                        queue.Enqueue(neighbor);
                }
            }

            return order;
        }

        public void DFS(string start, HashSet<string> visited)
        {
            visited.Add(start);
            if (!AdjacencyList.TryGetValue(start, out var neighbors))
                return;
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                    DFS(neighbor, visited);
            }
        }

        public bool HasCycle()
        {
            var visited = new HashSet<string>();
            var stack = new HashSet<string>();

            foreach (var v in Vertices.Keys)
                if (DetectCycle(v, visited, stack)) return true;

            return false;
        }

        private bool DetectCycle(string v, HashSet<string> visited, HashSet<string> stack)
        {
            if (stack.Contains(v)) return true;
            if (visited.Contains(v)) return false;

            visited.Add(v);
            stack.Add(v);

            if (AdjacencyList.TryGetValue(v, out var neighbors))
            {
                foreach (var neighbor in neighbors)
                    if (DetectCycle(neighbor, visited, stack))
                        return true;
            }

            stack.Remove(v);
            return false;
        }

        public List<Vertex> GetWithoutFollowers()
        {
            var followers = AdjacencyList.Values.SelectMany(x => x).ToHashSet();
            return Vertices.Values.Where(v => !followers.Contains(v.Id)).ToList();
        }

        public Vertex GetMostInfluential()
        {
            var inDegree = Vertices.ToDictionary(v => v.Key, v => 0);
            foreach (var list in AdjacencyList.Values)
                foreach (var to in list)
                    if (inDegree.ContainsKey(to)) inDegree[to]++;

            int max = inDegree.Values.Max();
            return Vertices[inDegree.First(x => x.Value == max).Key];
        }

        public Vertex GetMostActive()
        {
            int max = AdjacencyList.Values.Max(l => l.Count);
            var id = AdjacencyList.First(x => x.Value.Count == max).Key;
            return Vertices[id];
        }

        public bool CanReach(string from, string to)
        {
            return BFS(from).Contains(to);
        }

        // MÉTRICAS DEL MODELO
        public int EdgeCount()
        {
            return AdjacencyList.Values.Sum(l => l.Count);
        }

        public double AverageOutDegree()
        {
            if (Vertices.Count == 0) return 0.0;
            return AdjacencyList.Values.Average(l => l.Count);
        }

        public double AverageInDegree()
        {
            if (Vertices.Count == 0) return 0.0;
            var inDeg = BuildInDegreeMap();
            return inDeg.Values.Average();
        }

        private Dictionary<string,int> BuildInDegreeMap()
        {
            var inDegree = Vertices.ToDictionary(v => v.Key, v => 0);
            foreach (var list in AdjacencyList.Values)
                foreach (var to in list)
                    if (inDegree.ContainsKey(to)) inDegree[to]++;
            return inDegree;
        }

        // Retorna el orden de descubrimiento de un DFS desde un inicio específico
        public List<string> DFSOrderFrom(string start)
        {
            var order = new List<string>();
            if (!Vertices.ContainsKey(start)) return order;

            var visited = new HashSet<string>();
            DFSOrderHelper(start, visited, order);
            return order;
        }

        // Retorna el orden de descubrimiento de un DFS completo sobre todos los vértices
        public List<string> DFSAll()
        {
            var order = new List<string>();
            var visited = new HashSet<string>();

            foreach (var v in Vertices.Keys)
            {
                if (!visited.Contains(v))
                    DFSOrderHelper(v, visited, order);
            }

            return order;
        }

        private void DFSOrderHelper(string v, HashSet<string> visited, List<string> order)
        {
            visited.Add(v);
            order.Add(v);

            if (!AdjacencyList.TryGetValue(v, out var neighbors))
                return;

            foreach (var n in neighbors)
                if (!visited.Contains(n))
                    DFSOrderHelper(n, visited, order);
        }
    }
}