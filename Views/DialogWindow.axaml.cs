using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace DoraTheExplorer.Views;

public partial class DialogWindow : Window
{
    public DialogWindow() { }

    public DialogWindow(string message)
    {
        InitializeComponent();
        var textBlock = this.FindControl<TextBlock>("MessageTextBlock");
        textBlock.Text = message;
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
        Close();
    }
}