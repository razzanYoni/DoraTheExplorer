using System;
using System.Collections.Generic;
using DoraTheExplorer.Structure;
using System.Linq;

namespace DoraTheExplorer
{
    public class Solver
    {
        public static void BFS<T>(Graph<T> graph, T startVertexInfo) where T : notnull, IEquatable<T>
        {
            var vertices = graph.Vertices;
            if (vertices.Length == 0) return;
            var visited = new HashSet<int>();
            var start = vertices.Where(e => e.Info.Equals(startVertexInfo)).FirstOrDefault(vertices[0]);
            BFSImpl<T>(graph, visited, start);
        }

        private static void BFSImpl<T>(Graph<T> graph, HashSet<int> visited, Vertex<T> start) where T : notnull
        {
            var q = new Queue<Vertex<T>>();
            q.Enqueue(start);
            visited.Add(start.Id);
            while (q.Count > 0)
            {
                Vertex<T> v = q.Dequeue();
                Console.Write(v.Info + " ");

                if (v.Up is not null && !visited.Contains(v.Up.Id))
                {
                    q.Enqueue(v.Up);
                    visited.Add(v.Up.Id);
                }
                if (v.Right is not null && !visited.Contains(v.Right.Id))
                {
                    q.Enqueue(v.Right);
                    visited.Add(v.Right.Id);
                }
                if (v.Down is not null && !visited.Contains(v.Down.Id))
                {
                    q.Enqueue(v.Down);
                    visited.Add(v.Down.Id);
                }
                if (v.Left is not null && !visited.Contains(v.Left.Id))
                {
                    q.Enqueue(v.Left);
                    visited.Add(v.Left.Id);
                }
            }
        }

        public static void DFS<T>(Graph<T> graph, T startVertexInfo) where T : notnull, IEquatable<T>
        {
            var vertices = graph.Vertices;
            if (vertices.Length == 0) return;
            var visited = new HashSet<int>();
            var start = vertices.Where(e => e.Info.Equals(startVertexInfo)).FirstOrDefault(vertices[0]);
            DFSImpl<T>(graph, visited, start);
        }

        private static void DFSImpl<T>(Graph<T> graph, HashSet<int> visited, Vertex<T> start) where T : notnull
        {
            visited.Add(start.Id);
            Console.Write(start.Info + " ");
            if (start.Up is not null && !visited.Contains(start.Up.Id))
            {
                DFSImpl<T>(graph, visited, start.Up);
            }
            if (start.Right is not null && !visited.Contains(start.Right.Id))
            {
                DFSImpl<T>(graph, visited, start.Right);
            }
            if (start.Down is not null && !visited.Contains(start.Down.Id))
            {
                DFSImpl<T>(graph, visited, start.Down);
            }
            if (start.Left is not null && !visited.Contains(start.Left.Id))
            {
                DFSImpl<T>(graph, visited, start.Left);
            }
        }
    }
}