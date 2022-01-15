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
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.Item;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IItem;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities.Tests.RelatedEntities
{
    [TestClass]
    public class RelatedEntityOneToManyAliasTests
    {
        private MockRepository _MockRepository;

        private Mock<INamedFactory<IEntityClientAsync>> _MockNamedFactory;
        private Mock<IEntityClientAsync> _MockClient;
        private Mock<AttributeEvaluator> _MockAttributeEvaluator;
        private Mock<IRelatedEntitySorterHelper<TInterface, TId>> _MockRelatedEntitySorterHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockNamedFactory = _MockRepository.Create<INamedFactory<IEntityClientAsync>>();
            _MockClient = _MockRepository.Create<IEntityClientAsync>();
            _MockAttributeEvaluator = _MockRepository.Create<AttributeEvaluator>();
            _MockRelatedEntitySorterHelper = _MockRepository.Create<IRelatedEntitySorterHelper<TInterface, TId>>();
            _MockNamedFactory.Setup(m=>m.Create("ItemPair")).Returns(_MockClient.Object);
        }

        private RelatedEntityOneToMany<TEntity, TInterface, TId> CreateRelatedEntityOneToMany()
        {
            return new RelatedEntityOneToMany<TEntity, TInterface, TId>(
                _MockNamedFactory.Object,
                _MockAttributeEvaluator.Object,
                _MockRelatedEntitySorterHelper.Object);
        }

        #region Properties
        #endregion

        #region Alias tests
        [TestMethod]
        public async Task GetRelatedEntities_Entity_Without_Alias_Test()
        {
            // Arrange
            var expandPaths = new List<ExpandPath> { new ExpandPath { Entity = "ItemPair" } };
            var relatedEntityManager = CreateRelatedEntityOneToMany();

            var item10 = new TEntity { Id = 10, Name = "Item10" };
            var evaluator = new AttributeEvaluator();

            var itemPair5 = new ItemPair { Id = 5, Name = "Pair5", ItemId = 10, OtherItemId = 20 };

            var odataItemPair5 = new[] { itemPair5 }.AsOdata<ItemPair, int>();
            var jsonItemPair5 = JsonConvert.SerializeObject(odataItemPair5);

            _MockClient.Setup(c => c.GetByPropertyValuesAsync(It.Is<string>(p => p == "ItemId"), It.Is<IEnumerable<string>>(ids => ids.First() == "10"),
                                                  It.IsAny<string>(), false)).ReturnsAsync(jsonItemPair5);

            var sorter = new RelatedEntitySorterHelper<TInterface, TId>(new RelatedEntitySorterWrapper<TInterface, TId>());
            _MockRelatedEntitySorterHelper.Setup(m => m.Sort(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<RelatedEntityCollection>(), It.IsAny<SortDetails>(), It.IsAny<List<RelatedEntityCollection>>()))
                                         .Callback((IEnumerable<TInterface> ts, RelatedEntityCollection c, SortDetails sd, List<RelatedEntityCollection> list) =>
                                         {
                                             sorter.Sort(ts, c, sd, list);
                                         });
            // Act
            var actual = await relatedEntityManager.GetRelatedEntitiesAsync(new[] { item10 }, expandPaths);

            // Assert
            Assert.AreEqual(1, actual.Count);

            Assert.AreEqual("Item", actual[0].Entity);
            Assert.AreEqual("10", actual[0].EntityId);
            Assert.AreEqual("ItemPair", actual[0].RelatedEntity);
            Assert.AreEqual(1, actual[0].Count);
            Assert.AreEqual("5", actual[0][0].Id);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetRelatedEntities_Entity_WithOnly_Alias_Test()
        {
            // Arrange
            var expandPaths = new List<ExpandPath> { new ExpandPath { Entity = "OtherItemPair" } };
            var relatedEntityManager = CreateRelatedEntityOneToMany();

            var item10 = new TEntity { Id = 10, Name = "Item10" };
            var evaluator = new AttributeEvaluator();

            var itemPair6 = new ItemPair { Id = 6, Name = "Pair6", ItemId = 20, OtherItemId = 10 };


            var odataItemPair6 = new[] { itemPair6 }.AsOdata<ItemPair, int>();
            var jsonItemPair6 = JsonConvert.SerializeObject(odataItemPair6);

            _MockClient.Setup(c => c.GetByPropertyValuesAsync(It.Is<string>(p => p == "OtherItemId"), It.Is<IEnumerable<string>>(ids => ids.First() == "10"),
                                                  It.IsAny<string>(), false)).ReturnsAsync(jsonItemPair6);

            var sorter = new RelatedEntitySorterHelper<TInterface, TId>(new RelatedEntitySorterWrapper<TInterface, TId>());
            _MockRelatedEntitySorterHelper.Setup(m => m.Sort(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<RelatedEntityCollection>(), It.IsAny<SortDetails>(), It.IsAny<List<RelatedEntityCollection>>()))
                                         .Callback((IEnumerable<TInterface> ts, RelatedEntityCollection c, SortDetails sd, List<RelatedEntityCollection> list) =>
                                         {
                                             sorter.Sort(ts, c, sd, list);
                                         });

            // Act
            var actual = await relatedEntityManager.GetRelatedEntitiesAsync(new[] { item10 }, expandPaths);

            // Assert
            Assert.AreEqual(1, actual.Count);

            Assert.AreEqual("Item", actual[0].Entity);
            Assert.AreEqual("10", actual[0].EntityId);
            Assert.AreEqual("OtherItemPair", actual[0].RelatedEntity);
            Assert.AreEqual(1, actual[0].Count);
            Assert.AreEqual("6", actual[0][0].Id);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetRelatedEntities_Both_Entity_And_Alias_Test()
        {
            // Arrange
            var expandPaths = new List<ExpandPath> { new ExpandPath { Entity = "ItemPair" }, new ExpandPath { Entity = "OtherItemPair" } };
            var relatedEntityManager = CreateRelatedEntityOneToMany();

            var item10 = new TEntity { Id = 10, Name = "Item10" };
            var evaluator = new AttributeEvaluator();

            var itemPair5 = new ItemPair { Id = 5, Name = "Pair5", ItemId = 10, OtherItemId = 20 };
            var itemPair6 = new ItemPair { Id = 6, Name = "Pair6", ItemId = 20, OtherItemId = 10 };

            var odataItemPair5 = new[] { itemPair5 }.AsOdata<ItemPair, int>();
            var jsonItemPair5 = JsonConvert.SerializeObject(odataItemPair5);

            var odataItemPair6 = new[] { itemPair6 }.AsOdata<ItemPair, int>();
            var jsonItemPair6 = JsonConvert.SerializeObject(odataItemPair6);

            _MockClient.Setup(c => c.GetByPropertyValuesAsync(It.Is<string>(p => p == "ItemId"), It.Is<IEnumerable<string>>(ids => ids.First() == "10"),
                                                  It.IsAny<string>(), false)).ReturnsAsync(jsonItemPair5);
            _MockClient.Setup(c => c.GetByPropertyValuesAsync(It.Is<string>(p => p == "OtherItemId"), It.Is<IEnumerable<string>>(ids => ids.First() == "10"),
                                                  It.IsAny<string>(), false)).ReturnsAsync(jsonItemPair6);

            var sorter = new RelatedEntitySorterHelper<TInterface, TId>(new RelatedEntitySorterWrapper<TInterface, TId>());
            _MockRelatedEntitySorterHelper.Setup(m => m.Sort(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<RelatedEntityCollection>(), It.IsAny<SortDetails>(), It.IsAny<List<RelatedEntityCollection>>()))
                                         .Callback((IEnumerable<TInterface> ts, RelatedEntityCollection c, SortDetails sd, List<RelatedEntityCollection> list) =>
                                         {
                                             sorter.Sort(ts, c, sd, list);
                                         });

            // Act
            var actual = await relatedEntityManager.GetRelatedEntitiesAsync(new[] { item10 }, expandPaths);

            // Assert
            Assert.AreEqual(2, actual.Count);

            Assert.AreEqual("Item", actual[0].Entity);
            Assert.AreEqual("10", actual[0].EntityId);
            Assert.AreEqual("ItemPair", actual[0].RelatedEntity);
            Assert.AreEqual(1, actual[0].Count);
            Assert.AreEqual("5", actual[0][0].Id);

            Assert.AreEqual("Item", actual[1].Entity);
            Assert.AreEqual("10", actual[1].EntityId);
            Assert.AreEqual("OtherItemPair", actual[1].RelatedEntity);
            Assert.AreEqual(1, actual[1].Count);
            Assert.AreEqual("6", actual[1][0].Id);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}