using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.HeaderValidators;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Specialized;

namespace Interfaces.Common.Tests.Business
{
    [TestClass]
    public class PathNormalizerTests
    {
        private MockRepository _MockRepository;

        private Mock<IAppSettings> _MockAppSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAppSettings = _MockRepository.Create<IAppSettings>();
        }

        private PathNormalizer CreatePathNormalizer()
        {
            return new PathNormalizer(
                _MockAppSettings.Object);
        }

        #region Normalize
        [TestMethod]
        public void PathNormalizer_Normalize_RemovesSvcAndBeginningFolder_Test()
        {
            // Arrange
            var pathNormalizer = CreatePathNormalizer();
            string path = "/LicenseGeneratorService.svc/GetLicenseByOrder";
            var nvcAppSettings = new NameValueCollection();
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);

            // Act
            var result = pathNormalizer.Normalize(path);

            // Assert
            Assert.AreEqual("/LicenseGeneratorService/GetLicenseByOrder", result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PathNormalizer_Normalize_RemovesSvcAndBeginningFolderFromAppSettings_Test()
        {
            // Arrange
            var pathNormalizer = CreatePathNormalizer();
            string path = "/SomeFolder/SomeService.svc/SomeAction";
            var nvcAppSettings = new NameValueCollection { { "EntitySubpath", "SomeFolder" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);

            // Act
            var result = pathNormalizer.Normalize(path);

            // Assert
            Assert.AreEqual("/SomeService/SomeAction", result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
