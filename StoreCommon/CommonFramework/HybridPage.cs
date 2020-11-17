using StoreCommon;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreCommon
{
    public class HybridPage : CommonFramework
    {
        public Grid grid;
        public TabControl Parent;
        public TabItem ThisTab;
        public string Tag;
        public HybridPage(TabControl parent, string header, Brush brush)
        {
            Parent = parent;
            var thisTab = new TabItem { Header = header };
            Tag = header;
            parent.Items.Add(thisTab);
            ThisTab = thisTab;

            grid = new Grid { ShowGridLines = true, Background = brush };
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            thisTab.Content = grid;

        }
    }
}
