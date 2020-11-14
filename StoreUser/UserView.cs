/*
 TODO:
  * Right Column does not have a ScrollViewer
  * The Image in the right column should scale better, e.g. it should not take up more than X% of the available height.
 */

// Disable this debugging symbol by commenting the line out.
#define DEBUG_SET_BACKGROUND_COLOR

using StoreCommon;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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
        private static TabControl _tabControl;

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
        private static TabItem _shoppingCartTab;

        private static ListView _shoppingList_listView;

        // TODO(johancz): Move to Settings-class?
        internal struct ProductItem_LayoutSettings
        {
            internal const double gridItemWidth = 200;
            internal const double gridItemHeight = 200;
            internal const int gridItemImageHeight = 175;
            internal const double gridItemTextHeight = 25;
        }

        /// <summary>
        /// Create the UserView (which can be used in StoreUser and StoreAdmin
        /// </summary>
        /// <returns></returns>
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

            // Left Column
            {
                // Left Column Content Root: TabControl
                _tabControl = new TabControl { Padding = new Thickness(0) }; // TODO(johancz): convert to local variable
                _tabControl.Items.Add(BrowseStoreTab(header: "Browse Store"));
                _shoppingCartTab = ShoppingCartTab(header: "My Shopping Cart");
                _tabControl.Items.Add(_shoppingCartTab);

                Grid.SetColumn(_tabControl, 0);
                _rootGrid.Children.Add(_tabControl);
            }

            // TODO(johancz): The contents of the right column probably needs a ScrollViewer, and maybe the Image should scale better (e.g. not take up more than X% of the available height).
            // Right Column
            {
                _rightColumnContentRoot = RightColumn();
                // Add the right-column to the "root"-Grid.
                Grid.SetColumn(_rightColumnContentRoot, 1);
                _rootGrid.Children.Add(_rightColumnContentRoot);
            }

            // Add "root" Grid to "root" Canvas
            _root.Children.Add(_rootGrid);

            return _root;
        }

        /******************************************************/
        /******************* Main Controls ********************/
        /******************************************************/


        /// <summary>
        /// The "browse store" TabItem
        /// </summary>
        /// <returns></returns>
        private static TabItem BrowseStoreTab(string header)
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

            // Create the TabItem and return it.
            return new TabItem
            {
                Header = new Label { Content = header, FontSize = 16 },
                Content = tabContent_browseStore
            };
        }

        /// <summary>
        /// The Shopping cart TabItem
        /// </summary>
        /// <returns></returns>
        private static TabItem ShoppingCartTab(string header)
        {
            GridView gridView;
            var shoppingCartRootGrid = new Grid();
            var shoppingCartScrollViewer = new ScrollViewer();
            shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Shopping cart toolbar (with load and save buttons, total sum label)
            {
                var shoppingCart_toolbar = new Grid {};
#if DEBUG_SET_BACKGROUND_COLOR
                shoppingCart_toolbar.Background = Brushes.LightGray; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif
                shoppingCart_toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                shoppingCart_toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                shoppingCart_toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                // TotalSum-Label
                var shoppingCart_itemCountLabel = new Label
                {
                    Content = $"{Store.ShoppingCart.Products.Sum(p => p.Value)} items.\n{Store.ShoppingCart.TotalSum} kr"
                };
                // Add Label to toolbar
                Grid.SetColumn(shoppingCart_itemCountLabel, 0);
                shoppingCart_toolbar.Children.Add(shoppingCart_itemCountLabel);

                // Save-button
                var shoppingCart_saveButton = new Button
                {
                    Content = "Save Shopping Cart",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(5),
                    Margin = new Thickness(5),
                };
                shoppingCart_saveButton.Click += shoppingCart_saveButton_Click;
                // Add Button to toolbar
                Grid.SetColumn(shoppingCart_saveButton, 1);
                shoppingCart_toolbar.Children.Add(shoppingCart_saveButton);

                // Load-button
                var shoppingCart_loadButton = new Button
                {
                    Content = "Load Shopping Cart",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(5),
                    Margin = new Thickness(5),
                };
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


                var buttonFactory_buttonRemove1 = new FrameworkElementFactory(typeof(Button));
                buttonFactory_buttonRemove1.SetBinding(Button.ContentProperty, new Binding("buttonRemove1.Content"));
                buttonFactory_buttonRemove1.SetBinding(Button.TagProperty, new Binding("buttonRemove1.Tag"));
                buttonFactory_buttonRemove1.AddHandler(Button.ClickEvent, new RoutedEventHandler(UserView_ShoppingCartRemoveProduct_Click));

                var add1_buttonFactory = new FrameworkElementFactory(typeof(Button));
                add1_buttonFactory.SetBinding(Button.ContentProperty, new Binding("buttonAdd1.Content"));
                add1_buttonFactory.SetBinding(Button.TagProperty, new Binding("buttonAdd1.Tag"));
                add1_buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(UserView_ShoppingCartAddProduct_Click));

                var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
                stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
                stackPanelFactory.AppendChild(buttonFactory_buttonRemove1);
                stackPanelFactory.AppendChild(add1_buttonFactory);

                _shoppingList_listView = new ListView();
                _shoppingList_listView.ItemsSource = CreateShoppingCartData();

                gridView = new GridView { AllowsColumnReorder = false };
                var style = new Style { TargetType = typeof(GridViewColumnHeader) };
                style.Setters.Add(new Setter(ListViewItem.IsEnabledProperty, false));
                var t = new Trigger { Property = ListViewItem.IsEnabledProperty, Value = false };
                t.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.Black));
                style.Triggers.Add(t);
                gridView.ColumnHeaderContainerStyle = style;
                gridView.Columns.Add(new GridViewColumn
                {
                    //width
                    DisplayMemberBinding = new Binding("productName"),
                    Header = "Produkt"
                });
                gridView.Columns.Add(new GridViewColumn
                {
                    DisplayMemberBinding = new Binding("productPrice"),
                    Header = "Price"
                });
                gridView.Columns.Add(new GridViewColumn
                {
                    DisplayMemberBinding = new Binding("productCount"),
                    Header = "# of items"
                });
                gridView.Columns.Add(new GridViewColumn
                {
                    DisplayMemberBinding = new Binding("productTotalPrice"),
                    Header = "Total Price"
                });
                gridView.Columns.Add(new GridViewColumn
                {
                    CellTemplate = new DataTemplate { VisualTree = stackPanelFactory },
                    Header = "+/- items"
                });
                _shoppingList_listView.View = gridView;

                // TODO(johancz): ifall vi byter till en dummare control.
                //foreach (KeyValuePair<Product, int> product in Store.ShoppingCart.Products)
                //{
                //    var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                //    var labelName = new Label { Content = product.Key.Name };
                //    var labelCount = new Label { Content = product.Value };
                //    var labelTotalPrice = new Label { Content = product.Value * product.Key.Price };
                //    stackPanel.Children.Add(labelName);
                //    stackPanel.Children.Add(labelCount);
                //    stackPanel.Children.Add(labelTotalPrice);

                //    //var listBoxItem = new ListBoxItem();
                //    //listBoxItem.Content = stackPanel;
                //    //listBox.Items.Add(listBoxItem);

                //    var listViewItem = new ListViewItem();
                //    listViewItem.Content = stackPanel;
                //    listView.Items.Add(listViewItem);
                //}
                shoppingCartScrollViewer.Content = _shoppingList_listView;
            }

            Grid.SetRow(shoppingCartScrollViewer, 1);
            shoppingCartRootGrid.Children.Add(shoppingCartScrollViewer);

            return new TabItem
            {
                Name = "UserView_ShoppingCartTab",
                Header = new Label { Content = header, FontSize = 16 },
                Content = shoppingCartRootGrid
            };
        }

        private static IEnumerable<object> CreateShoppingCartData()
        {
            var combinedData = Store.ShoppingCart.Products.Select(product =>
            {
                var productRow = new
                {
                    productName = product.Key.Name,
                    productPrice = product.Key.Price + product.Key.Currency.Symbol,
                    productCount = product.Value,
                    productTotalPrice = product.Key.Price * product.Value + product.Key.Currency.Symbol,
                    buttonRemove1 = new { Content = " - ", Tag = product.Key },
                    buttonAdd1 = new { Content = " + ", Tag = product.Key },
                };

                return productRow;
            });

            return combinedData;
        }

        private static StackPanel RightColumn()
        {
            var rightColumnContentRoot = new StackPanel { Orientation = Orientation.Vertical, Visibility = Visibility.Hidden };

            // Create and add a Product.Image to the right column's root (StackPanel)
            rightColumnContentRoot.Children.Add(_rightColumn_DetailsImage = new Image());

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

                rightColumnContentRoot.Children.Add(rightColumn_detailsPanel);
            }

            return rightColumnContentRoot;
        }

        ////////////////////////////////////////////////////////
        //////////////////// Helper Methods ////////////////////
        ////////////////////////////////////////////////////////

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
            _shoppingList_listView.ItemsSource = CreateShoppingCartData();
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
            _shoppingList_listView.ItemsSource = CreateShoppingCartData();
        }

        static void UserView_ShoppingCartRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            Store.ShoppingCart.RemoveProduct((Product)((Button)sender).Tag);
            _shoppingList_listView.ItemsSource = CreateShoppingCartData();
        }

        static void UserView_ShoppingCartAddProduct_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            Store.ShoppingCart.AddProduct((Product)((Button)sender).Tag, 1);
            _shoppingList_listView.ItemsSource = CreateShoppingCartData();
        }
    }
}
