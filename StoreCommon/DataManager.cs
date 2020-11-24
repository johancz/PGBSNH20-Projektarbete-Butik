﻿using System;
using System.IO;

namespace StoreCommon
{
    //This class holds the filePaths and copies the app-folders to the tempfolder of the users computer.
    public static class DataManager
    {
        public static string ProjectName = "Fight Club & Veggies_JC.RA";
        public static string RootFolderPath;
        public static DirectoryInfo ImageFolder;
        public static string ImageFolderPath;
        public static string ProductCSV;
        public static string DiscountCSV;
        public static string ShoppingCartCSV;

        public static string StoreDataCsvPath { get; set; }
        public static string InputProductsCSV;
        public static string InputDiscountCodesCSV;
        public static string InputShoppingCartCSV;
        public static string InputImages;


        public static void SetPaths(string inputFolderPath = null, string outputFolderPath = null, bool overwrite = false)
        {
            RootFolderPath = outputFolderPath ?? Path.Combine(Path.GetTempPath(), ProjectName);
            ImageFolderPath = Path.Combine(RootFolderPath, "Images");
            ProductCSV = Path.Combine(RootFolderPath, "Products.csv");
            DiscountCSV = Path.Combine(RootFolderPath, "DiscountCodes.csv");
            ShoppingCartCSV = Path.Combine(RootFolderPath, "ShoppingCart.csv");

            StoreDataCsvPath = inputFolderPath ?? Path.Combine(Environment.CurrentDirectory, "StoreData", ".CSVs");
            InputProductsCSV = Path.Combine(StoreDataCsvPath, "ExampleProducts.csv");
            InputDiscountCodesCSV = Path.Combine(StoreDataCsvPath, "ExampleDiscountCodes.csv");
            InputShoppingCartCSV = Path.Combine(StoreDataCsvPath, "ExampleShoppingCart.csv");
            InputImages = Path.Combine(Environment.CurrentDirectory, "StoreData", "Images");


            CopyInputDataToTemp(overwrite);
        }

        public static void CopyInputDataToTemp(bool overwrite = false, bool FakeStore = false)
        {
            var storeFolder = new DirectoryInfo(RootFolderPath);
            ImageFolder = new DirectoryInfo(ImageFolderPath);
            storeFolder.Create();
            ImageFolder.Create();

            if (!File.Exists(ProductCSV) || (File.Exists(InputProductsCSV) && overwrite) || FakeStore)
            {
                File.Copy(InputProductsCSV, ProductCSV, overwrite);
            }

            if (!File.Exists(DiscountCSV) || (File.Exists(InputDiscountCodesCSV) && overwrite) || FakeStore)
            {
                File.Copy(InputDiscountCodesCSV, DiscountCSV, overwrite);
            }

            if (!File.Exists(ShoppingCartCSV) || (File.Exists(InputShoppingCartCSV) && overwrite) || FakeStore)
            {
                File.Copy(InputShoppingCartCSV, ShoppingCartCSV, overwrite);
            }

            if (ImageFolder.GetFiles().Length == 0 || FakeStore)
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
