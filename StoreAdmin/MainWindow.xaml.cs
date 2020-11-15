﻿using StoreCommon;
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
        private TabControl _adminView;
        private Canvas _userView;
        private Canvas _manageProducts;
        private WrapPanel _wrapPanel;
        private Grid _rootGrid;

        public TabControl MainTabControl;

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Title = ".... Store (admin mode)";
            Width = SystemParameters.WorkArea.Width >= 1000 ? SystemParameters.WorkArea.Width - 200 : 800;
            Height = SystemParameters.WorkArea.Height >= 800 ? SystemParameters.WorkArea.Height - 200 : 600;
            MinWidth = 800;
            MinHeight = 600;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SizeChanged += MainWindow_SizeChanged;
            KeyUp += MainWindow_KeyUp;

            Canvas rootCanvas = new Canvas();
            MainTabControl = new TabControl();

            Store.Init();

            _userView = UserView.Create();
            _adminView = new TabControl();
            //_manageProducts = ManageProductsView.Create();
            _rootGrid = ManageProductsView.CreateGrid();

            var adminModeTab_manageProducts = new TabItem { Header = "Manage Products"};
            var adminModeTab_manageDiscountCodes = new TabItem { Header = "Manage Discount Codes" };
            _adminView.Items.Add(adminModeTab_manageProducts);
            _adminView.Items.Add(adminModeTab_manageDiscountCodes);

            MainTabControl.Background = Brushes.LightBlue;
            _userView.Background = Brushes.Salmon;

            var userViewTab = new TabItem { Header = "Store (User mode)", Content = _userView };
            var adminViewTab = new TabItem { Header = "Store (Admin mode)", Content = _adminView };
            adminModeTab_manageProducts.Content = _rootGrid;

            MainTabControl.Items.Add(userViewTab);
            MainTabControl.Items.Add(adminViewTab);

            MainTabControl.SelectedIndex = 1;

            rootCanvas.Children.Add(MainTabControl);
            Content = rootCanvas;
        }

        ////////////////////////////////////////////////////////
        //////////////////// Event Handling ////////////////////
        ////////////////////////////////////////////////////////

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
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
