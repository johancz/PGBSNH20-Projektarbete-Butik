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
    public abstract class HybridFramework
    {
        public static HybridAppWindow AppWindow;  
            public static Window ActualWindow;
                public static Canvas WindowCanvas;
                    public static TabControl WindowTabControl;
                        public static TabItem EditPageTabItem;                    
                            public static Grid EditPageGrid;

                                public static ScrollViewer BrowserRootScrollViewer;
                                    public static WrapPanel ProductAndImageWrapPanel;
                                        public static List<Grid> ImageGrids = new List<Grid>();
                                        public static List<Grid> ProductGrids = new List<Grid>();

                                public static Grid DetailsPanelRootGrid;
                                    public static Image DetailsPanelImage;
                                    public static Grid DetailsTextAndButtonGrid;
                                        public static Grid DetailsTitleAndDescriptionGrid;
                                            public static ScrollViewer DetailsDescriptionScrollViewer;
                                                public static TextBox DetailsPanelDescription;

                                            public static StackPanel NameAndPricePanel;
                                                public static TextBox DetailsPanelName;
                                                public static TextBox DetailsPanelPrice;
                                                public static TextBox DetailsPanelCurrency;
                                                    public static List<TextBox> EditDetailsTextBoxes = new List<TextBox>();

                                        public static StackPanel DetailsButtonPanel;
                                            public static Button EditProductButton;
                                                public static Button CancelEditButton;
                                                public static Button SaveEditButton;
                                            public static Button ChangeImageButton;
                                                public static Button SaveImageButton;
                                                public static Button CancelImageButton;
                                            public static Button NewProductButton;
                                                public static Button NewProductSaveButton;
                                                public static Button NewProductAbortButton;
                                            public static Button RemoveButton;
                                                public static List<Button> AdminButtons = new List<Button>();

                                public static TabItem EditDiscountCodeTabItem;
                                    public static Grid EditDiscountCodePageGrid;
                                        public static ScrollViewer EditDiscountCodePageScrollViewer = StoreAdmin.Views.ManageDiscountCodesView._rootScrollViewer;
                                            //public static Grid EditDiscountCodeRowsGrid = ;

            public static Label DevLabel;
                public static TabItem DevTab;
                    public static TreeView DevTreeView;
    }
}   


