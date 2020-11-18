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
    public abstract class AdminFramework
    {
        public static HybridAppWindow AppWindow;  
            public static Window MainWindow;
                public static TabControl WindowTabControl;
                    public static TabItem EditPageTabItem;                    
                        public static Grid EditPageGrid; //two columns

                            public static ScrollViewer BrowserRootScrollViewer;
                                public static WrapPanel BrowserProductsPanel;
                                    public static List<Grid> ImageGrids = new List<Grid>();
                                    public static List<Grid> ProductGrids = new List<Grid>();

                            public static Grid DetailsPanelRootGrid; //two rows
                                public static Image DetailsPanelImage = new Image();
                                public static Grid DetailsTextAndButtonGrid; //two columns
                                    public static Grid DetailsTitleAndDescriptionGrid; //two rows
                                        public static ScrollViewer DetailsDescriptionScrollViewer;
                                            public static TextBox DetailsPanelDescription;

                                        public static StackPanel NameAndPricePanel;
                                            public static TextBox DetailsPanelName;
                                            public static TextBox DetailsPanelPrice;
                                            public static TextBox DetailsPanelCurrency;

                                    public static StackPanel DetailsButtonPanel;
                                        public static Button EditButton;
                                        public static Button RemoveButton;
                                        public static Button ChangeImageButton;
                                        public static Button NewProductButton;
                                        public static Button CancelButton;
                                        public static Button CancelImageButton;
                                        public static Button SaveImageButton;
                                        public static Button SaveChangesButton;

        //State Framework
        public static Product SelectedProduct = null;
        public static Image? SelectedImage = null; //tveksamt

    }
}   


