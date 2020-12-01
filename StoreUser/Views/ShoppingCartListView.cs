using StoreCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StoreUser.Views
{
    public static class ShoppingCartListView
    {
        private static ListView _root;
        private static GridView _gridView;

        // Defines data for each row/"item" in the ListView.
        private class ShoppingCartItemData
        {
            public Product Product { get; }
            public string ProductPrice { get; }
            public int ProductCount { get; }
            public string ProductFinalPrice { get; }

            public ShoppingCartItemData(KeyValuePair<Product, int> item)
            {
                Product = item.Key;
                // discounted price for single item cost?
                ProductCount = item.Value;
                decimal afterDiscount = 1;
                if (Store.ShoppingCart.ActiveDiscountCode != null)
                {
                    afterDiscount -= (decimal)(1 - Store.ShoppingCart.ActiveDiscountCode.Percentage);
                }
                ProductPrice = Math.Round(item.Key.Price * afterDiscount, 2) + " " + Store.Currency.Symbol;
                ProductFinalPrice = Math.Round(Product.Price * ProductCount * afterDiscount, 2) + " " + Store.Currency.Symbol;
            }
        };

        public static ListView Init()
        {
            CreateGUI();
            Update();
            return _root;
        }

        public static void CreateGUI()
        {
            static FrameworkElementFactory CreateButtonTemplate(string buttonText, RoutedEventHandler eventHandler)
            {
                // Create a "Button"-template which represents a Button and (in our case) the only control inserted into cells.
                var buttonTemplate = new FrameworkElementFactory(typeof(Button));
                // No need for a binding since the button will have the same value on every ListView row/"item". 
                buttonTemplate.SetValue(Button.ContentProperty, buttonText);
                // Bind Tag-property on Button to the row's/item's "Product"-property in its "ShoppingCartItemData".
                buttonTemplate.SetBinding(Button.TagProperty, new Binding("Product"));
                buttonTemplate.AddHandler(Button.ClickEvent, new RoutedEventHandler(eventHandler));

                return buttonTemplate;
            }

            _root = new ListView();
            _root.SelectionChanged += ListSelectionChanged;
            _gridView = new GridView { AllowsColumnReorder = false };
            _root.View = _gridView;

            var remove1Template = CreateButtonTemplate(" - ", ShoppingCartRemoveProduct_Click);
            var add1Template = CreateButtonTemplate(" + ", ShoppingCartAddProduct_Click);

            // Add columns to the GridView and bind each column to a property in the data-object ("ShoppingCartItemData").
            _gridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("Product.Name"), Header = "Product", });
            _gridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("ProductPrice"), Header = "Price", });
            _gridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("ProductCount"), Header = "# of items", });
            _gridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("ProductFinalPrice"), Header = "Total Price", });
            _gridView.Columns.Add(new GridViewColumn { CellTemplate = new DataTemplate { VisualTree = remove1Template, }, Header = "-", });
            _gridView.Columns.Add(new GridViewColumn { CellTemplate = new DataTemplate { VisualTree = add1Template, }, Header = "+", });
        }

        public static void Update()
        {
            UpdateData();
            UpdateGUI();
        }

        private static void UpdateData()
        {
            _root.ItemsSource = Store.ShoppingCart.Products.Select(productItem => new ShoppingCartItemData(productItem));
        }

        public static void UpdateGUI()
        {
            // And to be sure the ListView layout updates, we also call UpdateLayout() on the ListView.
            _root.UpdateLayout();

            //Resize each column to fit its content, double.NaN == auto
            foreach (GridViewColumn column in _gridView.Columns)
            {
                if (double.IsNaN(column.Width))
                {
                    column.Width = column.ActualWidth;
                }

                column.Width = double.NaN;
            }
        }

        /******************************************************/
        /******************* Event Handling *******************/
        /******************************************************/

        private static void ShoppingCartRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = (Product)((Button)sender).Tag;
            if (product != null)
            {
                Store.ShoppingCart.RemoveProduct(product);
                Store.ShoppingCart.SaveToFile(DataManager.ShoppingCartCSV);
                UserView.SelectedProduct = product;
                UserView.UpdateGUI();
            }
        }

        private static void ShoppingCartAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = (Product)((Button)sender).Tag;
            if (product != null)
            {
                UserView.SelectedProduct = product;
                Store.ShoppingCart.AddProduct(product, 1);
                Store.ShoppingCart.SaveToFile(DataManager.ShoppingCartCSV);
                UserView.UpdateGUI();
            }
        }

        private static void ListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listViewItemData = ((ShoppingCartItemData)_root.SelectedItem);
            if (listViewItemData != null)
            {
                var product = (Product)listViewItemData.Product;
                UserView.SelectedProduct = product;
                UserView.UpdateGUI();
            }
        }
    }
}
