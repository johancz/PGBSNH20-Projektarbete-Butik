using StoreAdmin.Views;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StoreCommon
{
    public class HybridAppWindow : AdminFramework
    {
        internal struct ProductItem_LayoutSettings
        {
            internal const double gridItemWidth = 200;
            internal const double gridItemHeight = 200;
            internal const int gridItemImageHeight = 175;
        }
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
        }

        public void CreateAdminGUI()
        {
            CreateAdminPage("Administrator mode", Brushes.AliceBlue);
                CreateBrowser(Brushes.WhiteSmoke);
                    CreateProductGridsToCollection(Brushes.LightGray);
                    CreateImageGridsToCollection(Brushes.Black);
                CreateDetailsPanel();
                    CreateEditableTextBoxes();
                    CreateAdminButtonsToCollection();
            //CreateAdminPage("Manage Discount Codes", Brushes.Azure);

            // Edit Discount Codes
            WindowTabControl.Items.Add(ManageDiscountCodesView.Init());
        }
        private void CreateAdminPage(string header, Brush brush)
        {            
            var editPageTabItem = new TabItem { Header = header };
            WindowTabControl.Items.Add(editPageTabItem);

                var editPageGrid = new Grid { Background = brush };
                editPageGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                editPageGrid.ColumnDefinitions.Add(new ColumnDefinition());
                editPageGrid.ColumnDefinitions.Add(new ColumnDefinition());
                editPageTabItem.Content = editPageGrid;                    

            EditPageTabItem = editPageTabItem;
                EditPageGrid = editPageGrid;
                    
        }
            private void CreateDetailsPanel()
            {
                var detailsPanelRootGrid = new Grid { Background = Brushes.AntiqueWhite };
                detailsPanelRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                detailsPanelRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                Grid.SetColumn(detailsPanelRootGrid, 1);
                EditPageGrid.Children.Add(detailsPanelRootGrid);

                    var detailsPanelImage = new Image();
                    detailsPanelRootGrid.Children.Add(detailsPanelImage);

                    var detailsTextAndButtonGrid = new Grid { };
                    detailsTextAndButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                    detailsTextAndButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    Grid.SetRow(detailsTextAndButtonGrid, 1);
                    detailsPanelRootGrid.Children.Add(detailsTextAndButtonGrid);

                        var detailsTitleDescriptionGrid = new Grid { };
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
                public void CreateEditableTextBoxes()
                {
                    var detailsPanelDescription = new TextBox
                    {
                        TextWrapping = TextWrapping.Wrap,
                        AcceptsReturn = true,
                        IsUndoEnabled = true,                        
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
                    };
                    DetailsDescriptionScrollViewer.Content = detailsPanelDescription;

                    var detailsPanelName = new TextBox
                    {
                        FontSize = 16,
                        FontWeight = FontWeights.SemiBold,
                    };
                    NameAndPricePanel.Children.Add(detailsPanelName);

                    var detailsPanelPrice = new TextBox
                    {
                        FontSize = 16
                    };
                    NameAndPricePanel.Children.Add(detailsPanelPrice);

                    var detailsPanelCurrency = new TextBox
                    {
                        FontSize = 16,
                        Background = Brushes.Transparent,
                        Text = "kr",
                        BorderBrush = Brushes.Transparent
                    };
                    NameAndPricePanel.Children.Add(detailsPanelCurrency);

                    DetailsPanelDescription = detailsPanelDescription;
                    DetailsPanelName = detailsPanelName;
                    DetailsPanelPrice = detailsPanelPrice;
                    DetailsPanelCurrency = detailsPanelCurrency;
                    
                    EditDetailsTextBoxes = new List<TextBox> { detailsPanelDescription, detailsPanelName, detailsPanelPrice};                    
                }
                private void CreateAdminButtonsToCollection()
                {
                    //Parent DetailsButtonPanel
                    EditProductButton = CreateButton("Edit Product");
                        SaveChangesButton = CreateButton("Save Changes");
                        CancelButton = CreateButton("Cancel");

                    ChangeImageButton = CreateButton("Change Image");
                        SaveImageButton = CreateButton("Save Image");
                        CancelImageButton = CreateButton("Cancel");
                    
                    NewProductButton = CreateButton("New Product");
                        NewProductSaveButton = CreateButton("Save Image");
                        NewProductAbortButton = CreateButton("Abort Product");

                    RemoveButton = CreateButton("Remove Product");
                    
                    AdminButtons = new List<Button> { NewProductButton, EditProductButton, ChangeImageButton, RemoveButton, SaveImageButton, CancelImageButton, SaveChangesButton, CancelButton, NewProductAbortButton, NewProductSaveButton };
                }
                    private Button CreateButton(string content)
                    {
                        var newButton = new Button
                        {
                            Padding = new Thickness(5),
                            Content = new Label { Content = content, HorizontalAlignment = HorizontalAlignment.Center },
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                        };
                        return newButton;
                    }
            private void CreateBrowser(Brush background)
            {
                var browserRootScrollViewer = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Hidden, Background = background };
                Grid.SetColumn(browserRootScrollViewer, 0);
                EditPageGrid.Children.Add(browserRootScrollViewer);

                    var browserProductsPanel = new WrapPanel
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Top,
                    };
                    browserRootScrollViewer.Content = browserProductsPanel;

                BrowserRootScrollViewer = browserRootScrollViewer;
                    ProductAndImageWrapPanel = browserProductsPanel;
            }
                private void CreateProductGridsToCollection(Brush background)
                {
                    foreach (var product in Store.Products)
                    {
                        var productGrid = CreateProductGridWithContent(product);
                        ProductGrids.Add(productGrid);
                        productGrid.Background = background;
                    }
                }
                    public Grid CreateProductGridWithContent(Product product)
                    {
                        var productGrid = CreateProductGrid(product); //2 rows //2 columns
                        CreateProductThumbnail(productGrid, product);
                        CreateGridNameLabel(productGrid, product, column: 0, row: 1);
                        CreateGridPriceLabel(productGrid, product, column: 1, row: 1);
                        return productGrid;
                    }
                        private Grid CreateProductGrid(Product product)
                        {
                            var productGrid = new Grid
                            {
                                VerticalAlignment = VerticalAlignment.Top,
                                Width = ProductItem_LayoutSettings.gridItemWidth,
                                Height = ProductItem_LayoutSettings.gridItemHeight,
                                Margin = new Thickness(5),
                                Tag = product
                            };
                            productGrid.ColumnDefinitions.Add(new ColumnDefinition());
                            productGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                            productGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                            productGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                            
                            return productGrid;
                        }
                            public void CreateProductThumbnail(Grid parent, Product product)
                            {
                                var productThumbnail = Helpers.CreateNewImage(product.Uri, ProductItem_LayoutSettings.gridItemImageHeight);
                                productThumbnail.Stretch = Stretch.UniformToFill;
                                productThumbnail.VerticalAlignment = VerticalAlignment.Center;
                                productThumbnail.HorizontalAlignment = HorizontalAlignment.Center;
                                productThumbnail.Tag = product;
                                product.Tag = productThumbnail;
                                Grid.SetColumnSpan(productThumbnail, 2);
                                parent.Children.Add(productThumbnail);
                            }
                            public void CreateGridNameLabel(Grid parent, Product product, int column, int row)
                            {
                                var nameLabel = new Label
                                {
                                    Content = $"{product.Name}",
                                    FontSize = 14,
                                };
                                Grid.SetColumn(nameLabel, column);
                                Grid.SetRow(nameLabel, row);
                                parent.Children.Add(nameLabel);
                            }
                            public void CreateGridPriceLabel(Grid parent, Product product, int column, int row)
                            {
                                var priceLabel = new Label
                                {
                                    Content = $"{product.Price} kr",
                                    FontSize = 14,
                                };
                                Grid.SetColumn(priceLabel, column);
                                Grid.SetRow(priceLabel, row);
                                parent.Children.Add(priceLabel);
                            }
                    private void CreateImageGridsToCollection(Brush background)
                    {
                        foreach (var imageFilePath in Store.ImageItemFilePaths)
                        {
                            var imageGrid = CreateImageGridWithContent(imageFilePath, background);
                            ImageGrids.Add(imageGrid);
                        }
                    }
                        public Grid CreateImageGridWithContent(string filePath, Brush background)
                        {
                            var selectableImage = Helpers.CreateNewImage(filePath, ProductItem_LayoutSettings.gridItemImageHeight);
                            selectableImage.Tag = filePath;
                            selectableImage.Stretch = Stretch.UniformToFill;
                            selectableImage.VerticalAlignment = VerticalAlignment.Center;
                            selectableImage.HorizontalAlignment = HorizontalAlignment.Center;
                            
                            var imageGrid = new Grid
                            {
                                Tag = selectableImage.Source,
                                VerticalAlignment = VerticalAlignment.Top,
                                Width = ProductItem_LayoutSettings.gridItemWidth,
                                Height = ProductItem_LayoutSettings.gridItemHeight,
                                Margin = new Thickness(5),
                                Background = background
                            };            
                            imageGrid.Children.Add(selectableImage);

                            return imageGrid;
                        }        
    }
}
