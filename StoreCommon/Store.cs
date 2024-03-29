﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace StoreCommon
{
    public static class Store
    {
        //Creates all the objects that handles the data from csv-files and images.
        public static (string Code, string Symbol) Currency { get; set; }

        public static List<Product> Products { get; set; } = new List<Product>();
        public static ShoppingCart ShoppingCart { get; set; } = new ShoppingCart();
        public static List<string> ImageItemFilePaths { get; set; } = new List<string>();
        public static List<DiscountCode> DiscountCodes { get; set; } = new List<DiscountCode>();

        public static void Init()
        {
            DataManager.SetPaths();
            Store.Currency = (Code: "SEK", Symbol: "kr");
            LoadProducts(DataManager.ProductCSV);
            LoadImagePaths(DataManager.ImageFolderPath);
            LoadDiscountCodes(DataManager.DiscountCSV);
            LoadShoppingCart(DataManager.ShoppingCartCSV);
        }

        public static void LoadProducts(string pathAndFileName)
        {
            var products = new List<Product>();
            string productTextInput = File.ReadAllText(pathAndFileName);
            var productStrings = productTextInput.Split('}').ToList();

            foreach (var productString in productStrings)
            {
                var properties = productString.Split('#');
                if (properties.Length != 4) continue;

                decimal price;
                var name = properties[0].Trim();
                var uri = properties[1].Trim();
                if (!decimal.TryParse(properties[2].Trim(), out price)) price = 0;
                price = Math.Round(price, 2);
                var description = properties[3].Trim();
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
        public static void SaveRuntimeAdminProductsToCSV()
        {
            string productText = String.Empty;
            foreach (var product in Products)
            {
                productText += String.Join('#', new[] {
                    product.Name,
                    product.Uri,
                    product.Price.ToString(),
                    product.Description + "\n}\n"
                });
            }
            File.WriteAllText(DataManager.ProductCSV, productText);
        }

        public static void LoadShoppingCart(string path)
        {
            ShoppingCart = ShoppingCart.LoadFromFile(path);
        }

        public static void SaveShoppingCart()
        {
            ShoppingCart.SaveToFile(DataManager.ShoppingCartCSV);
        }

        public static void LoadDiscountCodes(string path)
        {
            string[] fileLines;
            var discountCodes = new List<DiscountCode>();

            try
            {
                fileLines = File.ReadAllLines(path);
            }
            catch (Exception e)
            {
                // Silently fail.
                Debug.WriteLine("The discount codes could not be loaded " + e.Message);
                return;
            }

            foreach (string line in fileLines)
            {
                string[] items = line.Split(';').Select(item => item.Trim()).ToArray();

                // Silently ignore lines that are do have the required number of items.
                if (items.Length != 2)
                {
                    continue;
                }

                string discountCode = items[0].Trim();
                double discountPercentage;

                if (discountCode == "" || !double.TryParse(items[1], out discountPercentage))
                {
                    // Silently ignore this line.
                    continue;
                }

                discountCodes.Add(new DiscountCode(code: discountCode, percentage: discountPercentage));
            }

            DiscountCodes = discountCodes;
        }

        public static bool AddDiscountCode(string text)
        {
            var discountCode = DiscountCodes.Find(dc => {
                return string.Equals(dc.Code, text.Trim(), StringComparison.OrdinalIgnoreCase);
            });

            if (discountCode == null) return false;
            ShoppingCart.SetDiscountCode(discountCode);
            return true;
        }

        public static void RemoveDiscountCode()
        {
            ShoppingCart.RemoveDiscountCode();
        }

        public static void SaveDiscountCodesToFile()
        {
            string[] linesToSave = DiscountCodes.Select(discountCode => $"{discountCode.Code};{discountCode.Percentage}").ToArray();
            File.WriteAllLines(DataManager.DiscountCSV, linesToSave);
        }

        public static void ClearShoppingCart()
        {
            Store.ShoppingCart = new ShoppingCart();
            Store.SaveShoppingCart();
        }
    }
}
