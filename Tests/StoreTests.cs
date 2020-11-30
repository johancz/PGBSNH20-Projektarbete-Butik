using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace StoreCommon.Tests
{
    [TestClass]
    public class StoreTests
    {
        private static string TestDataPath = Path.Combine(Environment.CurrentDirectory, "TestData");
        private static string TestOutputPath = Path.Combine(Path.GetTempPath(), DataManager.ProjectName + "__TESTS");

        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            DataManager.SetPaths(TestDataPath, TestOutputPath, true);
            Store.Currency = (Code: "SEK", Symbol: "kr");
            Store.LoadProducts(DataManager.ProductCSV);
            Store.LoadImagePaths(DataManager.ImageFolderPath);
            Store.LoadDiscountCodes(DataManager.DiscountCSV);
            Store.LoadShoppingCart(DataManager.ShoppingCartCSV);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure we don't delete a folder we shouldn't.
            var actualPath = new DirectoryInfo(TestOutputPath).Parent.FullName.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var expectedPath = Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (Directory.Exists(TestOutputPath) && actualPath == expectedPath)
            {
                Directory.Delete(TestOutputPath, true);
            }
        }

        [TestMethod]
        public void ProductLoadAll_AllNamesExist() //testing if all products where created and with correct name.
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
        public void ProductLoadAll_NoUnwantedWhiteInProperties()
        {
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
                charList.Add(p.Description[^1]);
            }
            bool WhiteSpaceExists = charList.Exists(c => c == '\n');
            Assert.IsFalse(WhiteSpaceExists);
        }

        [TestMethod]
        public void ProductLoadAll_WantedNewLinesExistInAnyDescription()
        {
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
        // Testing that edit and remove is saved correctly.
        public void SaveRuntimeAdminProductsToCSV_IsStoreProductsCorrectEditedAndSaved()
        {
            string allProductPreInfo = String.Empty;
            Store.Products.RemoveAt(0);
            Store.Products[^1].Description = "Testing description...";

            Store.Products.ForEach(product => allProductPreInfo += String.Join(product.Name, product.Price, product.Uri, product.Description));
            Store.SaveRuntimeAdminProductsToCSV();
            Store.Products.Clear();
            Store.LoadProducts(DataManager.ProductCSV);
            string allProductPostInfo = String.Empty;
            foreach (var product in Store.Products)
            {
                allProductPostInfo += String.Join(product.Name, product.Price, product.Uri, product.Description);
            }
            Assert.AreEqual(allProductPreInfo, allProductPostInfo);
        }

        [TestMethod]
        public void LoadDiscountCodes_LoadFromExampleFile_Success()
        {
            var expectedDiscountCodes = new List<DiscountCode>
            {
                new DiscountCode(code: "fightclubrule1", percentage: 0.01),
                new DiscountCode(code: "fightclubrule2", percentage: 0.02),
                new DiscountCode(code: "broccolibroccoli", percentage: 0.3),
                new DiscountCode(code: "donotboilthatveg", percentage: 0.2),
                new DiscountCode(code: "gimmefreestuff", percentage: 1),
                new DiscountCode(code: "50percentoff", percentage: 0.5),
            };

            var expected = expectedDiscountCodes.Select(discountCode => (discountCode.Code, discountCode.Percentage)).ToArray();
            var actual = Store.DiscountCodes.Select(discountCode => (discountCode.Code, discountCode.Percentage)).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SaveDiscountCodes_LoadFromExampleFileModifyAndSave_Success()
        {
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_CodeParamNotValidTooLong_ArgumentException()
        {
            // 21 character long string
            var discountCode = new DiscountCode("abcdefghijklmnopqrstu", 0.1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_PercentageParamValueTooSmall_ArgumentException()
        {
            var discountCode = new DiscountCode("abc", 0.0);
        }

        [TestMethod]
        public void ImageFiles_ExistAndCopiedSuccessfully()
        {
            var expectedInputImages = new List<string> { "banana.jpg", "broccoli.jpg", "Fight Club Brad Pitt Notebook.png", "Fight Club Pin.png", "Fight Club Poster.png", "orange.jpg", "Tyler Sticker.png" };

            var actualInputImages = Directory.EnumerateFiles(DataManager.InputImages).Select(file =>
            {
                return file.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar })[^1];
            }).ToList();

            var actualOutputImages = Directory.EnumerateFiles(DataManager.ImageFolderPath).Select(file =>
            {
                return file.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar })[^1];
            }).ToList();

            CollectionAssert.AreEqual(expectedInputImages, actualInputImages);
            CollectionAssert.AreEqual(actualInputImages, actualOutputImages);
        }

        [TestMethod]
        public void CSVFiles_ExistAndCopiedSuccessfully()
        {
            List<string> actualContentOfInputCsvFiles = new List<string>();
            foreach (string file in Directory.GetFiles(DataManager.StoreDataCsvPath))
            {
                if (file.EndsWith(".csv"))
                {
                    actualContentOfInputCsvFiles.AddRange(File.ReadAllLines(file));
                }
            }

            List<string> actualContentOfOutputCsvFiles = new List<string>();
            foreach (string file in Directory.GetFiles(DataManager.RootFolderPath))
            {
                if (file.EndsWith(".csv"))
                {
                    actualContentOfOutputCsvFiles.AddRange(File.ReadAllLines(file));
                }
            }

            Assert.IsTrue(actualContentOfInputCsvFiles.Count > 0 && actualContentOfOutputCsvFiles.Count > 0);
            CollectionAssert.AreEquivalent(actualContentOfInputCsvFiles, actualContentOfOutputCsvFiles);
        }
    }
}
