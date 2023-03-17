using System;
using System.Collections.Generic;
using DoraTheExplorer.Structure;
using System.Linq;

namespace DoraTheExplorer.Algorithm;

public class DFSSolver
{
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