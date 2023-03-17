using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Text.Json;
using DoraTheExplorer.Structure;
using DoraTheExplorer.DTO;

namespace DoraTheExplorer;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't
    // initialized yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        State s = new State(new Coordinate(0, 0), 0);
        s.AddVisitedLocation(new Coordinate(0, 1));
        s.AddVisitedLocation(new Coordinate(0, 2));
        string jsonString = JsonSerializer.Serialize(StateDTO.From(s));
        StateDTO? data = JsonSerializer.Deserialize<StateDTO>(jsonString);
        Console.WriteLine(jsonString);
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>().UsePlatformDetect().LogToTrace();
}
