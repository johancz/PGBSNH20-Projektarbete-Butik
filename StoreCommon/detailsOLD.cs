//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace StoreCommon
//{
//    using System.Windows;
//    using System.Windows.Controls;
//    using System.Windows.Controls.Primitives;
//    using System.Windows.Input;
//    using System.Windows.Media;
//    using System.Windows.Media.Imaging;

//    namespace StoreCommon
//    {
//        public class DetailsPanel : AdminFramework
//        {
//            //private static Product Selected;
//            public string Tag;
//            private Grid Parent;
//            private Grid _rightColumnContentRoot;
//            private Grid _detailsColumn_detailsGrid;
//            private TextBox _rightColumn_DetailsName;
//            private TextBox _rightColumn_DetailsDescription;
//            private Image _rightColumn_DetailsImage;
//            private StackPanel _rightColumn_detailsPanel_nameAndPrice;
//            public DetailsPanel(Grid parent, Brush brush, string tag)
//            {
//                Tag = tag;
//                Parent = parent;
//                detailsPanel = this;

//                _rightColumnContentRoot = new Grid { ShowGridLines = true, Background = brush };

//                _rightColumnContentRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//                _rightColumnContentRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//                // Create and add a Product.Image to the right column's root (StackPanel)
//                var rightColumn_DetailsImage = new Image { Tag = "rightcolumn detailsimage" };
//                _rightColumn_DetailsImage = rightColumn_DetailsImage;

//                Elements.Add(rightColumn_DetailsImage);

//                _rightColumnContentRoot.Children.Add(rightColumn_DetailsImage);

//                var detailsColumn_detailsGrid = new Grid { ShowGridLines = true };
//                _detailsColumn_detailsGrid = detailsColumn_detailsGrid;

//                detailsColumn_detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
//                detailsColumn_detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

//                Grid.SetRow(detailsColumn_detailsGrid, 1);
//                _rightColumnContentRoot.Children.Add(detailsColumn_detailsGrid);

//                var detailsColumn_namePriceDescription = new Grid { ShowGridLines = true };
//                detailsColumn_namePriceDescription.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//                detailsColumn_namePriceDescription.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//                // Create the product "Name" and "Price" labels and a StackPanel-parent for them. Add the parent to the detailsPanel.

//                _rightColumn_detailsPanel_nameAndPrice = new StackPanel
//                {
//                    Orientation = Orientation.Horizontal,
//                    Margin = new Thickness(5),
//                };
//                _rightColumnContentRoot.Visibility = Visibility.Hidden;

//                _rightColumn_DetailsName = new TextBox
//                {
//                    Tag = "rightcolumn detailsname",
//                    FontSize = 16,
//                    FontWeight = FontWeights.SemiBold,
//                    Background = Brushes.Transparent,
//                    IsReadOnly = true
//                };
//                Elements.Add(_rightColumn_DetailsName);
//                _rightColumn_detailsPanel_nameAndPrice.Children.Add(_rightColumn_DetailsName);

//                var rightColumn_DetailsPrice = new TextBox
//                {
//                    Tag = "rightcolumn detailsprice",
//                    FontSize = 16,
//                    Background = Brushes.Transparent,
//                    IsReadOnly = true
//                };
//                Elements.Add(rightColumn_DetailsPrice);

//                _rightColumn_detailsPanel_nameAndPrice.Children.Add(rightColumn_DetailsPrice);

//                var rightColumn_DetailsCurrency = new TextBox
//                {
//                    Tag = "rightcolumn detailscurrency",
//                    FontSize = 16,
//                    Background = Brushes.Transparent,
//                    Text = "kr",
//                    IsReadOnly = true
//                };
//                Elements.Add(rightColumn_DetailsPrice);

//                _rightColumn_detailsPanel_nameAndPrice.Children.Add(rightColumn_DetailsCurrency);


//                Grid.SetRow(_rightColumn_detailsPanel_nameAndPrice, 0);
//                detailsColumn_namePriceDescription.Children.Add(_rightColumn_detailsPanel_nameAndPrice);


//                _rightColumn_DetailsDescription = new TextBox
//                {
//                    Tag = "rightcolumn detailsdescription",
//                    TextWrapping = TextWrapping.Wrap,
//                    Background = Brushes.Transparent,
//                    IsReadOnly = true,
//                    AcceptsReturn = true
//                };
//                Elements.Add(_rightColumn_DetailsDescription);
//                // Create the product description Label
//                var scrollViewer = new ScrollViewer
//                {
//                    Margin = new Thickness(5),
//                    Content = _rightColumn_DetailsDescription,
//                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
//                    HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
//                };
//                Grid.SetRow(scrollViewer, 1);
//                detailsColumn_namePriceDescription.Children.Add(scrollViewer);
//                Grid.SetColumn(detailsColumn_namePriceDescription, 1);
//                detailsColumn_detailsGrid.Children.Add(detailsColumn_namePriceDescription);

//                // Add the right-column to the "root"-Grid.
//                Grid.SetColumn(_rightColumnContentRoot, 1);
//                Parent.Children.Add(_rightColumnContentRoot);
//            }
//            public void AddNewProductButtonPanel()
//            {
//                var rightColumn_detailsPanel_NewProductsButton = new StackPanel
//                {
//                    Orientation = Orientation.Vertical,
//                    Margin = new Thickness(5),
//                    Tag = "new product buttons"
//                };

//                Grid.SetColumn(rightColumn_detailsPanel_NewProductsButton, 0);
//                _detailsColumn_detailsGrid.Children.Add(rightColumn_detailsPanel_NewProductsButton);

//                var saveChangesButton = new Button
//                {
//                    Tag = "save changes",
//                    Padding = new Thickness(5),
//                    Content = new Label { Content = "Save Changes", HorizontalAlignment = HorizontalAlignment.Left },
//                    HorizontalAlignment = HorizontalAlignment.Stretch,
//                    HorizontalContentAlignment = HorizontalAlignment.Left,
//                };
//                Elements.Add(saveChangesButton);
//                rightColumn_detailsPanel_NewProductsButton.Children.Add(saveChangesButton);

//            }
//            public void AddAdminButtonPanel()
//            {
//                var rightColumn_detailsPanel_AdminButtons = new StackPanel
//                {
//                    Orientation = Orientation.Vertical,
//                    Margin = new Thickness(5),
//                    Tag = "admin buttons"
//                };
//                Elements.Add(rightColumn_detailsPanel_AdminButtons);

//                var editButton = new Button
//                {
//                    Tag = "edit",
//                    Padding = new Thickness(5),
//                    Content = new Label { Content = "Edit", HorizontalAlignment = HorizontalAlignment.Left },
//                    HorizontalAlignment = HorizontalAlignment.Stretch,
//                    HorizontalContentAlignment = HorizontalAlignment.Left,
//                };
//                Elements.Add(editButton);

//                editButton.Click += EditButton_Click;

//                var saveChangesButton = new Button
//                {
//                    Tag = "save changes",
//                    Padding = new Thickness(5),
//                    Content = new Label { Content = "Save Changes", HorizontalAlignment = HorizontalAlignment.Left },
//                    HorizontalAlignment = HorizontalAlignment.Stretch,
//                    HorizontalContentAlignment = HorizontalAlignment.Left,
//                };
//                Elements.Add(saveChangesButton);
//                saveChangesButton.Click += SaveChangesButton_Click;

//                var removeButton = new Button
//                {
//                    Tag = "remove",
//                    Padding = new Thickness(5),
//                    Content = new Label { Content = "Remove", HorizontalAlignment = HorizontalAlignment.Left },
//                    HorizontalAlignment = HorizontalAlignment.Stretch,
//                    HorizontalContentAlignment = HorizontalAlignment.Left,
//                };
//                Elements.Add(removeButton);
//                removeButton.Click += RemoveButton_Click;

//                var changeImageButton = new Button
//                {
//                    Tag = "change image",
//                    Padding = new Thickness(5),
//                    Content = new Label { Content = "Change Image", HorizontalAlignment = HorizontalAlignment.Left },
//                    HorizontalAlignment = HorizontalAlignment.Stretch,
//                    HorizontalContentAlignment = HorizontalAlignment.Left,
//                };
//                Elements.Add(changeImageButton);
//                changeImageButton.Click += ChangeImageButton_Click;

//                var cancelButton = new Button
//                {
//                    Tag = "cancel",
//                    Padding = new Thickness(5),
//                    Content = new Label { Content = "Cancel", HorizontalAlignment = HorizontalAlignment.Left },
//                    HorizontalAlignment = HorizontalAlignment.Stretch,
//                    HorizontalContentAlignment = HorizontalAlignment.Left,
//                };
//                Elements.Add(cancelButton);
//                cancelButton.Click += CancelButton_Click;

//                var newProductButton = new Button
//                {
//                    Tag = "new product",
//                    Padding = new Thickness(5),
//                    Content = new Label { Content = "New Product", HorizontalAlignment = HorizontalAlignment.Left },
//                    HorizontalAlignment = HorizontalAlignment.Stretch,
//                    HorizontalContentAlignment = HorizontalAlignment.Left,
//                };
//                Elements.Add(newProductButton);
//                newProductButton.Click += NewProductButton_Click;

//                rightColumn_detailsPanel_AdminButtons.Children.Add(cancelButton);
//                rightColumn_detailsPanel_AdminButtons.Children.Add(changeImageButton);
//                rightColumn_detailsPanel_AdminButtons.Children.Add(editButton);
//                rightColumn_detailsPanel_AdminButtons.Children.Add(removeButton);
//                rightColumn_detailsPanel_AdminButtons.Children.Add(saveChangesButton);
//                rightColumn_detailsPanel_AdminButtons.Children.Add(newProductButton);

//                Grid.SetColumn(rightColumn_detailsPanel_AdminButtons, 0);
//                _detailsColumn_detailsGrid.Children.Add(rightColumn_detailsPanel_AdminButtons);
//            }

//            private void CancelButton_Click(object sender, RoutedEventArgs e)
//            {
//                //Disables editable fields
//                var textbox = ((TextBox)GetElement("rightcolumn detailsdescription"));
//                textbox.IsReadOnly = true;
//                textbox.Background = Brushes.Transparent;

//                var textPrice = ((TextBox)GetElement("rightcolumn detailsprice"));
//                textPrice.IsReadOnly = true;
//                textPrice.Background = Brushes.Transparent;

//                var textName = ((TextBox)GetElement("rightcolumn detailsname"));
//                textName.IsReadOnly = true;
//                textName.Background = Brushes.Transparent;

//                UpdatePanel();
//                if (ChangeImageModeEnabled)
//                {
//                    _browser.SwitchContent();
//                }

//                EditProductModeEnabled = false;
//                ChangeImageModeEnabled = false;
//                foreach (var item in ProductBrowserItems)
//                {
//                    item.SwitchOpacityMode();
//                }
//                SelectedImage = null;

//                var newButton = (Button)GetElement("new product");
//                var removeButton = (Button)GetElement("remove");
//                var buttonParent = (StackPanel)GetElement("admin buttons");
//                var changeButton = (Button)GetElement("change image");
//                try
//                {
//                    buttonParent.Children.Add(changeButton);
//                    buttonParent.Children.Add(removeButton);
//                    buttonParent.Children.Add(newButton);
//                }
//                catch (System.Exception)
//                {
//                }
//            }

//            private void RemoveButton_Click(object sender, RoutedEventArgs e)
//            {
//                if (!ChangeImageModeEnabled && !EditProductModeEnabled)
//                {
//                    var result = MessageBox.Show("Do you want to completly remove this product?", "", MessageBoxButton.YesNo);
//                    if (result == MessageBoxResult.Yes)
//                    {
//                        Store.Products.Remove(SelectedProduct);
//                        Store.SaveRuntimeProductsToCSV();
//                        var productsItem = ProductBrowserItems.Find(x => x._product == SelectedProduct);
//                        var parent = productsItem.Parent;
//                        parent.Children.Remove(productsItem.ItemGrid);
//                        ProductBrowserItems.Remove(productsItem);
//                        _rightColumnContentRoot.Visibility = Visibility.Hidden;
//                        SelectedProduct = null;
//                    }
//                }
//            }

//            private void NewProductButton_Click(object sender, RoutedEventArgs e)
//            {
//                if (!ChangeImageModeEnabled && !EditProductModeEnabled)
//                {
//                    var newProduct = new Product("Title...", "uri missing", 0, "Enter your text...");
//                    var newBrowserItem = new BrowserItem(_browser.BrowserWrapPanel);
//                    newBrowserItem.LoadProductBrowserItem(newProduct);
//                    SelectedProduct = newProduct;
//                    Store.Products.Add(newProduct);
//                    SelectedImage = null;
//                    detailsPanel.UpdatePanel();
//                }
//            }

//            private void ChangeImageButton_Click(object sender, RoutedEventArgs e)
//            {
//                _browser.SwitchContent();
//            }

//            private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
//            {
//                var textPrice = ((TextBox)GetElement("rightcolumn detailsprice"));
//                string price = textPrice.Text;
//                decimal decPrice;
//                if (price.Contains(','))
//                {
//                    price = textPrice.Text.Replace(',', '.');
//                    textPrice.Text = price;
//                }
//                if (decimal.TryParse(price, out decPrice))
//                {
//                    //Disables editable fields
//                    var textbox = ((TextBox)GetElement("rightcolumn detailsdescription"));
//                    textbox.IsReadOnly = true;
//                    textbox.Background = Brushes.Transparent;

//                    textPrice.IsReadOnly = true;
//                    textPrice.Background = Brushes.Transparent;

//                    var textName = ((TextBox)GetElement("rightcolumn detailsname"));
//                    textName.IsReadOnly = true;
//                    textName.Background = Brushes.Transparent;

//                    //updates name, description, image soon prize
//                    SelectedProduct.Description = textbox.Text;
//                    SelectedProduct.Name = textName.Text;
//                    SelectedProduct.Price = decimal.Parse(textPrice.Text);

//                    if (SelectedImage != null)
//                    {
//                        SelectedProduct.Uri = SelectedImage.Source.ToString().Split('/')[^1];
//                    }
//                    Store.SaveRuntimeProductsToCSV();

//                    var browserItem = ProductBrowserItems.Find(x => x.ItemGrid.Tag == SelectedProduct); //Gets senders browserItem

//                    browserItem.RefreshProductContent();
//                    EditProductModeEnabled = false;
//                    ChangeImageModeEnabled = false;
//                    foreach (var item in ProductBrowserItems)
//                    {
//                        item.SwitchOpacityMode();
//                    }
//                    var buttonParent = (StackPanel)GetElement("admin buttons");
//                    var newButton = (Button)GetElement("new product");
//                    var removeButton = (Button)GetElement("remove");
//                    var changeButton = (Button)GetElement("change image");
//                    buttonParent.Children.Add(changeButton);
//                    buttonParent.Children.Add(removeButton);
//                    buttonParent.Children.Add(newButton);
//                }
//                else
//                {
//                    MessageBox.Show("Try entering a digit as price!");
//                }
//            }

//            private void EditButton_Click(object sender, RoutedEventArgs e)
//            {
//                var newButton = (Button)GetElement("new product");

//                var removeButton = (Button)GetElement("remove");
//                var changeButton = (Button)GetElement("change image");
//                var buttonParent = (StackPanel)GetElement("admin buttons");
//                buttonParent.Children.Remove(changeButton);
//                buttonParent.Children.Remove(removeButton);
//                buttonParent.Children.Remove(newButton);

//                EditProductModeEnabled = true;

//                var textbox = ((TextBox)GetElement("rightcolumn detailsdescription"));
//                textbox.IsReadOnly = false;
//                textbox.Background = Brushes.White;

//                var textPrice = ((TextBox)GetElement("rightcolumn detailsprice"));
//                textPrice.IsReadOnly = false;
//                textPrice.Background = Brushes.White;

//                var textName = ((TextBox)GetElement("rightcolumn detailsname"));
//                textName.IsReadOnly = false;
//                textName.Background = Brushes.White;

//                foreach (var item in ProductBrowserItems)
//                {
//                    item.SwitchOpacityMode();
//                }
//            }

//            public void UpdatePanel()
//            {
//                var product = SelectedProduct;
//                var image = ((Image)GetElement("rightcolumn detailsimage"));
//                image.Source = Helpers.CreateBitmapImageFromUriString(product.Uri);
//                _rightColumn_DetailsName.Text = product.Name;
//                ((TextBox)GetElement("rightcolumn detailsprice")).Text = product.Price.ToString();

//                var textbox = ((TextBox)GetElement("rightcolumn detailsdescription"));

//                textbox.MaxWidth = ((ScrollViewer)textbox.Parent).ActualWidth;
//                textbox.Text = product.Description;
//                _rightColumnContentRoot.Visibility = Visibility.Visible;
//            }
//        }
//    }

//}
