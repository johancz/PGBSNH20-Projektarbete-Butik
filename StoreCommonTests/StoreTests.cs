using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Helpers.StoreDataCsvPath = Path.Combine(Helpers.StoreDataPath, ".CSVs"); // Reset StoreDataCsvPath
        }

        [TestMethod]
        public void ProductLoadAll_NameInstances()
        {
            var products = new List<Product>();
            Store.LoadProducts(Path.Combine(Helpers.StoreDataCsvPath, "TestProducts_Clean_5instances.csv"), out products);
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
            var products = new List<Product>();
            Store.LoadProducts(Path.Combine(Helpers.StoreDataCsvPath, "TestProducts_Clean_5instances.csv"), out products);
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
            var products = new List<Product>();
            Store.LoadProducts(Path.Combine(Helpers.StoreDataCsvPath, "TestProducts_Clean_5instances.csv"), out products);
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
            var products = new List<Product>();
            Store.LoadProducts(Path.Combine(Helpers.StoreDataCsvPath, "TestProducts_Clean_5instances.csv"), out products);
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
            var products = new List<Product>();
            Store.LoadProducts(Path.Combine(Helpers.StoreDataCsvPath, "TestProducts_Clean_5instances.csv"), out products);
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
            // The contents of the test file (ExampleDiscountCodes.csv):
            // Gimme-free-stuff;1
            // Half-Off;0.5

            // Set the StoreDatePath so that this test's test files are used instead of the the actual files.
            Helpers.StoreDataCsvPath = Path.Combine(Helpers.StoreDataPath, "TestFiles", "StoreTests_LoadDiscountCodes", "csvFiles");
            Store.LoadDiscountCodes(Helpers.StoreDataCsvPath);

            var expectedDiscountCodes = new List<DiscountCode>
            {
                new DiscountCode(code: "Gimme-free-stuff", percentage: 1),
                new DiscountCode(code: "Half-Off", percentage: 0.5),
            };

            var expectedCodes = expectedDiscountCodes.Select(discountCode => discountCode.Code).ToArray();
            var expectedPercentages = expectedDiscountCodes.Select(discountCode => discountCode.Percentage).ToArray();
            var actualCodes = Store.DiscountCodes.Select(discountCode => discountCode.Code).ToArray();
            var actualPercentages = Store.DiscountCodes.Select(discountCode => discountCode.Percentage).ToArray();

            CollectionAssert.AreEqual(expectedCodes, actualCodes);
            CollectionAssert.AreEqual(expectedPercentages, actualPercentages);
        }
    }
}
