using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.Wrappers;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Models
{
    [TestClass]
    public class TokenKeyPairTests
    {
        private MockRepository _MockRepository;

        private Mock<IAppSettings> _MockAppSettings;
        private Mock<IFileIO> _MockFile;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAppSettings = _MockRepository.Create<IAppSettings>();
            _MockFile = _MockRepository.Create<IFileIO>();
        }

        private TokenKeyPair CreateTokenKeyPair()
        {
            return new TokenKeyPair(_MockAppSettings.Object,
                                    _MockFile.Object);
        }

        #region GetKey
        [TestMethod]
        [PrimitiveList("Public","Private")]
        public void TokenKeyPair_PublicOrPrivateKey_AppSettings_Missing_Test(string publicOrPrivate)
        {
            // Arrange
            var tokenKeyPair = CreateTokenKeyPair();
            var appSettings = new NameValueCollection();
            _MockAppSettings.Setup(m => m.Collection).Returns(appSettings);

            // Act & Assert
            Assert.ThrowsException<MissingConfigurationException>(() =>
            {
                var publicKey = tokenKeyPair.GetKey(publicOrPrivate);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList("Public", "Private")]
        public void TokenKeyPair_PublicOrPrivateKey_AppSettings_SetToNull_Test(string publicOrPrivate)
        {
            // Arrange
            var tokenKeyPair = CreateTokenKeyPair();
            string expectedPath = null;
            var appSettings = new NameValueCollection { { $"JWT{publicOrPrivate}Key", expectedPath } };
            _MockAppSettings.Setup(m => m.Collection).Returns(appSettings);

            // Act & Assert
            Assert.ThrowsException<MissingConfigurationException>(() =>
            {
                var key = tokenKeyPair.GetKey(publicOrPrivate);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList("Public", "Private")]
        public void TokenKeyPair_PublicOrPrivateKey_FileAtPath_Missing_Test(string publicOrPrivate)
        {
            // Arrange
            var tokenKeyPair = CreateTokenKeyPair();
            var expectedPath = $@"c:\some\fake\path\to\Key.{publicOrPrivate}";
            var appSettings = new NameValueCollection { { $"JWT{publicOrPrivate}Key", expectedPath } };
            _MockAppSettings.Setup(m => m.Collection).Returns(appSettings);
            _MockFile.Setup(m => m.Exists(expectedPath)).Returns(false);

            // Act & Assert
            Assert.ThrowsException<ConfigurationException>(() =>
            {
                var key = tokenKeyPair.GetKey(publicOrPrivate);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList("Public", "Private")]
        public void TokenKeyPair_PublicOrPrivateKey_FileAtPath_Empty_Test(string publicOrPrivate)
        {
            // Arrange
            var tokenKeyPair = CreateTokenKeyPair();
            var expectedPath = $@"c:\some\fake\path\to\Key.{publicOrPrivate}";
            var appSettings = new NameValueCollection { { $"JWT{publicOrPrivate}Key", expectedPath } };
            _MockAppSettings.Setup(m => m.Collection).Returns(appSettings);
            _MockFile.Setup(m => m.Exists(expectedPath)).Returns(true);
            _MockFile.Setup(m => m.ReadAllText(expectedPath)).Returns("");

            // Act & Assert
            Assert.ThrowsException<ConfigurationException>(() =>
            {
                var key = tokenKeyPair.GetKey(publicOrPrivate);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList("Public", "Private")]
        public void TokenKeyPair_PublicOrPrivateKey_Test(string publicOrPrivate)
        {
            // Arrange
            var tokenKeyPair = CreateTokenKeyPair();
            var expectedKey = $"fake{publicOrPrivate}Key";
            var expectedPath = $@"c:\some\fake\path\to\Key.{publicOrPrivate}";
            var appSettings = new NameValueCollection { { $"JWT{publicOrPrivate}Key", expectedPath } };

            _MockAppSettings.Setup(m => m.Collection).Returns(appSettings);
            _MockFile.Setup(m => m.Exists(expectedPath)).Returns(true);
            _MockFile.Setup(m => m.ReadAllText(expectedPath)).Returns(expectedKey);

            // Act
            var publicKey = tokenKeyPair.GetKey(publicOrPrivate);

            // Assert
            Assert.AreEqual(expectedKey, publicKey);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region PublicKey
        [TestMethod]
        public void TokenKeyPair_PublicKey_Test()
        {
            // Arrange
            var tokenKeyPair = CreateTokenKeyPair();
            var expectedKey = $"fakePublicKey";
            var expectedPath = $@"c:\some\fake\path\to\Key.public";
            var appSettings = new NameValueCollection { { "JWTPublicKey", expectedPath } };

            _MockAppSettings.Setup(m => m.Collection).Returns(appSettings);
            _MockFile.Setup(m => m.Exists(expectedPath)).Returns(true);
            _MockFile.Setup(m => m.ReadAllText(expectedPath)).Returns(expectedKey);

            // Act
            var publicKey = tokenKeyPair.PublicKey;

            // Assert
            Assert.AreEqual(expectedKey, publicKey);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region PrivateKey
        [TestMethod]
        public void TokenKeyPair_PrivateKey_Test()
        {
            // Arrange
            var tokenKeyPair = CreateTokenKeyPair();
            var expectedKey = $"fakePrivateKey";
            var expectedPath = $@"c:\some\fake\path\to\Key.private";
            var appSettings = new NameValueCollection { { "JWTPrivateKey", expectedPath } };

            _MockAppSettings.Setup(m => m.Collection).Returns(appSettings);
            _MockFile.Setup(m => m.Exists(expectedPath)).Returns(true);
            _MockFile.Setup(m => m.ReadAllText(expectedPath)).Returns(expectedKey);

            // Act
            var privateKey = tokenKeyPair.PrivateKey;

            // Assert
            Assert.AreEqual(expectedKey, privateKey);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
