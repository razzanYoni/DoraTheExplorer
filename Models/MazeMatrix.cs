using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using System;

namespace DoraTheExplorer.Models;

public class MazeMatrix : Grid, IStyleable
{
   Type IStyleable.StyleKey => typeof(Grid);

    // public MazeMatrix(int row = 10, int col = 10)
    public MazeMatrix()
    { 
        int row = 10;
        int col = 10;
        for (int i = 0; i < row; i++)
        {
            RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
        }
        for (int i = 0; i < col; i++)
        {
            ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
        }

        Background = Brushes.White;
        
        for(int i = 0;i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                var cell = new MazeCell();
                cell.SetValue(Grid.RowProperty, i);
                cell.SetValue(Grid.ColumnProperty, j);
                if ((j % 2 == 0) && (i % 2 != 0))
                {
                    cell.TypeOfCell = MazeCell.CellType.Wall;
                    cell.SetValue(Grid.BackgroundProperty, Brushes.Black);
                }
                Children.Add(cell);
            }
        }

    }
}