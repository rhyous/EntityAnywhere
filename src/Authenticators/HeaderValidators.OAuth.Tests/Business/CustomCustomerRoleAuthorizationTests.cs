using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.HeaderValidators.Tests
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
            var headers = new NameValueCollection { { "HttpMethod", "GET" } };

            // Act
            var result = headerValidator.IsAuthorized(token, headers);

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
            var headers = new NameValueCollection { { "AbsolutePath", "/Api/Entity1Service/Entities" } };


            // Act
            var result = headerValidator.IsAuthorized(token, headers);

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
            var headers = new NameValueCollection { { "AbsolutePath", "/Api/Entity1Service/Entities" } };


            // Act
            var result = headerValidator.IsAuthorized(token, headers);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Headers_Null_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = new AccessToken { UserRoleId = 10 };
            NameValueCollection headers = null;

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
            IAccessToken token = new AccessToken { UserRoleId = 10 };
            var headers = new NameValueCollection();

            // Act
            var result = headerValidator.IsAuthorized(token, headers);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Headers_MissingAbsolutePath_Null_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = new AccessToken { UserRoleId = 10 };
            var headers = new NameValueCollection { { "HttpMethod", "GET" } };

            // Act
            var result = headerValidator.IsAuthorized(token, headers);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Headers_MissingHttpMethod_Null_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = new AccessToken { UserRoleId = 10 };
            var headers = new NameValueCollection { { "AbsolutePath", "/Api/Entity1Service/Entities" } };

            // Act
            var result = headerValidator.IsAuthorized(token, headers);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Claims_NotACustomerRole_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = new AccessToken { UserRoleId = 10 };
            var headers = new NameValueCollection { { "AbsolutePath", "/Api/Entity1Service/Entities" }, { "HttpMethod", "GET" } };

            // Act
            var result = headerValidator.IsAuthorized(token, headers);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_UrlAllowed_True()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = new AccessToken { UserRoleId = 2 };
            var absolutePath = "/Api/Entity1Service/Entities";
            var normalizedPath = "Entity1Service/Entities";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath }, { "HttpMethod", "GET" } };


            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { { normalizedPath, new[] { "GET" } } };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            // Act
            var result = headerValidator.IsAuthorized(token, headers);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void TokenHeaderValidator_IsAuthorized_Headers_PathAndQuery_Null_True(string pathAndQuery)
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = new AccessToken { UserRoleId = 2 };
            var absolutePath = "/Api/Entity1Service/Entities";
            var normalizedPath = "Entity1Service/Entities";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath }, { "HttpMethod", "GET" }, { "PathAndQuery", pathAndQuery } };

            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            _MockPathNormalizer.Setup(m => m.Normalize(pathAndQuery)).Returns(pathAndQuery);

            // Act
            var result = headerValidator.IsAuthorized(token, headers);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_UrlRegexAllowed_True()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = new AccessToken { UserRoleId = 2 };
            var absolutePath = "/Api/Entity1Service/Entities";
            var normalizedPath = "Entity1Service/Entities";
            var urlParams = "OrgId={0}";
            var pathAndQuery = $"{absolutePath}?{urlParams}";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath }, { "HttpMethod", "GET" }, { "PathAndQuery", pathAndQuery } };
            var normalizedPathAndQuery = $"{normalizedPath}?{urlParams}";

            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            _MockPathNormalizer.Setup(m => m.Normalize(pathAndQuery)).Returns(normalizedPathAndQuery);

            // Act
            var result = headerValidator.IsAuthorized(token, headers);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_GetInternalCustomerCalls_True()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = new AccessToken { UserRoleId = WellknownUserRoleIds.InternalCustomer };
            var absolutePath = "/Api/Entity1Service/Entities";
            var normalizedPath = "Entity1Service/Entities";
            var urlParams = "OrgId={0}";
            var pathAndQuery = $"{absolutePath}?{urlParams}";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath }, { "HttpMethod", "GET" }, { "PathAndQuery", pathAndQuery } };
            var normalizedPathAndQuery = $"{normalizedPath}?{urlParams}";

            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            _MockPathNormalizer.Setup(m => m.Normalize(pathAndQuery)).Returns(normalizedPathAndQuery);

            // Act
            var result = headerValidator.IsAuthorized(token, headers);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_GetInternalCustomerCalls_NotCalled_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            IAccessToken token = new AccessToken { UserRoleId = 2 };
            var absolutePath = "/Api/Entity1Service/Entities";
            var normalizedPath = "Entity1Service/Entities";
            var urlParams = "OrgId={0}";
            var pathAndQuery = $"{absolutePath}?{urlParams}";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath }, { "HttpMethod", "GET" }, { "PathAndQuery", pathAndQuery } };
            var normalizedPathAndQuery = $"{normalizedPath}?{urlParams}";

            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            _MockPathNormalizer.Setup(m => m.Normalize(pathAndQuery)).Returns(normalizedPathAndQuery);

            // Act
            var result = headerValidator.IsAuthorized(token, headers);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion
    }
}
