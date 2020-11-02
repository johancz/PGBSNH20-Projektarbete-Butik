using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Butik_User
{
    public partial class MainWindow : Window
    {
        // UI-elements
        TabControl Tabs;
        Grid ProductsGrid;
        TextBox Logging;
        ListBox ProductListBox;

        // Data
        private List<List> Lists = new List<List>();
        private List<Product> Products = new List<Product>();
        private ShoppingCart ActiveShoppingCart = new ShoppingCart();

        public ListBox ProductsListbox { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            // Window options
            Title = "Dollar Brad Store";
            MinWidth = 600;
            MinHeight = 600;
            Width = System.Windows.SystemParameters.WorkArea.Width - 200;
            Height = System.Windows.SystemParameters.WorkArea.Height - 200;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            CreateUIElements();
            //ProductsListbox = new ListBox();
            //DrawProductListbox();

            //SizeChanged += MainWindow_OnSizeChanged;
            KeyUp += OnKeyUp;
        }

        //private TabItem CreateMockupTabItem(HeaderedContentControl header, ContentControl content)
        private TabItem CreateMockupTabItem(string header, UIElement content)
        {
            var border = new Border
            {
                BorderThickness = new Thickness(0, 1, 0, 0),
                BorderBrush = Brushes.Black,
            };

            var tabItem = new TabItem
            {
                Header = header,
                Content = border
            };
            border.Child = content;

            return tabItem;
        }

        private void CreateUIElements()
        {
            ////////////////////////// [START] REMOVE BEFORE "RELEASE"
            {
                try
                {
                    Background = new SolidColorBrush(GetChromeColor().Value);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Could not create a SolidColorBrush from color created with GetChromeColor()");
                }

            }
            ////////////////////////// [END] REMOVE BEFORE "RELEASE"

            // Root element
            // Tabs
            Tabs = new TabControl();
            Tabs.Padding = new Thickness(-1, 5, -1, -1);

            // Mockup 1
            {
                // Mockup content
                var tabs = new TabControl();
                var tabItem1 = new TabItem
                {
                    Header = "Browse Products"
                };
                var tabItem2 = new TabItem
                {
                    Header = "My Shopping Cart"
                };

                var tabItem1_rootGrid = new Grid { ShowGridLines = true };
                tabItem1_rootGrid.RowDefinitions.Add(new RowDefinition());
                tabItem1_rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
                tabItem1_rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
                tabItem1.Content = tabItem1_rootGrid;

                tabs.Items.Add(tabItem1);
                tabs.Items.Add(tabItem2);

                // Mockup tabitem
                Tabs.Items.Add(CreateMockupTabItem("Mockup 1", tabs));
            }

            // Mockup 2
            {
                var tabItem_mockup2 = new TabItem();
                tabItem_mockup2.Header = "Mockup 2";

                var grid = new Grid
                {
                    ShowGridLines = true,
                };
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                // Mockup tabitem
                Tabs.Items.Add(CreateMockupTabItem("Mockup 2", grid));
            }

            var tabItem_products = new TabItem();
            tabItem_products.Header = "Products";
            var tabItem_shoppingCart = new TabItem();
            tabItem_shoppingCart.Header = "Shopping Cart";

            // "Product"-tabItem grid
            ProductsGrid = new Grid();
            ProductsGrid.Children.Add(new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1)
            });
            ProductsGrid.Margin = new Thickness(5);
            ProductsGrid.RowDefinitions.Add(new RowDefinition());
            ProductsGrid.RowDefinitions.Add(new RowDefinition());
            //grid_products.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            ProductsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ProductsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            tabItem_products.Content = ProductsGrid;
            ProductsGrid.HorizontalAlignment = HorizontalAlignment.Stretch;

            var searchStackPanel = new DockPanel
            {
                //HorizontalAlignment = HorizontalAlignment.Stretch,
                //padd
                LastChildFill = true
            };
            var searchTextbox = new TextBox
            {
                Name = "SearchTextBox",
                Background = Brushes.AliceBlue,
                VerticalAlignment = VerticalAlignment.Top,
                //HorizontalAlignment = HorizontalAlignment.Left,
                FontSize = 14,
                Padding = new Thickness(5),
                //HorizontalAlignment = HorizontalAlignment.Stretch
            };
            searchTextbox.TextChanged += SearchTextChanged;
            var searchLabel = new Label
            {
                Content = "Search",
                FontSize = 14,
                Target = searchTextbox
            };
            searchLabel.Tag = searchTextbox;
            searchLabel.MouseUp += (object sender, MouseButtonEventArgs e) =>
            {
                if (((Label)sender).Target != null)
                {
                    ((Label)sender).Target.Focus();
                }
            };
            searchStackPanel.Children.Add(searchLabel);
            searchStackPanel.Children.Add(searchTextbox);
            AddControlToGrid(ProductsGrid, searchStackPanel, 0, 0);

            // temporary logging
            Logging = new TextBox
            {
                Background = Brushes.AliceBlue,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            AddControlToGrid(ProductsGrid, Logging, 0, 1);

            /////////////////////////// START OF PLACEHOLDER CONTENT //////////////////////////////////
            {
                var listbox = new ListBox();
                var stackPanel0 = new StackPanel();
                stackPanel0.Orientation = Orientation.Horizontal;
                var uri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative);
                var bmi = new BitmapImage(uri);
                var image = new Image
                {
                    Source = bmi,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5),
                    Stretch = Stretch.Uniform,
                    Height = 50,
                    ToolTip = new ToolTip
                    {
                        Content = new Image
                        {
                            Source = bmi,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(5),
                            Stretch = Stretch.Uniform,
                        }
                    }
                };

                stackPanel0.Children.Add(image);
                //AddImageToGrid(grid_products, image, 1, 1);
                ///////////// REPLACE WITH DATAGRID? //////////////////////////////////////////////////////////////////////////////////
                var stackPanelItem = new StackPanel { Orientation = Orientation.Vertical };
                stackPanelItem.Children.Add(new Label() { Content = "Product name blabla" });
                stackPanelItem.Children.Add(new Label() { Content = "Product description blablabblalbla bla blabla" });
                stackPanel0.Children.Add(stackPanelItem);
                stackPanel0.Children.Add(new Label() { Content = "2,50 kr", FontSize = 30 });
                stackPanel0.Children.Add(new Label() { Content = "+", FontSize = 30 });

                listbox.Items.Add(new ListBoxItem() { Content = stackPanel0 });
                listbox.Items.Add(new ListBoxItem() { Content = stackPanel0 });
                listbox.Items.Add(new ListBoxItem() { Content = stackPanel0 });
                listbox.Items.Add(new ListBoxItem() { Content = stackPanel0 });
                AddControlToGrid(ProductsGrid, listbox, 1, 0);

                // "Shopping Cart"-tabItem grid
                Grid grid_shoppingCart = new Grid();
                grid_shoppingCart.Margin = new Thickness(5);
                grid_shoppingCart.RowDefinitions.Add(new RowDefinition());
                grid_shoppingCart.ColumnDefinitions.Add(new ColumnDefinition());
                tabItem_shoppingCart.Content = grid_shoppingCart;
            }
            /////////////////////////// END OF PLACEHOLDER CONTENT //////////////////////////////////

            Tabs.Items.Add(tabItem_products);
            Tabs.Items.Add(tabItem_shoppingCart);
            Content = Tabs;
        }

        private void DrawProductListbox()
        {
            foreach (Product product in Products)
            {
                var listBoxItem = CreateListboxItem(product);

                if (listBoxItem != null)
                {
                    ProductsListbox.Items.Add(listBoxItem);
                }
            }
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO(johancz): only show products that match the search query
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Logging.Text = $"window width: {e.NewSize.Width}\nwindow height: {e.NewSize.Height}";
        }

        private void AddControlToGrid(Grid grid, UIElement control, int row, int column)
        {
            Grid.SetRow(control, row);
            Grid.SetColumn(control, column);
            grid.Children.Add(control);
        }

        private StackPanel Mockup1_CreateProductGridItem(Product product)
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,

            };

            return stackPanel;
        }

        private Image CreateNewImage(string uriString, int height, string tooltipText = null, bool largerImageInTooltip = false)
        {
            Image image = new Image();
            try
            {
                var uri = new Uri(uriString, UriKind.Relative);
                image.Source = new BitmapImage(uri);
                image.HorizontalAlignment = HorizontalAlignment.Center;
                image.VerticalAlignment = VerticalAlignment.Center;
                image.Margin = new Thickness(5);
                image.Stretch = Stretch.Uniform;
                image.Height = 50;

                if (tooltipText != null || largerImageInTooltip != false)
                {
                    var tooltipStackpanel = new StackPanel { Orientation = Orientation.Vertical };
                    image.ToolTip = tooltipStackpanel;

                    if (tooltipText != null)
                    {
                        tooltipStackpanel.Children.Add(new Label
                        {
                            Content = tooltipText
                        });
                    }

                    if (largerImageInTooltip != false)
                    {
                        tooltipStackpanel.Children.Add(new Image
                        {
                            Source = new BitmapImage(uri),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(5),
                            Stretch = Stretch.Uniform,
                        });
                    }
                }
            }
            catch (Exception e)
            {
                // TODO(johancz): exception-handling
            }

            return image;
        }

        private ListBoxItem? CreateListboxItem(Product product)
        {
            ///////////// REPLACE WITH DATAGRID? //////////////////////////////////////////////////////////////////////////////////
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            Image image = CreateNewImage("/Images/broccoli-1238250_640.jpg", 50, null, true);
            //Image image = new Image();
            //try
            //{
            //    var uri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative);
            //    var bmi = new BitmapImage(uri);

            //    image.Source = new BitmapImage(uri);
            //    image.HorizontalAlignment = HorizontalAlignment.Center;
            //    image.VerticalAlignment = VerticalAlignment.Center;
            //    image.Margin = new Thickness(5);
            //    image.Stretch = Stretch.Uniform;
            //    image.Height = 50;
            //    image.ToolTip = new ToolTip
            //    {
            //        Content = new Image
            //        {
            //            Source = bmi,
            //            HorizontalAlignment = HorizontalAlignment.Center,
            //            VerticalAlignment = VerticalAlignment.Center,
            //            Margin = new Thickness(5),
            //            Stretch = Stretch.Uniform,
            //        }
            //    };
            //}
            //catch (Exception e)
            //{
            //    // TODO(johancz): exception-handling
            //}

            stackPanel.Children.Add(image);
            ///////////// REPLACE WITH DATAGRID? //////////////////////////////////////////////////////////////////////////////////
            var stackPanelNameDesciption = new StackPanel { Orientation = Orientation.Vertical };
            stackPanelNameDesciption.Children.Add(new Label() { Content = product.Name });
            stackPanelNameDesciption.Children.Add(new Label() { Content = product.Description });
            stackPanel.Children.Add(stackPanelNameDesciption);
            stackPanel.Children.Add(new Label() { Content = $"{product.Price} kr", FontSize = 30 });
            stackPanel.Children.Add(new Label() { Content = "+", FontSize = 30 });

            // TODO(johancz): return null if the image can't be created?
            // TODO(johancz): return null if the product lacks data?
            return new ListBoxItem() { Content = stackPanel };
        }

        private void AddImageToGrid(Grid grid, Image image, int row, int column)
        {
            Grid.SetRow(image, row);
            Grid.SetColumn(image, column);
            grid.Children.Add(image);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        ////////////////////////// [START] REMOVE BEFORE "RELEASE"
        public static Color? GetChromeColor()
        {
            bool isEnabled;
            var hr1 = DwmIsCompositionEnabled(out isEnabled);
            if ((hr1 != 0) || !isEnabled) // 0 means S_OK.
                return null;

            DWMCOLORIZATIONPARAMS parameters;
            try
            {
                // This API is undocumented and so may become unusable in future versions of OSes.
                var hr2 = DwmGetColorizationParameters(out parameters);
                if (hr2 != 0) // 0 means S_OK.
                    return null;
            }
            catch
            {
                return null;
            }

            // Convert colorization color parameter to Color ignoring alpha channel.
            var targetColor = Color.FromRgb(
                (byte)(parameters.colorizationColor >> 16),
                (byte)(parameters.colorizationColor >> 8),
                (byte)parameters.colorizationColor);

            // Prepare base gray color.
            var baseColor = Color.FromRgb(217, 217, 217);

            // Blend the two colors using colorization color balance parameter.
            return BlendColor(targetColor, baseColor, (double)(100 - parameters.colorizationColorBalance));
        }

        private static Color BlendColor(Color color1, Color color2, double color2Perc)
        {
            if ((color2Perc < 0) || (100 < color2Perc))
                throw new ArgumentOutOfRangeException("color2Perc");

            return Color.FromRgb(
                BlendColorChannel(color1.R, color2.R, color2Perc),
                BlendColorChannel(color1.G, color2.G, color2Perc),
                BlendColorChannel(color1.B, color2.B, color2Perc));
        }

        private static byte BlendColorChannel(double channel1, double channel2, double channel2Perc)
        {
            var buff = channel1 + (channel2 - channel1) * channel2Perc / 100D;
            return Math.Min((byte)Math.Round(buff), (byte)255);
        }

        [DllImport("Dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled([MarshalAs(UnmanagedType.Bool)] out bool pfEnabled);

        [DllImport("Dwmapi.dll", EntryPoint = "#127")] // Undocumented API
        private static extern int DwmGetColorizationParameters(out DWMCOLORIZATIONPARAMS parameters);

        [StructLayout(LayoutKind.Sequential)]
        private struct DWMCOLORIZATIONPARAMS
        {
            public uint colorizationColor;
            public uint colorizationAfterglow;
            public uint colorizationColorBalance; // Ranging from 0 to 100
            public uint colorizationAfterglowBalance;
            public uint colorizationBlurBalance;
            public uint colorizationGlassReflectionIntensity;
            public uint colorizationOpaqueBlend;
        }
        ////////////////////////// [END] REMOVE BEFORE "RELEASE"
    }
}
