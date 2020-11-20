using StoreAdmin.Views;
using StoreCommon;
using StoreUser;
using System.Globalization;
using System.Windows;

namespace StoreAdmin
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            
            InitializeComponent();
            Store.Init();
            var AdminApp = new HybridAppWindow(this);
            AdminApp.CreateAdminGUI();
            var appEvents = new AdminAppEvents();
            appEvents.Init();

            // Edit Discount Codes
            AdminApp.tabControl.Items.Add(ManageDiscountCodesView.Init());
        }
       
    }
}
