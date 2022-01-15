using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    [TestClass]
    public class EntitySettingsProviderTests
    {
        [TestMethod]
        public async Task EntitySettingsProvider_GetAsync_NullResponseFromClient_Test()
        {
            // Arrange
            var mockEntityClient = new Mock<IAdminEntityClientAsync<Entity, int>>();
            OdataObjectCollection<Entity, int> ooc = null;
            mockEntityClient.Setup(m => m.GetByQueryParametersAsync(It.IsAny<string>(), It.IsAny<bool>()))
                            .ReturnsAsync(ooc);

            var unitUnderTest = new EntitySettingsProvider(mockEntityClient.Object);

            // Act
            var result = await unitUnderTest.GetAsync();

            // Assert
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task EntitySettingsProvider_GetAsync_DataResponseFromClient_Test()
        {
            // Arrange
            var json = File.ReadAllText(@"Data\EntitySettings.json");
            var  ooc = JsonConvert.DeserializeObject<OdataObjectCollection<Entity, int>>(json);
            var mockEntityClient = new Mock<IAdminEntityClientAsync<Entity, int>>();
            mockEntityClient.Setup(m => m.GetByQueryParametersAsync(It.IsAny<string>(), It.IsAny<bool>()))
                            .ReturnsAsync(ooc);

            var unitUnderTest = new EntitySettingsProvider(mockEntityClient.Object);

            // Act
            var result = await unitUnderTest.GetAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.TryGetValue("ActivationAttempt", out EntitySetting value));
            Assert.AreEqual("ActivationAttempt", value.Name);
            Assert.AreEqual("Miscellaneous", value.EntityGroup);
            Assert.AreEqual(5, value.EntityProperties.Count);
        }

        [TestMethod]
        public async Task EntitySettingsProvider_GetAsync_DataResponseFromClient_MissingRelatedEntities_Test()
        {
            // Arrange
            var json = File.ReadAllText(@"Data\EntitySettings.MissingRelatedEntities.json");
            var ooc = JsonConvert.DeserializeObject<OdataObjectCollection<Entity, int>>(json);
            var mockEntityClient = new Mock<IAdminEntityClientAsync<Entity, int>>();
            mockEntityClient.Setup(m => m.GetByQueryParametersAsync(It.IsAny<string>(), It.IsAny<bool>()))
                            .ReturnsAsync(ooc);

            var unitUnderTest = new EntitySettingsProvider(mockEntityClient.Object);

            // Act
            var result = await unitUnderTest.GetAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.TryGetValue("ActivationAttempt", out EntitySetting value));
            Assert.AreEqual("ActivationAttempt", value.Name);
            Assert.IsNull(value.EntityGroup);
            Assert.AreEqual(0, value.EntityProperties.Count);
        }
    }
}
