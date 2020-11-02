using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            Data.Init();

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
            KeyUp += MainWindow_KeyUp;
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
                    Background = new SolidColorBrush(TEMPORARY_AND_PLACEHOLDER_STUFF.GetChromeColor().Value);
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

            // Mockup 0 ///////////////////////////////////////////////////////////////////
            {
                // Mockup content
                var tabs = new TabControl();
                var tabItem0 = new TabItem
                {
                    Header = "Browse Products"
                };
                var tabItem1 = new TabItem
                {
                    Header = "My Shopping Cart"
                };

                var tabItem0_rootGrid = new Grid { ShowGridLines = true };
                tabItem0_rootGrid.RowDefinitions.Add(new RowDefinition());
                tabItem0_rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
                tabItem0_rootGrid.ColumnDefinitions.Add(new ColumnDefinition());

                var tabItem1_rootGrid = new Grid { ShowGridLines = true };
                tabItem1_rootGrid.RowDefinitions.Add(new RowDefinition());
                tabItem1_rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
                tabItem1_rootGrid.ColumnDefinitions.Add(new ColumnDefinition());

                // Column 0: grid items
                {
                    //var productsGrid = new Grid { ShowGridLines = true };
                    var productsPanel = new WrapPanel();

                    foreach (Product product in Data.Products)
                    {
                        var gridItem = Mockup0.CreateProductGridItem(product);

                        if (gridItem != null)
                        {
                            productsPanel.Children.Add(gridItem);
                        }
                    }

                    Grid.SetColumn(productsPanel, 0);
                    Grid.SetRow(productsPanel, 0);
                    tabItem0_rootGrid.Children.Add(productsPanel);
                }

                // Column 1: right column width product image (large), name, description, price and buttons for adding (and removing?) the product to the shopping cart
                {
                    //var productsGrid = new Grid { ShowGridLines = true };
                    Mockup0.RightColumn = new StackPanel { Orientation = Orientation.Vertical };

                    // TODO(johancz): change to image and split Helpers.CreateNewImage into smaller parts:
                    Mockup0.RightColumn.Children.Add(Helpers.CreateNewImage());
                    var detailsPanel = new StackPanel { Orientation = Orientation.Vertical };
                    Mockup0.RightColumn.Children.Add(detailsPanel);


                    Grid.SetColumn(Mockup0.RightColumn, 1);
                    Grid.SetRow(Mockup0.RightColumn, 0);
                    tabItem0_rootGrid.Children.Add(Mockup0.RightColumn);
                }

                tabItem0.Content = tabItem0_rootGrid;
                tabItem1.Content = tabItem1_rootGrid;

                tabs.Items.Add(tabItem0);
                tabs.Items.Add(tabItem1);
                // Mockup tabitem
                Tabs.Items.Add(CreateMockupTabItem("Mockup 0", tabs));
            }

            // Mockup 1 ///////////////////////////////////////////////////////////////////
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
                Tabs.Items.Add(CreateMockupTabItem("Mockup 1", grid));
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
                //var listbox = new ListBox();
                //var stackPanel0 = new StackPanel();
                //stackPanel0.Orientation = Orientation.Horizontal;
                //var uri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative);
                //var bmi = new BitmapImage(uri);
                //var image = new Image
                //{
                //    Source = bmi,
                //    HorizontalAlignment = HorizontalAlignment.Center,
                //    VerticalAlignment = VerticalAlignment.Center,
                //    Margin = new Thickness(5),
                //    Stretch = Stretch.Uniform,
                //    Height = 50,
                //    ToolTip = new ToolTip
                //    {
                //        Content = new Image
                //        {
                //            Source = bmi,
                //            HorizontalAlignment = HorizontalAlignment.Center,
                //            VerticalAlignment = VerticalAlignment.Center,
                //            Margin = new Thickness(5),
                //            Stretch = Stretch.Uniform,
                //        }
                //    }
                //};

                //stackPanel0.Children.Add(image);
                ////AddImageToGrid(grid_products, image, 1, 1);
                /////////////// REPLACE WITH DATAGRID? //////////////////////////////////////////////////////////////////////////////////
                //var stackPanelItem = new StackPanel { Orientation = Orientation.Vertical };
                //stackPanelItem.Children.Add(new Label() { Content = "Product name blabla" });
                //stackPanelItem.Children.Add(new Label() { Content = "Product description blablabblalbla bla blabla" });
                //stackPanel0.Children.Add(stackPanelItem);
                //stackPanel0.Children.Add(new Label() { Content = "2,50 kr", FontSize = 30 });
                //stackPanel0.Children.Add(new Label() { Content = "+", FontSize = 30 });

                //listbox.Items.Add(new ListBoxItem() { Content = stackPanel0 });
                //listbox.Items.Add(new ListBoxItem() { Content = stackPanel0 });
                //listbox.Items.Add(new ListBoxItem() { Content = stackPanel0 });
                //listbox.Items.Add(new ListBoxItem() { Content = stackPanel0 });
                //AddControlToGrid(ProductsGrid, listbox, 1, 0);

                //// "Shopping Cart"-tabItem grid
                //Grid grid_shoppingCart = new Grid();
                //grid_shoppingCart.Margin = new Thickness(5);
                //grid_shoppingCart.RowDefinitions.Add(new RowDefinition());
                //grid_shoppingCart.ColumnDefinitions.Add(new ColumnDefinition());
                //tabItem_shoppingCart.Content = grid_shoppingCart;
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

        private ListBoxItem? CreateListboxItem(Product product)
        {
            ///////////// REPLACE WITH DATAGRID? //////////////////////////////////////////////////////////////////////////////////
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            Image image = Helpers.CreateNewImage("/Images/broccoli-1238250_640.jpg", 50, null, true);

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

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }
    }

    public static class Data
    {
        public static List<Product> Products { get; set; } = new List<Product>();

        public static void Init()
        {
            Data.Products = TEMPORARY_AND_PLACEHOLDER_STUFF.CreatePlaceHolderProducts();
        }
    }

    public static class Helpers
    {
        public static BitmapImage? CreateBitmapImageFromUriString(string uriString)
        {
            try
            {
                var uri = new Uri(uriString, UriKind.Relative);
                var bitMapImage = new BitmapImage(uri);

                return bitMapImage;
            }
            catch (Exception e)
            {
                // TODO(johancz): exception-handling
                return null;
            }
        }
        public static Image CreateNewImage(string uriString = null,
                                           int? height = null,
                                           string tooltipText = null,
                                           bool imageInTooltip = false)
        {
            Image image = new Image();

            try
            {
                if (uriString != null)
                {
                    image.Source = CreateBitmapImageFromUriString(uriString);
                }
                image.HorizontalAlignment = HorizontalAlignment.Center;
                image.VerticalAlignment = VerticalAlignment.Center;
                image.Margin = new Thickness(5);
                image.Stretch = Stretch.Uniform;

                if (height != null)
                {
                    image.Height = height.Value;
                }

                if (tooltipText != null || imageInTooltip != false)
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

                    if (imageInTooltip != false)
                    {
                        tooltipStackpanel.Children.Add(new Image
                        {
                            Source = CreateBitmapImageFromUriString(uriString),
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
    }

    public static class Mockup0
    {
        public static UIElement RootElement { get; private set; }
        private static WrapPanel LeftColumn;
        public static StackPanel RightColumn;
        private static Image RightColumn_DetailsImage;
        private static Label RightColumn_DetailsName;
        private static Label RightColumn_DetailsDescription;
        private static Button RightColumn_DetailsAddToCartButton;

        public static void Create()
        {
            // Right/Details Column
            //RightColumn = new WrapPanel();
            //LeftColumn_ = new Image();
            //LeftColumn_ = new Label();
            //LeftColumn_ = new Label();
            //LeftColumn_ = new Button();

            // Right/Details Column
            //RightColumn = new StackPanel();
            RightColumn_DetailsImage = new Image();
            var rightColumn_DetailsPanel = new StackPanel();
            RightColumn_DetailsName = new Label();
            RightColumn_DetailsDescription = new Label();
            RightColumn_DetailsAddToCartButton = new Button { FontSize = 14, Content = "(+) Add to shopping cart" };

            // TODO(johancz): replace mockup 0 code in MainWindow.CreateUIElements with this class.

            rightColumn_DetailsPanel.Children.Add(RightColumn_DetailsName);
            rightColumn_DetailsPanel.Children.Add(RightColumn_DetailsDescription);
            rightColumn_DetailsPanel.Children.Add(RightColumn_DetailsAddToCartButton);

            RightColumn.Children.Add(RightColumn_DetailsImage);
            RightColumn.Children.Add(rightColumn_DetailsPanel);
        }

        public static StackPanel CreateProductGridItem(Product product)
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Tag = product
            };
            stackPanel.Children.Add(Helpers.CreateNewImage(product.ImageUri.ToString(), 25));
            stackPanel.Children.Add(new StackPanel() { Orientation = Orientation.Horizontal });
            ((StackPanel)stackPanel.Children[1]).Children.Add(new Label { Content = product.Name });
            ((StackPanel)stackPanel.Children[1]).Children.Add(new Label { Content = $"{product.Price} kr" });
            //var stackPanel2 = new StackPanel() { Orientation = Orientation.Horizontal };
            //stackPanel2.Children.Add(new Label { Content = product.Name });
            //stackPanel2.Children.Add(new Label { Content = product.Price });
            //stackPanel.Children.Add(stackPanel2);

            stackPanel.MouseUp += Mockup0.ProductItem_MouseUp;

            return stackPanel;
        }

        public static void ProductItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            UpdateDetailsColumn((Product)((StackPanel)sender).Tag);
        }

        private static void UpdateDetailsColumn(Product product)
        {
            ((Image)RightColumn.Children[0]).Source = Helpers.CreateBitmapImageFromUriString(product.ImageUri.ToString());
            ((StackPanel)RightColumn.Children[1]).Children.Add(new Label { Content = $"{product.Price} kr" });
            ((StackPanel)RightColumn.Children[1]).Children.Add(new Label { Content = product.Name });
            ((StackPanel)RightColumn.Children[1]).Children.Add(new Label { Content = product.Description });
        }
    }
}
