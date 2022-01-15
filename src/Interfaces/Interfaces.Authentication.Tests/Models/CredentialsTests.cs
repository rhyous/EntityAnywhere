using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.UnitTesting;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Models
{
    [TestClass]
    public class CredentialsTests
    {
        #region AuthenticationPlugin
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void Credentials_AuthenticationPlugin_DefaultValue_Test(string authenticationPlugin)
        {
            // Arrange
            // Act
            var credentials = new Credentials { AuthenticationPlugin = authenticationPlugin };

            // Assert
            Assert.AreEqual("Any", credentials.AuthenticationPlugin);
        }
        #endregion


        #region User
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void Credentials_User_IsTrimmed_Test(string whitespace)
        {
            // Arrange
            var user = "user27";

            // Act
            var credentials = new Credentials { User = user + whitespace };

            // Assert
            Assert.AreEqual(user, credentials.User);
        }

        [TestMethod]
        public void Credentials_User_Null_NotTrimmed_Test()
        {
            // Arrange
            string user = null;

            // Act
            var credentials = new Credentials { User = user  };

            // Assert
            Assert.AreEqual(user, credentials.User);
        }
        #endregion
    }
}