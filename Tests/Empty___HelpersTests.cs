using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;

namespace StoreCommon.Tests
{
    //TODO(johancz) : Move to a test project?
    [TestClass()]
    public class HelpersTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Helpers.StoreDataCsvPath = Path.Combine(Helpers.StoreDataPath, ".CSVs"); // Reset StoreDataCsvPath
        }

        [TestMethod()]
        public void CreateBitmapImageFromUriStringTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void CreateNewImageTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // MethodBeingTested_Input_Output
        public void CreateNewImage_ValidUriString_Image()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // MethodBeingTested_Input_Output
        public void CreateNewImage_InValidUriString_null()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // MethodBeingTested_Input_Output
        public void CreateBitmapImageFromUriString_ValidUriString_BitmapImage()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // MethodBeingTested_Input_Output
        public void CreateBitmapImageFromUriString_InvalidUriString_null()
        {
            throw new NotImplementedException();
        }
    }
}
