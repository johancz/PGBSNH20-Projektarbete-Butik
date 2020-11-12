#define DEBUG_SET_BACKGROUND_COLOR

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StoreCommon;
using StoreUser;

namespace StoreAdmin
{
    public partial class MainWindow : Window
    {
        private TabControl _adminView;
        private Canvas _userView;

        public TabControl MainTabControl;

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            // Window options
            Title = ".... Store (admin mode)";
            Width = SystemParameters.WorkArea.Width >= 1000 ? SystemParameters.WorkArea.Width - 200 : 800;
            Height = SystemParameters.WorkArea.Height >= 800 ? SystemParameters.WorkArea.Height - 200 : 600;
            MinWidth = 400;
            MinHeight = 300;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SizeChanged += MainWindow_SizeChanged;
            KeyUp += MainWindow_KeyUp;

            Canvas rootCanvas = new Canvas(); // TODO(johancz): can be removed if we don't implement animations? use MainTabControl instead?
            MainTabControl = new TabControl();

            // Load all data; products, saved shopping carts, discount codes.
            Store.Init();

            // Get the "UserView" from StoreUser, essentially the root-control of the StoreUser-program.
            _userView = UserView.Create();

            _adminView = new TabControl();
            {
                // TODO(johancz): create Content for the tabs

                // Create "Admin Mode" Tabs
                var adminModeTab_manageProducts = new TabItem { Header = "Manage Products" };
                var adminModeTab_manageDiscountCodes = new TabItem { Header = "Manage Discount Codes" };
                _adminView.Items.Add(adminModeTab_manageProducts);
                _adminView.Items.Add(adminModeTab_manageDiscountCodes);
            }

#if DEBUG_SET_BACKGROUND_COLOR
            MainTabControl.Background = Brushes.LightBlue; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif

#if DEBUG_SET_BACKGROUND_COLOR
            _userView.Background = Brushes.Salmon; // TODO(johancz): Only for Mark I debugging, remove before RELEASE.
#endif

            // Create Tabs
            var userViewTab = new TabItem {  Header = "Store (User mode)", Content = _userView };
            var adminViewTab = new TabItem { Header = "Store (Admin mode)", Content = _adminView };
            MainTabControl.Items.Add(userViewTab);
            MainTabControl.Items.Add(adminViewTab);
            MainTabControl.SelectedIndex = 1;

            // Add the main TabControl to the Canvas
            rootCanvas.Children.Add(MainTabControl);

            // Set rootCanvas as the Content of the MainWindow
            Content = rootCanvas;
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

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((Grid)_userView.Children[0]).Height = _userView.Height = ActualHeight;
            ((Grid)_userView.Children[0]).Width = _userView.Width = ActualWidth;
        }
    }
}
