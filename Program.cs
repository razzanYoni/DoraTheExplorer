﻿using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using DoraTheExplorer.Structure;
using DoraTheExplorer.DTO;
using DoraTheExplorer.Algorithm;
using System.Reflection;

namespace DoraTheExplorer;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't
    // initialized yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // State s = new State(new Coordinate(0, 0), 0);
        // s.AddVisitedLocation(new Coordinate(0, 1));
        // s.AddVisitedLocation(new Coordinate(0, 2));
        // string jsonString = JsonSerializer.Serialize(StateDTO.From(s));
        // StateDTO? data = JsonSerializer.Deserialize<StateDTO>(jsonString);
        // Console.WriteLine(jsonString);
        //         int[,] grid =
        //         {
        // {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        // {1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0},
        // {0,1,1,0,1,1,0,0,0,0,1,1,0,1,1,0,0,0,0,1,1,0,1,1,0,0,0,0,1,1,0},
        // {0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,0},
        // {0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,1,1,0,0,0,0,1,1,0,1,1,0,1,1,0},
        // {0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1,0},
        // {0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,1,1,0,0,0,0,1,1,0,1,1,0},
        // {0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0},
        // {0,1,1,0,1,1,0,1,1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0},
        // {0,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,0},
        // {0,1,1,0,0,0,0,1,1,0,1,1,0,0,0,0,0,0,0,1,1,0,1,1,0,1,1,0,1,1,0},
        // {0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0},
        // {0,0,0,0,1,1,0,1,1,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,1,1,0},
        // {0,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,0},
        // {0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,1,1,0,1,1,0,1,1,0,0,0,0},
        // {0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,1,0,1,1,1,1,1,0},
        // {0,1,1,0,0,0,0,1,1,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0},
        // {0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1,0},
        // {0,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,1,1,0,1,1,0,1,1,0,1,1,0},
        // {0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0},
        // {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0},
        //         };
        //         int row = grid.GetLength(0);
        //         int col = grid.GetLength(1);
        //         Vertex<Coordinate>[,] vertices = new Vertex<Coordinate>[row, col];
        //         Graph<Coordinate> g = new Graph<Coordinate>();
        //         for (int i = 0; i < row; i++)
        //         {
        //             for (int j = 0; j < col; j++)
        //             {
        //                 if (grid[i, j] == 1)
        //                 {
        //                     vertices[i, j] = new Vertex<Coordinate>(new Coordinate(i, j));
        //                     if (j - 1 >= 0 && grid[i, j - 1] == 1) // left
        //                     {
        //                         vertices[i, j].ConnectLeft(vertices[i, j - 1]);
        //                     }
        //                     if (i - 1 >= 0 && grid[i - 1, j] == 1) // up
        //                     {
        //                         vertices[i, j].ConnectUp(vertices[i - 1, j]);
        //                     }
        //                     g.AddVertex(vertices[i, j]);
        //                 }
        //             }
        //         }
        //         var (path, states) = DFSSolver.FindPath(g, new Coordinate(0, 0), new Coordinate(4, 3));
        //         // path?.ToList().ForEach(e => Console.WriteLine(e));
        //         List<StateDTO> data = new List<StateDTO>();
        //         foreach (State s in states)
        //         {
        //             data.Add(StateDTO.From(s));
        //         }
        //         Console.WriteLine(JsonSerializer.Serialize(data));
        Console.WriteLine("Hello World!");
        

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>().UsePlatformDetect().LogToTrace();
}
