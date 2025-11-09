using System;
using CampusNet.Model;
using CampusNet.View;
using System.Collections.Generic;
using System.Linq;

namespace CampusNet.Controller
{
    public class GraphController
    {
        private Graph graph;
        private GraphView view;

        public GraphController()
        {
            graph = new Graph();
            view = new GraphView();
        }

        public void Run()
        {
            view.ShowMessage("=== Controller: Orquestación de casos de uso ===");
            CrearUsuarios();
            CrearRelaciones();
            MostrarResumen();
            view.ShowAdjacencyList(graph);
            EjecutarRecorridos();
            ConsultasSociales();
            OperacionesCRUD();
        }

        private void MostrarResumen()
        {
            int totalUsuarios = graph.Vertices.Count;
            int totalRelaciones = graph.AdjacencyList.Values.Sum(l => l.Count);
            view.ShowMessage($"Usuarios: {totalUsuarios} | Relaciones: {totalRelaciones}");
        }

        private void CrearUsuarios()
        {
            var usuarios = new List<Vertex>
            {
                new Vertex("U1","Ana","Estudiante"),
                new Vertex("U2","Luis","Profesor"),
                new Vertex("U3","Sofía","Egresado"),
                new Vertex("U4","Carlos","Estudiante"),
                new Vertex("U5","Laura","Profesor"),
                new Vertex("U6","Mario","Egresado"),
                new Vertex("U7","Diana","Estudiante"),
                new Vertex("U8","Pedro","Profesor"),
                new Vertex("U9","Lucía","Egresado"),
                new Vertex("U10","Jorge","Estudiante"),
                new Vertex("U11","Marta","Profesor"),
                new Vertex("U12","Andrés","Egresado")
            };

            usuarios.ForEach(u => graph.AddVertex(u));
        }

        private void CrearRelaciones()
        {
            var relaciones = new List<(string, string)>
            {
                ("U1","U2"),("U1","U3"),("U1","U4"),("U1","U5"),
                ("U2","U3"),("U2","U6"),("U2","U7"),("U2","U8"),
                ("U3","U1"),("U3","U9"),("U3","U12"),
                ("U4","U2"),("U5","U3"),("U6","U2"),
                ("U7","U8"),("U8","U9"),
                ("U9","U3"),("U9","U1")
            };
            relaciones.ForEach(r => graph.AddEdge(r.Item1, r.Item2));
        }

        private void EjecutarRecorridos()
        {
            var bfs1 = graph.BFS("U1");
            view.ShowTraversal("BFS desde Ana", bfs1, graph);

            var bfs2 = graph.BFS("U2");
            view.ShowTraversal("BFS desde Luis", bfs2, graph);

            var bfs3 = graph.BFS("U3");
            view.ShowTraversal("BFS desde Sofía", bfs3, graph);

            view.ShowCycleResult(graph.HasCycle());

            var dfsAll = graph.DFSAll();
            view.ShowDFS("DFS completo", dfsAll, graph);
        }

        private void ConsultasSociales()
        {
            view.ShowUsersList("Usuarios sin seguidores", graph.GetWithoutFollowers());
            view.ShowSingle("Usuario más influyente", graph.GetMostInfluential());
            view.ShowSingle("Usuario más activo", graph.GetMostActive());
            view.ShowReachability("Ana", "Mario", graph.CanReach("U1","U6"));

            view.ShowUsersList("Profesores", graph.Vertices.Values.Where(v => v.Role == "Profesor"));
            view.ShowUsersList("Estudiantes", graph.Vertices.Values.Where(v => v.Role == "Estudiante"));
            view.ShowUsersList("Egresados", graph.Vertices.Values.Where(v => v.Role == "Egresado"));
        }

        private void OperacionesCRUD()
        {
            view.ShowMessage("\n=== CRUD ===");
            graph.AddVertex(new Vertex("U13","Valeria","Estudiante"));
            graph.AddEdge("U13","U1");
            view.ShowMessage("Agregado U13 y relación con Ana.");
            view.ShowAdjacencyList(graph);

            graph.RemoveEdge("U1","U2");
            view.ShowMessage("Eliminada relación Ana → Luis.");
            view.ShowAdjacencyList(graph);

            graph.Vertices["U3"].Name = "Sofía Gómez";
            view.ShowMessage("Actualizado nombre de U3.");
            view.ShowAdjacencyList(graph);

            graph.RemoveVertex("U12");
            view.ShowMessage("Eliminado U12.");
            view.ShowAdjacencyList(graph);

            graph.Vertices["U1"].Role = "Administrador";
            view.ShowMessage("Actualizado rol de U1 a Administrador.");
            view.ShowUsersList("Usuarios con rol Administrador", graph.Vertices.Values.Where(v => v.Role == "Administrador"));
        }
    }
}