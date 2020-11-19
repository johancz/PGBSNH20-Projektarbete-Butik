using System.IO;

namespace StoreCommon
{
    public static class AppFolder
    {
        public static string ProjectName = "Fight Club & Veggies_JC.RA";
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
            ProductCSV = Path.Combine(RootFolderPath, "Products.csv");
            DiscountCSV = Path.Combine(RootFolderPath, "DiscountCodes.csv");
            ShoppingCartCSV = Path.Combine(RootFolderPath, "ShoppingCart.csv");

            var storeFolder = new DirectoryInfo(RootFolderPath);
            var imageFolder = new DirectoryInfo(ImageFolderPath);
            storeFolder.Create();
            imageFolder.Create();

            if (!File.Exists(ProductCSV))
            {
                File.Copy(InputProductsCSV, ProductCSV);
            }

            if (!File.Exists(DiscountCSV))
            {
                File.Copy(InputDiscountCodesCSV, DiscountCSV);
            }

            if (!File.Exists(ShoppingCartCSV))
            {
                File.Copy(InputShoppingCartCSV, ShoppingCartCSV);
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
