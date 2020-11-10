﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreClassLibrary
{
    public static class Helpers
    {
        public static string ImageFolderPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\Images\\";
        public static BitmapImage CreateBitmapImageFromUriString(string uriString)
        {
            try
            {
                var uri = new Uri(ImageFolderPath + uriString, UriKind.Absolute);
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

    // TODO(johancz): Move to a test project?
//#if DEBUG
//    [TestClass]
//    public class HelpersTests
//    {
//        [TestMethod]
//        // MethodBeingTested_Input_Output
//        public void CreateNewImage_ValidUriString_Image()
//        {
//            throw new NotImplementedException();
//        }

//        [TestMethod]
//        // MethodBeingTested_Input_Output
//        public void CreateNewImage_InValidUriString_null()
//        {
//            throw new NotImplementedException();
//        }

//        [TestMethod]
//        // MethodBeingTested_Input_Output
//        public void CreateBitmapImageFromUriString_ValidUriString_BitmapImage()
//        {
//            throw new NotImplementedException();
//        }

//        [TestMethod]
//        // MethodBeingTested_Input_Output
//        public void CreateBitmapImageFromUriString_InvalidUriString_null()
//        {
//            throw new NotImplementedException();
//        }
//    }
//#endif
}