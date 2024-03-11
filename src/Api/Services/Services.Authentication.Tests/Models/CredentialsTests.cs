using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Tests.Models
{
    [TestClass]
    public class CredentialsTests
    {
        [TestMethod]
        public void Credentials_NullUsername_Test()
        {
            // Arrange

            // Act
            var creds = new Credentials { User = null };

            // Assert
            Assert.IsNull(creds.User);
        }

        [TestMethod]
        public void Credentials_UsernameTrimmed_Test()
        {
            // Arrange
            var untrimmedUsername = " User1 ";
            var expected = "User1";

            // Act
            var creds = new Credentials { User = untrimmedUsername };

            // Assert
            Assert.AreEqual(expected, creds.User);
        }

        [TestMethod]
        public void Credentials_PasswordNeverTrimmed_Test()
        {
            // Arrange
            var untrimmedPassword = " mySecret ";

            // Act
            var creds = new Credentials { User = " User1 ", Password = " mySecret " };

            // Assert
            Assert.AreEqual(untrimmedPassword, creds.Password);
        }
    }
}
