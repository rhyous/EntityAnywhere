using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.UnitTesting;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.HeaderValidators.Tests.Business
{
    [TestClass]
    public class CustomCustomerRoleAuthorizationTests
    {
        private MockRepository _MockRepository;

        private Mock<ITokenSecurityList> _MockTokenSecurityList;
        private Mock<IPathNormalizer> _MockPathNormalizer;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockTokenSecurityList = _MockRepository.Create<ITokenSecurityList>();
            _MockPathNormalizer = _MockRepository.Create<IPathNormalizer>();
        }

        private CustomCustomerRoleAuthorization CreateCustomCustomerRoleAuthorization()
        {
            return new CustomCustomerRoleAuthorization(
                _MockTokenSecurityList.Object,
                _MockPathNormalizer.Object);
        }

        #region IsAllowedUrl
        [TestMethod]
        public void TokenHeaderValidator_IsAllowedUrl_GivenValidArguments_ReturnsTrue()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            string url = "/AddendumService.svc/Addenda";
            var httpVerb = "GET";
            var urlsDict = new Dictionary<string, IEnumerable<string>> { { url, new[] { "GET" } } };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls())
                                  .Returns(urlsDict);
            // Act
            var actual = headerValidator.IsAllowedUrl(url, httpVerb);

            // Assert
            Assert.IsTrue(actual, "Path is " + url);
        }

        [TestMethod]
        public void OAuthHeaderValidator_IsNotAllowedUrl_GivenValidArguments_ReturnsFalse()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            string testData = "/Acme.svc/SomeEndPoint";
            var httpVerb = "GET";
            var urlsDict = new Dictionary<string, IEnumerable<string>>();
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls())
                                  .Returns(urlsDict);

            // Act
            var actual = headerValidator.IsAllowedUrl(testData, httpVerb);
            // Assert
            Assert.IsFalse(actual, "Path is " + testData);
        }
        #endregion

        #region IsAuthorized

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Headers_Null_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            
            IHeadersContainer headers = null;
            var userRoleId = 10;

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_HeadersEmpty_Null_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();

            var mockHeaders = new Mock<IHeadersContainer>();
            mockHeaders.Setup(m=>m.Count).Returns(0);
            var userRoleId = 10;

            // Act
            var result = headerValidator.IsAuthorized(mockHeaders.Object, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Headers_MissingAbsolutePath_Null_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();

            var mockHeaders = new Mock<IHeadersContainer>();
            mockHeaders.Setup(m => m.Count).Returns(1);
            string absolutePath = null;
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns(absolutePath);
            var userRoleId = 10;

            // Act
            var result = headerValidator.IsAuthorized(mockHeaders.Object, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Headers_MissingHttpMethod_Null_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();

            var mockHeaders = new Mock<IHeadersContainer>();
            mockHeaders.Setup(m => m.Count).Returns(1);
            string absolutePath = "/Api/Entity1Service/Entities";
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns(absolutePath);
            var userRoleId = 10;

            // Act
            var result = headerValidator.IsAuthorized(mockHeaders.Object, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [PrimitiveList(0, -1, int.MinValue)]
        public void TokenHeaderValidator_IsAuthorized_Claims_NullOrEmpty_False(int userRoleId)
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();

            var mockHeaders = new Mock<IHeadersContainer>();
            mockHeaders.Setup(m => m.Count).Returns(1);
            string absolutePath = "/Api/Entity1Service/Entities";
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns(absolutePath);
            mockHeaders.Setup(m => m.Get("HttpMethod")).Returns("GET");

            // Act
            var result = headerValidator.IsAuthorized(mockHeaders.Object, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Claims_NotACustomerRole_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            var mockHeaders = new Mock<IHeadersContainer>();
            mockHeaders.Setup(m => m.Count).Returns(1);
            string absolutePath = "/Api/Entity1Service/Entities";
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns(absolutePath);
            mockHeaders.Setup(m => m.Get("HttpMethod")).Returns("GET");
            var userRoleId = 10;

            // Act
            var result = headerValidator.IsAuthorized(mockHeaders.Object, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_UrlAllowed_True()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();

            var mockHeaders = new Mock<IHeadersContainer>();
            mockHeaders.Setup(m => m.Count).Returns(1);
            string absolutePath = "/Api/Entity1Service/Entities";
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns(absolutePath);
            mockHeaders.Setup(m => m.Get("HttpMethod")).Returns("GET");

            var normalizedPath = "Entity1Service/Entities";
            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { { normalizedPath, new[] { "GET" } } };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            var userRoleId = WellknownUserRoleIds.Customer;

            // Act
            var result = headerValidator.IsAuthorized(mockHeaders.Object, userRoleId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void TokenHeaderValidator_IsAuthorized_Headers_PathAndQuery_Null_True(string pathAndQuery)
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();

            var mockHeaders = new Mock<IHeadersContainer>();
            mockHeaders.Setup(m => m.Count).Returns(1);
            string absolutePath = "/Api/Entity1Service/Entities";
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns(absolutePath);
            mockHeaders.Setup(m => m.Get("HttpMethod")).Returns("GET");
            mockHeaders.Setup(m => m.Get("PathAndQuery")).Returns(pathAndQuery);

            var normalizedPath = "Entity1Service/Entities";
            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);
            _MockPathNormalizer.Setup(m => m.Normalize(pathAndQuery)).Returns(pathAndQuery);

            var userRoleId = 2;

            // Act
            var result = headerValidator.IsAuthorized(mockHeaders.Object, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_UrlRegexAllowed_True()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();


            var mockHeaders = new Mock<IHeadersContainer>();
            mockHeaders.Setup(m => m.Count).Returns(1);
            string absolutePath = "/Api/Entity1Service/Entities";
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns(absolutePath);
            mockHeaders.Setup(m => m.Get("HttpMethod")).Returns("GET");
            var urlParams = "OrgId={0}";
            var pathAndQuery = $"{absolutePath}?{urlParams}";
            mockHeaders.Setup(m => m.Get("PathAndQuery")).Returns(pathAndQuery);

            var normalizedPath = "Entity1Service/Entities";
            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            var normalizedPathAndQuery = $"{normalizedPath}?{urlParams}";
            _MockPathNormalizer.Setup(m => m.Normalize(pathAndQuery)).Returns(normalizedPathAndQuery);

            var userRoleId = WellknownUserRoleIds.Customer;

            // Act
            var result = headerValidator.IsAuthorized(mockHeaders.Object, userRoleId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_GetInternalCustomerCalls_True()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();

            var mockHeaders = new Mock<IHeadersContainer>();
            mockHeaders.Setup(m => m.Count).Returns(1);
            string absolutePath = "/Api/Entity1Service/Entities";
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns(absolutePath);
            mockHeaders.Setup(m => m.Get("HttpMethod")).Returns("GET");
            var urlParams = "OrgId={0}";
            var pathAndQuery = $"{absolutePath}?{urlParams}";
            mockHeaders.Setup(m => m.Get("PathAndQuery")).Returns(pathAndQuery);

            var normalizedPath = "Entity1Service/Entities";
            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            var normalizedPathAndQuery = $"{normalizedPath}?{urlParams}";
            _MockPathNormalizer.Setup(m => m.Normalize(pathAndQuery)).Returns(normalizedPathAndQuery);

            var userRoleId = WellknownUserRoleIds.Customer;

            // Act
            var result = headerValidator.IsAuthorized(mockHeaders.Object, userRoleId);

            // Assert
            Assert.IsTrue(result);
        }
        #endregion
    }
}
