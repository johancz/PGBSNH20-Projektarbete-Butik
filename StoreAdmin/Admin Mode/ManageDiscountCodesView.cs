using StoreCommon;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StoreAdmin.Views
{
    public static class ManageDiscountCodesView
    {
        // This class contains the content of "Manage Discount Codes"-tab, selfcontained with both frameworkelements and events.
        // It returns a "root"-element which is added to the main gui in "AdminHybridWindow.CreateAdminGUI()".

        private static ScrollViewer _rootScrollViewer;
        private static Grid _grid;

        private static List<DiscountCode> _newDiscountCodes;
        private static List<TextBox> _textboxesWithInvalidData = new List<TextBox>();

        public static ScrollViewer Init()
        {
            _newDiscountCodes = Store.DiscountCodes;
            CreateGUI();
            UpdateGUI();
            return _rootScrollViewer;
        }

        public static void CreateGUI()
        {
            _rootScrollViewer = new ScrollViewer {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(20),
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            };
            _grid = new Grid { Margin = new Thickness(0, 0, 0, 20), HorizontalAlignment = HorizontalAlignment.Center, };
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto), });
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto), });
            _rootScrollViewer.Content = _grid;
        }

        public static void UpdateGUI()
        {
            // Create the "Header"-row
            var codeHeader = new Label { FontSize = 16, Content = "Discount Code", Padding = new Thickness(5), };
            var percentageHeader = new Label { FontSize = 16, Content = "Percentage (0 - 1)", Padding = new Thickness(5), };
            AppendGridRow((codeHeader, null), (percentageHeader, null));

            // Create a row for each DiscountCode
            _newDiscountCodes.ForEach(discountCode => AppendDiscountCodeRow(discountCode));
            
            var gridSplitter = new GridSplitter
            {
                IsEnabled = false,
                Background = Brushes.Gray,
                Height = 2,
                Margin = new Thickness(0, 5, 0, 5),
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };
            Grid.SetColumnSpan(gridSplitter, 3);
            AppendGridRow((gridSplitter, null));

            // Create a 2nd "header"-row for the controls used to create a new DiscountCode.
            var codeHeader2 = new Label { FontSize = 14, Content = "Discount Code", Padding = new Thickness(5), };
            var percentageHeader2 = new Label { FontSize = 14, Content = "Percentage (0 - 1)", Padding = new Thickness(5), };
            AppendGridRow((codeHeader2, null), (percentageHeader2, null));

            // Create the Button and TextBoxes used for creating new DiscountCodes.
            var newCodeTextBox = new TextBox { Padding = new Thickness(5), };
            var newPercentageTextBox = new TextBox { Padding = new Thickness(5), };
            var addButton = new Button { Content = "Add Discount Code", Padding = new Thickness(5), };
            addButton.Click += AddButton_Click;
            AppendGridRow((newCodeTextBox, null), (newPercentageTextBox, null), (addButton, null));

            _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto), });
            var gridSplitter2 = new GridSplitter
            {
                IsEnabled = false,
                Background = Brushes.Gray,
                Height = 2,
                Margin = new Thickness(0, 5, 0, 5),
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };
            Grid.SetColumnSpan(gridSplitter2, 3);
            AppendGridRow((gridSplitter2, null));

            // Button for saving the new DiscountCodes-list to file.
            var saveButton = new Button { Content = "Save Discount Codes", Padding = new Thickness(5), };
            saveButton.Click += SaveButton_Click;
            AppendGridRow((saveButton, 2));
        }

        // Takes any number of tuples or an array of tuples, where each tuple is an element and the column the element should be inserted into.
        private static void AppendGridRow(params (FrameworkElement element, int? column)[] elements)
        {
            int row = _grid.RowDefinitions.Count;
            _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto), });

            for (int i = 0; i < elements.Length; i++)
            {
                var (element, column) = elements[i];
                Grid.SetRow(element, row);
                // Use rowData.column if it isn't null, if null use 'i'.
                // This for example allows for a row to look like this:
                // " | 'column[0]' contains element0 | 'column[1]' is empty | 'column[2]' contains element1 | "
                // if the tuple[] 'elements' looks like "... { (element0, null), (element1, 2) }".
                Grid.SetColumn(element, column ?? i);
                _grid.Children.Add(element);
            }
        }

        private static void AppendDiscountCodeRow(DiscountCode discountCode)
        {
            var codeTextBox = new TextBox { Text = discountCode.Code, Padding = new Thickness(5), Tag = discountCode, };
            codeTextBox.TextChanged += CodeTextBox_TextChanged;

            var percentageTextBox = new TextBox { Text = discountCode.Percentage.ToString(), Padding = new Thickness(5), Tag = discountCode, };
            percentageTextBox.TextChanged += PercentageTextBox_TextChanged;

            var deleteButton = new Button { Content = "(X) Delete Discount Code", Padding = new Thickness(5), Tag = discountCode, };
            deleteButton.Click += DeleteButton_Click;

            AppendGridRow((codeTextBox, null), (percentageTextBox, null), (deleteButton, null));
        }

        /***********************/
        /*** Event Handling ***/
        /***********************/

        private static void CodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;

            try
            {
                DiscountCode discountCode = (DiscountCode)textBox.Tag;
                discountCode.SetValues(textBox.Text, discountCode.Percentage);

                if (textBox.Background == Brushes.LightPink)
                {
                    textBox.ClearValue(TextBox.BackgroundProperty);
                    _textboxesWithInvalidData.Remove(textBox);
                }
            }
            catch (Exception)
            {
                textBox.Background = Brushes.LightPink;
                if (!_textboxesWithInvalidData.Contains(textBox))
                {
                    _textboxesWithInvalidData.Add(textBox);
                }
            }
        }

        private static void PercentageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;

            try
            {
                if (!double.TryParse(textBox.Text, out double percentage))
                {
                    throw new Exception("Could not parse to Double");
                }

                var discountCode = (DiscountCode)textBox.Tag;
                discountCode.SetValues(discountCode.Code, percentage);
                if (textBox.Background == Brushes.LightPink)
                {
                    textBox.ClearValue(TextBox.BackgroundProperty);
                    _textboxesWithInvalidData.Remove(textBox);
                }
            }
            catch (Exception)
            {
                textBox.Background = Brushes.LightPink;
                if (!_textboxesWithInvalidData.Contains(textBox))
                {
                    _textboxesWithInvalidData.Add(textBox);
                }
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
            var buttonIndex = _grid.Children.IndexOf((Button)sender);

            try
            {
                string newCode = ((TextBox)_grid.Children[buttonIndex - 2]).Text;

                if (!double.TryParse(((TextBox)_grid.Children[buttonIndex - 1]).Text, out double newPercentage))
                {
                    throw new Exception("The \"Percentage\" value is not valid, enter a number between 0-1 (inclusive).");
                }

                _newDiscountCodes.Add(new DiscountCode(newCode, newPercentage));

                _grid.Children.Clear();
                _grid.RowDefinitions.Clear();
                UpdateGUI();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error!");
            }
        }

        private static void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_textboxesWithInvalidData.Count > 0)
            {
                MessageBox.Show("Please correct any values marked with a light-red background.", "Invalid Data!");
            }
            else
            {
                Store.DiscountCodes = _newDiscountCodes;
                Store.SaveDiscountCodesToFile();
                MessageBox.Show("The Discount Codes were successfully saved to file.");
            }
        }
    }
}
