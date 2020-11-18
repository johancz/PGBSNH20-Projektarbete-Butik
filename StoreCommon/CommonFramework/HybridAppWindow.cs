using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreCommon
{
    public class HybridAppWindow : CommonFramework
    {
        public HybridAppWindow(Window mainWindow, string title, Brush brush)
        {
            mainWindow.Title = title;
            mainWindow.Width = SystemParameters.WorkArea.Width >= 1000 ? SystemParameters.WorkArea.Width - 200 : 800;
            mainWindow.Height = SystemParameters.WorkArea.Height >= 800 ? SystemParameters.WorkArea.Height - 200 : 600;
            mainWindow.MinWidth = 800;
            mainWindow.MinHeight = 600;
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    var windowTabControl = new TabControl { Background = brush };
                        mainWindow.Content = windowTabControl;
            
            MainWindow = mainWindow;
                WindowTabControl = windowTabControl;

            MainWindow.KeyUp += MainWindow_KeyUp;
        }
        public void AddDefaultAdminPage(string header, Brush brush)
        {            
            var editTabItem = new TabItem { Header = header };
            WindowTabControl.Items.Add(editTabItem);

                var editTabGrid = new Grid { ShowGridLines = true, Background = brush };
                editTabGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                editTabGrid.ColumnDefinitions.Add(new ColumnDefinition());
                editTabGrid.ColumnDefinitions.Add(new ColumnDefinition());
                editTabItem.Content = editTabGrid;                    

            EditTabItem = editTabItem;
                EditTabGrid = editTabGrid;
                    
        }
        public void AddBrowser()
        {
            var browserRootScrollViewer = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            Grid.SetColumn(browserRootScrollViewer, 0);
            EditTabGrid.Children.Add(browserRootScrollViewer);

                var browserProductsPanel = new WrapPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top
                };
                browserRootScrollViewer.Content = browserProductsPanel;

            BrowserRootScrollViewer = browserRootScrollViewer;
                BrowserProductsPanel = browserProductsPanel;

            browserRootScrollViewer.SizeChanged += Browser.ScrollViewer_SizeChanged;
        }
        public void AddDetailsPanel(Brush brush)
        {
            var detailsPanelRootGrid = new Grid { ShowGridLines = true, Background = brush };
            detailsPanelRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            detailsPanelRootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            BrowserRootScrollViewer.Content = detailsPanelRootGrid;

                var detailsPanelImage = new Image();
                detailsPanelRootGrid.Children.Add(DetailsPanelImage);

                var detailsTextAndButtonGrid = new Grid { ShowGridLines = true };
                detailsTextAndButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                detailsTextAndButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Grid.SetRow(detailsTextAndButtonGrid, 1);
                detailsPanelRootGrid.Children.Add(detailsTextAndButtonGrid);

                    var productTextBoxesGrid = new Grid { ShowGridLines = true };
                    productTextBoxesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    productTextBoxesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    detailsTextAndButtonGrid.Children.Add(productTextBoxesGrid);

            

            DetailsPanelRootGrid = detailsPanelRootGrid;
                DetailsPanelImage = detailsPanelImage;
                DetailsTextAndButtonGrid = detailsTextAndButtonGrid;
                    ProductTextBoxesGrid = productTextBoxesGrid;
        }
        public void AddEditableTextBoxes()
        {

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
