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
        bool ProductGridsSelectable = true;
        public void Init()
        {
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
            DetailsPanelRootGrid.Visibility = Visibility.Hidden;
            LoadDefaultButtonPanel();
        }
        //Image Selection Events
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
            if (ProductGridsSelectable)
            {
                DetailsPanelRootGrid.Visibility = Visibility.Visible;
                var product = (Product)((Grid)sender).Tag;
                SelectedProduct = product;
                UpdateDetailsPanel(product);
            }
        }

        //Button Click Events
        private void NewProductButton_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new Product("Title...", "uri missing", 0, "Enter your text...");
            Store.Products.Add(newProduct);
            Store.SaveRuntimeProductsToCSV();
            AppWindow.CreateProductGridWithContent(newProduct);
            SelectedProduct = newProduct;
            UpdateDetailsPanel(newProduct);
            EnterEditmode();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EnterEditmode();
        }

        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            
            LeaveEditMode();
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

                UpdateDetailsPanel(product);
            }
            else
            {
                MessageBox.Show("Try entering a digit as price!");
            }
            LeaveEditMode();
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
                }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            LeaveEditMode();

            var product = SelectedProduct;
            DetailsPanelImage.Source = Helpers.CreateBitmapImageFromUriString(product.Uri);
            DetailsPanelName.Text = product.Name;
            DetailsPanelPrice.Text = product.Price.ToString();
            DetailsPanelDescription.MaxWidth = DetailsDescriptionScrollViewer.ActualWidth;
            DetailsPanelDescription.Text = product.Description;
        }

        private void CancelImageButton_Click(object sender, RoutedEventArgs e)
        {
            LeaveImageMode();
        }
        public void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            EnterChangeImageMode();
        }
        //Switch mode methods
        public void EnterEditmode()
        {
            ProductGridsSelectable = false;
            DetailsPanelContentEditable(true);

            foreach (var grid in ProductGrids)
            {
                grid.Opacity = 0.7;
            }
            LoadEditButtonPanel();            
        }
        public void LeaveEditMode()
        {
            ProductGridsSelectable = true;
            DetailsPanelContentEditable(false);

            foreach (var grid in ProductGrids)
            {
                grid.Opacity = 1;
            }
            LoadDefaultButtonPanel();
        }
        public void EnterChangeImageMode()
        {
            foreach (var productGrid in ProductGrids)
            {
                BrowserProductsPanel.Children.Remove(productGrid);
            }
            foreach (var imageGrid in ImageGrids)
            {
                BrowserProductsPanel.Children.Add(imageGrid);
            }
            LoadChangeImageButtonPanel();
        }
        public void LeaveImageMode()
        {
            foreach (var productGrid in ProductGrids)
            {
                BrowserProductsPanel.Children.Remove(productGrid);
            }
            foreach (var imageGrid in ImageGrids)
            {
                BrowserProductsPanel.Children.Add(imageGrid);
            }
            LoadDefaultButtonPanel();
        }
        public void UpdateDetailsPanel(Product product)
        {
            DetailsPanelImage.Source = Helpers.CreateBitmapImageFromUriString(product.Uri);
            DetailsPanelName.Text = product.Name;
            DetailsPanelPrice.Text = product.Price.ToString();
            DetailsPanelDescription.MaxWidth = ((ScrollViewer)DetailsPanelDescription.Parent).ActualWidth;
            DetailsPanelDescription.Text = product.Description;
        }
        //simplyfications
        public void LoadDefaultButtonPanel()
        {
            HideButtons(new List<Button> { CancelButton, SaveChangesButton, CancelImageButton });
            ShowButtons(new List<Button> { NewProductButton, ChangeImageButton, EditButton, RemoveButton });
        }
        public void LoadEditButtonPanel()
        {
            HideButtons(new List<Button> { NewProductButton, EditButton, RemoveButton, CancelImageButton, ChangeImageButton });
            ShowButtons(new List<Button> { CancelButton, SaveChangesButton });
        }
        public void LoadChangeImageButtonPanel()
        {
            HideButtons(new List<Button> { NewProductButton, EditButton, RemoveButton, CancelImageButton, ChangeImageButton });
            ShowButtons(new List<Button> { CancelButton, SaveChangesButton });
        }
        public void DetailsPanelContentEditable(bool isContentEditable)
        {
            if (isContentEditable)
            {
                DetailsPanelRootGrid.Visibility = Visibility.Visible;
                DetailsPanelDescription.IsReadOnly = false;
                DetailsPanelDescription.Background = Brushes.White;
                DetailsPanelPrice.IsReadOnly = false;
                DetailsPanelPrice.Background = Brushes.White;
                DetailsPanelName.IsReadOnly = false;
                DetailsPanelName.Background = Brushes.White;
            }
            else
            {
                DetailsPanelRootGrid.Visibility = Visibility.Visible;
                DetailsPanelDescription.IsReadOnly = true;
                DetailsPanelDescription.Background = Brushes.Transparent;
                DetailsPanelPrice.IsReadOnly = true;
                DetailsPanelPrice.Background = Brushes.Transparent;
                DetailsPanelName.IsReadOnly = true;
                DetailsPanelName.Background = Brushes.Transparent;
            }
        }
        private void ShowButtons(List<Button> buttonsToView)
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
