
#define DEBUG_SET_BACKGROUND_COLOR

using StoreCommon;
using StoreUser;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreAdmin
{
    public class ManageProductsView
    {
        public static Canvas _root;
        private static Grid _rootGrid;
        private static StackPanel _rightColumnContentRoot;

        internal struct ProductItem_LayoutSettings
        {
            internal const double gridItemWidth = 200;
            internal const double gridItemHeight = 200;
            internal const int gridItemImageHeight = 175;
            internal const double gridItemTextHeight = 25;
        }
        public static Canvas Create()
        {
            var root = new Canvas 
            { 
                Background = Brushes.Black
            };
            _root = root;

            _rootGrid = new Grid { ShowGridLines = true};
            _rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _rootGrid.ColumnDefinitions.Add(new ColumnDefinition());

            _root.Children.Add(_rootGrid);

            return _root;
        }
    }
    public class ManageProductsView_UserCopy
    {
        private static Canvas _root;
        private static Grid _rootGrid;
        private static StackPanel _rightColumnContentRoot;

        private static Image _rightColumn_DetailsImage;
        private static Label _rightColumn_DetailsName;
        private static Label _rightColumn_DetailsPrice;
        private static Label _rightColumn_DetailsDescription;
        private static Button _rightColumn_DetailsRemoveFromCartButton;
        private static Button _rightColumn_detailsAddToCartButton;

        internal struct ProductItem_LayoutSettings
        {
            internal const double gridItemWidth = 200;
            internal const double gridItemHeight = 200;
            internal const int gridItemImageHeight = 175;
            internal const double gridItemTextHeight = 25;
        }

        public static Canvas Create()
        {
            _root = new Canvas();
            _root.SizeChanged += RootElement_SizeChanged;
            _root.Background = Brushes.LightBlue;

            _rootGrid = new Grid { ShowGridLines = true };

            _rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _rootGrid.ColumnDefinitions.Add(new ColumnDefinition());

            var tabControl = new TabControl();
            tabControl.Background = Brushes.Magenta;
            var tabContent_browseStore = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };

            tabContent_browseStore.Background = Brushes.LightCyan;

            var productsPanel = new WrapPanel { HorizontalAlignment = HorizontalAlignment.Center };

            foreach (Product product in Store.Products)
            {
                var productItem = UserView.CreateProductItem(product);

                if (productItem != null)
                {
                    productsPanel.Children.Add(productItem);
                }
            }

            tabContent_browseStore.Content = productsPanel;

            var tabItem_BrowseStore = new TabItem { Header = "Browse Store", Content = tabContent_browseStore };
            tabControl.Items.Add(tabItem_BrowseStore);

            var shoppingCartRootGrid = new Grid { ShowGridLines = true, Height = 50 };
#if DEBUG_SET_BACKGROUND_COLOR
            shoppingCartRootGrid.Background = Brushes.LightGoldenrodYellow;
#endif
            var shoppingCartScrollViewer = new ScrollViewer();
            shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition());
            shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var shoppingCart_toolbar = new Grid { ShowGridLines = true, Height = 50 };
#if DEBUG_SET_BACKGROUND_COLOR
            shoppingCart_toolbar.Background = Brushes.LightSeaGreen;
#endif
            shoppingCart_toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            shoppingCart_toolbar.ColumnDefinitions.Add(new ColumnDefinition());
            shoppingCart_toolbar.ColumnDefinitions.Add(new ColumnDefinition());

            var shoppingCart_itemCountLabel = new Label { Content = $"{Store.ShoppingCart.TotalSum} kr" };

            Grid.SetColumn(shoppingCart_itemCountLabel, 0);
            shoppingCart_toolbar.Children.Add(shoppingCart_itemCountLabel);


            var shoppingCart_saveButton = new Button { Content = "Save Shopping Cart" };
            shoppingCart_saveButton.Click += shoppingCart_saveButton_Click;

            Grid.SetColumn(shoppingCart_saveButton, 1);
            shoppingCart_toolbar.Children.Add(shoppingCart_saveButton);


            var shoppingCart_loadButton = new Button { Content = "Load Shopping Cart" };
            shoppingCart_loadButton.Click += ShoppingCart_loadButton_Click;

            Grid.SetColumn(shoppingCart_loadButton, 2);
            shoppingCart_toolbar.Children.Add(shoppingCart_loadButton);
            shoppingCartRootGrid.Children.Add(shoppingCart_toolbar);

            var shoppingCartPanel = new StackPanel { Orientation = Orientation.Vertical };

            foreach (KeyValuePair<Product, int> product in Store.ShoppingCart.Products)
            {
                //TODO(johancz): Create and draw a WPF - structure for each product in the shopping cart
                //shoppingCartPanel.Children.Add(...);
            }

            shoppingCartScrollViewer.Content = shoppingCartPanel;
            Grid.SetRow(shoppingCartScrollViewer, 1);
            shoppingCartRootGrid.Children.Add(shoppingCartScrollViewer);
            var tabItem_ShoppingCart = new TabItem { Header = "Shopping Cart", Content = shoppingCartRootGrid };
            tabControl.Items.Add(tabItem_ShoppingCart);
            Grid.SetColumn(tabControl, 0);
            _rootGrid.Children.Add(tabControl);
            _rightColumnContentRoot = new StackPanel { Orientation = Orientation.Vertical, Visibility = Visibility.Hidden };
            _rightColumnContentRoot.Children.Add(_rightColumn_DetailsImage = new Image());
            var rightColumn_detailsPanel = new StackPanel { Orientation = Orientation.Vertical };
#if DEBUG_SET_BACKGROUND_COLOR
            rightColumn_detailsPanel.Background = Brushes.LightSalmon;
#endif
            var rightColumn_detailsPanel_nameAndPrice = new StackPanel { Orientation = Orientation.Horizontal };
            _rightColumn_DetailsName = new Label();
            _rightColumn_DetailsPrice = new Label();

            rightColumn_detailsPanel_nameAndPrice.Children.Add(_rightColumn_DetailsName);
            rightColumn_detailsPanel_nameAndPrice.Children.Add(_rightColumn_DetailsPrice);
            rightColumn_detailsPanel.Children.Add(rightColumn_detailsPanel_nameAndPrice);

            _rightColumn_DetailsDescription = new Label();
            rightColumn_detailsPanel.Children.Add(_rightColumn_DetailsDescription);

            var rightColumn_detailsPanel_shoppingCartButtons = new StackPanel { Orientation = Orientation.Horizontal };

            (_rightColumn_DetailsRemoveFromCartButton = new Button
            {
                FontSize = 14,
                Content = "(-) Remove from shopping cart",
                HorizontalAlignment = HorizontalAlignment.Right,
            }).Click += _rightColumn_DetailsRemoveFromCartButton_Click;


            (_rightColumn_detailsAddToCartButton = new Button
            {
                FontSize = 14,
                Content = "(+) Add to shopping cart",
                HorizontalAlignment = HorizontalAlignment.Right,
            }).Click += _rightColumn_DetailsAddToCartButton_Click;


            rightColumn_detailsPanel.Children.Add(_rightColumn_DetailsRemoveFromCartButton);
            rightColumn_detailsPanel.Children.Add(_rightColumn_detailsAddToCartButton);
            rightColumn_detailsPanel.Children.Add(rightColumn_detailsPanel_shoppingCartButtons);

            _rightColumnContentRoot.Children.Add(rightColumn_detailsPanel);

            Grid.SetColumn(_rightColumnContentRoot, 1);
            _rootGrid.Children.Add(_rightColumnContentRoot);
            _root.Children.Add(_rootGrid);

            return _root;
        }

        public static Grid CreateProductItem(Product product)
        {
            var tooltip = new ToolTip
            {
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

            productGrid.MouseLeave += (sender, e) =>
            {
                tooltip.IsOpen = false;
            };

            productGrid.Background = Brushes.LightSlateGray;
            productGrid.ColumnDefinitions.Add(new ColumnDefinition());
            productGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            productGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(ProductItem_LayoutSettings.gridItemImageHeight, GridUnitType.Pixel) });
            productGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(ProductItem_LayoutSettings.gridItemTextHeight, GridUnitType.Pixel) });
            productGrid.MouseUp += UserView.ProductItem_MouseUp;

            var productThumbnail = Helpers.CreateNewImage(product.Uri, ProductItem_LayoutSettings.gridItemImageHeight);
            productThumbnail.Stretch = Stretch.UniformToFill;
            productThumbnail.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumnSpan(productThumbnail, 2);
            productGrid.Children.Add(productThumbnail);

            var nameLabel = new Label
            {
                Content = product.Name,
                Background = Brushes.LawnGreen
            };
            var priceLabel = new Label
            {
                Content = $"{product.Price} kr",
                Background = Brushes.LightCoral

            };
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
            _rootGrid.Height = _root.ActualHeight;
            _rootGrid.Width = _root.ActualWidth;
        }

        public static void ProductItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            UpdateDetailsColumn((Product)((Grid)sender).Tag);
        }
        private static void _rightColumn_DetailsRemoveFromCartButton_Click(object sender, RoutedEventArgs e)
        {
            Store.ShoppingCart.RemoveProduct((Product)((Button)sender).Tag);
        }
        private static void _rightColumn_DetailsAddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            Store.ShoppingCart.AddProduct((Product)((Button)sender).Tag, 1);
        }
    }
}
