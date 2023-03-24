using System;
using System.Collections.Generic;
using DoraTheExplorer.Util;

namespace DoraTheExplorer.Structure;

public class CompressedState
{
    public Coordinate CurrentLocation;
    public Utils.Direction Dir = Utils.Direction.Up;
    private readonly int[] _visitedLocations;
    private readonly int[] _backtrackLocations;
    private readonly int[] _savedVisitedLocations;

    private readonly int _width;
    private readonly int _height;
    private readonly Stack<Coordinate> _history;

    public CompressedState(Coordinate currentLocation, int width, int height)
    {
        CurrentLocation = currentLocation;
        this._width = width;
        this._height = height;
        _history = new Stack<Coordinate>();
        _visitedLocations = new int[width * height / (sizeof(int) * 8) + 1];
        _backtrackLocations = new int[width * height / (sizeof(int) * 8) + 1];
        _savedVisitedLocations = new int[width * height / (sizeof(int) * 8) + 1];
    }

    public CompressedState(CompressedState other)
    {
        CurrentLocation = other.CurrentLocation;
        Dir = other.Dir;
        _width = other._width;
        _height = other._height;
        _history = new Stack<Coordinate>(other._history);
        _visitedLocations = new int[other._visitedLocations.Length];
        other._visitedLocations.CopyTo(_visitedLocations, 0);
        _backtrackLocations = new int[other._backtrackLocations.Length];
        other._backtrackLocations.CopyTo(_backtrackLocations, 0);
        _savedVisitedLocations = new int[other._savedVisitedLocations.Length];
        other._savedVisitedLocations.CopyTo(_savedVisitedLocations, 0);
    }

    public void AddVisitedLocation(Coordinate coordinate)
    {
        var (idx, bit) = Calculate(coordinate);
        _visitedLocations[idx] |= 1 << bit;
        _history.Push(coordinate);
    }

    public void AddBacktrackLocation(Coordinate coordinate)
    {
        var (idx, bit) = Calculate(coordinate);
        _backtrackLocations[idx] |= 1 << bit;
    }

    private void AddSavedVisitedLocation(Coordinate coordinate)
    {
        var (idx, bit) = Calculate(coordinate);
        _savedVisitedLocations[idx] |= 1 << bit;
    }

    public void RemoveLatestVisitedLocation()
    {
        var coordinate = _history.Pop();
        var (idx, bit) = Calculate(coordinate);
        _visitedLocations[idx] &= ~(1 << bit);
        AddSavedVisitedLocation(coordinate);
    }

    public bool IsVisited(Coordinate coordinate)
    {
        var (idx, bit) = Calculate(coordinate);
        return ((_visitedLocations[idx] >> bit) & 1) == 1;
    }

    public void RemoveVisitedLocation(Coordinate coordinate)
    {
        var (idx, bit) = Calculate(coordinate);
        _visitedLocations[idx] &= ~(1 << bit);
        AddSavedVisitedLocation(coordinate);
    }

    public void SaveAllVisitedLocations()
    {
        for (var i = 0; i < _visitedLocations.Length; i++)
        {
            _savedVisitedLocations[i] |= _visitedLocations[i];
        }
    }

    public void ClearVisitedLocations()
    {
        for (var i = 0; i < _savedVisitedLocations.Length; i++)
        {
            _visitedLocations[i] = 0;
        }
    }

    public bool IsBacktracked(Coordinate coordinate)
    {
        var (idx, bit) = Calculate(coordinate);
        return ((_backtrackLocations[idx] >> bit) & 1) == 1;
    }

    public bool IsSavedVisited(Coordinate coordinate)
    {
        var (idx, bit) = Calculate(coordinate);
        return ((_savedVisitedLocations[idx] >> bit) & 1) == 1;
    }

    private (int, int) Calculate(Coordinate coordinate)
    {
        var n = coordinate.Y * _width + coordinate.X;
        var (idx, bit) = Math.DivRem(n, sizeof(int) * 8);
        return (idx, bit);
    }
}