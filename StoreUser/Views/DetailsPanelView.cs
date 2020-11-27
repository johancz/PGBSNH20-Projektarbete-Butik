using System.Windows;
using System.Windows.Controls;
using StoreCommon;

namespace StoreUser.Views
{
    public static class DetailsPanelView
    {
        //Creates the right column detailspanel in User Mode, same layout as admin.
        private static Grid _root;

        public static Image DetailsImage;
        private static Label _detailsName;
        private static TextBlock _detailsDescription;
        private static Button _detailsAddToCartButton;
        private static Button _detailsRemoveFromCartButton;
        private static Label _detailsPrice;

        public static Grid Init()
        {
            CreateGUI();
            return _root;
        }

        public static void CreateGUI()
        {
            _root = new Grid { Visibility = Visibility.Hidden, };
            _root.SizeChanged += EventHandler.Root_SizeChanged;
            _root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            // Needs to be GridUnitType.Star for MaxWidth to work on the TextBlock containing the product's description.
            _root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            _root.Children.Add(DetailsImage = new Image());
            // Details Column: name, price, description and shopping cart buttons.
            {
                var detailsColumn_detailsGrid = new Grid();
                detailsColumn_detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                detailsColumn_detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Grid.SetRow(detailsColumn_detailsGrid, 1);
                _root.Children.Add(detailsColumn_detailsGrid);

                var detailsColumn_namePriceDescription = new Grid();
                detailsColumn_namePriceDescription.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                detailsColumn_namePriceDescription.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                // Create the product "Name" and "Price" labels and a StackPanel-parent for them. Add the parent to the detailsPanel.
                {
                    var rightColumn_detailsPanel_nameAndPrice = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(5),
                    };
                    _detailsName = new Label { FontSize = 16, FontWeight = FontWeights.SemiBold };

                    rightColumn_detailsPanel_nameAndPrice.Children.Add(_detailsName);
                    Grid.SetRow(rightColumn_detailsPanel_nameAndPrice, 0);
                    detailsColumn_namePriceDescription.Children.Add(rightColumn_detailsPanel_nameAndPrice);
                }

                _detailsDescription = new TextBlock
                {
                    TextWrapping = TextWrapping.Wrap,
                };
                // Create the product description Label
                var scrollViewer = new ScrollViewer
                {
                    Margin = new Thickness(5),
                    Content = _detailsDescription,
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
                    _detailsAddToCartButton = new Button
                    {
                        Padding = new Thickness(5),
                        Content = new Label { Content = "(+) Add to shopping cart", HorizontalAlignment = HorizontalAlignment.Left },
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                    };
                    _detailsAddToCartButton.Click += EventHandler.DetailsAddToCartButton_Click;

                    // Create "Remove from Shopping Cart" button with "click"-event listener.
                    _detailsRemoveFromCartButton = new Button
                    {
                        Padding = new Thickness(5),
                        Content = new Label { Content = "(-) Remove from shopping cart", HorizontalAlignment = HorizontalAlignment.Left },
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Left,

                    };
                    _detailsRemoveFromCartButton.Click += EventHandler.DetailsRemoveFromCartButton_Click;

                    _detailsPrice = new Label { FontSize = 16 };
                    rightColumn_detailsPanel_shoppingCartButtons.Children.Add(_detailsPrice);
                    // Add buttons to their parent StackPanel and then add the StackPanel to the "details"-StackPanel
                    rightColumn_detailsPanel_shoppingCartButtons.Children.Add(_detailsAddToCartButton);
                    rightColumn_detailsPanel_shoppingCartButtons.Children.Add(_detailsRemoveFromCartButton);
                    Grid.SetColumn(rightColumn_detailsPanel_shoppingCartButtons, 0);
                    detailsColumn_detailsGrid.Children.Add(rightColumn_detailsPanel_shoppingCartButtons);
                }
            }
        }

        internal static void UpdateGUI()
        {
            if (UserView.SelectedProduct == null)
            {
                return;
            }

            DetailsImage.Source = ImageCreation.CreateBitmapImageFromUriString(UserView.SelectedProduct.Uri);
            _detailsName.Content = UserView.SelectedProduct.Name;
            _detailsPrice.Content = $"{UserView.SelectedProduct.Price} kr";
            // Necessary for text-wrapping to work. Not setting the MaxWidth property will cause the TextBlock.Width to grow beyond its bounds.
            _detailsDescription.MaxWidth = ((ScrollViewer)_detailsDescription.Parent).ActualWidth;
            _detailsDescription.Text = UserView.SelectedProduct.Description;
            _detailsRemoveFromCartButton.Tag = UserView.SelectedProduct;
            _detailsAddToCartButton.Tag = UserView.SelectedProduct;
            _detailsRemoveFromCartButton.Visibility = Store.ShoppingCart.Products.ContainsKey(UserView.SelectedProduct)
                                                                  ? Visibility.Visible
                                                                  : Visibility.Hidden;
            _root.Visibility = Visibility.Visible;
        }

        private static class EventHandler
        {
            internal static void DetailsRemoveFromCartButton_Click(object sender, RoutedEventArgs e)
            {
                var product = (Product)((Button)sender).Tag;
                if (product != null)
                {
                    Store.ShoppingCart.RemoveProduct(product);
                    Store.ShoppingCart.SaveToFile(DataManager.ShoppingCartCSV);
                    UserView.UpdateGUI();
                }
            }

            internal static void DetailsAddToCartButton_Click(object sender, RoutedEventArgs e)
            {
                var product = (Product)((Button)sender).Tag;
                if (product != null)
                {
                    Store.ShoppingCart.AddProduct(product, 1);
                    Store.ShoppingCart.SaveToFile(DataManager.ShoppingCartCSV);
                    UserView.UpdateGUI();
                }
            }

            internal static void Root_SizeChanged(object sender, SizeChangedEventArgs e)
            {
                // Necessary for text-wrapping to work. Not setting the MaxWidth property will cause the TextBlock.Width to grow beyond its bounds.
                _detailsDescription.MaxWidth = ((ScrollViewer)_detailsDescription.Parent).ActualWidth;
            }
        }
    }
}
