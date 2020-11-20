using Microsoft.VisualStudio.TestTools.UnitTesting;
using Store.Tests;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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
            TestSetup.Cleanup();
        }

        [TestMethod]
        public void ProductLoadAll_NameInstances()
        {
            TestSetup.CopyTestFiles("TestLoadProducts");

            Store.LoadProducts(Path.Combine(DataManager.RootFolderPath, "Products.csv"));
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
            TestSetup.CopyTestFiles("TestLoadProducts");

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
            TestSetup.CopyTestFiles("TestLoadProducts");

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
            TestSetup.CopyTestFiles("TestLoadProducts");

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
        public void ProductLoadAll_Only_JPG_PNG()
        {
            TestSetup.CopyTestFiles("TestLoadProducts");

            Store.LoadProducts(Path.Combine(DataManager.RootFolderPath, "Products.csv"));
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
            TestSetup.CopyTestFiles("StoreTests_LoadDiscountCodes");

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
            TestSetup.CopyTestFiles("StoreTests_LoadDiscountCodes");

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
