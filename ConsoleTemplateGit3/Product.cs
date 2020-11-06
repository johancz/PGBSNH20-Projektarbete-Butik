using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace ConsoleTemplateGit3
{
    public class Product
    {
        public static string FilePath  = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\LoadProducts.csv";

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

        public void ToFile()
        {
            File.AppendAllText(FilePath, $"{Name}#{Uri}#{Price}#{Description}#");
        }

        public static List<Product> LoadAll()
        {
            var products = new List<Product>();
            string input = File.ReadAllText(FilePath);

            var infoArray = input.Split('#');

            for (int i = 0; i < infoArray.Length; i++)
            {
                var name = infoArray[i].Trim().Trim('\n');
                i++;
                var uri = infoArray[i];
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
    [TestClass]

    public class ExampleTest
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [TestMethod]
        public void GenarateProducts()
        {
            var products = Product.LoadAll();
        }
        public void IndexOutOfBounce()
        {
            try
            {
                int zero = 0;
                int result = 5 / zero;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Index was outside the bounds of the array.");
            }
        }
    }
}
