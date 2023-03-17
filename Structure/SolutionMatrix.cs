using System.Collections.Generic;
using System.Linq;

namespace DoraTheExplorer.Structure;

public class SolutionMatrix
{
    public readonly int Width;
    public readonly int Height;
    private List<Cell> cells;
    private List<State> states;

    public Cell[] Cells => cells.ToArray();

    public SolutionMatrix(int width, int height)
    {
        states = new List<State>();
        cells = new List<Cell>();
        Width = width;
        Height = height;
    }
}
