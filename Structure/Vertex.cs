﻿namespace DoraTheExplorer.Structure;

public class Vertex<T>
    where T : notnull
{
    private static int _id = 0;
    public readonly int Id;
    public T Info;
    private Vertex<T>? _left;
    private Vertex<T>? _up;
    private Vertex<T>? _right;
    private Vertex<T>? _down;

    public Vertex(T info)
    {
        this.Info = info;
        this.Id = _id;
        _id++;
    }

    public Vertex<T>? Left
    {
        get => _left;
    }
    public Vertex<T>? Right
    {
        get => _right;
    }
    public Vertex<T>? Up
    {
        get => _up;
    }
    public Vertex<T>? Down
    {
        get => _down;
    }

    public void ConnectLeft(Vertex<T> left)
    {
        this._left = left;
        left._right = this;
    }

    public void ConnectRight(Vertex<T> right)
    {
        this._right = right;
        right._left = this;
    }

    public void ConnectUp(Vertex<T> up)
    {
        this._up = up;
        up._down = this;
    }

    public void ConnectDown(Vertex<T> down)
    {
        this._down = down;
        down._up = this;
    }
}
