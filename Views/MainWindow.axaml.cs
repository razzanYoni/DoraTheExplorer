using System;
using System.Threading.Tasks;
using System.Threading; 
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Logging;
using Avalonia.Media;
using ReactiveUI;
using DoraTheExplorer.ViewModels;
using DoraTheExplorer.Algorithm;
using DoraTheExplorer.DTO;
using DoraTheExplorer.Models;
using DoraTheExplorer.Structure;
using DoraTheExplorer.Util;
using DynamicData;
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
    public Label routeLabel;

    public string filePath;

    private List<List<int>>? listMatrix;
    private List<List<Vertex<Coordinate>>> vertices;
    private Graph<Coordinate>? graph;
    private List<Coordinate>? path;
    private List<State>? states;
    private State startState;
    private List<Coordinate>? goals;
    private bool isNotError;
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
        System.Diagnostics.Debug.WriteLine(this.mazeSlider.Value);

        this.executionTimeLabel = this.FindControl<Label>("ExecutionTimeLabel");
        this.executionTimeLabel.Content = "Execution Time : 00 ms";

        this.stepsLabel = this.FindControl<Label>("StepsLabel");
        this.stepsLabel.Content = "Steps : 0";

        this.nodesLabel = this.FindControl<Label>("NodesLabel");
        this.nodesLabel.Content = "Nodes : ";
        
        this.routeLabel = this.FindControl<Label>("RouteLabel");
        // dummy
        this.routeLabel.Content = "Route : L-U-R-U-R-R\n" +
                                           "U-R-D-D-D-D-D\n";

        this.isNotError = false;
    }

    public void BrowseFileButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Filters.Add(new FileDialogFilter() { Name = "Text Files", Extensions = { "txt" } });
        dialog.AllowMultiple = false;
        var result = dialog.ShowAsync(this);
        if (result.Result != null)
        {
            // path file : result.Result[0]
            System.Diagnostics.Debug.WriteLine(result.Result[0]);

            // Show File Name in Window
            this.fileNameLabel.Content = "Filename : " + result.Result[0].Substring(result.Result[0].LastIndexOf('\\')+1);

            this.filePath = result.Result[0];
            
            /* Read File */
            // var readFromFile = new ReadFromFile();
            if (vertices is not null)
            {
                vertices.Clear();
            }

            if (goals is not null)
            {
                goals.Clear();
            }

            if (listMatrix is not null)
            {
                listMatrix.Clear();
            }

            if (graph is not null)
            {
                graph.vertices.Clear();
            }
            
            (listMatrix, vertices, graph, startState, goals, isNotError) = ReadFromFile.ReadFile(result.Result[0]);
            if (!isNotError)
            {
                // file error alert in window
                System.Diagnostics.Debug.WriteLine("File Error");
                var alert = new DialogWindow("File Error");
                
                alert.ShowDialog(this);
                this.fileNameLabel.Content = "No File Selected";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Udah Bener");
            }
        } else
        {
            System.Diagnostics.Debug.WriteLine("Masukkan File Dengan Benar");
        }
    }

    public void VisualizeButton_Click(object sender, RoutedEventArgs e )
    {
        /* Visualisasi Maze */
        if ((isNotError) && (filePath is not null))
        {
            int row = listMatrix.Count;
            int col = listMatrix[0].Count;
            
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
            
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    var cell = new Grid();
                    cell.SetValue(Grid.RowProperty, i);
                    cell.SetValue(Grid.ColumnProperty, j);
                    if (listMatrix[i][j] == -2)
                    {
                        cell.SetValue(Grid.BackgroundProperty, Avalonia.Media.Brushes.Aquamarine);
                    } else if (listMatrix[i][j] == 0)
                    {
                        cell.SetValue(Grid.BackgroundProperty, Avalonia.Media.Brushes.Azure);
                    } else if (listMatrix[i][j] == -999)
                    {
                        cell.SetValue(Grid.BackgroundProperty, Avalonia.Media.Brushes.Gold);
                    }
                    else
                    {
                        cell.SetValue(Grid.BackgroundProperty, Avalonia.Media.Brushes.Black);
                    }
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
        if ((isNotError) && (this.filePath is not null) && (graph is not null) && (goals is not null))
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
                    System.Diagnostics.Debug.WriteLine("Eksekusi bfs dengan tsp \n");
                    (path, states) = BFSSolver.FindPath(graph, startState, goals.ToArray(), true);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Eksekusi dfs dengan tsp \n");

                    (path, states) = BFSSolver.FindPath(graph, startState, goals.ToArray(), true);
                }
            }
            else
            {
                if (this.bfsRadioButton.IsChecked == true)
                {
                    System.Diagnostics.Debug.WriteLine("Eksekusi bfs tanpa tsp \n");
                    (path, states) = BFSSolver.FindPath(graph, startState, goals.ToArray(), false);

                    this.mazeSlider.Maximum = states.Count;
                    
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Eksekusi dfs tanpa tsp \n");
                    
                    (path, states) = BFSSolver.FindPath(graph, startState, goals.ToArray(), false);

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
