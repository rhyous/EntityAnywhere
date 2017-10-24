using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Rhyous.WebFramework.Behaviors;

namespace Behaviors.CustomDispatchMessageFormatter.Tests.Business
{
    [TestClass]
    public class PreferentialComparerTests
    {
        [TestMethod]
        public void PreferentialComparerTest()
        {
            // Arrange
            var list = new List<string> { "CreatedBy", "CreateDate", "Id", "LastUpdated", "LastUpdatedBy", "Name", "Enabled" };
            var expectedOrderedList = new List<string> { "Id", "Name","CreateDate", "CreatedBy", "Enabled", "LastUpdated", "LastUpdatedBy" };

            // Act
            var actual = list.OrderBy(l => l, new PreferentialComparer());

            // Assert
            Assert.IsTrue(expectedOrderedList.SequenceEqual(actual));

        }
    }
}
