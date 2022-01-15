using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Behaviors;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Behaviors.HeaderValidation.Tests.Business
{
    [TestClass]
    public class PluginHeaderValidatorTests
    {
        private MockRepository _MockRepository;

        private Mock<IAppDomain> _MockAppDomain;
        private Mock<IPluginLoaderSettings> _MockPluginLoaderSettings;
        private Mock<IPluginLoaderFactory<IHeaderValidator>> _MockPluginLoaderFactory;
        private Mock<IPluginObjectCreator<IHeaderValidator>> _MockPluginObjectCreator;
        private Mock<IPluginPaths> _MockPluginPaths;
        private Mock<IPluginLoaderLogger> _MockPluginLoaderLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAppDomain = _MockRepository.Create<IAppDomain>();
            _MockPluginLoaderSettings = _MockRepository.Create<IPluginLoaderSettings>();
            _MockPluginLoaderFactory = _MockRepository.Create<IPluginLoaderFactory<IHeaderValidator>>();
            _MockPluginObjectCreator = _MockRepository.Create<IPluginObjectCreator<IHeaderValidator>>();
            _MockPluginPaths = _MockRepository.Create<IPluginPaths>();
            _MockPluginLoaderLogger = _MockRepository.Create<IPluginLoaderLogger>();
        }

        private PluginHeaderValidator CreatePluginHeaderValidator()
        {
            return new PluginHeaderValidator(
                _MockAppDomain.Object,
                _MockPluginLoaderSettings.Object,
                _MockPluginLoaderFactory.Object,
                _MockPluginObjectCreator.Object,
                _MockPluginPaths.Object,
                _MockPluginLoaderLogger.Object);
        }

        [TestMethod]
        public void PluginHeaderValidator_Valid_Test()
        {
            // Arrange
            var mockHeaderValidator = new Mock<IHeaderValidator>();
            mockHeaderValidator.Setup(m => m.Headers).Returns(new List<string> { "header1" });
            mockHeaderValidator.Setup(m => m.IsValidAsync(It.IsAny<NameValueCollection>())).ReturnsAsync(true);
            mockHeaderValidator.Setup(m => m.UserId).Returns(227);

            _MockPluginPaths.Setup(m => m.Paths).Returns(new List<string> { @"c:\path1", @"c:\path2" });

            var validator = CreatePluginHeaderValidator();
            validator.HeaderValidators = new[] { mockHeaderValidator.Object };

            var nameValueCollection = new NameValueCollection { { "Header1", "value1" } };

            // Act
            var result = validator.IsValidAsync(nameValueCollection).Result;

            // Assert
            Assert.IsTrue(result);
            mockHeaderValidator.Verify(m => m.Headers, Times.Once);
            mockHeaderValidator.Verify(m => m.IsValidAsync(It.IsAny<NameValueCollection>()), Times.Once);
            mockHeaderValidator.Verify(m => m.UserId, Times.Exactly(2));
        }

        [TestMethod]
        public void PluginHeaderValidator_NotRun_Test()
        {
            // Arrange
            var mockHeaderValidator = new Mock<IHeaderValidator>();
            mockHeaderValidator.Setup(m => m.Headers).Returns(new List<string> { "header1" });
            mockHeaderValidator.Setup(m => m.IsValidAsync(It.IsAny<NameValueCollection>())).ReturnsAsync(true);
            mockHeaderValidator.Setup(m => m.UserId).Returns(227);

            _MockPluginPaths.Setup(m=>m.Paths).Returns(new List<string> { @"c:\path1", @"c:\path2" });

            var validator = CreatePluginHeaderValidator();
            validator.HeaderValidators = new[] { mockHeaderValidator.Object };

            var nameValueCollection = new NameValueCollection { { "Header2", "value1" } };

            // Act
            var result = validator.IsValidAsync(nameValueCollection).Result;

            // Assert
            Assert.IsFalse(result);
            mockHeaderValidator.Verify(m => m.Headers, Times.Once);
            mockHeaderValidator.Verify(m => m.IsValidAsync(It.IsAny<NameValueCollection>()), Times.Never);
            mockHeaderValidator.Verify(m => m.UserId, Times.Never);
        }
    }
}
