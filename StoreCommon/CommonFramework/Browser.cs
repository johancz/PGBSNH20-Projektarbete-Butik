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
    public class Browser : CommonFramework
    {
        public Grid Parent;
        public ScrollViewer ThisScrollViewer;

        public Browser(Grid parent)
        {
            Parent = parent;
            _browser = this;

            var scrollViewer = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            ThisScrollViewer = scrollViewer;

            parent.Children.Add(scrollViewer);
            Grid.SetColumn(scrollViewer, 0);

            var productParentPanel = new WrapPanel
            {
                Tag = "product panel",
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = Brushes.LightBlue,
                VerticalAlignment = VerticalAlignment.Top
            };
            ProductParentPanel = productParentPanel;

            scrollViewer.Content = productParentPanel;
            ProductParentPanel = productParentPanel;

            scrollViewer.SizeChanged += ScrollViewer_SizeChanged;
        }
        public void LoadBrowserImages()
        {
            foreach (var filePath in Store.ImageItemFilePaths)
            {
                var newProductItem = new BrowserItem(ProductParentPanel);
                newProductItem.LoadImageBrowserItem(filePath);
            }
        }
        public void LoadBrowserItems()
        {
            foreach (var product in Store.Products)
            {
                var newProductItem = new BrowserItem(ProductParentPanel);
                newProductItem.LoadProductBrowserItem(product);
            }
        }
        public void SwitchContent()
        {
            if (!ChangeImageModeEnabled)
            {
                ImageModeButtons();
                ChangeImageModeEnabled = true;
                EditProductModeEnabled = true;
                foreach (var productItem in ProductBrowserItems)
                {
                    ProductParentPanel.Children.Remove(productItem.ItemGrid);
                    productItem.SwitchOpacityMode();

                }
                foreach (var imageItem in ImageBrowserItems)
                {
                    ProductParentPanel.Children.Add(imageItem.ItemGrid);
                }
               
                ChangeImageButton.Width = ChangeImageButton.ActualWidth;
                ChangeImageButton.Content = new Label { Content = "Select Image", HorizontalAlignment = HorizontalAlignment.Left };
            }
            else
            {
                ChangeImageModeEnabled = false;

                foreach (var productItem in ProductBrowserItems)
                {
                    ProductParentPanel.Children.Add(productItem.ItemGrid);
                }
                foreach (var imageItem in ImageBrowserItems)
                {
                    ProductParentPanel.Children.Remove(imageItem.ItemGrid);
                }
                ChangeImageButton.Width = ChangeImageButton.ActualWidth;
                ChangeImageButton.Content = new Label { Content = "Change Image", HorizontalAlignment = HorizontalAlignment.Left };
            }
        }
        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ProductParentPanel.Width = ThisScrollViewer.ActualWidth;
        }
    }
}
