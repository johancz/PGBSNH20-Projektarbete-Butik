using StoreCommon;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
    public static class UserView
    {
        private static Canvas _root; // Needed for event-handling
        private static Grid _rootGrid; // Needed for event-handling

        // Left Column
        private static TabControl _leftColumnTabControl;

        // Left Column - Shopping Cart tab
        private static TabItem _shoppingCartTab;
        private static ListView _shoppingList_listView;

        // Right Column - Details column
        // TODO(johancz): can this be removed? Its only use: "_rightColumnContentRoot.Visibility = Visibility.Visible;"
        // TODO(johancz): Add another "_rightColumnContentRoot" showing a placeholder for when a product is not selected.
        private static Grid _rightColumnContentRoot;
        private static Image _rightColumn_DetailsImage;
        private static Label _rightColumn_DetailsName;
        private static Label _rightColumn_DetailsPrice;
        private static TextBlock _rightColumn_DetailsDescription;
        private static Button _rightColumn_DetailsRemoveFromCartButton;
        private static Button _rightColumn_detailsAddToCartButton;

        // TODO(johancz): Move to Settings-class?
        private struct ProductItem_LayoutSettings
        {
            internal static double gridItemWidth = 200;
            internal static double gridItemHeight = 200;
            internal static int gridItemImageHeight = 175;
        }

        public static Canvas Create()
        {
            _root = new Canvas(); // TODO(johancz): use a different control if we don't implement animations?
            _root.SizeChanged += RootElement_SizeChanged;

            // Grid with two columns;
            // the first column (left) contains a tabcontrol with "Browse Store" and "ShoppingCart" tabs,
            // the Second column (right) contains details about the selected product.
            _rootGrid = new Grid { ShowGridLines = true };
            _rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _rootGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // _rootGrid child: Left Column (TabControl)
            {
                // Left Column Content Root: TabControl
                _leftColumnTabControl = new TabControl(); // TODO(johancz): convert to local variable

                // Children of _leftColumnTabControl
                //{
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
                    _leftColumnTabControl.Items.Add(tabItem_BrowseStore);
                }

                // "Shopping Cart" Tab
                {
                    GridView gridView;
                    var shoppingCartRootGrid = new Grid();
                    var shoppingCartScrollViewer = new ScrollViewer();
                    shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                    // Shopping cart toolbar (with load and save buttons, total sum label)
                    {
                        var shoppingCart_toolbar = new Grid { };
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
                        shoppingCart_saveButton.Click += ShoppingCart_saveButton_Click;
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
                        _shoppingList_listView.SelectionChanged += _shoppingList_listView_SelectionChanged;
                        UpdateShoppingCartView();

                        gridView = new GridView { AllowsColumnReorder = false };
                        var style = new Style { TargetType = typeof(GridViewColumnHeader) };
                        style.Setters.Add(new Setter(ListViewItem.IsEnabledProperty, false));
                        var t = new Trigger { Property = ListViewItem.IsEnabledProperty, Value = false };
                        t.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.Black));
                        style.Triggers.Add(t);
                        gridView.ColumnHeaderContainerStyle = style;
                        gridView.Columns.Add(new GridViewColumn
                        {
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

                    _shoppingCartTab = new TabItem
                    {
                        Name = "UserView_ShoppingCartTab",
                        Header = new Label { Content = "My Shopping Cart", FontSize = 16 },
                        Content = shoppingCartRootGrid
                    };
                    _leftColumnTabControl.Items.Add(_shoppingCartTab);
                }
                //}

                Grid.SetColumn(_leftColumnTabControl, 0);
                _rootGrid.Children.Add(_leftColumnTabControl);
            }

            // _rootGrid child: Right Column (Details)
            {
                //  Right Column Content Root: Grid
                _rightColumnContentRoot = new Grid
                {
                    ShowGridLines = true,
                    Visibility = Visibility.Hidden,
                };

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
                    var rightColumn_detailsPanel_shoppingCartButtons = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(5)
                    };
                    {
                        // Create "Add to Shopping Cart" button with "click"-event listener.
                        _rightColumn_detailsAddToCartButton = new Button
                        {
                            Padding = new Thickness(5),
                            Content = new Label { Content = "(+) Add to shopping cart", HorizontalAlignment = HorizontalAlignment.Left },
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                        };
                        _rightColumn_detailsAddToCartButton.Click += RightColumn_DetailsAddToCartButton_Click;

                        // Create "Remove from Shopping Cart" button with "click"-event listener.
                        _rightColumn_DetailsRemoveFromCartButton = new Button
                        {
                            Padding = new Thickness(5),
                            Content = new Label { Content = "(-) Remove from shopping cart", HorizontalAlignment = HorizontalAlignment.Left },
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            HorizontalContentAlignment = HorizontalAlignment.Left,

                        };
                        _rightColumn_DetailsRemoveFromCartButton.Click += RightColumn_DetailsRemoveFromCartButton_Click;

                        _rightColumn_DetailsPrice = new Label { FontSize = 16 };
                        rightColumn_detailsPanel_shoppingCartButtons.Children.Add(_rightColumn_DetailsPrice);
                        // Add buttons to their parent StackPanel and then add the StackPanel to the "details"-StackPanel
                        rightColumn_detailsPanel_shoppingCartButtons.Children.Add(_rightColumn_detailsAddToCartButton);
                        rightColumn_detailsPanel_shoppingCartButtons.Children.Add(_rightColumn_DetailsRemoveFromCartButton);
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

        /******************************************************/
        /******************* Main Controls ********************/
        /******************************************************/

        //private static TabItem CreateBrowseStoreTab(string header)
        //{
        //}

        //private static TabItem CreateShoppingCartTab(string header)
        //{
        //}

        //private static CreateStackPanel RightColumn()
        //{
        //}

        private static void UpdateShoppingCartView()
        {
            var combinedData = Store.ShoppingCart.Products.Select(product =>
            {
                dynamic productRow = new ExpandoObject();
                productRow.product = product.Key;
                productRow.productName = product.Key.Name;
                productRow.productPrice = product.Key.Price + product.Key.Currency.Symbol;
                productRow.productCount = product.Value;
                productRow.productTotalPrice = product.Key.Price * product.Value + product.Key.Currency.Symbol;
                productRow.buttonRemove1 = new { Content = " - ", Tag = product.Key };
                productRow.buttonAdd1 = new { Content = " + ", Tag = product.Key };

                return productRow;
            });

            _shoppingList_listView.ItemsSource = combinedData;
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
            if (Store.ShoppingCart.Products.ContainsKey(product))
            {
                _rightColumn_DetailsRemoveFromCartButton.Visibility = Visibility.Visible;
            }
            else
            {
                _rightColumn_DetailsRemoveFromCartButton.Visibility = Visibility.Hidden;
            }

            //_rightColumnContentRoot.Visibility = Visibility.Visible;
        }

        /******************************************************/
        /******************* Event Handling *******************/
        /******************************************************/

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
            _rightColumnContentRoot.Visibility = Visibility.Visible;
        }

        private static void RightColumn_DetailsRemoveFromCartButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            var product = (Product)((Button)sender).Tag;
            Store.ShoppingCart.RemoveProduct(product);
            UpdateShoppingCartView();
            UpdateDetailsColumn(product);
        }

        private static void RightColumn_DetailsAddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            var product = (Product)((Button)sender).Tag;
            Store.ShoppingCart.AddProduct(product, 1);
            UpdateShoppingCartView();
            UpdateDetailsColumn(product);
        }

        static void UserView_ShoppingCartRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            var product = (Product)((Button)sender).Tag;
            Store.ShoppingCart.RemoveProduct(product);
            UpdateShoppingCartView();
            UpdateDetailsColumn(product);
        }

        static void UserView_ShoppingCartAddProduct_Click(object sender, RoutedEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            var product = (Product)((Button)sender).Tag;
            Store.ShoppingCart.AddProduct(product, 1);
            UpdateShoppingCartView();
            UpdateDetailsColumn(product);
        }

        private static void ShoppingCart_saveButton_Click(object sender, RoutedEventArgs e)
        {
            Store.SaveShoppingCart();
        }

        private static void ShoppingCart_loadButton_Click(object sender, RoutedEventArgs e)
        {
            Store.LoadShoppingCart(AppFolder.ShoppingCartCSV);
            UserView.UpdateShoppingCartView();
        }

        private static void _shoppingList_listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExpandoObject listViewItemData = ((ExpandoObject)_shoppingList_listView.SelectedItem);
            if (listViewItemData != null)
            {
                var product = (Product)listViewItemData.ToList()[0].Value;
                UpdateDetailsColumn(product); // TODO: bug if clicking button in item
                _rightColumnContentRoot.Visibility = Visibility.Visible;
            }
        }
    }
}
