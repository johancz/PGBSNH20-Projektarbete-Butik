using StoreCommon;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreCommon
{
    public abstract class CommonFrameWork
    {
        public static List<Page> Pages = new List<Page>();
        public static List<DetailsPanel> detailsPanels = new List<DetailsPanel>();
        public static List<BrowserItem> browserItems = new List<BrowserItem>();
        public static List<FrameworkElement> Elements = new List<FrameworkElement>();

        public static Product SelectedProduct;
        public static object GetElement(string _tag)
        {
            try
            {
                return Elements.Find(x => (string)(x.Tag) == _tag);
            }
            catch (System.Exception ex)
            {
                return ex;
            }
        }
    }
    public class BrowserItem : CommonFrameWork
    {
        public WrapPanel Parent;
        public Grid ItemGrid;
        public Product _product;
        public BrowserItem(WrapPanel parent, Product product)
        { 
            Parent = parent;
            _product = product;

            var tooltip = new ToolTip
            {
                Placement = PlacementMode.Mouse,
                MaxWidth = 800,
                Content = new TextBlock
                {
                    Text = $"{product.Name}\n{product.Description}\n",
                    TextWrapping = TextWrapping.Wrap,
                }
            };

            var itemGrid = new Grid
            {
                Tag = product,
                VerticalAlignment = VerticalAlignment.Top,
                Width = ProductItem_LayoutSettings.gridItemWidth,
                Height = ProductItem_LayoutSettings.gridItemHeight,
                Margin = new Thickness(5),
                ToolTip = tooltip,
                Background = Brushes.LightGray,
            };

            Parent.Children.Add(itemGrid);
            ItemGrid = itemGrid;

            ItemGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ItemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            ItemGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            ItemGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            var productThumbnail = Helpers.CreateNewImage(product.Uri, ProductItem_LayoutSettings.gridItemImageHeight);
            productThumbnail.Stretch = Stretch.UniformToFill;
            productThumbnail.VerticalAlignment = VerticalAlignment.Center;
            productThumbnail.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetColumnSpan(productThumbnail, 2);
            ItemGrid.Children.Add(productThumbnail);

            var nameLabel = new Label
            {
                Content = product.Name,
                FontSize = 14,
            };
            Grid.SetColumn(nameLabel, 0);
            Grid.SetRow(nameLabel, 1);
            ItemGrid.Children.Add(nameLabel);

            var priceLabel = new Label
            {
                Content = $"{product.Price} kr",
            };
            Grid.SetColumn(priceLabel, 1);
            Grid.SetRow(priceLabel, 1);
            ItemGrid.Children.Add(priceLabel);

            Elements.Add(ItemGrid);
            browserItems.Add(this);
            ItemGrid.MouseUp += ItemGrid_MouseUp;
        }
        public void ItemGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SelectedProduct = (Product)(ItemGrid.Tag);
            detailsPanels.Find(x => x.Tag == "edit panel").Update();
        }

        internal struct ProductItem_LayoutSettings
        {
            internal const double gridItemWidth = 200;
            internal const double gridItemHeight = 200;
            internal const int gridItemImageHeight = 175;
        }
    }
    public class Browser : CommonFrameWork
    {
        public WrapPanel BrowserWrapPanel;
        public Grid Parent;
        public ScrollViewer ThisScrollViewer;
        public SizeChangedEventHandler sizeChangedEventHandler;
        public Browser(Grid parent)
        {
            Parent = parent;           

            var scrollViewer = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            ThisScrollViewer = scrollViewer;

            parent.Children.Add(scrollViewer);
            Grid.SetColumn(scrollViewer, 0);

            var productPanel = new WrapPanel
            {
                Tag = "product panel",
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = Brushes.LightBlue,
                VerticalAlignment = VerticalAlignment.Top
            };
            Elements.Add(productPanel);

            scrollViewer.Content = productPanel;
            BrowserWrapPanel = productPanel;
            scrollViewer.SizeChanged += ScrollViewer_SizeChanged;
        }

        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BrowserWrapPanel.Width = ThisScrollViewer.ActualWidth;   
        }
    }
    public class HybridPage : CommonFrameWork
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
    public class HybridAppWindow
    {
        private Window _mainWindow;
        public readonly TabControl tabControl;
        public HybridAppWindow(Window mainWindow, string title, Brush brush)
        {
            _mainWindow = mainWindow;
            WindowSettings(title);
            var _tabcontrol = new TabControl();
            _mainWindow.Content = _tabcontrol;
            tabControl = _tabcontrol;
            _tabcontrol.Background = brush;

            _mainWindow.KeyUp += MainWindow_KeyUp;
        }
        private void WindowSettings(string title)
        {
            _mainWindow.Title = title;
            _mainWindow.Width = SystemParameters.WorkArea.Width >= 1000 ? SystemParameters.WorkArea.Width - 200 : 800;
            _mainWindow.Height = SystemParameters.WorkArea.Height >= 800 ? SystemParameters.WorkArea.Height - 200 : 600;
            _mainWindow.MinWidth = 800;
            _mainWindow.MinHeight = 600;
            _mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }
    }

}
