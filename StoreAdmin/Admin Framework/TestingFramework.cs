using StoreCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreAdmin
{
    class TestingFramework : AdminFramework
    {
        private bool GridLinesVisible = false;
        public TestingFramework()
        {
            var devTab = new TabItem { Header = "Visual Tree", FontSize=12 };
            WindowTabControl.Items.Add(devTab);

            var devButtonPanel = new StackPanel { HorizontalAlignment = HorizontalAlignment.Left, Orientation = Orientation.Horizontal };
            WindowCanvas.Children.Add(devButtonPanel);
            Canvas.SetLeft(devButtonPanel, 500);      

            var appFolderButton = AppWindow.CreateButton("App Folder");     
            var showGridButton = AppWindow.CreateButton("Grid Lines");
            var reloadTreeButton = AppWindow.CreateButton("Reload Tree");
            devButtonPanel.Children.Add(appFolderButton);
            devButtonPanel.Children.Add(showGridButton);
            devButtonPanel.Children.Add(reloadTreeButton);


            var testLabel = new Label { Background = Brushes.White};
            WindowCanvas.Children.Add(testLabel);            
           
            var treeView = new TreeView { IsEnabled = true};
            devTab.Content = treeView;

            DevLabel = testLabel;
                DevTab = devTab;
                    DevTreeView = treeView;


            foreach (var grid in ProductGrids)
            {
                Image image = (Image)grid.Children[0];
                image.MouseEnter += Image_MouseEnter;
            }
                DetailsPanelImage.MouseEnter += Image_MouseEnter;

            ActualWindow.Title = ActualWindow.Title + ": Developer Mode";

            appFolderButton.Click += Button_Click;
            showGridButton.Click += ShowGridButton_Click;
            reloadTreeButton.Click += ReloadTreeButton_Click;
            ActualWindow.SizeChanged += MainWindow_SizeChanged;
            ActualWindow.MouseMove += MainWindow_MouseMove;
        }

        private void ReloadTreeButton_Click(object sender, RoutedEventArgs e)
        {
            //ShowWPFVisualTree(EditPageGrid);
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            var image = ((Image)sender);
            image.ToolTip = image.Source;
        }

        private void ShowGridButton_Click(object sender, RoutedEventArgs e)
        {
             
            GridLinesVisible = !GridLinesVisible;
            var grids = new List<Grid> { DetailsPanelRootGrid, DetailsTextAndButtonGrid, DetailsTitleAndDescriptionGrid, Views.ManageDiscountCodesView._grid }; //EditPageGrid
            grids.ForEach(x => x.ShowGridLines = GridLinesVisible);
            ProductGrids.ForEach(x => x.ShowGridLines = GridLinesVisible);

        }

        public void AddElementsAtVisualTree(DependencyObject element, TreeViewItem previtem)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = element.GetType().Name;
            item.IsExpanded = false;
            if (previtem == null)
            {
                item.Header = "Administrator EditPage";
                DevTreeView.Items.Add(item);
            }
            else
            {
                previtem.Items.Add(item);
            }
            int totalElementcount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < totalElementcount; i++)
            {
                AddElementsAtVisualTree(VisualTreeHelper.GetChild(element, i), item);
            }
        }
        public void ShowWPFVisualTree(DependencyObject element)
        {
            DevTreeView.Items.Clear();
            AddElementsAtVisualTree(element, null);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Process.Start("explorer.exe", DataManager.RootFolderPath);
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            var element = (FrameworkElement)Mouse.DirectlyOver;

            DevLabel.Content = $"Width: {Math.Round(element.ActualWidth, 1)}, Height: {Math.Round(element.ActualHeight, 1)}, Element:  {element.GetType()} parent: {element.Parent}";
    
        }
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetTop(DevLabel, ActualWindow.ActualHeight-80);
        }

    }
}
