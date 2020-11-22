using StoreCommon;
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
            if (MessageBox.Show("Do you want to start in developer mode?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var testingFrameWork = new TestingFramework();
            }
        }
       
    }
}
