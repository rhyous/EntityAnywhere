using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests.Clients
{
    [TestClass]
    public class ExtensionEntityClientAsyncJsonTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityClientConnectionSettings> _MockEntityClientConnectionSettings;
        private Mock<IHttpClientRunner> _MockHttpClientRunner;
        private MockSetupHelper _MockSetupHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityClientConnectionSettings = _MockRepository.Create<IEntityClientConnectionSettings>();
            _MockHttpClientRunner = _MockRepository.Create<IHttpClientRunner>();
            _MockSetupHelper = new MockSetupHelper(_MockEntityClientConnectionSettings,
                                           _MockHttpClientRunner);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private IExtensionEntityClientAsync CreateExtensionEntityClientAsync()
        {
            return new ExtensionEntityClientAsync(
                _MockEntityClientConnectionSettings.Object,
                _MockHttpClientRunner.Object);
        }

        #region  GetByEntityIdentifiersAsync
        [TestMethod]
        public async Task ExtensionEntityClientAsync_GetByEntityIdentifiersAsync_Ids_Null()
        {
            // Arrange
            var extensionEntityClientAsync = CreateExtensionEntityClientAsync();
            IEnumerable<EntityIdentifier> entityIdentifiers = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await extensionEntityClientAsync.GetByEntityIdentifiersAsync(
                    entityIdentifiers,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task ExtensionEntityClientAsync_GetByEntityIdentifiersAsync_Test()
        {
            // Arrange
            var extensionEntityClientAsync = CreateExtensionEntityClientAsync();
            var entityIdentifier = new EntityIdentifier();
            IEnumerable<EntityIdentifier> entityIdentifiers = new[] { entityIdentifier };
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var odataEntities = _MockSetupHelper.SetupRunTContentMock<IEnumerable<EntityIdentifier>>();

            // Act
            var result = await extensionEntityClientAsync.GetByEntityIdentifiersAsync(
                entityIdentifiers,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }
        #endregion

        #region GetByEntityPropertyValueAsync
        [TestMethod]
        public async Task ExtensionEntityClientAsync_GetByEntityPropertyValueAsync_Entity_Null()
        {
            // Arrange
            var extensionEntityClientAsync = CreateExtensionEntityClientAsync();
            string entity = null;
            string property = "AlternateName";
            string value = "E27";
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await extensionEntityClientAsync.GetByEntityPropertyValueAsync(
                    entity,
                    property,
                    value,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task ExtensionEntityClientAsync_GetByEntityPropertyValueAsync_Property_Null()
        {
            // Arrange
            var extensionEntityClientAsync = CreateExtensionEntityClientAsync();
            string entity = "User";
            string property = null;
            string value = "E27";
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await extensionEntityClientAsync.GetByEntityPropertyValueAsync(
                    entity,
                    property,
                    value,
                    forwardExceptions);
            });
        }



        [TestMethod]
        public async Task ExtensionEntityClientAsync_GetByEntityPropertyValueAsync_Value_Null()
        {
            // Arrange
            var extensionEntityClientAsync = CreateExtensionEntityClientAsync();
            string entity = "User";
            string property = "AlternateName";
            string value = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await extensionEntityClientAsync.GetByEntityPropertyValueAsync(
                    entity,
                    property,
                    value,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task ExtensionEntityClientAsync_GetByEntityPropertyValueAsync_Test()
        {
            // Arrange
            var extensionEntityClientAsync = CreateExtensionEntityClientAsync();
            string entity = "User";
            string property = "AlternateName";
            string value = "E27";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var odataEntities = _MockSetupHelper.SetupRunMock();

            // Act
            var result = await extensionEntityClientAsync.GetByEntityPropertyValueAsync(
                entity,
                property,
                value,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }
        #endregion

        #region GetByEntityPropertyValueAsync
        [TestMethod]
        public async Task ExtensionEntityClientAsync_GetDistinctExtensionPropertyValues_Entity_Null()
        {
            // Arrange
            var extensionEntityClientAsync = CreateExtensionEntityClientAsync();
            string entity = null;
            string property = "Property";
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await extensionEntityClientAsync.GetDistinctExtensionPropertyValuesAsync(
                    entity,
                    property,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task ExtensionEntityClientAsync_GetDistinctExtensionPropertyValues_Property_Null()
        {
            // Arrange
            var extensionEntityClientAsync = CreateExtensionEntityClientAsync();
            string entity = "User";
            string property = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await extensionEntityClientAsync.GetDistinctExtensionPropertyValuesAsync(
                    entity,
                    property,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task ExtensionEntityClientAsync_GetDistinctExtensionPropertyValues_Test()
        {
            // Arrange
            var extensionEntityClientAsync = CreateExtensionEntityClientAsync();
            string entity = "User";
            string property = "Property";
            bool forwardExceptions = false;
            
            _MockSetupHelper.SetupEntityClientSettings();
            List<string> distinctProperties = _MockSetupHelper.SetupRunAndDeserializeList();

            // Act
            var result = await extensionEntityClientAsync.GetDistinctExtensionPropertyValuesAsync(
                entity,
                property,
                forwardExceptions);

            // Assert
            Assert.AreEqual(distinctProperties, result);
        }

        #endregion
    }
}
