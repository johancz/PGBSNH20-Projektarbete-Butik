using StoreAdmin.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreCommon
{
    // This class creates all Framework Elements in the App Window - and wraps around the wpf Window object. It sepparates the creation of elements from events and links all elements through the abstract Admin Framework class. The elements and layout is mostly copyed from user view with some tweaks.
    public class AdminHybridWindow : SharedElementTree
    {
        // This is gridsize for the smaller product-images in the left column, the width is overriden by the window size-changed event.
        internal struct ProductItem_LayoutSettings
        {
            internal const double gridItemWidth = 200;
            internal const double gridItemHeight = 200;
            internal const int gridItemImageHeight = 175;
        }
        public AdminHybridWindow(Window mainWindow)
        {
            AppWindow = this;
            mainWindow.Title = DataManager.ProjectName + " (admin mode)";
            mainWindow.Width = SystemParameters.WorkArea.Width >= 1000 ? SystemParameters.WorkArea.Width - 200 : 800;
            mainWindow.Height = SystemParameters.WorkArea.Height >= 800 ? SystemParameters.WorkArea.Height - 200 : 600;
            mainWindow.MinWidth = 800;
            mainWindow.MinHeight = 600;
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Uri iconUri = new Uri(Path.Combine(Environment.CurrentDirectory, "StoreData", "Image Helpers", "soap.ico"), UriKind.Absolute);
            mainWindow.Icon = BitmapFrame.Create(iconUri);

            var windowTabControl = new TabControl { Background = Brushes.LightBlue };
            mainWindow.Content = windowTabControl;

            MainWindow = mainWindow;
            WindowTabControl = windowTabControl;
        }

        // Gives an overview of the wpf parts created, the colorparameter gives a simple way to find the elements and to do some styling.
        public void CreateAdminGUI()
        {
            CreateEditPage("Administrator mode", Brushes.WhiteSmoke);
            CreateBrowser(Brushes.WhiteSmoke);
            CreateProductGridItems(Brushes.WhiteSmoke);
            CreateImageGridItems(Brushes.Black);
            CreateDetailsPanel(Brushes.WhiteSmoke);
            CreateEditableTextBoxes();
            CreateAdminButtons();

            CreateManageDiscountCodesView("Manage Discount Codes", Brushes.WhiteSmoke); //Is defined in ManageDiscountCodeView.cs
        }
        private void CreateEditPage(string header, Brush brush)
        {
            var editPageTabItem = new TabItem { Header = header };
            WindowTabControl.Items.Add(editPageTabItem);

            var editPageGrid = new Grid { Background = brush };
            editPageGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            editPageGrid.ColumnDefinitions.Add(new ColumnDefinition());
            editPageGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            editPageTabItem.Content = editPageGrid;

            EditPageTabItem = editPageTabItem;
            EditPageGrid = editPageGrid;
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
                private void CreateProductGridItems(Brush background)
                {
                    foreach (var product in Store.Products)
                    {
                        var productGridItem = CreateProductGridItem(product);
                        productGridItem.Background = background;
                    }
                }
                    public Grid CreateProductGridItem(Product product)
                    {
                        var productGridItem = new Grid
                        {
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = ProductItem_LayoutSettings.gridItemWidth,
                            Height = ProductItem_LayoutSettings.gridItemHeight,
                            Margin = new Thickness(5),
                            Tag = product
                        };
                        productGridItem.ColumnDefinitions.Add(new ColumnDefinition());
                        productGridItem.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                        productGridItem.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                        productGridItem.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                        CreateProductThumbnail(productGridItem, product);
                        CreateGridNameLabel(productGridItem, product, column: 0, row: 1);

                        ProductGridItems.Add(productGridItem);
                        return productGridItem;
                    }
                        public void CreateProductThumbnail(Grid parent, Product product)
                        {
                            var productThumbnail = ImageCreation.CreateNewImage(product.Uri, ProductItem_LayoutSettings.gridItemImageHeight);
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
                                Content = $"{product.Name} {product.Price} kr",
                                FontSize = 14,
                            };
                            Grid.SetColumn(nameLabel, 0);
                            Grid.SetRow(nameLabel, 1);
                            parent.Children.Add(nameLabel);
                        }
                private void CreateImageGridItems(Brush background)
                {
                    foreach (var imageFilePath in Store.ImageItemFilePaths)
                    {
                        var imageGridItem = CreateImageGridItem(imageFilePath, background);
                        ImageGridItems.Add(imageGridItem);
                    }
                }
                    public Grid CreateImageGridItem(string filePath, Brush background)
                    {
                        var selectableImage = ImageCreation.CreateNewImage(filePath, ProductItem_LayoutSettings.gridItemImageHeight);
                        selectableImage.Tag = filePath;
                        selectableImage.Stretch = Stretch.UniformToFill;
                        selectableImage.VerticalAlignment = VerticalAlignment.Center;
                        selectableImage.HorizontalAlignment = HorizontalAlignment.Center;

                        var imageGridItem = new Grid
                        {
                            Tag = selectableImage.Source,
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = ProductItem_LayoutSettings.gridItemWidth,
                            Height = ProductItem_LayoutSettings.gridItemHeight,
                            Margin = new Thickness(5),
                            Background = background
                        };
                        imageGridItem.Children.Add(selectableImage);

                        return imageGridItem;
                    }
                private void CreateDetailsPanel(Brush background)
                {
                    var detailsPanelRootGrid = new Grid
                    {
                        Background = background,
                        HorizontalAlignment = HorizontalAlignment.Left,
                    };
                    detailsPanelRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    detailsPanelRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    Grid.SetColumn(detailsPanelRootGrid, 1);
                    EditPageGrid.Children.Add(detailsPanelRootGrid);

                        var detailsPanelImage = new Image();
                        detailsPanelRootGrid.Children.Add(detailsPanelImage);

                        var detailsTextAndButtonGrid = new Grid { };
                        detailsTextAndButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
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
                            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                            Width = 270
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

                        EditDetailsTextBoxes = new List<TextBox> { detailsPanelDescription, detailsPanelName, detailsPanelPrice };
                    }
                    private void CreateAdminButtons()
                    {
                        // The parent of all of these buttons is: 'DetailsButtonPanel'
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

                        foreach (var button in AdminButtons)
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
                        private Button CreateButton(string content)
                        {
                            var newButton = new Button
                            {
                                Padding = new Thickness(5),
                                Content = new Label { Content = content, },
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                HorizontalContentAlignment = HorizontalAlignment.Left,
                                Visibility = Visibility.Collapsed,
                            };
                            return newButton;
                        }
        private void CreateManageDiscountCodesView(string header, Brush brush)
        {
            var editPageTabItem = new TabItem { Header = header };
            editPageTabItem.Content = ManageDiscountCodesView.Init();
            ((ScrollViewer)editPageTabItem.Content).Background = brush;
            WindowTabControl.Items.Add(editPageTabItem);
            EditDiscountCodeTabItem = editPageTabItem;
        }
    }
}
