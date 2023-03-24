using System;
using System.Collections.Generic;
using DoraTheExplorer.Structure;
using System.Linq;
using DoraTheExplorer.Util;

namespace DoraTheExplorer.Algorithm;

public static class BfsSolver
{
    public static void Bfs<T>(Graph<T> graph, T startVertexInfo) where T : IEquatable<T>
    {
        var vertices = graph.Vertices;
        if (vertices.Length == 0) return;
        var visited = new HashSet<int>();
        var start = vertices.Where(e => e.Info.Equals(startVertexInfo)).FirstOrDefault(vertices[0]);
        BfsImpl(visited, start);
    }

    private static void BfsImpl<T>(HashSet<int> visited, Vertex<T> start) where T : notnull
    {
        var q = new Queue<Vertex<T>>();
        q.Enqueue(start);
        visited.Add(start.Id);
        while (q.Count > 0)
        {
            var v = q.Dequeue();
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

    public static (List<Coordinate>?, List<CompressedState>) FindPath(Graph<Coordinate> graph,
        CompressedState initialState, IEnumerable<Coordinate> goals, bool tsp = false,
        string directionPriority = "RDLU")
    {
        var start = initialState.CurrentLocation;
        var state = new CompressedState(initialState);
        var paths = new List<List<Coordinate>>();
        var statesList = new List<List<CompressedState>>();
        var goalSet = new List<Coordinate>(goals);
        while (goalSet.Count > 0)
        {
            var shortestGoal = goalSet[0];
            var (shortestPath, shortestStates) =
                FindPath(graph, new CompressedState(state), shortestGoal, directionPriority);
            for (var i = 1; i < goalSet.Count; i++)
            {
                var goal = goalSet[i];
                var (path, states) = FindPath(graph, new CompressedState(state), goal, directionPriority);
                if (states.Count >= shortestStates.Count) continue;
                shortestPath = path;
                shortestStates = states;
                shortestGoal = goal;
            }

            if (shortestPath is not null) paths.Add(shortestPath);
            statesList.Add(shortestStates);
            state = new CompressedState(shortestStates.Last());
            goalSet.Remove(shortestGoal);
        }

        if (tsp)
        {
            var revDir = new string(directionPriority.Reverse().ToArray());
            var (path, states) = FindPath(graph, new CompressedState(state), start, revDir);
            if (path is not null) paths.Add(path);
            statesList.Add(states);
        }

        var resPath = new List<Coordinate>(paths.First());
        var resStates = new List<CompressedState>(statesList.First());
        for (var i = 1; i < paths.Count; i++) resPath.AddRange(paths[i].Skip(1));
        for (var i = 1; i < statesList.Count; i++) resStates.AddRange(statesList[i].Skip(1));
        return (resPath, resStates);
    }

    public static (List<Coordinate>?, List<CompressedState>) FindPath(Graph<Coordinate> graph,
        CompressedState initialState, Coordinate goal, string directionPriority)
    {
        var states = new List<CompressedState>();
        var state = new CompressedState(initialState);
        var q = new Queue<Vertex<Coordinate>>();
        var v = graph.Vertices.Where(e => e.Info.Equals(initialState.CurrentLocation))
            .FirstOrDefault(graph.Vertices[0]);
        var track = new Queue<List<Coordinate>>();
        var t = new List<Coordinate>();
        var path = new List<Coordinate>();
        q.Enqueue(v);
        t.Add(v.Info);
        track.Enqueue(new List<Coordinate>(t));
        // backtrack
        while (q.Count > 0)
        {
            v = q.Dequeue();
            t = track.Dequeue();
            state.CurrentLocation = v.Info;
            if (t.Count > 1)
            {
                state.Dir = Utils.DetermineDirection(t[^2], t[^1]);
            }

            if (v.Info.Equals(goal))
            {
                states.Add(new CompressedState(state));
                path = t;
                break;
            }

            if (state.IsVisited(v.Info))
            {
                continue;
            }

            var blocked = true;
            foreach (var dir in directionPriority)
            {
                Seek(v, q, t, track, state, dir, ref blocked);
            }

            if (blocked && q.Count == 0)
            {
                state.SaveAllVisitedLocations();
                state.ClearVisitedLocations();
                q.Enqueue(v);
                track.Enqueue(t);
            }
            else
            {
                states.Add(new CompressedState(state));
                state.AddVisitedLocation(v.Info);
            }
        }

        states[0].Dir = states[1].Dir;
        return (path.Count > 0 ? path : null, states);
    }

    private static void Seek(Vertex<Coordinate> v, Queue<Vertex<Coordinate>> q, IEnumerable<Coordinate> t,
        Queue<List<Coordinate>> track, CompressedState state, char direction, ref bool blocked)
    {
        var neighbour = v.GetNeighbour(direction);
        if (neighbour is null || state.IsVisited(neighbour.Info) || state.IsBacktracked(neighbour.Info)) return;
        q.Enqueue(neighbour);
        var curTrack = new List<Coordinate>(t);
        if (curTrack.Count > 1 && curTrack[^2].Equals(neighbour.Info))
        {
            curTrack.Remove(curTrack.Last());
        }
        else
        {
            curTrack.Add(neighbour.Info);
        }

        track.Enqueue(curTrack);
        blocked = false;
    }
}