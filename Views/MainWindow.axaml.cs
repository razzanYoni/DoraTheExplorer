using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using DoraTheExplorer.Algorithm;
using DoraTheExplorer.Structure;
using DoraTheExplorer.Util;

// ReSharper disable UnusedParameter.Local

namespace DoraTheExplorer.Views;

public partial class MainWindow : Window
{
    // Input
    private readonly Label _fileNameLabel;
    private readonly RadioButton _bfsRadioButton;
    private readonly CheckBox _tspCheckBox;

    private readonly StackPanel _mazePanel;
    private StackPanel[] _mazeRows = null!;
    private Panel[,] _cells = null!;
    private Slider _mazeSlider;
    private readonly Label _executionTimeLabel;
    private readonly Label _stepsLabel;
    private readonly Label _nodesLabel;
    private readonly TextBlock _routeTextBlock;

    private Graph<Coordinate>? _graph;
    private bool _isNotError;
    private SolutionMatrix? _solutionMatrix;

    private readonly Image _doraImage;
    private readonly Image _closedTreasureImage;
    private readonly Image _openedTreasureImage;
    private readonly Image _dirtImage;
    private readonly Image _grassImage;
    private readonly Image _footsteps;

    // Media Player
    private bool _isPlayed;

    public MainWindow()
    {
        InitializeComponent();

        _bfsRadioButton = this.FindControl<RadioButton>("BfsRadioButton");
        _bfsRadioButton.SetValue(ToggleButton.IsCheckedProperty, true);

        this.FindControl<RadioButton>("DfsRadioButton");

        _tspCheckBox = this.FindControl<CheckBox>("TspCheckBox");

        _fileNameLabel = this.FindControl<Label>("FileNameLabel");
        _fileNameLabel.Content = "No File Selected";

        _mazePanel = this.FindControl<StackPanel>("MazePanel");

        _mazeSlider = this.FindControl<Slider>("MazeSlider");
        _mazeSlider.Orientation = Orientation.Horizontal;
        _mazeSlider.TickPlacement = TickPlacement.TopLeft;
        _mazeSlider.TickFrequency = 1;

        _executionTimeLabel = this.FindControl<Label>("ExecutionTimeLabel");
        _executionTimeLabel.Content = "-";

        _stepsLabel = this.FindControl<Label>("StepsLabel");
        _stepsLabel.Content = "-";

        _nodesLabel = this.FindControl<Label>("NodesLabel");
        _nodesLabel.Content = "-";

        _routeTextBlock = this.FindControl<TextBlock>("RouteTextBlock");

        _isNotError = false;
        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;

        Bitmap LoadBitmap(string filename) =>
            new(AvaloniaLocator.Current.GetService<IAssetLoader>()
                ?.Open(new Uri($"avares://{assemblyName}/Assets/image/{filename}")));

        _doraImage = new Image { Source = LoadBitmap("dora.png") };
        _closedTreasureImage = new Image { Source = LoadBitmap("treasure-chest-closed.png") };
        _openedTreasureImage = new Image { Source = LoadBitmap("treasure-chest-opened.png") };
        _dirtImage = new Image { Source = LoadBitmap("dirt.png") };
        _grassImage = new Image { Source = LoadBitmap("grass.png") };
        var footstepBitmap = LoadBitmap("footsteps.png");
        _footsteps = new Image { Source = footstepBitmap, RenderTransform = new RotateTransform(0) };
    }

    public async void BrowseFileButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Filters?.Add(new FileDialogFilter { Name = "Text Files", Extensions = { "txt" } });
        dialog.AllowMultiple = false;
        var result = await dialog.ShowAsync(this);
        if (result == null)
        {
            Debug.WriteLine("Masukkan File Dengan Benar");
            return;
        }

        // path file : result.Result[0]
        Debug.WriteLine(result[0]);

        // Show File Name in Window
        _fileNameLabel.Content =
            "Filename : " + Path.GetFileName(result[0]);

        /* Read File */
        // var readFromFile = new ReadFromFile();
        _graph?.ClearVertices();
        _solutionMatrix?.Clear();

        (_solutionMatrix, _graph, _isNotError) = Utils.ReadFile(result[0]);
        if (!_isNotError)
        {
            // file error alert in window
            Debug.WriteLine("File Error");
            var alert = new DialogWindow("File Error");

            await alert.ShowDialog(this);
            _fileNameLabel.Content = "No File Selected";
        }
        else
        {
            Debug.WriteLine("Udah Bener");
            _mazeSlider.Maximum = 0;
            Visualize();
        }
    }

    private void Visualize()
    {
        /* Visualisasi Maze */
        var row = _solutionMatrix!.Height;
        var col = _solutionMatrix.Width;
        _mazeRows = new StackPanel[row];
        _cells = new Panel[row, col];
        var size = Math.Max(Math.Min(600 / row, 800 / col), 40);
        _doraImage.Height = size;
        _closedTreasureImage.Width = size * .6;
        _openedTreasureImage.Width = size * .6;
        _footsteps.Width = size * .4;

        _mazePanel.Children.Clear();
        for (var i = 0; i < row; i++)
        {
            _mazeRows[i] = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            for (var j = 0; j < col; j++)
            {
                _cells[i, j] = new Panel
                {
                    Children =
                    {
                        new Border
                        {
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(.5),
                            Width = size,
                            Height = size
                        }
                    }
                };
                _mazeRows[i].Children.Add(_cells[i, j]);
            }

            _mazePanel.Children.Add(_mazeRows[i]);
        }

        ClearCells();
        // start cell
        var startCoord = _solutionMatrix.States[0].CurrentLocation;
        _cells[startCoord.Y, startCoord.X].Children.Add(new Image { Source = _dirtImage.Source, Width = size, });
        _cells[startCoord.Y, startCoord.X].Children
            .Add(new Image { Source = _doraImage.Source, Height = _doraImage.Height });

        foreach (var c in _solutionMatrix.Cells)
        {
            var coord = c.Coord;
            var cell = _cells[coord.Y, coord.X];
            if (_solutionMatrix.TreasureLocations.ToList().Any(coordinate => coordinate.Equals(coord)))
            {
                cell.Children.Add(new Image { Source = _dirtImage.Source, Width = size, });
                cell.Children.Add(new Image
                    { Source = _closedTreasureImage.Source, Width = _closedTreasureImage.Width, });
            }
            else if (!c.Coord.Equals(startCoord))
            {
                cell.Children.Add(new Image
                    { Source = c.Visitable ? _dirtImage.Source : _grassImage.Source, Width = size, });
            }
        }
    }

    public async void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        /* Run Time */
        if (!_isNotError || _graph is null || _solutionMatrix is null || _solutionMatrix.TreasureLocations.Count == 0)
        {
            // send alert
            Debug.WriteLine("Belum ada file\n");
            // show dialog alert
            var alert = new DialogWindow("Belum ada file");

            await alert.ShowDialog(this);
            return;
        }

        List<Coordinate>? path;
        List<CompressedState>? states;
        _solutionMatrix.Path.Clear();

        var watch = Stopwatch.StartNew();
        watch.Start();

        if (_bfsRadioButton.IsChecked == true)
        {
            (path, states) = await Task.Run(() => BfsSolver.FindPath(_graph, _solutionMatrix.States.First(),
                _solutionMatrix.TreasureLocations, _tspCheckBox.IsChecked != null && _tspCheckBox.IsChecked.Value));
        }
        else
        {
            (path, states) = await Task.Run(() => DfsSolver.FindPath(_graph, _solutionMatrix.States.First(),
                _solutionMatrix.TreasureLocations, _tspCheckBox.IsChecked != null && _tspCheckBox.IsChecked.Value));
        }

        watch.Stop();
        var elapsedMs = watch.Elapsed;
        _executionTimeLabel.Content = elapsedMs.TotalMilliseconds + " ms";
        _routeTextBlock.Text = string.Join(" ", Utils.ConvertRoute(path!)) + " ";
        _stepsLabel.Content = (path!.Count - 1);
        _nodesLabel.Content = states.Count;

        _solutionMatrix.SetStates(states);
        _solutionMatrix.SetPath(path);
        _mazeSlider.SetValue(RangeBase.MaximumProperty, states.Count);
        _mazeSlider.SetValue(RangeBase.ValueProperty, states.Count);
    }

    private async void MazeSlider_OnPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        _mazeSlider = (sender as Slider)!;
        if (e.Property.Name != "Value" || _solutionMatrix is null ||
            _solutionMatrix.States.Count == 0) return;
        var idx = (int)_mazeSlider.Value;
        ClearCells();

        if (idx >= _solutionMatrix.States.Count)
        {
            RenderPath();
            return;
        }

        var state = _solutionMatrix.States[idx];
        await RenderFootsteps(idx);
        await RenderTreasures(state);

        var loc = state.CurrentLocation;
        await Dispatcher.UIThread.InvokeAsync(() => _cells[loc.Y, loc.X].Children
            .Add(new Image { Source = _doraImage.Source, Height = _doraImage.Height }));
    }

    private async void PlayButton_Click(object sender, RoutedEventArgs e)
    {
        if (_solutionMatrix?.States.Count == 0 ||
            Math.Abs(_mazeSlider.Value - _mazeSlider.Maximum) < double.Epsilon) return;
        _isPlayed = true;

        while (_isPlayed && _mazeSlider.Value < _mazeSlider.Maximum)
        {
            _mazeSlider.Value += 1;
            await Task.Run(() => Thread.Sleep(100));
        }

        _isPlayed = false;
    }

    public void PauseButton_Click(object sender, RoutedEventArgs e)
    {
        _isPlayed = false;
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        _isPlayed = false;
        _mazeSlider.Value = 0;
    }

    private void ClearCells()
    {
        foreach (var p in _cells)
        {
            var filter = p.Children.Where(c =>
                c is not Border && c is Image image && image.Source != _dirtImage.Source &&
                image.Source != _grassImage.Source).ToList();
            foreach (var c in filter)
            {
                p.Children.Remove(c);
            }
        }
    }

    private void RenderPath()
    {
        for (var i = 0; i < _solutionMatrix!.Path.Count; i++)
        {
            var (x, y) = (_solutionMatrix.Path[i].X, _solutionMatrix.Path[i].Y);
            int dir;
            if (i > 0)
            {
                dir = (int)Utils.DetermineDirection(_solutionMatrix.Path[i - 1], _solutionMatrix.Path[i]);
            }
            else
            {
                dir = (int)Utils.DetermineDirection(_solutionMatrix.Path[0], _solutionMatrix.Path[1]);
            }

            _cells[y, x].Children.Add(new Image
            {
                Source = _footsteps.Source,
                RenderTransform = new RotateTransform(90 * dir),
                Width = _footsteps.Width,
                Opacity = .5
            });
        }

        var loc = _solutionMatrix!.States[0].CurrentLocation;
        _cells[loc.Y, loc.X].Children.Add(new Image
        {
            Source = _doraImage.Source,
            Height = _doraImage.Height * .4,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(_doraImage.Height * .04)
        });
        foreach (var treasureLocation in _solutionMatrix!.TreasureLocations)
        {
            _cells[treasureLocation.Y, treasureLocation.X].Children.Add(new Image
            {
                Source = _openedTreasureImage.Source,
                Width = _openedTreasureImage.Width * .5,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(_doraImage.Height * .05)
            });
        }
    }

    private async Task RenderFootsteps(int states)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            for (var i = 0; i < states; i++)
            {
                var st = _solutionMatrix!.States[i];
                var loc = st.CurrentLocation;
                _cells[loc.Y, loc.X].Children.Add(new Image
                {
                    Source = _footsteps.Source,
                    RenderTransform = new RotateTransform(90 * (int)st.Dir),
                    Width = _footsteps.Width,
                    Opacity = .5
                });
            }
        });
    }

    private async Task RenderTreasures(CompressedState state)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
            {
                foreach (var treasureLocation in _solutionMatrix!.TreasureLocations)
                {
                    var (x, y) = (treasureLocation.X, treasureLocation.Y);
                    if (state.IsVisited(treasureLocation) || state.IsSavedVisited(treasureLocation))
                    {
                        _cells[y, x].Children.Add(new Image
                        {
                            Source = _openedTreasureImage.Source,
                            Width = _openedTreasureImage.Width,
                        });
                    }
                    else
                    {
                        _cells[y, x].Children.Add(new Image
                        {
                            Source = _closedTreasureImage.Source,
                            Width = _closedTreasureImage.Width,
                        });
                    }
                }
            }
        );
    }
}
