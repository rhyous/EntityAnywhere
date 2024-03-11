using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Exceptions;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Services.Security.Tests.Models
{
    [TestClass]
    public class PasswordSecuritySettingsTests
    {
        private MockRepository _MockRepository;

        private Mock<IAppSettings> _MockAppSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAppSettings = _MockRepository.Create<IAppSettings>();
        }

        private PasswordSecuritySettings CreatePasswordSecuritySettings()
        {
            return new PasswordSecuritySettings(_MockAppSettings.Object);
        }

        [TestMethod]
        public void PasswordSecuritySettings_Constructor_SystemSecurityKey_NotDefinedInAppSettings_Throws_Test()
        {
            // Arrange
            var nvc = new NameValueCollection();
            var settings = CreatePasswordSecuritySettings();
            _MockAppSettings.Setup(m=>m.Collection).Returns(nvc);

            // Act & Assert
            Assert.ThrowsException<MissingConfigurationException>(() => 
            {
                var key = settings.CipherKey;
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordSecuritySettings_Constructor_SystemSecurityDerivationIterationsy_NotDefinedInAppSettings_UsesDefault_Test()
        {
            // Arrange
            var cipherKey = "somekey000011112223333";
            var nvc = new NameValueCollection { { PasswordSecuritySettings.SystemSecurityKey, cipherKey } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            // Act
            var actual = CreatePasswordSecuritySettings();

            // Assert
            Assert.AreEqual(cipherKey, actual.CipherKey);
            Assert.AreEqual(PasswordSecuritySettings.DefaultSystemSecurityDerivationIterations, actual.DerivationIterations);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordSecuritySettings_Constructor_SystemSecurityDerivationIterationsy_DefinedInAppSettings_Test()
        {
            // Arrange
            var cipherKey = "somekey000011112223333";
            var iterations = 2500;
            var nvc = new NameValueCollection 
            {
                { PasswordSecuritySettings.SystemSecurityKey, cipherKey },
                { PasswordSecuritySettings.SystemSecurityDerivationIterations, iterations.ToString() },
            };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            // Act
            var actual = CreatePasswordSecuritySettings();

            // Assert
            Assert.AreEqual(iterations, actual.DerivationIterations);
            _MockRepository.VerifyAll();
        }
    }
}
