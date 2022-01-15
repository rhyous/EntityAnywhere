using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Business
{
    [TestClass]
    public class EntityConfigurationProviderTests
    {
        private MockRepository _MockRepository;
        private Mock<IAdminEntityClientAsync<Entity, int>> _EntityEntityClient;
        private Mock<IAdminEntityClientAsync<EntityProperty, int>> _EntityPropertyEntityClient;

        [TestInitialize]
        public void Init()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _EntityEntityClient = _MockRepository.Create<IAdminEntityClientAsync<Entity, int>>();
            _EntityPropertyEntityClient = _MockRepository.Create<IAdminEntityClientAsync<EntityProperty, int>>();
        }

        public EntityConfigurationProvider CreateEntityConfigurationProvider()
        {
            return new EntityConfigurationProvider(
                _EntityEntityClient.Object,
                _EntityPropertyEntityClient.Object);
        }

        [TestMethod]
        public void CanCreateAnEntityConfiguration()
        {
            // Arrange
            var configProvider = CreateEntityConfigurationProvider();
            var entity = new Entity()
            {
                SortOrder = SortOrder.Descending,
                SortByPropertyId = 1
            };
            var odataEntity = entity.AsOdata<Entity, int>();
            _EntityEntityClient.Setup(x => x.GetByAlternateKeyAsync(It.IsAny<string>(), It.IsAny<bool>()))
                               .ReturnsAsync(odataEntity);
            var entityProperty = new EntityProperty()
            {
                Name = "MyPropertyName"
            };
            var odataEntityProperty = entityProperty.AsOdata<EntityProperty, int>();
            _EntityPropertyEntityClient.Setup(x => x.GetAsync(It.IsAny<int>(), It.IsAny<bool>()))
                                       .ReturnsAsync(odataEntityProperty);

            // Act
            var result = configProvider.ProvideAsync(typeof(string)).Result;

            // Assert
            Assert.AreEqual("MyPropertyName", result.DefaultSortByProperty);
            Assert.AreEqual(SortOrder.Descending, result.DefaultSortOrder);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void DontEncounterAnInfiniteLoop()
        {
            // Arrange
            var configProvider = CreateEntityConfigurationProvider();

            // Act
            var result = configProvider.ProvideAsync(typeof(Entity)).Result;
            var result2 = configProvider.ProvideAsync(typeof(EntityProperty)).Result;

            // Assert
            Assert.AreEqual("Id", result.DefaultSortByProperty);
            Assert.AreEqual(default, result.DefaultSortOrder);

            Assert.AreEqual("Id", result2.DefaultSortByProperty);
            Assert.AreEqual(default, result2.DefaultSortOrder);
            _MockRepository.VerifyAll();

        }
    }
}
