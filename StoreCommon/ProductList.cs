using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StoreCommon
{
    /// <summary>
    /// Generic list of products which can be used for; shopping carts, shopping lists, wishlists, store-curated lists.
    /// </summary>
    public class ProductList
    {
        // Move to a Settings-class in StoreCommon-namespace? Or StoreCommon.Settings-namespace?
        private static readonly DirectoryInfo _textFolderPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent;
        /// <summary>
        /// Collection of "KeyValuePair"s where:
        ///     Key (Product): instance of Product-class
        ///     Value (int):   itemcount of (Key)"Product"
        /// </summary>
        public Dictionary<Product, int> Products { get; private set; } = new Dictionary<Product, int>();
        // TODO(johancz): Should this be saved to file?
        public decimal TotalSum { get; private set; } = 0;
        // TODO(johancz): should this live in the Store-class?
        // TODO(johancz): should this be saved to file?
        public DiscountCode ActiveDiscountCode { get; private set; }

        public void AddProduct(Product product, int count)
        {
            if (product == null)
            {
                // TODO(johancz): this silently handles cases where product is null, should it be handled otherwise?
                return;
            }

            Products.TryAdd(product, 0);
            Products[product] += count;
            TotalSum += product.Price * count;
        }

        public void RemoveProduct(Product product)
        {
            if (product == null)
            {
                // TODO(johancz): this silently handles cases where product is null, should it be handled otherwise?
                return;
            }

            if (Products.ContainsKey(product))
            {
                if (Products[product] > 1)
                {
                    Products[product]--;
                }
                else
                {
                    Products.Remove(product);
                }

                TotalSum -= product.Price;
            }
        }

        // TODO(johancz): should this live in the Store-class?
        // TODO(johancz): should this be saved to file?
        public void SetDiscountCode(DiscountCode discountCode)
        {
            ActiveDiscountCode = discountCode;
        }

        // TODO(johancz): should this live in the Store-class?
        // TODO(johancz): should this be saved to file?
        public void RemoveDiscountCode()
        {
            ActiveDiscountCode = null;
        }

        public bool SaveToFile(string path)
        {
            if (Products.Count == 0)
            {
                // TODO(johancz): visa ett meddelande om kunden försöker spara en tom lista? eller det kanske är bättre att disabla/gömma knappen
                // The ProductList is empty, do nothing.
                return false;
            }

            string[] fileContents = Products.Select(productItem => productItem.Key.Name + ";" + productItem.Value).ToArray();

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception)
            {
                // TODO(johancz): exception handling
                System.Diagnostics.Debug.WriteLine("Could not create the Directory, it already exists");
            }

            try
            {
                File.WriteAllLines(path, fileContents);
                return true;
            }
            catch (Exception e)
            {
                // TODO(johancz): exception handling
                throw e;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathAndFileName">Filename (including file-extension) of the list to be loaded.</param>
        /// <returns></returns>
        public static ProductList LoadFromFile(string pathAndFileName)
        {
            string[] fileLines;

            try
            {
                // TODO (johancz): Copy files in .csproj instead? This would simplify the path to the ExampleShoppingCart.csv file.
                fileLines = File.ReadAllLines(pathAndFileName);
            }
            catch (Exception)
            {
                // TODO(johancz): exception handling
                throw;
            }

            var shoppingList = new ProductList();
            decimal totalSum = 0;

            foreach (string line in fileLines)
            {
                // Split the line-string into an items-array and trim each item.
                string[] items = line.Split(';').Select(item => item.Trim()).ToArray();

                // Silently ignore lines that are do have the required number of items.
                if (items.Length != 2)
                {
                    continue;
                }

                var product = Store.Products.Find(product => product.Name == items[0]);
                int count = 0;

                if (product == null || !int.TryParse(items[1], out count))
                {
                    // The Product on ShoppingList does not exist in the Store's list of Products ...
                    // or
                    // the 2nd column could not be parsed to an Integer.
                    // Silently ignore this line.
                    continue;
                }

                shoppingList.AddProduct(product: product, count: count);
                totalSum += product.Price * count;
            }

            return shoppingList;
        }
    }
}
