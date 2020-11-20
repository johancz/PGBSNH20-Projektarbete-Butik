using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.IO;
using StoreCommon;
using System.Linq;
using Store.Tests;

namespace StoreAdmin.Tests
{
    [TestClass]
    public class IOTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            TestSetup.Init();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TestSetup.Cleanup();
        }

        [TestMethod]
        public void HardcodedPathsAndFilesExist()
        {
            TestSetup.CopyTestFiles("HardcodedPathsAndFilesExist");

            // Sanity check:
            Assert.IsFalse(
                Directory.Exists(Path.Combine(DataManager.StoreDataCsvPath, "folderThatDoesNotExist")),
                "\"StoreData\\folderThatDoesNotExist\\\"-folder does not exist");
            // Check if the folders exist in the "output directory"\ and "output directory"\StoreData\
            Assert.IsTrue(Directory.Exists(DataManager.StoreDataCsvPath), "\"StoreData\\\"-folder could not be found in the output directory.");
            Assert.IsTrue(Directory.Exists(DataManager.InputImages), "\"StoreData\\Images\\\"-folder could not be found in the output directory.");
            // Make sure the ".csv"-files exist in the "source"-directory "\StoreData\.CSVs\".
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.StoreDataCsvPath, "ExampleProducts.csv")), "Can't find .csv-file.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.StoreDataCsvPath, "ExampleShoppingCart.csv")), "Can't find .csv-file.");
            // Make sure the images exist in the "source"-directory "\StoreData\Images\".
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "banana.jpg")), "Can't find image.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "broccoli.jpg")), "Can't find image.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "Fight Club Brad Pitt NoteBook.png")), "Can't find image.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "Fight Club Pin.png")), "Can't find image.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "Fight Club Poster.png")), "Can't find image.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "orange.jpg")), "Can't find image.");
            Assert.IsTrue(File.Exists(Path.Combine(DataManager.InputImages, "Tyler Sticker.png")), "Can't find image.");

            // Make sure the images and ".csv"-files were copied successfully to the "Temp"-folder.
            Assert.IsTrue(Directory.Exists(DataManager.ImageFolderPath), "Directory doesn't exist.");
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
