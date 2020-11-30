using StoreCommon;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace StoreAdmin
{
    // This Class contains All admin-events: MainWindow-loaded, size changed, Buttonclicks, imageclicks. Some are similar to User Mode but to get better control and a clear view over the events they are created in a class of its own.
    // An exception to the above are events for the content of the "Manage Discount Codes"-tab, these can be found in ManageDiscountCodesView.cs
    public class AdminModeEvents : SharedElementTree
    {
        public static Product SelectedProduct = null;
        private bool ProductGridItemsAreSelectable;
        private bool NewProductMode = false;
        public void Init()
        {
            MainWindow.Loaded += MainWindow_Loaded;
            MainWindow.SizeChanged += MainWindow_SizeChanged;

            ProductGridItems.ForEach(productGridItem => productGridItem.MouseUp += ProductGridItem_MouseUp);
            ImageGridItems.ForEach(imageGridItem => imageGridItem.MouseUp += ImageGridItem_MouseUp);

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
            AddAllProductGridItemsToProductBrowser();
            SelectedProduct = null;
            DetailsPanelRootGrid.Visibility = Visibility.Hidden;
            ProductGridItemsAreSelectable = true;
            MainWindow.KeyUp += MainWindow_KeyUp;
        }
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ProductGridItems.ForEach(x => x.Width = (MainWindow.ActualWidth - 50) / 7.0);
            ImageGridItems.ForEach(x => x.Width = (MainWindow.ActualWidth - 50) / 7.0);
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Application.Current.Shutdown();
        }

        private void ProductGridItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ProductGridItemsAreSelectable)
            {
                DetailsPanelRootGrid.Visibility = Visibility.Visible;
                SelectedProduct = (Product)((Grid)sender).Tag;
                UpdateDetailsPanel(SelectedProduct);
                var image = DetailsPanelImage; 
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DisableProductGridItems();
            EnableEditBoxes();
            LoadEditButtonPanel();
        }
            private void DisableProductGridItems()
            {
                ProductGridItemsAreSelectable = false;
                ProductGridItems.ForEach(grid => grid.Opacity = 0.5);
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
                if (IsPriceInCorrectFormat(out decimal decPrice) && !IsDelimiterInDescription())
                {
                    var product = SelectedProduct;
                    product.Description = DetailsPanelDescription.Text;
                    product.Name = DetailsPanelName.Text;
                    product.Price = Math.Round(decPrice, 2);
                    Store.SaveRuntimeAdminProductsToCSV();                    
                    DisableEditBoxes();
                    UpdateDetailsPanel(product);
                    UpdateTextInProductBrowser(product);
                    EnableProductGridItems();
                    LoadDefaultButtonPanel();
                }
            }
            private void UpdateTextInProductBrowser(Product product)
            {
                var productGridItem = ProductGridItems.Find(x => x.Tag == product);
                var nameLabel = (Label)(productGridItem.Children[1]);
                nameLabel.Content = $"{product.Name} {product.Price} kr";
            }
            private bool IsPriceInCorrectFormat(out decimal result)
            {
            
                string price = DetailsPanelPrice.Text;
                price = price.Replace(',', '.');
                price = price.Trim();
                if (!decimal.TryParse(price, out result))
                {
                    MessageBox.Show("Try entering a digit as price!");
                }
                return decimal.TryParse(price, out result);
            }
            private bool IsDelimiterInDescription()
            {
                if (DetailsPanelDescription.Text.Contains('#'))
                {
                    MessageBox.Show("The description can not contain \"#\"");
                }
                return DetailsPanelDescription.Text.Contains('#');
        }

        private void CancelEditButton_Click(object sender, RoutedEventArgs e)
        {
            DisableEditBoxes();
            UpdateDetailsPanel(SelectedProduct);
            EnableProductGridItems();
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
            private void EnableProductGridItems()
            {
                ProductGridItemsAreSelectable = true;
                ProductGridItems.ForEach(grid => grid.Opacity = 1);
                BrowserRootScrollViewer.IsEnabled = true;
            }

        private void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchGridsToImageModeInBrowser();
            LoadChangeImageButtonPanel();
        }
            public void SwitchGridsToImageModeInBrowser()
            {
                ProductGridItems.ForEach(productGrid => ProductAndImageWrapPanel.Children.Remove(productGrid));
                ImageGridItems.ForEach(imageGrid => ProductAndImageWrapPanel.Children.Remove(imageGrid));
                ImageGridItems.ForEach(imageGrid => ProductAndImageWrapPanel.Children.Add(imageGrid));
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
                        Store.SaveRuntimeAdminProductsToCSV(); //Save to drive
                        
                        SelectedProduct = newProduct;
                        var productGridItem = AppWindow.CreateProductGridItem(newProduct);
                        productGridItem.Background = ProductGridItems[0].Background;
                        productGridItem.Width = ProductGridItems[0].Width;
                        productGridItem.MouseUp += ProductGridItem_MouseUp; //Create a new clickable Grid to browser
                        
                        SwitchGridsToDefaultModeInBrowser();
                        DisableProductGridItems(); //changes left column to Edit mode

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
                Store.SaveRuntimeAdminProductsToCSV();
                RemoveProductGridItemFromBrowser();                    
                SelectedProduct = null;
                DetailsPanelRootGrid.Visibility = Visibility.Hidden;
            }
        }
            private bool IsRemoveTrue()
            {
                return MessageBoxResult.Yes == MessageBox.Show("Do you want to completly remove this product?", "", MessageBoxButton.YesNo);
            }
            private void RemoveProductGridItemFromBrowser()
            {
                var productsGridItem = ProductGridItems.Find(x => x.Tag == SelectedProduct);
                ProductAndImageWrapPanel.Children.Remove(productsGridItem);
            }
        
        private void ImageGridItem_MouseUp(object sender, MouseButtonEventArgs e)
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
           Store.SaveRuntimeAdminProductsToCSV();
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
                ImageGridItems.ForEach(imageGrid => ProductAndImageWrapPanel.Children.Remove(imageGrid));
                ProductGridItems.ForEach(productGrid => ProductAndImageWrapPanel.Children.Remove(productGrid));
                ProductGridItems.ForEach(productGrid => ProductAndImageWrapPanel.Children.Add(productGrid));
            }
        private void AddAllProductGridItemsToProductBrowser()
        {
            foreach (var productGridItem in ProductGridItems)
            {
                ProductAndImageWrapPanel.Children.Add(productGridItem);
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
