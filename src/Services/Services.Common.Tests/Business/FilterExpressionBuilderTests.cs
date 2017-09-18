using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Business
{
    [TestClass]
    public class FilterExpressionBuilderTests
    {
        public class Entity1 : IEntity<int>
        {
            public int Id { get; set; }
        }

        [TestMethod]
        public void OneEqualsTest()
        {
            // Arrange
            var filterstring = "Id eq 1";
            var expected = "e => e.Id.Equals(1)";
            var builder = new FilterExpressionBuilder<Entity1>(filterstring);

            // Act
            var actual = builder.Expression.ToString();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
