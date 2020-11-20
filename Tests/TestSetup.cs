using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using StoreCommon;

namespace Store.Tests
{
    public static class TestSetup
    {
        private static bool _initiated = false;
        public static string TestDataPath;
        public static string TestOutputPath;

        /* 
         * To avoid cluttering the program's save folder in the system's temp folder 
         * (e.g. "...\AppData\Local\Temp\Fight Club & Veggies - JohanC RobinA\") with test files,
         * tests do not save edited ".csv"-files (e.g. admin adds a new product) to to the same folder.
         * They instead save to e.g. "...\AppData\Local\Temp\Fight Club & Veggies - JohanC RobinA__TESTS\
         *                                                                          difference = ^^^^^^
         * This method copies the correct .csv file for tests to the latter folder.
         */
        public static void Init()
        {
            TestDataPath = Path.Combine(Environment.CurrentDirectory, "TestData");
            TestOutputPath = Path.Combine(Path.GetTempPath(), DataManager.ProjectName + "__TESTS");
            _initiated = true;
        }

        public static void Cleanup()
        {
            if (Directory.Exists(TestOutputPath) && new DirectoryInfo(TestOutputPath).Parent == new DirectoryInfo(Path.GetTempPath()))
            {
                Directory.Delete(TestOutputPath, true);
            }
        }

        /// <summary>
        /// <p
        /// </summary>
        /// <param name="testDataFolder">
        ///     The name of the folder in "\Tests\TestData\" (project source folder) which contains the necessary test-files.
        /// </param>
        public static void CopyTestFiles(string testDataFolder)
        {
            if (!_initiated)
            {
                throw new Exception("Run TestSetup.Init() first.");
            }

            if (testDataFolder == null || testDataFolder == "")
            {
                throw new ArgumentException("'testDataFolder cannot be null or an empty string.");
            }

            // First load the real example files used by the program to the test's own folder in the system's "Temp"-folder.
            // AppFolder.AppFolder() expects all ".csv"-files and image to exist, and the testdata-folder does not include all files,
            // which necessitates this step.
            DataManager.SetPaths(null, TestOutputPath);

            string PathTestCSVFiles = Path.Combine(TestDataPath, testDataFolder);
            var TestCSVFiles = new DirectoryInfo(PathTestCSVFiles);
            var files = TestCSVFiles.GetFiles();

            // Create the '\TestData\.csvFiles\' folder structure in the programs's save folder in the system's "Temp"-folder.
            // E.g.: "C:\Users\**user**\AppData\Local\Temp\Fight Club & Veggies - JohanC RobinA\TestData\.csvFiles\"
            //var outputTestDataCSVFolder = Directory.CreateDirectory(Path.Combine(AppFolder.RootFolderPath, "TestData", ".csvFiles"));

            // Get the paths for all .csv file from "\TestData\.csvFiles\" in
            // the program's "run directory" (E.g.: "...\ProjectDir\Tests\bin\Debug\netcoreapp3.1\".
            string[] pathSourceTestDataCSVFiles = Directory.GetFiles(PathTestCSVFiles, "*.csv");
            foreach (string csvFilePath in pathSourceTestDataCSVFiles)
            {
                // Copy all ".csv"-files to the folder in the system's "Temp"-folder. Always overwrite.
                File.Copy(csvFilePath, Path.Combine(DataManager.RootFolderPath, Path.GetFileName(csvFilePath).Substring("Example".Length)), true);
            }
        }
    }
}
