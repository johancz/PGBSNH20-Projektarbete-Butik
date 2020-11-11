using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;

namespace StoreCommon.Tests
{
    [TestClass()]
    public class ProductListTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Helpers.StoreDataCsvPath = Path.Combine(Helpers.StoreDataPath, ".CSVs"); // Reset StoreDataCsvPath
        }

        [TestMethod()]
        public void AddProductTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void RemoveProductTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void SetDiscountCodeTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void RemoveDiscountCodeTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void SaveToFileTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void LoadFromFileTest()
        {
            throw new NotImplementedException();
        }
    }
}
