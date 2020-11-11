﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void MainWindowTest()
        {
            throw new NotImplementedException();
        }
    }
}
