using System.Collections.Generic;
using System.Linq;

namespace DoraTheExplorer.Structure;

public class SolutionMatrix
{
    public readonly int Width;
    public readonly int Height;
    private List<Cell> cells;
    private List<State> states;
    private List<Coordinate> treasureLocations;

    public Cell[] Cells => cells.ToArray();
    public State[] States => states.ToArray();
    public Coordinate[] TreasureLocations => treasureLocations.ToArray();

    public SolutionMatrix(int width, int height)
    {
        this.states = new List<State>();
        this.treasureLocations = new List<Coordinate>();
        this.cells = new List<Cell>();
        this.Width = width;
        this.Height = height;
    }

    public void AddState(State state)
    {
        this.states.Add(state);
    }
}
