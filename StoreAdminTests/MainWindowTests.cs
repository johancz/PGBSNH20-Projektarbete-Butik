using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreCommon;
using System;
using System.Globalization;
using System.IO;

namespace StoreAdmin.Tests
{
    // TODO(johancz): Den här klassen kanske inte skall testat, så den här filen är mer av en placeholder
    [TestClass()]
    public class MainWindowTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Helpers.StoreDataCsvPath = Path.Combine(Helpers.StoreDataPath, ".CSVs"); // Reset StoreDataCsvPath
        }

        [TestMethod()]
        public void MainWindowTest()
        {
            throw new NotImplementedException();
        }
    }
}
