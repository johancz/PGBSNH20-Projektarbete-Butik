using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreCommon
{
    public class BrowserItem : CommonFramework
    {
        public WrapPanel Parent;
        public Grid ItemGrid;
        public Image ImageThumbnail;
        public Product? _product = null;
        public BrowserItem(WrapPanel parent)
        {
            Parent = parent;

            var itemGrid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Top,
                Width = ProductItem_LayoutSettings.gridItemWidth,
                Height = ProductItem_LayoutSettings.gridItemHeight,
                Margin = new Thickness(5),
                Background = Brushes.LightGray,
            };

            ItemGrid = itemGrid;
        }
        public void LoadImageBrowserItem(string filePath)
        {
            var productThumbnail = Helpers.CreateNewImage(filePath, ProductItem_LayoutSettings.gridItemImageHeight);
            ImageThumbnail = productThumbnail;
            productThumbnail.Stretch = Stretch.UniformToFill;
            productThumbnail.VerticalAlignment = VerticalAlignment.Center;
            productThumbnail.HorizontalAlignment = HorizontalAlignment.Center;
            ItemGrid.Children.Add(productThumbnail);
            ImageBrowserItems.Add(this);

            ItemGrid.MouseUp += ImageItemGrid_MouseUp;
        }

        public void LoadProductBrowserItem(Product product)
        {
            _product = product;
            Parent.Children.Add(ItemGrid);
            ItemGrid.Tag = product;
            var productThumbnail = Helpers.CreateNewImage(product.Uri, ProductItem_LayoutSettings.gridItemImageHeight);
            ImageThumbnail = productThumbnail;
            productThumbnail.Stretch = Stretch.UniformToFill;
            productThumbnail.VerticalAlignment = VerticalAlignment.Center;
            productThumbnail.HorizontalAlignment = HorizontalAlignment.Center;
            ItemGrid.Children.Add(productThumbnail);
            Grid.SetColumnSpan(ImageThumbnail, 2);

            var tooltip = new ToolTip
            {

                Placement = PlacementMode.Mouse,
                MaxWidth = 800,
                Content = new TextBlock
                {
                    Text = $"{product.Name}\n{product.Description}\n",
                    TextWrapping = TextWrapping.Wrap,
                }
            };
            ItemGrid.ToolTip = tooltip;

            ItemGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ItemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            ItemGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            ItemGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            var nameLabel = new Label
            {
                Content = product.Name,
                FontSize = 14,
            };
            Grid.SetColumn(nameLabel, 0);
            Grid.SetRow(nameLabel, 1);
            ItemGrid.Children.Add(nameLabel);

            var priceLabel = new Label
            {
                Content = $"{product.Price} kr",
            };
            Grid.SetColumn(priceLabel, 1);
            Grid.SetRow(priceLabel, 1);
            ItemGrid.Children.Add(priceLabel);

            ProductBrowserItems.Add(this);
            ItemGrid.MouseUp += ProductItemGrid_MouseUp;
        }
        public void SwitchOpacityMode()
        {
            if (EditProductModeEnabled || ChangeImageModeEnabled)
            {
                ImageThumbnail.Opacity = 0.7;
            }
            else
            {
                ImageThumbnail.Opacity = 1.0;
            }
        }
        private void ImageItemGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var itemGrid = (Grid)sender;
            var itemImage = (Image)(itemGrid.Children[0]);
            SelectedImage = itemImage;
            var source = itemImage.Source;
            var displayImage = DetailsPanelImage;
            displayImage.Source = source;
        }

        public void ProductItemGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!EditProductModeEnabled)
            {
                SelectedProduct = (Product)(ItemGrid.Tag);
                detailsPanel.UpdatePanel();
                SelectedImage = null;
            }
        }
        public void RefreshProductContent()
        {
            var product = ((Product)(ItemGrid.Tag));
            var nameLabel = (Label)(ItemGrid.Children[1]);
            
            if (SelectedImage != null)
            {
                ImageThumbnail.Source = SelectedImage.Source;
            }
            nameLabel.Content = product.Name;
            var priceLabel = (Label)(ItemGrid.Children[2]);
            priceLabel.Content = $"{product.Price} kr";
        }

        internal struct ProductItem_LayoutSettings
        {
            internal const double gridItemWidth = 200;
            internal const double gridItemHeight = 200;
            internal const int gridItemImageHeight = 175;
        }
    }
}