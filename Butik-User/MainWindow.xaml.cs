/*
 TODO:
  * Right Column does not have a ScrollViewer
  * The Image in the right column should scale better, e.g. it should not take up more than X% of the available height.
 */

// Disable this debugging symbol by commenting the line out.
#define DEBUG_SET_BACKGROUND_COLOR

using StoreClassLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Butik_User
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;


            // Load all data; products, saved shopping carts, discount codes.
            Store.Init(); // Move into UserMode?

            // Window options
            Title = ".... Store (user mode)"; // TODO(johancz): Change before RELEASE
            Width = SystemParameters.WorkArea.Width >= 1000 ? SystemParameters.WorkArea.Width - 200 : 800;
            Height = SystemParameters.WorkArea.Height >= 800 ? SystemParameters.WorkArea.Height - 200 : 600;
            MinWidth = 400;
            MinHeight = 300;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Content = UserMode.Create();

            //SizeChanged += MainWindow_OnSizeChanged;
            KeyUp += MainWindow_KeyUp;
        }

        ////////////////////////////////////////////////////////
        //////////////////// Event Handling ////////////////////
        ////////////////////////////////////////////////////////

        //private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    Mockup1.MainWindow_OnSizeChanged(sender, e);
        //}

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            // Close Window with ESC-key.
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }
    }

   

    public static class Helpers
    {
        public static BitmapImage CreateBitmapImageFromUriString(string uriString)
        {
            try
            {
                var uri = new Uri(@"Images\" + uriString, UriKind.Relative);
                var bitMapImage = new BitmapImage(uri);

                return bitMapImage;
            }
            catch (Exception)
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
                        // TODO(johancz): Use the available Helper for creating images.
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
            catch (Exception)
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
        public static Canvas RootElement { get; private set; } // Needed for event-handling, TODO(johancz): remove getter & setter & make private?
        private static Grid _rootGrid; // Needed for event-handling
        private static TabControl _tabControl; // TODO(johancz): convert to local variable
        private static TabItem _tabItem_BrowseStore; // TODO(johancz): convert to local variable
        private static TabItem _tabItem_ShoppingCart; // TODO(johancz): convert to local variable
        private static StackPanel _rightColumnContentRoot; // TODO(johancz): can this be removed? Its only use: "_rightColumnContentRoot.Visibility = Visibility.Visible;"
        private static Image _rightColumn_DetailsImage;
        private static Label _rightColumn_DetailsName;
        private static Label _rightColumn_DetailsPrice;
        private static Label _rightColumn_DetailsDescription;
        private static Button _rightColumn_DetailsRemoveFromCartButton;
        private static Button _rightColumn_detailsAddToCartButton;

        public static UIElement Create()
        {
            // Root Canvas element
            RootElement = new Canvas();
            RootElement.SizeChanged += RootElement_SizeChanged;
#if DEBUG_SET_BACKGROUND_COLOR
            RootElement.Background = Brushes.LightBlue; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
            // Grid with two columns;
            // the first column (left) for a tabcontrol with "Browse Store" and "ShoppingCart" tabs,
            // the Second column contains details about the selected product.
            _rootGrid = new Grid { ShowGridLines = true };
#if DEBUG_SET_BACKGROUND_COLOR
            _rootGrid.Background = Brushes.LightGoldenrodYellow; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
            _rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _rootGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // Left Column
            {
                // Left Column Content Root: TabControl
                _tabControl = new TabControl(); // TODO(johancz): convert to local variable
#if DEBUG_SET_BACKGROUND_COLOR
                _tabControl.Background = Brushes.Magenta; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif

                // "Browse Store" Tab
                {
                    var tabContent_browseStore = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
#if DEBUG_SET_BACKGROUND_COLOR
                    tabContent_browseStore.Background = Brushes.LightCyan; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
                    var productsPanel = new WrapPanel();

                    foreach (Product product in Store.Products)
                    {
                        var productItem = UserMode.CreateProductItem(product);

                        if (productItem != null)
                        {
                            productsPanel.Children.Add(productItem);
                        }
                    }

                    // Add the Products-WrapPanel to the ScrollViewer
                    tabContent_browseStore.Content = productsPanel;

                    // Create the TabItem and add it to the TabControl
                    _tabItem_BrowseStore = new TabItem { Header = "Browse Store", Content = tabContent_browseStore }; // TODO(johancz): convert to local variable
                    _tabControl.Items.Add(_tabItem_BrowseStore);
                }

                // "Shopping Cart" Tab Contents
                {
                    var shoppingCartRootGrid = new Grid { ShowGridLines = true, Height = 50 };
#if DEBUG_SET_BACKGROUND_COLOR
                    shoppingCartRootGrid.Background = Brushes.LightGoldenrodYellow; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
                    var shoppingCartScrollViewer = new ScrollViewer();
                    shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition());
                    shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                    // Shopping cart toolbar (with load and save buttons, total sum label)
                    {
                        var shoppingCart_toolbar = new Grid { ShowGridLines = true, Height = 50 };
#if DEBUG_SET_BACKGROUND_COLOR
                        shoppingCart_toolbar.Background = Brushes.LightSeaGreen; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
                        shoppingCart_toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        shoppingCart_toolbar.ColumnDefinitions.Add(new ColumnDefinition());
                        shoppingCart_toolbar.ColumnDefinitions.Add(new ColumnDefinition());

                        // TotalSum-Label
                        var shoppingCart_itemCountLabel = new Label { Content = $"{Data.ActiveShoppingCart.TotalSum} kr" };
                        // Add Label to toolbar
                        Grid.SetColumn(shoppingCart_itemCountLabel, 0);
                        shoppingCart_toolbar.Children.Add(shoppingCart_itemCountLabel);

                        // Save-button
                        var shoppingCart_saveButton = new Button { Content = "Save Shopping Cart" };
                        shoppingCart_saveButton.Click += shoppingCart_saveButton_Click;
                        // Add Button to toolbar
                        Grid.SetColumn(shoppingCart_saveButton, 1);
                        shoppingCart_toolbar.Children.Add(shoppingCart_saveButton);

                        // Load-button
                        var shoppingCart_loadButton = new Button { Content = "Load Shopping Cart" };
                        shoppingCart_loadButton.Click += ShoppingCart_loadButton_Click;
                        // Add Button to toolbar
                        Grid.SetColumn(shoppingCart_loadButton, 2);
                        shoppingCart_toolbar.Children.Add(shoppingCart_loadButton);

                        // Add the toolbar to the Grid
                        shoppingCartRootGrid.Children.Add(shoppingCart_toolbar);
                    }

                    // Shopping cart items (StackPanel)
                    {
                        var shoppingCartPanel = new StackPanel { Orientation = Orientation.Vertical };

                        foreach (KeyValuePair<Product, int> product in Data.ActiveShoppingCart.Products)
                        {
                            //TODO(johancz): Create and draw a WPF - structure for each product in the shopping cart
                            //shoppingCartPanel.Children.Add(...);
                        }

                        shoppingCartScrollViewer.Content = shoppingCartPanel;
                    }
                    // Shopping Cart Save-button


                    // Add the shopping cart's ScrollViewer to its "root"-Grid
                    Grid.SetRow(shoppingCartScrollViewer, 1);
                    shoppingCartRootGrid.Children.Add(shoppingCartScrollViewer);

                    // Create the TabItem and add it to the TabControl
                    _tabItem_ShoppingCart = new TabItem { Header = "Shopping Cart", Content = shoppingCartRootGrid }; // TODO(johancz): convert to local variable
                    _tabControl.Items.Add(_tabItem_ShoppingCart);
                }

                // Add the left-column to the "root"-Grid.
                Grid.SetColumn(_tabControl, 0);
                _rootGrid.Children.Add(_tabControl);
            }

            // TODO(johancz): The contents of the right column probably needs a ScrollViewer, and maybe the Image should scale better (e.g. not take up more than X% of the available height).
            // Right Column
            {
                //  Right Column Content Root: StackPanel
                _rightColumnContentRoot = new StackPanel { Orientation = Orientation.Vertical, Visibility = Visibility.Hidden };

                // Create and add a Product.Image to the right column's root (StackPanel)
                _rightColumnContentRoot.Children.Add(_rightColumn_DetailsImage = new Image());

                // Details Column: name, price, description and shopping cart buttons.
                {
                    var rightColumn_detailsPanel = new StackPanel { Orientation = Orientation.Vertical };
#if DEBUG_SET_BACKGROUND_COLOR
                    rightColumn_detailsPanel.Background = Brushes.LightSalmon; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif

                    // Create the product "Name" and "Price" labels and a StackPanel-parent for them. Add the parent to the detailsPanel.
                    {
                        var rightColumn_detailsPanel_nameAndPrice = new StackPanel { Orientation = Orientation.Horizontal };
                        _rightColumn_DetailsName = new Label();
                        _rightColumn_DetailsPrice = new Label();

                        rightColumn_detailsPanel_nameAndPrice.Children.Add(_rightColumn_DetailsName);
                        rightColumn_detailsPanel_nameAndPrice.Children.Add(_rightColumn_DetailsPrice);
                        rightColumn_detailsPanel.Children.Add(rightColumn_detailsPanel_nameAndPrice);
                    }

                    // Create the product description Label
                    _rightColumn_DetailsDescription = new Label();
                    rightColumn_detailsPanel.Children.Add(_rightColumn_DetailsDescription);

                    // Create a StackPanel-parent for the "Shopping Cart"-buttons
                    var rightColumn_detailsPanel_shoppingCartButtons = new StackPanel { Orientation = Orientation.Horizontal };
                    {
                        // Create "Remove from Shopping Cart" button with "click"-event listener.
                        (_rightColumn_DetailsRemoveFromCartButton = new Button
                        {
                            FontSize = 14,
                            Content = "(-) Remove from shopping cart",
                            HorizontalAlignment = HorizontalAlignment.Right,
                        }).Click += _rightColumn_DetailsRemoveFromCartButton_Click;

                        // Create "Add to Shopping Cart" button with "click"-event listener.
                        (_rightColumn_detailsAddToCartButton = new Button
                        {
                            FontSize = 14,
                            Content = "(+) Add to shopping cart",
                            HorizontalAlignment = HorizontalAlignment.Right,
                        }).Click += _rightColumn_DetailsAddToCartButton_Click;

                        // Add buttons to their parent StackPanel and then add the StackPanel to the "details"-StackPanel
                        rightColumn_detailsPanel.Children.Add(_rightColumn_DetailsRemoveFromCartButton);
                        rightColumn_detailsPanel.Children.Add(_rightColumn_detailsAddToCartButton);
                        rightColumn_detailsPanel.Children.Add(rightColumn_detailsPanel_shoppingCartButtons);
                    }

                    _rightColumnContentRoot.Children.Add(rightColumn_detailsPanel);
                }

                // Add the right-column to the "root"-Grid.
                Grid.SetColumn(_rightColumnContentRoot, 1);
                _rootGrid.Children.Add(_rightColumnContentRoot);
            }

            // Add "root" Grid to "root" Canvas
            RootElement.Children.Add(_rootGrid);

            return RootElement;
        }

        public static StackPanel CreateProductItem(Product product)
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Tag = product
            };
            stackPanel.Children.Add(Helpers.CreateNewImage(product.Uri, 50));
            stackPanel.Children.Add(new StackPanel() { Orientation = Orientation.Horizontal });
            ((StackPanel)stackPanel.Children[1]).Children.Add(new Label { Content = product.Name });
            ((StackPanel)stackPanel.Children[1]).Children.Add(new Label { Content = $"{product.Price} kr" });

            stackPanel.MouseUp += UserMode.ProductItem_MouseUp;

            return stackPanel;
        }

        private static void UpdateDetailsColumn(Product product)
        {
            _rightColumn_DetailsImage.Source = Helpers.CreateBitmapImageFromUriString(product.Uri);

            _rightColumn_DetailsName.Content = product.Name;
            _rightColumn_DetailsPrice.Content = $"{product.Price} kr";
            _rightColumn_DetailsDescription.Content = product.Description;
            _rightColumn_DetailsRemoveFromCartButton.Tag = product;
            _rightColumn_detailsAddToCartButton.Tag = product;
            _rightColumn_detailsAddToCartButton.Visibility = Visibility.Visible;

            _rightColumnContentRoot.Visibility = Visibility.Visible;
        }

        ////////////////////////////////////////////////////////
        //////////////////// Event Handling ////////////////////
        ////////////////////////////////////////////////////////

        private static void shoppingCart_saveButton_Click(object sender, RoutedEventArgs e)
        {
            Data.ActiveShoppingCart.SaveToFile();
        }

        private static void ShoppingCart_loadButton_Click(object sender, RoutedEventArgs e)
        {
            Data.ActiveShoppingCart.LoadFromFile();
        }

        private static void RootElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Resize the "root"-Grid-control so that it fills the "root"-Canvas-control.
            _rootGrid.Height = RootElement.ActualHeight;
            _rootGrid.Width = RootElement.ActualWidth;
        }

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
        private static void _rightColumn_DetailsRemoveFromCartButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            Data.ActiveShoppingCart.RemoveProduct((Product)((Button)sender).Tag); // Cast "sender" to a Button, and then cast its Tag-object to a Product.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _rightColumn_DetailsAddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            Store.ShoppingCart.AddProduct((Product)((Button)sender).Tag); // Cast "sender" to a Button, and then cast its Tag-object to a Product.
        }
    }
}