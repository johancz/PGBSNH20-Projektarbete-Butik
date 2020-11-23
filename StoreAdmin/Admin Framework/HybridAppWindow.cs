using StoreAdmin.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        public HybridAppWindow(Window actualWindow)
        {
            AppWindow = this;
            actualWindow.Title = "Aministrator View";
            actualWindow.Width = SystemParameters.WorkArea.Width >= 1000 ? SystemParameters.WorkArea.Width - 200 : 800;
            actualWindow.Height = SystemParameters.WorkArea.Height >= 800 ? SystemParameters.WorkArea.Height - 200 : 600;
            actualWindow.MinWidth = 800;
            actualWindow.MinHeight = 600;
            actualWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var windowCanvas = new Canvas();

            var windowTabControl = new TabControl { Background = Brushes.LightBlue };
                actualWindow.Content = windowCanvas;
                    windowCanvas.Children.Add(windowTabControl);
            Canvas.SetLeft(windowTabControl, 0);
            Canvas.SetRight(windowTabControl, 0);
            ActualWindow = actualWindow;
                WindowCanvas = windowCanvas;
                    WindowTabControl = windowTabControl;
        }

        public void CreateAdminGUI()
        {
            CreateEditPage("Administrator mode", Brushes.AliceBlue);
                CreateBrowser(Brushes.WhiteSmoke);
                    CreateProductGridsToCollection(Brushes.LightGray);
                    CreateImageGridsToCollection(Brushes.Black);
                CreateDetailsPanel();
                    CreateEditableTextBoxes();
                    CreateAdminButtonsToCollection();
            CreateDiscountPage("Manage Discount Codes", Brushes.Azure);
                EditDiscountCodePageGrid.Children.Add(ManageDiscountCodesView.Init());
            CreateDiscountCodeImage("TylerBeast.jpeg");

            NewRootPanel.Children.Add(BrowserRootScrollViewer);
            NewRootPanel.Children.Add(DetailsButtonPanel);
            NewRootPanel.Children.Add(NewDetailsPanel);
                   
        }
        private void CreateDiscountPage(string header, Brush brush)
        {
            var editPageTabItem = new TabItem { Header = header };
            WindowTabControl.Items.Add(editPageTabItem);

            var editPageGrid = new Grid { Background = brush };
            editPageGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            editPageGrid.ColumnDefinitions.Add(new ColumnDefinition());
            editPageGrid.ColumnDefinitions.Add(new ColumnDefinition());
            editPageTabItem.Content = editPageGrid;

            EditDiscountCodeTabItem = editPageTabItem;
                EditDiscountCodePageGrid = editPageGrid;
        }
            private void CreateDiscountCodeImage(string uriRelative)
            {
                var discountCodeImage = new Image();
                discountCodeImage.Source = Helpers.CreateBitmapImageFromUriString(uriRelative, true);
                discountCodeImage.Stretch = Stretch.UniformToFill;
                discountCodeImage.VerticalAlignment = VerticalAlignment.Center;
                discountCodeImage.HorizontalAlignment = HorizontalAlignment.Center;
            
                Grid.SetColumn(discountCodeImage, 1);
                EditDiscountCodePageGrid.Children.Add(discountCodeImage);
            }
        private void CreateEditPage(string header, Brush brush)
        {            
            var editPageTabItem = new TabItem { Header = header };
            WindowTabControl.Items.Add(editPageTabItem);

                var editPageGrid = new Grid { Background = brush };
                editPageGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                editPageGrid.ColumnDefinitions.Add(new ColumnDefinition());
                editPageGrid.ColumnDefinitions.Add(new ColumnDefinition());
                //editPageTabItem.Content = editPageGrid;                    

            var newRootPanel = new StackPanel { Orientation = Orientation.Horizontal };
            editPageTabItem.Content = newRootPanel;

            EditPageTabItem = editPageTabItem;
                NewRootPanel = newRootPanel;
                //EditPageGrid = editPageGrid;                    
        }
            private void CreateDetailsPanel()
            {

            //var detailsPanelRootGrid = new Grid { Background = Brushes.AntiqueWhite };
            //detailsPanelRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //detailsPanelRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //Grid.SetColumn(detailsPanelRootGrid, 1);
            //EditPageGrid.Children.Add(detailsPanelRootGrid);

            var newDetailsPanel = new StackPanel { Orientation = Orientation.Vertical };


                    var detailsPanelImage = new Image {HorizontalAlignment = HorizontalAlignment.Left};
                    newDetailsPanel.Children.Add(detailsPanelImage);

                    //var detailsTextAndButtonGrid = new Grid { };
                    //detailsTextAndButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                    //detailsTextAndButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    //Grid.SetRow(detailsTextAndButtonGrid, 1);
                    //newDetailsPanel.Children.Add(detailsTextAndButtonGrid);

                        //var detailsTitleDescriptionGrid = new Grid { };
                        //detailsTitleDescriptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                        //detailsTitleDescriptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                        //Grid.SetColumn(detailsTitleDescriptionGrid, 1);
                
                        //detailsTextAndButtonGrid.Children.Add(detailsTitleDescriptionGrid);

                            //var productDescriptionScrollViewer = new ScrollViewer
                            //{
                            //    Margin = new Thickness(5),
                            //    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                            //    HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                            //};
                            //Grid.SetRow(productDescriptionScrollViewer, 1);
                            //detailsTitleDescriptionGrid.Children.Add(productDescriptionScrollViewer);
                            var nameAndPricePanel = new StackPanel
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(5),
                            };
                            Grid.SetRow(nameAndPricePanel, 0);
                            newDetailsPanel.Children.Add(nameAndPricePanel);


                        var detailsButtonPanel = new StackPanel
                        {
                            Orientation = Orientation.Vertical,
                            Margin = new Thickness(5)
                        };
                        Grid.SetColumn(detailsButtonPanel, 0);
                        //detailsTextAndButtonGrid.Children.Add(detailsButtonPanel);
                NewDetailsPanel = newDetailsPanel;
                //DetailsPanelRootGrid = detailsPanelRootGrid;
                    DetailsPanelImage = detailsPanelImage;
                    //DetailsTextAndButtonGrid = detailsTextAndButtonGrid;
                    //    DetailsTitleAndDescriptionGrid = detailsTitleDescriptionGrid;
                            NameAndPricePanel = nameAndPricePanel;
                            //DetailsDescriptionScrollViewer = productDescriptionScrollViewer;
                        DetailsButtonPanel = detailsButtonPanel;
            }
                public void CreateEditableTextBoxes()
                {
                    var detailsPanelDescription = new TextBox
                    {
                        TextWrapping = TextWrapping.Wrap,
                        AcceptsReturn = true,
                        IsUndoEnabled = true,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                    };
                    NewDetailsPanel.Children.Add(detailsPanelDescription);

                    var detailsPanelName = new TextBox
                    {
                        FontSize = 16,
                        FontWeight = FontWeights.SemiBold,
                    };
                    NameAndPricePanel.Children.Add(detailsPanelName);

                    var detailsPanelPrice = new TextBox
                    {
                        FontSize = 16,
                        Background = Brushes.Transparent,
                        BorderBrush = Brushes.Transparent,
                    };
                    NameAndPricePanel.Children.Add(detailsPanelPrice);

                    var detailsPanelCurrency = new TextBox
                    {
                        FontSize = 16,
                        Background = Brushes.Transparent,
                        Text = Store.Currency.Symbol,
                        BorderBrush = Brushes.Transparent,
                        IsReadOnly = true
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
                        SaveEditButton = CreateButton("Save");
                        CancelEditButton = CreateButton("Cancel Edit");

                    ChangeImageButton = CreateButton("Change Image");
                        SaveImageButton = CreateButton("Save Image");
                        CancelImageButton = CreateButton("Cancel Image");
                    
                    NewProductButton = CreateButton("New Product");
                        NewProductSaveButton = CreateButton("Save New");
                        NewProductAbortButton = CreateButton("Abort New");

                    RemoveButton = CreateButton("Remove Product");
                    
                    AdminButtons = new List<Button> { NewProductButton, EditProductButton, ChangeImageButton, RemoveButton, SaveImageButton, CancelImageButton, SaveEditButton, CancelEditButton, NewProductAbortButton, NewProductSaveButton };
                }
                    public Button CreateButton(string content)
                    {
                        var newButton = new Button
                        {
                            Padding = new Thickness(5),
                            Content = new Label { Content = content, HorizontalAlignment = HorizontalAlignment.Center },
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            HorizontalContentAlignment = HorizontalAlignment.Center,
                        };
                        return newButton;
                    }
            private void CreateBrowser(Brush background)
            {
                var browserRootScrollViewer = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Hidden, Background = background };
                //Grid.SetColumn(browserRootScrollViewer, 0);
                //EditPageGrid.Children.Add(browserRootScrollViewer);

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
                        productGrid.Background = background;
                    }
                }
                    public Grid CreateProductGridWithContent(Product product)
                    {
                        var productGrid = CreateProductGrid(product); //2 rows //2 columns
                        CreateProductThumbnail(productGrid, product);
                        CreateGridNameLabel(productGrid, product, column: 0, row: 1);
                        //CreateGridPriceLabel(productGrid, product, column: 1, row: 1);
                        ProductGrids.Add(productGrid);
                        return productGrid;
                    }
                        private Grid CreateProductGrid(Product product)
                        {
                            var productGrid = new Grid
                            {
                                VerticalAlignment = VerticalAlignment.Top,
                                Width = ProductItem_LayoutSettings.gridItemWidth,
                                //Height = ProductItem_LayoutSettings.gridItemHeight,
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
                                    Content = $"{product.Name} {product.Price} {Store.Currency.Symbol}",
                                    FontStretch = FontStretches.UltraExpanded
                                    //FontSize = 14,
                                };
                                Grid.SetColumn(nameLabel, column);
                                Grid.SetRow(nameLabel, row);
                                parent.Children.Add(nameLabel);
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
