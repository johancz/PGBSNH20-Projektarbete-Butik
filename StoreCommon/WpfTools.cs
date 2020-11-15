using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace StoreCommon
{
    public static class WpfTools
    {
        public static void SetWindow(Window mainWindow)
        {

        }
        public static void AddToGrid(Grid grid, UIElement element, int row, int column)
        {
            grid.Children.Add(element);
            Grid.SetColumn(element, column);
            Grid.SetRow(element, row);
        }
        public static RowDefinition AddRow(Grid grid)
        {
            var rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions.Add(rowDefinition);
            return rowDefinition;
        }
        public static ColumnDefinition AddColumn(Grid grid)
        {
            var columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength();
            grid.ColumnDefinitions.Add(columnDefinition);
            return columnDefinition;
        }
        public static void ColumnsAndRows(Grid grid, int columns, int rows)
        {
            for (int i = 0; i < columns; i++)
            {
                AddColumn(grid);
            }
            for (int i = 0; i < rows; i++)
            {
                AddRow(grid);
            }
        }
    }
}
