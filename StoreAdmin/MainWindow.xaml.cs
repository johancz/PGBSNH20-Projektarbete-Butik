using StoreCommon;
using StoreUser;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreAdmin
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Store.Init();
            var AdminApp = new HybridAppWindow(this);
            AdminApp.LoadGUI();
            var appEvents = new AdminAppEvents();
            appEvents.Init();            
        }
       
    }
}
