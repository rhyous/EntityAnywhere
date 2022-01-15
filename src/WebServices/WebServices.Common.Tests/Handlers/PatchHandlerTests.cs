using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityBasic;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityBasic;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class PatchHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRelatedEntityEnforcer<TEntity>> _MockRelatedEntityEnforcer;
        private Mock<IDistinctPropertiesEnforcer<TEntity, TInterface, TId>> _MockDistinctPropertiesEnforcer;
        private Mock<IEntityEventAll<TEntity, TId>> _MockEntityEventAll;
        private Mock<IInputValidator<TEntity, TId>> _MockInputValidator;
        private Mock<IServiceCommon<TEntity, TInterface, TId>> _MockServiceCommon;
        private Mock<IRelatedEntityProvider<TEntity, TInterface, TId>> _MockRelatedEntityProvider;
        private Mock<IEntityInfo<TEntity>> _MockEntityInfo;
        private IUrlParameters _UrlParameters;
        private RequestUri _RequestUri;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRelatedEntityEnforcer = _MockRepository.Create<IRelatedEntityEnforcer<TEntity>>();
            _MockDistinctPropertiesEnforcer = _MockRepository.Create<IDistinctPropertiesEnforcer<TEntity, TInterface, TId>>();
            _MockEntityEventAll = _MockRepository.Create<IEntityEventAll<TEntity, TId>>();
            _MockInputValidator = _MockRepository.Create<IInputValidator<TEntity, TId>>();
            _MockServiceCommon = _MockRepository.Create<IServiceCommon<TEntity, TInterface, TId>>();
            _MockRelatedEntityProvider = _MockRepository.Create<IRelatedEntityProvider<TEntity, TInterface, TId>>();
            _MockEntityInfo = _MockRepository.Create<IEntityInfo<TEntity>>();
            _UrlParameters = new UrlParameters { Collection = new NameValueCollection() };
            _RequestUri = new RequestUri { Uri = new Uri("https://fake.domain.tld/Api/EntityBasicService/EntityBasics") };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private PatchHandler<TEntity, TInterface, TId> CreatePatchHandler()
        {
            return new PatchHandler<TEntity, TInterface, TId>(
                _MockRelatedEntityEnforcer.Object,
                _MockDistinctPropertiesEnforcer.Object,
                _MockEntityEventAll.Object,
                _MockInputValidator.Object,
                _MockServiceCommon.Object,
                _MockRelatedEntityProvider.Object,
                _MockEntityInfo.Object,
                _UrlParameters,
                _RequestUri);
        }

        [TestMethod]
        public async Task PatchHandler_Handle_CleanAndValidate_ReturnsFalse_Test()
        {
            // Arrange
            var patchHandler = CreatePatchHandler();
            string id = null;
            PatchedEntity<TEntity, TId> patchedEntity = null;
            _MockInputValidator.Setup(m => m.CleanAndValidate(ref It.Ref<string>.IsAny, It.IsAny<PatchedEntity<TEntity, TId>>()))
                               .Returns(false);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () => 
            {
                await patchHandler.HandleAsync(id, patchedEntity);
            });
        }

        [TestMethod]
        public async Task PatchHandler_Handle_CleanAndValidate_ReturnsTrue_Test()
        {
            // Arrange
            string id = "1027";
            var priorEntity = new TEntity { Id = 1027, Name = "NewName" };
            var changedEntity = new TEntity { Id = 1027, Name = "NewName" };
            var changedProperties = new HashSet<string> { "Name" };
            PatchedEntity<TEntity, TId> patchedEntity = new PatchedEntity<TEntity, TId> { Entity = changedEntity, ChangedProperties = changedProperties };
            _MockInputValidator.Setup(m => m.CleanAndValidate(ref It.Ref<string>.IsAny, It.IsAny<PatchedEntity<TEntity, TId>>()))
                               .Returns(true);
            _MockEntityEventAll.Setup(m => m.BeforePatch(It.IsAny<PatchedEntityComparison<TEntity, TId>>()));
            _MockEntityEventAll.Setup(m => m.AfterPatch(It.IsAny<PatchedEntityComparison<TEntity, TId>>()));
            _MockServiceCommon.Setup(m => m.Get(It.IsAny<TId>()))
                              .Returns(priorEntity);
            _MockServiceCommon.Setup(m => m.Update(It.IsAny<TId>(), It.IsAny<PatchedEntity<TInterface, TId>>()))
                              .Returns((TId inId, PatchedEntity<TInterface, TId> pe) => 
                              {
                                  return pe.Entity;
                              });
            _MockRelatedEntityEnforcer.Setup(m => m.Enforce(It.IsAny<IEnumerable<TEntity>>(), changedProperties))
                                      .Returns(Task.CompletedTask);
            _MockDistinctPropertiesEnforcer.Setup(m => m.Enforce(It.IsAny<IEnumerable<TEntity>>(), It.IsAny<ChangeType>()))
                                           .Returns(Task.CompletedTask);
            _MockRelatedEntityProvider.Setup(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()))
                                      .Returns(Task.CompletedTask);

            var props = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                       .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
            _MockEntityInfo.Setup(m => m.Properties).Returns(props);

            var patchHandler = CreatePatchHandler();

            // Act
            var actual = await patchHandler.HandleAsync(id, patchedEntity);

            // Assert
            Assert.AreEqual(changedEntity.Name, actual.Object.Name);
            _MockRepository.VerifyAll();
        }
    }
}
