using System.Collections.Generic;

namespace DoraTheExplorer.Structure;

public struct State
{
    private HashSet<Coordinate> visitedLocations;
    private HashSet<Coordinate> backtrackLocations;
    private readonly Coordinate currentLocation;
    public readonly int Step;

    public Coordinate CurrentLocation => currentLocation;
    public Coordinate[] VisitedLocations => GetVisitedLocations();
    public Coordinate[] BacktrackLocations => GetBacktrackLocations();

    public State(Coordinate currentLocation, int step)
    {
        this.currentLocation = currentLocation;
        backtrackLocations = new HashSet<Coordinate>();
        visitedLocations = new HashSet<Coordinate>();
        this.Step = step;
    }

    public void AddVisitedLocation(Coordinate coordinate)
    {
        visitedLocations.Add(coordinate);
    }

    public void AddBacktrackLocation(Coordinate coordinate)
    {
        backtrackLocations.Add(coordinate);
    }

    public Coordinate[] GetVisitedLocations()
    {
        Coordinate[] coords = new Coordinate[visitedLocations.Count];
        visitedLocations.CopyTo(coords);
        return coords;
    }

    public Coordinate[] GetBacktrackLocations()
    {
        Coordinate[] coords = new Coordinate[visitedLocations.Count];
        visitedLocations.CopyTo(coords);
        return coords;
    }
}
