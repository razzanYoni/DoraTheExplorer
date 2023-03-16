using System;
using System.Collections.Generic;
using DoraTheExplorer.Structure;

namespace DoraTheExplorer
{
    public class Solver
    {
        public static void BFS<T>(Graph<T> graph, T startVertexInfo) where T : notnull, IEquatable<T>
        {
            var vertices = graph.Vertices;
            bool[] visited = new bool[vertices.Length];
            Vertex<T> start = vertices[0];
            foreach (var v in vertices)
            {
                if (v.Info.Equals(startVertexInfo))
                {
                    start = v;
                    break;
                }
            }
            BFSImpl<T>(graph, visited, start);
        }

        private static void BFSImpl<T>(Graph<T> graph, bool[] visited, Vertex<T> start) where T : notnull
        {
            Queue<Vertex<T>> q = new Queue<Vertex<T>>();
            q.Enqueue(start);
            visited[start.Id] = true;
            while (q.Count != 0)
            {
                Vertex<T> v = q.Dequeue();
                Console.Write(v.Info + " ");

                if (v.Top != null && !visited[v.Top.Id])
                {
                    q.Enqueue(v.Top);
                    visited[v.Top.Id] = true;
                }
                if (v.Right != null && !visited[v.Right.Id])
                {
                    q.Enqueue(v.Right);
                    visited[v.Right.Id] = true;
                }
                if (v.Down != null && !visited[v.Down.Id])
                {
                    q.Enqueue(v.Down);
                    visited[v.Down.Id] = true;
                }
                if (v.Left != null && !visited[v.Left.Id])
                {
                    q.Enqueue(v.Left);
                    visited[v.Left.Id] = true;
                }
            }
        }
    }
}