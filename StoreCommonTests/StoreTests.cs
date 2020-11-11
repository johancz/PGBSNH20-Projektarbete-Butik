using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace StoreCommon.Tests
{
    [TestClass]
    public class StoreTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [TestMethod()]
        public void StoreDataPathsAndFilesExist()
        {
            // Sanity check.
            Assert.IsFalse(
                Directory.Exists(Path.Combine(Helpers.StoreDataCsvPath, "folderThatDoesNotExist")),
                "\"StoreData\\folderThatDoesNotExist\\\"-folder does not exist");
            // Check if the folders exist in the "output directory"\ and "output directory"\StoreData\
            Assert.IsTrue(Directory.Exists(Helpers.StoreDataCsvPath), "\"StoreData\\\"-folder could not be found in the output directory.");
            Assert.IsTrue(Directory.Exists(Helpers.StoreDataImagesPath), "\"StoreData\\Images\\\"-folder could not be found in the output directory.");
            // Check if the images exist in "the output directory"\StoreData\
            Assert.IsTrue(File.Exists(Path.Combine(Helpers.StoreDataCsvPath, "ExampleDiscountCodes.csv")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(Helpers.StoreDataCsvPath, "ExampleProducts.csv")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(Helpers.StoreDataCsvPath, "ExampleShoppingCart.csv")), "TODO(johancz)");
            // Check if the images exist in "the output directory"\StoreData\Images\
            Assert.IsTrue(File.Exists(Path.Combine(Helpers.StoreDataImagesPath, "banana.jpg")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(Helpers.StoreDataImagesPath, "broccoli.jpg")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(Helpers.StoreDataImagesPath, "Fight Club Brad Pitt NoteBook.png")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(Helpers.StoreDataImagesPath, "Fight Club Pin.png")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(Helpers.StoreDataImagesPath, "Fight Club Poster.png")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(Helpers.StoreDataImagesPath, "orange.jpg")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(Helpers.StoreDataImagesPath, "Tyler Sticker.png")), "TODO(johancz)");
        }

        [TestMethod]
        public void ProductLoadAll_NameInstances()
        {
            var products = Store.LoadProducts(Path.Combine(Helpers.StoreDataCsvPath, "TestProducts_Clean_5instances.csv"));
            var nameListActual = new List<string>();

            foreach (var p in products)
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
            var products = Store.LoadProducts(Path.Combine(Helpers.StoreDataCsvPath, "TestProducts_Clean_5instances.csv"));
            var charList = new List<char>();

            foreach (var p in products)
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
            var products = Store.LoadProducts(Path.Combine(Helpers.StoreDataCsvPath, "TestProducts_Clean_5instances.csv"));
            var charList = new List<char>();

            foreach (var p in products)
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
            var products = Store.LoadProducts(Path.Combine(Helpers.StoreDataCsvPath, "TestProducts_Clean_5instances.csv"));
            var charList = new List<char>();

            foreach (var p in products)
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
            var products = Store.LoadProducts(Path.Combine(Helpers.StoreDataCsvPath, "TestProducts_Clean_5instances.csv"));
            var fileExtensions = new List<string>();
            string extension = "";

            foreach (var p in products)
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
            Helpers.StoreDataCsvPath = Path.Combine(Helpers.StoreDataPath, "TestFiles", "csvFiles");
            //Helpers.StoreDataTemporaryOutputPath = Path.Combine(Helpers.StoreDataPath, "TestFiles", "csvFiles");

            Store.LoadDiscountCodes();
        }
    }
}
