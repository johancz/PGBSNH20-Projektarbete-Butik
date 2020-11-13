using System.IO;

namespace StoreCommon
{
    public static class WinTemp
    {
        private static readonly string Name = "Merch & Veggies inc";
        private static readonly string StoreFolder = Path.Combine(Path.GetTempPath(), Name);
        public static readonly string Images = Path.Combine(StoreFolder, "Images");

        private static readonly string InputProductsCSV = Path.Combine(Helpers.StoreDataCsvPath, "ExampleProducts.csv");
        private static readonly string InputDiscountCodesCSV = Path.Combine(Helpers.StoreDataCsvPath, "ExampleDiscountCodes.csv");
        private static readonly string InputShoppingCartCSV = Path.Combine(Helpers.StoreDataCsvPath, "ExampleShoppingCart.csv");
        private static readonly string InputImages = Helpers.StoreDataImagesPath;

        public static readonly string ProductCSV;
        public static readonly string DiscountCSV;
        public static readonly string ShoppingCartCSV;

        static WinTemp()
        {
            string productCSV = Path.Combine(StoreFolder, "Products.csv");
            string discountCSV = Path.Combine(StoreFolder, "DiscountCodes.csv");
            string shoppingCartCSV = Path.Combine(StoreFolder, "ShoppingCart.csv");

            ProductCSV = productCSV;
            DiscountCSV = discountCSV;
            ShoppingCartCSV = shoppingCartCSV;

            var storeFolder = new DirectoryInfo(StoreFolder);
            var imageFolder = new DirectoryInfo(Images);
            storeFolder.Create();
            imageFolder.Create();

            if (!File.Exists(productCSV))
            {
                File.Copy(InputProductsCSV, productCSV);
            }
            if (!File.Exists(discountCSV))
            {
                File.Copy(InputDiscountCodesCSV, discountCSV);
            }
            if (!File.Exists(shoppingCartCSV))
            {
                File.Copy(InputShoppingCartCSV, shoppingCartCSV);
            }
            if (imageFolder.GetFiles().Length == 0)
            {
                var images = Directory.GetFiles(InputImages);
                foreach (var image in images)
                {
                    string name = image.Split('\\')[^1];
                    File.Copy(image, Path.Combine(Images, name));
                }
            }

        }
    }
}
