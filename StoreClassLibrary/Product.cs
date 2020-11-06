using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace StoreClassLibrary
{
    public class Product
    {

        public string Name { get; set; }
        public string Description;
        public decimal Price { get; set; }
        public string Uri;

        public Product(string name, string uri, decimal price, string description)
        {
            Name = name;
            Uri = uri;
            Price = price;
            Description = description;
        }

        public static string FilePath(string fileName = "LoadProducts.csv")
        {
            return Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\" + fileName;
        }
        public void ToFile(string fileName = "LoadProducts.csv")
        {
            File.AppendAllText(FilePath(), $"{Name}#{Uri}#{Price}#{Description}#");
        }

        public static List<Product> LoadAll(string fileName = "LoadProducts.csv")
        {
            var products = new List<Product>();
            string input = File.ReadAllText(FilePath(fileName));

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
