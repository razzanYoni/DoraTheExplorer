using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using DoraTheExplorer.ViewModels;
using DoraTheExplorer.Algorithm;
using DoraTheExplorer.Structure;
using DoraTheExplorer.Util;
using Tubes2_DoraTheExplorer.Views;

namespace DoraTheExplorer.Views;

// TODO : Execution Time, Steps, Route
// TODO : Visualisasi Maze
// TODO : Slider
// TODO : Counter untuk alpha
// TODO : JSON reader

public partial class MainWindow : Window
{
    public MainWindowViewModel viewModel;

    public RadioButton bfsRadioButton;
    public RadioButton dfsRadioButton;
    public CheckBox tspCheckBox;

    public Label fileNameLabel;
    public Button browseFileButton;
    public Button visualizeButton;
    public Button searchButton;

    public Grid mazeGrid;
    public Slider mazeSlider;
    public Label executionTimeLabel;
    public Label stepsLabel;
    public Label nodesLabel;
    public TextBox routeTextBox;

    public string filePath;

    private Graph<Coordinate>? graph;
    private List<Coordinate>? path;
    private List<State>? states;
    private bool isNotError;
    private SolutionMatrix solutionMatrix;

    public MainWindow()
    {
        InitializeComponent();

        this.viewModel = new MainWindowViewModel();
        this.bfsRadioButton = this.FindControl<RadioButton>("BfsRadioButton");
        this.bfsRadioButton.SetValue(ToggleButton.IsCheckedProperty, true);

        this.dfsRadioButton = this.FindControl<RadioButton>("DfsRadioButton");

        this.tspCheckBox = this.FindControl<CheckBox>("TspCheckBox");

        this.fileNameLabel = this.FindControl<Label>("FileNameLabel");
        this.fileNameLabel.Content = "No File Selected";

        this.browseFileButton = this.FindControl<Button>("BrowseFileButton");

        this.visualizeButton = this.FindControl<Button>("VisualizeButton");

        this.searchButton = this.FindControl<Button>("SearchButton");

        this.mazeGrid = this.FindControl<Grid>("MazeGrid");
        this.mazeGrid.Background = Brushes.Black;

        this.mazeSlider = this.FindControl<Slider>("MazeSlider");
        this.mazeSlider.Orientation = Orientation.Horizontal;
        this.mazeSlider.TickPlacement = TickPlacement.TopLeft;
        this.mazeSlider.TickFrequency = 1;

        this.executionTimeLabel = this.FindControl<Label>("ExecutionTimeLabel");
        this.executionTimeLabel.Content = "Execution Time : 00 ms";

        this.stepsLabel = this.FindControl<Label>("StepsLabel");
        this.stepsLabel.Content = "Steps : 0";

        this.nodesLabel = this.FindControl<Label>("NodesLabel");
        this.nodesLabel.Content = "Nodes : ";

        this.routeTextBox = this.FindControl<TextBox>("RouteTextBox");
        this.routeTextBox.SetValue(TextBox.TextProperty, "Route : fdashooaspdfpoasjodfhaoipfhdoashfopsadhfoiashdoifpahodhasofphpoahfopaosdfhdsaohfpo");

        this.isNotError = false;
    }

    public void BrowseFileButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Filters?.Add(new FileDialogFilter() { Name = "Text Files", Extensions = { "txt" } });
        dialog.AllowMultiple = false;
        var result = dialog.ShowAsync(this);
        if (result.Result != null)
        {
            // path file : result.Result[0]
            System.Diagnostics.Debug.WriteLine(result.Result[0]);

            // Show File Name in Window
            this.fileNameLabel.Content =
                "Filename : " + result.Result[0].Substring(result.Result[0].LastIndexOf('\\') + 1);

            this.filePath = result.Result[0];

            /* Read File */
            // var readFromFile = new ReadFromFile();
            if (graph is not null)
            {
                graph.ClearVertices();
            }

            (solutionMatrix, graph, isNotError) = Utils.ReadFile(result.Result[0]);
            if (!isNotError)
            {
                // file error alert in window
                Debug.WriteLine("File Error");
                var alert = new DialogWindow("File Error");

                alert.ShowDialog(this);
                this.fileNameLabel.Content = "No File Selected";
            }
            else
            {
                Debug.WriteLine("Udah Bener");
            }
        }
        else
        {
            Debug.WriteLine("Masukkan File Dengan Benar");
        }
    }

    public void VisualizeButton_Click(object sender, RoutedEventArgs e)
    {
        /* Visualisasi Maze */
        if (isNotError)
        {
            int row = solutionMatrix.Height;
            int col = solutionMatrix.Width;

            mazeGrid.RowDefinitions.Clear();
            mazeGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < row; i++)
            {
                mazeGrid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
            }

            for (int j = 0; j < col; j++)
            {
                mazeGrid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
            }

            // start cell
            var startCoord = solutionMatrix.States[0].CurrentLocation;
            var cell = new Grid();
            cell.SetValue(Grid.RowProperty, startCoord.y);
            cell.SetValue(Grid.ColumnProperty, startCoord.x);
            cell.SetValue(Grid.BackgroundProperty, Brushes.Aquamarine);
            mazeGrid.Children.Add(cell);

            foreach (var c in solutionMatrix.Cells)
            {
                cell = new Grid();
                var coord = c.Coord;
                cell.SetValue(Grid.RowProperty, coord.y);
                cell.SetValue(Grid.ColumnProperty, coord.x);
                if (solutionMatrix.TreasureLocations.ToList().Any(coordinate => coordinate.Equals(coord)))
                {
                    cell.SetValue(Grid.BackgroundProperty, Brushes.Gold);
                    mazeGrid.Children.Add(cell);
                }
                else if (!c.Coord.Equals(startCoord))
                {
                    cell.SetValue(Grid.BackgroundProperty,
                    c.Visitable ? Brushes.Azure : Brushes.Black);
                    mazeGrid.Children.Add(cell);

                }

            }
            /*
            (path, states) = BFSSolver.FindPath(graph, startState, new Coordinate[] {startState.CurrentLocation});
            /* Do something #1#
            List<StateDTO> data = new List<StateDTO>();
            foreach (State s in states)
            {
                data.Add(StateDTO.From(s));
            }

            var jsonResult = JsonSerializer.Serialize(data);

            path.Clear();
            states.Clear();*/
        }
        else
        {
            var alert = new DialogWindow("File Belum Ada");

            alert.ShowDialog(this);
        }
    }

    public void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        /* Run Time */
        if ((isNotError) && (graph is not null) && (solutionMatrix.TreasureLocations.Length != 0))
        {
            if (path is not null)
            {
                path.Clear();
            }

            if (states is not null)
            {
                states.Clear();
            }

            if (this.tspCheckBox.IsChecked == true)
            {
                if (this.bfsRadioButton.IsChecked == true)
                {
                    Debug.WriteLine("Eksekusi bfs dengan tsp \n");
                    (path, states) = BFSSolver.FindPath(graph, solutionMatrix.States[0],
                        solutionMatrix.TreasureLocations, true);
                }
                else
                {
                    Debug.WriteLine("Eksekusi dfs dengan tsp \n");

                    (path, states) = DFSSolver.FindPath(graph, solutionMatrix.States[0],
                        solutionMatrix.TreasureLocations, true);
                }
            }
            else
            {
                if (this.bfsRadioButton.IsChecked == true)
                {
                    Debug.WriteLine("Eksekusi bfs tanpa tsp \n");
                    (path, states) = BFSSolver.FindPath(graph, solutionMatrix.States[0],
                        solutionMatrix.TreasureLocations, false);

                    this.mazeSlider.Maximum = states.Count;
                }
                else
                {
                    Debug.WriteLine("Eksekusi dfs tanpa tsp \n");

                    (path, states) = DFSSolver.FindPath(graph, solutionMatrix.States[0],
                        solutionMatrix.TreasureLocations, false);

                    this.mazeSlider.Maximum = states.Count;
                }
            }
        }
        else
        {
            // send alert
            System.Diagnostics.Debug.WriteLine("Belum ada file\n");
            // show dialog alert
            var alert = new DialogWindow("Belum ada file");

            alert.ShowDialog(this);
        }
    }

    public enum CellType
    {
        Start = -2,
        Space = 0,
        Wall = -1,
        Treasure = -999,
        undefined = -9999,
    }
}