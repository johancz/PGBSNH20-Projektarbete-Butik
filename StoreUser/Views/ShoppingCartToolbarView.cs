﻿using StoreCommon;
using System;
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

            // Children of _root:
            {
                /*-------------------*/
                /*----- Child 0 -----*/
                /*-------------------*/
                _summaryCountPrice = new Grid { Margin = new Thickness(2.5), };
                _summaryCountPrice.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                _summaryCountPrice.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                _summaryCountPrice.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                // _summaryCountPrice children:
                {
                    _summary_count = new Label
                    {
                        Content = $"Items: {Store.ShoppingCart.Products.Sum(p => p.Value)}",
                        Margin = new Thickness(2.5),
                        Padding = new Thickness(0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    _summaryCountPrice.Children.Add(_summary_count);

                    _summary_totalPrice = new TextBlock
                    {
                        TextDecorations = TextDecorations.Strikethrough,
                        Text = $"Total: {Math.Round(Store.ShoppingCart.TotalSum, 2)} {Store.Currency.Symbol}",
                        Margin = new Thickness(2.5),
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    Grid.SetRow(_summary_totalPrice, 1);
                    _summaryCountPrice.Children.Add(_summary_totalPrice);

                    // Active discount Label
                    _summary_finalPrice = new TextBlock
                    {
                        Visibility = Visibility.Hidden,
                        Text = $"Total: {Math.Round(Store.ShoppingCart.FinalSum, 2)} {Store.Currency.Symbol}",
                        Margin = new Thickness(2.5),
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    Grid.SetRow(_summary_finalPrice, 2);
                    _summaryCountPrice.Children.Add(_summary_finalPrice);
                }
                Grid.SetColumn(_summaryCountPrice, 0);
                _root.Children.Add(_summaryCountPrice);

                /*-------------------*/
                /*----- Child 1 -----*/
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
                        VerticalAlignment = VerticalAlignment.Center,
                        Padding = new Thickness(5),
                        Margin = new Thickness(2.5),
                    };
                    _discountCodeInput.GotFocus += DiscountCodeInput_GotFocus;
                    _discountCodeInput.LostFocus += DiscountCodeInput_LostFocus;
                    _discountCodeInput.TextChanged += DiscountCodeInput_TextChanged;
                    _discountCodeInput.KeyUp += DiscountCodeInput_KeyUp;
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
                    _discountCodeSubmit.Click += DiscountSubmitEventHandler;
                    discountForm.Children.Add(_discountCodeSubmit);

                }
                Grid.SetColumn(discountForm, 2);
                _root.Children.Add(discountForm);
            }
        }

        public static void UpdateGUI()
        {
            // Update Shooping Cart Toolbar values (item count, total price & total price with discount if activated):
            _summary_count.Content = $"Items: {Store.ShoppingCart.Products.Sum(p => p.Value)}";
            _summary_totalPrice.Text = $"Total: {Math.Round(Store.ShoppingCart.TotalSum, 2)} {Store.Currency.Symbol}";
            _summary_finalPrice.Text = $"Total: {Math.Round(Store.ShoppingCart.FinalSum, 2)} {Store.Currency.Symbol}";

            if (Store.ShoppingCart.ActiveDiscountCode != null)
            {
                _summary_totalPrice.TextDecorations = TextDecorations.Strikethrough;
                _summary_finalPrice.Text += $" (-{Store.ShoppingCart.ActiveDiscountCode.Percentage * 100}%)";
            }
            else
            {
                _summary_totalPrice.TextDecorations = null;
                ResetDiscountCodeForm();
            }
        }

        private static void ResetDiscountCodeForm()
        {
            _discountCodeInput.ClearValue(TextBox.BorderBrushProperty);
            _discountCodeInput.ClearValue(TextBox.BackgroundProperty);
            _discountCodeInput.IsEnabled = true;
            _summary_finalPrice.Visibility = Visibility.Hidden;
            _discountCodeInput.IsEnabled = true;
            _discountCodeSubmit.Content = "+ Add discount code";
            _discountCodeSubmit.Background = Brushes.LightGreen;
        }

        /******************************************************/
        /******************* Event Handling *******************/
        /******************************************************/

        private static void DiscountSubmitEventHandler(object s, RoutedEventArgs e)
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
                    ShoppingCartListView.Update();
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
                ShoppingCartListView.Update();
                UpdateGUI();
            }
        }

        private static void DiscountCodeInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_discountCodeInput.Text == (string)_discountCodeInput.Tag)
            {
                _discountCodeInput.Text = "";
            }
        }

        private static void DiscountCodeInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_discountCodeInput.Text == "")
            {
                _discountCodeInput.Text = (string)_discountCodeInput.Tag;
                _discountCodeInput.ClearValue(TextBox.BorderBrushProperty);
                _discountCodeInput.ClearValue(TextBox.BackgroundProperty);
            }
        }

        private static void DiscountCodeInput_TextChanged(object sender, TextChangedEventArgs e)
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

        private static void DiscountCodeInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DiscountSubmitEventHandler(sender, e);
            }
        }
    }
}
