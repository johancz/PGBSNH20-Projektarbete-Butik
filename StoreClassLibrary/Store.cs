using System;
using System.Collections.Generic;
using System.IO;

namespace StoreClassLibrary
{
        public static class Store
        {
            public static string TextFolderPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\";
            public static List<Product> Products { get; set; } = LoadProducts("LoadProducts.csv");
            public static ProductList ShoppingCart { get; set; } = new ProductList();
            public static List<DiscountCode> DiscountCodes { get; set; }

            public static List<Product> LoadProducts(string fileName)
            {
                var products = new List<Product>();
                string input = File.ReadAllText(TextFolderPath + fileName);

                var infoArray = input.Trim().Split('#');

                for (int i = 0; i < infoArray.Length; i++)
                {
                    if (infoArray[i] == "") { break; }
                    var name = infoArray[i].Trim();
                    i++;
                    var uri = infoArray[i].Trim();
                    i++;
                    var price = decimal.Parse(infoArray[i].Trim());
                    i++;
                    var description = infoArray[i].Trim();

                    var newProduct = new Product(name, uri, price, description);
                    products.Add(newProduct);
                }
                return products;
            }

            public static void Init()
            {
                LoadShoppingCart();
                LoadDiscountCodes();
            }

            // TODO(johancz): not required if the method lives in the ProductList-class.
            public static void LoadShoppingCart()
            {
            // TODO(johancz): error checking? the ShoppingCart might already contain items.
            //ShoppingCart.AddRange(ProductList.LoadFromFile("ShoppingCart.csv")); // possible solution to the above, if they should be merged.
            //MessageBox.Show("You already have items in your shopping cart, do you want to merge shopping cart you're trying to merge?", "Merge Shopping Carts?", MessageBoxButton.YesNoCancel);
                ShoppingCart = ProductList.LoadFromFile("ShoppingCart.csv"); // TODO(johancz): Should the ShoppingCart be loaded by default? We would need a new shopping cart button which creates a new shoppingcart and overwrites the file with a blank file.
            }

        // TODO(johancz): not required if the method lives in the ProductList-class.
        public static void SaveShoppingCart()
            {
                ShoppingCart.SaveToFile("ShoppingCart.csv");
            }

            private static void LoadDiscountCodes()
            {
                // TODO(johancz): DiscountCodes = ...
            }
    }
}
