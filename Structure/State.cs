using System.Collections.Generic;
using System.Linq;

namespace DoraTheExplorer.Structure;

public struct State
{
    private HashSet<Coordinate> visitedLocations;
    private HashSet<Coordinate> backtrackLocations;
    public Coordinate CurrentLocation;
    public int Step;

    public Coordinate[] VisitedLocations => GetVisitedLocations();
    public Coordinate[] BacktrackLocations => GetBacktrackLocations();

    public State(Coordinate currentLocation, int step)
    {
        this.CurrentLocation = currentLocation;
        backtrackLocations = new HashSet<Coordinate>();
        visitedLocations = new HashSet<Coordinate>();
        this.Step = step;
    }

    public State(State other)
    {
        this.CurrentLocation = other.CurrentLocation;
        this.Step = other.Step;
        this.visitedLocations = new HashSet<Coordinate>(other.visitedLocations);
        this.backtrackLocations = new HashSet<Coordinate>(other.backtrackLocations);
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
        Coordinate[] coords = new Coordinate[backtrackLocations.Count];
        backtrackLocations.CopyTo(coords);
        return coords;
    }
}
