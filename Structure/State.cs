using System.Collections.Generic;

namespace DoraTheExplorer.Structure;

public class State
{
    public readonly List<Coordinate> VisitedLocations;
    public readonly List<Coordinate> BacktrackLocations;
    public Coordinate CurrentLocation;
    public readonly HashSet<Coordinate> SavedVisitedLocations;

    public State(Coordinate currentLocation)
    {
        this.CurrentLocation = currentLocation;
        BacktrackLocations = new List<Coordinate>();
        VisitedLocations = new List<Coordinate>();
        SavedVisitedLocations = new HashSet<Coordinate>();
    }

    public State(State other)
    {
        this.CurrentLocation = other.CurrentLocation;
        this.VisitedLocations = new List<Coordinate>(other.VisitedLocations);
        this.BacktrackLocations = new List<Coordinate>(other.BacktrackLocations);
        this.SavedVisitedLocations = new HashSet<Coordinate>(other.SavedVisitedLocations);
    }

    public void AddVisitedLocation(Coordinate coordinate)
    {
        VisitedLocations.Add(coordinate);
    }

    public void AddBacktrackLocation(Coordinate coordinate)
    {
        BacktrackLocations.Add(coordinate);
    }

    public void RemoveLatestVisitedLocation()
    {
        VisitedLocations.RemoveAt(VisitedLocations.Count - 1);
    }

    public void RemoveEarliestVisitedLocation()
    {
        VisitedLocations.RemoveAt(0);
    }

}
