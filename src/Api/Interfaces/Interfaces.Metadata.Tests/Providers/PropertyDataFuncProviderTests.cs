using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces.Common.Tests;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Providers
{
    [TestClass]
    public class PropertyDataFuncProviderTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntitySettingsCache> _MockEntitySettingsCache;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntitySettingsCache = _MockRepository.Create<IEntitySettingsCache>();
        }

        private PropertyDataFuncProvider CreateProvider()
        {
            return new PropertyDataFuncProvider(_MockEntitySettingsCache.Object);
        }

        #region Provide
        [TestMethod]
        public void Provider_Provide_ReturnsMethod()
        {
            // Arrange
            var provider = CreateProvider();

            // Act
            var result = provider.Provide();

            // Assert
            Assert.IsNotNull(result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GetPropertyData
        [TestMethod]
        public void Provider_GetPropertyData_EntitySettingsDictionary_Empty_ReturnsNull()
        {
            // Arrange
            var provider = CreateProvider();
            string entity = nameof(EntityInt);
            string property = nameof(EntityInt.Id);

            var entitySettingsDictionary = new EntitySettingsDictionary();
            _MockEntitySettingsCache.Setup(m => m.ProvideAsync(false)).ReturnsAsync(entitySettingsDictionary);

            // Act
            var result = provider.GetPropertyData(entity, property);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void Provider_GetPropertyData_EntitySettingsDotEntityProperties_Empty_ReturnsNull()
        {
            // Arrange
            var provider = CreateProvider();
            string entity = nameof(EntityInt);
            string property = nameof(EntityInt.Id);

            var entitySettingsDictionary = new EntitySettingsDictionary();
            var entitySettings = new EntitySettings();
            entitySettingsDictionary.TryAdd(nameof(EntityInt), entitySettings);
            _MockEntitySettingsCache.Setup(m => m.ProvideAsync(false)).ReturnsAsync(entitySettingsDictionary);

            // Act
            var result = provider.GetPropertyData(entity, property);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void Provider_GetPropertyData_EntitySettingsDotEntityProperties_HasProperty()
        {
            // Arrange
            var provider = CreateProvider();
            string entity = nameof(EntityInt);
            string property = nameof(EntityInt.Id);

            var entitySettingsDictionary = new EntitySettingsDictionary();
            var entitySettings = new EntitySettings();
            var entityProperty = new EntityProperty { Order = 3, Searchable = true };
            entitySettings.EntityProperties.TryAdd(property, entityProperty);
            entitySettingsDictionary.TryAdd(nameof(EntityInt), entitySettings);
            _MockEntitySettingsCache.Setup(m => m.ProvideAsync(false)).ReturnsAsync(entitySettingsDictionary);

            // Act
            var result = provider.GetPropertyData(entity, property).ToArray();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(PropertyDataFuncProvider.UIDisplayOrder, result[0].Key);
            Assert.AreEqual(entityProperty.Order, result[0].Value);
            Assert.AreEqual(PropertyDataFuncProvider.UISearchable, result[1].Key);
            Assert.AreEqual(entityProperty.Searchable, result[1].Value);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
