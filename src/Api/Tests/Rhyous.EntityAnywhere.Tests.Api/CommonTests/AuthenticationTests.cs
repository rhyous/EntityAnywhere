using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    [TestClass]
    public class AuthenticationTests
    {
        public static IEntityClientConfig EntityClientConfig;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            EntityClientConfig = new TestConfiguration(context);
        }

        [TestMethod]
        [TestCategory("Ready")]
        [TestCategory("ProdReady")]
        public async Task AuthenticationServiceTest()
        {
            // Arrange
            var host = EntityClientConfig.EntityWebHost;
            var subPath = EntityClientConfig.EntitySubpath;

            var serviceUrl = StringConcat.WithSeparator('/', host);
            if (!string.IsNullOrWhiteSpace(subPath))
                serviceUrl = StringConcat.WithSeparator('/', serviceUrl, subPath);
            serviceUrl = StringConcat.WithSeparator('/', serviceUrl, "AuthenticationService");
            var client = new AuthenticationClientFactory(null).Create(serviceUrl);
            var user = "admin";
            var pw = "@dmin1234!";

            // Act
            var token = await client.AuthenticateAsync(user, pw);

            // Assert
            Assert.IsNotNull(token);
        }

        [TestMethod]
        [TestCategory("Ready")]
        [TestCategory("ProdReady")]
        public async Task AuthenticationService_Credentials_UserPluginSpecified_Test()
        {
            // Arrange
            var host = EntityClientConfig.EntityWebHost;
            var subPath = EntityClientConfig.EntitySubpath;

            var serviceUrl = StringConcat.WithSeparator('/', host);
            if (!string.IsNullOrWhiteSpace(subPath))
                serviceUrl = StringConcat.WithSeparator('/', serviceUrl, subPath);
            serviceUrl = StringConcat.WithSeparator('/', serviceUrl, "AuthenticationService");
            var client = new AuthenticationClientFactory(null).Create(serviceUrl);
            var credentials = new Credentials { User = "admin", Password = "@dmin1234!", AuthenticationPlugin = "Users" };

            // Act
            var token = await client.AuthenticateAsync(credentials);

            // Assert
            Assert.IsNotNull(token);
            Assert.AreEqual("Admin", token.ClaimDomains.First(cd => cd.Subject == "UserRole").Claims.First(c => c.Name == "Role").Value);
        }
    }
}