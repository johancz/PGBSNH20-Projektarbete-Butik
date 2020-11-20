using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.IO;
using StoreCommon;
using System.Linq;

namespace StoreAdmin.Tests
{
    [TestClass]
    public class IOTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            DataManager.StoreDataCsvPath = Path.Combine(DataManager.RootFolderPath, ".CSVs"); // Reset StoreDataCsvPath
        }

        [TestMethod]
        public void HardcodedPathsAndFilesExist()
        {
            Assert.IsFalse(
                Directory.Exists(Path.Combine(DataManager.StoreDataCsvPath, "folderThatDoesNotExist")),
                "\"StoreData\\folderThatDoesNotExist\\\"-folder does not exist");
            // Check if the folders exist in the "output directory"\ and "output directory"\StoreData\
            Assert.IsTrue(Directory.Exists(DataManager.StoreDataCsvPath), "\"StoreData\\\"-folder could not be found in the output directory.");
            Assert.IsTrue(Directory.Exists(DataManager.InputImages), "\"StoreData\\Images\\\"-folder could not be found in the output directory.");
            // TODO(johancz): Add Asserts for "temp"-folder and its files 
            // Check if the images exist in "the output directory"\StoreData\
            Assert.IsTrue(File.Exists(DataManager.ImageFolderPath), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.StoreDataCsvPath, "ExampleProducts.csv")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.StoreDataCsvPath, "ExampleShoppingCart.csv")), "TODO(johancz)");
            // Check if the images exist in "the output directory"\StoreData\Images\
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "banana.jpg")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "broccoli.jpg")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "Fight Club Brad Pitt NoteBook.png")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "Fight Club Pin.png")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "Fight Club Poster.png")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "orange.jpg")), "TODO(johancz)");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "Tyler Sticker.png")), "TODO(johancz)");

            // TODO(johancz): Add Asserts for "temp"-folder and its files 
            string[] exampleImagePaths = Directory.EnumerateFiles(DataManager.InputImages).Where(file =>
            {
                return new[] { ".jpg", ".jpeg", ".png" }.Any(file.ToLower().EndsWith);
            }).ToArray();
            exampleImagePaths = exampleImagePaths.Select(file => file.Split('\\')[^1]).ToArray();
            string[] actualImages = Directory.GetFiles(DataManager.ImageFolderPath).Select(file => file.Split('\\')[^1]).ToArray();

            Assert.IsTrue(exampleImagePaths.Length > 0);
            Assert.IsTrue(actualImages.Length > 0);
            CollectionAssert.AreEquivalent(exampleImagePaths, actualImages);
        }
    }
}
