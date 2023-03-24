using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Media;
using DoraTheExplorer.Structure;

namespace DoraTheExplorer.Util;

public static class Utils
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        Unknown
    }

    public static (SolutionMatrix?, Graph<Coordinate>?, bool) ReadFile(string path)
    {
        int countStart = 0;
        string[] lines = System.IO.File.ReadAllLines(path);
        int row = lines.Length;
        int col = lines[0].Replace(" ", "").Length;
        for (int i = 0; i < row; i++)
        {
            lines[i] = lines[i].Replace(" ", "");
            if (lines[i].Length != col)
            {
                return (null, null, false);
            }
        }

        var solutionMatrix = new SolutionMatrix(col, row);
        var graph = new Graph<Coordinate>();
        var vertices = new Vertex<Coordinate>[row, col];

        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                if (lines[i][j] != 'X')
                {
                    if (lines[i][j] != 'K' && lines[i][j] != 'T' && lines[i][j] != 'R')
                    {
                        return (null, null, false);
                    }

                    vertices[i, j] = new Vertex<Coordinate>(new Coordinate(j, i));
                    solutionMatrix.AddCell(new Cell(new Coordinate(j, i), true));
                    if (j - 1 >= 0 && lines[i][j - 1] != 'X')
                    {
                        vertices[i, j].ConnectLeft(vertices[i, j - 1]);
                    }

                    if (i - 1 >= 0 && lines[i - 1][j] != 'X')
                    {
                        vertices[i, j].ConnectUp(vertices[i - 1, j]);
                    }

                    if (lines[i][j] == 'K')
                    {
                        solutionMatrix.AddState(new CompressedState(new Coordinate(j, i), solutionMatrix.Width,
                            solutionMatrix.Height));
                        if (++countStart > 1)
                        {
                            return (null, null, false);
                        }
                    }

                    if (lines[i][j] == 'T')
                    {
                        solutionMatrix.AddTreasureLocation(new Coordinate(j, i));
                    }

                    graph.AddVertex(vertices[i, j]);
                }
                else
                {
                    solutionMatrix.AddCell(new Cell(new Coordinate(j, i), false));
                }
            }
        }

        return (solutionMatrix, graph, countStart == 1);
    }

    public static IEnumerable<char> ConvertRoute(List<Coordinate> route)
    {
        var res = new List<char>();
        for (var i = 0; i < route.Count - 1; i++)
        {
            switch (DetermineDirection(route[i], route[i + 1]))
            {
                case Direction.Up:
                    res.Add('\uf0aa');
                    break;
                case Direction.Left:
                    res.Add('\uf0a8');
                    break;
                case Direction.Down:
                    res.Add('\uf0ab');
                    break;
                case Direction.Right:
                    res.Add('\uf0a9');
                    break;
                case Direction.Unknown:
                    break;
                default:
                    throw new UnreachableException("gak mungkin masuk sini lah, lu ngapain sih?");
            }
        }
        return res;
    }

    public static Direction DetermineDirection(Coordinate start, Coordinate end)
    {
        var delta = (end.X - start.X, end.Y - start.Y);
        return delta switch
        {
            (0, -1) => Direction.Up,
            (-1, 0) => Direction.Left,
            (0, 1) => Direction.Down,
            (1, 0) => Direction.Right,
            _ => Direction.Unknown
        };
    }

    public static ISolidColorBrush Darken(ISolidColorBrush color, double factor)
    {
        var r = color.Color.R;
        var g = color.Color.G;
        var b = color.Color.B;
        var newR = (byte)(r * (1 - factor));
        var newG = (byte)(g * (1 - factor));
        var newB = (byte)(b * (1 - factor));
        return new SolidColorBrush(Color.FromRgb(newR, newG, newB));
    }
}