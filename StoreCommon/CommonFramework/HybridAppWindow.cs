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
        }

        public void LoadGUI()
        {
            LoadDefaultAdminPage("Administrator mode", Brushes.AliceBlue);
                LoadBrowser();
                    LoadItemGrids();
                LoadDetailsPanel();
                    LoadEditableTextBoxes();
                    LoadButtons();
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
                private void LoadButtons()
                {
                    //Parent DetailsButtonPanel
                    NewProductButton = CreateButton("New Product");
                    EditProductButton = CreateButton("Edit Product");
                    ChangeImageButton = CreateButton("Change Image");
                    RemoveButton = CreateButton("Remove Product");

                    SaveImageButton = CreateButton("Save Image");
                    CancelImageButton = CreateButton("Cancel");

                    SaveChangesButton = CreateButton("Save Changes");
                    CancelButton = CreateButton("Cancel");            
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
                    ProductAndImageWrapPanel = browserProductsPanel;

                browserRootScrollViewer.SizeChanged += BrowserRootScrollViewer_SizeChanged;
            }
                private void LoadItemGrids()
                {
                    foreach (var product in Store.Products)
                    {
                        CreateProductGridWithContent(product);
                    }
                    foreach (var imageFilePath in Store.ImageItemFilePaths)
                    {
                        var imageGrid = CreateImageGridWithContent(imageFilePath, Brushes.Black);
                        ImageGrids.Add(imageGrid);
                    }
                }
                    public void CreateProductGridWithContent(Product product)
                    {
                        var productAndImageGrid = CreateProductGrid(product, Brushes.AliceBlue); //2 rows //2 columns
                        CreateProductThumbnail(productAndImageGrid, product, 1, 0);
                        CreateGridNameLabel(productAndImageGrid, product, 0, 1);
                        CreateGridPriceLabel(productAndImageGrid, product, 1, 1);
                    }
                        private Grid CreateProductGrid(Product product, Brush background) //2 columns //2 rows //needs to return grid right now
                        {
                            var productGrid = new Grid
                            {
                                VerticalAlignment = VerticalAlignment.Top,
                                Width = ProductItem_LayoutSettings.gridItemWidth,
                                Height = ProductItem_LayoutSettings.gridItemHeight,
                                Margin = new Thickness(5),
                                Background = background,
                                Tag = product
                            };

                            productGrid.ColumnDefinitions.Add(new ColumnDefinition());
                            productGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                            productGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                            productGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                            ProductGrids.Add(productGrid);
                            return productGrid;
                        }
                            public void CreateProductThumbnail(Grid parent, Product product, int column, int row)
                            {
                                var productThumbnail = Helpers.CreateNewImage(product.Uri, ProductItem_LayoutSettings.gridItemImageHeight);
                                productThumbnail.Stretch = Stretch.UniformToFill;
                                productThumbnail.VerticalAlignment = VerticalAlignment.Center;
                                productThumbnail.HorizontalAlignment = HorizontalAlignment.Center;
                                productThumbnail.Tag = product;

                                Grid.SetColumn(productThumbnail, column);
                                Grid.SetRow(productThumbnail, row);
                                parent.Children.Add(productThumbnail);
                            }
                            public void CreateGridNameLabel(Grid parent, Product product, int column, int row) //0,1 name
                            {
                                var nameLabel = new Label
                                {
                                    Content = product.Name,
                                    FontSize = 14,
                                };
                                Grid.SetColumn(nameLabel, column);
                                Grid.SetRow(nameLabel, row);
                                parent.Children.Add(nameLabel);
                            }
                            public void CreateGridPriceLabel(Grid parent, Product product, int column, int row) //1,1 price
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
                    public Grid CreateImageGridWithContent(string filePath, Brush background)
                    {
                        var imageGrid = new Grid
                        {
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = ProductItem_LayoutSettings.gridItemWidth,
                            Height = ProductItem_LayoutSettings.gridItemHeight,
                            Margin = new Thickness(5),
                            Background = background
                        };

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
            ProductAndImageWrapPanel.Width = BrowserRootScrollViewer.ActualWidth;
        }


        internal struct ProductItem_LayoutSettings
        {
            internal const double gridItemWidth = 200;
            internal const double gridItemHeight = 200;
            internal const int gridItemImageHeight = 175;
        }

    }
}
