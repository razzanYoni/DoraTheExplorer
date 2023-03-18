using System;
using System.Diagnostics;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Logging;
using ReactiveUI;
using DoraTheExplorer.ViewModels;
using DoraTheExplorer.Algorithm;
using DoraTheExplorer.Util;
namespace DoraTheExplorer.Views;

// TODO : Execution Time, Steps, Route
// TODO : Visualisasi Maze
// TODO : Slider

public partial class MainWindow : Window
{
    public RadioButton bfsRadioButton;
    public RadioButton dfsRadioButton;
    public CheckBox tspCheckBox;

    public Label fileNameLabel;
    public Button browseFileButton;
    public Button visualizeButton;
    public Button searchButton;

    public Label executionTimeLabel;
    public Label stepsLabel;
    public Label routeLabel;

    public MainWindow()
    {
        // var algorithmChecker = this.DataContext as ViewModelBase;
        // Current folder
        // System.Diagnostics.Debug.WriteLine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location));
        
        // Logger.TryGet(LogEventLevel.Fatal, LogArea.Control)?.Log(this, "Avalonia Infrastructure");
        // searchButton.Command = ReactiveCommand.Create(() => viewModel.SearchButton_Click()); // bound to ViewModelBase
        // searchButton.Command = ReactiveCommand.Create(() => Console.WriteLine("Hello World!")); bound to ViewModelBase
        
        InitializeComponent();
        
        this.bfsRadioButton = this.FindControl<RadioButton>("BfsRadioButton");
        this.bfsRadioButton.SetValue(ToggleButton.IsCheckedProperty, true);

        this.dfsRadioButton = this.FindControl<RadioButton>("DfsRadioButton");

        this.tspCheckBox = this.FindControl<CheckBox>("TspCheckBox");

        this.fileNameLabel = this.FindControl<Label>("FileNameLabel");
        this.fileNameLabel.Content = "No File Selected";

        this.browseFileButton = this.FindControl<Button>("BrowseFileButton");
        this.browseFileButton.Click += BrowseFileButton_Click;

        this.visualizeButton = this.FindControl<Button>("VisualizeButton");
        this.visualizeButton.Click += VisualizeButton_Click;
        
        this.searchButton = this.FindControl<Button>("SearchButton");
        this.searchButton.Click += SearchButton_Click;

        this.executionTimeLabel = this.FindControl<Label>("ExecutionTimeLabel");
        this.executionTimeLabel.Content = "Execution Time : 00 ms";

        this.stepsLabel = this.FindControl<Label>("StepsLabel");
        this.stepsLabel.Content = "Steps : 0";

        this.routeLabel = this.FindControl<Label>("RouteLabel");
        // dummy
        this.routeLabel.Content = "Route : L-U-R-U-R-R\n" +
                                           "U-R-D-D-D-D-D\n";
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

            /* Read File */
            var readFromFile = new ReadFromFile();
            var (matrix, error) = readFromFile.ReadFile(result.Result[0]);
            if (!error)
            {
                // file error alert in window
                System.Diagnostics.Debug.WriteLine("File Error");
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
    }
    
    public void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        /* Run Time */

        if (this.tspCheckBox.IsChecked == true)
        {
            if (this.bfsRadioButton.IsChecked == true)
            {
                System.Diagnostics.Debug.WriteLine("Eksekusi bfs dengan tsp \n");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Eksekusi dfs dengan tsp \n");
            }
        }
        else
        {
            if (this.bfsRadioButton.IsChecked == true)
            {
                System.Diagnostics.Debug.WriteLine("Eksekusi bfs tanpa tsp \n");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Eksekusi dfs tanpa tsp \n");
            }
        }
    }
}
