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

    public static (List<Coordinate>?, List<State>) FindPath(Graph<Coordinate> graph, State initialState, Coordinate[] goals, bool tsp = false)
    {
        Coordinate start = initialState.CurrentLocation;
        var state = new State(initialState);
        var paths = new List<List<Coordinate>>();
        var statesList = new List<List<State>>();
        var goalSet = new List<Coordinate>(goals);
        while (goalSet.Count > 0)
        {
            var shortestGoal = goalSet[0];
            var (shortestPath, shortestStates) = FindPath(graph, new State(state), shortestGoal);
            for (int i = 1; i < goalSet.Count; i++)
            {
                var goal = goalSet[i];
                var (path, states) = FindPath(graph, new State(state), goal);
                if (states.Count < shortestStates.Count)
                {
                    shortestPath = path;
                    shortestStates = states;
                    shortestGoal = goal;
                }
            }
            if (shortestPath is not null) paths.Add(shortestPath);
            statesList.Add(shortestStates);
            state = new State(shortestStates.Last());
            goalSet.Remove(shortestGoal);
        }
        if (tsp)
        {
            var (path, states) = FindPath(graph, new State(state), start);
            if (path is not null) paths.Add(path);
            statesList.Add(states);
        }
        var resPath = new List<Coordinate>();
        var resStates = new List<State>();
        foreach (var p in paths) resPath.AddRange(p.Skip(1));
        for (int i = 1; i < statesList.Count; i++) resStates.AddRange(statesList[i].Skip(1));
        return (resPath, resStates);
    }

    public static (List<Coordinate>?, List<State>) FindPath(Graph<Coordinate> graph, State initialState, Coordinate goal)
    {
        var states = new List<State>();
        var state = initialState;
        var q = new Stack<Vertex<Coordinate>>();
        var v = graph.Vertices.Where(e => e.Info.Equals(initialState.CurrentLocation)).FirstOrDefault(graph.Vertices[0]);
        var track = new Stack<List<Coordinate>>();
        var t = new List<Coordinate>();
        var path = new List<Coordinate>();
        q.Push(v);
        t.Add(v.Info);
        track.Push(new List<Coordinate>(t));
        // backtrack
        bool backtrack = false;
        while (q.Count > 0)
        {
            v = q.Pop();
            t = track.Pop();
            state.CurrentLocation = v.Info;
            if (v.Info.Equals(goal))
            {
                states.Add(new State(state));
                path = t;
                break;
            }
            bool blocked = true;
            if (v.Up is not null && !state.VisitedLocations.Contains(v.Up.Info) && !state.BacktrackLocations.Contains(v.Up.Info))
            {
                var curTrack = new List<Coordinate>(t);
                q.Push(v.Up);
                curTrack.Add(v.Up.Info);
                track.Push(curTrack);
                blocked = false;
            }
            if (v.Left is not null && !state.VisitedLocations.Contains(v.Left.Info) && !state.BacktrackLocations.Contains(v.Left.Info))
            {
                var curTrack = new List<Coordinate>(t);
                q.Push(v.Left);
                curTrack.Add(v.Left.Info);
                track.Push(curTrack);
                blocked = false;
            }
            if (v.Down is not null && !state.VisitedLocations.Contains(v.Down.Info) && !state.BacktrackLocations.Contains(v.Down.Info))
            {
                var curTrack = new List<Coordinate>(t);
                q.Push(v.Down);
                curTrack.Add(v.Down.Info);
                track.Push(curTrack);
                blocked = false;
            }
            if (v.Right is not null && !state.VisitedLocations.Contains(v.Right.Info) && !state.BacktrackLocations.Contains(v.Right.Info))
            {
                var curTrack = new List<Coordinate>(t);
                q.Push(v.Right);
                curTrack.Add(v.Right.Info);
                track.Push(curTrack);
                blocked = false;
            }
            if (blocked && q.Count == 0)
            {
                state.RemoveEarliestVisitedLocation();
                q.Push(v);
                track.Push(t);
                backtrack = true;
            }
            else if (backtrack)
            {
                states.Add(new State(state));
                state.AddBacktrackLocation(v.Info);
                backtrack = false;
            }
            else
            {
                states.Add(new State(state));
                state.AddVisitedLocation(v.Info);
            }
        }
        return (path.Count > 0 ? path : null, states);
    }
}
