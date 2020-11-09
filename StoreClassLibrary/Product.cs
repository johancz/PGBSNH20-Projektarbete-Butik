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
        public decimal Price { get; set; } // TODO(johancz): is decimal necessary?
        public string Uri;

        public Product(string name, string uri, decimal price, string description)
        {
            Name = name;
            Uri = uri;
            Price = price;
            Description = description;
        }
    }
}
