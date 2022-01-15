using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Services.Tests.Models
{
    [TestClass]
    public class AuthenticationSettingsTests
    {
        private MockRepository _MockRepository;

        private Mock<IAppSettings> _MockAppSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAppSettings = _MockRepository.Create<IAppSettings>();
        }

        private AuthenticationSettings CreateAuthenticationSettings()
        {
            return new AuthenticationSettings(_MockAppSettings.Object);
        }

        [TestMethod]
        public void AuthenticationSettings_DefaultsSettings_Test()
        {
            // Arrange
            var appSettings = new NameValueCollection();
            _MockAppSettings.Setup(m=>m.Collection).Returns(appSettings);

            // Act
            var settings = CreateAuthenticationSettings();

            // Assert
            Assert.AreEqual(AuthenticationSettings.MaxFailedAttemptsDefaultValue, settings.MaxFailedAttempts);
            Assert.AreEqual(AuthenticationSettings.MaxFailedAttemptsMinutesDefaultValue, settings.MaxFailedAttemptsMinutes);
        }

        [TestMethod]
        public void AuthenticationSettings_ConfiguredSettings_Test()
        {
            // Arrange
            var appSettings = new NameValueCollection
            {
                { AuthenticationSettings.MaxFailedAttemptsKey, "10" },
                { AuthenticationSettings.MaxFailedAttemptsMinutesKey, "120" }
            };
            _MockAppSettings.Setup(m => m.Collection).Returns(appSettings);

            // Act
            var settings = CreateAuthenticationSettings();

            // Assert
            Assert.AreEqual(10, settings.MaxFailedAttempts);
            Assert.AreEqual(120, settings.MaxFailedAttemptsMinutes);
        }
    }
}