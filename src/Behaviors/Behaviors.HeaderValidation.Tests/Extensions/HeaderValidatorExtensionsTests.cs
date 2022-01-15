using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Behaviors;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Behaviors.HeaderValidation.Tests.Business
{
    [TestClass]
    public class HeaderValidatorExtensionsTests
    {
        [TestMethod]
        public void PluginHeaderValidator_ValidatorSupportsHeaders_NullIsTrue_Test()
        {
            // Arrange
            var mockValidator = new Mock<IHeaderValidator>();
            mockValidator.Setup(m => m.Headers).Returns((IList<string>)null);

            // Act
            var actual = mockValidator.Object.CanValidateHeaders(null);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void PluginHeaderValidator_ValidatorSupportsHeaders_EmptyIsTrue_Test()
        {
            // Arrange
            var mockValidator = new Mock<IHeaderValidator>();
            mockValidator.Setup(m => m.Headers).Returns(new List<string>());

            // Act
            var actual = mockValidator.Object.CanValidateHeaders(null);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void PluginHeaderValidator_ValidatorSupportsHeaders_MatchingHeaderIsTrue_Test()
        {
            // Arrange
            var mockValidator = new Mock<IHeaderValidator>();
            mockValidator.Setup(m => m.Headers).Returns(new List<string> { "Header1" });

            // Act
            var actual = mockValidator.Object.CanValidateHeaders(new[] { "Header1" });

            // Assert
            Assert.IsTrue(actual);
        }


        [TestMethod]
        public void PluginHeaderValidator_ValidatorSupportsHeaders_MatchingHeaderCaseInsensitiveIsTrue_Test()
        {
            // Arrange
            var mockValidator = new Mock<IHeaderValidator>();
            mockValidator.Setup(m => m.Headers).Returns(new List<string> { "Header1" });

            // Act
            var actual = mockValidator.Object.CanValidateHeaders(new[] { "header1" });

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void PluginHeaderValidator_ValidatorSupportsHeaders_NoMatchingHeaderIsFalse_Test()
        {
            // Arrange
            var mockValidator = new Mock<IHeaderValidator>();
            mockValidator.Setup(m => m.Headers).Returns(new List<string> { "Header1" });

            // Act
            var actual = mockValidator.Object.CanValidateHeaders(new[] { "Header2" });

            // Assert
            Assert.IsFalse(actual);
        }
    }

}
