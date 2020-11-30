using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreCommon
{
    public static class ImageCreation
    {
        public static BitmapImage CreateBitmapImageFromUriString(string uriString)
        {
            try
            {
                var uri = new Uri(Path.Combine(DataManager.ImageFolderPath, uriString), UriKind.Absolute);
                var bitMapImage = new BitmapImage(uri);

                return bitMapImage;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Image CreateNewImage(string uriString = null, int? height = null)
        {
            Image image = new Image();

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

            return image;
        }
    }
}
