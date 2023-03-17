namespace DoraTheExplorer.Structure;

// for visualization purpose
public struct Cell
{
    public readonly Coordinate Coord;
    public bool Visitable;

    public Cell(Coordinate coord, bool visitable)
    {
        this.Coord = coord;
        this.Visitable = visitable;
    }
}
