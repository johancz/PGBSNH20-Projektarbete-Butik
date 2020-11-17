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
    public abstract class CommonFramework
    {
        public static DetailsPanel detailsPanel;
        public static Browser _browser;
        public static List<BrowserItem> ProductBrowserItems = new List<BrowserItem>();
        public static List<BrowserItem> ImageBrowserItems = new List<BrowserItem>();
        public static List<FrameworkElement> Elements = new List<FrameworkElement>();
        public static bool AddImage = false;
        public static Image? SelectedImage = new Image();
        public static Product SelectedProduct;
        public static object GetElement(string _tag)
        {
            try
            {
                return Elements.Find(x => (string)(x.Tag) == _tag);
            }
            catch (System.Exception ex)
            {
                return ex;
            }
        }    
    }   

}
