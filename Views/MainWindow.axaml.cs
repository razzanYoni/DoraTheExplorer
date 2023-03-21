using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using DoraTheExplorer.Algorithm;
using DoraTheExplorer.Structure;
using DoraTheExplorer.Util;
using Tubes2_DoraTheExplorer.Views;

namespace DoraTheExplorer.Views;

// TODO : Execution Time, Steps, Route
// TODO : Slider SHow solution path 
// TODO : Counter untuk alpha


public partial class MainWindow : Window
{

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
    public TextBlock routeTextBlock;

    public string filePath;

    private Graph<Coordinate>? graph;
    private List<Coordinate>? path;
    private List<State>? states;
    private bool isNotError;
    private SolutionMatrix solutionMatrix;

    public MainWindow()
    {
        InitializeComponent();

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

        this.routeTextBlock = this.FindControl<TextBlock>("RouteTextBlock");
        this.routeTextBlock.SetValue(
            TextBlock.TextProperty,
            "Route : fdashooaspdfpoasjodfhaoipfhdoashfopsadhfoiashdoifpahodhasofphpoahfopaosdfhdsaohfpo" +
            "dsfasfasdfasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "afdasfasdfasdffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffasdfasfasdf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "dfasfasfadfdafdasfdasfasdfasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "asfdasadsgafsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "dfasfafsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf" +
            "fsadfasdfasdfasdfadsfasdfasdfasdfadsfdasffsadfasdfasdfasdfadsfasdfasdfasdfadsfdasf");

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
                    this.mazeSlider.Maximum = states.Count;
                    this.solutionMatrix.SetStates(states);
                }
                else
                {
                    Debug.WriteLine("Eksekusi dfs dengan tsp \n");

                    (path, states) = DFSSolver.FindPath(graph, solutionMatrix.States[0],
                        solutionMatrix.TreasureLocations, true);
                    this.mazeSlider.Maximum = states.Count;
                    this.solutionMatrix.SetStates(states);
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
                    this.solutionMatrix.SetStates(states);
                }
                else
                {
                    Debug.WriteLine("Eksekusi dfs tanpa tsp \n");

                    (path, states) = DFSSolver.FindPath(graph, solutionMatrix.States[0],
                        solutionMatrix.TreasureLocations, false);

                    this.mazeSlider.Maximum = states.Count;
                    this.solutionMatrix.SetStates(states);
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

    private void MazeSlider_OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        mazeSlider = (Slider)sender;
        if (mazeSlider is not null && e.Property.Name == "Value" && states is not null &&
            solutionMatrix.States.Length != 0)
        {
            int idx = (int)mazeSlider.Value;
            State state = solutionMatrix.States[idx];
            Grid? cell;
            foreach (var visited in state.VisitedLocations.Concat(state.SavedVisitedLocations))
            {
                cell = new Grid();
                cell.SetValue(Grid.RowProperty, visited.y);
                cell.SetValue(Grid.ColumnProperty, visited.x);
                cell.SetValue(Panel.BackgroundProperty, Brushes.Yellow);
                mazeGrid.Children.Add(cell);
            }

            var current = state.CurrentLocation;
            cell = new Grid();
            cell.SetValue(Grid.RowProperty, current.y);
            cell.SetValue(Grid.ColumnProperty, current.x);
            cell.SetValue(Panel.BackgroundProperty, Brushes.Blue);
            mazeGrid.Children.Add(cell);
        }
    }
}