﻿/*
 TODO(johancz):
  * --- DONE --- Add Canvas-control as the Window's root control.
  * Clean up the code for merging into master.
  * --- DONE --- Fix Shopping Cart tab crash.
  * --- DONE --- Finish layout (of name, description, price, buttons) in the details column.
  * Fix: The "root"-Grid does not stretch to fill its parent (the "root"-Canvas control). Set height and width manually in a MainWindow.SizeChanged listener.
 */

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

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            Data.Init();

            // Window options
            Title = "Dollar Brad Store"; // TODO(johancz): Change before FINAL
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

            //// Root element
            //// Tabs
            //Tabs = new TabControl();
            //Tabs.Padding = new Thickness(-1, -1, -1, -1);

            //// Mockup 0 ///////////////////////////////////////////////////////////////////
            //{
            //    var mockup0_root_UIelement = UserMode.Create();
            //    Tabs.Items.Add(CreateMockupTabItem("Mockup 0", mockup0_root_UIelement));
            //}

            //// Mockup 1 ///////////////////////////////////////////////////////////////////
            //{
            //    //var mockupTEMPLATE_root_UIelement = Mockup0.Create();

            //    //// Mockup tabitem
            //    //Tabs.Items.Add(CreateMockupTabItem("Mockup 1", mockupTEMPLATE_root_UIelement));
            //}

            Content = UserMode.Create();
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Mockup1.MainWindow_OnSizeChanged(sender, e);
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
        public static ShoppingCart ActiveShoppingCart { get; set; } = new ShoppingCart();

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
        public static Image CreateNewImage(string uriString = null, int? height = null, string tooltipText = null, bool imageInTooltip = false)
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

    /// <summary>
    /// Mockup0
    /// </summary>
    public static class UserMode
    {
        public static Canvas RootElement { get; private set; }
        private static TabControl TabControl;
        private static TabItem TabItem_BrowseStore;
        private static TabItem TabItem_ShoppingCart;
        private static WrapPanel LeftColumn;
        public static StackPanel RightColumn;
        private static Image RightColumn_DetailsImage;
        private static Label RightColumn_DetailsName;
        private static Label RightColumn_DetailsPrice;
        private static Label RightColumn_DetailsDescription;
        private static Button RightColumn_DetailsRemoveFromCartButton;
        private static Button RightColumn_detailsAddToCartButton;

        public static UIElement Create()
        {
            // Root Canvas element
            RootElement = new Canvas();
            RootElement.Background = Brushes.LightBlue;
            // Grid with two columns, first column (left) for a tabcontrol with "Browse Store" and "ShoppingCart" tabs.
            // Second column contains details about the selected product and a small ScrollViewer containing the users "bookmarked" products.
            //var RootStackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            var rootGrid = new Grid { ShowGridLines = true };
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
            rootGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // Left Column
            {
                // Left Column Content Root: TabControl
                TabControl = new TabControl();

                // "Browse Store" Tab
                {
                    var tabContent_browseStore = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
                    var productsPanel = new WrapPanel();

                    foreach (Product product in Data.Products)
                    {
                        var productItem = UserMode.CreateProductItem(product);

                        if (productItem != null)
                        {
                            productsPanel.Children.Add(productItem);
                        }
                    }

                    TabItem_BrowseStore = new TabItem { Header = "Browse Store", Content = tabContent_browseStore };
                    TabControl.Items.Add(TabItem_BrowseStore);
                }

                // "Shopping Cart" Tab Contents
                {
                    var tabContent_shoppingCart = new ScrollViewer();
                    var shoppingCartPanel = new StackPanel { Orientation = Orientation.Vertical };

                    foreach (KeyValuePair<Product, int> product in Data.ActiveShoppingCart.Products)
                    {
                        // TODO(johancz): Create and draw a WPF-structure for each product in the shopping cart
                        //shoppingCartPanel.Children.Add(...);
                    }

                    TabItem_ShoppingCart = new TabItem { Header = "Shopping Cart", Content = tabContent_shoppingCart };
                    TabControl.Items.Add(TabItem_ShoppingCart);
                }

                // Add the left-column to the "root"-Grid.
                Grid.SetColumn(TabControl, 0);
                rootGrid.Children.Add(TabControl);
            }

            // Right Column
            {
                //  Right Column Content Root: StackPanel
                var rightColumnContentRoot = new StackPanel { Orientation = Orientation.Vertical, Visibility = Visibility.Hidden };

                // Create and add a Product.Image to the right column's root (StackPanel)
                rightColumnContentRoot.Children.Add(RightColumn_DetailsImage = new Image());

                // Details Column: name, price, description and shopping cart buttons.
                {
                    var rightColumn_detailsPanel = new StackPanel { Orientation = Orientation.Vertical };

                    // Create the product "Name" and "Price" labels and a StackPanel-parent for them. Add the parent to the detailsPanel.
                    {
                        var rightColumn_detailsPanel_nameAndPrice = new StackPanel { Orientation = Orientation.Horizontal };
                        RightColumn_DetailsName = new Label();
                        RightColumn_DetailsPrice = new Label();

                        rightColumn_detailsPanel_nameAndPrice.Children.Add(RightColumn_DetailsName);
                        rightColumn_detailsPanel_nameAndPrice.Children.Add(RightColumn_DetailsPrice);
                        rightColumn_detailsPanel.Children.Add(rightColumn_detailsPanel_nameAndPrice);
                    }

                    // Create the product description Label
                    RightColumn_DetailsDescription = new Label();
                    rightColumn_detailsPanel.Children.Add(RightColumn_DetailsDescription);

                    // Create a StackPanel-parent for the "Shopping Cart"-buttons
                    var rightColumn_detailsPanel_shoppingCartButtons = new StackPanel { Orientation = Orientation.Horizontal };
                    {
                        // Create "Remove from Shopping Cart" button with "click"-event listener.
                        (RightColumn_DetailsRemoveFromCartButton = new Button
                        {
                            FontSize = 14,
                            Content = "(-) Remove from shopping cart",
                            HorizontalAlignment = HorizontalAlignment.Right,
                        }).Click += RightColumn_DetailsRemoveFromCartButton_Click;

                        // Create "Add to Shopping Cart" button with "click"-event listener.
                        (RightColumn_detailsAddToCartButton = new Button
                        {
                            FontSize = 14,
                            Content = "(+) Add to shopping cart",
                            HorizontalAlignment = HorizontalAlignment.Right,
                        }).Click += RightColumn_DetailsAddToCartButton_Click;

                        // Add buttons to their parent StackPanel and then add the StackPanel to the "details"-StackPanel
                        rightColumn_detailsPanel.Children.Add(RightColumn_DetailsRemoveFromCartButton);
                        rightColumn_detailsPanel.Children.Add(RightColumn_detailsAddToCartButton);
                        rightColumn_detailsPanel.Children.Add(rightColumn_detailsPanel_shoppingCartButtons);
                    }

                    rightColumnContentRoot.Children.Add(rightColumn_detailsPanel);
                }

                // Add the right-column to the "root"-Grid.
                Grid.SetColumn(rightColumnContentRoot, 1);
                rootGrid.Children.Add(rightColumnContentRoot);
            }

            // Add "root" Grid to "root" Canvas
            RootElement.Children.Add(rootGrid);

            //var tabItem0_rootGrid = new Grid { ShowGridLines = true };
            //tabItem0_rootGrid.RowDefinitions.Add(new RowDefinition());
            //tabItem0_rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
            //tabItem0_rootGrid.ColumnDefinitions.Add(new ColumnDefinition());

            //var tabItem1_rootGrid = new Grid { ShowGridLines = true };
            //tabItem1_rootGrid.RowDefinitions.Add(new RowDefinition());
            //tabItem1_rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
            //tabItem1_rootGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // Column 0: "grid" of products
            //{
            //    //var productsGrid = new Grid { ShowGridLines = true };
            //    var productsPanel = new WrapPanel();

            //    foreach (Product product in Data.Products)
            //    {
            //        var gridItem = UserMode.CreateProductItem(product);

            //        if (gridItem != null)
            //        {
            //            productsPanel.Children.Add(gridItem);
            //        }
            //    }

            //    Grid.SetColumn(productsPanel, 0);
            //    Grid.SetRow(productsPanel, 0);
            //    tabItem0_rootGrid.Children.Add(productsPanel);
            //}

            // Column 1: right column width product image (large), name, description, price and buttons for adding (and removing?) the product to the shopping cart
            //{
            //    //var productsGrid = new Grid { ShowGridLines = true };
            //    UserMode.RightColumn = new StackPanel { Orientation = Orientation.Vertical };

            //    // TODO(johancz): change to image and split Helpers.CreateNewImage into smaller parts:
            //    //Mockup0.RightColumn.Children.Add(Helpers.CreateNewImage());
            //    //var detailsPanel = new StackPanel { Orientation = Orientation.Vertical };
            //    //Mockup0.RightColumn.Children.Add(detailsPanel);


            //    Grid.SetColumn(UserMode.RightColumn, 1);
            //    Grid.SetRow(UserMode.RightColumn, 0);
            //    tabItem0_rootGrid.Children.Add(UserMode.RightColumn);
            //}

            //tabItem0.Content = tabItem0_rootGrid;
            //tabItem1.Content = RightColumn;

            //RootElement.Items.Add(tabItem0);
            //RootElement.Items.Add(tabItem1);

            // Left/Products-list Column
            //LeftColumn = new WrapPanel();
            //LeftColumn_ = new Image();
            //LeftColumn_ = new Label();
            //LeftColumn_ = new Label();
            //LeftColumn_ = new Button();

            // Right/Details Column
            //RightColumn = new StackPanel();
            //RightColumn_DetailsImage = new Image();
            //var rightColumn_DetailsPanel = new StackPanel { Orientation = Orientation.Vertical };
            //RightColumn_DetailsName = new Label();
            //RightColumn_DetailsPrice = new Label();
            //RightColumn_DetailsDescription = new Label();
            //RightColumn_DetailsAddToCartButton = new Button
            //{
            //    FontSize = 14,
            //    Content = "(+) Add to shopping cart",
            //    HorizontalAlignment = HorizontalAlignment.Right,
            //    Visibility = Visibility.Collapsed
            //};
            //RightColumn_DetailsAddToCartButton.Click += RightColumn_DetailsAddToCartButton_Click;

            //// TODO(johancz): replace mockup 0 code in MainWindow.CreateUIElements with this class.

            //rightColumn_DetailsPanel.Children.Add(RightColumn_DetailsName);
            //rightColumn_DetailsPanel.Children.Add(RightColumn_DetailsPrice);
            //rightColumn_DetailsPanel.Children.Add(RightColumn_DetailsDescription);
            //rightColumn_DetailsPanel.Children.Add(RightColumn_DetailsAddToCartButton);

            //RightColumn.Children.Add(RightColumn_DetailsImage);
            //RightColumn.Children.Add(rightColumn_DetailsPanel);

            return RootElement;
        }

        public static StackPanel CreateProductItem(Product product)
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Tag = product
            };
            stackPanel.Children.Add(Helpers.CreateNewImage(product.ImageUri.ToString(), 50));
            stackPanel.Children.Add(new StackPanel() { Orientation = Orientation.Horizontal });
            ((StackPanel)stackPanel.Children[1]).Children.Add(new Label { Content = product.Name });
            ((StackPanel)stackPanel.Children[1]).Children.Add(new Label { Content = $"{product.Price} kr" });
            //var stackPanel2 = new StackPanel() { Orientation = Orientation.Horizontal };
            //stackPanel2.Children.Add(new Label { Content = product.Name });
            //stackPanel2.Children.Add(new Label { Content = product.Price });
            //stackPanel.Children.Add(stackPanel2);

            stackPanel.MouseUp += UserMode.ProductItem_MouseUp;

            return stackPanel;
        }

        private static void UpdateDetailsColumn(Product product)
        {
            ((Image)RightColumn.Children[0]).Source = Helpers.CreateBitmapImageFromUriString(product.ImageUri.ToString());
            //((StackPanel)RightColumn.Children[1]).Children.Add(new Label { Content = $"{product.Price} kr" });
            //((StackPanel)RightColumn.Children[1]).Children.Add(new Label { Content = product.Name });
            //((StackPanel)RightColumn.Children[1]).Children.Add(new Label { Content = product.Description });
            //RightColumn_DetailsImage = Helpers.CreateNewImage(product.ImageUri.ToString());
            RightColumn_DetailsName.Content = product.Name;
            RightColumn_DetailsPrice.Content = $"{product.Price} kr";
            RightColumn_DetailsDescription.Content = product.Description;
            RightColumn_detailsAddToCartButton.Tag = product;
            RightColumn_detailsAddToCartButton.Visibility = Visibility.Visible;
        }

        ////////////////////////////////////////////////////////
        //////////////////// Event Handling ////////////////////
        ////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ProductItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            UpdateDetailsColumn((Product)((StackPanel)sender).Tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void RightColumn_DetailsRemoveFromCartButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void RightColumn_DetailsAddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            Data.ActiveShoppingCart.AddProduct((Product)((StackPanel)sender).Tag);
        }
    }

    /// <summary>
    /// Mockup1
    /// </summary>
    public static class Mockup1
    {
        private static List<List> Lists = new List<List>();
        private static List<Product> Products = new List<Product>();
        private static ShoppingCart ActiveShoppingCart = new ShoppingCart();

        // UIElements
        public static UIElement RootElement { get; private set; }
        public static ListBox ProductsListbox { get; private set; }
        static Grid ProductsGrid;
        static TextBox Logging;
        static ListBox ProductListBox;

        public static void Create()
        {
            //var tabItem_products = new TabItem();
            //tabItem_products.Header = "Products";
            //var tabItem_shoppingCart = new TabItem();
            //tabItem_shoppingCart.Header = "Shopping Cart";

            //// "Product"-tabItem grid
            //ProductsGrid = new Grid();
            //ProductsGrid.Children.Add(new Border
            //{
            //    BorderBrush = Brushes.Black,
            //    BorderThickness = new Thickness(1)
            //});
            //ProductsGrid.Margin = new Thickness(5);
            //ProductsGrid.RowDefinitions.Add(new RowDefinition());
            //ProductsGrid.RowDefinitions.Add(new RowDefinition());
            ////grid_products.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            //ProductsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            //ProductsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            //tabItem_products.Content = ProductsGrid;
            //ProductsGrid.HorizontalAlignment = HorizontalAlignment.Stretch;

            //var searchStackPanel = new DockPanel
            //{
            //    //HorizontalAlignment = HorizontalAlignment.Stretch,
            //    //padd
            //    LastChildFill = true
            //};
            //var searchTextbox = new TextBox
            //{
            //    Name = "SearchTextBox",
            //    Background = Brushes.AliceBlue,
            //    VerticalAlignment = VerticalAlignment.Top,
            //    //HorizontalAlignment = HorizontalAlignment.Left,
            //    FontSize = 14,
            //    Padding = new Thickness(5),
            //    //HorizontalAlignment = HorizontalAlignment.Stretch
            //};
            //searchTextbox.TextChanged += SearchTextChanged;
            //var searchLabel = new Label
            //{
            //    Content = "Search",
            //    FontSize = 14,
            //    Target = searchTextbox
            //};
            //searchLabel.Tag = searchTextbox;
            //searchLabel.MouseUp += (object sender, MouseButtonEventArgs e) =>
            //{
            //    if (((Label)sender).Target != null)
            //    {
            //        ((Label)sender).Target.Focus();
            //    }
            //};
            //searchStackPanel.Children.Add(searchLabel);
            //searchStackPanel.Children.Add(searchTextbox);
            //AddControlToGrid(ProductsGrid, searchStackPanel, 0, 0);

            //// temporary logging
            //Logging = new TextBox
            //{
            //    Background = Brushes.AliceBlue,
            //    VerticalAlignment = VerticalAlignment.Top,
            //    HorizontalAlignment = HorizontalAlignment.Left
            //};
            //AddControlToGrid(ProductsGrid, Logging, 0, 1);

            //Tabs.Items.Add(tabItem_products);
            //Tabs.Items.Add(tabItem_shoppingCart);
        }

        private static void DrawProductListbox()
        {
            // LINQ?
            foreach (Product product in Data.Products)
            {
                var listBoxItem = CreateListboxItem(product);

                if (listBoxItem != null)
                {
                    ProductsListbox.Items.Add(listBoxItem);
                }
            }
        }

        private static void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO(johancz): only show products that match the search query
        }

        private static void AddControlToGrid(Grid grid, UIElement control, int row, int column)
        {
            Grid.SetRow(control, row);
            Grid.SetColumn(control, column);
            grid.Children.Add(control);
        }

        private static ListBoxItem? CreateListboxItem(Product product)
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

        private static void AddImageToGrid(Grid grid, Image image, int row, int column)
        {
            Grid.SetRow(image, row);
            Grid.SetColumn(image, column);
            grid.Children.Add(image);
        }

        public static void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Logging.Text = $"window width: {e.NewSize.Width}\nwindow height: {e.NewSize.Height}";
        }
    }

    /// <summary>
    /// MockupTEMPLATE
    /// </summary>
    public static class MockupTEMPLATE
    {
        public static UIElement RootElement { get; private set; }

        public static void Create()
        {
            var mockupTEMPLATE_root_UIelement = new Grid
            {
                ShowGridLines = true,
            };
            mockupTEMPLATE_root_UIelement.RowDefinitions.Add(new RowDefinition());
            mockupTEMPLATE_root_UIelement.RowDefinitions.Add(new RowDefinition());
            mockupTEMPLATE_root_UIelement.ColumnDefinitions.Add(new ColumnDefinition());
            mockupTEMPLATE_root_UIelement.ColumnDefinitions.Add(new ColumnDefinition());
        }
    }
}
