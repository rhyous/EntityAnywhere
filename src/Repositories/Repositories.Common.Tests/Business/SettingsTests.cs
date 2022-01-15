using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Repositories.Common.Tests.Business
{
    [TestClass]
    public class SettingsTests
    {
        private MockRepository _MockRepository;

        private Mock<IAppSettings> _MockAppSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAppSettings = _MockRepository.Create<IAppSettings>();
        }

        private Settings<User> CreateSettings()
        {
            return new Settings<User>(_MockAppSettings.Object);
        }

        [TestMethod]
        public void Settings_Constructor_DefaultSettingsUsed_Test()
        {
            // Arrange
            var appSettings = new NameValueCollection();
            _MockAppSettings.Setup(m => m.Collection).Returns(appSettings);
            // Act
            var actual = CreateSettings();

            // Assert
            Assert.AreEqual("EAF.User", actual.ContextKey);
            Assert.AreEqual(true, actual.UseEntityFrameworkDatabaseManagement);
            Assert.AreEqual(true, actual.AutomaticMigrationsEnabled);
            Assert.AreEqual(false, actual.AutomaticMigrationDataLossAllowed);
        }

        [TestMethod]
        public void Settings_Constructor_EntitySpecificSettingsUsed_Test()
        {
            // Arrange
            var entity = "User";
            var appSettings = new NameValueCollection
            {
                { $"{Settings<User>.AutomaticMigrationsEnabledSetting}For{entity}", "false" },
                { $"{Settings<User>.AutomaticMigrationDataLossAllowedSetting}For{entity}", "true" },
                { $"{Settings<User>.UseEntityFrameworkDatabaseManagementSetting}For{entity}", "false" },
                { $"{Settings<User>.ContextKeySetting}For{entity}", "EAF2.User" }
            };
            _MockAppSettings.Setup(m => m.Collection).Returns(appSettings);

            // Act
            var actual = CreateSettings();

            // Assert
            Assert.AreEqual("EAF2.User", actual.ContextKey);
            Assert.AreEqual(false, actual.UseEntityFrameworkDatabaseManagement);
            Assert.AreEqual(false, actual.AutomaticMigrationsEnabled);
            Assert.AreEqual(true, actual.AutomaticMigrationDataLossAllowed);
        }
    }
}
