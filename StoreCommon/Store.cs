using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StoreCommon
{
    public static class Store
    {
        public static (string Code, string Symbol) Currency { get; set; }

        public static List<Product> Products { get; set; } = new List<Product>();
        public static ProductList ShoppingCart { get; set; } = new ProductList();
        public static List<string> ImageItemFilePaths { get; set; } = new List<string>();
        public static List<DiscountCode> DiscountCodes { get; set; } = new List<DiscountCode>();

        public static void LoadProducts(string pathAndFileName)
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
            Products = products;
        }
        public static void LoadImagePaths(string imageFolderPath)
        {
            var imageFolder = new DirectoryInfo(imageFolderPath);
            var files = imageFolder.GetFiles().ToList();
            foreach (var file in files)
            {
                ImageItemFilePaths.Add(file.FullName);
            }
        }
        public static void SaveRuntimeProductsToCSV()
        {
            string productText = "";
            foreach (var product in Products)
            {
                productText += String.Join('#', new[] {
                    product.Name,
                    product.Uri,
                    product.Price.ToString(),
                    product.Description + "#\n\n"
                });
            }
            File.WriteAllText(AppFolder.ProductCSV, productText);
        }
        public static void LoadProducts(string pathAndFileName, out List<Product> products)
        {
            products = new List<Product>();
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
        }

        public static void Init()
        {
            Store.Currency = (Code: "SEK", Symbol: "kr");
            LoadProducts(AppFolder.ProductCSV);
            LoadImagePaths(AppFolder.ImageFolderPath);
            LoadDiscountCodes(AppFolder.DiscountCSV);
            LoadShoppingCart(AppFolder.ShoppingCartCSV);
        }

        // TODO(johancz): not required if the method lives in the ProductList-class.
        public static void LoadShoppingCart(string path)
        {
            // TODO(johancz): error checking? the ShoppingCart might already contain items.
            //ShoppingCart.AddRange(ProductList.LoadFromFile("ExampleShoppingCart.csv")); // possible solution to the above, if they should be merged.
            //MessageBox.Show("You already have items in your shopping cart, do you want to merge shopping cart you're trying to merge?", "Merge Shopping Carts?", MessageBoxButton.YesNoCancel);
            ShoppingCart = ProductList.LoadFromFile(path);
            // TODO(johancz): Should the ShoppingCart be loaded by default? We would need a new shopping cart button which creates a new shoppingcart and overwrites the file with a blank file.
        }

        // TODO(johancz): not required if the method lives in the ProductList-class.
        public static void SaveShoppingCart()
        {
            ShoppingCart.SaveToFile(AppFolder.ShoppingCartCSV);
        }

        public static void LoadDiscountCodes(string path)
        {
            string[] fileLines;

            try
            {
                fileLines = File.ReadAllLines(path);
            }
            catch (Exception)
            {
                // TODO(johancz): exception handling
                throw;
            }

            var discountCodes = new List<DiscountCode>();

            foreach (string line in fileLines)
            {
                // Split the line-string into an items-array and trim each item.
                string[] items = line.Split(';').Select(item => item.Trim()).ToArray();

                // Silently ignore lines that are do have the required number of items.
                if (items.Length != 2)
                {
                    continue;
                }

                string discountCode = items[0].Trim();
                double discountPercentage;

                if (discountCode == "" || !Double.TryParse(items[1], out discountPercentage))
                {
                    // the item in the 1st column (DiscountCode.Code) is an emptry string.
                    // or
                    // the item in the 2nd column could not be parsed to a double.
                    // Silently ignore this line.
                    continue;
                }

                discountCodes.Add(new DiscountCode(code: discountCode, percentage: discountPercentage));
            }

            DiscountCodes = discountCodes;
        }

        public static bool AddDiscountCode(string text)
        {
            var discountCode = DiscountCodes.Find(dc => dc.Code == text.Trim());

            if (discountCode == null)
            {
                return false;
            }

            ShoppingCart.SetDiscountCode(discountCode);
            return true;
        }

        public static void RemoveDiscountCode()
        {
            ShoppingCart.RemoveDiscountCode();
        }
    }
}
