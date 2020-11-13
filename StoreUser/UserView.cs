/*
 TODO:
  * Right Column does not have a ScrollViewer
  * The Image in the right column should scale better, e.g. it should not take up more than X% of the available height.
 */

// Disable this debugging symbol by commenting the line out.
#define DEBUG_SET_BACKGROUND_COLOR

using StoreCommon;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreUser
{
    /// <summary>
    /// Mockup0
    /// </summary>
    public static class UserView
    {
        private static Canvas _root; // Needed for event-handling, TODO(johancz): remove getter & setter & make private?
        private static Grid _rootGrid; // Needed for event-handling

        private static StackPanel _rightColumnContentRoot; // TODO(johancz): can this be removed? Its only use: "_rightColumnContentRoot.Visibility = Visibility.Visible;"
        private static Image _rightColumn_DetailsImage;
        private static Label _rightColumn_DetailsName;
        private static Label _rightColumn_DetailsPrice;
        private static Label _rightColumn_DetailsDescription;
        private static Button _rightColumn_DetailsRemoveFromCartButton;
        private static Button _rightColumn_detailsAddToCartButton;
        // use the above or the struct below
        //internal struct RightColumn
        //{
        //    internal static StackPanel Root; // TODO(johancz): can this be removed? Its only use: "_rightColumnContentRoot.Visibility = Visibility.Visible;"
        //    internal static Image DetailsImage;
        //    internal static Label DetailsName;
        //    internal static Label DetailsPrice;
        //    internal static Label DetailsDescription;
        //    internal static Button DetailsRemoveFromCartButton;
        //    internal static Button detailsAddToCartButton;
        //};

        // TODO(johancz): Move to Settings-class?
        internal struct ProductItem_LayoutSettings
        {
            internal const double gridItemWidth = 200;
            internal const double gridItemHeight = 200;
            internal const int gridItemImageHeight = 175;
            internal const double gridItemTextHeight = 25;
        }

        public static Canvas Create()
        {
            _root = new Canvas(); // TODO(johancz): use a different control if we don't implement animations?
            _root.SizeChanged += RootElement_SizeChanged;
#if DEBUG_SET_BACKGROUND_COLOR
            _root.Background = Brushes.LightBlue; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
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
                var tabControl = new TabControl(); // TODO(johancz): convert to local variable
#if DEBUG_SET_BACKGROUND_COLOR
                tabControl.Background = Brushes.Magenta; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif

                // "Browse Store" Tab
                {
                    var tabContent_browseStore = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
#if DEBUG_SET_BACKGROUND_COLOR
                    tabContent_browseStore.Background = Brushes.LightCyan; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
                    var productsPanel = new WrapPanel { HorizontalAlignment = HorizontalAlignment.Center };

                    foreach (Product product in Store.Products)
                    {
                        var productItem = UserView.CreateProductItem(product);

                        if (productItem != null)
                        {
                            productsPanel.Children.Add(productItem);
                        }
                    }

                    // Add the Products-WrapPanel to the ScrollViewer
                    tabContent_browseStore.Content = productsPanel;

                    // Create the TabItem and add it to the TabControl
                    var tabItem_BrowseStore = new TabItem { Header = "Browse Store", Content = tabContent_browseStore }; // TODO(johancz): convert to local variable
                    tabControl.Items.Add(tabItem_BrowseStore);
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
                        var shoppingCart_itemCountLabel = new Label { Content = $"{Store.ShoppingCart.TotalSum} kr" };
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

                        foreach (KeyValuePair<Product, int> product in Store.ShoppingCart.Products)
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
                    var tabItem_ShoppingCart = new TabItem { Header = "Shopping Cart", Content = shoppingCartRootGrid }; // TODO(johancz): convert to local variable
                    tabControl.Items.Add(tabItem_ShoppingCart);
                }

                // Add the left-column to the "root"-Grid.
                Grid.SetColumn(tabControl, 0);
                _rootGrid.Children.Add(tabControl);
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
            _root.Children.Add(_rootGrid);

            return _root;
        }

        public static Grid CreateProductItem(Product product)
        {
            // TODO(johancz): use the ProductItem_LayoutSettings-struct or the following lines
            //int gridItemWidth = 200;
            //int gridItemHeight = 200;
            //int gridItemImageHeight = 175;
            //int gridItemTextHeight = 25;

            var tooltip = new ToolTip
            {
                //Placement = PlacementMode.Relative, // alt placement, doesn't require the MouseLeave-fix below.
                Placement = PlacementMode.Mouse,
                Content = $"{product.Name}\n{product.Description}\n"
            };
            var productGrid = new Grid
            {
                Tag = product,
                VerticalAlignment = VerticalAlignment.Top,
                Width = ProductItem_LayoutSettings.gridItemWidth,
                Height = ProductItem_LayoutSettings.gridItemHeight,
                Margin = new Thickness(5),
                ToolTip = tooltip
            };
            // This is required for the tooltip to appear at 'PlacementMode.Mouse' when hovering over another "productItem".
            // Otherwise the tooltip will "stick" to the old (this) "productItem" if the mouse is moved to the other "productItem" too quickly.
            productGrid.MouseLeave += (sender, e) =>
            {
                tooltip.IsOpen = false;
            };

#if DEBUG_SET_BACKGROUND_COLOR
            productGrid.Background = Brushes.LightSlateGray; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
            productGrid.ColumnDefinitions.Add(new ColumnDefinition());
            productGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            productGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(ProductItem_LayoutSettings.gridItemImageHeight, GridUnitType.Pixel) });
            productGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(ProductItem_LayoutSettings.gridItemTextHeight, GridUnitType.Pixel) });
            productGrid.MouseUp += UserView.ProductItem_MouseUp;

            // Image
            var productThumbnail = Helpers.CreateNewImage(product.Uri, ProductItem_LayoutSettings.gridItemImageHeight);
            productThumbnail.Stretch = Stretch.UniformToFill;
            productThumbnail.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumnSpan(productThumbnail, 2);
            productGrid.Children.Add(productThumbnail);

            // Name and price
            //var nameAndPricePanel = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Top };
            var nameLabel = new Label
            {
                Content = product.Name,
#if DEBUG_SET_BACKGROUND_COLOR
                Background = Brushes.LawnGreen // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
            };
            var priceLabel = new Label
            {
                Content = $"{product.Price} kr",
#if DEBUG_SET_BACKGROUND_COLOR
                Background = Brushes.LightCoral // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
            };
#if DEBUG_SET_BACKGROUND_COLOR
            //nameAndPricePanel.Background = Brushes.LightGreen; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
            Grid.SetColumn(nameLabel, 0);
            Grid.SetRow(nameLabel, 1);
            productGrid.Children.Add(nameLabel);
            Grid.SetColumn(priceLabel, 1);
            Grid.SetRow(priceLabel, 1);
            productGrid.Children.Add(priceLabel);

            return productGrid;
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
            Store.SaveShoppingCart();
        }

        private static void ShoppingCart_loadButton_Click(object sender, RoutedEventArgs e)
        {
            Store.LoadShoppingCart(WinTemp.ShoppingCartCSV);
        }

        private static void RootElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Resize the "root"-Grid-control so that it fills the "root"-Canvas-control.
            _rootGrid.Height = _root.ActualHeight;
            _rootGrid.Width = _root.ActualWidth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ProductItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            UpdateDetailsColumn((Product)((Grid)sender).Tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _rightColumn_DetailsRemoveFromCartButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            Store.ShoppingCart.RemoveProduct((Product)((Button)sender).Tag); // Cast "sender" to a Button, and then cast its Tag-object to a Product.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _rightColumn_DetailsAddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            Store.ShoppingCart.AddProduct((Product)((Button)sender).Tag, 1); // Cast "sender" to a Button, and then cast its Tag-object to a Product.
        }
    }
}
