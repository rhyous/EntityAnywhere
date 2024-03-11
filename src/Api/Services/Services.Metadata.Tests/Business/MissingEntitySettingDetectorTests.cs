using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Tests.Business
{
    [TestClass]
    public class MissingEntitySettingDetectorTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityGroupCache> _MockEntitySettingsGroupCache;
        private Mock<ITypeInfoResolver> _MockEntityInfoResolver;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntitySettingsGroupCache = _MockRepository.Create<IEntityGroupCache>();
            _MockEntityInfoResolver = _MockRepository.Create<ITypeInfoResolver>();
        }

        private MissingEntitySettingDetector CreateMissingEntitySettingDetector()
        {
            return new MissingEntitySettingDetector(
                _MockEntitySettingsGroupCache.Object,
                _MockEntityInfoResolver.Object);
        }

        #region Detect
        [TestMethod]
        public void MissingEntitySettingDetector_Detect_EntitySettingsNull_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = CreateMissingEntitySettingDetector();
            IEntitySettingsDictionary settings = null;
            var entityTypes = new[] { typeof(Entity), typeof(EntityGroup), typeof(EntityProperty) };

            // Act
            // Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await unitUnderTest.DetectAsync(settings, entityTypes);
            });
        }

        [TestMethod]
        public void MissingEntitySettingDetector_Detect_EntityTypesNull_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = CreateMissingEntitySettingDetector();
            var settings = new EntitySettingsDictionary();
            IEnumerable<Type> entityTypes = null;

            // Act
            // Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await unitUnderTest.DetectAsync(settings, entityTypes);
            });
        }

        [TestMethod]
        public async Task MissingEntitySettingDetector_Detect_Test()
        {
            // Arrange
            var unitUnderTest = CreateMissingEntitySettingDetector();
            var settings = new EntitySettingsDictionary();
            var entityTypes = new[] { typeof(EntityString), typeof(EntityInt) };
            var entityGroupDictionary = new EntityGroupDictionary();
            _MockEntitySettingsGroupCache.Setup(m => m.ProvideAsync(false))
                                         .ReturnsAsync(entityGroupDictionary);

            // Act
            var result = await unitUnderTest.DetectAsync(settings, entityTypes);

            // Assert
            Assert.AreEqual(2, result.Entities.Count);
            Assert.AreEqual(nameof(EntityString), result.Entities[nameof(EntityString)].Object.Entity.Name);
            Assert.IsTrue(result.Entities[nameof(EntityString)].IsMissing);
            Assert.AreEqual(nameof(EntityInt), result.Entities[nameof(EntityInt)].Object.Entity.Name);
            Assert.IsTrue(result.Entities[nameof(EntityInt)].IsMissing);
            Assert.AreEqual(2, result.EntityGroups.Count);
            Assert.AreEqual("Miscellaneous", result.EntityGroups["Miscellaneous"].Object.Name);
            Assert.IsTrue(result.EntityGroups["Miscellaneous"].IsMissing);
            Assert.AreEqual("TestGroup", result.EntityGroups["TestGroup"].Object.Name);
            Assert.IsTrue(result.EntityGroups["TestGroup"].IsMissing);
        }

        [TestMethod]
        public async Task MissingEntitySettingDetector_Detect_EntityAlreadyExists_Test()
        {
            // Arrange
            var unitUnderTest = CreateMissingEntitySettingDetector();
            var settings = new EntitySettingsDictionary();
            var setting = new EntitySettings
            {
                Entity = new Entity
                {
                    Id = 127,
                    Name = nameof(EntityString),
                    EntityGroupId = 17
                }
            };
            settings.TryAdd(setting.Entity.Name, setting);
            
            var entityTypes = new[] { typeof(EntityString), typeof(EntityInt) };
            var entityStringInfo = new EntityInfo<EntityString>();
            _MockEntityInfoResolver.Setup(m => m.Resolve(typeof(EntityString)))
                                   .Returns(entityStringInfo);

            var entityIntInfo = new EntityInfo<EntityInt>();
            _MockEntityInfoResolver.Setup(m => m.Resolve(typeof(EntityInt)))
                                   .Returns(entityIntInfo);

            var entityGroupDictionary = new EntityGroupDictionary();
            var group = new EntityGroup { Id = 11, Name = "Miscellaneous" };
            entityGroupDictionary.TryAdd(group.Name, group);
            _MockEntitySettingsGroupCache.Setup(m => m.ProvideAsync(false))
                                         .ReturnsAsync(entityGroupDictionary);

            // Act
            var result = await unitUnderTest.DetectAsync(settings, entityTypes);

            // Assert
            Assert.AreEqual(2, result.Entities.Count);

            var entityString = result.Entities[nameof(EntityString)];
            Assert.AreEqual(nameof(EntityString), entityString.Object.Entity.Name);
            Assert.IsFalse(entityString.IsMissing);
            Assert.AreEqual(17, entityString.Object.Entity.EntityGroupId);
            Assert.AreEqual(3, entityString.Object.EntityProperties.Count);

            var entityInt = result.Entities[nameof(EntityInt)];
            Assert.AreEqual(nameof(EntityInt), entityInt.Object.Entity.Name);
            Assert.IsTrue(entityInt.IsMissing);
            Assert.AreEqual(11, entityInt.Object.Entity.EntityGroupId);
            Assert.AreEqual(2, entityInt.Object.EntityProperties.Count);

            Assert.AreEqual(0, result.EntityGroups.Count);
        }
        #endregion

        #region AddEntitySettingFromTypeAsync
        [TestMethod]
        public async Task MissingEntitySettingDetector_AddEntitySettingFromTypeAsync_NulSettings_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = CreateMissingEntitySettingDetector();
            MissingEntitySettings settings = null;
            var entityType = typeof(EntityString);

            // Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await unitUnderTest.AddEntitySettingFromTypeAsync(settings, entityType);
            });
        }

        [TestMethod]
        public async Task MissingEntitySettingDetector_AddEntitySettingFromTypeAsync_NullType_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = CreateMissingEntitySettingDetector();
            var settings = new MissingEntitySettings();
            Type entityType = null;

            // Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await unitUnderTest.AddEntitySettingFromTypeAsync(settings, entityType);
            });
        }

        [TestMethod]
        public async Task MissingEntitySettingDetector_AddEntitySettingFromTypeAsync_Test()
        {
            // Arrange
            var unitUnderTest = CreateMissingEntitySettingDetector();
            var settings = new MissingEntitySettings();
            var entityType = typeof(EntityString);

            var entityGroupDictionary = new EntityGroupDictionary();
            _MockEntitySettingsGroupCache.Setup(m => m.ProvideAsync(false))
                                         .ReturnsAsync(entityGroupDictionary);

            // Act
            await unitUnderTest.AddEntitySettingFromTypeAsync(settings, entityType);

            // Assert
            Assert.AreEqual(1, settings.Entities.Count);
            Assert.AreEqual(1, settings.EntityGroups.Count);
            Assert.AreEqual("TestGroup", settings.EntityGroups.Keys.First());
            Assert.AreEqual(nameof(EntityString), settings.Entities.First().Key);
            Assert.IsTrue(settings.Entities[nameof(EntityString)].IsMissing);
            var es = settings.Entities[nameof(EntityString)].Object;
            Assert.AreEqual(nameof(EntityString), es.Entity.Name);
            Assert.AreEqual("TestGroup", es.EntityGroup.Name);
            Assert.AreEqual(3, es.EntityProperties.Count);
        }
        #endregion

        #region AddPropertiesFromType
        [TestMethod]
        public void MissingEntitySettingDetector_AddPropertiesFromType_NullEntitySetting_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = CreateMissingEntitySettingDetector();
            EntityWithMissingProperties setting = null;
            var entityType = typeof(Entity);

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                unitUnderTest.AddPropertiesFromType(setting, entityType);
            });
        }

        [TestMethod]
        public void MissingEntitySettingDetector_AddPropertiesFromType_NullType_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = CreateMissingEntitySettingDetector();
            var setting = new EntityWithMissingProperties();
            Type entityType = null;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                unitUnderTest.AddPropertiesFromType(setting, entityType);
            });
        }

        [TestMethod]
        public void MissingEntitySettingDetector_AddPropertiesFromType_Test()
        {
            // Arrange
            var unitUnderTest = CreateMissingEntitySettingDetector();
            var setting = new EntityWithMissingProperties { Entity = new Entity() };
            setting.SearchableProperties.Add(nameof(EntityString.Name));
            var entityType = typeof(EntityString);

            // Act
            unitUnderTest.AddPropertiesFromType(setting, entityType);

            // Assert
            Assert.AreEqual(3, setting.EntityProperties.Count);
            
            Assert.IsNotNull(setting.EntityProperties["Id"]);
            Assert.IsTrue(setting.EntityProperties["Id"].Object.Searchable);

            Assert.IsNotNull(setting.EntityProperties["Name"]);
            Assert.IsTrue(setting.EntityProperties["Name"].Object.Searchable);
        }
        #endregion

        #region AddFromPropertyInfo

        [TestMethod]
        public void MissingEntitySettingDetector_AddFromPropertyInfo_NullEntitySetting_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = CreateMissingEntitySettingDetector();
            EntityWithMissingProperties setting = null;
            var propInfo = typeof(Entity).GetProperty("Name");

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => 
            {
                unitUnderTest.AddFromPropertyInfo(setting, propInfo);
            });
        }

        [TestMethod]
        public void MissingEntitySettingDetector_AddFromPropertyInfo_NullPropInfo_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = CreateMissingEntitySettingDetector();
            var entitySetting = new EntityWithMissingProperties();
            PropertyInfo propInfo = null;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                unitUnderTest.AddFromPropertyInfo(entitySetting, propInfo);
            });
        }

        [TestMethod]
        public void MissingEntitySettingDetector_AddFromPropertyInfo_Test()
        {
            // Arrange
            var unitUnderTest = CreateMissingEntitySettingDetector();
            var entitySetting = new EntityWithMissingProperties { Entity = new Entity() };
            var propInfo = typeof(Entity).GetProperty("Name");

            // Act
            unitUnderTest.AddFromPropertyInfo(entitySetting, propInfo);

            // Assert
            Assert.IsNotNull(entitySetting.EntityProperties);
            Assert.AreEqual(1, entitySetting.EntityProperties.Count);
            Assert.AreEqual("Name", entitySetting.EntityProperties.Keys.First());
            Assert.IsTrue(entitySetting.EntityProperties.Values.First().IsMissing);
        }
        #endregion
    }
}
