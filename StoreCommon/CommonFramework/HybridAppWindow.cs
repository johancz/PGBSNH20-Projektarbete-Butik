using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreCommon
{
    public class HybridAppWindow : AdminFramework
    {
        public HybridAppWindow(Window mainWindow)
        {
            AppWindow = this;
            mainWindow.Title = "Aministrator View";
            mainWindow.Width = SystemParameters.WorkArea.Width >= 1000 ? SystemParameters.WorkArea.Width - 200 : 800;
            mainWindow.Height = SystemParameters.WorkArea.Height >= 800 ? SystemParameters.WorkArea.Height - 200 : 600;
            mainWindow.MinWidth = 800;
            mainWindow.MinHeight = 600;
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    var windowTabControl = new TabControl { Background = Brushes.LightBlue };
                        mainWindow.Content = windowTabControl;
            
            MainWindow = mainWindow;
                WindowTabControl = windowTabControl;

            MainWindow.KeyUp += MainWindow_KeyUp;
        }

        public void LoadGUI()
        {
            LoadDefaultAdminPage("Administrator mode", Brushes.AliceBlue);
            LoadBrowser();
            LoadDetailsPanel();
            LoadEditableTextBoxes();
            LoadButtons();
            LoadItemGrids();
        }
        private void LoadDefaultAdminPage(string header, Brush brush)
        {            
            var editPageTabItem = new TabItem { Header = header };
            WindowTabControl.Items.Add(editPageTabItem);

                var editPageGrid = new Grid { ShowGridLines = true, Background = brush };
                editPageGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                editPageGrid.ColumnDefinitions.Add(new ColumnDefinition());
                editPageGrid.ColumnDefinitions.Add(new ColumnDefinition());
                editPageTabItem.Content = editPageGrid;                    

            EditPageTabItem = editPageTabItem;
                EditPageGrid = editPageGrid;
                    
        }
        private void LoadBrowser()
        {
            var browserRootScrollViewer = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            Grid.SetColumn(browserRootScrollViewer, 0);
            EditPageGrid.Children.Add(browserRootScrollViewer);

                var browserProductsPanel = new WrapPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top
                };
                browserRootScrollViewer.Content = browserProductsPanel;

            BrowserRootScrollViewer = browserRootScrollViewer;
                BrowserProductsPanel = browserProductsPanel;

            browserRootScrollViewer.SizeChanged += BrowserRootScrollViewer_SizeChanged;
        }
        private void LoadDetailsPanel()
        {
            var detailsPanelRootGrid = new Grid { ShowGridLines = true, Background = Brushes.AntiqueWhite };
            detailsPanelRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            detailsPanelRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            Grid.SetColumn(detailsPanelRootGrid, 1);
            EditPageGrid.Children.Add(detailsPanelRootGrid);

                var detailsPanelImage = new Image();
                detailsPanelRootGrid.Children.Add(detailsPanelImage);

                var detailsTextAndButtonGrid = new Grid { ShowGridLines = true };
                detailsTextAndButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                detailsTextAndButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Grid.SetRow(detailsTextAndButtonGrid, 1);
                detailsPanelRootGrid.Children.Add(detailsTextAndButtonGrid);

                    var detailsTitleDescriptionGrid = new Grid { ShowGridLines = true };
                    detailsTitleDescriptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    detailsTitleDescriptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    Grid.SetColumn(detailsTitleDescriptionGrid, 1);
                
                    detailsTextAndButtonGrid.Children.Add(detailsTitleDescriptionGrid);

                        var productDescriptionScrollViewer = new ScrollViewer
                        {
                            Margin = new Thickness(5),
                            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                            HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                        };
                        Grid.SetRow(productDescriptionScrollViewer, 1);
                        detailsTitleDescriptionGrid.Children.Add(productDescriptionScrollViewer);
                        var nameAndPricePanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(5),
                        };
                        Grid.SetRow(nameAndPricePanel, 0);
                        detailsTitleDescriptionGrid.Children.Add(nameAndPricePanel);


                    var detailsButtonPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(5)
                    };
                    Grid.SetColumn(detailsButtonPanel, 0);
                    detailsTextAndButtonGrid.Children.Add(detailsButtonPanel);

            DetailsPanelRootGrid = detailsPanelRootGrid;
                DetailsPanelImage = detailsPanelImage;
                DetailsTextAndButtonGrid = detailsTextAndButtonGrid;
                    DetailsTitleAndDescriptionGrid = detailsTitleDescriptionGrid;
                        NameAndPricePanel = nameAndPricePanel;
                        DetailsDescriptionScrollViewer = productDescriptionScrollViewer;
                    DetailsButtonPanel = detailsButtonPanel;
        }
        public void LoadEditableTextBoxes()
        {
            var detailsPanelDescription = new TextBox
            {
                TextWrapping = TextWrapping.Wrap,
                Background = Brushes.Transparent,
                IsReadOnly = true,
                AcceptsReturn = true
            };
            DetailsDescriptionScrollViewer.Content = detailsPanelDescription;

            var detailsPanelName = new TextBox
            {
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Background = Brushes.Transparent,
                IsReadOnly = true
            };
            NameAndPricePanel.Children.Add(detailsPanelName);

            var detailsPanelPrice = new TextBox
            {
                Tag = "rightcolumn detailsprice",
                FontSize = 16,
                Background = Brushes.Transparent,
                IsReadOnly = true
            };
            NameAndPricePanel.Children.Add(detailsPanelPrice);

            var detailsPanelCurrency
                = new TextBox
            {
                FontSize = 16,
                Background = Brushes.Transparent,
                Text = "kr",
                IsReadOnly = true
            };
            NameAndPricePanel.Children.Add(detailsPanelCurrency);

            DetailsPanelDescription = detailsPanelDescription;
            DetailsPanelName = detailsPanelName;
            DetailsPanelPrice = detailsPanelPrice;
            DetailsPanelCurrency = detailsPanelCurrency;
        }
        private void LoadItemGrids()
        {
            foreach (var product in Store.Products)
            {
                var productGrid = CreateProductGridWithContent(product);
                ProductGrids.Add(productGrid);
                BrowserProductsPanel.Children.Add(productGrid);
            }
            foreach (var imageFilePath in Store.ImageItemFilePaths)
            {
                var imageGrid = CreateImageGridWithContent(imageFilePath);
                ImageGrids.Add(imageGrid);
            }
        }
        public Grid CreateProductGridWithContent(Product product)
        {
            var productsItem = CreateItemGridRoot();
            productsItem.Tag = product;

            productsItem.ColumnDefinitions.Add(new ColumnDefinition());
            productsItem.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            productsItem.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            productsItem.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            var productThumbnail = Helpers.CreateNewImage(product.Uri, ProductItem_LayoutSettings.gridItemImageHeight);
            Grid.SetColumn(productThumbnail, 1);
            productThumbnail.Tag = product.Uri;
            productThumbnail.Stretch = Stretch.UniformToFill;
            productThumbnail.VerticalAlignment = VerticalAlignment.Center;
            productThumbnail.HorizontalAlignment = HorizontalAlignment.Center;
            productThumbnail.Tag = product;

            var nameLabel = new Label
            {
                Content = product.Name,
                FontSize = 14,
            };
            Grid.SetColumn(nameLabel, 0);
            Grid.SetRow(nameLabel, 1);

            var priceLabel = new Label
            {
                Content = $"{product.Price} kr",
            };
            Grid.SetColumn(priceLabel, 1);
            Grid.SetRow(priceLabel, 1);

            productsItem.Children.Add(productThumbnail);
            productsItem.Children.Add(nameLabel);
            productsItem.Children.Add(priceLabel);

            ProductGrids.Add(productsItem);

            return productsItem;
        }
        public Grid CreateImageGridWithContent(string filePath)
        {
            var imageGrid = CreateItemGridRoot();
            var productThumbnail = Helpers.CreateNewImage(filePath, ProductItem_LayoutSettings.gridItemImageHeight);
            productThumbnail.Tag = filePath;
            productThumbnail.Stretch = Stretch.UniformToFill;
            productThumbnail.VerticalAlignment = VerticalAlignment.Center;
            productThumbnail.HorizontalAlignment = HorizontalAlignment.Center;
            imageGrid.Children.Add(productThumbnail);
            return imageGrid;
        }

        private void BrowserRootScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BrowserProductsPanel.Width = BrowserRootScrollViewer.ActualWidth;
        }
        private void LoadButtons()
        {
            var editButton = new Button
            {
                Tag = "edit",
                Padding = new Thickness(5),
                Content = new Label { Content = "Edit", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            EditButton = editButton;

            var saveChangesButton = new Button
            {
                Tag = "save changes",
                Padding = new Thickness(5),
                Content = new Label { Content = "Save Changes", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            SaveChangesButton = saveChangesButton;

            var removeButton = new Button
            {
                Tag = "remove",
                Padding = new Thickness(5),
                Content = new Label { Content = "Remove", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            RemoveButton = removeButton;

            var changeImageButton = new Button
            {
                Tag = "change image",
                Padding = new Thickness(5),
                Content = new Label { Content = "Change Image", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            ChangeImageButton = changeImageButton;

            var saveImageButton = new Button
            {
                Tag = "save image",
                Padding = new Thickness(5),
                Content = new Label { Content = "Save Image", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            SaveImageButton = saveImageButton;

            var cancelImageButton = new Button
            {
                Tag = "cancel image",
                Padding = new Thickness(5),
                Content = new Label { Content = "Cancel", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            CancelImageButton = cancelImageButton;

            var cancelButton = new Button
            {
                Tag = "cancel",
                Padding = new Thickness(5),
                Content = new Label { Content = "Cancel", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            CancelButton = cancelButton;

            var newProductButton = new Button
            {
                Tag = "new product",
                Padding = new Thickness(5),
                Content = new Label { Content = "New Product", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            NewProductButton = newProductButton;
        }
        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }
        private Grid CreateItemGridRoot()
        {
            var itemGrid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Top,
                Width = ProductItem_LayoutSettings.gridItemWidth,
                Height = ProductItem_LayoutSettings.gridItemHeight,
                Margin = new Thickness(5),
                Background = Brushes.LightGray
            };

            return itemGrid;
        }


        internal struct ProductItem_LayoutSettings
        {
            internal const double gridItemWidth = 200;
            internal const double gridItemHeight = 200;
            internal const int gridItemImageHeight = 175;
        }

    }
}
