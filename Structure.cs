using System;
using System.Collections.Generic;

namespace DoraTheExplorer
{
    namespace Structure
    {
        public class Vertex<T> where T : notnull
        {
            private static int id = 0;
            public readonly int Id;
            public T Info;
            public Vertex<T>? Left;
            public Vertex<T>? Top;
            public Vertex<T>? Right;
            public Vertex<T>? Down;

            public Vertex(T info)
            {
                Info = info;
                Id = id;
                id++;
            }
        }
        public class Graph<T> where T : notnull
        {
            private LinkedList<Vertex<T>> vertices;
            public Vertex<T>[] Vertices { get => GetVertices(); }

            public Graph()
            {
                vertices = new LinkedList<Vertex<T>>();
            }

            public void AddVertex(Vertex<T> vert)
            {
                if (!vertices.Contains(vert))
                {
                    vertices.AddLast(vert);
                }
            }

            public Vertex<T>[] GetVertices()
            {
                Vertex<T>[] arr = new Vertex<T>[vertices.Count];
                int i = 0;
                foreach (var v in vertices)
                {
                    arr[i] = v;
                    i++;
                }
                return arr;
            }
        }
    }
}