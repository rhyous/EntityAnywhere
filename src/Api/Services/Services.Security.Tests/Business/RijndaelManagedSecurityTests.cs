using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Rhyous.EntityAnywhere.Services.Security.Tests.Classes
{
    [TestClass]
    public class RijndaelManagedSecurityTests
    {
        private MockRepository _MockRepository;

        private Mock<IPasswordSecuritySettings> _MockPasswordSecuritySettings;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockPasswordSecuritySettings = _MockRepository.Create<IPasswordSecuritySettings>();
        }

        private RijndaelManagedSecurity CreateRijndaelManagedSecurity()
        {
            return new RijndaelManagedSecurity(_MockPasswordSecuritySettings.Object);
        }

        [TestMethod]
        public void RijndaelManagedSecurity_CanEncryptAndDecrpyt()
        {
            // Arrange
            var cipherKey = "M8uo4JpITQlwXXx6gWPuPJ5dFKxjt8eMK7rt5cD386w4D0jX9EBYt85JUO0qw0ix";
            var iterations = 1000;
            _MockPasswordSecuritySettings.Setup(m => m.CipherKey).Returns(cipherKey);
            _MockPasswordSecuritySettings.Setup(m => m.DerivationIterations).Returns(iterations);
            var rijndaelManaged = CreateRijndaelManagedSecurity();
            var password = "this is my super serial secret";

            // Act
            var encryptdPw = rijndaelManaged.Encrypt(password);
            var decrpytedPw = rijndaelManaged.Decrypt(encryptdPw);

            // Assert
            Assert.AreNotEqual(password, encryptdPw);
            Assert.AreEqual(password, decrpytedPw);
        }

        [TestMethod]
        public void RijndaelManagedSecurity_CanCompareWithoutDecrypting()
        {
            // Arrange
            var cipherKey = "M8uo4JpITQlwXXx6gWPuPJ5dFKxjt8eMK7rt5cD386w4D0jX9EBYt85JUO0qw0ix";
            var iterations = 1000;
            _MockPasswordSecuritySettings.Setup(m => m.CipherKey).Returns(cipherKey);
            _MockPasswordSecuritySettings.Setup(m => m.DerivationIterations).Returns(iterations);
            var rijndaelManaged = CreateRijndaelManagedSecurity();
            var password = "this is my super serial secret";

            // Act
            var encryptdPw = rijndaelManaged.Encrypt(password);
            var actual = rijndaelManaged.Compare(password, encryptdPw);

            // Assert
            Assert.AreNotEqual(password, encryptdPw);
            Assert.IsTrue(actual);
        }
    }
}
