using System;
using System.Collections.Generic;
using DoraTheExplorer.Structure;
using System.Linq;

namespace DoraTheExplorer.Algorithm;

public class BFSSolver
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
            if (v.Up is not null && !visited.Contains(v.Up.Id))
            {
                q.Enqueue(v.Up);
                visited.Add(v.Up.Id);
            }
        }
    }

    public static (Queue<Coordinate>?, List<State>) FindPath(Graph<Coordinate> graph, Coordinate start, Coordinate goal)
    {
        var states = new List<State>();
        var state = new State(start, 1);
        var q = new Queue<Vertex<Coordinate>>();
        var v = graph.Vertices.Where(e => e.Equals(start)).FirstOrDefault(graph.Vertices[0]);
        var track = new Queue<Queue<Coordinate>>();
        var t = new Queue<Coordinate>();
        var path = new Queue<Coordinate>();
        q.Enqueue(v);
        t.Enqueue(v.Info);
        track.Enqueue(new Queue<Coordinate>(t));
        state.AddVisitedLocation(start);
        states.Add(new State(state));
        while (q.Count > 0)
        {
            v = q.Dequeue();
            t = track.Dequeue();
            if (v.Info.Equals(goal))
            {
                path = t;
                break;
            }
            if (v.Right is not null && !state.VisitedLocations.Contains(v.Right.Info))
            {
                var curTrack = new Queue<Coordinate>(t);
                q.Enqueue(v.Right);
                curTrack.Enqueue(v.Right.Info);
                track.Enqueue(curTrack);
                state.AddVisitedLocation(v.Right.Info);
                state.CurrentLocation = v.Right.Info;
                state.Step++;
                states.Add(new State(state));
            }
            if (v.Down is not null && !state.VisitedLocations.Contains(v.Down.Info))
            {
                var curTrack = new Queue<Coordinate>(t);
                q.Enqueue(v.Down);
                curTrack.Enqueue(v.Down.Info);
                track.Enqueue(curTrack);
                state.AddVisitedLocation(v.Down.Info);
                state.CurrentLocation = v.Down.Info;
                state.Step++;
                states.Add(new State(state));
            }
            if (v.Left is not null && !state.VisitedLocations.Contains(v.Left.Info))
            {
                var curTrack = new Queue<Coordinate>(t);
                q.Enqueue(v.Left);
                curTrack.Enqueue(v.Left.Info);
                track.Enqueue(curTrack);
                state.AddVisitedLocation(v.Left.Info);
                state.CurrentLocation = v.Left.Info;
                state.Step++;
                states.Add(new State(state));
            }
            if (v.Up is not null && !state.VisitedLocations.Contains(v.Up.Info))
            {
                var curTrack = new Queue<Coordinate>(t);
                q.Enqueue(v.Up);
                curTrack.Enqueue(v.Up.Info);
                track.Enqueue(curTrack);
                state.AddVisitedLocation(v.Up.Info);
                state.CurrentLocation = v.Up.Info;
                state.Step++;
                states.Add(new State(state));
            }
        }
        if (path.Count == 0)
        {
            return (null, states);
        }
        else
        {
            return (path, states);
        }
    }
}
