using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Services.Common.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.SuiteMembership;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.ISuiteMembership;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities.Tests.RelatedEntities
{
    [TestClass]
    public class RelatedEntityManyToOneAliasTests
    {
        private MockRepository _MockRepository;

        private Mock<INamedFactory<IEntityClientAsync>> _MockNamedFactory;
        private Mock<AttributeEvaluator> _MockAttributeEvaluator;
        private Mock<IRelatedEntitySorterHelper<TInterface, TId>> _MockRelatedEntitySorterHelper;
        private Mock<IEntityClientAsync> _MockClient;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockNamedFactory = _MockRepository.Create<INamedFactory<IEntityClientAsync>>();
            _MockAttributeEvaluator = _MockRepository.Create<AttributeEvaluator>();
            _MockRelatedEntitySorterHelper = _MockRepository.Create<IRelatedEntitySorterHelper<TInterface, TId>>();
            _MockClient = _MockRepository.Create<IEntityClientAsync>();
            _MockNamedFactory.Setup(m=>m.Create("Product")).Returns(_MockClient.Object);
        }

        private RelatedEntityManyToOne<TEntity, TInterface, TId> CreateRelatedEntityManyToOne()
        {
            return new RelatedEntityManyToOne<TEntity, TInterface, TId>(
                _MockNamedFactory.Object,
                _MockAttributeEvaluator.Object,
                _MockRelatedEntitySorterHelper.Object);
        }

        #region Alias tests
        [TestMethod]
        public async Task GetRelatedEntities_Entity_Without_Alias_Test()
        {
            // Arrange
            var expandPaths = new List<ExpandPath> { new ExpandPath { Entity = "Product" } };
            var relatedEntityManager = CreateRelatedEntityManyToOne();

            var evaluator = new AttributeEvaluator();
            _MockClient.Setup(m => m.Entity).Returns("Product");
            var product10 = new Product { Id = 10, Name = "Suite10", Version = "1.0", Type = 2 };
            var product101 = new Product { Id = 101, Name = "P101", Version = "1.0.1", Type = 1 };
            var odata10 = new[] { product10 }.AsOdata<Product, int>();
            var json10 = JsonConvert.SerializeObject(odata10);
            var odata101 = new[] { product101 }.AsOdata<Product, int>();
            var json101 = JsonConvert.SerializeObject(odata101);

            _MockClient.Setup(c => c.GetByIdsAsync(It.Is<IEnumerable<string>>(ids => ids.First() == "10"),
                                                  It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(json10);
            _MockClient.Setup(c => c.GetByIdsAsync(It.Is<IEnumerable<string>>(ids => ids.First() == "101"),
                                                  It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(json101);
            var sorter = new RelatedEntitySorterHelper<ISuiteMembership, int>(new RelatedEntitySorterWrapper<ISuiteMembership, int>());
            _MockRelatedEntitySorterHelper.Setup(m => m.Sort(It.IsAny<IEnumerable<ISuiteMembership>>(), It.IsAny<RelatedEntityCollection>(), It.IsAny<SortDetails>(), It.IsAny<List<RelatedEntityCollection>>()))
                                         .Callback((IEnumerable<ISuiteMembership> sms, RelatedEntityCollection c, SortDetails sd, List<RelatedEntityCollection> list) =>
                                         {
                                             sorter.Sort(sms, c, sd, list);
                                         });
            var membership1 = new SuiteMembership { Id = 1, SuiteId = 10, ProductId = 101 };

            // Act
            var actual = await relatedEntityManager.GetRelatedEntitiesAsync(new[] { membership1 }, expandPaths);

            // Assert
            Assert.AreEqual(1, actual.Count);

            Assert.AreEqual("SuiteMembership", actual[0].Entity);
            Assert.AreEqual("1", actual[0].EntityId);
            Assert.AreEqual("Product", actual[0].RelatedEntity);
            Assert.AreEqual(1, actual[0].Count);
            Assert.AreEqual("101", actual[0][0].Id);
        }

        [TestMethod]
        public async Task GetRelatedEntities_Entity_WithOnly_Alias_Test()
        {
            // Arrange
            var expandPaths = new List<ExpandPath> { new ExpandPath { Entity = "Suite" } };
            var relatedEntityManager = CreateRelatedEntityManyToOne();

            var evaluator = new AttributeEvaluator();
            _MockClient.Setup(m => m.Entity).Returns("Product");
            var product10 = new Product { Id = 10, Name = "Suite10", Version = "1.0", Type = 2 };
            var product101 = new Product { Id = 101, Name = "P101", Version = "1.0.1", Type = 1 };
            var odata10 = new[] { product10 }.AsOdata<Product, int>();
            var json10 = JsonConvert.SerializeObject(odata10);
            var odata101 = new[] { product101 }.AsOdata<Product, int>();
            var json101 = JsonConvert.SerializeObject(odata101);

            _MockClient.Setup(c => c.GetByIdsAsync(It.Is<IEnumerable<string>>(ids => ids.First() == "10"),
                                                  It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(json10);
            _MockClient.Setup(c => c.GetByIdsAsync(It.Is<IEnumerable<string>>(ids => ids.First() == "101"),
                                                  It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(json101);
            var sorter = new RelatedEntitySorterHelper<ISuiteMembership, int>(new RelatedEntitySorterWrapper<ISuiteMembership, int>());
            _MockRelatedEntitySorterHelper.Setup(m => m.Sort(It.IsAny<IEnumerable<ISuiteMembership>>(), It.IsAny<RelatedEntityCollection>(), It.IsAny<SortDetails>(), It.IsAny<List<RelatedEntityCollection>>()))
                                         .Callback((IEnumerable<ISuiteMembership> sms, RelatedEntityCollection c, SortDetails sd, List<RelatedEntityCollection> list) =>
                                         {
                                             sorter.Sort(sms, c, sd, list);
                                         });
            var membership1 = new SuiteMembership { Id = 1, SuiteId = 10, ProductId = 101 };

            // Act
            var actual = await relatedEntityManager.GetRelatedEntitiesAsync(new[] { membership1 }, expandPaths);

            // Assert
            Assert.AreEqual(1, actual.Count);

            Assert.AreEqual("SuiteMembership", actual[0].Entity);
            Assert.AreEqual("1", actual[0].EntityId);
            Assert.AreEqual("Suite", actual[0].RelatedEntity);
            Assert.AreEqual(1, actual[0].Count);
            Assert.AreEqual("10", actual[0][0].Id);
        }

        [TestMethod]
        public async Task GetRelatedEntities_Both_Entity_And_Alias_Test()
        {
            // Arrange
            var expandPaths = new List<ExpandPath> { new ExpandPath { Entity = "Product" }, new ExpandPath { Entity = "Suite" } };
            var relatedEntityManager = CreateRelatedEntityManyToOne();

            var evaluator = new AttributeEvaluator();
            _MockClient.Setup(m => m.Entity).Returns("Product");
            var product10 = new Product { Id = 10, Name = "Suite10", Version = "1.0", Type = 2 };
            var product101 = new Product { Id = 101, Name = "P101", Version = "1.0.1", Type = 1 };
            var odata10 = new[] { product10 }.AsOdata<Product, int>();
            var json10 = JsonConvert.SerializeObject(odata10);
            var odata101 = new[] { product101 }.AsOdata<Product, int>();
            var json101 = JsonConvert.SerializeObject(odata101);

            _MockClient.Setup(c => c.GetByIdsAsync(It.Is<IEnumerable<string>>(ids => ids.First() == "10"),
                                                  It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(json10);
            _MockClient.Setup(c => c.GetByIdsAsync(It.Is<IEnumerable<string>>(ids => ids.First() == "101"),
                                                  It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(json101);
            var sorter = new RelatedEntitySorterHelper<ISuiteMembership, int>(new RelatedEntitySorterWrapper<ISuiteMembership, int>());
            _MockRelatedEntitySorterHelper.Setup(m => m.Sort(It.IsAny<IEnumerable<ISuiteMembership>>(), It.IsAny<RelatedEntityCollection>(), It.IsAny<SortDetails>(), It.IsAny<List<RelatedEntityCollection>>()))
                                         .Callback((IEnumerable<ISuiteMembership> sms, RelatedEntityCollection c, SortDetails sd, List<RelatedEntityCollection> list) =>
                                         {
                                             sorter.Sort(sms, c, sd, list);
                                         });
            var membership1 = new SuiteMembership { Id = 1, SuiteId = 10, ProductId = 101 };

            // Act
            var actual = await relatedEntityManager.GetRelatedEntitiesAsync(new[] { membership1 }, expandPaths);

            // Assert
            Assert.AreEqual(2, actual.Count);

            Assert.AreEqual("SuiteMembership", actual[0].Entity);
            Assert.AreEqual("1", actual[0].EntityId);
            Assert.AreEqual("Suite", actual[0].RelatedEntity);
            Assert.AreEqual(1, actual[0].Count);
            Assert.AreEqual("10", actual[0][0].Id);

            Assert.AreEqual("SuiteMembership", actual[1].Entity);
            Assert.AreEqual("1", actual[1].EntityId);
            Assert.AreEqual("Product", actual[1].RelatedEntity);
            Assert.AreEqual(1, actual[1].Count);
            Assert.AreEqual("101", actual[1][0].Id);
        }
        #endregion
    }
}
