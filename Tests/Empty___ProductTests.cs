using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;

namespace StoreCommon.Tests
{
    [TestClass()]
    public class Empty___ProductTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Helpers.StoreDataCsvPath = Path.Combine(Helpers.StoreDataPath, ".CSVs"); // Reset StoreDataCsvPath
        }

        [TestMethod()]
        public void ProductTest()
        {
            throw new NotImplementedException();
        }
    }
}
