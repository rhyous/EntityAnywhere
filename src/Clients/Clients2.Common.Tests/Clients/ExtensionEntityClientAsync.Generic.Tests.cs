using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TEntity = Rhyous.EntityAnywhere.Clients2.Common.Tests.EntityInt;
using TId = System.Int32;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests
{
    [TestClass]
    public class ExtensionEntityClientAsyncTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityClientConnectionSettings<TEntity>> _MockEntityClientConnectionSettings;
        private Mock<IHttpClientRunner> _MockHttpClientRunner;
        private MockSetupHelper _MockSetupHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityClientConnectionSettings = _MockRepository.Create<IEntityClientConnectionSettings<TEntity>>();
            _MockHttpClientRunner = _MockRepository.Create<IHttpClientRunner>();
            _MockSetupHelper = new MockSetupHelper(_MockEntityClientConnectionSettings,
                                           _MockHttpClientRunner);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private IExtensionEntityClientAsync<TEntity, TId> CreateExtensionEntityClientAsync()
        {
            return new ExtensionEntityClientAsync<TEntity, TId>(
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
            var odataEntities = _MockSetupHelper.SetupRunAndDeserializeCollectionWithTContent<IEnumerable<EntityIdentifier>>();

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
            var odataEntities = _MockSetupHelper.SetupRunAndDeserializeCollectionMock();

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
    }
}
