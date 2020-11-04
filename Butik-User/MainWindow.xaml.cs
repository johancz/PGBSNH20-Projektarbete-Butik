/*
 TODO(johancz):
  * --- DONE --- Add Canvas-control as the Window's root control.
  * --- DONE --- Clean up the code for merging into master.
  * --- DONE --- Fix Shopping Cart tab crash.
  * --- DONE --- Finish layout (of name, description, price, buttons) in the details column.
  * --- DONE --- Fix: The "root"-Grid does not stretch to fill its parent (the "root"-Canvas control). Set height and width manually in a MainWindow.SizeChanged listener.
  * --- DONE --- Get the Products' WrapPanel to work again.
  * --- DONE --- Get the details column to work again.
  * 
 TODO:
  * Right Column does not have a ScrollViewer
  * The Image in the right column should scale better, e.g. it should not take up more than X% of the available height.
  * 
  * Rows of Code:
  * 2020-11-03 20:47: 673 (-12) rows
  * 2020-11-03 20:47: 406 (-18) rows
 */

// Disable this debugging symbol by commenting the line out.
#define DEBUG_SET_BACKGROUND_COLOR

using System;
using System.Collections.Generic;
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
            // Load all data; products, saved shopping carts, discount codes.
            Data.Init(); // Move into UserMode?

            // Window options
            Title = ".... Store (user mode)"; // TODO(johancz): Change before RELEASE
            Width = System.Windows.SystemParameters.WorkArea.Width >= 1000 ? System.Windows.SystemParameters.WorkArea.Width - 200 : 800;
            Height = System.Windows.SystemParameters.WorkArea.Height >= 800 ? System.Windows.SystemParameters.WorkArea.Height - 200 : 600;
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

    public static class Data
    {
        public static List<Product> Products { get; set; } = new List<Product>();
        public static ShoppingCart ActiveShoppingCart { get; set; } = new ShoppingCart();
        public static List<DiscountCode> DiscountCodes { get; set; }

        public static void Init()
        {
            Data.Products = TEMPORARY_AND_PLACEHOLDER_STUFF.CreatePlaceHolderProducts();
            LoadShoppingCart();
            LoadDiscountCodes();
        }

        private static void LoadShoppingCart()
        {
            //ActiveShoppingCart = ...
        }

        private static void LoadDiscountCodes()
        {
            //DiscountCodes = ...
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
        private static Grid RootGrid;
        private static TabControl TabControl;
        private static TabItem TabItem_BrowseStore;
        private static TabItem TabItem_ShoppingCart;
        private static StackPanel RightColumnContentRoot;
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
            RootElement.SizeChanged += RootElement_SizeChanged;
#if DEBUG_SET_BACKGROUND_COLOR
            RootElement.Background = Brushes.LightBlue; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
            // Grid with two columns;
            // the first column (left) for a tabcontrol with "Browse Store" and "ShoppingCart" tabs,
            // the Second column contains details about the selected product.
            RootGrid = new Grid { ShowGridLines = true };
#if DEBUG_SET_BACKGROUND_COLOR
            RootGrid.Background = Brushes.LightGoldenrodYellow; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
            RootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            RootGrid.ColumnDefinitions.Add(new ColumnDefinition());
            RootGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // Left Column
            {
                // Left Column Content Root: TabControl
                TabControl = new TabControl();

                // "Browse Store" Tab
                {
                    var tabContent_browseStore = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
#if DEBUG_SET_BACKGROUND_COLOR
                    tabContent_browseStore.Background = Brushes.LightCyan; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
                    var productsPanel = new WrapPanel();

                    foreach (Product product in Data.Products)
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

                    // Add the Products-WrapPanel to the ScrollViewer
                    tabContent_shoppingCart.Content = shoppingCartPanel;

                    // Create the TabItem and add it to the TabControl
                    TabItem_ShoppingCart = new TabItem { Header = "Shopping Cart", Content = tabContent_shoppingCart };
                    TabControl.Items.Add(TabItem_ShoppingCart);
                }

                // Add the left-column to the "root"-Grid.
                Grid.SetColumn(TabControl, 0);
                RootGrid.Children.Add(TabControl);
            }

            // TODO(johancz): The contents of the right column probably needs a ScrollViewer, and maybe the Image should scale better (e.g. not take up more than X% of the available height).
            // Right Column
            {
                //  Right Column Content Root: StackPanel
                RightColumnContentRoot = new StackPanel { Orientation = Orientation.Vertical, Visibility = Visibility.Hidden };

                // Create and add a Product.Image to the right column's root (StackPanel)
                RightColumnContentRoot.Children.Add(RightColumn_DetailsImage = new Image());

                // Details Column: name, price, description and shopping cart buttons.
                {
                    var rightColumn_detailsPanel = new StackPanel { Orientation = Orientation.Vertical };
#if DEBUG_SET_BACKGROUND_COLOR
                    rightColumn_detailsPanel.Background = Brushes.LightSalmon; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif

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

                    RightColumnContentRoot.Children.Add(rightColumn_detailsPanel);
                }

                // Add the right-column to the "root"-Grid.
                Grid.SetColumn(RightColumnContentRoot, 1);
                RootGrid.Children.Add(RightColumnContentRoot);
            }

            // Add "root" Grid to "root" Canvas
            RootElement.Children.Add(RootGrid);

            return RootElement;
        }

        private static void RootElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Resize the "root"-Grid-control so that it fills the "root"-Canvas-control.
            RootGrid.Height = RootElement.ActualHeight;
            RootGrid.Width = RootElement.ActualWidth;
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

            stackPanel.MouseUp += UserMode.ProductItem_MouseUp;

            return stackPanel;
        }

        private static void UpdateDetailsColumn(Product product)
        {
            RightColumn_DetailsImage.Source = Helpers.CreateBitmapImageFromUriString(product.ImageUri.ToString());
            RightColumn_DetailsName.Content = product.Name;
            RightColumn_DetailsPrice.Content = $"{product.Price} kr";
            RightColumn_DetailsDescription.Content = product.Description;
            RightColumn_detailsAddToCartButton.Tag = product;
            RightColumn_detailsAddToCartButton.Visibility = Visibility.Visible;

            RightColumnContentRoot.Visibility = Visibility.Visible;
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
            Data.ActiveShoppingCart.AddProduct((Product)((Button)sender).Tag); // Cast "sender" to a Button, and then cast its Tag-object to a Product.
        }
    }
}