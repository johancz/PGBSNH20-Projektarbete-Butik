using StoreCommon;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreUser.Views
{
    public static class BrowseProductsTabView
    {
        private static ScrollViewer _root;

        private struct ProductItem_LayoutSettings
        {
            // These need to have the 'internal' modifier so that they can be accessed. The 'internal' modifier makes the member accessible from the entire assembly (e.g. '.exe', '.dll'). In this case however, because they live in a private struct, they are accessible from wherever the private struct is accessible from (i.e. the UserView-class).
            internal static double gridItemWidth = 200;
            internal static double gridItemHeight = 200;
            internal static int gridItemImageHeight = 175;
        }

        public static ScrollViewer Init()
        {
            CreateGUI();
            return _root;
        }

        // "Browse Store" Tab
        private static void CreateGUI()
        {
            _root = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            var productsPanel = new WrapPanel { HorizontalAlignment = HorizontalAlignment.Center };

            foreach (Product product in Store.Products)
            {
                var productItem = CreateProductItem(product);

                if (productItem != null)
                {
                    productsPanel.Children.Add(productItem);
                }
            }

            // Add the Products-WrapPanel to the ScrollViewer
            _root.Content = productsPanel;
        }

        private static Grid CreateProductItem(Product product)
        {
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
            var productItem = new Grid
            {
                Tag = product,
                VerticalAlignment = VerticalAlignment.Top,
                Width = ProductItem_LayoutSettings.gridItemWidth,
                Height = ProductItem_LayoutSettings.gridItemHeight,
                Margin = new Thickness(5),
                ToolTip = tooltip,
                Background = Brushes.LightGray,
            };
            productItem.MouseUp += ProductItem_MouseUp;
            // This is required for the tooltip to appear at 'PlacementMode.Mouse' when hovering over another "productItem".
            // Otherwise the tooltip will "stick" to the old (this) "productItem" if the mouse is moved to the other "productItem" too quickly.
            productItem.MouseLeave += (sender, e) =>
            {
                tooltip.IsOpen = false;
            };

            productItem.ColumnDefinitions.Add(new ColumnDefinition());
            productItem.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            productItem.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            productItem.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            // productItem children:
            {
                var productThumbnail = ImageCreation.CreateNewImage(product.Uri, ProductItem_LayoutSettings.gridItemImageHeight);
                productThumbnail.Stretch = Stretch.UniformToFill;
                productThumbnail.VerticalAlignment = VerticalAlignment.Center;
                productThumbnail.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetColumnSpan(productThumbnail, 2);
                productItem.Children.Add(productThumbnail);

                var nameLabel = new Label
                {
                    Content = product.Name,
                    FontSize = 14,
                };
                Grid.SetColumn(nameLabel, 0);
                Grid.SetRow(nameLabel, 1);
                productItem.Children.Add(nameLabel);

                var priceLabel = new Label
                {
                    Content = $"{product.Price} kr",
                };
                Grid.SetColumn(priceLabel, 1);
                Grid.SetRow(priceLabel, 1);
                productItem.Children.Add(priceLabel);
            }

            return productItem;
        }

        /******************************************************/
        /******************* Event Handling *******************/
        /******************************************************/

        public static void ProductItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var product = (Product)((Grid)sender).Tag;
            if (product != null)
            {
                UserView.SelectedProduct = product;
                DetailsPanelView.UpdateGUI();
            }
        }
    }
}
