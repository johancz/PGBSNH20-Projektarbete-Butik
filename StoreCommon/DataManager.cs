using System;
using System.IO;

namespace StoreCommon
{
    public static class DataManager
    {
        public static string ProjectName = "Fight Club & Veggies_JC.RA";
        public static string RootFolderPath;
        public static DirectoryInfo ImageFolder;
        public static string ImageFolderPath;
        public static string ImageStylingFolderPath;

        public static string StoreDataCsvPath { get; set; }
        private static string InputProductsCSV;
        private static string InputDiscountCodesCSV;
        private static string InputShoppingCartCSV;
        public static string InputImages;
        public static string InputStylingImages;

        public static string ProductCSV;
        public static string DiscountCSV;
        public static string ShoppingCartCSV;

        //public static void Init()
        //{
        //    SetPaths();
        //}

        public static void SetPaths(string inputFolderPath = null, string outputFolderPath = null, bool overwrite = false)
        {
            StoreDataCsvPath = inputFolderPath ?? Path.Combine(Environment.CurrentDirectory, "StoreData", ".CSVs");
            RootFolderPath = outputFolderPath ?? Path.Combine(Path.GetTempPath(), ProjectName);
            ImageFolderPath = Path.Combine(RootFolderPath, "Images");
            ImageStylingFolderPath = Path.Combine(RootFolderPath, "AppImages");

            InputProductsCSV = Path.Combine(StoreDataCsvPath, "ExampleProducts.csv");
            InputDiscountCodesCSV = Path.Combine(StoreDataCsvPath, "ExampleDiscountCodes.csv");
            InputShoppingCartCSV = Path.Combine(StoreDataCsvPath, "ExampleShoppingCart.csv");
            InputImages = Path.Combine(Environment.CurrentDirectory, "StoreData", "Images");
            InputStylingImages = Path.Combine(Environment.CurrentDirectory, "StoreData", "Image Styling");

            ProductCSV = Path.Combine(RootFolderPath, "Products.csv");
            DiscountCSV = Path.Combine(RootFolderPath, "DiscountCodes.csv");
            ShoppingCartCSV = Path.Combine(RootFolderPath, "ShoppingCart.csv");

            CopyFiles(overwrite);
        }

        private static void CopyFiles(bool overwrite = false)
        {
            var storeFolder = new DirectoryInfo(RootFolderPath);
            ImageFolder = new DirectoryInfo(ImageFolderPath);
            var imageStylingFolder = new DirectoryInfo(ImageStylingFolderPath);
            storeFolder.Create();
            ImageFolder.Create();
            imageStylingFolder.Create();

            if (!File.Exists(ProductCSV) || (File.Exists(InputProductsCSV) && overwrite))
            {
                File.Copy(InputProductsCSV, ProductCSV, overwrite);
            }

            if (!File.Exists(DiscountCSV) || (File.Exists(InputDiscountCodesCSV) && overwrite))
            {
                File.Copy(InputDiscountCodesCSV, DiscountCSV, overwrite);
            }

            if (!File.Exists(ShoppingCartCSV) || (File.Exists(InputShoppingCartCSV) && overwrite))
            {
                File.Copy(InputShoppingCartCSV, ShoppingCartCSV, overwrite);
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
            if (imageStylingFolder.GetFiles().Length == 0)
            {
                var images = Directory.GetFiles(InputStylingImages);
                foreach (var image in images)
                {
                    string name = image.Split('\\')[^1];
                    File.Copy(image, Path.Combine(ImageStylingFolderPath, name));
                }
            }
        }
    }
}
