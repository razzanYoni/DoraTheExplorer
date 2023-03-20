using System.Collections.Generic;

namespace DoraTheExplorer.Structure;

public class SolutionMatrix
{
    public readonly int Width;
    public readonly int Height;
    private readonly List<Cell> _cells;
    private readonly List<State> _states;
    private readonly List<Coordinate> _treasureLocations;

    public Cell[] Cells => _cells.ToArray();
    public State[] States => _states.ToArray();
    public Coordinate[] TreasureLocations => _treasureLocations.ToArray();

    public SolutionMatrix(int width, int height)
    {
        this._states = new List<State>();
        this._treasureLocations = new List<Coordinate>();
        this._cells = new List<Cell>();
        this.Width = width;
        this.Height = height;
    }

    public void AddState(State state)
    {
        this._states.Add(state);
    }

    public void AddTreasureLocation(Coordinate coordinate)
    {
        this._treasureLocations.Add(coordinate);
    }

    public void AddCell(Cell cell)
    {
        this._cells.Add(cell);
    }
}
