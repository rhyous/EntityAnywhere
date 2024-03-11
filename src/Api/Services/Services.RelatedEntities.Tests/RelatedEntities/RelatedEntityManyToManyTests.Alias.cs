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
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.Product;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IProduct;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities.Tests.RelatedEntities
{
    [TestClass]
    public class RelatedEntityManyToManyAliasTests
    {
        private MockRepository _MockRepository;

        private Mock<IRelatedEntityOneToMany<TEntity, TInterface, TId>> _MockRelatedEntityOneToMany;
        private Mock<AttributeEvaluator> _MockAttributeEvaluator;
        private Mock<INamedFactory<IEntityClientAsync>> _MockNamedFactory;
        private Mock<IEntityClientAsync> _MockProductClient;
        private Mock<IEntityClientAsync> _MockSuiteMembershipClient;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            _MockRepository = new MockRepository(MockBehavior.Strict);
            
            _MockRelatedEntityOneToMany = _MockRepository.Create<IRelatedEntityOneToMany<TEntity, TInterface, TId>>();
            _MockAttributeEvaluator = _MockRepository.Create<AttributeEvaluator>();
            _MockNamedFactory = _MockRepository.Create<INamedFactory<IEntityClientAsync>>();
            _MockProductClient = _MockRepository.Create<IEntityClientAsync>();
            _MockSuiteMembershipClient = _MockRepository.Create<IEntityClientAsync>();
        }
        private RelatedEntityManyToMany<TEntity, TInterface, TId> CreateRelatedEntityManyToMany()
        {
            return new RelatedEntityManyToMany<TEntity, TInterface, TId>(
                _MockRelatedEntityOneToMany.Object,
                _MockAttributeEvaluator.Object);
        }

        #region Properties
        public SuiteMembership Membership1 => new SuiteMembership { Id = 1, SuiteId = 10, ProductId = 101 };

        public Product Suite10 => new Product { Id = 10, Name = "Suite10", Version = "1.0", Type = 2 };
        public Product Product101 => new Product { Id = 101, Name = "P101", Version = "1.0.1", Type = 1 };
        public IEnumerable<RelatedEntityAttribute> Attributes { get; set; }
        #endregion

        #region Alias tests
        [TestMethod]
        public async Task GetRelatedEntities_Entity_Without_Alias_Test()
        {
            // Arrange
            var expandPaths = new List<ExpandPath> { new ExpandPath { Entity = "Product" } };
            var relatedEntityManager = CreateRelatedEntityManyToMany();

            var odataSuite10 = new[] { Suite10 }.AsOdata<Product, int>();
            RelatedEntityCollection re10 = odataSuite10;
            re10.RelatedEntity = "Suite";
            var jsonSuite10 = JsonConvert.SerializeObject(odataSuite10);
            var odataProduct101 = new[] { Product101 }.AsOdata<Product, int>();
            var jsonProduct101 = JsonConvert.SerializeObject(odataProduct101);

            var membership1 = new SuiteMembership { Id = 1, SuiteId = 10, ProductId = 101 };

            var odataMembershipA = new[] { membership1 }.AsOdata<SuiteMembership, int>();
            odataMembershipA.First().RelatedEntityCollection.Add(re10);
            var jsonMembershipA = JsonConvert.SerializeObject(odataMembershipA);

            var odataMembershipB = new[] { membership1 }.AsOdata<SuiteMembership, int>();
            odataMembershipB.First().RelatedEntityCollection.Add(odataProduct101);
            var jsonMembershipB = JsonConvert.SerializeObject(odataMembershipB);

            _MockSuiteMembershipClient.Setup(c => c.GetByPropertyValuesAsync(It.Is<string>(e => e == "SuiteId"),
                                                  It.IsAny<IEnumerable<string>>(),
                                                  It.IsAny<string>(), false)).ReturnsAsync(jsonMembershipB);
            var mockRelatedEntitySorterHelper = _MockRepository.Create<IRelatedEntitySorterHelper<TInterface, TId>>();
            var relatedEntitySorterHelper = new RelatedEntitySorterHelper<TInterface, TId>(new RelatedEntitySorterWrapper<TInterface, TId>());
            mockRelatedEntitySorterHelper.Setup(m => m.Sort(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<RelatedEntityCollection>(), It.IsAny<SortDetails>(), It.IsAny<List<RelatedEntityCollection>>()))
                                         .Callback((IEnumerable<TInterface> ts, RelatedEntityCollection c, SortDetails sd, List<RelatedEntityCollection> list) =>
                                         {
                                             relatedEntitySorterHelper.Sort(ts, c, sd, list);
                                         });
            var relatedEntityOneToMany = new RelatedEntityOneToMany<TEntity, TInterface, TId>(_MockNamedFactory.Object,
                                                                                              _MockAttributeEvaluator.Object,
                                                                                              mockRelatedEntitySorterHelper.Object);
            _MockRelatedEntityOneToMany.Setup(m => m.GetRelatedEntitiesAsync(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<IEnumerable<ExpandPath>>()))
                                       .ReturnsAsync((IEnumerable<TInterface> entities, IEnumerable<ExpandPath> expandPaths1) =>
                                       {
                                           return relatedEntityOneToMany.GetRelatedEntitiesAsync(entities, expandPaths1).Result;
                                       });
            _MockNamedFactory.Setup(m => m.Create("SuiteMembership")).Returns(_MockSuiteMembershipClient.Object);
            
            // Act
            var actual = await relatedEntityManager.GetRelatedEntitiesAsync(new[] { Suite10 }, expandPaths);

            // Assert
            Assert.AreEqual(1, actual.Count);

            Assert.AreEqual("Suite", actual[0].Entity);
            Assert.AreEqual("10", actual[0].EntityId);
            Assert.AreEqual("Product", actual[0].RelatedEntity);
            Assert.AreEqual(1, actual[0].Count);
            Assert.AreEqual("101", actual[0][0].Id);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetRelatedEntities_Entity_WithOnly_Alias_Test()
        {
            // Arrange
            var expandPaths = new List<ExpandPath> { new ExpandPath { Entity = "Suite" } };
            var relatedEntityManager = CreateRelatedEntityManyToMany();

            var odataSuite10 = new[] { Suite10 }.AsOdata<Product, int>();
            RelatedEntityCollection re10 = odataSuite10;
            re10.RelatedEntity = "Suite";
            var jsonSuite10 = JsonConvert.SerializeObject(odataSuite10);
            var odataProduct101 = new[] { Product101 }.AsOdata<Product, int>();
            var jsonProduct101 = JsonConvert.SerializeObject(odataProduct101);

            var membership1 = new SuiteMembership { Id = 1, SuiteId = 10, ProductId = 101 };

            var odataMembershipA = new[] { membership1 }.AsOdata<SuiteMembership, int>();
            odataMembershipA.First().RelatedEntityCollection.Add(re10);
            var jsonMembershipA = JsonConvert.SerializeObject(odataMembershipA);

            var odataMembershipB = new[] { membership1 }.AsOdata<SuiteMembership, int>();
            odataMembershipB.First().RelatedEntityCollection.Add(odataProduct101);
            var jsonMembershipB = JsonConvert.SerializeObject(odataMembershipB);

            _MockSuiteMembershipClient.Setup(c => c.GetByPropertyValuesAsync(It.Is<string>(e => e == "ProductId"),
                                                  It.IsAny<IEnumerable<string>>(),
                                                  It.IsAny<string>(), false)).ReturnsAsync(jsonMembershipA);
            var mockRelatedEntitySorterHelper = _MockRepository.Create<IRelatedEntitySorterHelper<TInterface, TId>>();
            var relatedEntitySorterHelper = new RelatedEntitySorterHelper<TInterface, TId>(new RelatedEntitySorterWrapper<TInterface, TId>());
            mockRelatedEntitySorterHelper.Setup(m => m.Sort(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<RelatedEntityCollection>(), It.IsAny<SortDetails>(), It.IsAny<List<RelatedEntityCollection>>()))
                                         .Callback((IEnumerable<TInterface> ts, RelatedEntityCollection c, SortDetails sd, List<RelatedEntityCollection> list) =>
                                         {
                                             relatedEntitySorterHelper.Sort(ts, c, sd, list);
                                         });
            var relatedEntityOneToMany = new RelatedEntityOneToMany<TEntity, TInterface, TId>(_MockNamedFactory.Object,
                                                                                              _MockAttributeEvaluator.Object,
                                                                                              mockRelatedEntitySorterHelper.Object);
            _MockRelatedEntityOneToMany.Setup(m => m.GetRelatedEntitiesAsync(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<IEnumerable<ExpandPath>>()))
                                       .ReturnsAsync((IEnumerable<TInterface> entities, IEnumerable<ExpandPath> expandPaths1) =>
                                       {
                                           return relatedEntityOneToMany.GetRelatedEntitiesAsync(entities, expandPaths1).Result;
                                       });
            _MockNamedFactory.Setup(m => m.Create("SuiteMembership")).Returns(_MockSuiteMembershipClient.Object);

            // Act
            var actual = await relatedEntityManager.GetRelatedEntitiesAsync(new[] { Product101 }, expandPaths);

            // Assert
            Assert.AreEqual(1, actual.Count);

            Assert.AreEqual("Product", actual[0].Entity);
            Assert.AreEqual("101", actual[0].EntityId);
            Assert.AreEqual("Suite", actual[0].RelatedEntity);
            Assert.AreEqual(1, actual[0].Count);
            Assert.AreEqual("10", actual[0][0].Id);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
