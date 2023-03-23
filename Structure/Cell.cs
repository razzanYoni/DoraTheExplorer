namespace DoraTheExplorer.Structure;

// for visualization purpose
public class Cell
{
    public readonly Coordinate Coord;
    public readonly bool Visitable;

    public Cell(Coordinate coord, bool visitable)
    {
        Coord = coord;
        Visitable = visitable;
    }
}