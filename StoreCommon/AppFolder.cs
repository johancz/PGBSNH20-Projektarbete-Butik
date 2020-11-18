using System.IO;

namespace StoreCommon
{
    public static class AppFolder
    {
        private static readonly string ProjectName = "Fight Club & Veggies_JC.RA";
        private static readonly string RootFolderPath = Path.Combine(Path.GetTempPath(), ProjectName);
        public static readonly string ImageFolderPath = Path.Combine(RootFolderPath, "Images");

        private static readonly string InputProductsCSV = Path.Combine(Helpers.StoreDataCsvPath, "ExampleProducts.csv");
        private static readonly string InputDiscountCodesCSV = Path.Combine(Helpers.StoreDataCsvPath, "ExampleDiscountCodes.csv");
        private static readonly string InputShoppingCartCSV = Path.Combine(Helpers.StoreDataCsvPath, "ExampleShoppingCart.csv");
        private static readonly string InputImages = Helpers.StoreDataImagesPath;

        public static readonly string ProductCSV;
        public static readonly string DiscountCSV;
        public static readonly string ShoppingCartCSV;

        static AppFolder()
        {
            string productCSV = Path.Combine(RootFolderPath, "Products.csv");
            string discountCSV = Path.Combine(RootFolderPath, "DiscountCodes.csv");
            string shoppingCartCSV = Path.Combine(RootFolderPath, "ShoppingCart.csv");

            ProductCSV = productCSV;
            DiscountCSV = discountCSV;
            ShoppingCartCSV = shoppingCartCSV;

            var storeFolder = new DirectoryInfo(RootFolderPath);
            var imageFolder = new DirectoryInfo(ImageFolderPath);
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
                    File.Copy(image, Path.Combine(ImageFolderPath, name));
                }
            }
        }
    }
}
