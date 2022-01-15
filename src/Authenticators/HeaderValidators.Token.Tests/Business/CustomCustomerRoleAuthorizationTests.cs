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
        [TestCategory("DataDriven")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"Data\IsAllowedUrlData.csv", "IsAllowedUrlData#csv", DataAccessMethod.Sequential)]
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

        #endregion

        #region IsAuthorized

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Headers_Null_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            
            NameValueCollection headers = null;
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
            
            
            var headers = new NameValueCollection();
            var userRoleId = 10;

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Headers_MissingAbsolutePath_Null_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
             
            var headers = new NameValueCollection { { "HttpMethod", "GET" } };
            var userRoleId = 10;

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Headers_MissingHttpMethod_Null_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            
            var headers = new NameValueCollection { { "AbsolutePath", "/Api/Entity1Service/Entities" } };
            var userRoleId = 10;

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [PrimitiveList(0, -1, int.MinValue)]
        public void TokenHeaderValidator_IsAuthorized_Claims_NullOrEmpty_False(int userRoleId)
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            
            var headers = new NameValueCollection { { "AbsolutePath", "/Api/Entity1Service/Entities" }, { "HttpMethod", "GET" } };

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [PrimitiveList(0, -1, int.MinValue)]
        public void TokenHeaderValidator_IsAuthorized_orgId_NotPositiveInt_False(int orgId)
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            NameValueCollection headers = null;
            var userRoleId = 10;

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void TokenHeaderValidator_IsAuthorized_sapId_NullEmptyOrWhitespace_False(string sapId)
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            NameValueCollection headers = null;
            var userRoleId = 10;

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_Claims_NotACustomerRole_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            
            var absolutePath = "/Api/Entity1Service/Entities";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath }, { "HttpMethod", "GET" } };
            var userRoleId = 10;

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_UrlAllowed_True()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            
            var absolutePath = "/Api/Entity1Service/Entities";
            var normalizedPath = "Entity1Service/Entities";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath }, { "HttpMethod", "GET" } };
            var userRoleId = 2;

            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { { normalizedPath, new[] { "GET" } } };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void TokenHeaderValidator_IsAuthorized_Headers_PathAndQuery_Null_True(string pathAndQuery)
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            
            var absolutePath = "/Api/Entity1Service/Entities";
            var normalizedPath = "Entity1Service/Entities";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath }, { "HttpMethod", "GET" }, { "PathAndQuery", pathAndQuery } };
            var userRoleId = 2;

            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            _MockPathNormalizer.Setup(m => m.Normalize(pathAndQuery)).Returns(pathAndQuery);

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_UrlRegexAllowed_True()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            
            var absolutePath = "/Api/Entity1Service/Entities";
            var normalizedPath = "Entity1Service/Entities";
            var urlParams = "OrgId={0}";
            var pathAndQuery = $"{absolutePath}?{urlParams}";
            var normalizedPathAndQuery = $"{normalizedPath}?{urlParams}";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath }, { "HttpMethod", "GET" }, { "PathAndQuery", pathAndQuery } };
            var userRoleId = 2;

            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            _MockPathNormalizer.Setup(m => m.Normalize(pathAndQuery)).Returns(normalizedPathAndQuery);

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_GetInternalCustomerCalls_True()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            
            var absolutePath = "/Api/Entity1Service/Entities";
            var normalizedPath = "Entity1Service/Entities";
            var urlParams = "OrgId={0}";
            var pathAndQuery = $"{absolutePath}?{urlParams}";
            var normalizedPathAndQuery = $"{normalizedPath}?{urlParams}";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath }, { "HttpMethod", "GET" }, { "PathAndQuery", pathAndQuery } };            
            var userRoleId = WellknownUserRoleIds.InternalCustomer;

            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            _MockPathNormalizer.Setup(m => m.Normalize(pathAndQuery)).Returns(normalizedPathAndQuery);

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsAuthorized_GetInternalCustomerCalls_NotCalled_False()
        {
            // Arrange
            var headerValidator = CreateCustomCustomerRoleAuthorization();
            
            
            var absolutePath = "/Api/Entity1Service/Entities";
            var normalizedPath = "Entity1Service/Entities";
            var urlParams = "OrgId={0}";
            var pathAndQuery = $"{absolutePath}?{urlParams}";
            var normalizedPathAndQuery = $"{normalizedPath}?{urlParams}";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath }, { "HttpMethod", "GET" }, { "PathAndQuery", pathAndQuery } };
            var userRoleId = 2;

            _MockPathNormalizer.Setup(m => m.Normalize(absolutePath)).Returns(normalizedPath);
            var customerCalls = new Dictionary<string, IEnumerable<string>> { };
            _MockTokenSecurityList.Setup(m => m.GetCustomerCalls()).Returns(customerCalls);

            _MockPathNormalizer.Setup(m => m.Normalize(pathAndQuery)).Returns(normalizedPathAndQuery);

            // Act
            var result = headerValidator.IsAuthorized(headers, userRoleId);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion
    }
}
