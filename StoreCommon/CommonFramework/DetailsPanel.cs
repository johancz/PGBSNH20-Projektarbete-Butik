using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreCommon
{
    public class DetailsPanel : CommonFramework
    {
        public string Tag;
        private Grid Parent;
        public Grid _detailsColumn_detailsGrid;
        private TextBox _rightColumn_DetailsDescription;
        
        public DetailsPanel(Grid parent, Brush brush, string tag)
        {
            Tag = tag;
            Parent = parent;
            detailsPanel = this;

            var detailsPanelParentGrid = new Grid { ShowGridLines = true, Background = brush };

            detailsPanelParentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            detailsPanelParentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            // Create and add a Product.Image to the right column's root (StackPanel)
            
            var detailsPanelImage = new Image { Tag = "rightcolumn detailsimage" };
            DetailsPanelParentGrid.Children.Add(DetailsPanelImage);

            var detailsColumn_detailsGrid = new Grid { ShowGridLines = true };
            _detailsColumn_detailsGrid = detailsColumn_detailsGrid;

            detailsColumn_detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            detailsColumn_detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Grid.SetRow(detailsColumn_detailsGrid, 1);
            detailsPanelParentGrid.Children.Add(detailsColumn_detailsGrid);

            var detailsColumn_namePriceDescription = new Grid { ShowGridLines = true };
            detailsColumn_namePriceDescription.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            detailsColumn_namePriceDescription.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            // Create the product "Name" and "Price" labels and a StackPanel-parent for them. Add the parent to the detailsPanel.

            NameAndPriceParentPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5),
            };
            DetailsPanelParentGrid.Visibility = Visibility.Hidden;

            DetailsPanelName = new TextBox
            {
                Tag = "rightcolumn detailsname",
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Background = Brushes.Transparent,
                IsReadOnly = true
            };
            NameAndPriceParentPanel.Children.Add(DetailsPanelName);

            var rightColumn_DetailsPrice = new TextBox
            {
                Tag = "rightcolumn detailsprice",
                FontSize = 16,
                Background = Brushes.Transparent,
                IsReadOnly = true
            };

            NameAndPriceParentPanel.Children.Add(rightColumn_DetailsPrice);

            var rightColumn_DetailsCurrency = new TextBox
            {
                Tag = "rightcolumn detailscurrency",
                FontSize = 16,
                Background = Brushes.Transparent,
                Text = "kr",
                IsReadOnly = true
            };

            NameAndPriceParentPanel.Children.Add(rightColumn_DetailsCurrency);


            Grid.SetRow(NameAndPriceParentPanel, 0);
            detailsColumn_namePriceDescription.Children.Add(NameAndPriceParentPanel);


            _rightColumn_DetailsDescription = new TextBox
            {
                Tag = "rightcolumn detailsdescription",
                TextWrapping = TextWrapping.Wrap,
                Background = Brushes.Transparent,
                IsReadOnly = true,
                AcceptsReturn = true
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

            // Add the right-column to the "root"-Grid.
            Grid.SetColumn(DetailsPanelParentGrid, 1);
            Parent.Children.Add(DetailsPanelParentGrid);

            ButtonParentPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(ButtonParentPanel, 0);
            detailsColumn_detailsGrid.Children.Add(ButtonParentPanel);

        }

        public void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {

        }
        public void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //Disables editable fields
            DetailsPanelDescription.IsReadOnly = true;
            DetailsPanelDescription.Background = Brushes.Transparent;

            DetailsPanelPrice.IsReadOnly = true;
            DetailsPanelPrice.Background = Brushes.Transparent;
            ;
            CommonFramework.DetailsPanelName.IsReadOnly = true;
            CommonFramework.DetailsPanelName.Background = Brushes.Transparent;
           
            UpdatePanel();
            DefaultModeButtons();

            EditProductModeEnabled = false;
            ChangeImageModeEnabled = false;
            foreach (var item in ProductBrowserItems)
            {
                item.SwitchOpacityMode();
            }
            SelectedImage = null;           
        }

        public void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ChangeImageModeEnabled && !EditProductModeEnabled)
            {
                var result = MessageBox.Show("Do you want to completly remove this product?", "", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Store.Products.Remove(SelectedProduct);
                    Store.SaveRuntimeProductsToCSV();
                    var productsItem = ProductBrowserItems.Find(x => x._product == SelectedProduct);
                    var parent = productsItem.Parent;
                    parent.Children.Remove(productsItem.ItemGrid);
                    ProductBrowserItems.Remove(productsItem);
                    DetailsPanelParentGrid.Visibility = Visibility.Hidden;
                    SelectedProduct = null;
                }
            }
        }

        public void NewProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ChangeImageModeEnabled && !EditProductModeEnabled)
            {
                var newProduct = new Product("Title...", "uri missing", 0, "Enter your text...");
                var newBrowserItem = new BrowserItem(ProductParentPanel);
                newBrowserItem.LoadProductBrowserItem(newProduct);
                SelectedProduct = newProduct;
                Store.Products.Add(newProduct);
                SelectedImage = null;
                detailsPanel.UpdatePanel();
            }
        }

        public void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            ImageModeButtons();
            ChangeImageModeEnabled = true;
            EditProductModeEnabled = true;
            foreach (var productItem in ProductBrowserItems)
            {
                ProductParentPanel.Children.Remove(productItem.ItemGrid);
                productItem.SwitchOpacityMode();

            }
            foreach (var imageItem in ImageBrowserItems)
            {
                ProductParentPanel.Children.Add(imageItem.ItemGrid);
            }
            ChangeImageButton.Width = ChangeImageButton.ActualWidth;
            ChangeImageButton.Content = new Label { Content = "Select Image", HorizontalAlignment = HorizontalAlignment.Left };
        }

        public void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string price = DetailsPanelPrice.Text;
            decimal decPrice;
            if (price.Contains(','))
            {
                price = DetailsPanelPrice.Text.Replace(',', '.');
                DetailsPanelPrice.Text = price;
            }
            if (decimal.TryParse(price, out decPrice))
            {
                //Disables editable fields
                DetailsPanelDescription.IsReadOnly = true;
                DetailsPanelDescription.Background = Brushes.Transparent;

                DetailsPanelPrice.IsReadOnly = true;
                DetailsPanelPrice.Background = Brushes.Transparent;

                CommonFramework.DetailsPanelName.IsReadOnly = true;
                CommonFramework.DetailsPanelName.Background = Brushes.Transparent;

                //updates name, description, image soon prize
                SelectedProduct.Description = DetailsPanelDescription.Text;
                SelectedProduct.Name = CommonFramework.DetailsPanelName.Text;
                SelectedProduct.Price = decimal.Parse(DetailsPanelPrice.Text);

                if (SelectedImage != null)
                {
                    SelectedProduct.Uri = SelectedImage.Source.ToString().Split('/')[^1];
                }
                Store.SaveRuntimeProductsToCSV();
            
                var browserItem = ProductBrowserItems.Find(x => x.ItemGrid.Tag == SelectedProduct); //Gets senders browserItem

                browserItem.RefreshProductContent();
                EditProductModeEnabled = false;
                ChangeImageModeEnabled = false;
                foreach (var item in ProductBrowserItems)
                {
                    item.SwitchOpacityMode();
                }
                DefaultModeButtons();
            }
            else
            {
                MessageBox.Show("Try entering a digit as price!");
            }
        }

        public void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditModeButtons();

            DetailsPanelDescription.IsReadOnly = false;
            DetailsPanelDescription.Background = Brushes.White;

            DetailsPanelPrice.IsReadOnly = false;
            DetailsPanelPrice.Background = Brushes.White;

            CommonFramework.DetailsPanelName.IsReadOnly = false;
            CommonFramework.DetailsPanelName.Background = Brushes.White;

            foreach (var item in ProductBrowserItems)
            {
                item.SwitchOpacityMode();
            }
        }
  
        public void UpdatePanel()
        {
            var product = SelectedProduct;

            DetailsPanelImage.Source = Helpers.CreateBitmapImageFromUriString(product.Uri);
            CommonFramework.DetailsPanelName.Text = product.Name;
            DetailsPanelPrice.Text = product.Price.ToString();

            DetailsPanelDescription.MaxWidth = ((ScrollViewer)DetailsPanelDescription.Parent).ActualWidth;
            DetailsPanelDescription.Text = product.Description;
            DetailsPanelParentGrid.Visibility = Visibility.Visible;
        }
    }
}
