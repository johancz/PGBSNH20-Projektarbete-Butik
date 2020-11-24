using System;
using System.Collections.Generic;
using System.IO;
using Store;
using System.Text;
using StoreCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Store.Tests;
using System.Globalization;
using System.Linq;

namespace StoreCommon.Tests
{
    using static DataManager;
    using static Store;
    public static class FakeStore //Creates testfolder and moves datafiles
    {
        public static string TestDataPath;
        public static string TestOutputPath;
        public static void SetFakePaths()
        {
            ProjectName = "__TESTS";
            RootFolderPath = Path.Combine(Path.GetTempPath(), ProjectName);
                ProductCSV = Path.Combine(RootFolderPath, "Products.csv");
                DiscountCSV = Path.Combine(RootFolderPath, "DiscountCodes.csv");
                ShoppingCartCSV = Path.Combine(RootFolderPath, "ShoppingCart.csv");
                ImageFolderPath = Path.Combine(RootFolderPath, "FakeImages");

            StoreDataCsvPath = @"C:\Users\axels\source\repos\johancz\PGBSNH20-Projektarbete-Butik\Tests\FakeStoreData\"; //Behöver hjälp här

                InputProductsCSV = Path.Combine(StoreDataCsvPath, "Fake.CSVs", "FakeExampleProducts.csv");
                InputDiscountCodesCSV = Path.Combine(StoreDataCsvPath, "Fake.CSVs", "FakeExampleDiscountCodes.csv");
                InputShoppingCartCSV = Path.Combine(StoreDataCsvPath, "Fake.CSVs", "FakeExampleShoppingCart.csv");
            InputImages = @"C:\Users\axels\source\repos\johancz\PGBSNH20-Projektarbete-Butik\Tests\FakeStoreData\FakeImages\"; //och här

        }
    }
        
    [TestClass]
    public class FakeStoreTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            FakeStore.SetFakePaths();
            DataManager.CopyInputDataToTemp();
            Store.Currency = (Code: "SEK", Symbol: "kr");
            LoadProducts(DataManager.ProductCSV);
            LoadImagePaths(DataManager.ImageFolderPath);
            LoadDiscountCodes(DataManager.DiscountCSV);
            LoadShoppingCart(DataManager.ShoppingCartCSV);
        }

        [TestCleanup]
        public void TestCleanup()
        {
           Directory.Delete(RootFolderPath, true);            
        }
        [TestMethod]
        public void ProductLoadAll_Only_JPG_PNG()
        {
            var fileExtensions = new List<string>();
            string extension = "";

            foreach (var p in Store.Products)
            {
                for (int i = 4; i >= 1; i--)
                {
                    extension += p.Uri[^i];
                }
                fileExtensions.Add(extension);
                extension = "";
            }
            bool onlyJpgPng = fileExtensions.TrueForAll(e => e == ".jpg" || e == ".png");
            Assert.IsTrue(onlyJpgPng);
        }

        [TestMethod]
        public void ProductLoadAll_NameInstances()
        {
           
            var nameListActual = new List<string>();

            foreach (var p in Store.Products)
            {
                nameListActual.Add(p.Name);
            }
            var nameListExpected = new List<string>
            {
                "Gulebøj", "Trump", "Broccoli", "Fight Club Brad Pitt Notebook", "Tyler Sticker", "Fight Club Poster", "Fight Club Pin", "Sample"
            };
            CollectionAssert.AreEqual(nameListExpected, nameListActual);
        }

        [TestMethod]
        public void ProductLoadAll_UnWantedHashtags()
        {
            Store.LoadProducts(Path.Combine(DataManager.RootFolderPath, "Products.csv"));
            var charList = new List<char>();

            foreach (var p in Store.Products)
            {
                foreach (var c in p.Name)
                {
                    charList.Add(c);
                }
                foreach (var c in p.Uri)
                {
                    charList.Add(c);
                }
                foreach (var c in p.Description)
                {
                    charList.Add(c);
                }
            }
            bool targetNotExists = !charList.Exists(c => c == '#');
            Assert.IsTrue(targetNotExists);
        }
        [TestMethod]
        public void ProductLoadAll_UnWantedWhite()
        {

            Store.LoadProducts(Path.Combine(DataManager.RootFolderPath, "Products.csv"));
            var charList = new List<char>();

            foreach (var p in Store.Products)
            {
                foreach (var c in p.Name)
                {
                    charList.Add(c);
                }
                foreach (var c in p.Uri)
                {
                    charList.Add(c);
                }
                charList.Add(p.Description[0]);
            }
            bool targetNotExists = !charList.Exists(c => c == '\n');
            Assert.IsTrue(targetNotExists);
        }
        [TestMethod]
        public void ProductLoadAll_WantedNewLinesCanExist()
        {

            Store.LoadProducts(Path.Combine(DataManager.RootFolderPath, "Products.csv"));
            var charList = new List<char>();

            foreach (var p in Store.Products)
            {
                foreach (var c in p.Description)
                {
                    charList.Add(c);
                }
            }
            bool targetExists = charList.Exists(c => c == '\n');
            Assert.IsTrue(targetExists);
        }
        [TestMethod]
        public void LoadDiscountCodes_LoadFromExampleFile_Success()
        {

            Store.LoadDiscountCodes(Path.Combine(DataManager.RootFolderPath, "DiscountCodes.csv"));

            var expectedDiscountCodes = new List<DiscountCode>
        {
            new DiscountCode(code: "Gimmefreestuff", percentage: 1),
            new DiscountCode(code: "HalfOff", percentage: 0.5),
        };

            var expected = expectedDiscountCodes.Select(discountCode => (discountCode.Code, discountCode.Percentage)).ToArray();
            var actual = Store.DiscountCodes.Select(discountCode => (discountCode.Code, discountCode.Percentage)).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SaveDiscountCodes_LoadFromExampleFileModifyAndSave_Success()
        {

            Store.LoadDiscountCodes(Path.Combine(DataManager.RootFolderPath, "DiscountCodes.csv"));

            var expectedDiscountCodes = new List<DiscountCode>
        {
            new DiscountCode(code: "Gimmefreestuff", percentage: 1),
        };

            Store.DiscountCodes = expectedDiscountCodes;
            Store.SaveDiscountCodesToFile();
            Store.LoadDiscountCodes(DataManager.DiscountCSV);

            var expected = expectedDiscountCodes.Select(discountCode => (discountCode.Code, discountCode.Percentage)).ToArray();
            var actual = Store.DiscountCodes.Select(discountCode => (discountCode.Code, discountCode.Percentage)).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
    
}
