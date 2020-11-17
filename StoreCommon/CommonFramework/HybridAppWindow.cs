using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreCommon
{
    public class HybridAppWindow
    {
        private Window _mainWindow;
        public readonly TabControl tabControl;
        public HybridAppWindow(Window mainWindow, string title, Brush brush)
        {
            _mainWindow = mainWindow;
            WindowSettings(title);
            var _tabcontrol = new TabControl();
            _mainWindow.Content = _tabcontrol;
            tabControl = _tabcontrol;
            _tabcontrol.Background = brush;

            _mainWindow.KeyUp += MainWindow_KeyUp;
        }
        private void WindowSettings(string title)
        {
            _mainWindow.Title = title;
            _mainWindow.Width = SystemParameters.WorkArea.Width >= 1000 ? SystemParameters.WorkArea.Width - 200 : 800;
            _mainWindow.Height = SystemParameters.WorkArea.Height >= 800 ? SystemParameters.WorkArea.Height - 200 : 600;
            _mainWindow.MinWidth = 800;
            _mainWindow.MinHeight = 600;
            _mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
