using StoreCommon;
using System;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StoreUser.Views
{
    public static class ShoppingCartListView
    {
        private static ListView _root;
        internal static GridView _gridView;

        public static ListView Init()
        {
            CreateGUI();
            Update();
            return _root;
        }

        public static void CreateGUI()
        {
            var remove1Template = new FrameworkElementFactory(typeof(Button));
            remove1Template.SetBinding(Button.ContentProperty, new Binding("buttonRemove1"));
            remove1Template.SetBinding(Button.TagProperty, new Binding("productItem.Key"));
            remove1Template.AddHandler(Button.ClickEvent, new RoutedEventHandler(EventHandler.ShoppingCartRemoveProduct_Click));

            var add1Template = new FrameworkElementFactory(typeof(Button));
            add1Template.SetBinding(Button.ContentProperty, new Binding("buttonAdd1"));
            add1Template.SetBinding(Button.TagProperty, new Binding("productItem.Key"));
            add1Template.AddHandler(Button.ClickEvent, new RoutedEventHandler(EventHandler.ShoppingCartAddProduct_Click));

            _root = new ListView { HorizontalContentAlignment = HorizontalAlignment.Stretch };
            _root.SelectionChanged += EventHandler.ListSelectionChanged;
            _gridView = new GridView { AllowsColumnReorder = false };
            _root.View = _gridView;

            // Add columns to the GridView and bind each column to a property/field in the data-object.
            _gridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("productItem.Key.Name"), Header = "Product", });
            _gridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("productPrice"), Header = "Price", });
            _gridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("productItem.Value"),  Header = "# of items", });
            _gridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("productFinalPrice"), Header = "Total Price", });
            _gridView.Columns.Add(new GridViewColumn { CellTemplate = new DataTemplate { VisualTree = remove1Template,  }, Header = "-", });
            _gridView.Columns.Add(new GridViewColumn { CellTemplate = new DataTemplate { VisualTree = add1Template }, Header = "+", });
        }

        public static void Update()
        {
            UpdateData();
            UpdateGUI();
        }

        internal static void UpdateData()
        {
            var combinedData = Store.ShoppingCart.Products.Select(productItem =>
            {
                dynamic productRow = new ExpandoObject();
                productRow.product = productItem;
                productRow.productPrice = Math.Round(productItem.Key.Price, 2) + Store.Currency.Symbol;
                productRow.productFinalPrice = productItem.Key.Price * productItem.Value;
                if (Store.ShoppingCart.ActiveDiscountCode != null)
                {
                    productRow.productFinalPrice *= (decimal)(1 - Store.ShoppingCart.ActiveDiscountCode.Percentage);
                }
                productRow.productFinalPrice += " " + Store.Currency.Symbol;
                productRow.buttonRemove1 = " - ";
                productRow.buttonAdd1 = " + ";

                return productRow;
            });

            // When Updating the data for the ListView, it takes take of updating the layout on it's own.
            _root.ItemsSource = combinedData;
            // And to be sure the ListView layout updates, we also call UpdateLayout.
            _root.UpdateLayout();
        }

        public static void UpdateGUI()
        {
            //Resize each column to fit its content, double.NaN
            foreach (GridViewColumn column in _gridView.Columns)
            {
                if (double.IsNaN(column.Width))
                {
                    column.Width = column.ActualWidth;
                }

                column.Width = double.NaN;
            }
        }

        private static class EventHandler
        {
            internal static void ShoppingCartRemoveProduct_Click(object sender, RoutedEventArgs e)
            {
                // TODO(johancz): Error/Exception-handling
                var product = (Product)((Button)sender).Tag;
                Store.ShoppingCart.RemoveProduct(product);
                Store.ShoppingCart.SaveToFile(DataManager.ShoppingCartCSV);
                UserView.SelectedProduct = product;
                UserView.UpdateGUI();
            }

            internal static void ShoppingCartAddProduct_Click(object sender, RoutedEventArgs e)
            {
                // TODO(johancz): Error/Exception-handling
                var product = (Product)((Button)sender).Tag;
                UserView.SelectedProduct = product;
                Store.ShoppingCart.AddProduct(product, 1);
                Store.ShoppingCart.SaveToFile(DataManager.ShoppingCartCSV);
                UserView.UpdateGUI();
            }

            internal static void ListSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                ExpandoObject listViewItemData = ((ExpandoObject)_root.SelectedItem);
                if (listViewItemData != null)
                {
                    var product = (Product)listViewItemData.ToList()[0].Value;
                    UserView.SelectedProduct = product;
                    UserView.UpdateGUI();
                }
            }
        }
    }
}
