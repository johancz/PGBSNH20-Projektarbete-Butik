using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreCommon;
using System.Collections.Generic;
using System.Globalization;
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
        }

        [TestMethod]
        public void ProductLoadAll_NameInstances()
        {
            var products = Store.LoadProducts("TestProducts_Clean_5instances.csv");
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
            var products = Store.LoadProducts("TestProducts_Clean_5instances.csv");
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
            var products = Store.LoadProducts("TestProducts_Clean_5instances.csv");
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
            var products = Store.LoadProducts("TestProducts_Clean_5instances.csv");
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
            var products = Store.LoadProducts("TestProducts_Clean_5instances.csv");
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
    }
}
