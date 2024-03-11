using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.HeaderValidators.Tests.Business
{
    [TestClass]
    public class EntityNameProviderTests
    {
        private MockRepository _MockRepository;

        private Mock<IAppSettings> _MockAppSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAppSettings = _MockRepository.Create<IAppSettings>();
        }

        private EntityNameProvider CreateProvider()
        {
            return new EntityNameProvider(
                _MockAppSettings.Object);
        }

        #region Provide
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void Provider_Provide_absolutePath_NullEmptyOrWhitespace_Throws(string absolutePath)
        {
            // Arrange
            var provider = CreateProvider();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                provider.Provide(absolutePath);
            }, $"'{nameof(absolutePath)}' cannot be null or whitespace.", nameof(absolutePath));
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList(
            "/Entity1Service.svc/SomeEndpoint",
            "/Entity1Service/SomeEndpointWithSvc",
            "/Entity1Service.svc/SomeEndpoint/With/Multiple/Slashes",
            "/Entity1Service/SomeEndpointWithSvc/With/Multiple/Slashes"
            )]
        public void Provider_Provide_Valid_Urls_NoSubPath_Works(string absolutePath)
        {
            // Arrange
            var provider = CreateProvider();
            var nvc = new NameValueCollection();
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            // Act
            var result = provider.Provide(absolutePath);

            // Assert
            Assert.AreEqual("Entity1", result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList(
            "/Api/Entity1Service.svc/SomeEndpoint",
            "/Api/Entity1Service/SomeEndpointWithSvc",
            "/Api/Entity1Service.svc/SomeEndpoint/With/Multiple/Slashes",
            "/Api/Entity1Service/SomeEndpointWithSvc/With/Multiple/Slashes"
            )]
        public void Provider_Provide_Valid_Urls_WithSubPath_Works(string absolutePath)
        {
            // Arrange
            var provider = CreateProvider();
            var nvc = new NameValueCollection
            {
                { "EntityWebHost", "" },
                { "EntitySubPath", "Api" }
            };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            // Act
            var result = provider.Provide(absolutePath);

            // Assert
            Assert.AreEqual("Entity1", result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList(
            "/Api/Entity1Service.svc/SomeEndpoint",
            "/Api/Entity1Service/SomeEndpointWithSvc",
            "/Api/Entity1Service.svc/SomeEndpoint/With/Multiple/Slashes",
            "/Api/Entity1Service/SomeEndpointWithSvc/With/Multiple/Slashes"
            )]                  
        public void Provider_Provide_Valid_Urls_WithSubPath_PathsHaveSlashes_Works(string absolutePath)
        {
            // Arrange
            var provider = CreateProvider();
            var nvc = new NameValueCollection
            {
                { "EntityWebHost", "/" },
                { "EntitySubPath", "/Api/" }
            };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            // Act
            var result = provider.Provide(absolutePath);

            // Assert
            Assert.AreEqual("Entity1", result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
