using System;
using System.IO;

namespace StoreCommon
{
    public static class AppFolder
    {
        public static string ProjectName = "Fight Club & Veggies_JC.RA";
        public static string RootFolderPath;
        public static DirectoryInfo ImageFolder;
        public static string ImageFolderPath;

        public static string StoreDataCsvPath { get; set; }
        private static string InputProductsCSV;
        private static string InputDiscountCodesCSV;
        private static string InputShoppingCartCSV;
        public static string InputImages;

        public static string ProductCSV;
        public static string DiscountCSV;
        public static string ShoppingCartCSV;

        public static void SetPaths(string inputFolderPath = null, string outputFolderPath = null)
        {
            StoreDataCsvPath = inputFolderPath ?? Path.Combine(Environment.CurrentDirectory, "StoreData", ".CSVs");
            RootFolderPath = outputFolderPath ?? Path.Combine(Path.GetTempPath(), ProjectName);

            RootFolderPath = Path.Combine(Path.GetTempPath(), ProjectName);
            ImageFolderPath = Path.Combine(RootFolderPath, "Images");

            InputProductsCSV = Path.Combine(StoreDataPath, "ExampleProducts.csv");
            InputDiscountCodesCSV = Path.Combine(StoreDataPath, "ExampleDiscountCodes.csv");
            InputShoppingCartCSV = Path.Combine(StoreDataPath, "ExampleShoppingCart.csv");
            InputImages = Path.Combine(Environment.CurrentDirectory, "StoreData", "Images");

            ProductCSV = Path.Combine(RootFolderPath, "Products.csv");
            DiscountCSV = Path.Combine(RootFolderPath, "DiscountCodes.csv");
            ShoppingCartCSV = Path.Combine(RootFolderPath, "ShoppingCart.csv");

            var storeFolder = new DirectoryInfo(RootFolderPath);
            ImageFolder = new DirectoryInfo(ImageFolderPath);
            storeFolder.Create();
            ImageFolder.Create();
        }

        static AppFolder()
        {
            SetPaths();

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

            if (ImageFolder.GetFiles().Length == 0)
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
