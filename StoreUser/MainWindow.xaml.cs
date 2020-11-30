using StoreCommon;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace StoreUser
{
    //There is an easter egg somewhere in user mode.
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            // Window options
            Title = DataManager.ProjectName + " (user mode)";
            Width = SystemParameters.WorkArea.Width >= 1000 ? SystemParameters.WorkArea.Width - 200 : 800;
            Height = SystemParameters.WorkArea.Height >= 800 ? SystemParameters.WorkArea.Height - 200 : 600;
            MinWidth = 800;
            MinHeight = 600;
            Uri iconUri = new Uri(Path.Combine(Environment.CurrentDirectory, "StoreData", "Image Helpers", "soap.ico"), UriKind.Absolute);
            this.Icon = BitmapFrame.Create(iconUri);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            KeyUp += MainWindow_KeyUp;

            // Load all data; products, saved shopping carts, discount codes.
            Store.Init();
            Content = UserView.Create();
        }
        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            // Close Window with ESC-key.
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }
    }
}