using System;

namespace DoraTheExplorer.Structure;

public readonly struct Coordinate : IEquatable<Coordinate>
{
    public readonly int x;
    public readonly int y;

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool Equals(Coordinate other)
    {
        return this.x == other.x && this.y == other.y;
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }
}