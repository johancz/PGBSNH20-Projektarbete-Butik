using System.Windows.Controls;

namespace StoreCommon
{
    public class Product
    {
        public string Name { get; set; }
        public string Description;
        public decimal Price { get; set; }
        public string Uri;
        public Image Tag { get; set; } //Simplifys the adminevents switching image.

        public Product(string name, string uri, decimal price, string description)
        {
            Name = name;
            Uri = uri;
            Price = price;
            Description = description;
        }
    }
}
