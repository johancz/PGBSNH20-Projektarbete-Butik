using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace StoreCommon.Tests
{
    //TODO(johancz) : Move to a test project?
    [TestClass()]
    public class HelpersTests
    {
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
