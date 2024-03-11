using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityBasic;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityBasic;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class PostHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRelatedEntityEnforcer<TEntity>> _MockRelatedEntityEnforcer;
        private Mock<IDistinctPropertiesEnforcer<TEntity, TInterface, TId>> _MockDistinctPropertiesEnforcer;
        private Mock<IEntityEventAll<TEntity, TId>> _MockEntityEventAll;
        private Mock<IServiceCommon<TEntity, TInterface, TId>> _MockServiceCommon;
        private Mock<IRelatedEntityProvider<TEntity, TInterface, TId>> _MockRelatedEntityProvider;
        private IUrlParameters _UrlParameters;
        private IRequestUri _RequestUri;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRelatedEntityEnforcer = _MockRepository.Create<IRelatedEntityEnforcer<TEntity>>();
            _MockDistinctPropertiesEnforcer = _MockRepository.Create<IDistinctPropertiesEnforcer<TEntity, TInterface, TId>>();
            _MockEntityEventAll = _MockRepository.Create<IEntityEventAll<TEntity, TId>>();
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

        private PostHandler<TEntity, TInterface, TId> CreatePostHandler()
        {
            return new PostHandler<TEntity, TInterface, TId>(
                _MockRelatedEntityEnforcer.Object,
                _MockDistinctPropertiesEnforcer.Object,
                _MockEntityEventAll.Object,
                _MockServiceCommon.Object,
                _MockRelatedEntityProvider.Object,
                _UrlParameters,
                _RequestUri);
        }

        [TestMethod]
        public async Task PostHandler_Handle_NullList_Test()
        {
            // Arrange
            var PostHandler = CreatePostHandler();
            List<TEntity> entities = null;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await PostHandler.HandleAsync(entities);
            });
        }

        [TestMethod]
        public async Task PostHandler_Handle_EmptyList_Test()
        {
            // Arrange
            var PostHandler = CreatePostHandler();
            var entities = new List<TEntity>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await PostHandler.HandleAsync(entities);
            });
        }

        [TestMethod]
        public async Task PostHandler_Handle_CleanAndValidate_ReturnsTrue_Test()
        {
            // Arrange
            var PostHandler = CreatePostHandler();
            var entity1 = new TEntity { Id = 1027, Name = "Name1027" };
            var entity2 = new TEntity { Id = 1028, Name = "Name1028" };
            var entities = new List<TEntity> { entity1, entity2 };

            _MockEntityEventAll.Setup(m => m.BeforePost(It.IsAny<IEnumerable<TEntity>>()));
            _MockEntityEventAll.Setup(m => m.AfterPost(It.IsAny<IEnumerable<TEntity>>()));
            _MockServiceCommon.Setup(m => m.AddAsync(It.IsAny<IList<TInterface>>()))
                              .ReturnsAsync(entities.ToList<TInterface>());
            _MockRelatedEntityEnforcer.Setup(m => m.Enforce(It.IsAny<IEnumerable<TEntity>>(), null))
                                      .Returns(Task.CompletedTask);
            _MockDistinctPropertiesEnforcer.Setup(m => m.Enforce(It.IsAny<IEnumerable<TEntity>>(), It.IsAny<ChangeType>()))
                                           .Returns(Task.CompletedTask);
            _MockRelatedEntityProvider.Setup(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()))
                                      .Returns(Task.CompletedTask);

            // Act
            var actual = await PostHandler.HandleAsync(entities);

            // Assert
            Assert.AreEqual(entities[0], actual[0].Object);
            Assert.AreEqual(entities[1], actual[1].Object);
        }
    }
}
