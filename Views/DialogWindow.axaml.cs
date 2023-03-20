using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Tubes2_DoraTheExplorer.Views;

public partial class DialogWindow : Window
{
    private string _message;
    
    public DialogWindow()
    {
        InitializeComponent();
    }
    public DialogWindow(string message)
    {
        InitializeComponent();
        _message = message;
        var textBlock = this.FindControl<TextBlock>("MessageTextBlock");
        textBlock.Text = _message;
        var alertButton = this.FindControl<Button>("AlertButton");
        alertButton.Click += AlertButton_OnClick;
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void AlertButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}