using System;

namespace DoraTheExplorer.Structure;

public class Coordinate : IEquatable<Coordinate>
{
    public readonly int X;
    public readonly int Y;

    public Coordinate(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
    
    public Coordinate(Coordinate other)
    {
        X = other.X;
        Y = other.Y;
    }

    public bool Equals(Coordinate? other)
    {
        return other is not null && X == other.X && Y == other.Y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Coordinate);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}