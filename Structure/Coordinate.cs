using System;

namespace DoraTheExplorer.Structure;

public class Coordinate : IEquatable<Coordinate>
{
    public readonly int x;
    public readonly int y;

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    
    public Coordinate(Coordinate other)
    {
        x = other.x;
        y = other.y;
    }

    public bool Equals(Coordinate? other)
    {
        return other is not null && x == other.x && y == other.y;
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Coordinate);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }
}