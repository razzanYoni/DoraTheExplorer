using System.Collections.Generic;

namespace DoraTheExplorer.Structure;

public class SolutionMatrix
{
    public readonly int Width;
    public readonly int Height;
    private readonly List<Coordinate> _path;
    private readonly List<Cell> _cells;
    private readonly List<CompressedState> _states;
    private readonly List<Coordinate> _treasureLocations;

    public IEnumerable<Cell> Cells => _cells.ToArray();
    public CompressedState[] States => _states.ToArray();
    public Coordinate[] TreasureLocations => _treasureLocations.ToArray();
    public Coordinate[] Path => _path.ToArray();

    public SolutionMatrix(int width, int height)
    {
        _states = new List<CompressedState>();
        _treasureLocations = new List<Coordinate>();
        _cells = new List<Cell>();
        _path = new List<Coordinate>();
        Width = width;
        Height = height;
    }

    public void AddState(CompressedState state)
    {
        _states.Add(state);
    }

    public void AddTreasureLocation(Coordinate coordinate)
    {
        _treasureLocations.Add(coordinate);
    }

    public void AddCell(Cell cell)
    {
        _cells.Add(cell);
    }

    public void SetStates(IEnumerable<CompressedState> states)
    {
        _states.Clear();
        _states.AddRange(states);
    }

    public void SetPath(IEnumerable<Coordinate> path)
    {
        _path.Clear();
        _path.AddRange(path);
    }

    public void Clear()
    {
        _states.Clear();
        _treasureLocations.Clear();
        _cells.Clear();
    }
}
