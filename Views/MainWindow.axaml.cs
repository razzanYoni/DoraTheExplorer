using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
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

    private StackPanel mazePanel;
    private StackPanel[] mazeRows;
    private Rectangle[,] cells;
    private ISolidColorBrush[,] cellColors;
    public Slider? mazeSlider;
    public Label executionTimeLabel;
    public Label stepsLabel;
    public Label nodesLabel;
    public TextBlock routeTextBlock;

    public string FilePath;

    private Graph<Coordinate>? _graph;
    private List<Coordinate>? _path;
    private List<State>? _states;
    private bool _isNotError;
    private SolutionMatrix _solutionMatrix;

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

        this.mazePanel = this.FindControl<StackPanel>("MazePanel");

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

        this._isNotError = false;
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
            Debug.WriteLine(result.Result[0]);

            // Show File Name in Window
            this.fileNameLabel.Content =
                "Filename : " + result.Result[0].Substring(result.Result[0].LastIndexOf('\\') + 1);

            this.FilePath = result.Result[0];

            /* Read File */
            // var readFromFile = new ReadFromFile();
            if (_graph is not null)
            {
                _graph.ClearVertices();
            }

            (_solutionMatrix, _graph, _isNotError) = Utils.ReadFile(result.Result[0]);
            if (!_isNotError)
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
        if (!_isNotError)
        {
            var alert = new DialogWindow("File Belum Ada");
            alert.ShowDialog(this);
            return;
        }

        var row = _solutionMatrix!.Height;
        var col = _solutionMatrix.Width;
        mazeRows = new StackPanel[row];
        cells = new Rectangle[row, col];
        cellColors = new ISolidColorBrush[row, col];
        var size = Math.Min(500 / row, 800 / col);

        mazePanel.Children.Clear();
        for (var i = 0; i < row; i++)
        {
            mazeRows[i] = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            for (var j = 0; j < col; j++)
            {
                cells[i, j] = new Rectangle
                {
                    Width = size,
                    Height = size,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                mazeRows[i].Children.Add(cells[i, j]);
            }

            mazePanel.Children.Add(mazeRows[i]);
        }

        // start cell
        var startCoord = _solutionMatrix.States[0].CurrentLocation;
        cells[startCoord.y, startCoord.x].Fill = Brushes.Aquamarine;
        cellColors[startCoord.y, startCoord.x] = Brushes.Aquamarine;

        foreach (var c in _solutionMatrix.Cells)
        {
            var coord = c.Coord;
            if (_solutionMatrix.TreasureLocations.ToList().Any(coordinate => coordinate.Equals(coord)))
            {
                cellColors[coord.y, coord.x] = Brushes.Gold;
                cells[coord.y, coord.x].Fill = cellColors[coord.y, coord.x];
            }
            else if (!c.Coord.Equals(startCoord))
            {
                cellColors[coord.y, coord.x] = c.Visitable ? Brushes.Azure : Brushes.Black;
                cells[coord.y, coord.x].Fill = cellColors[coord.y, coord.x];
            }
        }
    }

    public void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        /* Run Time */
        if ((_isNotError) && (_graph is not null) && (_solutionMatrix.TreasureLocations.Length != 0))
        {
            _path?.Clear();
            _states?.Clear();

            if (this.tspCheckBox.IsChecked == true)
            {
                if (this.bfsRadioButton.IsChecked == true)
                {
                    Debug.WriteLine("Eksekusi bfs dengan tsp \n");
                    (_path, _states) = BFSSolver.FindPath(_graph, _solutionMatrix.States.First(),
                        _solutionMatrix.TreasureLocations, true);
                }
                else
                {
                    Debug.WriteLine("Eksekusi dfs dengan tsp \n");

                    (_path, _states) = DFSSolver.FindPath(_graph, _solutionMatrix.States.First(),
                        _solutionMatrix.TreasureLocations, true);
                }
            }
            else
            {
                if (this.bfsRadioButton.IsChecked == true)
                {
                    Debug.WriteLine("Eksekusi bfs tanpa tsp \n");
                    (_path, _states) = BFSSolver.FindPath(_graph, _solutionMatrix.States.First(),
                        _solutionMatrix.TreasureLocations);
                }
                else
                {
                    Debug.WriteLine("Eksekusi dfs tanpa tsp \n");

                    (_path, _states) = DFSSolver.FindPath(_graph, _solutionMatrix.States.First(),
                        _solutionMatrix.TreasureLocations);
                }
            }

            this.mazeSlider!.Maximum = _states.Count;
            this.mazeSlider.TickFrequency = 1;
            this._solutionMatrix.SetStates(_states);
        }
        else
        {
            // send alert
            Debug.WriteLine("Belum ada file\n");
            // show dialog alert
            var alert = new DialogWindow("Belum ada file");

            alert.ShowDialog(this);
        }
    }

    private void MazeSlider_OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        mazeSlider = sender as Slider;
        if (mazeSlider is null || e.Property.Name != "Value" || _states is null ||
            _solutionMatrix.States.Length == 0) return;
        var idx = (int)mazeSlider.Value;
        var row = _solutionMatrix.Height;
        var col = _solutionMatrix.Width;
        if (idx >= _solutionMatrix.States.Length)
        {
            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    var loc = new Coordinate(j, i);
                    cells[i, j].Fill = _path!.Any(coordinate => coordinate.Equals(loc))
                        ? Utils.Darken(Brushes.LightGreen,
                            Math.Min(0.2 * (_path!.Count(coordinate => coordinate.Equals(loc)) - 1), 0.95))
                        : cellColors[i, j];
                }
            }

            return;
        }

        var state = _solutionMatrix.States[idx];
        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                var loc = new Coordinate(j, i);
                if (state.CurrentLocation.Equals(loc))
                {
                    cells[i, j].Fill = Brushes.Blue;
                }
                else if (state.BacktrackLocations.ToList().Any(coordinate => coordinate.Equals(loc)))
                {
                    cells[i, j].Fill = Brushes.Red;
                }
                else if (state.VisitedLocations.ToList().Concat(state.SavedVisitedLocations)
                         .Any(coordinate => coordinate.Equals(loc)))
                {
                    cells[i, j].Fill = Brushes.Yellow;
                }
                else
                {
                    cells[i, j].Fill = cellColors[i, j];
                }
            }
        }
    }
}