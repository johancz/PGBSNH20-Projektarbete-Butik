using System.Windows;
using System.Windows.Controls;
using StoreCommon;

namespace StoreUser.Views
{
    public static class DetailsPanelView
    {
        private static Grid _root;

        private static Label _detailsName;
        private static TextBlock _description;
        private static Button _addToCartButton;
        private static Button _removeFromCartButton;
        private static Label _priceLabel;

        public static Image DetailsImage;

        public static Grid Init()
        {
            CreateGUI();
            return _root;
        }

        public static void CreateGUI()
        {
            _root = new Grid { Visibility = Visibility.Hidden, };
            _root.SizeChanged += Root_SizeChanged;
            _root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            // Needs to be GridUnitType.Star for MaxWidth to work on the TextBlock containing the product's description.
            _root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Children of '_root'
            {
                /*** Child 0 ***/
                _root.Children.Add(DetailsImage = new Image());

                /*** Child 1 ***/
                var detailsGrid = new Grid();
                detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Grid.SetRow(detailsGrid, 1);
                _root.Children.Add(detailsGrid);

                // Children of 'detailsGrid':
                {
                    /*** Child 0 ***/
                    var shoppingCartButtons = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(5) };
                    Grid.SetColumn(shoppingCartButtons, 0);
                    detailsGrid.Children.Add(shoppingCartButtons);

                    // Children of 'shoppingCartButtons':
                    {
                        /*** Child 0 ***/
                        _addToCartButton = new Button
                        {
                            Padding = new Thickness(5),
                            Content = new Label { Content = "(+) Add to shopping cart", HorizontalAlignment = HorizontalAlignment.Left },
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                        };
                        _addToCartButton.Click += DetailsAddToCartButton_Click;
                        shoppingCartButtons.Children.Add(_addToCartButton);

                        /*** Child 1 ***/
                        _removeFromCartButton = new Button
                        {
                            Padding = new Thickness(5),
                            Content = new Label { Content = "(-) Remove from shopping cart", HorizontalAlignment = HorizontalAlignment.Left },
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            HorizontalContentAlignment = HorizontalAlignment.Left,

                        };
                        _removeFromCartButton.Click += DetailsRemoveFromCartButton_Click;
                        shoppingCartButtons.Children.Add(_removeFromCartButton);

                        /*** Child 2 ***/
                        _priceLabel = new Label { FontSize = 16 };
                        shoppingCartButtons.Children.Add(_priceLabel);
                    }

                    /*** Child 1 ***/
                    var namePriceDescription = new Grid();
                    namePriceDescription.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    namePriceDescription.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    Grid.SetColumn(namePriceDescription, 1);
                    detailsGrid.Children.Add(namePriceDescription);

                    // Children of 'namePriceDescription':
                    {
                        /*** Child 0 ***/
                        var nameAndPrice = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(5),
                        };
                        Grid.SetRow(nameAndPrice, 0);
                        namePriceDescription.Children.Add(nameAndPrice);

                        _detailsName = new Label { FontSize = 16, FontWeight = FontWeights.SemiBold };
                        nameAndPrice.Children.Add(_detailsName);

                        /*** Child 1 ***/
                        _description = new TextBlock { TextWrapping = TextWrapping.Wrap, };
                        var descriptionScrollViewer = new ScrollViewer
                        {
                            Margin = new Thickness(5),
                            Content = _description,
                            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                            HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                        };
                        Grid.SetRow(descriptionScrollViewer, 1);
                        namePriceDescription.Children.Add(descriptionScrollViewer);
                    } // end of: Children of 'namePriceDescription'
                } // end of: Children of 'detailsGrid'
            } // end of: Children of '_root'
        }

        // Needs the 'internal'-modifier so that it is accessible outside the UserView-class (note: it could also be 'public').
        internal static void UpdateGUI()
        {
            if (UserView.SelectedProduct == null)
            {
                _root.Visibility = Visibility.Hidden;
                return;
            }

            DetailsImage.Source = ImageCreation.CreateBitmapImageFromUriString(UserView.SelectedProduct.Uri);
            _detailsName.Content = UserView.SelectedProduct.Name;
            _priceLabel.Content = $"{UserView.SelectedProduct.Price} kr";
            // Necessary for text-wrapping to work. Not setting the MaxWidth property will cause the TextBlock.Width to grow beyond its bounds.
            _description.MaxWidth = ((ScrollViewer)_description.Parent).ActualWidth;
            _description.Text = UserView.SelectedProduct.Description;
            _removeFromCartButton.Tag = UserView.SelectedProduct;
            _addToCartButton.Tag = UserView.SelectedProduct;
            // Hide the _removeFromCartButton if the selected product is not in the shopping cart.
            _removeFromCartButton.Visibility = Store.ShoppingCart.Products.ContainsKey(UserView.SelectedProduct) ? Visibility.Visible : Visibility.Hidden;
            _root.Visibility = Visibility.Visible;
        }

        /******************************************************/
        /******************* Event Handling *******************/
        /******************************************************/

        private static void DetailsRemoveFromCartButton_Click(object sender, RoutedEventArgs e)
        {
            var product = (Product)((Button)sender).Tag;
            if (product != null)
            {
                Store.ShoppingCart.RemoveProduct(product);
                Store.ShoppingCart.SaveToFile(DataManager.ShoppingCartCSV);
                UserView.UpdateGUI();
            }
        }

        private static void DetailsAddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            var product = (Product)((Button)sender).Tag;
            if (product != null)
            {
                Store.ShoppingCart.AddProduct(product, 1);
                Store.ShoppingCart.SaveToFile(DataManager.ShoppingCartCSV);
                UserView.UpdateGUI();
            }
        }

        private static void Root_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Necessary for text-wrapping to work. Not setting the MaxWidth property will cause the TextBlock.Width to grow beyond its bounds.
            _description.MaxWidth = ((ScrollViewer)_description.Parent).ActualWidth;
        }
    }
}
