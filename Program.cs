using Avalonia;
using System;
using DoraTheExplorer.Structure;

namespace DoraTheExplorer;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    // [STAThread]
    // public static void Main(string[] args) => BuildAvaloniaApp()
    //     .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    // public static AppBuilder BuildAvaloniaApp()
    //     => AppBuilder.Configure<App>()
    //         .UsePlatformDetect()
    //         .LogToTrace();

    readonly struct Coordinate : IEquatable<Coordinate>
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

    public static void Main(string[] args)
    {
        int[,] grid = {
            {1, 1, 0, 1, 1},
            {1, 1, 1, 1, 0},
            {0, 1, 0, 0, 1},
            {1, 1, 1, 1, 1},
            {1, 0, 0, 1, 0}
        };
        int row = grid.GetLength(0);
        int col = grid.GetLength(1);
        Vertex<Coordinate>[,] vertices = new Vertex<Coordinate>[row, col];
        Graph<Coordinate> g = new Graph<Coordinate>();
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (grid[i, j] == 1)
                {
                    vertices[i, j] = new Vertex<Coordinate>(new Coordinate(i, j));
                    if (j - 1 >= 0 && grid[i, j - 1] == 1) // left
                    {
                        vertices[i, j].ConnectLeft(vertices[i, j - 1]);
                    }
                    if (i - 1 >= 0 && grid[i - 1, j] == 1) // up
                    {
                        vertices[i, j].ConnectUp(vertices[i - 1, j]);
                    }
                    g.AddVertex(vertices[i, j]);
                }
            }
        }
        Solver.BFS<Coordinate>(g, new Coordinate(1, 2));
    }
}
