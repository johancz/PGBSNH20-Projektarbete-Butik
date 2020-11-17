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
        public static DetailsPanel detailsPanel;
        public static Browser _browser;
        public static List<BrowserItem> ProductBrowserItems = new List<BrowserItem>();
        public static List<BrowserItem> ImageBrowserItems = new List<BrowserItem>();
        public static List<FrameworkElement> Elements = new List<FrameworkElement>();
        public static bool AddImage = false;

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
        public Image ImageThumbnail;
        public BrowserItem(WrapPanel parent)
        {
            Parent = parent;          

            var itemGrid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Top,
                Width = ProductItem_LayoutSettings.gridItemWidth,
                Height = ProductItem_LayoutSettings.gridItemHeight,
                Margin = new Thickness(5),
                Background = Brushes.LightGray,
            };

            ItemGrid = itemGrid;
        }
        public void LoadImageBrowserItem(string filePath)
        {
            var productThumbnail = Helpers.CreateNewImage(filePath, ProductItem_LayoutSettings.gridItemImageHeight);
            ImageThumbnail = productThumbnail;
            productThumbnail.Stretch = Stretch.UniformToFill;
            productThumbnail.VerticalAlignment = VerticalAlignment.Center;
            productThumbnail.HorizontalAlignment = HorizontalAlignment.Center;
            ItemGrid.Children.Add(productThumbnail);
            ImageBrowserItems.Add(this);

            ItemGrid.MouseUp += ImageItemGrid_MouseUp1;
        }

        private void ImageItemGrid_MouseUp1(object sender, MouseButtonEventArgs e)
        {
            var itemGrid = (Grid)sender;
            var itemImage = (Image)(itemGrid.Children[0]);
            var source = itemImage.Source;

            var displayImage = ((Image)GetElement("rightcolumn detailsimage"));
            displayImage.Source = source;
        }

        public void LoadProductBrowserItem(Product product)
        {
            Parent.Children.Add(ItemGrid);
            ItemGrid.Tag = product;
            var productThumbnail = Helpers.CreateNewImage(product.Uri, ProductItem_LayoutSettings.gridItemImageHeight);
            ImageThumbnail = productThumbnail;
            productThumbnail.Stretch = Stretch.UniformToFill;
            productThumbnail.VerticalAlignment = VerticalAlignment.Center;
            productThumbnail.HorizontalAlignment = HorizontalAlignment.Center;
            ItemGrid.Children.Add(productThumbnail);
            Grid.SetColumnSpan(ImageThumbnail, 2);

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
            ItemGrid.ToolTip = tooltip;

            ItemGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ItemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            ItemGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            ItemGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });            

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
            ProductBrowserItems.Add(this);
            ItemGrid.MouseUp += ProductItemGrid_MouseUp;
        }        
        public void RefreshProductContent()
        {
            var product = ((Product)(ItemGrid.Tag));
            var thumbNail = ImageThumbnail;
            var nameLabel = (Label)(ItemGrid.Children[1]);
            nameLabel.Content = product.Name;
            var priceLabel = (Label)(ItemGrid.Children[2]);
            priceLabel.Content = $"{product.Price} kr";
        }
        public void ProductItemGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SelectedProduct = (Product)(ItemGrid.Tag);
            detailsPanel.Update();
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
       
        public Browser(Grid parent)
        {
            Parent = parent;           
            _browser = this;

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
        public void LoadBrowserItems()
        {
            foreach (var product in Store.Products)
            {
                var newProductItem = new BrowserItem(BrowserWrapPanel);
                newProductItem.LoadProductBrowserItem(product);
            }
        }
        public void SwitchContent()
        {
            var changeButton = (Button)GetElement("change image");
            var editButton = (Button)GetElement("edit");
            var removeButton = (Button)GetElement("remove");
            var saveButton = (Button)GetElement("save changes");
            var buttonParent = (StackPanel)(changeButton.Parent);
            changeButton.Width = changeButton.ActualWidth;

            if (!AddImage)
            {
                AddImage = true;
                foreach (var productItem in ProductBrowserItems)
                {
                    BrowserWrapPanel.Children.Remove(productItem.ItemGrid);
                }
                foreach (var imageItem in ImageBrowserItems)
                {
                    BrowserWrapPanel.Children.Add(imageItem.ItemGrid);
                }
                changeButton.Content = new Label { Content = "Select Image", HorizontalAlignment = HorizontalAlignment.Left };
                buttonParent.Children.Remove(editButton);
                buttonParent.Children.Remove(removeButton);
                buttonParent.Children.Remove(saveButton);
            }
            else
            {
                AddImage = false;

                foreach (var productItem in ProductBrowserItems)
                {
                    BrowserWrapPanel.Children.Add(productItem.ItemGrid);
                }
                foreach (var imageItem in ImageBrowserItems)
                {
                    BrowserWrapPanel.Children.Remove(imageItem.ItemGrid);
                }
                changeButton.Content = new Label { Content = "Change Image", HorizontalAlignment = HorizontalAlignment.Left };
                buttonParent.Children.Add(editButton);
                buttonParent.Children.Add(removeButton);
                buttonParent.Children.Add(saveButton);
            }
        }
        public void LoadBrowserImages()
        {
            foreach (var filePath in Store.ImageItemFilePaths)
            {
                var newProductItem = new BrowserItem(BrowserWrapPanel);
                newProductItem.LoadImageBrowserItem(filePath);
            }
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
