// using DoraTheExplorer.Models;
using System;
using System.Drawing;
using Avalonia.Media.Imaging;
using Avalonia.Shared.PlatformSupport;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace DoraTheExplorer.ViewModels;

public class MainWindowViewModel
{
    private Bitmap? _icon;
    public MainWindowViewModel()
    {
    }
    
    /*
     * Initialize icon
     */
    // public Avalonia.Media.Imaging.Bitmap Icon { get; } = new Bitmap(new Uri("avares://DoraTheExplorer/Assets/Icon.png"));
}
