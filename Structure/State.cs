using System.Collections.Generic;
using System.Linq;

namespace DoraTheExplorer.Structure;

public struct State
{
    private List<Coordinate> visitedLocations;
    private List<Coordinate> backtrackLocations;
    public Coordinate CurrentLocation;

    public Coordinate[] VisitedLocations => visitedLocations.ToArray();
    public Coordinate[] BacktrackLocations => backtrackLocations.ToArray();

    public State(Coordinate currentLocation)
    {
        this.CurrentLocation = currentLocation;
        backtrackLocations = new List<Coordinate>();
        visitedLocations = new List<Coordinate>();
    }

    public State(State other)
    {
        this.CurrentLocation = other.CurrentLocation;
        this.visitedLocations = new List<Coordinate>(other.visitedLocations);
        this.backtrackLocations = new List<Coordinate>(other.backtrackLocations);
    }

    public void AddVisitedLocation(Coordinate coordinate)
    {
        visitedLocations.Add(coordinate);
    }

    public void AddBacktrackLocation(Coordinate coordinate)
    {
        backtrackLocations.Add(coordinate);
    }

    public void RemoveLatestVisitedLocation()
    {
        visitedLocations.RemoveAt(visitedLocations.Count - 1);
    }

}
