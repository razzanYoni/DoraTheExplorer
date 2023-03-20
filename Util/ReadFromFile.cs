using DoraTheExplorer.Structure;

namespace DoraTheExplorer.Util;

public class ReadFromFile
{
    public static (SolutionMatrix?, Graph<Coordinate>?, bool) ReadFile(string path)
    {
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
        Vertex<Coordinate>[,] vertices = new Vertex<Coordinate>[row, col];

        int[,] matrix = new int[row, col];

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
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
                        solutionMatrix.AddState(new State(new Coordinate(j, i)));
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
        return (solutionMatrix, graph, true);
    }
}