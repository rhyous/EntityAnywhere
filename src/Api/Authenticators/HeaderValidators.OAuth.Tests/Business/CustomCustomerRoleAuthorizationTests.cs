using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.HeaderValidators;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

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

        public TestContext TestContext { get; set; }

        #region IsAllowedUrl
        [TestMethod]
        public void OAuthHeaderValidator_IsAllowedUrl_GivenValidArguments_ReturnsTrue()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            string testData = "/AddendumService.svc/Addenda";
            var httpVerb = "GET";
            var urlsDict = new Dictionary<string, IEnumerable<string>> { { testData, new[] { "GET" } } };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls())
                                  .Returns(urlsDict);
            // Act
            var actual = headerValidator.IsAllowedUrl(testData, httpVerb);

            // Assert
            Assert.IsTrue(actual, "Path is " + testData);
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
        public void TokenHeaderValidator_IsAuthorized_Token_Null_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = null; 
            var mockHeaders = new Mock<IHeadersContainer>();
            mockHeaders.Setup(m => m.Count).Returns(1);
            mockHeaders.Setup(m => m.Get("HttpMethod")).Returns("GET");

            // Act
            var result = headerValidator.IsAuthorized(token, mockHeaders.Object);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [PrimitiveList(0, -1, int.MinValue)]
        public void TokenHeaderValidator_IsAuthorized_Token_OrgId_NotPositiveInt_False(int orgId)
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = new AccessToken { UserRoleId = 10 };
            var mockHeaders = new Mock<IHeadersContainer>();
            mockHeaders.Setup(m => m.Count).Returns(1);
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns("/Api/Entity1Service/Entities");

            // Act
            var result = headerValidator.IsAuthorized(token, mockHeaders.Object);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [PrimitiveList(0, -1, int.MinValue)]
        public void TokenHeaderValidator_IsAuthorized_Token_UserRoleId_NotPositiveInt_False(int userRoleId)
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = new AccessToken { UserRoleId = userRoleId };
            var mockHeaders = new Mock<IHeadersContainer>();
            mockHeaders.Setup(m => m.Count).Returns(1);
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns("/Api/Entity1Service/Entities");


            // Act
            var result = headerValidator.IsAuthorized(token, mockHeaders.Object);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Headers_Null_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = new AccessToken { UserRoleId = 10 };
            IHeadersContainer headers = null;

            // Act
            var result = headerValidator.IsAuthorized(token, headers);

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
            IAccessToken token = new AccessToken { UserRoleId = 10 };

            // Act
            var result = headerValidator.IsAuthorized(token, mockHeaders.Object);

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
            IAccessToken token = new AccessToken { UserRoleId = 10 };

            // Act
            var result = headerValidator.IsAuthorized(token, mockHeaders.Object);

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

            IAccessToken token = new AccessToken { UserRoleId = 10 };

            // Act
            var result = headerValidator.IsAuthorized(token, mockHeaders.Object);

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

            IAccessToken token = new AccessToken { UserRoleId = userRoleId };

            // Act
            var result = headerValidator.IsAuthorized(token, mockHeaders.Object);

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

            IAccessToken token = new AccessToken { UserRoleId = WellknownUserRoleIds.Customer };

            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { { normalizedPath, new[] { "GET" } } };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            // Act
            var result = headerValidator.IsAuthorized(token, mockHeaders.Object);

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

            IAccessToken token = new AccessToken { UserRoleId = 2 };
            
            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            _MockPathNormalizer.Setup(m => m.Normalize(pathAndQuery)).Returns(pathAndQuery);

            // Act
            var result = headerValidator.IsAuthorized(token, mockHeaders.Object);

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

            IAccessToken token = new AccessToken { UserRoleId = WellknownUserRoleIds.Customer };


            // Act
            var result = headerValidator.IsAuthorized(token, mockHeaders.Object);

            // Assert
            Assert.IsTrue(result);
        }
        #endregion
    }
}
