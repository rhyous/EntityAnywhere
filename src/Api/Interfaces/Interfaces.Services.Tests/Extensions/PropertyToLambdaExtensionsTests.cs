using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Extensions
{
    [TestClass]
    public class PropertyToLambdaExtensionsTests
    {
        #region GetOrderByExpression
        [TestMethod]
        public void PropertyToLambdaExtensions_GetOrderByExpression_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string orderBy = "Id";
            PropertyInfo propInfo = typeof(EntityInt).GetProperty(orderBy);

            // Act
            var result = orderBy.GetOrderByExpression<EntityInt>(propInfo);

            // Assert
            Assert.AreEqual("Param_0 => Param_0.Id", result.ToString());
        }
        #endregion
    }
}