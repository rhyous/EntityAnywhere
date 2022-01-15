using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Tests.Business
{
    [TestClass]
    public class EntitySettingsWriterTests
    {
        [TestMethod]
        public async Task EntitySettingsWriter_Write_()
        {
            #region Arrange
            var mockEntityClient = new Mock<IAdminEntityClientAsync<Entity, int>>();
            var postedEntity = new Entity { Id = 101, Name = "E1", EntityGroupId = 301 };
            mockEntityClient.Setup(m => m.PostAsync(It.IsAny<IEnumerable<Entity>>(), It.IsAny<bool>()))
                            .ReturnsAsync(new[] { postedEntity }.AsOdata<Entity, int>());

            var mockEntityPropClient = new Mock<IAdminEntityClientAsync<EntityProperty, int>>();
            var postedEntityProp = new EntityProperty
            {
                Id = 201,
                Name = "Id",
                Type = "System.Int32",
                Order = 1,
                Searchable = true
            };
            mockEntityPropClient.Setup(m => m.PostAsync(It.IsAny<IEnumerable<EntityProperty>>(),
                                                        It.IsAny<bool>()))
                                .ReturnsAsync(new[] { postedEntityProp }.AsOdata<EntityProperty, int>());
            var mockEntityGroupClient = new Mock<IAdminEntityClientAsync<EntityGroup, int>>();
            var postedEntityGroup = new EntityGroup { Id = 301, Name = "G1", };
            mockEntityGroupClient.Setup(m => m.PostAsync(It.IsAny<IEnumerable<EntityGroup>>(),
                                                        It.IsAny<bool>()))
                                .ReturnsAsync(new[] { postedEntityGroup }.AsOdata<EntityGroup, int>());

            var mockLogger = new Mock<ILogger>();

            var entity = new Entity2 { Name = "E1", EntityGroup = "G1" };
            var prop = new EntityProperty
            {
                Name = "Id",
                Type = "System.Int32",
                Order = 1,
                Searchable = true,
            };
            var missingProp = new Missing<IEntityProperty>(prop) { IsMissing = true };
            entity.EntityProperties.Add(prop.Name, missingProp);
            var missingEntity = new Missing<Entity2>(entity) { IsMissing = true };
            MissingEntitySettings settings = new MissingEntitySettings();
            settings.Entities.Add(entity.Name, missingEntity);
            var entityGroup = new EntityGroup { Name = "G1" };
            var missingEntityGroup = new Missing<EntityGroup>(entityGroup) { IsMissing = true };
            settings.EntityGroups.Add(entityGroup.Name, missingEntityGroup);

            var unitUnderTest = new EntitySettingsWriter(
                mockEntityClient.Object,
                mockEntityPropClient.Object,
                mockEntityGroupClient.Object,
                mockLogger.Object);
            #endregion

            // Act
            await unitUnderTest.Write(settings);

            // Assert
            Assert.AreEqual(postedEntity.Id, prop.EntityId);
            mockEntityClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<Entity>>(), It.IsAny<bool>()), Times.Once);
            mockEntityPropClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<EntityProperty>>(), It.IsAny<bool>()), Times.Once);
            mockEntityGroupClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<EntityGroup>>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public async Task EntitySettingsWriter_Write_EntityGroup_Test()
        {
            #region Arrange
            var mockEntityClient = new Mock<IAdminEntityClientAsync<Entity, int>>();
            var mockEntityPropClient = new Mock<IAdminEntityClientAsync<EntityProperty, int>>();
            var mockEntityGroupClient = new Mock<IAdminEntityClientAsync<EntityGroup, int>>();

            var postedEntityGroup = new EntityGroup { Id = 301, Name = "G1", };
            mockEntityGroupClient.Setup(m => m.PostAsync(It.IsAny<IEnumerable<EntityGroup>>(),
                                                        It.IsAny<bool>()))
                                .ReturnsAsync(new[] { postedEntityGroup }.AsOdata<EntityGroup, int>());

            var mockLogger = new Mock<ILogger>();

            var entity = new Entity2 { Name = "E1", EntityGroup = "G1" };
            var missingEntity = new Missing<Entity2>(entity) { IsMissing = true };
            MissingEntitySettings settings = new MissingEntitySettings();
            settings.Entities.Add(entity.Name, missingEntity);
            var entityGroup = new EntityGroup { Name = "G1" };
            var missingEntityGroup = new Missing<EntityGroup>(entityGroup) { IsMissing = true };
            settings.EntityGroups.Add(entityGroup.Name, missingEntityGroup);

            var unitUnderTest = new EntitySettingsWriter(
                mockEntityClient.Object,
                mockEntityPropClient.Object,
                mockEntityGroupClient.Object,
                mockLogger.Object);
            #endregion

            // Act
            await unitUnderTest.Write(settings, new[] { entityGroup });

            // Assert
            Assert.AreEqual(postedEntityGroup, settings.EntityGroups["G1"].Object);
            mockEntityClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<Entity>>(), It.IsAny<bool>()), Times.Never);
            mockEntityPropClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<EntityProperty>>(), It.IsAny<bool>()), Times.Never);
            mockEntityGroupClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<EntityGroup>>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void EntitySettingsWriter_UpdateEntityGroupIds_Test()
        {
            // Arrange
            #region Arrange
            var mockEntityClient = new Mock<IAdminEntityClientAsync<Entity, int>>();
            var mockEntityPropClient = new Mock<IAdminEntityClientAsync<EntityProperty, int>>();
            var mockEntityGroupClient = new Mock<IAdminEntityClientAsync<EntityGroup, int>>();

            var postedEntityGroup = new EntityGroup { Id = 301, Name = "G1", };
            mockEntityGroupClient.Setup(m => m.PostAsync(It.IsAny<IEnumerable<EntityGroup>>(),
                                                        It.IsAny<bool>()))
                                .ReturnsAsync(new[] { postedEntityGroup }.AsOdata<EntityGroup, int>());

            var mockLogger = new Mock<ILogger>();

            var entity = new Entity2 { Name = "E1", EntityGroup = "G1" };
            var missingEntity = new Missing<Entity2>(entity) { IsMissing = true };
            MissingEntitySettings settings = new MissingEntitySettings();
            settings.Entities.Add(entity.Name, missingEntity);
            var missingEntityGroup = new Missing<EntityGroup>(postedEntityGroup);
            settings.EntityGroups.Add(postedEntityGroup.Name, missingEntityGroup);

            var unitUnderTest = new EntitySettingsWriter(
                mockEntityClient.Object,
                mockEntityPropClient.Object,
                mockEntityGroupClient.Object,
                mockLogger.Object);
            #endregion

            // Act
            unitUnderTest.UpdateEntityGroupIds(settings);

            // Assert
            Assert.AreEqual(301, entity.EntityGroupId);
            mockEntityClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<Entity>>(), It.IsAny<bool>()), Times.Never);
            mockEntityPropClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<EntityProperty>>(), It.IsAny<bool>()), Times.Never);
            mockEntityGroupClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<EntityGroup>>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public async Task EntitySettingsWriter_Write_Entities_Test()
        {
            #region Arrange
            var mockEntityClient = new Mock<IAdminEntityClientAsync<Entity, int>>();
            var postedEntity = new Entity { Id = 101, Name = "E1", EntityGroupId = 301 };
            mockEntityClient.Setup(m => m.PostAsync(It.IsAny<IEnumerable<Entity>>(), It.IsAny<bool>()))
                            .ReturnsAsync(new[] { postedEntity }.AsOdata<Entity, int>());

            var mockEntityPropClient = new Mock<IAdminEntityClientAsync<EntityProperty, int>>();
            var mockEntityGroupClient = new Mock<IAdminEntityClientAsync<EntityGroup, int>>();

            var mockLogger = new Mock<ILogger>();

            var entity = new Entity2 { Name = "E1" };
            var missingEntity = new Missing<Entity2>(entity) { IsMissing = true };
            MissingEntitySettings settings = new MissingEntitySettings();
            settings.Entities.Add(entity.Name, missingEntity);

            var unitUnderTest = new EntitySettingsWriter(
                mockEntityClient.Object,
                mockEntityPropClient.Object,
                mockEntityGroupClient.Object,
                mockLogger.Object);
            #endregion

            // Act
            await unitUnderTest.Write(settings, new[] { entity });

            // Assert
            Assert.AreEqual(postedEntity.Id, entity.Id);
            Assert.AreEqual(postedEntity.EntityGroupId, entity.EntityGroupId);
            mockEntityClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<Entity>>(), It.IsAny<bool>()), Times.Once);
            mockEntityPropClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<EntityProperty>>(), It.IsAny<bool>()), Times.Never);
            mockEntityGroupClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<EntityGroup>>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void UpdateEntityProperties_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mockEntityClient = new Mock<IAdminEntityClientAsync<Entity, int>>();
            var mockEntityGroupClient = new Mock<IAdminEntityClientAsync<EntityGroup, int>>();
            var mockEntityPropClient = new Mock<IAdminEntityClientAsync<EntityProperty, int>>();
            
            var mockLogger = new Mock<ILogger>();
            var entity = new Entity2 { Id = 101, Name = "E1", EntityGroup = "G1" };

            var prop = new EntityProperty
            {
                Name = "Id",
                Type = "System.Int32",
                Order = 1,
                Searchable = true,
            };
            var missingProp = new Missing<IEntityProperty>(prop) { IsMissing = true };
            entity.EntityProperties.Add(prop.Name, missingProp);
            var missingEntity = new Missing<Entity2>(entity) { IsMissing = true };
            MissingEntitySettings settings = new MissingEntitySettings();
            settings.Entities.Add(entity.Name, missingEntity);

            var unitUnderTest = new EntitySettingsWriter(
                mockEntityClient.Object,
                mockEntityPropClient.Object,
                mockEntityGroupClient.Object,
                mockLogger.Object);

            // Act
            unitUnderTest.UpdateEntityProperties(settings);

            // Assert
            Assert.AreEqual(entity.Id, prop.EntityId);
            mockEntityClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<Entity>>(), It.IsAny<bool>()), Times.Never);
            mockEntityPropClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<EntityProperty>>(), It.IsAny<bool>()), Times.Never);
            mockEntityGroupClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<EntityGroup>>(), It.IsAny<bool>()), Times.Never);

        }

        [TestMethod]
        public async Task EntitySettingsWriter_Write_Properties_Test()
        {
            // Arrange
            var mockEntityClient = new Mock<IAdminEntityClientAsync<Entity, int>>();
            var mockEntityGroupClient = new Mock<IAdminEntityClientAsync<EntityGroup, int>>();

            var mockEntityPropClient = new Mock<IAdminEntityClientAsync<EntityProperty, int>>();
            var postedEntityProp = new EntityProperty
            {
                Id = 201,
                Name = "Id",
                Type = "System.Int32",
                Order = 1,
                Searchable = true
            };
            mockEntityPropClient.Setup(m => m.PostAsync(It.IsAny<IEnumerable<EntityProperty>>(),
                                                        It.IsAny<bool>()))
                                .ReturnsAsync(new[] { postedEntityProp }.AsOdata<EntityProperty, int>());

            var mockLogger = new Mock<ILogger>();
            var prop = new EntityProperty
            {
                Name = "Id",
                Type = "System.Int32",
                Order = 1,
                Searchable = true,
            };

            var unitUnderTest = new EntitySettingsWriter(
                mockEntityClient.Object,
                mockEntityPropClient.Object,
                mockEntityGroupClient.Object,
                mockLogger.Object);

            // Act
            await unitUnderTest.Write(new [] { prop });

            // Assert
            mockEntityClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<Entity>>(), It.IsAny<bool>()), Times.Never);
            mockEntityPropClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<EntityProperty>>(), It.IsAny<bool>()), Times.Once);
            mockEntityGroupClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<EntityGroup>>(), It.IsAny<bool>()), Times.Never);
        }
    }
}
