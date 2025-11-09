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
            foreach (var kvp in g.AdjacencyList)
            {
                string from = g.Vertices[kvp.Key].Name;
                string toList = string.Join(", ", kvp.Value.Select(id => g.Vertices[id].Name));
                Console.WriteLine($"{from} → [{toList}]");
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
            foreach (var v in users)
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