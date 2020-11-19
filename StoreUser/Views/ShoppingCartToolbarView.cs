using StoreCommon;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreUser.Views
{
    public static class ShoppingCartToolbarView
    {
        private static Grid _root;

        private static Grid _summaryCountPrice;
        private static Label _summary_count;
        private static TextBlock _summary_totalPrice;
        private static TextBlock _summary_finalPrice;

        private static TextBox _discountCodeInput;
        private static Button _discountCodeSubmit;

        public static Grid Init()
        {
            CreateGUI();
            UpdateGUI();
            return _root;
        }

        public static void CreateGUI()
        {
            _root = new Grid { Margin = new Thickness(2.5), };
            _root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            _root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            _root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            _root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            _root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            _root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            _root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            // Children of _root:
            {
                /*-------------------*/
                /*----- Child 1 -----*/
                /*-------------------*/
                //_summary_countPrice = new StackPanel { Orientation = Orientation.Vertical };
                _summaryCountPrice = new Grid { Margin = new Thickness(2.5), };
                _summaryCountPrice.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                _summaryCountPrice.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                _summaryCountPrice.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                // _shopppingCart_summary children:
                {
                    _summary_count = new Label
                    {
                        Content = $"Items: {Store.ShoppingCart.Products.Sum(p => p.Value)}",
                        Margin = new Thickness(2.5),
                        Padding = new Thickness(0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    //Grid.SetColumnSpan(_summary_count, 2);
                    _summaryCountPrice.Children.Add(_summary_count);
                    //_summary_count.Background = Brushes.LightSalmon;

                    _summary_totalPrice = new TextBlock
                    {
                        TextDecorations = TextDecorations.Strikethrough,
                        Text = $"Total: {Store.ShoppingCart.TotalSum} {Store.Currency.Symbol}",
                        Margin = new Thickness(2.5),
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    Grid.SetRow(_summary_totalPrice, 1);
                    _summaryCountPrice.Children.Add(_summary_totalPrice);
                    //_summary_totalPrice.Background = Brushes.LightBlue;

                    // Active discount Label
                    _summary_finalPrice = new TextBlock
                    {
                        Visibility = Visibility.Collapsed,
                        Text = $"Total: {Store.ShoppingCart.FinalSum} {Store.Currency.Symbol}",
                        Margin = new Thickness(2.5),
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    Grid.SetRow(_summary_finalPrice, 2);
                    //Grid.SetColumn(_summary_finalPrice, 1);
                    _summaryCountPrice.Children.Add(_summary_finalPrice);
                    //_summary_finalPrice.Background = Brushes.LightSalmon;
                }
                Grid.SetColumn(_summaryCountPrice, 0);
                _root.Children.Add(_summaryCountPrice);

                /*-------------------*/
                /*----- Child 2 -----*/
                /*-------------------*/
                var discountForm = new WrapPanel
                {
                    Orientation = Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                // Children of 'discountForm'
                {
                    _discountCodeInput = new TextBox
                    {
                        Width = 250,
                        MinWidth = 250,
                        MaxWidth = 250,
                        Text = "Enter your discount code here...",
                        // Placeholder text
                        Tag = "Enter your discount code here...",
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        //HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Padding = new Thickness(5),
                        Margin = new Thickness(2.5),
                    };
                    _discountCodeInput.GotFocus += EventHandler._discountCodeInput_GotFocus;
                    _discountCodeInput.LostFocus += EventHandler._discountCodeInput_LostFocus;
                    _discountCodeInput.TextChanged += EventHandler._discountCodeInput_TextChanged;
                    _discountCodeInput.KeyUp += EventHandler._discountCodeInput_KeyUp;
                    discountForm.Children.Add(_discountCodeInput);

                    _discountCodeSubmit = new Button
                    {
                        Content = "+ Add discount code",
                        Background = Brushes.LightGreen,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        Padding = new Thickness(5),
                        Margin = new Thickness(2.5),
                    };
                    _discountCodeSubmit.Click += EventHandler.discountSubmitEventHandler;
                    discountForm.Children.Add(_discountCodeSubmit);

                    Grid.SetColumn(discountForm, 2);
                    _root.Children.Add(discountForm);
                }

                /*-------------------*/
                /*----- Child 3 -----*/
                /*-------------------*/
                var gridSplitter = new GridSplitter
                {
                    Width = 1,
                    Background = Brushes.Gray,
                    IsEnabled = false,
                    Margin = new Thickness(5),
                };
                Grid.SetColumn(gridSplitter, 4);
                _root.Children.Add(gridSplitter);

                /*-------------------*/
                /*----- Child 4 -----*/
                /*-------------------*/
                var stackPanel_saveLoadButtons = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                };
                var shoppingCart_saveButton = new Button
                {
                    Content = "Save Shopping Cart",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(5),
                    Margin = new Thickness(2.5),
                };
                shoppingCart_saveButton.Click += EventHandler.ShoppingCart_saveButton_Click;

                var shoppingCart_loadButton = new Button
                {
                    Content = "Load Shopping Cart",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(5),
                    Margin = new Thickness(2.5),
                };
                shoppingCart_loadButton.Click += EventHandler.ShoppingCart_loadButton_Click;
                stackPanel_saveLoadButtons.Children.Add(shoppingCart_saveButton);
                stackPanel_saveLoadButtons.Children.Add(shoppingCart_loadButton);
                Grid.SetColumn(stackPanel_saveLoadButtons, 6);
                _root.Children.Add(stackPanel_saveLoadButtons);
            }
        }

        public static void UpdateGUI()
        {
            // Update Shooping Cart Toolbar values (item count, total price & total price with discount if activated):
            _summary_count.Content = $"Items: {Store.ShoppingCart.Products.Sum(p => p.Value)}";
            _summary_totalPrice.Text = $"Total: {Store.ShoppingCart.TotalSum} {Store.Currency.Symbol}";
            _summary_finalPrice.Text = $"Total: {Store.ShoppingCart.FinalSum} {Store.Currency.Symbol}";

            if (Store.ShoppingCart.ActiveDiscountCode != null)
            {
                _summary_totalPrice.TextDecorations = TextDecorations.Strikethrough;
                _summary_finalPrice.Text += $" (-{Store.ShoppingCart.ActiveDiscountCode.Percentage * 100}%)";
            }
            else
            {
                _summary_totalPrice.TextDecorations = null;
            }
        }

        private static void ResetDiscountCodeForm()
        {
            _discountCodeInput.ClearValue(TextBox.BorderBrushProperty);
            _discountCodeInput.ClearValue(TextBox.BackgroundProperty);
            _discountCodeInput.IsEnabled = true;
            _summary_finalPrice.Visibility = Visibility.Collapsed;
            _discountCodeInput.IsEnabled = true;
            _discountCodeSubmit.Content = "+ Add discount code";
            _discountCodeSubmit.Background = Brushes.LightGreen;
        }

        private static class EventHandler
        {
            internal static void discountSubmitEventHandler(object s, RoutedEventArgs e) // TODO: move to namespace
            {
                if ((string)_discountCodeSubmit.Content == "+ Add discount code")
                {
                    _discountCodeSubmit.IsEnabled = false;
                    bool success = Store.AddDiscountCode(_discountCodeInput.Text);

                    if (success)
                    {
                        _discountCodeInput.IsEnabled = false;
                        _discountCodeInput.BorderBrush = Brushes.Green;
                        _discountCodeInput.Background = Brushes.LightGreen;
                        _summary_finalPrice.Visibility = Visibility.Visible;
                        _discountCodeSubmit.Content = "- Remove discount code";
                        _discountCodeSubmit.Background = Brushes.LightPink;
                        UpdateGUI();
                    }
                    else
                    {
                        _discountCodeInput.BorderBrush = Brushes.Red;
                        _discountCodeInput.Background = Brushes.LightPink;
                    }

                    _discountCodeSubmit.IsEnabled = true;
                }
                else if ((string)_discountCodeSubmit.Content == "- Remove discount code")
                {
                    Store.RemoveDiscountCode();
                    ResetDiscountCodeForm();
                    UpdateGUI();
                }
            }

            internal static void _discountCodeInput_GotFocus(object sender, RoutedEventArgs e)
            {
                if (_discountCodeInput.Text == (string)_discountCodeInput.Tag)
                {
                    _discountCodeInput.Text = "";
                }
            }

            internal static void _discountCodeInput_LostFocus(object sender, RoutedEventArgs e)
            {
                if (_discountCodeInput.Text == "")
                {
                    _discountCodeInput.Text = (string)_discountCodeInput.Tag;
                    _discountCodeInput.ClearValue(TextBox.BorderBrushProperty);
                    _discountCodeInput.ClearValue(TextBox.BackgroundProperty);
                }
            }

            internal static void _discountCodeInput_TextChanged(object sender, TextChangedEventArgs e)
            {
                if (_discountCodeInput.Text == "" || _discountCodeInput.Text == (string)_discountCodeInput.Tag)
                {
                    _discountCodeSubmit.IsEnabled = false;
                    _discountCodeInput.ClearValue(TextBox.BorderBrushProperty);
                    _discountCodeInput.ClearValue(TextBox.BackgroundProperty);
                }
                else
                {
                    _discountCodeSubmit.IsEnabled = true;
                }
            }

            internal static void _discountCodeInput_KeyUp(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    EventHandler.discountSubmitEventHandler(sender, e);
                }
            }

            internal static void ShoppingCart_saveButton_Click(object sender, RoutedEventArgs e)
            {
                Store.SaveShoppingCart();
            }

            internal static void ShoppingCart_loadButton_Click(object sender, RoutedEventArgs e)
            {
                Store.LoadShoppingCart(AppFolder.ShoppingCartCSV);
                ResetDiscountCodeForm();
                UserView.UpdateGUI();
            }
        }
    }
}
