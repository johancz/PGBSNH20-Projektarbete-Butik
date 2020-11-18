using StoreCommon;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace StoreCommon
{
    public class AdminAppEvents : AdminFramework
    {
       
        public void Init()
        {
            HideMode();
            SaveImageButton.Click += SaveImageButton_Click;
            SaveChangesButton.Click += SaveChangesButton_Click;
            ChangeImageButton.Click += ChangeImageButton_Click;
            CancelImageButton.Click += CancelImageButton_Click;
            CancelButton.Click += CancelButton_Click;
            RemoveButton.Click += RemoveButton_Click;
            EditButton.Click += EditButton_Click;
            NewProductButton.Click += NewProductButton_Click;

            foreach (var productGrid in ProductGrids)
            {
                productGrid.MouseUp += ProductGrid_MouseUp;
            }
            foreach (var imageGrid in ImageGrids)
            {
                imageGrid.MouseUp += ImageGrid_MouseUp;
            }
        }

        private void NewProductButton_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new Product("Title...", "uri missing", 0, "Enter your text...");
            Store.Products.Add(newProduct);
            Store.SaveRuntimeProductsToCSV();
            AppWindow.CreateProductGridWithContent(newProduct);
            SelectedProduct = newProduct;
            UpdateDetailsPanel(newProduct);
            
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditMode();
        }

        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            DefaultMode();
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
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
                var product = SelectedProduct;
                var productGrid = ProductGrids.Find(x => x.Tag == product);
                var thumbNail = (Image)(productGrid.Children[0]);
                var nameLabel = (Label)(productGrid.Children[1]);
                var priceLabel = (Label)(productGrid.Children[2]);

                product.Description = DetailsPanelDescription.Text;
                product.Name = DetailsPanelName.Text;
                product.Price = decimal.Parse(DetailsPanelPrice.Text);

                nameLabel.Content = product.Name;
                priceLabel.Content = product.Price.ToString();
               
                Store.SaveRuntimeProductsToCSV();

                if (SelectedImage != null)
                {
                    thumbNail.Source = SelectedImage.Source;
                }
                DefaultMode();
                UpdateDetailsPanel(product);
            }
            else
            {
                MessageBox.Show("Try entering a digit as price!");
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
                var result = MessageBox.Show("Do you want to completly remove this product?", "", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Store.Products.Remove(SelectedProduct);
                    Store.SaveRuntimeProductsToCSV();
                    var productsGridItem = ProductGrids.Find(x => x.Tag == SelectedProduct);
                    BrowserProductsPanel.Children.Remove(productsGridItem);
                    SelectedProduct = null;
                    HideMode();
                }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DefaultMode();

            var product = SelectedProduct;
            DetailsPanelImage.Source = Helpers.CreateBitmapImageFromUriString(product.Uri);
            DetailsPanelName.Text = product.Name;
            DetailsPanelPrice.Text = product.Price.ToString();
            DetailsPanelDescription.MaxWidth = DetailsDescriptionScrollViewer.ActualWidth;
            DetailsPanelDescription.Text = product.Description;
        }

        private void CancelImageButton_Click(object sender, RoutedEventArgs e)
        {
            DefaultMode();
        }
        public void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeImageMode();
        }

        private void ImageGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var imageGrid = (Grid)sender;
            var selectedImage = (Image)(imageGrid.Children[0]);
            var selectedImageUri = selectedImage.Tag;
            SelectedImage = selectedImage;
            SelectedImage.Tag = selectedImageUri;
            string uri = (string)imageGrid.Tag;
            DetailsPanelImage.Source = selectedImage.Source;
        }

        private void ProductGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (AppMode != Mode.Edit)
            {
                var product = (Product)((Grid)sender).Tag;
                SelectedProduct = product;
                UpdateDetailsPanel(product);
                DefaultMode();
            }
        }
        public void HideMode()
        {
            AppMode = Mode.Hide;
            SwitchOpacity();
            DetailsPanelRootGrid.Visibility = Visibility.Hidden;
        }
        public void DefaultMode()
        {

            DetailsPanelRootGrid.Visibility = Visibility.Visible;
            DetailsPanelDescription.IsReadOnly = true;
            DetailsPanelDescription.Background = Brushes.Transparent;
            DetailsPanelPrice.IsReadOnly = true;
            DetailsPanelPrice.Background = Brushes.Transparent;
            DetailsPanelName.IsReadOnly = true;
            DetailsPanelName.Background = Brushes.Transparent;

            HideButtons(new List<Button> { CancelButton, SaveChangesButton, CancelImageButton });
            ViewButtons(new List<Button> { NewProductButton, ChangeImageButton, EditButton, RemoveButton });

            if (AppMode == Mode.ChangeImage)
            {
                foreach (var grid in ImageGrids)
                {
                    BrowserProductsPanel.Children.Remove(grid);
                }
                foreach (var grid in ProductGrids)
                {
                    BrowserProductsPanel.Children.Add(grid);
                }
            }
            AppMode = Mode.Default;
            SwitchOpacity();
        }
        public void UpdateDetailsPanel(Product product)
        {
            DetailsPanelImage.Source = Helpers.CreateBitmapImageFromUriString(product.Uri);
            DetailsPanelName.Text = product.Name;
            DetailsPanelPrice.Text = product.Price.ToString();
            DetailsPanelDescription.MaxWidth = ((ScrollViewer)DetailsPanelDescription.Parent).ActualWidth;
            DetailsPanelDescription.Text = product.Description;
        }
        public void ChangeImageMode()
        {
            AppMode = Mode.ChangeImage;
            SwitchOpacity();
            HideButtons(new List<Button> { NewProductButton, EditButton, RemoveButton, CancelButton, ChangeImageButton });
            ViewButtons(new List<Button> { SaveImageButton, CancelImageButton });

            //Switch items in browser
            foreach (var productGrid in ProductGrids)
            {
                BrowserProductsPanel.Children.Remove(productGrid);
            }
            foreach (var imageGrid in ImageGrids)
            {
                BrowserProductsPanel.Children.Add(imageGrid);
            }
            ChangeImageButton.Width = ChangeImageButton.ActualWidth;
        }

        public void EditMode()
        {
            AppMode = Mode.Edit;

            HideButtons(new List<Button> { NewProductButton, EditButton, RemoveButton, CancelImageButton, ChangeImageButton });
            ViewButtons(new List<Button> { CancelButton, SaveChangesButton });
            SwitchOpacity();
            DetailsPanelRootGrid.Visibility = Visibility.Visible;
            DetailsPanelDescription.IsReadOnly = false;
            DetailsPanelDescription.Background = Brushes.White;
            DetailsPanelPrice.IsReadOnly = false;
            DetailsPanelPrice.Background = Brushes.White;
            DetailsPanelName.IsReadOnly = false;
            DetailsPanelName.Background = Brushes.White;         

        }

        private void SwitchOpacity()
        {
            if (AppMode == Mode.Edit)
            {
                foreach (var grid in ProductGrids)
                {
                    grid.Opacity = 0.7;
                }
            }
            else
            {
                foreach (var grid in ProductGrids)
                {
                    grid.Opacity = 1;
                }
            }
        }
        private void ViewButtons(List<Button> buttonsToView)
        {
            foreach (var button in buttonsToView)
            {
                try
                {
                    DetailsButtonPanel.Children.Add(button);
                }
                catch (System.Exception)
                {
                }
            }
        }
        private void HideButtons(List<Button> buttonsToHide)
        {
            foreach (var button in buttonsToHide)
            {
                try
                {
                    DetailsButtonPanel.Children.Remove(button);
                }
                catch (System.Exception)
                {
                }
            }
        }
    }
}
