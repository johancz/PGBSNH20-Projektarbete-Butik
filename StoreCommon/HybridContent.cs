using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreCommon
{
    public class DetailsPanel : CommonFrameWork
    {
        //private static Product Selected;
        public string Tag;
        private Grid Parent;
        private Grid _rightColumnContentRoot;
        private Grid _detailsColumn_detailsGrid;
        private TextBox _rightColumn_DetailsName;
        private TextBox _rightColumn_DetailsDescription;
        private StackPanel _rightColumn_detailsPanel_nameAndPrice;
        public DetailsPanel(Grid parent, Brush brush, string tag)
        {
            Tag = tag;
            Parent = parent;
            detailsPanels.Add(this);

            _rightColumnContentRoot = new Grid { ShowGridLines = true, Background = brush };
   
            _rightColumnContentRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _rightColumnContentRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            // Create and add a Product.Image to the right column's root (StackPanel)
            var rightColumn_DetailsImage = new Image { Tag = "rightcolumn detailsimage" };

            Elements.Add(rightColumn_DetailsImage);

            _rightColumnContentRoot.Children.Add(rightColumn_DetailsImage);

            var detailsColumn_detailsGrid = new Grid { ShowGridLines = true };
            _detailsColumn_detailsGrid = detailsColumn_detailsGrid;

            detailsColumn_detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            detailsColumn_detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Grid.SetRow(detailsColumn_detailsGrid, 1);
            _rightColumnContentRoot.Children.Add(detailsColumn_detailsGrid);

            var detailsColumn_namePriceDescription = new Grid { ShowGridLines = true };
            detailsColumn_namePriceDescription.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            detailsColumn_namePriceDescription.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            // Create the product "Name" and "Price" labels and a StackPanel-parent for them. Add the parent to the detailsPanel.

            _rightColumn_detailsPanel_nameAndPrice = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5),
            };
            _rightColumnContentRoot.Visibility = Visibility.Hidden;
            _rightColumn_DetailsName = new TextBox
            {
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Background = Brushes.Transparent,
                IsReadOnly = true
            };
            _rightColumn_detailsPanel_nameAndPrice.Children.Add(_rightColumn_DetailsName);
            Grid.SetRow(_rightColumn_detailsPanel_nameAndPrice, 0);
            detailsColumn_namePriceDescription.Children.Add(_rightColumn_detailsPanel_nameAndPrice);

            _rightColumn_DetailsDescription = new TextBox
            {
                Tag = "rightcolumn detailsdescription",
                TextWrapping = TextWrapping.Wrap,
                Background = Brushes.Transparent,
                IsReadOnly = true
            };
            Elements.Add(_rightColumn_DetailsDescription);
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

            // Add the right-column to the "root"-Grid.
            Grid.SetColumn(_rightColumnContentRoot, 1);
            Parent.Children.Add(_rightColumnContentRoot);
        }
        public void EditProductContent()
        {

        }
        public void NewProductContent()
        {
            _rightColumnContentRoot.Visibility = Visibility.Visible;
            _rightColumn_DetailsName.IsReadOnly = false;
            _rightColumn_DetailsName.Text = "Enter Title";
            _rightColumn_DetailsName.Background = Brushes.White;
            _rightColumn_DetailsDescription.IsReadOnly = false;
            _rightColumn_DetailsDescription.Text = "Enter Description...";
            _rightColumn_DetailsDescription.Background = Brushes.White;
        }
        public void AddNewProductButtonPanel()
        {
            var rightColumn_detailsPanel_NewProductsButton = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(5),
                Tag = "new product buttons"
            };

            Grid.SetColumn(rightColumn_detailsPanel_NewProductsButton, 0);
            _detailsColumn_detailsGrid.Children.Add(rightColumn_detailsPanel_NewProductsButton);

            var saveChangesButton = new Button
            {
                Tag = "save changes",
                Padding = new Thickness(5),
                Content = new Label { Content = "Save Changes", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            Elements.Add(saveChangesButton);
            rightColumn_detailsPanel_NewProductsButton.Children.Add(saveChangesButton);

        }
        public void AddAdminButtonPanel()
        {
            var rightColumn_detailsPanel_AdminButtons = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(5),
                Tag = "admin buttons"
            };

            var editButton = new Button
            {
                Tag = "Edit",
                Padding = new Thickness(5),
                Content = new Label { Content = "Edit", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            Elements.Add(editButton);

            editButton.Click += EditButton_Click;

            var saveChangesButton = new Button
            {
                Tag = "save changes",
                Padding = new Thickness(5),
                Content = new Label { Content = "Save Changes", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            Elements.Add(saveChangesButton);
            saveChangesButton.Click += SaveChangesButton_Click;

            var removeButton = new Button
            {
                Tag = "Remove",
                Padding = new Thickness(5),
                Content = new Label { Content = "Remove", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            Elements.Add(removeButton);

            var changeImageButton = new Button
            {
                Tag = "Change Image",
                Padding = new Thickness(5),
                Content = new Label { Content = "Change Image", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            Elements.Add(changeImageButton);

            var cancelButton = new Button
            {
                Tag = "cancel",
                Padding = new Thickness(5),
                Content = new Label { Content = "Cancel", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };

            Elements.Add(cancelButton);

            var rightColumn_DetailsPrice = new Label { Tag = "rightcolumn detailsprice", FontSize = 16 };
            Elements.Add(rightColumn_DetailsPrice);

            rightColumn_detailsPanel_AdminButtons.Children.Add(rightColumn_DetailsPrice);

            rightColumn_detailsPanel_AdminButtons.Children.Add(cancelButton);
            rightColumn_detailsPanel_AdminButtons.Children.Add(editButton);
            rightColumn_detailsPanel_AdminButtons.Children.Add(removeButton);
            rightColumn_detailsPanel_AdminButtons.Children.Add(changeImageButton);
            rightColumn_detailsPanel_AdminButtons.Children.Add(saveChangesButton);

            Grid.SetColumn(rightColumn_detailsPanel_AdminButtons, 0);
            _detailsColumn_detailsGrid.Children.Add(rightColumn_detailsPanel_AdminButtons);
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            var textbox = ((TextBox)GetElement("rightcolumn detailsdescription"));

            SelectedProduct.Description = textbox.Text;
            textbox.IsReadOnly = true;
            textbox.Background = Brushes.Transparent;

            SelectedProduct.Name = _rightColumn_DetailsName.Text;
            _rightColumn_DetailsName.IsReadOnly = true;
            _rightColumn_DetailsName.Background = Brushes.Transparent;
            Store.SaveToText();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var textbox = ((TextBox)GetElement("rightcolumn detailsdescription"));
            textbox.IsReadOnly = false;
            textbox.Background = Brushes.White;
            _rightColumn_DetailsName.IsReadOnly = false;
            _rightColumn_DetailsName.Background = Brushes.White;
        }

        public void Update()
        {
            var product = SelectedProduct;
            var image = ((Image)GetElement("rightcolumn detailsimage"));
            image.Source = Helpers.CreateBitmapImageFromUriString(product.Uri);
            _rightColumn_DetailsName.Text = product.Name;
            ((Label)GetElement("rightcolumn detailsprice")).Content = $"{product.Price} kr";

            var textbox = ((TextBox)GetElement("rightcolumn detailsdescription"));

            textbox.MaxWidth = ((ScrollViewer)textbox.Parent).ActualWidth;
            textbox.Text = product.Description;
            _rightColumnContentRoot.Visibility = Visibility.Visible;
        }
    }
}
