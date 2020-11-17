using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using StoreCommon;

namespace StoreUser.Views
{
    public static class View_ShoppingCartList
    {
        private static ListView _root; // _shoppingList_listView

        internal static ListView _shoppingList_listView; // _root?
        internal static GridView _gridView;

        public static ListView Init()
        {
            CreateGUI();
            UpdateData();
            UpdateGUI();
            //return _root;
            return _shoppingList_listView;
        }

        public static void CreateGUI()
        {
            //var shoppingCartPanel = new StackPanel { Orientation = Orientation.Vertical };


            var buttonFactory_buttonRemove1 = new FrameworkElementFactory(typeof(Button));
            buttonFactory_buttonRemove1.SetBinding(Button.ContentProperty, new Binding("buttonRemove1"));
            buttonFactory_buttonRemove1.SetBinding(Button.TagProperty, new Binding("product"));
            buttonFactory_buttonRemove1.AddHandler(Button.ClickEvent, new RoutedEventHandler(EventHandler.UserView_ShoppingCartRemoveProduct_Click));

            var add1_buttonFactory = new FrameworkElementFactory(typeof(Button));
            add1_buttonFactory.SetBinding(Button.ContentProperty, new Binding("buttonAdd1"));
            add1_buttonFactory.SetBinding(Button.TagProperty, new Binding("product"));
            add1_buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(EventHandler.UserView_ShoppingCartAddProduct_Click));

            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stackPanelFactory.AppendChild(buttonFactory_buttonRemove1);
            stackPanelFactory.AppendChild(add1_buttonFactory);

            _shoppingList_listView = new ListView(); // _root
            _shoppingList_listView.SelectionChanged += EventHandler._shoppingList_listView_SelectionChanged;
            UpdateShoppingCartView();

            _gridView = new GridView { AllowsColumnReorder = false };
            var style = new Style { TargetType = typeof(GridViewColumnHeader) };
            style.Setters.Add(new Setter(ListViewItem.IsEnabledProperty, false));
            var t = new Trigger { Property = ListViewItem.IsEnabledProperty, Value = false };
            t.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.Black));
            style.Triggers.Add(t);
            _gridView.ColumnHeaderContainerStyle = style;
            _gridView.Columns.Add(new GridViewColumn
            {
                DisplayMemberBinding = new Binding("product.Name"),
                Header = "Produkt"
            });
            _gridView.Columns.Add(new GridViewColumn
            {
                DisplayMemberBinding = new Binding("productPrice"),
                Header = "Price"
            });
            _gridView.Columns.Add(new GridViewColumn
            {
                DisplayMemberBinding = new Binding("productCount"),
                Header = "# of items"
            });
            _gridView.Columns.Add(new GridViewColumn
            {
                DisplayMemberBinding = new Binding("productTotalPrice"),
                Header = "Total Price"
            });
            _gridView.Columns.Add(new GridViewColumn
            {
                CellTemplate = new DataTemplate { VisualTree = stackPanelFactory },
                Header = "+/- items"
            });
            _shoppingList_listView.View = _gridView;

            //////////////////// TODO(johancz): ifall vi byter till en dummare control.
            // 
            //foreach (KeyValuePair<Product, int> product in Store.ShoppingCart.Products)
            //{
            //    var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            //    var labelName = new Label { Content = product.Key.Name };
            //    var labelCount = new Label { Content = product.Value };
            //    var labelTotalPrice = new Label { Content = product.Value * product.Key.Price };
            //    stackPanel.Children.Add(labelName);
            //    stackPanel.Children.Add(labelCount);
            //    stackPanel.Children.Add(labelTotalPrice);

            //    //var listBoxItem = new ListBoxItem();
            //    //listBoxItem.Content = stackPanel;
            //    //listBox.Items.Add(listBoxItem);

            //    var listViewItem = new ListViewItem();
            //    listViewItem.Content = stackPanel;
            //    listView.Items.Add(listViewItem);
            //}
            ////////////////////
        }

        public static void UpdateGUI()
        {
        }

        private static void UpdateData()
        {

        }

        internal static void UpdateShoppingCartView()
        {
            var combinedData = Store.ShoppingCart.Products.Select(product =>
            {
                dynamic productRow = new ExpandoObject();
                productRow.product = product.Key;
                productRow.productPrice = product.Key.Price + Store.Currency.Symbol;
                productRow.productCount = product.Value;
                productRow.productTotalPrice = product.Key.Price * product.Value + Store.Currency.Symbol;
                productRow.buttonRemove1 = " - ";
                productRow.buttonAdd1 = " + ";

                return productRow;
            });

            _shoppingList_listView.ItemsSource = combinedData;
        }

        private static class EventHandler
        {
            internal static void UserView_ShoppingCartRemoveProduct_Click(object sender, RoutedEventArgs e)
            {
                // TODO(johancz): Error/Exception-handling
                var product = (Product)((Button)sender).Tag;
                Store.ShoppingCart.RemoveProduct(product);
                UserView.UpdateGUI();
            }

            internal static void UserView_ShoppingCartAddProduct_Click(object sender, RoutedEventArgs e)
            {
                // TODO(johancz): Error/Exception-handling
                var product = (Product)((Button)sender).Tag;
                UserView._selectedProduct = product;
                Store.ShoppingCart.AddProduct(product, 1);
                UserView.UpdateGUI();
            }

            internal static void _shoppingList_listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                ExpandoObject listViewItemData = ((ExpandoObject)_shoppingList_listView.SelectedItem);
                if (listViewItemData != null)
                {
                    var product = (Product)listViewItemData.ToList()[0].Value;
                    UserView.UpdateGUI();
                }
            }
        }
    }
}
