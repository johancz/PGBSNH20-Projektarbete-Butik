using System.Windows.Controls;

namespace StoreCommon
{
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Uri { get; set; }
        public Image Tag { get; set; } // Simplifies the admin Events regarding image switching.

        public Product(string name, string uri, decimal price, string description)
        {
            Name = name;
            Uri = uri;
            Price = price;
            Description = description;
        }
    }
}
