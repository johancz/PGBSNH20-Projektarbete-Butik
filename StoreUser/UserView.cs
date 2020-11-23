using StoreCommon;
using StoreUser.Views;
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

        // Left Column
        private static TabControl leftColumnTabControl;

        internal static Product _selectedProduct;

        /*** Views ***/
        public static TabItem ShoppingCartTab;
        public static Grid ShoppingCartToolbar;
        public static ListView ShoppingCartList;
        public static Grid DetailsPanel;

        private struct ProductItem_LayoutSettings
        {
            internal static double gridItemWidth = 200;
            internal static double gridItemHeight = 200;
            internal static int gridItemImageHeight = 175;
        }

        public static Canvas Create()
        {
            // NEW ////////////////////////////////
            /*** Views ***/
            ShoppingCartToolbar = ShoppingCartToolbarView.Init();
            ShoppingCartList = ShoppingCartListView.Init();
            ShoppingCartTab = ShoppingCartTabView.Init();
            DetailsPanel = DetailsPanelView.Init();

            ;
            // NEW ////////////////////////////////

            _root = new Canvas();
            _root.SizeChanged += RootElement_SizeChanged;

            // Grid with two columns;
            // the first column (left) contains a tabcontrol with "Browse Store" and "ShoppingCart" tabs,
            // the Second column (right) contains details about the selected product.
            _rootGrid = new Grid ();
            _rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _rootGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // Left Column Content Root: TabControl
            leftColumnTabControl = new TabControl();

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
                    Content = tabContent_browseStore
                };
                leftColumnTabControl.Items.Add(tabItem_BrowseStore);
            }

            // "My Shopping Cart" Tab
            leftColumnTabControl.Items.Add(ShoppingCartTab);

            Grid.SetColumn(leftColumnTabControl, 0);
            _rootGrid.Children.Add(leftColumnTabControl);

            Grid.SetColumn(DetailsPanel, 1);
            _rootGrid.Children.Add(DetailsPanel);

            // Add "root" Grid to "root" Canvas
            _root.Children.Add(_rootGrid);

            return _root;
        }

        internal static void UpdateGUI()
        {
            ShoppingCartTabView.UpdateShoppingCartTabHeader();
            ShoppingCartToolbarView.UpdateGUI();
            ShoppingCartListView.UpdateData();
            if (_selectedProduct != null)
            {
                DetailsPanelView.UpdateGUI(_selectedProduct);
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

        /******************************************************/
        /******************* Event Handling *******************/
        /******************************************************/

        private static void RootElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _rootGrid.Height = _root.ActualHeight;
            _rootGrid.Width = _root.ActualWidth;

            // Necessary for text-wrapping to work. Not setting the MaxWidth property will cause the TextBlock.Width to grow beyond its bounds.
            //__View_DetailsPanel._rightColumn_DetailsDescription.MaxWidth = ((ScrollViewer)_rightColumn_DetailsDescription.Parent).ActualWidth;
            DetailsPanelView.EventHandler.External_RootElement_SizeChanged(sender, e);
        }

        public static void ProductItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // TODO(johancz): Error/Exception-handling
            var product = (Product)((Grid)sender).Tag;
            _selectedProduct = product;
            DetailsPanelView.UpdateGUI(product);
        }
    }
}
