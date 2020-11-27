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
        private static TabControl leftColumnTabControl;
        private static readonly Label _shoppingCartTabLabel = new Label { FontSize = 16, };

        internal static Product SelectedProduct;

        /*** Views ***/
        public static ScrollViewer BrowseProductsTabViewRoot;
        public static Grid ShoppingCartToolbarRoot;
        public static ListView ShoppingCartListRoot;
        public static Grid ShoppingCartTabRoot;
        public static Grid DetailsPanelRoot;

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
            BrowseProductsTabViewRoot = BrowseProductsTabView.Init();
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

            /*-------------------*/
            /*----- Child 0 -----*/
            /*-------------------*/

            // Left Column Content Root: TabControl
            leftColumnTabControl = new TabControl() { BorderThickness = new Thickness(0, 1, 0, 0), };
            // Children of "leftColumnTabControl":
            {
                // "Browse Store" Tab
                var tabItem_BrowseStore = new TabItem
                {
                    Header = "Browse Store",
                    FontSize = 16,
                    Content = BrowseProductsTabViewRoot,
                };
                leftColumnTabControl.Items.Add(tabItem_BrowseStore);

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
            }

            /*-------------------*/
            /*----- Child 1 -----*/
            /*-------------------*/

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

            /*-------------------*/
            /*----- Child 2 -----*/
            /*-------------------*/

            Grid.SetColumn(DetailsPanelRoot, 2);
            _root.Children.Add(DetailsPanelRoot);

            return _root;
        }

        internal static void UpdateGUI()
        {
            UpdateShoppingCartTabHeader();
            ShoppingCartToolbarView.UpdateGUI();
            ShoppingCartListView.Update();
            if (SelectedProduct != null)
            {
                DetailsPanelView.UpdateGUI();
            }
        }

        /******************************************************/
        /******************* Event Handling *******************/
        /******************************************************/

        internal static void UpdateShoppingCartTabHeader()
        {
            _shoppingCartTabLabel.Content = $"My Shopping Cart ({Store.ShoppingCart.Products.Sum(p => p.Value)} items. {Math.Round(Store.ShoppingCart.FinalSum, 2)} kr)";
        }
    }
}
