using Microsoft.VisualStudio.TestTools.UnitTesting;
using Butik_User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Butik_User.Tests
{
    [TestClass]
    public class DiscountCodeTests
    {
        // TODO(johancz): unnecessary test? remove?
        [TestMethod]
        public void DiscountCode_AllParamsAreValidNoExpiry_ValidDiscountCode()
        {
            var discountCode = new DiscountCode("a", 0.0001, null);
            Assert.AreEqual("a", discountCode.Code);
            Assert.AreEqual(0.0001, discountCode.Percentage);
            Assert.AreEqual(null, discountCode.Expires);
        }

        // TODO(johancz): unnecessary test? remove?
        [TestMethod]
        public void DiscountCode_AllParamsAreValidWithExpiryNotExpired_ValidDiscountCode()
        {
            var now = DateTime.Now;
            var discountCode = new DiscountCode("a", 0.0001, now.AddYears(2));
            Assert.AreEqual("a", discountCode.Code);
            Assert.AreEqual(0.0001, discountCode.Percentage);
            Assert.AreEqual(now.AddYears(2), discountCode.Expires);
        }

        // TODO(johancz): unnecessary test? remove?
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DiscountCode_CodeParamIsNull_ArgumentNullException()
        {
            var discountCode = new DiscountCode(null, 0.0001, null);
        }

        // TODO(johancz): unnecessary test? remove?
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_CodeParamNotValidEmptyString_ArgumentException()
        {
            var discountCode = new DiscountCode("", 0.0001, null);
        }

        // TODO(johancz): unnecessary test? remove?
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_CodeParamNotValidTooLong_ArgumentException()
        {
            // 101 character long string
            var discountCode = new DiscountCode("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLM", 0.1, null);
        }

        // TODO(johancz): unnecessary test? remove?
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_PercentageParamValueTooSmall_ArgumentException()
        {
            var discountCode = new DiscountCode("a", 0.0, null);
        }

        // TODO(johancz): unnecessary test? remove?
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_PercentageParamValueTooBig_ArgumentException()
        {
            var discountCode = new DiscountCode("a", 1.0001, null);
        }

        // TODO(johancz): unnecessary test? remove?
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DiscountCode_DateTimeParamValueTooSmall_ArgumentException()
        {
            var discountCode = new DiscountCode("a", 1.0, DateTime.Now.AddSeconds(-1));
        }
    }
}