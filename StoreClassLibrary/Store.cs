using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StoreClassLibrary
{
        public static class Store
        {

            public static string TextFolderPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\";
            public static List<Product> Products { get; set; } = LoadProducts("LoadProducts.csv");
            public static ShoppingCart ActiveShoppingCart { get; set; } = new ShoppingCart();
            //public static ShoppingCart shoppingCart = LoadShoppingCart("ShoppingCart.csv");
            public static List<DiscountCode> DiscountCodes { get; set; }

            public static void Init()
            {
                LoadShoppingCart();
                LoadDiscountCodes();
            }

            private static void LoadShoppingCart()
            {
                
            }

            private static void LoadDiscountCodes()
            {
                //DiscountCodes = ...
            }
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
    }
    
}
