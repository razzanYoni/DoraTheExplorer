using System.Collections.Generic;
using System.Linq;

namespace DoraTheExplorer.Structure;

public class Graph<T>
    where T : notnull
{
    private readonly LinkedList<Vertex<T>> _vertices;

    public Vertex<T>[] Vertices => _vertices.ToArray();

    public Graph()
    {
        _vertices = new LinkedList<Vertex<T>>();
    }

    public void AddVertex(Vertex<T> vert)
    {
        if (!_vertices.Contains(vert))
        {
            _vertices.AddLast(vert);
        }
    }

    public void ClearVertices()
    {
        _vertices.Clear();
    }
}
