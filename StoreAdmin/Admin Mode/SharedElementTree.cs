using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace StoreCommon
{
    public class SharedElementTree
        
        //This class function is to give an clear overview of the used wpf elements and gives all classes possiblility to share those elements through inheritence. This removes the limitations of Main Window but has the drawback of limiting classes that could need inheritence from another class.
        //It is also modular as it can be shared with other window-apps/modes very easily.
    {
        public static AdminHybridWindow AppWindow;  
            public static Window MainWindow;
                public static TabControl WindowTabControl;
                    
                    public static TabItem EditPageTabItem;                    
                        public static Grid EditPageGrid;
                            public static ScrollViewer BrowserRootScrollViewer;
                                public static WrapPanel ProductAndImageWrapPanel;
                                    public static List<Grid> ImageGridItems = new List<Grid>();
                                    public static List<Grid> ProductGridItems = new List<Grid>();

                            public static Grid DetailsPanelRootGrid;
                                public static Image DetailsPanelImage;
                                public static Grid DetailsTextAndButtonGrid;
                                    public static Grid DetailsTitleAndDescriptionGrid;
                                        public static ScrollViewer DetailsDescriptionScrollViewer;
                                            public static TextBox DetailsPanelDescription;
                                                public static List<TextBox> EditDetailsTextBoxes = new List<TextBox>();

                                        public static StackPanel NameAndPricePanel;
                                            public static TextBox DetailsPanelName;
                                            public static TextBox DetailsPanelPrice;
                                            public static TextBox DetailsPanelCurrency;

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


    }
}   


