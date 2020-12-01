using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace StoreCommon
{
    public class ShoppingCart
    {
        public Dictionary<Product, int> Products { get; private set; } = new Dictionary<Product, int>();
        public decimal TotalSum { get; private set; } = 0;
        public decimal FinalSum { get; private set; } = 0;
        public DiscountCode ActiveDiscountCode { get; private set; }

        public void AddProduct(Product product, int count)
        {
            if (product == null)
            {
                return;
            }

            Products.TryAdd(product, 0);
            Products[product] += count;
            TotalSum += product.Price * count;
            UpdateFinalSum();
        }

        public void RemoveProduct(Product product)
        {
            if (product == null)
            {
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
                UpdateFinalSum();
            }
        }

        public void SetDiscountCode(DiscountCode discountCode)
        {
            ActiveDiscountCode = discountCode;
            UpdateFinalSum();
        }

        public void RemoveDiscountCode()
        {
            ActiveDiscountCode = null;
            UpdateFinalSum();
        }

        private void UpdateFinalSum()
        {
            if (ActiveDiscountCode == null)
            {
                FinalSum = TotalSum;
            }
            else
            {
                FinalSum = TotalSum - TotalSum * (decimal)ActiveDiscountCode.Percentage;
            }
        }

        public bool SaveToFile(string path)
        {
            if (Products.Count == 0)
            {
                try
                {
                    File.WriteAllText(path, string.Empty);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            string[] fileContents = Products.Select(productItem => productItem.Key.Name + ";" + productItem.Value).ToArray();

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(path);
            }

            try
            {
                File.WriteAllLines(path, fileContents);
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, your shopping cart could not be saved.");
                return false;
            }
        }

        public static ShoppingCart LoadFromFile(string pathAndFileName)
        {
            string[] fileLines;
            var shoppingList = new ShoppingCart();

            try
            {
                fileLines = File.ReadAllLines(pathAndFileName);
            }
            catch (Exception)
            {
                // Could not load the Shopping Cart from file, return an empty Shopping Cart.
                return shoppingList;
            }

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
