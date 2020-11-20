using Microsoft.VisualStudio.TestTools.UnitTesting;
using Store.Tests;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StoreCommon.Tests
{
    [TestClass]
    public class StoreTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            TestSetup.Init();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //AppFolder.StoreDataCsvPath = Path.Combine(AppFolder.RootFolderPath, ".CSVs"); // Reset StoreDataCsvPath
        }

        [TestMethod]
        public void ProductLoadAll_NameInstances()
        {
            //var products = new List<Product>();
            //Store.LoadProducts(Path.Combine(AppFolder.RootFolderPath, "TestProducts_Clean_5instances.csv"), out products);
            Store.LoadProducts(Path.Combine(AppFolder.RootFolderPath, "ExampleProducts.csv"));
            var nameListActual = new List<string>();

            foreach (var p in Store.Products)
            {
                nameListActual.Add(p.Name);
            }
            var nameListExpected = new List<string>
            {
                "Banana", "Orange", "Broccoli", "Pineapple", "Kiwi"
            };
            CollectionAssert.AreEqual(nameListExpected, nameListActual);
        }

        [TestMethod]
        public void ProductLoadAll_UnWantedHashtags()
        {
            //var products = new List<Product>();
            //Store.LoadProducts(Path.Combine(AppFolder.RootFolderPath, "TestProducts_Clean_5instances.csv"), out products);
            Store.LoadProducts(Path.Combine(AppFolder.RootFolderPath, "ExampleProducts.csv"));
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
            //var products = new List<Product>();
            //Store.LoadProducts(Path.Combine(AppFolder.RootFolderPath, "TestProducts_Clean_5instances.csv"), out products);
            Store.LoadProducts(Path.Combine(AppFolder.RootFolderPath, "ExampleProducts.csv"));
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
            // First load the real example files used by the program to the test's own folder in the system's "Temp"-folder.
            // AppFolder.AppFolder() expects all ".csv"-files and image to exist, and the testdata-folder does not include all files,
            // which necessitates this step.
            AppFolder.SetPaths(null, TestSetup.TestOutputPath);
            // TODO: is this unnecessary?
            AppFolder.SetPaths(Path.Combine(Environment.CurrentDirectory, "TestData"), TestSetup.TestOutputPath);
            TestSetup.CopyTestFiles("TestLoadProducts");

            //var products = new List<Product>();
            //Store.LoadProducts(Path.Combine(AppFolder.RootFolderPath, "TestProducts_Clean_5instances.csv"), out products);
            Store.LoadProducts(Path.Combine(AppFolder.RootFolderPath, "ExampleProducts.csv"));
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
        public void ProductLoadAll_Only_JPG_PNG()
        {
            //var products = new List<Product>();
            //Store.LoadProducts(Path.Combine(AppFolder.RootFolderPath, "TestProducts_Clean_5instances.csv"), out products);
            Store.LoadProducts(Path.Combine(AppFolder.RootFolderPath, "ExampleProducts.csv"));
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
        public void LoadDiscountCodes_LoadFromExampleFile_Success()
        {
            // The contents of the test file (ExampleDiscountCodes.csv):
            // Gimme-free-stuff;1
            // Half-Off;0.5

            // Set the StoreDatePath so that this test's test files are used instead of the the actual files.
            AppFolder.StoreDataCsvPath = Path.Combine(AppFolder.RootFolderPath, "..", "TestData", "StoreTests_LoadDiscountCodes", "csvFiles", "ExampleDiscountCodes.csv");
            Store.LoadDiscountCodes(AppFolder.StoreDataCsvPath);

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
            // The contents of the test file (ExampleDiscountCodes.csv):
            // Gimme-free-stuff;1
            // Half-Off;0.5
            AppFolder.ProjectName = AppFolder.ProjectName + "_Test_SaveDiscountCodes_LoadFromExampleFileModifyAndSave_Success";

            // Set the StoreDatePath so that this test's test files are used instead of the the actual files.
            AppFolder.StoreDataCsvPath = Path.Combine("TestData", "StoreTests_LoadDiscountCodes", "csvFiles", "ExampleDiscountCodes.csv");
            Store.LoadDiscountCodes(AppFolder.StoreDataCsvPath);
            //Store.DiscountCodes = Store.DiscountCodes.Remove(1);

            var expectedDiscountCodes = new List<DiscountCode>
            {
                new DiscountCode(code: "Gimmefreestuff", percentage: 1),
            };

            Store.DiscountCodes = expectedDiscountCodes;
            Store.SaveDiscountCodesToFile();
            Store.LoadDiscountCodes(AppFolder.DiscountCSV);

            var expected = expectedDiscountCodes.Select(discountCode => (discountCode.Code, discountCode.Percentage)).ToArray();
            var actual = Store.DiscountCodes.Select(discountCode => (discountCode.Code, discountCode.Percentage)).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
