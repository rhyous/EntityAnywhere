using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Model
{
    [TestClass]
    public class EnvironmentVariableAppSettingsTests
    {
        private MockRepository _mockRepository;



        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private EnvironmentVariableAppSettings CreateEnvironmentVariableAppSettings()
        {
            return new EnvironmentVariableAppSettings();
        }

        #region GetCollection
        [TestMethod]
        public void EnvironmentVariableAppSettings_GetCollection_OneVariable()
        {
            // Arrange
            var environmentVariableAppSettings = CreateEnvironmentVariableAppSettings();
            var existingCount = environmentVariableAppSettings.GetCollection().Count;
            var expectedValue = "1";
            Environment.SetEnvironmentVariable("EAF_Test", expectedValue, EnvironmentVariableTarget.Process);

            // Act
            var result = environmentVariableAppSettings.GetCollection();
            var testValue = result.Get("Test");

            // Assert
            Assert.AreEqual(existingCount + 1, result.Count);
            Assert.AreEqual(expectedValue, testValue);
            _mockRepository.VerifyAll();
        }
        #endregion
    }
}
