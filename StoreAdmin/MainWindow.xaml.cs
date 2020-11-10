using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using StoreUser;

namespace StoreAdmin
{
    public partial class MainWindow : Window
    {
        private Canvas RootCanvas;

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
            Width = 800;
            Height = 600;
            MinWidth = 400;
            MinHeight = 300;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            RootCanvas = new Canvas();
            MainTabControl = new TabControl();

            var userMode = UserView.Create();

            var userModeTabControl = new TabControl();
            // "User Mode" TabItem.Content
            {
                // TODO(johancz) add reference to Butik-User project create the content for the userModeTab with that?

                // Create "User Mode" Tabs
                var userModeTab_BrowseStore = new TabItem { Header = "Browse Store" };
                var userModeTab_ShoppingCart = new TabItem { Header = "Shopping Cart" };
                userModeTabControl.Items.Add(userModeTab_BrowseStore);
                userModeTabControl.Items.Add(userModeTab_ShoppingCart);
            }

            var adminModeTabControl = new TabControl();
            // "User Mode" TabItem.Content
            {
                // TODO(johancz): create Content for the tabs

                // Create "Admin Mode" Tabs
                var adminModeTab_manageProducts = new TabItem { Header = "Manage Products" };
                var adminModeTab_manageDiscountCodes = new TabItem { Header = "Manage Discount Codes" };
                adminModeTabControl.Items.Add(adminModeTab_manageProducts);
                adminModeTabControl.Items.Add(adminModeTab_manageDiscountCodes);
            }

            // Create Tabs
            var userModeTab = new TabItem {  Header = "Store (User mode)", Content = userModeTabControl };
            var adminModeTab = new TabItem {  Header = "Store (Admin mode", Content = adminModeTabControl };
            MainTabControl.Items.Add(userModeTab);
            MainTabControl.Items.Add(adminModeTab);

            // Add the main TabControl to the Canvas
            RootCanvas.Children.Add(MainTabControl);

            // Set RootCanvas as the Content of the MainWindow
            Content = RootCanvas;
        }
    }
}
