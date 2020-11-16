using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreCommon
{
    public static class Helpers
    {
        public static string StoreDataPath { get; set; } = Path.Combine(Environment.CurrentDirectory, "StoreData");
        public static string StoreDataCsvPath { get; set; } = Path.Combine(Environment.CurrentDirectory, "StoreData", ".CSVs");
        public static string StoreDataImagesPath { get; set; } = Path.Combine(Environment.CurrentDirectory, "StoreData", "Images");
        // TODO(johancz): Temporary output path, this should use the system's temp-folder.
        public static string StoreDataTemporaryOutputPath { get; set; } = Path.Combine(Environment.CurrentDirectory, "StoreData", "TemporaryOutput");
        public static void BackgroundImage(Control element, string uriRelative)
        {
            ImageSource source = new BitmapImage(new Uri(uriRelative, UriKind.Relative));
            ImageBrush imageBrush = new ImageBrush { ImageSource = source };
            ImageBrush imageBrushTitle = new ImageBrush { ImageSource = source };
            element.Background = imageBrush;
        }
        public static BitmapImage CreateBitmapImageFromUriString(string uriString)
        {
            try
            {
                var uri = new Uri(Path.Combine(AppFolder.ImagesPath, uriString), UriKind.Absolute);
                var bitMapImage = new BitmapImage(uri);

                return bitMapImage;
            }
            catch (Exception)
            {
                // TODO(johancz): exception-handling
                return null;
            }
        }
        public static Image CreateNewImage(string uriString = null, int? height = null, string tooltipText = null, bool imageInTooltip = false)
        {
            Image image = new Image();

            try
            {
                if (uriString != null)
                {
                    image.Source = CreateBitmapImageFromUriString(uriString);
                }
                image.HorizontalAlignment = HorizontalAlignment.Center;
                image.VerticalAlignment = VerticalAlignment.Center;
                image.Margin = new Thickness(5);
                image.Stretch = Stretch.Uniform;

                if (height != null)
                {
                    image.Height = height.Value;
                }

                if (tooltipText != null || imageInTooltip != false)
                {
                    var tooltipStackpanel = new StackPanel { Orientation = Orientation.Vertical };
                    image.ToolTip = tooltipStackpanel;

                    if (tooltipText != null)
                    {
                        tooltipStackpanel.Children.Add(new Label
                        {
                            Content = tooltipText
                        });
                    }

                    if (imageInTooltip != false)
                    {
                        // TODO(johancz): Use the available Helper for creating images.
                        tooltipStackpanel.Children.Add(new Image
                        {
                            Source = CreateBitmapImageFromUriString(uriString),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(5),
                            Stretch = Stretch.Uniform,
                        });
                    }
                }
            }
            catch (Exception)
            {
                // TODO(johancz): exception-handling
            }

            return image;
        }
    }
}
