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

        // This test makes no sense. Why are we testing the file extension of images when no code deals with file extensions?
        //[TestMethod]
        //public void ProductLoadAll_Only_JPG_PNG()
        //{
        //    var fileExtensions = new List<string>();
        //    string extension = "";

        //    foreach (var p in Store.Products)
        //    {
        //        for (int i = 4; i >= 1; i--)
        //        {
        //            extension += p.Uri[^i];
        //        }
        //        fileExtensions.Add(extension);
        //        extension = "";
        //    }
        //    bool onlyJpgPng = fileExtensions.TrueForAll(e => e == ".jpg" || e == ".png");
        //    Assert.IsTrue(onlyJpgPng);
        //}

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
        public void ProductLoadAll_NoUnwantedHashtags()
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
                foreach (var c in p.Description)
                {
                    charList.Add(c);
                }
            }
            bool targetNotExists = !charList.Exists(c => c == '#');
            Assert.IsTrue(targetNotExists);
        }

        [TestMethod]
        public void ProductLoadAll_NoUnwantedWhite()
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
            }
            bool targetNotExists = !charList.Exists(c => c == '\n');
            Assert.IsTrue(targetNotExists);
        }

        [TestMethod]
        public void ProductLoadAll_WantedNewLinesExist()
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
        public void LoadDiscountCodes_LoadFromExampleFile_Success()
        {
            var expectedDiscountCodes = new List<DiscountCode>
            {
                new DiscountCode(code: "fightclubrule1", percentage: 0.01  ),
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
        public void DiscountCode_AllParamsAreValid_ValidDiscountCode()
        {
            var discountCode = new DiscountCode("abc", 0.0001);
            Assert.AreEqual("abc", discountCode.Code);
            Assert.AreEqual(0.0001, discountCode.Percentage);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DiscountCode_CodeParamIsNull_ArgumentNullException()
        {
            var discountCode = new DiscountCode(null, 0.0001);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_CodeParamNotValidEmptyString_ArgumentException()
        {
            var discountCode = new DiscountCode("", 0.0001);
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
            var discountCode = new DiscountCode("a", 0.0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_PercentageParamValueTooBig_ArgumentException()
        {
            var discountCode = new DiscountCode("a", 1.0001);
        }

        // Is this one test or many tests hidden in one? There is no spoon.
        [TestMethod]
        public void HardcodedPathsAndFilesExist()
        {
            // Sanity check:
            Assert.IsFalse(
                Directory.Exists(Path.Combine(DataManager.StoreDataCsvPath, "folderThatDoesNotExist")),
                "\"StoreData\\folderThatDoesNotExist\\\"-folder does not exist");
            // Check if the folders exist in the "output directory"\ and "output directory"\StoreData\
            Assert.IsTrue(Directory.Exists(DataManager.StoreDataCsvPath), "\"StoreData\\\"-folder could not be found in the output directory.");
            Assert.IsTrue(Directory.Exists(DataManager.InputImages), "\"StoreData\\Images\\\"-folder could not be found in the output directory.");
            // Make sure the ".csv"-files exist in the "source"-directory "\StoreData\.CSVs\".
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.StoreDataCsvPath, "ExampleProducts.csv")), "Can't find .csv-file.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.StoreDataCsvPath, "ExampleShoppingCart.csv")), "Can't find .csv-file.");
            // Make sure the images exist in the "source"-directory "\StoreData\Images\".
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "banana.jpg")), "Can't find image.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "broccoli.jpg")), "Can't find image.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "Fight Club Brad Pitt NoteBook.png")), "Can't find image.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "Fight Club Pin.png")), "Can't find image.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "Fight Club Poster.png")), "Can't find image.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "orange.jpg")), "Can't find image.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "Tyler Sticker.png")), "Can't find image.");

            // Make sure the images and ".csv"-files were copied successfully to the "Temp"-folder.
            Assert.IsTrue(Directory.Exists(DataManager.ImageFolderPath), "Directory doesn't exist.");
            string[] exampleImagePaths = Directory.EnumerateFiles(DataManager.InputImages).Where(file =>
            {
                return new[] { ".jpg", ".jpeg", ".png" }.Any(file.ToLower().EndsWith);
            }).ToArray();
            exampleImagePaths = exampleImagePaths.Select(file => file.Split('\\')[^1]).ToArray();
            string[] actualImages = Directory.GetFiles(DataManager.ImageFolderPath).Select(file => file.Split('\\')[^1]).ToArray();

            Assert.IsTrue(exampleImagePaths.Length > 0);
            Assert.IsTrue(actualImages.Length > 0);
            CollectionAssert.AreEquivalent(exampleImagePaths, actualImages);
        }
    }
}
