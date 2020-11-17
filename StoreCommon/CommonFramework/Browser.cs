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
        public WrapPanel BrowserWrapPanel;
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

            var productPanel = new WrapPanel
            {
                Tag = "product panel",
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = Brushes.LightBlue,
                VerticalAlignment = VerticalAlignment.Top
            };
            Elements.Add(productPanel);

            scrollViewer.Content = productPanel;
            BrowserWrapPanel = productPanel;
            scrollViewer.SizeChanged += ScrollViewer_SizeChanged;
        }
        public void LoadBrowserImages()
        {
            foreach (var filePath in Store.ImageItemFilePaths)
            {
                var newProductItem = new BrowserItem(BrowserWrapPanel);
                newProductItem.LoadImageBrowserItem(filePath);
            }
        }
        public void LoadBrowserItems()
        {
            foreach (var product in Store.Products)
            {
                var newProductItem = new BrowserItem(BrowserWrapPanel);
                newProductItem.LoadProductBrowserItem(product);
            }
        }
        public void SwitchContent()
        {
            var newButton = (Button)GetElement("new product");
            var changeButton = (Button)GetElement("change image");
            var editButton = (Button)GetElement("edit");
            var removeButton = (Button)GetElement("remove");
            var saveButton = (Button)GetElement("save changes");
            var buttonParent = (StackPanel)(changeButton.Parent);
            changeButton.Width = changeButton.ActualWidth;

            if (!AddImage)
            {
                AddImage = true;
                foreach (var productItem in ProductBrowserItems)
                {
                    BrowserWrapPanel.Children.Remove(productItem.ItemGrid);
                }
                foreach (var imageItem in ImageBrowserItems)
                {
                    BrowserWrapPanel.Children.Add(imageItem.ItemGrid);
                }
                changeButton.Content = new Label { Content = "Select Image", HorizontalAlignment = HorizontalAlignment.Left };
                buttonParent.Children.Remove(editButton);
                buttonParent.Children.Remove(removeButton);
                buttonParent.Children.Remove(saveButton);
                buttonParent.Children.Remove(newButton);
            }
            else
            {
                AddImage = false;

                foreach (var productItem in ProductBrowserItems)
                {
                    BrowserWrapPanel.Children.Add(productItem.ItemGrid);
                }
                foreach (var imageItem in ImageBrowserItems)
                {
                    BrowserWrapPanel.Children.Remove(imageItem.ItemGrid);
                }
                changeButton.Content = new Label { Content = "Change Image", HorizontalAlignment = HorizontalAlignment.Left };
                buttonParent.Children.Add(editButton);
                buttonParent.Children.Add(removeButton);
                buttonParent.Children.Add(saveButton);
                buttonParent.Children.Add(newButton);
            }
        }
        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BrowserWrapPanel.Width = ThisScrollViewer.ActualWidth;
        }
    }
}
