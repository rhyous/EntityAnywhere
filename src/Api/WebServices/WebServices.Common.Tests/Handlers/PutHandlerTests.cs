using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityBasic;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityBasic;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class PutHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRelatedEntityEnforcer<TEntity>> _MockRelatedEntityEnforcer;
        private Mock<IDistinctPropertiesEnforcer<TEntity, TInterface, TId>> _MockDistinctPropertiesEnforcer;
        private Mock<IEntityEventAll<TEntity, TId>> _MockEntityEventAll;
        private Mock<IInputValidator<TEntity, TId>> _MockInputValidator;
        private Mock<IServiceCommon<TEntity, TInterface, TId>> _MockServiceCommon;
        private Mock<IRelatedEntityProvider<TEntity, TInterface, TId>> _MockRelatedEntityProvider;
        private UrlParameters _UrlParameters;
        private IRequestUri _RequestUri;

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
            _UrlParameters = new UrlParameters();
            _RequestUri = new RequestUri { Uri = new Uri("https://fake.domain.tld/Api/EntityBasicService/EntityBasics") };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private PutHandler<TEntity, TInterface, TId> CreatePutHandler()
        {
            return new PutHandler<TEntity, TInterface, TId>(
                _MockRelatedEntityEnforcer.Object,
                _MockDistinctPropertiesEnforcer.Object,
                _MockEntityEventAll.Object,
                _MockInputValidator.Object,
                _MockServiceCommon.Object,
                _MockRelatedEntityProvider.Object,
                _UrlParameters,
                _RequestUri);
        }

        [TestMethod]
        public async Task PutHandler_Handle_CleanAndValidate_ReturnsFalse_Test()
        {
            // Arrange
            var PutHandler = CreatePutHandler();
            string id = null;
            var entity = new TEntity { Id = 1027, Name = "Name1027" };
            _MockInputValidator.Setup(m => m.CleanAndValidate(ref It.Ref<string>.IsAny, It.IsAny<TEntity>()))
                               .Returns(false);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await PutHandler.Handle(id, entity);
            });
        }

        [TestMethod]
        public async Task PutHandler_Handle_CleanAndValidate_ReturnsTrue_Test()
        {
            // Arrange
            var PostHandler = CreatePutHandler();
            var entityBefore = new TEntity { Id = 1028, Name = "Name2028" };
            var entityAfter = new TEntity { Id = 1028, Name = "Name1028" };

            _MockInputValidator.Setup(m => m.CleanAndValidate(ref It.Ref<string>.IsAny, It.IsAny<TEntity>()))
                   .Returns(true);
            _MockEntityEventAll.Setup(m => m.BeforePut(It.IsAny<TEntity>(), It.IsAny<TEntity>()));
            _MockEntityEventAll.Setup(m => m.AfterPut(It.IsAny<TEntity>(), It.IsAny<TEntity>()));
            _MockServiceCommon.Setup(m => m.Get(It.IsAny<TId>()))
                              .Returns(entityBefore);
            _MockServiceCommon.Setup(m => m.Replace(It.IsAny<TId>(), It.IsAny<TInterface>()))
                              .Returns(entityAfter);
            _MockRelatedEntityEnforcer.Setup(m => m.Enforce(It.IsAny<IEnumerable<TEntity>>(), null))
                                      .Returns(Task.CompletedTask);
            _MockDistinctPropertiesEnforcer.Setup(m => m.Enforce(It.IsAny<IEnumerable<TEntity>>(), It.IsAny<ChangeType>()))
                                           .Returns(Task.CompletedTask);
            _MockRelatedEntityProvider.Setup(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()))
                                      .Returns(Task.CompletedTask);

            // Act
            var actual = await PostHandler.Handle("1028", entityBefore);

            // Assert
            Assert.AreEqual(entityAfter, actual.Object);
        }
    }
}
