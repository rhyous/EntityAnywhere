using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Services.Tests.Business
{
    [TestClass]
    public class MissingEntitySettingDetectorTests
    {
        #region Detect
        [TestMethod]
        public void MissingEntitySettingDetector_Detect_EntitySettingsNull_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            Dictionary<string, EntitySetting> settings = null;
            var entityTypes = new[] { typeof(Entity), typeof(EntityGroup), typeof(EntityProperty) };

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                unitUnderTest.Detect(settings, entityTypes);
            });
        }

        [TestMethod]
        public void MissingEntitySettingDetector_Detect_EntityTypesNull_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            var settings = new Dictionary<string, EntitySetting>();
            IEnumerable<Type> entityTypes = null;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => { unitUnderTest.Detect(settings, entityTypes); });
        }

        [TestMethod]
        public void MissingEntitySettingDetector_Detect_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            var settings = new Dictionary<string, EntitySetting>();
            var entityTypes = new[] { typeof(EntityString), typeof(EntityInt) };

            // Act
            var result = unitUnderTest.Detect(settings, entityTypes);

            // Assert
            Assert.AreEqual(2, result.Entities.Count);
            Assert.AreEqual(nameof(EntityString), result.Entities[nameof(EntityString)].Object.Name);
            Assert.IsTrue(result.Entities[nameof(EntityString)].IsMissing);
            Assert.AreEqual(nameof(EntityInt), result.Entities[nameof(EntityInt)].Object.Name);
            Assert.IsTrue(result.Entities[nameof(EntityInt)].IsMissing);
            Assert.AreEqual(2, result.EntityGroups.Count);
            Assert.AreEqual("Miscellaneous", result.EntityGroups["Miscellaneous"].Object.Name);
            Assert.IsTrue(result.EntityGroups["Miscellaneous"].IsMissing);
            Assert.AreEqual("TestGroup", result.EntityGroups["TestGroup"].Object.Name);
            Assert.IsTrue(result.EntityGroups["TestGroup"].IsMissing);
        }


        [TestMethod]
        public void MissingEntitySettingDetector_Detect_EntityAlreadyExists_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var group = new EntityGroup { Id = 11, Name = "Miscellaneous" };
            groups.Add(group.Name, group);
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            var settings = new Dictionary<string, EntitySetting>();
            var setting = new EntitySetting
            {
                Id = 127, Name = nameof(EntityString), EntityGroupId = 17
            };
            settings.Add(setting.Name, setting);
            var entityTypes = new[] { typeof(EntityString), typeof(EntityInt) };

            // Act
            var result = unitUnderTest.Detect(settings, entityTypes);

            // Assert
            Assert.AreEqual(2, result.Entities.Count);

            var entityString = result.Entities[nameof(EntityString)];
            Assert.AreEqual(nameof(EntityString), entityString.Object.Name);
            Assert.IsFalse(entityString.IsMissing);
            Assert.AreEqual(17, entityString.Object.EntityGroupId);
            Assert.AreEqual(3, entityString.Object.EntityProperties.Count);

            var entityInt = result.Entities[nameof(EntityInt)];
            Assert.AreEqual(nameof(EntityInt), entityInt.Object.Name);
            Assert.IsTrue(entityInt.IsMissing);
            Assert.AreEqual(11, entityInt.Object.EntityGroupId);
            Assert.AreEqual(2, entityInt.Object.EntityProperties.Count);

            Assert.AreEqual(0, result.EntityGroups.Count);
        }
        #endregion

        #region AddFromType
        [TestMethod]
        public void MissingEntitySettingDetector_AddFromType_NulSettings_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            MissingEntitySettings settings = null;
            var entityType = typeof(EntityString);

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                unitUnderTest.AddFromType(settings, entityType);
            });
        }

        [TestMethod]
        public void MissingEntitySettingDetector_AddFromType_NullType_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            var settings = new MissingEntitySettings();
            Type entityType = null;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                unitUnderTest.AddFromType(settings, entityType);
            });
        }

        [TestMethod]
        public void MissingEntitySettingDetector_AddFromType_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            var settings = new MissingEntitySettings();
            var entityType = typeof(EntityString);

            // Act
            unitUnderTest.AddFromType(settings, entityType);

            // Assert
            Assert.AreEqual(1, settings.Entities.Count);
            Assert.AreEqual(1, settings.EntityGroups.Count);
            Assert.AreEqual("TestGroup", settings.EntityGroups.Keys.First());
            Assert.AreEqual(nameof(EntityString), settings.Entities.First().Key);
            Assert.IsTrue(settings.Entities[nameof(EntityString)].IsMissing);
            var es = settings.Entities[nameof(EntityString)].Object;
            Assert.AreEqual(nameof(EntityString), es.Name);
            Assert.AreEqual("TestGroup", es.EntityGroup);
            Assert.AreEqual(3, es.EntityProperties.Count);
        }
        #endregion

        #region AddPropertiesFromType
        [TestMethod]
        public void MissingEntitySettingDetector_AddPropertiesFromType_NullEntitySetting_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            Entity2 setting = null;
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
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            var setting = new Entity2();
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
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            var setting = new Entity2();
            var entityType = typeof(EntityString);

            // Act
            unitUnderTest.AddPropertiesFromType(setting, entityType);

            // Assert
            Assert.AreEqual(3, setting.EntityProperties.Count);
            Assert.IsNotNull(setting.EntityProperties["Id"]);
            Assert.IsNotNull(setting.EntityProperties["Name"]);
        }
        #endregion

        #region AddFromPropertyInfo

        [TestMethod]
        public void MissingEntitySettingDetector_AddFromPropertyInfo_NullEntitySetting_Test()
        {
            // Arrange
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            Entity2 setting = null;
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
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            var entitySetting = new Entity2();
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
            var groups = new Dictionary<string, EntityGroup>();
            var unitUnderTest = new MissingEntitySettingDetector(groups);
            var entitySetting = new Entity2();
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
