using System.Collections.Generic;

namespace DoraTheExplorer.Structure;

public class SolutionMatrix
{
    public readonly int Width;
    public readonly int Height;
    public readonly List<Coordinate> Path;
    public readonly List<Cell> Cells;
    public readonly List<CompressedState> States;
    public readonly List<Coordinate> TreasureLocations;

    // public IEnumerable<Cell> Cells => _cells.ToArray();
    // public CompressedState[] States => _states.ToArray();
    // public Coordinate[] TreasureLocations => _treasureLocations.ToArray();
    // public Coordinate[] Path => _path.ToArray();

    public SolutionMatrix(int width, int height)
    {
        States = new List<CompressedState>();
        TreasureLocations = new List<Coordinate>();
        Cells = new List<Cell>();
        Path = new List<Coordinate>();
        Width = width;
        Height = height;
    }

    public void AddState(CompressedState state)
    {
        States.Add(state);
    }

    public void AddTreasureLocation(Coordinate coordinate)
    {
        TreasureLocations.Add(coordinate);
    }

    public void AddCell(Cell cell)
    {
        Cells.Add(cell);
    }

    public void SetStates(IEnumerable<CompressedState> states)
    {
        States.Clear();
        States.AddRange(states);
    }

    public void SetPath(IEnumerable<Coordinate> path)
    {
        Path.Clear();
        Path.AddRange(path);
    }

    public void Clear()
    {
        States.Clear();
        TreasureLocations.Clear();
        Cells.Clear();
    }
}
