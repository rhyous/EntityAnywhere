using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Behaviors;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Wrappers;
using System;
using System.Collections.Specialized;
using System.Net;

namespace Rhyous.EntityAnywhere.Behaviors.HeaderValidation.Tests.Business
{
    [TestClass]
    public class AccessControllerTests
    {
        private MockRepository _MockRepository;

        private Mock<IAppSettings> _MockAppSettings;
        private Mock<IAnonymousAllowedUrls> _MockAnonymousAllowedUrls;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAppSettings = _MockRepository.Create<IAppSettings>();
            _MockAnonymousAllowedUrls = _MockRepository.Create<IAnonymousAllowedUrls>();
        }

        public TestContext TestContext { get; set; }

        private AccessController CreateAccessController()
        {
            return new AccessController(
                _MockAppSettings.Object,
                _MockAnonymousAllowedUrls.Object);
        }

        #region IsAnonymousAllowed
        [TestMethod]
        [TestCategory("DataDriven")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"Data\IsAnonymousData.csv", "IsAnonymousData#csv", DataAccessMethod.Sequential)]
        public void IsAnonymousAllowed_TestAllPaths_HappyAndSadPaths()
        {
            // Arrange
            var accessController =  new AccessController(_MockAppSettings.Object, new AnonymousAllowedUrls());
            var nvcAppSettings = new NameValueCollection();
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);

            string testData = TestContext.DataRow[0].ToString();
            bool expected = Convert.ToBoolean(TestContext.DataRow[1]);

            // Act
            var result = accessController.IsAnonymousAllowed(testData);

            // Assert
            Assert.AreEqual(result, expected);

        }
        #endregion

        #region IsSystemAdmin
        [TestMethod]
        public void IsSystemAdmin_EntityAdminTokenAndHeaderIsValid_ReturnsTrue()
        {
            // Arrange
            var nvcAppSettings = new NameValueCollection { { "EntityAdminToken", "abc12345" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);
            var nvcHeaders = new NameValueCollection { { "EntityAdminToken", "abc12345" } };
            var accessController = CreateAccessController();
            var incomingRequestMock = _MockRepository.Create<IIncomingWebRequestContext>();
            incomingRequestMock.Setup(m => m.Headers).Returns(new WebHeaderCollection());
            var contextMock = _MockRepository.Create<IWebOperationContext>();
            contextMock.Setup(m => m.IncomingRequest).Returns(incomingRequestMock.Object);

            // Act
            var result = accessController.IsSystemAdmin(nvcHeaders, contextMock.Object);

            // Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void IsSystemAdmin_NoEntityAdminTokenAndHeaderIsValid_ReturnsFalse()
        {
            // Arrange
            var nvcAppSettings = new NameValueCollection { { "EntityAdminToken", "abc12345" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);
            var nvcHeaders = new NameValueCollection { { "EntityAdminToken", "invalid" } };
            var accessController = CreateAccessController();
            var contextMock = _MockRepository.Create<IWebOperationContext>();

            // Act
            var result = accessController.IsSystemAdmin(nvcHeaders, contextMock.Object);


            // Assert
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void IsSystemAdmin_EntityAdminTokenAndHeaderIsNotValid_ReturnsFalse()
        {
            // Arrange
            var nvcAppSettings = new NameValueCollection { { "EntityAdminToken", "abc12345" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);
            var nvcHeaders = new NameValueCollection { { "", "" } };
            var accessController = CreateAccessController();
            var contextMock = _MockRepository.Create<IWebOperationContext>();

            // Act
            var result = accessController.IsSystemAdmin(nvcHeaders, contextMock.Object);


            // Assert
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void IsSystemAdmin_WebContextValid_ReturnsTrue()
        {
            // Arrange
            var nvcAppSettings = new NameValueCollection { { "EntityAdminToken", "abc12345" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);
            var nvcHeaders = new NameValueCollection { { "EntityAdminToken", "abc12345" } };
            var accessController = CreateAccessController();
            var contextMock = _MockRepository.Create<IWebOperationContext>();
            var incomingWebResponseContextMock = _MockRepository.Create<IIncomingWebRequestContext>();
            contextMock.Setup(x => x.IncomingRequest).Returns(incomingWebResponseContextMock.Object);
            incomingWebResponseContextMock.Setup(x => x.Headers).Returns(new System.Net.WebHeaderCollection());

            // Act
            var result = accessController.IsSystemAdmin(nvcHeaders, contextMock.Object);

            // Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void IsSystemAdmin_WebContextValidNullIncomingRequest_ReturnsTrue()
        {
            // Arrange
            var nvcAppSettings = new NameValueCollection { { "EntityAdminToken", "abc12345" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);
            var nvcHeaders = new NameValueCollection { { "EntityAdminToken", "abc12345" } };
            var accessController = CreateAccessController();
            var incomingRequestMock = _MockRepository.Create<IIncomingWebRequestContext>();
            incomingRequestMock.Setup(m => m.Headers).Returns(new WebHeaderCollection());
            var contextMock = _MockRepository.Create<IWebOperationContext>();
            contextMock.Setup(m => m.IncomingRequest).Returns(incomingRequestMock.Object);

            // Act
            var result = accessController.IsSystemAdmin(nvcHeaders, contextMock.Object);

            // Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void IsSystemAdmin_WebContextValidNullHeaders_ReturnsTrue()
        {
            // Arrange
            var nvcAppSettings = new NameValueCollection { { "EntityAdminToken", "abc12345" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);
            var nvcHeaders = new NameValueCollection { { "EntityAdminToken", "abc12345" } };
            var accessController = CreateAccessController();
            var incomingRequestMock = _MockRepository.Create<IIncomingWebRequestContext>();
            incomingRequestMock.Setup(m => m.Headers).Returns(new WebHeaderCollection());
            var contextMock = _MockRepository.Create<IWebOperationContext>();
            contextMock.Setup(m => m.IncomingRequest).Returns(incomingRequestMock.Object);

            // Act
            var result = accessController.IsSystemAdmin(nvcHeaders, contextMock.Object);

            // Assert
            Assert.AreEqual(result, true);
        }
        #endregion
    }
}
