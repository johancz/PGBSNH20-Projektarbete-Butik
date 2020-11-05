using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;

namespace Butik_User
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

        public void ToFile(string filePath)
        {
            File.AppendAllText(filePath, $"{Name},{Uri},{Price},{Description}" + Environment.NewLine);
        }

        public static List<Product> LoadAll(string filePath)
        {
            var products = new List<Product>();
            var lines = File.ReadAllLines(filePath).ToList();
            foreach (var l in lines)
            {
                var property = l.Split(',');

                var name = property[0];
                var uri = property[1];
                var price = decimal.Parse(property[2]);
                var description = property[3];
                var newProduct = new Product(name, uri, price, description);
                products.Add(newProduct);
            }

            return products;
        }
    }
}
