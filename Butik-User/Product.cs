using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Butik_User
{
    class Product
    {
        public string Name { get; set; }
        public string Description;
        public double Price { get; set; }
        public Uri ImageUri;

        public Product() { }
    }
}
