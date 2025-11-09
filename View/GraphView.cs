using System;
using CampusNet.Model;
using System.Collections.Generic;
using System.Linq;

namespace CampusNet.View
{
    public class GraphView
    {
        public void ShowAdjacencyList(Graph g)
        {
            Console.WriteLine("\n=== LISTA DE ADYACENCIA ===");
            if (g.Vertices.Count == 0)
            {
                Console.WriteLine("(vacío)");
                return;
            }

            int nameWidth = g.Vertices.Values.Max(v => v.Name.Length);

            var orderedKeys = g.AdjacencyList
                .Keys
                .OrderBy(id => g.Vertices.ContainsKey(id) ? g.Vertices[id].Name : id)
                .ToList();

            foreach (var key in orderedKeys)
            {
                string from = g.Vertices.ContainsKey(key) ? g.Vertices[key].Name : key;
                var neighbors = g.AdjacencyList.TryGetValue(key, out var list) ? list : new List<string>();
                var toNames = neighbors
                    .Where(id => g.Vertices.ContainsKey(id))
                    .Select(id => g.Vertices[id].Name)
                    .OrderBy(n => n)
                    .ToList();

                string toList = string.Join(", ", toNames);
                Console.WriteLine($"{from.PadRight(nameWidth)} → [" + toList + $"] (siguiendo {toNames.Count})");
            }
        }

        public void ShowTraversal(string method, List<string> order, Graph g)
        {
            Console.WriteLine($"\nRecorrido {method}:");
            foreach (var id in order)
                Console.Write($"{g.Vertices[id].Name} -> ");
            Console.WriteLine($"\nTotal visitados: {order.Count}\n");
        }

        public void ShowDFS(string title, List<string> order, Graph g)
        {
            Console.WriteLine($"\nRecorrido {title}:");
            foreach (var id in order)
                Console.Write($"{g.Vertices[id].Name} -> ");
            Console.WriteLine($"\nTotal visitados (DFS): {order.Count}\n");
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowCycleResult(bool hasCycle)
        {
            Console.WriteLine($"¿El grafo tiene ciclo? {(hasCycle ? "Sí" : "No")}");
        }

        public void ShowUsersList(string title, IEnumerable<Vertex> users)
        {
            Console.WriteLine($"\n{title}:");
            foreach (var v in users.OrderBy(u => u.Name))
                Console.WriteLine($"- {v}");
        }

        public void ShowSingle(string title, Vertex v)
        {
            Console.WriteLine($"\n{title}: {v}");
        }

        public void ShowReachability(string fromLabel, string toLabel, bool canReach)
        {
            Console.WriteLine($"\n¿Puede {fromLabel} llegar a {toLabel}? {(canReach ? "Sí" : "No")}");
        }
    }
}