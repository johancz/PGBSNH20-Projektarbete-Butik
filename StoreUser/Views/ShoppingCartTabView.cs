using StoreCommon;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace StoreUser.Views
{
    public static class ShoppingCartTabView
    {
        private static TabItem _root;

        public static TabItem Init()
        {
            CreateGUI();
            return _root;
        }

        public static void CreateGUI()
        {
            var shoppingCartRootGrid = new Grid();
            var shoppingCartScrollViewer = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto, };
            shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            shoppingCartRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            // Shopping cart toolbar (with load and save buttons, total sum label)
            shoppingCartRootGrid.Children.Add(UserView.ShoppingCartToolbar);

            // Shopping cart items (StackPanel)
            shoppingCartScrollViewer.Content = UserView.ShoppingCartList;

            Grid.SetRow(shoppingCartScrollViewer, 1);
            shoppingCartRootGrid.Children.Add(shoppingCartScrollViewer);

            var tabLabel = $"({Store.ShoppingCart.Products.Sum(p => p.Value)} items. {Math.Round(Store.ShoppingCart.FinalSum, 2)} kr)";
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

            var placeOrderButton = new Button
            {
                Margin = new Thickness(10),
                Content = "Place Order",
                Padding = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            placeOrderButton.Click += PlaceOrderButton_Click;
            Grid.SetRow(placeOrderButton, 2);
            shoppingCartRootGrid.Children.Add(placeOrderButton);
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
            receiptWindow.Loaded += ReceiptWindow_Loaded;
            receiptWindow.Show();
        }

        private static void ReceiptWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var image = DetailsPanelView._rightColumn_DetailsImage;
            _originalImageSource = image.Source;
            var switchImageSource = Helpers.CreateBitmapImageFromUriString(Path.Combine(Environment.CurrentDirectory, "StoreData", "Image Helpers", "NewProductImage.jpeg"));
            _switchImageSource = switchImageSource;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();
            _timer = timer;
        }
        private static ImageSource _switchImageSource;
        private static ImageSource _originalImageSource;
        private static int counter = 0;
        private static DispatcherTimer _timer;
        private static void Timer_Tick(object sender, EventArgs e)
        {
            counter++;
            if (counter == 20)
            {
                DetailsPanelView._rightColumn_DetailsImage.Source = _switchImageSource;
            }
            if (counter > 22)
            {
                DetailsPanelView._rightColumn_DetailsImage.Source = _originalImageSource;
                _timer.Stop();
                counter = 0;
            }
        }

        internal static void UpdateShoppingCartTabHeader()
        {
            int itemCount = Store.ShoppingCart.Products.Sum(p => p.Value);
            ((Label)_root.Header).Content = $"My Shopping Cart ({itemCount} items. {Math.Round(Store.ShoppingCart.FinalSum, 2)} kr)";
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
            }
        }
    }
}
