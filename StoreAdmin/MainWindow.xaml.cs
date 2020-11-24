using StoreCommon;
using System.Globalization;
using System.Windows;

namespace StoreAdmin
{
    public partial class MainWindow : Window
    {
        //Initiates the store and instanciates the classes that builds the app
        //Abstract Class Admin Framework does per defintion not need to be instanciated but is essential for all code that builds the admin view.
        public MainWindow()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            
            InitializeComponent();

            Store.Init(); //Creates dataobjects from app-files in temp

            var AdminMode = new AdminHybridWindow(this); //Wraps around the MainWindow window.

            AdminMode.CreateAdminGUI();

            var adminModeEvents = new AdminModeEvents();

            adminModeEvents.Init();
        }       
    }
}
