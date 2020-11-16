using StoreCommon;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreUser
{
    public static class UserView
    {
        private static Canvas _root; // Needed for event-handling
        private static Grid _rootGrid; // Needed for event-handling

        // TODO(johancz): can this be removed? Its only use: "_rightColumnContentRoot.Visibility = Visibility.Visible;"
        // TODO(johancz): Add another "_rightColumnContentRoot" showing a placeholder for when a product is not selected.
        private static Grid _rightColumnContentRoot; 
        private static Image _rightColumn_DetailsImage;
        private static Label _rightColumn_DetailsName;
        private static Label _rightColumn_DetailsPrice;
        private static TextBlock _rightColumn_DetailsDescription;
        private static Button _rightColumn_DetailsRemoveFromCartButton;
        private static Button _rightColumn_detailsAddToCartButton;

        // TODO(johancz): Move to a Settings-class?
        internal struct ProductItem_LayoutSettings
        {
            internal const double gridItemWidth = 200;
            internal const double gridItemHeight = 200;
            internal const int gridItemImageHeight = 175;
        }

        public static Canvas Create()
        {
            _root = new Canvas(); // TODO(johancz): use a different control if we don't implement animations?
            _root.SizeChanged += RootElement_SizeChanged;

            // Grid with two columns;
            // the first column (left) for a tabcontrol with "Browse Store" and "ShoppingCart" tabs,
            // the Second column contains details about the selected product.
            _rootGrid = new Grid { ShowGridLines = true };
            _rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _rootGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // _rootGrid child: Left Column (TabControl)
            {
                // Left Column Content Root: TabControl
                var tabControl = new TabControl(); // TODO(johancz): convert to local variable

                // "Browse Store" Tab
                {
                    var tabContent_browseStore = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
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
                    var tabItem_BrowseStore = new TabItem { Header = "Browse Store", Content = tabContent_browseStore };
                    tabControl.Items.Add(tabItem_BrowseStore);
                }

                // "Shopping Cart" Tab Contents
                {
                    var shoppingCartRootGrid = new Grid { ShowGridLines = true, Height = 50 };
                    var shoppingCartScrollViewer = new ScrollViewer();
                    shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition());
                    shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                    // Shopping cart toolbar (with load and save buttons, total sum label)
                    {
                        var shoppingCart_toolbar = new Grid { ShowGridLines = true, Height = 50 };
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
                        shoppingCart_saveButton.Click += ShoppingCart_saveButton_Click;
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

            // _rootGrid child: Right Column (Details)
            {
                //  Right Column Content Root: StackPanel
                _rightColumnContentRoot = new Grid { Visibility = Visibility.Hidden, ShowGridLines = true };
                _rightColumnContentRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                // Needs to be GridUnitType.Star for MaxWidth to work on the TextBlock containing the product's description.
                _rightColumnContentRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                // Create and add a Product.Image to the right column's root (StackPanel)
                _rightColumnContentRoot.Children.Add(_rightColumn_DetailsImage = new Image());

                // Details Column: name, price, description and shopping cart buttons.
                {
                    var detailsColumn_detailsGrid = new Grid { ShowGridLines = true };
                    detailsColumn_detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                    detailsColumn_detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    Grid.SetRow(detailsColumn_detailsGrid, 1);
                    _rightColumnContentRoot.Children.Add(detailsColumn_detailsGrid);

                    var detailsColumn_namePriceDescription = new Grid { ShowGridLines = true };
                    detailsColumn_namePriceDescription.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    detailsColumn_namePriceDescription.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    // Create the product "Name" and "Price" labels and a StackPanel-parent for them. Add the parent to the detailsPanel.
                    {
                        var rightColumn_detailsPanel_nameAndPrice = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(5),
                        };
                        _rightColumn_DetailsName = new Label { FontSize = 16, FontWeight = FontWeights.SemiBold };

                        rightColumn_detailsPanel_nameAndPrice.Children.Add(_rightColumn_DetailsName);
                        Grid.SetRow(rightColumn_detailsPanel_nameAndPrice, 0);
                        detailsColumn_namePriceDescription.Children.Add(rightColumn_detailsPanel_nameAndPrice);
                    }

                    _rightColumn_DetailsDescription = new TextBlock
                    {
                        TextWrapping = TextWrapping.Wrap,
                    };
                    // Create the product description Label
                    var scrollViewer = new ScrollViewer
                    {
                        Margin = new Thickness(5),
                        Content = _rightColumn_DetailsDescription,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                    };
                    Grid.SetRow(scrollViewer, 1);
                    detailsColumn_namePriceDescription.Children.Add(scrollViewer);
                    Grid.SetColumn(detailsColumn_namePriceDescription, 1);
                    detailsColumn_detailsGrid.Children.Add(detailsColumn_namePriceDescription);

                    // Create a StackPanel-parent for the "Shopping Cart"-buttons
                    var rightColumn_detailsPanel_shoppingCartButtons = new StackPanel {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(5)
                    };
                    {
                        // Create "Remove from Shopping Cart" button with "click"-event listener.
                        _rightColumn_DetailsRemoveFromCartButton = new Button
                        {
                            Padding = new Thickness(5),
                            Content = new Label { Content = "(-) Remove from shopping cart", HorizontalAlignment = HorizontalAlignment.Left },
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            HorizontalContentAlignment = HorizontalAlignment.Left,

                        };
                        _rightColumn_DetailsRemoveFromCartButton.Click += RightColumn_DetailsRemoveFromCartButton_Click;

                        // Create "Add to Shopping Cart" button with "click"-event listener.
                        _rightColumn_detailsAddToCartButton = new Button
                        {
                            Padding = new Thickness(5),
                            Content = new Label { Content = "(+) Add to shopping cart", HorizontalAlignment = HorizontalAlignment.Left },
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                        };
                        _rightColumn_detailsAddToCartButton.Click += RightColumn_DetailsAddToCartButton_Click;

                        _rightColumn_DetailsPrice = new Label { FontSize = 16 };
                        rightColumn_detailsPanel_shoppingCartButtons.Children.Add(_rightColumn_DetailsPrice);
                        // Add buttons to their parent StackPanel and then add the StackPanel to the "details"-StackPanel
                        rightColumn_detailsPanel_shoppingCartButtons.Children.Add(_rightColumn_DetailsRemoveFromCartButton);
                        rightColumn_detailsPanel_shoppingCartButtons.Children.Add(_rightColumn_detailsAddToCartButton);
                        Grid.SetColumn(rightColumn_detailsPanel_shoppingCartButtons, 0);
                        detailsColumn_detailsGrid.Children.Add(rightColumn_detailsPanel_shoppingCartButtons);
                    }
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
            var productGrid = new Grid
            {
                Tag = product,
                VerticalAlignment = VerticalAlignment.Top,
                Width = ProductItem_LayoutSettings.gridItemWidth,
                Height = ProductItem_LayoutSettings.gridItemHeight,
                Margin = new Thickness(5),
                ToolTip = tooltip,
                Background = Brushes.LightGray,
            };
            productGrid.MouseUp += UserView.ProductItem_MouseUp;
            // This is required for the tooltip to appear at 'PlacementMode.Mouse' when hovering over another "productItem".
            // Otherwise the tooltip will "stick" to the old (this) "productItem" if the mouse is moved to the other "productItem" too quickly.
            productGrid.MouseLeave += (sender, e) =>
            {
                tooltip.IsOpen = false;
            };

            productGrid.ColumnDefinitions.Add(new ColumnDefinition());
            productGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            productGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            productGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            // productGrid children:
            {
                var productThumbnail = Helpers.CreateNewImage(product.Uri, ProductItem_LayoutSettings.gridItemImageHeight);
                productThumbnail.Stretch = Stretch.UniformToFill;
                productThumbnail.VerticalAlignment = VerticalAlignment.Center;
                productThumbnail.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetColumnSpan(productThumbnail, 2);
                productGrid.Children.Add(productThumbnail);

                var nameLabel = new Label
                {
                    Content = product.Name,
                    FontSize = 14,
                };
                Grid.SetColumn(nameLabel, 0);
                Grid.SetRow(nameLabel, 1);
                productGrid.Children.Add(nameLabel);

                var priceLabel = new Label
                {
                    Content = $"{product.Price} kr",
                };
                Grid.SetColumn(priceLabel, 1);
                Grid.SetRow(priceLabel, 1);
                productGrid.Children.Add(priceLabel);
            }

            return productGrid;
        }

        private static void UpdateDetailsColumn(Product product)
        {
            _rightColumn_DetailsImage.Source = Helpers.CreateBitmapImageFromUriString(product.Uri);

            _rightColumn_DetailsName.Content = product.Name;
            _rightColumn_DetailsPrice.Content = $"{product.Price} kr";
            // Necessary for text-wrapping to work. Not setting the MaxWidth property will cause the TextBlock.Width to grow beyond it's bounds.
            _rightColumn_DetailsDescription.MaxWidth = ((ScrollViewer)_rightColumn_DetailsDescription.Parent).ActualWidth;
            _rightColumn_DetailsDescription.Text = product.Description;
            _rightColumn_DetailsRemoveFromCartButton.Tag = product;
            _rightColumn_detailsAddToCartButton.Tag = product;
            _rightColumn_detailsAddToCartButton.Visibility = Visibility.Visible;
            
            _rightColumnContentRoot.Visibility = Visibility.Visible;
        }

        /******************************************************/
        /******************* Event Handling *******************/
        /******************************************************/

        private static void ShoppingCart_saveButton_Click(object sender, RoutedEventArgs e)
        {
            Store.SaveShoppingCart();
        }

        private static void ShoppingCart_loadButton_Click(object sender, RoutedEventArgs e)
        {
            Store.LoadShoppingCart(AppFolder.ShoppingCartCSV);
        }

        private static void RootElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _rootGrid.Height = _root.ActualHeight;
            _rootGrid.Width = _root.ActualWidth;
            // Necessary for text-wrapping to work. Not setting the MaxWidth property will cause the TextBlock.Width to grow beyond it's bounds.
            _rightColumn_DetailsDescription.MaxWidth = ((ScrollViewer)_rightColumn_DetailsDescription.Parent).ActualWidth;
        }

        public static void ProductItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            UpdateDetailsColumn((Product)((Grid)sender).Tag);
        }

        private static void RightColumn_DetailsRemoveFromCartButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            Store.ShoppingCart.RemoveProduct((Product)((Button)sender).Tag);
        }

        private static void RightColumn_DetailsAddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            Store.ShoppingCart.AddProduct((Product)((Button)sender).Tag, 1);
        }
    }
}
