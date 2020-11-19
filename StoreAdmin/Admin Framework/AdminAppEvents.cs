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
        public static Product SelectedProduct = null;
        private bool ProductGridsIsSelectable;
        public void Init()
        {
            MainWindow.Loaded += MainWindow_Loaded;

            ProductGrids.ForEach(productGrid => productGrid.MouseUp += ProductGrid_MouseUp);
            ImageGrids.ForEach(imageGrid => imageGrid.MouseUp += ImageGrid_MouseUp);

            NewProductButton.Click += NewProductButton_Click;
            RemoveButton.Click += RemoveButton_Click;
            EditProductButton.Click += EditButton_Click;
            ChangeImageButton.Click += ChangeImageButton_Click;            

            SaveImageButton.Click += SaveImageButton_Click;
            CancelImageButton.Click += CancelImageButton_Click;

            SaveChangesButton.Click += SaveChangesButton_Click;
            CancelButton.Click += CancelButton_Click;

            DetailsPanelRootGrid.Visibility = Visibility.Hidden;
            LoadDefaultButtonPanel();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Main Window Loaded"); //test
            AdminButtons.ForEach(button => button.Width = DetailsButtonPanel.ActualWidth);
            DisableEditBoxes();
            AddAllProductGridsToProductBrowser();

            SelectedProduct = null;
            DetailsPanelRootGrid.Visibility = Visibility.Hidden;
            ProductGridsIsSelectable = true;
            MainWindow.KeyUp += MainWindow_KeyUp;
        }
        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        private void ProductGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ProductGridsIsSelectable)
            {
                DetailsPanelRootGrid.Visibility = Visibility.Visible;
                SelectedProduct = (Product)((Grid)sender).Tag;
                UpdateDetailsPanel(SelectedProduct);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DisableProductGrids();
            EnableEditBoxes();
            LoadEditButtonPanel();
        }
            private void DisableProductGrids()
            {
                ProductGridsIsSelectable = false;
                ProductGrids.ForEach(grid => grid.Opacity = 0.5);
                BrowserRootScrollViewer.IsEnabled = false;
            }
            private void EnableEditBoxes()
            {
                EditDetailsTextBoxes.ForEach(box =>
                {
                    box.Background = Brushes.GhostWhite;
                    box.BorderBrush = Brushes.GhostWhite;
                    box.IsReadOnly = false;                    
                });
               DetailsPanelDescription.Focus();
            }
            public void LoadEditButtonPanel()
        {
            HideButtons(new List<Button> { NewProductButton, EditProductButton, RemoveButton, CancelImageButton, ChangeImageButton });
            ShowButtons(new List<Button> { CancelButton, SaveChangesButton });
        }
        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string price = DetailsPanelPrice.Text;
            decimal decPrice;
            
            if (IsPriceInCorrectFormat(out decPrice))
            {
                var product = SelectedProduct;
                product.Description = DetailsPanelDescription.Text;
                product.Name = DetailsPanelName.Text;
                product.Price = decPrice;
                Store.SaveCurrentProductsInStoreToCSV();
                UpdateTextInProductBrowser(product);
                UpdateDetailsPanel(product);
                EnableProductGrids();
                LoadDefaultButtonPanel();
            }
            else
            {
                MessageBox.Show("Try entering a digit as price!");
            }
        }
        private void UpdateTextInProductBrowser(Product product)
        {
            var productGrid = ProductGrids.Find(x => x.Tag == product);
            var nameLabel = (Label)(productGrid.Children[1]);
            var priceLabel = (Label)(productGrid.Children[2]);
            nameLabel.Content = product.Name;
            priceLabel.Content = product.Price.ToString();
        }
        private bool IsPriceInCorrectFormat(out decimal result)
        {
            
            string price = DetailsPanelPrice.Text;
            price = price.Replace(',', '.');
            price = price.Trim();
            return decimal.TryParse(price, out result);
        }
       

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DisableEditBoxes();

            var product = SelectedProduct;
            DetailsPanelImage.Source = Helpers.CreateBitmapImageFromUriString(product.Uri);
            DetailsPanelName.Text = product.Name;
            DetailsPanelPrice.Text = product.Price.ToString();
            DetailsPanelDescription.MaxWidth = DetailsDescriptionScrollViewer.ActualWidth;
            DetailsPanelDescription.Text = product.Description;
            EnableProductGrids();
            LoadDefaultButtonPanel();
        }
            private void DisableEditBoxes()
                {
                    EditDetailsTextBoxes.ForEach(box =>
                    { 
                        box.Background = Brushes.Transparent;
                        box.BorderBrush = Brushes.Transparent;
                        box.IsReadOnly = true;
                    });
                }
            private void EnableProductGrids()
            {
                ProductGridsIsSelectable = true;
                ProductGrids.ForEach(grid => grid.Opacity = 1);
                BrowserRootScrollViewer.IsEnabled = true;
            }

        private void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchGridsToImageModeInBrowser();
            LoadChangeImageButtonPanel();
        }
            public void SwitchGridsToImageModeInBrowser()
            {
                ProductGrids.ForEach(productGrid => ProductAndImageWrapPanel.Children.Remove(productGrid));
                ImageGrids.ForEach(imageGrid => ProductAndImageWrapPanel.Children.Add(imageGrid));
            }
            public void LoadChangeImageButtonPanel()
            {
                HideButtons(new List<Button> { NewProductButton, EditProductButton, RemoveButton, CancelImageButton, ChangeImageButton });
                ShowButtons(new List<Button> { CancelButton, SaveChangesButton });
            }
        private void NewProductButton_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new Product("Title...", "uri missing", 0, "Enter your text..."); 
            var newGrid = AppWindow.CreateProductGridWithContent(newProduct);
            ProductAndImageWrapPanel.Children.Add(newGrid);
            Store.Products.Add(newProduct);
            Store.SaveCurrentProductsInStoreToCSV();
            UpdateDetailsPanel(newProduct);
            DisableProductGrids();
            EnableEditBoxes();
            LoadEditButtonPanel();
            SelectedProduct = newProduct;
        }
            public void UpdateDetailsPanel(Product product)
            {
                DetailsPanelImage.Source = product.Tag.Source;
                DetailsPanelName.Text = product.Name; //gives the title from selected product
                DetailsPanelPrice.Text = product.Price.ToString();
                DetailsPanelDescription.MaxWidth = ((ScrollViewer)DetailsPanelDescription.Parent).ActualWidth;
                DetailsPanelDescription.Text = product.Description;
            }
        
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {               
            if (IsRemoveTrue())
            {
                Store.Products.Remove(SelectedProduct); //Remove from base class static list
                Store.SaveCurrentProductsInStoreToCSV();
                RemoveProductGridFromBrowser();                    
                SelectedProduct = null;
                DetailsPanelRootGrid.Visibility = Visibility.Hidden;
            }
            //else stay in default mode
        }
            private bool IsRemoveTrue()
            {
                return MessageBoxResult.Yes == MessageBox.Show("Do you want to completly remove this product?", "", MessageBoxButton.YesNo);
            }
            private void RemoveProductGridFromBrowser()
            {
                var productsGridItem = ProductGrids.Find(x => x.Tag == SelectedProduct);
                ProductAndImageWrapPanel.Children.Remove(productsGridItem);
            }
        
        private void ImageGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DetailsPanelImage.Source = (ImageSource)((Grid)sender).Tag; //switch right columns imagecontent
        }
        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
           SelectedProduct.Uri = DetailsPanelImage.Source.ToString().Split('/')[^1];
           Store.SaveCurrentProductsInStoreToCSV();
           SelectedProduct.Tag.Source = DetailsPanelImage.Source;
           SwitchGridsToDefaultModeInBrowser();
           LoadDefaultButtonPanel();
        }

        private void CancelImageButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchGridsToDefaultModeInBrowser();
            LoadDefaultButtonPanel();
            UpdateDetailsPanel(SelectedProduct);
        }
            public void SwitchGridsToDefaultModeInBrowser()
            {
                ImageGrids.ForEach(imageGrid => ProductAndImageWrapPanel.Children.Remove(imageGrid));
                ProductGrids.ForEach(productGrid => ProductAndImageWrapPanel.Children.Add(productGrid));
            }
        private void AddAllProductGridsToProductBrowser()
        {
            foreach (var productGrid in ProductGrids)
            {
                ProductAndImageWrapPanel.Children.Add(productGrid);
            }
        }

        public void LoadDefaultButtonPanel()
        {
            HideButtons(new List<Button> { CancelButton, SaveChangesButton, CancelImageButton });
            ShowButtons(new List<Button> { NewProductButton, ChangeImageButton, EditProductButton, RemoveButton });
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
    
     


