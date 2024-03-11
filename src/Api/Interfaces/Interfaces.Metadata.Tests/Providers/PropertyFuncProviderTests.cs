using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces.Common.Tests;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Providers
{
    [TestClass]
    public class PropertyFuncProviderTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntitySettingsCache> _MockEntitySettingsCache;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntitySettingsCache = _MockRepository.Create<IEntitySettingsCache>();
        }

        private PropertyFuncProvider CreateProvider()
        {
            return new PropertyFuncProvider(_MockEntitySettingsCache.Object);
        }

        #region Provide
        [TestMethod]
        public void Provider_Provide_Returns_Method()
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
        public void Provider_GetPropertyData_EntitySettingsForEntity_DoesNotExist_ReturnsNull_Test()
        {
            // Arrange
            var provider = CreateProvider();
            string entity = nameof(EntityInt);
            var entitySettingsDictionary = new EntitySettingsDictionary();
            _MockEntitySettingsCache.Setup(m => m.ProvideAsync(false)).ReturnsAsync(entitySettingsDictionary);

            // Act
            var result = provider.GetPropertyData(entity);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void Provider_GetPropertyData_EntitySettingsForEntity_Exists_ReturnsNull_Test()
        {
            // Arrange
            var provider = CreateProvider();
            string entity = nameof(EntityInt);
            var entitySettingsDictionary = new EntitySettingsDictionary();
            var entitySettings = new EntitySettings { EntityGroup = new EntityGroup { Name = "Group1" } };
            entitySettingsDictionary.TryAdd(nameof(EntityInt), entitySettings);
            _MockEntitySettingsCache.Setup(m => m.ProvideAsync(false)).ReturnsAsync(entitySettingsDictionary);

            // Act
            var result = provider.GetPropertyData(entity).ToArray();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1,result.Length);
            Assert.AreEqual(PropertyFuncProvider.EntityGroupPropertyName, result[0].Key);
            Assert.AreEqual(entitySettings.EntityGroup.Name, result[0].Value);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
