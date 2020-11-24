using StoreCommon;
using StoreUser.Views;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace StoreUser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       
    }
    public static class xaml
    {
        private static DispatcherTimer _timer;
        private static int _timerCounter = 0;
        private static ImageSource _switchImageSource;
        private static ImageSource _originalImageSource;
        public static void ReceiptWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // You found an easter egg, congrats!
            var image = DetailsPanelView.DetailsImage;
            _originalImageSource = image.Source;
            var switchImageSource = ImageCreation.CreateBitmapImageFromUriString(Path.Combine(Environment.CurrentDirectory, "StoreData", "Image Helpers", "NewProductImage.jpeg"));
            _switchImageSource = switchImageSource;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += ReceiptWindow_TimerTick;
            timer.Start();
            _timer = timer;
        }
        private static void ReceiptWindow_TimerTick(object sender, EventArgs e)
        {
            _timerCounter++;
            if (_timerCounter == 20)
            {
                DetailsPanelView.DetailsImage.Source = _switchImageSource;
            }
            if (_timerCounter > 22)
            {
                DetailsPanelView.DetailsImage.Source = _originalImageSource;
                _timer.Stop();
                _timerCounter = 0;
            }
        }
    }
}
