using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services.Tests.Extensions
{
    [TestClass]
    public class CredentialsValidatorResponseExtensionsTests
    {

        [TestMethod]
        public void CredentialsValidatorResponseExtensions_MergeFailed_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mockValidator = new Mock<ICredentialsValidatorLoader>();
            //mockValidator.Setup(m => m.)
            //var credentialsValidatorResponseExtensions = CreateCredentialsValidatorResponseExtensions();
            var respons1 = new CredentialsValidatorResponse
            {
                Success = false,
                Token = null,
                AuthenticationPlugin = "User",
                Message = "User not in a role"
            };

            var response2 = new CredentialsValidatorResponse
            {
                Success = false,
                Token = null,
                AuthenticationPlugin = "Creds",
                Message = "User has no creds"
            };

            var responses = new List<CredentialsValidatorResponse> { respons1, response2 };

            // Act
            var result = responses.MergeFailed();

            // Assert
            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Token);
            Assert.AreEqual(result.AuthenticationPlugin, "User, Creds");
            Assert.AreEqual(result.Message, "User:User not in a role, Creds:User has no creds");
                
        }
    }
}
