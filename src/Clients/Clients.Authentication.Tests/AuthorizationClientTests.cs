using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2.Tests
{
    [TestClass]
    public class AuthorizationClientTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityClientConfig> _MockEntityClientConfig;
        private Mock<IHttpClientRunner> _MockHttpClientRunner;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityClientConfig = _MockRepository.Create<IEntityClientConfig>();
            _MockHttpClientRunner = _MockRepository.Create<IHttpClientRunner>();
        }

        private AuthorizationClient CreateAuthorizationClient()
        {
            return new AuthorizationClient(
                _MockEntityClientConfig.Object,
                _MockHttpClientRunner.Object);
        }

        #region GetRoleDataAsync
        [TestMethod]
        public async Task AuthorizationClient_GetRoleDataAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var authorizationClient = CreateAuthorizationClient();
            var host = "https://some.site.tld";
            var subPath = "Api";
            _MockEntityClientConfig.Setup(m => m.EntityWebHost).Returns(host);
            _MockEntityClientConfig.Setup(m => m.EntitySubpath).Returns(subPath);
            var data = new UserRoleEntityData();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<UserRoleEntityData>(HttpMethod.Get, "https://some.site.tld/Api/AuthorizationService/MyRoleData", true))
                                 .ReturnsAsync(data);

            // Act
            var result = await authorizationClient.GetRoleDataAsync();

            // Assert
            Assert.AreEqual(data, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
