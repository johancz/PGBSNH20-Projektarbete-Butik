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
        private bool ProductGridsIsSelectable;
        public void Init()
        {
            //Default Mode Events
            MainWindow.Loaded += MainWindow_Loaded;
            NewProductButton.Click += NewProductButton_Click;
            RemoveButton.Click += RemoveButton_Click;
            EditProductButton.Click += EditButton_Click;
            ChangeImageButton.Click += ChangeImageButton_Click;
                foreach (var productGrid in ProductGrids)
                {
                    productGrid.MouseUp += ProductGrid_MouseUp;
                }
            //Image Mode Events
            SaveImageButton.Click += SaveImageButton_Click;
            CancelImageButton.Click += CancelImageButton_Click;
                foreach (var imageGrid in ImageGrids)
                {
                    imageGrid.MouseUp += ImageGrid_MouseUp;
                }

            //Edit Mode Events
            SaveChangesButton.Click += SaveChangesButton_Click;
            CancelButton.Click += CancelButton_Click;

            DetailsPanelRootGrid.Visibility = Visibility.Hidden;
            LoadDefaultButtonPanel();
        }


        //Image Selection Events


        //Default Mode Events
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AddAllProductGridsToProductAndImageWrapPanel();
            SelectedImage = null;
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
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
                var result = MessageBox.Show("Do you want to completly remove this product?", "", MessageBoxButton.YesNo);
               
                if (result == MessageBoxResult.Yes)
                {
                    RemoveProductFromTextFileWithSelectedProductAsInput();
                    RemoveProductGridFromProductAndImageWrapPanel();                    
                    SelectedProduct = null;
                    DetailsPanelRootGrid.Visibility = Visibility.Hidden;
                }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EnterEditmode();
        }
        private void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            EnterChangeImageMode();
        }
        private void ProductGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ProductGridsIsSelectable)
            {
                DetailsPanelRootGrid.Visibility = Visibility.Visible;
                var product = (Product)((Grid)sender).Tag;
                SelectedProduct = product;
                UpdateDetailsPanel(product);
            }
        }

        //Image Mode Events
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
        private void CancelImageButton_Click(object sender, RoutedEventArgs e)
        {
            LeaveImageMode();
        }
        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            LeaveImageMode();
        }
        //Edit Mode Events
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
                LeaveEditMode();
            }
            else
            {
                MessageBox.Show("Try entering a digit as price!");
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



        //Switch mode methods
        public void EnterEditmode()
        {
            ProductGridsIsSelectable = false;
            //DetailsPanelContentEditable(true);

            foreach (var grid in ProductGrids)
            {
                grid.Opacity = 0.7;
            }
            LoadEditButtonPanel();            
        }
        //Editable
        //Background color
        //opacity
        //not clickable grids
        public void LeaveEditMode()
        {
            ProductGridsIsSelectable = true;
            //DetailsPanelContentEditable(false);

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
                try
                {
                    ProductAndImageWrapPanel.Children.Remove(productGrid);
                }
                catch (System.Exception)
                {
                }
            }
            foreach (var imageGrid in ImageGrids)
            {
                try
                {
                    ProductAndImageWrapPanel.Children.Add(imageGrid);
                }
                catch (System.Exception)
                {
                }
            }
            LoadChangeImageButtonPanel();
        }
        //switch grids
        //ChangeButtons
        public void LeaveImageMode()
        {
            foreach (var imageGrid in ImageGrids)
            {
                try
                {
                    ProductAndImageWrapPanel.Children.Remove(imageGrid);
                }
                catch (System.Exception)
                {
                }
            }
            foreach (var productGrid in ProductGrids)
            {
                try
                {
                    ProductAndImageWrapPanel.Children.Add(productGrid);
                }
                catch (System.Exception)
                {
                }
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

        //public void EnterDefaultModeFromSaveImage()
        //{
        //    //Save to file
        //    //Update ProductsAndImagePanel
        //    //Update DetailsPanel with new image
        //    //Set Default Mode
        //    //switch grid content
        //    //Set Selected Image to null
        //}
        //public void EnterDefaultModeFromCancelImage()
        //{
        //    //Switch to old image
        //    //switch grids
        //    //activate grids
        //    //Change to default buttons
        //    //Set Selected Image to null
        //}

        //public void EnterDefaultModeFromSaveEdit()
        //{
        //    //Save changes to disk
        //    //Change content in grid
        //    //activate grids
        //    //Keep content in DetailsPanel
        //    //Set to default mode
        //}
        //public void EnterDefaultModeFromCancelEdit()
        //{
        //    //reset to ReadOnly, Transparent
        //    //reset TextBoxes with Selected Product
        //    //activate grids, opacity 1.0
        //}

        public void EnterDefaultFromStartUp()
        {
            //DetailsGrid Hidden
            //grids active
            //image grids not added
        }
        private void RemoveProductFromTextFileWithSelectedProductAsInput()
        {
            Store.Products.Remove(SelectedProduct);
            Store.SaveRuntimeProductsToCSV();
        }
        private void AddAllProductGridsToProductAndImageWrapPanel()
        {
            foreach (var productGrid in ProductGrids)
            {
                ProductAndImageWrapPanel.Children.Add(productGrid);
            }
        }
        private void RemoveProductGridFromProductAndImageWrapPanel()
        {
            var productsGridItem = ProductGrids.Find(x => x.Tag == SelectedProduct);
            ProductAndImageWrapPanel.Children.Remove(productsGridItem);
        }

        public void ReloadDetailsPanelWithSelectedProductAsOnlyInput()
        {
            var product = SelectedProduct;
            DetailsPanelName.Text = product.Name;
            DetailsPanelPrice.Text = product.Price.ToString();
            DetailsPanelDescription.MaxWidth = ((ScrollViewer)DetailsPanelDescription.Parent).ActualWidth;
            DetailsPanelDescription.Text = product.Description;
        }
        //simplyfications
        public void LoadDefaultButtonPanel()
        {
            HideButtons(new List<Button> { CancelButton, SaveChangesButton, CancelImageButton });
            ShowButtons(new List<Button> { NewProductButton, ChangeImageButton, EditProductButton, RemoveButton });
        }
        public void LoadEditButtonPanel()
        {
            HideButtons(new List<Button> { NewProductButton, EditProductButton, RemoveButton, CancelImageButton, ChangeImageButton });
            ShowButtons(new List<Button> { CancelButton, SaveChangesButton });
        }
        public void LoadChangeImageButtonPanel()
        {
            HideButtons(new List<Button> { NewProductButton, EditProductButton, RemoveButton, CancelImageButton, ChangeImageButton });
            ShowButtons(new List<Button> { CancelButton, SaveChangesButton });
        }
        public void EnableEditModeInDetailsPanelTextBoxes()
        {
            DetailsPanelRootGrid.Visibility = Visibility.Visible;
            DetailsPanelDescription.IsReadOnly = false;
            DetailsPanelDescription.Background = Brushes.White;
            DetailsPanelPrice.IsReadOnly = false;
            DetailsPanelPrice.Background = Brushes.White;
            DetailsPanelName.IsReadOnly = false;
            DetailsPanelName.Background = Brushes.White;
        }
        public void DisableEditModeInDetailsPanelTextBoxes()
        {
            DetailsPanelRootGrid.Visibility = Visibility.Visible;
            DetailsPanelDescription.IsReadOnly = true;
            DetailsPanelDescription.Background = Brushes.Transparent;
            DetailsPanelPrice.IsReadOnly = true;
            DetailsPanelPrice.Background = Brushes.Transparent;
            DetailsPanelName.IsReadOnly = true;
            DetailsPanelName.Background = Brushes.Transparent;
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
