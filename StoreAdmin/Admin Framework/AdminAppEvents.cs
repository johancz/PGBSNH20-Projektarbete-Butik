using StoreCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace StoreAdmin
{
    public class AdminAppEvents : HybridFramework
    {
        public static Product SelectedProduct = null;
        private bool ProductGridsIsSelectable;
        private bool NewProductMode = false;
        public void Init()
        {
            ActualWindow.Loaded += MainWindow_Loaded;
            ActualWindow.SizeChanged += MainWindow_SizeChanged;

            ProductGrids.ForEach(productGrid => productGrid.MouseUp += ProductGrid_MouseUp);
            ImageGrids.ForEach(imageGrid => imageGrid.MouseUp += ImageGrid_MouseUp);

            EditProductButton.Click += EditButton_Click;
                SaveEditButton.Click += SaveEditButton_Click;
                CancelEditButton.Click += CancelEditButton_Click;
            
            ChangeImageButton.Click += ChangeImageButton_Click;            
                SaveImageButton.Click += SaveImageButton_Click;
                CancelImageButton.Click += CancelImageButton_Click;
            NewProductButton.Click += NewProductButton_Click;
                NewProductSaveButton.Click += NewProductSaveButton_Click;
                NewProductAbortButton.Click += NewProductAbortButton_Click;
            RemoveButton.Click += RemoveButton_Click;

            DetailsPanelRootGrid.Visibility = Visibility.Hidden;
            LoadDefaultButtonPanel();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AdminButtons.ForEach(button => button.Width = DetailsButtonPanel.ActualWidth);
            DisableEditBoxes();
            AddAllProductGridsToProductBrowser();
            DetailsPanelDescription.Width = DetailsDescriptionScrollViewer.ActualWidth;
            SelectedProduct = null;
            DetailsPanelRootGrid.Visibility = Visibility.Hidden;
            ProductGridsIsSelectable = true;
            ActualWindow.KeyUp += MainWindow_KeyUp;
        }
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if((DetailsPanelImage.ActualWidth-100)>=100) DetailsPanelDescription.Width = DetailsPanelImage.ActualWidth-100;
            WindowTabControl.Width = ActualWindow.ActualWidth;
            WindowTabControl.Height = ActualWindow.ActualHeight;
            ProductGrids.ForEach(x => x.Width = (ActualWindow.ActualWidth - 50) / 7.0);
            ImageGrids.ForEach(x => x.Width = (ActualWindow.ActualWidth - 50) / 7.0);
        }
        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Application.Current.Shutdown();
        }

        private void ProductGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ProductGridsIsSelectable)
            {
                if ((DetailsPanelImage.ActualWidth - 100) >= 100) DetailsPanelDescription.Width = DetailsPanelImage.ActualWidth - 100;
                DetailsPanelRootGrid.Visibility = Visibility.Visible;
                SelectedProduct = (Product)((Grid)sender).Tag;
                UpdateDetailsPanel(SelectedProduct);
                var image = DetailsPanelImage; 
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
                HideAllButtons();
                ShowButtons(new List<Button> { CancelEditButton, SaveEditButton });
            }
            private void SaveEditButton_Click(object sender, RoutedEventArgs e)
            {
                string price = DetailsPanelPrice.Text;

                if (IsPriceInCorrectFormat(out decimal decPrice))
                {
                    var product = SelectedProduct;
                    product.Description = DetailsPanelDescription.Text;
                    product.Name = DetailsPanelName.Text;
                    product.Price = decPrice;
                    Store.SaveCurrentProductsInStoreToCSV();
                    
                    DisableEditBoxes();
                    UpdateDetailsPanel(product);
                    UpdateTextInProductBrowser(product);
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
                nameLabel.Content = $"{product.Name} {product.Price.ToString()}kr";
            }
            private bool IsPriceInCorrectFormat(out decimal result)
            {
            
                string price = DetailsPanelPrice.Text;
                price = price.Replace(',', '.');
                price = price.Trim();
                return decimal.TryParse(price, out result);
            }       

        private void CancelEditButton_Click(object sender, RoutedEventArgs e)
        {
            DisableEditBoxes();
            UpdateDetailsPanel(SelectedProduct);
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
            public void UpdateDetailsPanel(Product product)
            {
                DetailsPanelImage.Source = product.Tag.Source;
                DetailsPanelName.Text = product.Name; //gives the title from selected product
                DetailsPanelPrice.Text = product.Price.ToString();
                DetailsPanelDescription.MaxWidth = DetailsDescriptionScrollViewer.ActualWidth;
                DetailsPanelDescription.Text = product.Description;
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
                ImageGrids.ForEach(imageGrid => ProductAndImageWrapPanel.Children.Remove(imageGrid));
                ImageGrids.ForEach(imageGrid => ProductAndImageWrapPanel.Children.Add(imageGrid));
            }
            public void LoadChangeImageButtonPanel()
            {
                HideAllButtons();
                ShowButtons(new List<Button> { CancelImageButton, SaveImageButton });
            }
        private void NewProductButton_Click(object sender, RoutedEventArgs e)
        {
            NewProductMode = true;
            DetailsPanelTextVisiblilty(false);
            HideAllButtons();
            SwitchGridsToImageModeInBrowser();
            MessageBox.Show("Choose Image");

        }
                    private void NewProductAbortButton_Click(object sender, RoutedEventArgs e)
                    {                        
                        DetailsPanelTextVisiblilty(true);
                        DetailsPanelRootGrid.Visibility = Visibility.Hidden;
                        SwitchGridsToDefaultModeInBrowser();
                        HideAllButtons();
                        LoadDefaultButtonPanel();
                        NewProductMode = false;
                    }
                    private void NewProductSaveButton_Click(object sender, RoutedEventArgs e)
                    {
                        string productUri = DetailsPanelImage.Source.ToString().Split('/')[^1];
                        var newProduct = new Product("Title...", productUri, 0, "Enter your product description...");
                        Store.Products.Add(newProduct); 
                        Store.SaveCurrentProductsInStoreToCSV(); //Save to drive
                        
                        SelectedProduct = newProduct;
                        var productGrid = AppWindow.CreateProductGridWithContent(newProduct);
                        productGrid.Background = ProductGrids[0].Background;
                        productGrid.MouseUp += ProductGrid_MouseUp; //Create a new clickable Grid to browser
                        
                        SwitchGridsToDefaultModeInBrowser();
                        DisableProductGrids(); //changes left column to Edit mode

                        DetailsPanelTextVisiblilty(true);
                        UpdateDetailsPanel(SelectedProduct);
                        EnableEditBoxes();
                        NewProductMode = false;
                        HideAllButtons();
                        LoadEditButtonPanel();                       
                    }
                        private void DetailsPanelTextVisiblilty(bool Visible)
                        {
                            if (Visible)
                            {
                                DetailsPanelImage.Visibility = Visibility.Visible;
                                NameAndPricePanel.Visibility = Visibility.Visible;
                                DetailsPanelDescription.Visibility = Visibility.Visible;
                            }
                            else
                            {
                            DetailsPanelImage.Visibility = Visibility.Hidden;
                            NameAndPricePanel.Visibility = Visibility.Hidden;
                            DetailsPanelDescription.Visibility = Visibility.Hidden;
                            }
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
            if (NewProductMode)
            {
                DetailsPanelImage.Visibility = Visibility.Visible;
                ShowButtons(new List<Button> { NewProductSaveButton, NewProductAbortButton });
            }
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
                ProductGrids.ForEach(productGrid => ProductAndImageWrapPanel.Children.Remove(productGrid));
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
            HideAllButtons();
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
        private void HideAllButtons()
        {
            foreach (var button in AdminButtons)
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
    
     


