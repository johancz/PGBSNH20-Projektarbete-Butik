using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace StoreCommon
{
    public static class Store
    {
        public static List<Product> Products { get; set; } = LoadProducts(Path.Combine(Helpers.StoreDataCsvPath, "ExampleProducts.csv"));
        public static ProductList ShoppingCart { get; set; } = new ProductList();
        public static List<DiscountCode> DiscountCodes { get; set; }

        public static List<Product> LoadProducts(string pathAndFileName)
        {
            var products = new List<Product>();
            string input = File.ReadAllText(pathAndFileName);

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
            //ShoppingCart.AddRange(ProductList.LoadFromFile("ExampleShoppingCart.csv")); // possible solution to the above, if they should be merged.
            //MessageBox.Show("You already have items in your shopping cart, do you want to merge shopping cart you're trying to merge?", "Merge Shopping Carts?", MessageBoxButton.YesNoCancel);
            ShoppingCart = ProductList.LoadFromFile(Path.Combine(Helpers.StoreDataCsvPath, "ExampleShoppingCart.csv"));
            // TODO(johancz): Should the ShoppingCart be loaded by default? We would need a new shopping cart button which creates a new shoppingcart and overwrites the file with a blank file.
        }

        // TODO(johancz): not required if the method lives in the ProductList-class.
        public static void SaveShoppingCart()
        {
            ShoppingCart.SaveToFile(Helpers.StoreDataTemporaryOutput, "ShoppingCart.csv");
        }

        private static void LoadDiscountCodes()
        {
            // TODO(johancz): DiscountCodes = ...
        }
    }
}
