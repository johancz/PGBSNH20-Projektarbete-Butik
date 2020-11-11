using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreCommon;
using System;
using System.Globalization;
using System.IO;

namespace StoreUser.Tests
{
    [TestClass()]
    public class UserViewTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Helpers.StoreDataCsvPath = Path.Combine(Helpers.StoreDataPath, ".CSVs"); // Reset StoreDataCsvPath
        }

        [TestMethod()]
        public void CreateTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void CreateProductItemTest()
        {
            throw new NotImplementedException();
        }

        public void ProductItem_MouseUpTest()
        {
            throw new NotImplementedException();
        }
    }
}
