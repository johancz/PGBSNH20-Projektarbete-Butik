using StoreCommon;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace StoreUser
{
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
            Title = ".... Store (user mode)"; // TODO(johancz): Change before RELEASE
            Width = SystemParameters.WorkArea.Width >= 1000 ? SystemParameters.WorkArea.Width - 200 : 800;
            Height = SystemParameters.WorkArea.Height >= 800 ? SystemParameters.WorkArea.Height - 200 : 600;
            MinWidth = 400;
            MinHeight = 300;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            KeyUp += MainWindow_KeyUp;

            // Load all data; products, saved shopping carts, discount codes.
            Store.Init();
            Content = UserView.Create();
        }

        ////////////////////////////////////////////////////////
        //////////////////// Event Handling ////////////////////
        ////////////////////////////////////////////////////////

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