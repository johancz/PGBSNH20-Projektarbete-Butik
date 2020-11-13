namespace StoreCommon
{
    public class Product
    {
        public string Name { get; set; }
        public string Description;
        public decimal Price { get; set; }
        public (string Code, string Symbol) Currency { get; set; }
        public string Uri;

        public Product(string name, string uri, decimal price, string description, (string Code, string Symbol)? currency = null)
        {
            Name = name;
            Uri = uri;
            Price = price;
            Currency = currency ?? (Code: "SEK", Symbol: " kr");
            Description = description;
        }
    }
}
