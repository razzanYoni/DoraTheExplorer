using Avalonia.Controls;
using Avalonia.Styling;
using System;

namespace DoraTheExplorer.Models;

public class MazeCell : Grid, IStyleable
{

    public enum CellType
    {
        Start = -3,
        Space = 0,
        Wall = -1,
        Treasure = -999,
        undefined = -9999,
    }

    public CellType _cellType;
    public CellType  TypeOfCell{ 
        get { return _cellType; } 
        set { _cellType = value;} }

    public MazeCell()
    {
        _cellType = CellType.undefined;
    }
}