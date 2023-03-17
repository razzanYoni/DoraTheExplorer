using System;
using System.Collections.Generic;
using DoraTheExplorer.Structure;
using System.Linq;

namespace DoraTheExplorer.Algorithm;

public class DFSSolver
{
    public static void DFS<T>(Graph<T> graph, T startVertexInfo)
        where T : notnull, IEquatable<T>
    {
        var vertices = graph.Vertices;
        if (vertices.Length == 0)
            return;
        var visited = new HashSet<int>();
        var start = vertices.Where(e => e.Info.Equals(startVertexInfo)).FirstOrDefault(vertices[0]);
        DFSImpl<T>(graph, visited, start);
    }

    private static void DFSImpl<T>(Graph<T> graph, HashSet<int> visited, Vertex<T> start)
        where T : notnull
    {
        visited.Add(start.Id);
        Console.Write(start.Info + " ");
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
        if (start.Up is not null && !visited.Contains(start.Up.Id))
        {
            DFSImpl<T>(graph, visited, start.Up);
        }
    }

    public static (Queue<Coordinate>?, List<State>) FindPath(Graph<Coordinate> graph, Coordinate start, Coordinate goal)
    {
        var states = new List<State>();
        var state = new State(start, 1);
        var q = new Stack<Vertex<Coordinate>>();
        var v = graph.Vertices.Where(e => e.Equals(start)).FirstOrDefault(graph.Vertices[0]);
        var track = new Stack<Queue<Coordinate>>();
        var t = new Queue<Coordinate>();
        var path = new Queue<Coordinate>();
        q.Push(v);
        t.Enqueue(v.Info);
        track.Push(new Queue<Coordinate>(t));
        state.AddVisitedLocation(start);
        states.Add(new State(state));
        while (q.Count > 0)
        {
            v = q.Pop();
            t = track.Pop();
            if (v.Info.Equals(goal))
            {
                path = t;
                break;
            }
            if (v.Right is not null && !state.VisitedLocations.Contains(v.Right.Info))
            {
                var curTrack = new Queue<Coordinate>(t);
                q.Push(v.Right);
                curTrack.Enqueue(v.Right.Info);
                track.Push(curTrack);
                state.AddVisitedLocation(v.Right.Info);
                state.CurrentLocation = v.Right.Info;
                state.Step++;
                states.Add(new State(state));
            }
            if (v.Down is not null && !state.VisitedLocations.Contains(v.Down.Info))
            {
                var curTrack = new Queue<Coordinate>(t);
                q.Push(v.Down);
                curTrack.Enqueue(v.Down.Info);
                track.Push(curTrack);
                state.AddVisitedLocation(v.Down.Info);
                state.CurrentLocation = v.Down.Info;
                state.Step++;
                states.Add(new State(state));
            }
            if (v.Left is not null && !state.VisitedLocations.Contains(v.Left.Info))
            {
                var curTrack = new Queue<Coordinate>(t);
                q.Push(v.Left);
                curTrack.Enqueue(v.Left.Info);
                track.Push(curTrack);
                state.AddVisitedLocation(v.Left.Info);
                state.CurrentLocation = v.Left.Info;
                state.Step++;
                states.Add(new State(state));
            }
            if (v.Up is not null && !state.VisitedLocations.Contains(v.Up.Info))
            {
                var curTrack = new Queue<Coordinate>(t);
                q.Push(v.Up);
                curTrack.Enqueue(v.Up.Info);
                track.Push(curTrack);
                state.AddVisitedLocation(v.Up.Info);
                state.CurrentLocation = v.Up.Info;
                state.Step++;
                states.Add(new State(state));
            }
        }
        if (q.Count == 0)
        {
            return (null, states);
        }
        else
        {
            return (path, states);
        }
    }
}
