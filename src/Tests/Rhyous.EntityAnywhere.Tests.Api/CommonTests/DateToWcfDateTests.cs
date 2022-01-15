using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    [TestClass]
    public class DateToWcfDateTests
    {
        [TestMethod]
        public void DateToWcfDateTest()
        {
            // Arrange
            var date = new DateTime(2020,1,2);
            var expected = @"\/Date(1577948400000-0700)\/";

            // Act
            var actual = date.GetWcfDate();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetWcfDateNoEscapeTest()
        {
            // Arrange
            var date = new DateTime(2020, 1, 2);
            var expected = @"/Date(1577948400000-0700)/";

            // Act
            var actual = date.GetWcfDateNoEscape();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
