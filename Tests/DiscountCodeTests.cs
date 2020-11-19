using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;

namespace StoreCommon.Tests
{
    [TestClass]
    public class DiscountCodeTests
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Helpers.StoreDataCsvPath = Path.Combine(Helpers.StoreDataPath, ".CSVs"); // Reset StoreDataCsvPath
        }

        [TestMethod]
        public void DiscountCode_AllParamsAreValid_ValidDiscountCode()
        {
            var discountCode = new DiscountCode("abc", 0.0001);
            Assert.AreEqual("abc", discountCode.Code);
            Assert.AreEqual(0.0001, discountCode.Percentage);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DiscountCode_CodeParamIsNull_ArgumentNullException()
        {
            var discountCode = new DiscountCode(null, 0.0001);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_CodeParamNotValidEmptyString_ArgumentException()
        {
            var discountCode = new DiscountCode("", 0.0001);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_CodeParamNotValidTooLong_ArgumentException()
        {
            // 21 character long string
            var discountCode = new DiscountCode("abcdefghijklmnopqrstu", 0.1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_PercentageParamValueTooSmall_ArgumentException()
        {
            var discountCode = new DiscountCode("a", 0.0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_PercentageParamValueTooBig_ArgumentException()
        {
            var discountCode = new DiscountCode("a", 1.0001);
        }
    }
}
