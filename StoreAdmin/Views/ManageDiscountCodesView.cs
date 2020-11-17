using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using StoreCommon;

namespace StoreAdmin.Views
{
    public static class ManageDiscountCodesView
    {
        private static TabItem _root;
        private static Grid _grid;

        private static List<DiscountCode> _newDiscountCodes;
        private static int _errorsInNewData;

        public static TabItem Init()
        {
            CreateGUI();
            //UpdateData();
            //UpdateGUI();
            return _root;
        }

        public static void CreateGUI()
        {
            _newDiscountCodes = new List<DiscountCode>(Store.DiscountCodes);
            _root = new TabItem { Header = "Manage Discount Codes", };

            var rootScrollViewer = new ScrollViewer();
            _grid = new Grid();
            int row = 0;
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto), });
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto), });
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto), });

            // Create the "Header"-row
            {
                _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto), });
                var newCodeTextBox = new Label
                {
                    Content = "Discount Code",
                    Padding = new Thickness(5),
                };
                Grid.SetRow(newCodeTextBox, row);
                _grid.Children.Add(newCodeTextBox);

                var newPercentageTextBox = new Label
                {
                    Content = "Percentage (0 - 1)",
                    Padding = new Thickness(5),
                };
                Grid.SetRow(newPercentageTextBox, row);
                Grid.SetColumn(newPercentageTextBox, 1);
                _grid.Children.Add(newPercentageTextBox);

                var addButton = new Label
                {
                    Content = "Remove/Add",
                    Padding = new Thickness(5),
                };
                Grid.SetRow(addButton, row);
                Grid.SetColumn(addButton, 2);
                _grid.Children.Add(addButton);

                row++;
            }

            foreach (DiscountCode discountCode in Store.DiscountCodes)
            {
                _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto), });

                var codeTextBox = new TextBox
                {
                    Text = discountCode.Code,
                    Padding = new Thickness(5),
                    Tag = discountCode,
                };
                codeTextBox.LostFocus += CodeTextBox_LostFocus;
                Grid.SetRow(codeTextBox, row);
                _grid.Children.Add(codeTextBox);

                var percentageTextBox = new TextBox
                {
                    Text = discountCode.Percentage.ToString(),
                    Padding = new Thickness(5),
                    Tag = discountCode,
                };
                percentageTextBox.LostFocus += PercentageTextBox_LostFocus;
                Grid.SetRow(percentageTextBox, row);
                Grid.SetColumn(percentageTextBox, 1);
                _grid.Children.Add(percentageTextBox);

                var deleteButton = new Button
                {
                    Content = "X",
                    Padding = new Thickness(5),
                    Tag = discountCode,
                };
                deleteButton.Click += DeleteButton_Click;
                Grid.SetRow(deleteButton, row);
                Grid.SetColumn(deleteButton, 2);
                _grid.Children.Add(deleteButton);

                row++;
            }

            _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto), });
            var gridSplitter = new GridSplitter
            {
                Height = 1,
                Background = Brushes.Gray,
                IsEnabled = false,
                Margin = new Thickness(5),
            };
            Grid.SetRow(gridSplitter, ++row); // pre-increment = increment value before it is passed to Grid.SetRow().
            Grid.SetColumnSpan(gridSplitter, 3);
            _grid.Children.Add(gridSplitter);

            // Create the Button and TextBoxes used for creating new DiscountCodes.
            {
                _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto), });
                row++;

                var newCodeTextBox = new TextBox
                {
                    Padding = new Thickness(5),
                };
                Grid.SetRow(newCodeTextBox, row);
                _grid.Children.Add(newCodeTextBox);

                var newPercentageTextBox = new TextBox
                {
                    Padding = new Thickness(5),
                };
                Grid.SetRow(newPercentageTextBox, row);
                Grid.SetColumn(newPercentageTextBox, 1);
                _grid.Children.Add(newPercentageTextBox);

                var addButton = new Button
                {
                    Content = "Add Discount Code",
                    Padding = new Thickness(5),
                };
                addButton.Click += AddButton_Click;
                Grid.SetRow(addButton, row);
                Grid.SetColumn(addButton, 2);
                _grid.Children.Add(addButton);
            }

            _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto), });
            var saveButton = new Button
            {
                Content = "Add Discount Code",
                Padding = new Thickness(5),
            };
            saveButton.Click += SaveButton_Click; ;
            Grid.SetRow(saveButton, ++row);
            Grid.SetColumn(saveButton, 2);
            _grid.Children.Add(saveButton);

            rootScrollViewer.Content = _grid;
            _root.Content = rootScrollViewer;
        }

        public static void UpdateGUI()
        {
        }

        //private static void UpdateData()
        //{

        //}

        internal static void UpdateShoppingCartView()
        {
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto), });
        }

        /***********************/
        /*** Event Handling ***/
        /***********************/

        private static void CodeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;

            try
            {
                DiscountCode discountCode = (DiscountCode)textBox.Tag;
                discountCode.SetValues(textBox.Text, discountCode.Percentage);
                if (textBox.Background == Brushes.LightPink)
                {
                    _errorsInNewData--;
                    textBox.Background = Brushes.LightGreen;
                }
            }
            catch (Exception)
            {
                // Split trims the 
                //MessageBox.Show(error.Message.Split('(')[0].TrimEnd(), "Error!");
                textBox.Background = Brushes.LightPink;
                _errorsInNewData++;
            }
        }

        private static void PercentageTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            double percentage;

            try
            {
                if (!Double.TryParse(textBox.Text, out percentage))
                {
                    throw new Exception("Could not parse to Double");
                }
                //((DiscountCode)textBox.Tag).Percentage = percentage;

                var discountCode = (DiscountCode)textBox.Tag;
                discountCode.SetValues(discountCode.Code, percentage);
                if (textBox.Background == Brushes.LightPink)
                {
                    _errorsInNewData--;
                    textBox.Background = Brushes.LightGreen;
                }
            }
            catch (Exception)
            {
                //MessageBox.Show(error.Message, "Error!");
                textBox.Background = Brushes.LightPink;
                _errorsInNewData++;
            }
        }

        private static void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure? This cannot be undone.", "Delete Discount Code?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var button = (Button)sender;
                _newDiscountCodes.Remove((DiscountCode)(button).Tag);
                var index = _grid.Children.IndexOf(button);
                _grid.Children.RemoveRange(index - 2, 3);
            }
        }

        private static void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DiscountCode newDiscountCode;
            var buttonIndex = _grid.Children.IndexOf((Button)sender);

            try
            {
                string newCode = ((TextBox)_grid.Children[buttonIndex - 2]).Text;
                double newPercentage;

                if (Double.TryParse(((TextBox)_grid.Children[buttonIndex - 1]).Text, out newPercentage))
                {
                    newDiscountCode = new DiscountCode(newCode, newPercentage);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error!");
            }
        }

        private static void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_errorsInNewData == 0)
            {
                Store.DiscountCodes = _newDiscountCodes;
            }
            else
            {
                MessageBox.Show("Please correct any values marked with a light-red background.", "Invalid Data!");
            }
        }
    }
}
