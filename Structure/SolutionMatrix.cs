using System.Collections.Generic;

namespace DoraTheExplorer.Structure;

public class SolutionMatrix
{
    public readonly int Width;
    public readonly int Height;
    private readonly List<Cell> _cells;
    private readonly List<CompressedState> _states;
    private readonly List<Coordinate> _treasureLocations;

    public IEnumerable<Cell> Cells => _cells.ToArray();
    public CompressedState[] States => _states.ToArray();
    public Coordinate[] TreasureLocations => _treasureLocations.ToArray();

    public SolutionMatrix(int width, int height)
    {
        _states = new List<CompressedState>();
        _treasureLocations = new List<Coordinate>();
        _cells = new List<Cell>();
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
    
    public void AddStates(IEnumerable<CompressedState> states)
    {
        _states.AddRange(states);
    }
    
    public void SetStates(IEnumerable<CompressedState> states)
    {
        _states.Clear();
        _states.AddRange(states);
    }

    public void Clear()
    {
        this._states.Clear();
        this._treasureLocations.Clear();
        this._cells.Clear();
    }
}
