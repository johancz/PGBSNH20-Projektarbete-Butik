using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.IO;

namespace StoreCommon.Tests
{
    [TestClass]
    public class IOTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Helpers.StoreDataCsvPath = Path.Combine(Helpers.StoreDataPath, ".CSVs"); // Reset StoreDataCsvPath
        }

        [TestMethod]
        public void HardcodedPathsAndFilesExist()
        {
            Assert.IsFalse(
                Directory.Exists(Path.Combine(Helpers.StoreDataCsvPath, "folderThatDoesNotExist")),
                "\"StoreData\\folderThatDoesNotExist\\\"-folder does not exist");
            // Check if the folders exist in the "output directory"\ and "output directory"\StoreData\
            Assert.IsTrue(Directory.Exists(Helpers.StoreDataCsvPath), "\"StoreData\\\"-folder could not be found in the output directory.");
            Assert.IsTrue(Directory.Exists(Helpers.StoreDataImagesPath), "\"StoreData\\Images\\\"-folder could not be found in the output directory.");
            // TODO(johancz): Add Asserts for "temp"-folder and its files 
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

            // TODO(johancz): Add Asserts for "temp"-folder and its files 
        }
    }
}
