using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Services;
using System;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.AltKeyEntity;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IAltKeyEntity;
using TId = System.Int32;
using TAltKey = System.String;
using System.Collections.Generic;
using System.Linq;
using Rhyous.EntityAnywhere.Interfaces;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    [TestClass]
    public class ServiceCommonAlternateKeyTests
    {
        private MockRepository _MockRepository;

        private Mock<IServiceHandlerProviderAltKey<TEntity, TInterface, TId, TAltKey>> _MockServiceHandlerProviderAltKey;
        private Mock<IAddAltKeyHandler<TEntity, TInterface, TId, TAltKey>> _MockAddAltKeyHandler;
        private Mock<IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>> _MockGetByAlternateKeyHandler;
        private Mock<ISearchByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>> _MockSearchByAlternateKeyHandler;
        private Mock<IUpdateAltKeyHandler<TEntity, TInterface, TId, TAltKey>> _MockUpdateAltKeyHandler;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockServiceHandlerProviderAltKey = _MockRepository.Create<IServiceHandlerProviderAltKey<TEntity, TInterface, TId, TAltKey>>();
            _MockAddAltKeyHandler = _MockRepository.Create<IAddAltKeyHandler<TEntity, TInterface, TId, TAltKey>>();
            _MockGetByAlternateKeyHandler = _MockRepository.Create<IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>>();
            _MockSearchByAlternateKeyHandler = _MockRepository.Create<ISearchByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>>();
            _MockUpdateAltKeyHandler = _MockRepository.Create<IUpdateAltKeyHandler<TEntity, TInterface, TId, TAltKey>>();
        }

        private ServiceCommonAlternateKey<TEntity, TInterface, TId, TAltKey> CreateService()
        {
            return new ServiceCommonAlternateKey<TEntity, TInterface, TId, TAltKey>(
                _MockServiceHandlerProviderAltKey.Object);
        }

        #region GetByAlternateKey
        [TestMethod]
        public void ServiceCommonAlternateKey_GetByAlternateKey_ServiceProvider_Test()
        {
            // Arrange
            var serviceCommonAlternateKey = CreateService();
            _MockServiceHandlerProviderAltKey.Setup(m => m.GetByAlternateKeyHandler)
                                             .Returns(_MockGetByAlternateKeyHandler.Object);
            TAltKey propertyValue = default(TAltKey);
            var returnedEntity = new TEntity();
            _MockGetByAlternateKeyHandler.Setup(m => m.Get(propertyValue))
                                         .Returns(returnedEntity);

            // Act
            var result = serviceCommonAlternateKey.GetByAlternateKey(propertyValue);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Search
        [TestMethod]
        public void ServiceCommonAlternateKey_Search_ServiceProvider_Test()
        {
            // Arrange
            var serviceCommonAlternateKey = CreateService();
            _MockServiceHandlerProviderAltKey.Setup(m => m.SearchByAlternateKeyHandler)
                                             .Returns(_MockSearchByAlternateKeyHandler.Object);
            TAltKey searchValue = default(TAltKey);
            var returnedEntities = new List<TInterface>();
            _MockSearchByAlternateKeyHandler.Setup(m => m.Search(searchValue))
                                            .Returns(returnedEntities);

            // Act
            var result = serviceCommonAlternateKey.Search(searchValue);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Add
        [TestMethod]
        public async Task ServiceCommonAlternateKey_Add_ServiceProvider_Test()
        {
            // Arrange
            var serviceCommonAlternateKey = CreateService();
            _MockServiceHandlerProviderAltKey.Setup(m => m.AddAltKeyHandler)
                                             .Returns(_MockAddAltKeyHandler.Object);
            IEnumerable<TInterface> entities = new List<TInterface>();
            _MockAddAltKeyHandler.Setup(m=>m.AddAsync(entities))
                                 .ReturnsAsync(entities.ToList());

            // Act
            var result = await serviceCommonAlternateKey.AddAsync(entities);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Update(TId id, PatchedEntity<TInterface, TId> patchedEntity)
        [TestMethod]
        public void ServiceCommonAlternateKey_Update_PatchedEntity_ServiceProvider_Test()
        {
            // Arrange
            var serviceCommonAlternateKey = CreateService();
            _MockServiceHandlerProviderAltKey.Setup(m => m.UpdateAltKeyHandler)
                                             .Returns(_MockUpdateAltKeyHandler.Object);
            TId id = default(TId);
            var entity = new TEntity();
            PatchedEntity<TInterface, TId> patchedEntity = new PatchedEntity<TInterface, TId> { Entity = entity };
            _MockUpdateAltKeyHandler.Setup(m => m.Update(id, patchedEntity))
                                    .Returns(entity);

            // Act
            var result = serviceCommonAlternateKey.Update(id, patchedEntity);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Update(PatchedEntityCollection<TInterface, TId> patchedEntityCollection)
        [TestMethod]
        public void ServiceCommonAlternateKey_Update_PatchedEntityCollection_ServiceProvider_Test()
        {
            // Arrange
            var serviceCommonAlternateKey = CreateService();
            _MockServiceHandlerProviderAltKey.Setup(m => m.UpdateAltKeyHandler)
                                             .Returns(_MockUpdateAltKeyHandler.Object);
            var entity = new TEntity();
            var returned = new List<TInterface>(); 
            PatchedEntity<TInterface, TId> patchedEntity = new PatchedEntity<TInterface, TId> { Entity = entity };
            PatchedEntityCollection<TInterface, TId> patchedEntityCollection = new PatchedEntityCollection<TInterface, TId>();
            patchedEntityCollection.PatchedEntities.Add(patchedEntity);
            _MockUpdateAltKeyHandler.Setup(m => m.Update(patchedEntityCollection))
                                    .Returns(returned);

            // Act
            var result = serviceCommonAlternateKey.Update(patchedEntityCollection);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
