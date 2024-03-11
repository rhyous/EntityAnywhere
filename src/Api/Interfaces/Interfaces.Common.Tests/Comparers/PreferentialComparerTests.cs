using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    [TestClass]
    public class PreferentialComparerTests
    {
        [TestMethod]
        public void PreferentialComparerTest()
        {
            // Arrange
            var list = new List<string> { "CreatedBy", "CreateDate", "Id", "LastUpdated", "LastUpdatedBy", "Name", "Enabled" };
            var expectedOrderedList = new List<string> { "Id", "Name", "Enabled", "CreateDate", "CreatedBy", "LastUpdated", "LastUpdatedBy" };

            // Act
            var actual = list.OrderBy(l => l, PreferentialPropertyComparer.Instance).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedOrderedList, actual);
        }

        [TestMethod]
        public void PreferentialComparer_Dispreferred_Order_Test()
        {
            // Arrange
            var list = new List<string> { "CreatedBy", "CreateDate", "LastUpdated", "LastUpdatedBy" };
            var expectedOrderedList = new List<string> { "CreateDate", "CreatedBy", "LastUpdated", "LastUpdatedBy" };

            // Act
            var actual = list.OrderBy(l => l, PreferentialPropertyComparer.Instance).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedOrderedList, actual);
        }
    }
}