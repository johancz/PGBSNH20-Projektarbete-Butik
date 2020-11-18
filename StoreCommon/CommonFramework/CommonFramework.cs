using StoreCommon;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreCommon
{
    public abstract class CommonFramework
    {
        
        public static Window MainWindow;
            public static TabControl WindowTabControl;
                public static TabItem EditTabItem;                    
                    public static Grid EditTabGrid;
                        public static ScrollViewer BrowserRootScrollViewer;
                            public static WrapPanel BrowserProductsPanel;
                                public static List<Grid> ImageGrids;
                                public static List<Grid> ProductGrids;
                        public static Grid DetailsPanelRootGrid;
                            public static Image DetailsPanelImage;
                            public static Grid DetailsTextAndButtonGrid;
                                public static Grid ProductTextBoxesGrid;
                                    public static TextBox DetailsPanelName;
                                    public static TextBox DetailsPanelDescription;
                                    public static TextBox DetailsPanelPrice;
                                public static WrapPanel DetailsButtonPanel;
                                    public static Button EditButton;
                                    public static Button RemoveButton;
                                    public static Button ChangeImageButton;
                                    public static Button NewProductButton;
                                    public static Button CancelButton;
                                    public static Button CancelImageButton;
                                    public static Button SaveImageButton;
                                    public static Button SaveChangesButton;

        //State Framework
        public static Product SelectedProduct;
        public static Image? SelectedImage = new Image();
 
        public static WrapPanel ProductParentPanel;
        public static Grid DetailsPanelParentGrid;
        public static StackPanel ButtonParentPanel;
        public static StackPanel NameAndPriceParentPanel; 
        public static DetailsPanel detailsPanel;
        public static Browser _browser;
        public static List<BrowserItem> ProductBrowserItems = new List<BrowserItem>();
        public static List<BrowserItem> ImageBrowserItems = new List<BrowserItem>();
        public static bool ChangeImageModeEnabled = false;
        public static bool EditProductModeEnabled = false;

        public void LoadAllButtons()
        {

            ViewButtons(new List<Button> { NewProductButton, ChangeImageButton, EditButton, RemoveButton, SaveChangesButton, CancelButton, CancelImageButton });
        }
        public void DefaultModeButtons()
        {
            HideButtons(new List<Button> { CancelButton, SaveChangesButton, CancelImageButton });
            ViewButtons(new List<Button> { NewProductButton, ChangeImageButton, EditButton, RemoveButton });
        }
        public void ImageModeButtons()
        {
            HideButtons(new List<Button> { NewProductButton, EditButton, RemoveButton, CancelButton, ChangeImageButton });
            ViewButtons(new List<Button> { SaveImageButton, CancelImageButton });
        }
        public void EditModeButtons()
        {
            HideButtons(new List<Button> { NewProductButton, EditButton, RemoveButton, CancelImageButton, ChangeImageButton });
            ViewButtons(new List<Button> { CancelButton, SaveChangesButton });
        }

        private void ViewButtons(List<Button> buttonsToView)
        {
            foreach (var button in buttonsToView)
            {
                try
                {
                    ButtonParentPanel.Children.Add(button);
                }
                catch (System.Exception)
                {
                }
            }
        }
        private void HideButtons(List<Button> buttonsToHide)
        {
            foreach (var button in buttonsToHide)
            {
                try
                {
                    ButtonParentPanel.Children.Remove(button);
                }
                catch (System.Exception)
                {
                }
            }
        }
        public static void CreateAdminButtons()
        {

            var rightColumn_detailsPanel_AdminButtons = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(5),
                Tag = "admin buttons"
            };
            ButtonParentPanel = rightColumn_detailsPanel_AdminButtons;

            var editButton = new Button
            {
                Tag = "edit",
                Padding = new Thickness(5),
                Content = new Label { Content = "Edit", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            EditButton = editButton;
            editButton.Click += detailsPanel.EditButton_Click;

            var saveChangesButton = new Button
            {
                Tag = "save changes",
                Padding = new Thickness(5),
                Content = new Label { Content = "Save Changes", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            SaveChangesButton = saveChangesButton;
            saveChangesButton.Click += detailsPanel.SaveChangesButton_Click;

            var removeButton = new Button
            {
                Tag = "remove",
                Padding = new Thickness(5),
                Content = new Label { Content = "Remove", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            RemoveButton = removeButton;
            removeButton.Click += detailsPanel.RemoveButton_Click;

            var changeImageButton = new Button
            {
                Tag = "change image",
                Padding = new Thickness(5),
                Content = new Label { Content = "Change Image", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            ChangeImageButton = changeImageButton;
            changeImageButton.Click += detailsPanel.ChangeImageButton_Click;

            var saveImageButton = new Button
            {
                Tag = "save image",
                Padding = new Thickness(5),
                Content = new Label { Content = "Save Image", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            SaveImageButton = saveImageButton;
            saveImageButton.Click += detailsPanel.SaveImageButton_Click;

            var cancelImageButton = new Button
            {
                Tag = "cancel image",
                Padding = new Thickness(5),
                Content = new Label { Content = "Cancel", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            CancelImageButton = cancelImageButton;
            saveImageButton.Click += detailsPanel.SaveImageButton_Click;

            var cancelButton = new Button
            {
                Tag = "cancel",
                Padding = new Thickness(5),
                Content = new Label { Content = "Cancel", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            CancelButton = cancelButton;
            cancelButton.Click += detailsPanel.CancelButton_Click;

            var newProductButton = new Button
            {
                Tag = "new product",
                Padding = new Thickness(5),
                Content = new Label { Content = "New Product", HorizontalAlignment = HorizontalAlignment.Left },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
            NewProductButton = newProductButton;
            newProductButton.Click += detailsPanel.NewProductButton_Click;
            Grid.SetColumn(rightColumn_detailsPanel_AdminButtons, 0);
            detailsPanel._detailsColumn_detailsGrid.Children.Add(rightColumn_detailsPanel_AdminButtons);
        }

    }
    //public static object GetElement(string _tag)
    //{
    //    try
    //    {
    //        return Elements.Find(x => (string)(x.Tag) == _tag);
    //    }
    //    catch (System.Exception ex)
    //    {
    //        return ex;
    //    }
    //}    
}   


