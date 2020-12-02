using StoreCommon;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StoreUser.Views
{
    public static class ShoppingCartTabView
    {     
        private static Grid _root;       

        public static Grid Init()
        {
            CreateGUI();
            return _root;
        }

        public static void CreateGUI()
        {
            _root = new Grid();
            _root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            _root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            // Shopping cart toolbar (# of items, total and final sum, discount input & button):
            _root.Children.Add(UserView.ShoppingCartToolbarRoot);

            // List of items in shopping cart:
            Grid.SetRow(UserView.ShoppingCartListRoot, 1);
            _root.Children.Add(UserView.ShoppingCartListRoot);

            var bottomToolbarGrid = new Grid();
            bottomToolbarGrid.ColumnDefinitions.Add(new ColumnDefinition());
            bottomToolbarGrid.ColumnDefinitions.Add(new ColumnDefinition());
            // children of bottomToolbarGrid:
            {
                var clearShoppingCartButton = new Button
                {
                    Content = "Clear Shopping Cart",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5),
                    Padding = new Thickness(10),
                };
                clearShoppingCartButton.Click += ShoppingCart_clearButton_Click;
                bottomToolbarGrid.Children.Add(clearShoppingCartButton);

                var placeOrderButton = new Button
                {
                    Content = "Place Order",
                    Margin = new Thickness(5),
                    Padding = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Right,
                };
                placeOrderButton.Click += PlaceOrderButton_Click;
                Grid.SetColumn(placeOrderButton, 1);
                bottomToolbarGrid.Children.Add(placeOrderButton);
            }
            Grid.SetRow(bottomToolbarGrid, 2);
            _root.Children.Add(bottomToolbarGrid);
        }

        private static void ShowReceipt()
        {
            var receiptGrid = new Grid
            {
                Margin = new Thickness(50),
            };
            receiptGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            receiptGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            receiptGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            // Header row
            receiptGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            var headerProduct = new Label { Content = "Product", FontSize = 14, FontWeight = FontWeights.Bold, };
            var headerCount = new Label { Content = "Count", FontSize = 14, FontWeight = FontWeights.Bold, };
            var headerTotalCost = new Label { Content = "Total Cost", FontSize = 14, FontWeight = FontWeights.Bold, };
            Grid.SetColumn(headerCount, 1);
            Grid.SetColumn(headerTotalCost, 2);
            receiptGrid.Children.Add(headerProduct);
            receiptGrid.Children.Add(headerCount);
            receiptGrid.Children.Add(headerTotalCost);

            // start at 1 because of the header row occupying index 0
            for (int i = 1; i <= Store.ShoppingCart.Products.Keys.Count; i++)
            {
                receiptGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                var product = Store.ShoppingCart.Products.ElementAt(i - 1); // -1 because the loop starts at 1

                var rowProduct = new Label { Content = product.Key.Name };
                var rowCount = new Label { Content = product.Value };
                decimal productTotalCost = product.Key.Price * product.Value;
                if (Store.ShoppingCart.ActiveDiscountCode != null)
                {
                    productTotalCost *= (decimal)(1 - Store.ShoppingCart.ActiveDiscountCode.Percentage);
                }
                var rowTotalCost = new Label { Content = Math.Round(productTotalCost, 2) + " " + Store.Currency.Symbol };
                Grid.SetColumn(rowCount, 1);
                Grid.SetColumn(rowTotalCost, 2);
                Grid.SetRow(rowProduct, i);
                Grid.SetRow(rowCount, i);
                Grid.SetRow(rowTotalCost, i);
                receiptGrid.Children.Add(rowProduct);
                receiptGrid.Children.Add(rowCount);
                receiptGrid.Children.Add(rowTotalCost);
            }

            receiptGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            string finalCost = $"Total: {Math.Round(Store.ShoppingCart.FinalSum, 2)} {Store.Currency.Symbol}";
            if (Store.ShoppingCart.ActiveDiscountCode != null)
            {
                finalCost += $" (you saved {Math.Round(Store.ShoppingCart.FinalSum - Store.ShoppingCart.TotalSum, 2)} {Store.Currency.Symbol}!)";
            }
            var totalsLabel = new Label
            {
                Content = finalCost += "\n\nThank you for your order!",
                FontWeight = FontWeights.SemiBold,
                FontSize = 14,
            };
            Grid.SetColumnSpan(totalsLabel, 3);
            Grid.SetRow(totalsLabel, receiptGrid.RowDefinitions.Count);
            receiptGrid.Children.Add(totalsLabel);

            var receiptWindow = new Window
            {
                Title = "Your Receipt",
                Content = receiptGrid,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
            };
            receiptWindow.Loaded += Egg.ReceiptWindow_Loaded;
            receiptWindow.Show();
        }

        /******************************************************/
        /******************* Event Handling *******************/
        /******************************************************/

        private static void ShoppingCart_clearButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to empty your shopping cart?",
                                         "Clear Shopping Cart?",
                                         MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Store.ClearShoppingCart();
                UserView.UpdateGUI();
            }
        }

        private static void PlaceOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (Store.ShoppingCart.Products.Count == 0)
            {
                MessageBox.Show("Your Shopping Cart is empty, please add prodcuts first.");
                return;
            }

            var resultConfirm = MessageBox.Show("Please confirm the order", "Confirm", MessageBoxButton.OKCancel);

            if (resultConfirm == MessageBoxResult.OK)
            {
                ShowReceipt();
                Store.ClearShoppingCart();
                UserView.UpdateGUI();
            }
        }
    }
}
