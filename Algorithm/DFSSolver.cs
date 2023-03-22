using System;
using System.Collections.Generic;
using DoraTheExplorer.Structure;
using System.Linq;

namespace DoraTheExplorer.Algorithm;

public class DFSSolver
{
    public static void DFS<T>(Graph<T> graph, T startVertexInfo)
        where T : IEquatable<T>
    {
        var vertices = graph.Vertices;
        if (vertices.Length == 0)
            return;
        var visited = new HashSet<int>();
        var start = vertices.Where(e => e.Info.Equals(startVertexInfo)).FirstOrDefault(vertices[0]);
        DFSImpl<T>(visited, start);
    }

    private static void DFSImpl<T>(HashSet<int> visited, Vertex<T> start)
        where T : notnull
    {
        visited.Add(start.Id);
        Console.Write(start.Info + " ");
        if (start.Right is not null && !visited.Contains(start.Right.Id))
        {
            DFSImpl(visited, start.Right);
        }

        if (start.Down is not null && !visited.Contains(start.Down.Id))
        {
            DFSImpl(visited, start.Down);
        }

        if (start.Left is not null && !visited.Contains(start.Left.Id))
        {
            DFSImpl(visited, start.Left);
        }

        if (start.Up is not null && !visited.Contains(start.Up.Id))
        {
            DFSImpl(visited, start.Up);
        }
    }

    public static (List<Coordinate> resPath, List<CompressedState> resStates) FindPath(Graph<Coordinate> graph, CompressedState initialState,
        IEnumerable<Coordinate> goals, bool tsp = false)
    {
        var start = initialState.CurrentLocation;
        var state = new CompressedState(initialState);
        var paths = new List<List<Coordinate>>();
        var statesList = new List<List<CompressedState>>();
        var goalSet = new List<Coordinate>(goals);
        while (goalSet.Count > 0)
        {
            var shortestGoal = goalSet[0];
            var (shortestPath, shortestStates) = FindPath(graph, new CompressedState(state), shortestGoal);
            for (var i = 1; i < goalSet.Count; i++)
            {
                var goal = goalSet[i];
                var (path, states) = FindPath(graph, new CompressedState(state), goal);
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
            state.RemoveVisitedLocation(start);
            var (path, states) = FindPath(graph, new CompressedState(state), start);
            if (path is not null) paths.Add(path);
            statesList.Add(states);
        }

        var resPath = new List<Coordinate>(paths.First());
        var resStates = new List<CompressedState>(statesList.First());
        for (int i = 1; i < paths.Count; i++) resPath.AddRange(paths[i].Skip(1));
        for (int i = 1; i < statesList.Count; i++) resStates.AddRange(statesList[i].Skip(1));
        return (resPath, resStates);
    }

    public static (List<Coordinate>?, List<CompressedState>) FindPath(Graph<Coordinate> graph, CompressedState initialState,
        Coordinate goal)
    {
        var states = new List<CompressedState>();
        var state = new CompressedState(initialState);
        var q = new Stack<Vertex<Coordinate>>();
        var v = graph.Vertices.Where(e => e.Info.Equals(initialState.CurrentLocation))
            .FirstOrDefault(graph.Vertices[0]);
        var track = new Stack<List<Coordinate>>();
        var t = new List<Coordinate>();
        var path = new List<Coordinate>();
        q.Push(v);
        t.Add(v.Info);
        track.Push(new List<Coordinate>(t));
        // backtrack
        var backtrack = false;
        while (q.Count > 0)
        {
            v = q.Pop();
            t = track.Pop();
            state.CurrentLocation = v.Info;
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
            if (v.Up is not null && !state.IsVisited(v.Up.Info) && !state.IsBacktracked(v.Up.Info))
            {
                var curTrack = new List<Coordinate>(t);
                q.Push(v.Up);
                curTrack.Add(v.Up.Info);
                track.Push(curTrack);
                blocked = false;
            }

            if (v.Left is not null && !state.IsSavedVisited(v.Left.Info) && !state.IsBacktracked(v.Left.Info))
            {
                var curTrack = new List<Coordinate>(t);
                q.Push(v.Left);
                curTrack.Add(v.Left.Info);
                track.Push(curTrack);
                blocked = false;
            }

            if (v.Down is not null && !state.IsSavedVisited(v.Down.Info) && !state.IsBacktracked(v.Down.Info))
            {
                var curTrack = new List<Coordinate>(t);
                q.Push(v.Down);
                curTrack.Add(v.Down.Info);
                track.Push(curTrack);
                blocked = false;
            }

            if (v.Right is not null && !state.IsSavedVisited(v.Right.Info) && !state.IsBacktracked(v.Right.Info))
            {
                var curTrack = new List<Coordinate>(t);
                q.Push(v.Right);
                curTrack.Add(v.Right.Info);
                track.Push(curTrack);
                blocked = false;
            }

            if (blocked && q.Count == 0)
            {
                state.RemoveLatestVisitedLocation();
                q.Push(v);
                track.Push(t);
                backtrack = true;
            }
            else if (backtrack)
            {
                states.Add(new CompressedState(state));
                state.AddBacktrackLocation(v.Info);
                backtrack = false;
            }
            else
            {
                states.Add(new CompressedState(state));
                state.AddVisitedLocation(v.Info);
            }
        }

        return (path.Count > 0 ? path : null, states);
    }
}