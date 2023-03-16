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
            var visited = new bool[vertices.Length];
            var start = vertices.Where(e => e.Info.Equals(startVertexInfo)).FirstOrDefault(vertices[0]);
            BFSImpl<T>(graph, visited, start);
        }

        private static void BFSImpl<T>(Graph<T> graph, bool[] visited, Vertex<T> start) where T : notnull
        {
            var q = new Queue<Vertex<T>>();
            q.Enqueue(start);
            visited[start.Id] = true;
            while (q.Count != 0)
            {
                Vertex<T> v = q.Dequeue();
                Console.Write(v.Info + " ");

                if (v.Up is not null && !visited[v.Up.Id])
                {
                    q.Enqueue(v.Up);
                    visited[v.Up.Id] = true;
                }
                if (v.Right is not null && !visited[v.Right.Id])
                {
                    q.Enqueue(v.Right);
                    visited[v.Right.Id] = true;
                }
                if (v.Down is not null && !visited[v.Down.Id])
                {
                    q.Enqueue(v.Down);
                    visited[v.Down.Id] = true;
                }
                if (v.Left is not null && !visited[v.Left.Id])
                {
                    q.Enqueue(v.Left);
                    visited[v.Left.Id] = true;
                }
            }
        }
    }
}