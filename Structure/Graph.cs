using System.Collections.Generic;
using System.Linq;

namespace DoraTheExplorer.Structure;

public class Graph<T>
    where T : notnull
{
    public LinkedList<Vertex<T>> vertices;
    public Vertex<T>[] Vertices
    {
        get => vertices.ToArray();
    }

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
}
