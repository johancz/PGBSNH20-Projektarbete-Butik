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
    public static class DetailsPanelView
    {
        private static Grid _root;

        private static Image _rightColumn_DetailsImage;
        private static Label _rightColumn_DetailsName;
        private static TextBlock _rightColumn_DetailsDescription;
        private static Button _rightColumn_detailsAddToCartButton;
        private static Button _rightColumn_DetailsRemoveFromCartButton;
        private static Label _rightColumn_DetailsPrice;

        public static Grid Init()
        {
            CreateGUI();
            UpdateGUI();
            return _root;
        }

        public static void CreateGUI()
        {
            //  Right Column Content Root: Grid
            _root = new Grid
            {
                ShowGridLines = true,
                Visibility = Visibility.Hidden,
            };

            _root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            // Needs to be GridUnitType.Star for MaxWidth to work on the TextBlock containing the product's description.
            _root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Create and add a Product.Image to the right column's root (StackPanel)
            _root.Children.Add(_rightColumn_DetailsImage = new Image());

            // Details Column: name, price, description and shopping cart buttons.
            {
                var detailsColumn_detailsGrid = new Grid { ShowGridLines = true };
                detailsColumn_detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                detailsColumn_detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Grid.SetRow(detailsColumn_detailsGrid, 1);
                _root.Children.Add(detailsColumn_detailsGrid);

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
                    _rightColumn_detailsAddToCartButton.Click += EventHandler.RightColumn_DetailsAddToCartButton_Click;

                    // Create "Remove from Shopping Cart" button with "click"-event listener.
                    _rightColumn_DetailsRemoveFromCartButton = new Button
                    {
                        Padding = new Thickness(5),
                        Content = new Label { Content = "(-) Remove from shopping cart", HorizontalAlignment = HorizontalAlignment.Left },
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Left,

                    };
                    _rightColumn_DetailsRemoveFromCartButton.Click += EventHandler.RightColumn_DetailsRemoveFromCartButton_Click;

                    _rightColumn_DetailsPrice = new Label { FontSize = 16 };
                    rightColumn_detailsPanel_shoppingCartButtons.Children.Add(_rightColumn_DetailsPrice);
                    // Add buttons to their parent StackPanel and then add the StackPanel to the "details"-StackPanel
                    rightColumn_detailsPanel_shoppingCartButtons.Children.Add(_rightColumn_detailsAddToCartButton);
                    rightColumn_detailsPanel_shoppingCartButtons.Children.Add(_rightColumn_DetailsRemoveFromCartButton);
                    Grid.SetColumn(rightColumn_detailsPanel_shoppingCartButtons, 0);
                    detailsColumn_detailsGrid.Children.Add(rightColumn_detailsPanel_shoppingCartButtons);
                }
            }
        }

        public static void UpdateGUI()
        {
        }

        internal static void UpdateDetailsColumn(Product product)
        {
            _rightColumn_DetailsImage.Source = Helpers.CreateBitmapImageFromUriString(product.Uri);

            _rightColumn_DetailsName.Content = product.Name;
            _rightColumn_DetailsPrice.Content = $"{product.Price} kr";
            // Necessary for text-wrapping to work. Not setting the MaxWidth property will cause the TextBlock.Width to grow beyond it's bounds.
            _rightColumn_DetailsDescription.MaxWidth = ((ScrollViewer)_rightColumn_DetailsDescription.Parent).ActualWidth;
            _rightColumn_DetailsDescription.Text = product.Description;
            _rightColumn_DetailsRemoveFromCartButton.Tag = product;
            _rightColumn_detailsAddToCartButton.Tag = product;
            _rightColumn_DetailsRemoveFromCartButton.Visibility = Store.ShoppingCart.Products.ContainsKey(product)
                                                                  ? Visibility.Visible
                                                                  : Visibility.Hidden;
            _root.Visibility = Visibility.Visible;
        }

        internal static class EventHandler
        {
            internal static void RightColumn_DetailsRemoveFromCartButton_Click(object sender, RoutedEventArgs e)
            {
                // TODO(johancz): Error/Exception-handling
                var product = (Product)((Button)sender).Tag;
                Store.ShoppingCart.RemoveProduct(product);
                UserView.UpdateGUI();
            }

            internal static void RightColumn_DetailsAddToCartButton_Click(object sender, RoutedEventArgs e)
            {
                // TODO(johancz): Error/Exception-handling
                var product = (Product)((Button)sender).Tag;
                Store.ShoppingCart.AddProduct(product, 1);
                UserView.UpdateGUI();
            }

            internal static void External_RootElement_SizeChanged(object sender, SizeChangedEventArgs e)
            {
                _rightColumn_DetailsDescription.MaxWidth = ((ScrollViewer)_rightColumn_DetailsDescription.Parent).ActualWidth;
            }
        }
    }
}
