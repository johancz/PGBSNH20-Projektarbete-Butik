using StoreCommon;
using StoreUser.Views;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreUser
{
    public static class UserView
    {
        private static Grid _root;

        // Left Column
        private static TabControl leftColumnTabControl;

        internal static Product SelectedProduct;

        /*** Views ***/
        public static Grid ShoppingCartTabRoot;
        public static Grid ShoppingCartToolbarRoot;
        public static ListView ShoppingCartListRoot;
        public static Grid DetailsPanelRoot;
        private static readonly Label _shoppingCartTabLabel = new Label { FontSize = 16, };

        private struct ProductItem_LayoutSettings
        {
            internal static double gridItemWidth = 200;
            internal static double gridItemHeight = 200;
            internal static int gridItemImageHeight = 175;
        }

        public static Grid Create()
        {
            /*** Views ***/
            // Initiate and "import" all of the subviews (a subview in this context is a smaller part of the GUI which can be kept in it's own class/file which means not having to deal with a massive file/class, this simplifies implementation/maintenance/debugging.
            // A view is mostly self-contained, in that it keeps track of and handles it's own wpf-controls and events, but still relies on data stored in the "Store"-class, and on occasion is told to or tells other views to update their GUI. The last detail could be improved upon by instead using Events.
            ShoppingCartToolbarRoot = ShoppingCartToolbarView.Init();
            ShoppingCartListRoot = ShoppingCartListView.Init();
            ShoppingCartTabRoot = ShoppingCartTabView.Init();
            DetailsPanelRoot = DetailsPanelView.Init();

            // Grid with two columns;
            // the first column (left) contains a tabcontrol with "Browse Store" and "ShoppingCart" tabs,
            // the Second column (right) contains details about the selected product.
            _root = new Grid();
            _root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _root.ColumnDefinitions.Add(new ColumnDefinition { MinWidth = 400, });
            _root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto), });
            _root.ColumnDefinitions.Add(new ColumnDefinition { MinWidth = 400, });

            // Left Column Content Root: TabControl
            leftColumnTabControl = new TabControl() { BorderThickness = new Thickness(0, 1, 0, 0), };

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
                var tabItem_BrowseStore = new TabItem
                {
                    Header = "Browse Store",
                    FontSize = 16,
                    Content = tabContent_browseStore,
                };
                leftColumnTabControl.Items.Add(tabItem_BrowseStore);
            }

            // "My Shopping Cart" Tab
            var tabItem_ShoppingCart = new TabItem
            {
                Header = new Label
                {
                    Content = _shoppingCartTabLabel,
                    FontSize = 16
                },
                // Add the subview created in and "imported" from "ShoppingCartTabView.cs".
                Content = ShoppingCartTabRoot
            };
            UpdateShoppingCartTabHeader();
            leftColumnTabControl.Items.Add(tabItem_ShoppingCart);

            Grid.SetColumn(leftColumnTabControl, 0);
            _root.Children.Add(leftColumnTabControl);

            var gridSplitter = new GridSplitter
            {
                Width = 2,
                IsEnabled = true,
                Background = Brushes.Gray,
                Margin = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Stretch,

            };
            Grid.SetColumn(gridSplitter, 1);
            //_rootGrid.Children.Add(gridSplitter);
            _root.Children.Add(gridSplitter);

            Grid.SetColumn(DetailsPanelRoot, 2);
            //_rootGrid.Children.Add(DetailsPanelRoot);
            _root.Children.Add(DetailsPanelRoot);

            // Add "root" Grid to "root" Canvas
            //_root.Children.Add(_rootGrid);

            return _root;
        }

        internal static void UpdateGUI()
        {
            UpdateShoppingCartTabHeader();
            ShoppingCartToolbarView.UpdateGUI();
            ShoppingCartListView.Update();
            if (SelectedProduct != null)
            {
                DetailsPanelView.UpdateGUI(SelectedProduct);
            }
        }

        /******************************************************/
        /******************* Main Controls ********************/
        /******************************************************/

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
            var productItem = new Grid
            {
                Tag = product,
                VerticalAlignment = VerticalAlignment.Top,
                Width = ProductItem_LayoutSettings.gridItemWidth,
                Height = ProductItem_LayoutSettings.gridItemHeight,
                Margin = new Thickness(5),
                ToolTip = tooltip,
                Background = Brushes.LightGray,
            };
            productItem.MouseUp += UserView.ProductItem_MouseUp;
            // This is required for the tooltip to appear at 'PlacementMode.Mouse' when hovering over another "productItem".
            // Otherwise the tooltip will "stick" to the old (this) "productItem" if the mouse is moved to the other "productItem" too quickly.
            productItem.MouseLeave += (sender, e) =>
            {
                tooltip.IsOpen = false;
            };

            productItem.ColumnDefinitions.Add(new ColumnDefinition());
            productItem.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            productItem.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            productItem.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            // productItem children:
            {
                var productThumbnail = ImageCreation.CreateNewImage(product.Uri, ProductItem_LayoutSettings.gridItemImageHeight);
                productThumbnail.Stretch = Stretch.UniformToFill;
                productThumbnail.VerticalAlignment = VerticalAlignment.Center;
                productThumbnail.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetColumnSpan(productThumbnail, 2);
                productItem.Children.Add(productThumbnail);

                var nameLabel = new Label
                {
                    Content = product.Name,
                    FontSize = 14,
                };
                Grid.SetColumn(nameLabel, 0);
                Grid.SetRow(nameLabel, 1);
                productItem.Children.Add(nameLabel);

                var priceLabel = new Label
                {
                    Content = $"{product.Price} kr",
                };
                Grid.SetColumn(priceLabel, 1);
                Grid.SetRow(priceLabel, 1);
                productItem.Children.Add(priceLabel);
            }

            return productItem;
        }

        /******************************************************/
        /******************* Event Handling *******************/
        /******************************************************/

        public static void ProductItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var product = (Product)((Grid)sender).Tag;
            if (product != null)
            {
                SelectedProduct = product;
                DetailsPanelView.UpdateGUI(product);
            }
        }

        internal static void UpdateShoppingCartTabHeader()
        {
            _shoppingCartTabLabel.Content = $"My Shopping Cart ({Store.ShoppingCart.Products.Sum(p => p.Value)} items. {Math.Round(Store.ShoppingCart.FinalSum, 2)} kr)";
        }
    }
}
