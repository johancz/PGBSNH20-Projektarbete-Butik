using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using StoreCommon;

namespace StoreUser.Views
{
    public static class View_ShoppingCartTab
    {
        private static TabItem _root;

        public static TabItem Init()
        {
            CreateGUI();
            UpdateData();
            UpdateGUI();
            return _root;
        }

        public static void CreateGUI()
        {
            var shoppingCartRootGrid = new Grid();
            var shoppingCartScrollViewer = new ScrollViewer();
            shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Shopping cart toolbar (with load and save buttons, total sum label)
            {
                shoppingCartRootGrid.Children.Add(UserView.ShoppingCartToolbar);
            }

            // Shopping cart items (StackPanel)
            {
                shoppingCartScrollViewer.Content = UserView.ShoppingCartList;
            }

            Grid.SetRow(shoppingCartScrollViewer, 1);
            shoppingCartRootGrid.Children.Add(shoppingCartScrollViewer);

            var tabLabel = $"({Store.ShoppingCart.Products.Sum(p => p.Value)} items. {Store.ShoppingCart.TotalSum} kr)";
            _root = new TabItem
            {
                Name = "UserView_root",
                Header = new Label
                {
                    Content = "My Shopping Cart " + tabLabel,
                    FontSize = 16
                },
                Content = shoppingCartRootGrid
            };
        }

        public static void UpdateGUI()
        {
        }

        internal static void UpdateShoppingCartTabHeader()
        {
            int itemCount = Store.ShoppingCart.Products.Sum(p => p.Value);
            ((Label)_root.Header).Content = $"My Shopping Cart ({itemCount} items. {Store.ShoppingCart.FinalSum} kr)";
        }

        private static void UpdateData()
        {

        }

        private static void UpdateShoppingCartView()
        {
        }

        private static class EventHandler
        {
        }
    }
}
