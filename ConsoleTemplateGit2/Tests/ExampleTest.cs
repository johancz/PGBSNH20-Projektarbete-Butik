using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTemplateGit2
{
    [TestClass]

    public class ExampleTest
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [TestMethod]
        public void DivideByZero()
        {
            try
            {
                int zero = 0;
                int result = 5 / zero;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Attempted to divide by zero.");
            }
        }
        public void IndexOutOfBounce()
        {
            try
            {
                int zero = 0;
                int result = 5 / zero;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Index was outside the bounds of the array.");
            }
        }
    }
}
