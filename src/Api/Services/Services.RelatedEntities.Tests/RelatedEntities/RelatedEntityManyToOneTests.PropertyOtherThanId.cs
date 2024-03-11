using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Services.Common.Tests;
using System.Collections.Generic;
using System.Threading.Tasks;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.User2;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IUser2;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities.Tests
{
    [TestClass]
    public class RelatedEntityManyToOneTestsPropertyOtherThanId
    {
        private MockRepository _MockRepository;

        private Mock<INamedFactory<IEntityClientAsync>> _MockNamedFactory;
        private Mock<AttributeEvaluator> _MockAttributeEvaluator;
        private Mock<IRelatedEntitySorterHelper<TInterface, TId>> _MockRelatedEntitySorterHelper;
        private Mock<IEntityClientAsync> _MockClient = new Mock<IEntityClientAsync>();

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockNamedFactory = _MockRepository.Create<INamedFactory<IEntityClientAsync>>();
            _MockAttributeEvaluator = _MockRepository.Create<AttributeEvaluator>();
            _MockRelatedEntitySorterHelper = _MockRepository.Create<IRelatedEntitySorterHelper<TInterface, TId>>();
            _MockClient = new Mock<IEntityClientAsync>();
            _MockNamedFactory.Setup(m => m.Create("UserType")).Returns(_MockClient.Object);
        }

        private RelatedEntityManyToOne<TEntity, TInterface, TId> CreateRelatedEntityManyToOne()
        {
            return new RelatedEntityManyToOne<TEntity, TInterface, TId>(
                _MockNamedFactory.Object,
                _MockAttributeEvaluator.Object,
                _MockRelatedEntitySorterHelper.Object);
        }

        #region RelatedEntityManyToOne Property other than Id test
        [TestMethod]
        public async Task GetRelatedEntitiesByPropertyOtherThanIdTest()
        {
            // Arrange
            var user = new User2 { Id = 1, UserTypeName = "Internal" };
            var userType = new UserType { Id = 2, Name = "Internal" };
            var userTypeJson = JsonConvert.SerializeObject(userType);
            var odataUserType = userType.AsOdata<UserType, int>();
            var odataUserTypeJson = JsonConvert.SerializeObject(odataUserType);
            var odataCollection = new[] { userType }.AsOdata<UserType, int>();
            var odataCollectionJson = JsonConvert.SerializeObject(odataCollection);

            _MockClient.Setup(c => c.GetByPropertyValuesAsync("Name", It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), false)).ReturnsAsync(odataCollectionJson);

            var manager = CreateRelatedEntityManyToOne();
            var evaluator = new AttributeEvaluator();


            var attributes = evaluator.GetAttributesToExpand<RelatedEntityAttribute>(typeof(User2), new[] { "UserType" });
            var expandPaths = new List<ExpandPath> { new ExpandPath { Entity = "UserType" } };

            var sorter = new RelatedEntitySorterHelper<IUser2, int>(new RelatedEntitySorterWrapper<IUser2, int>());
            _MockRelatedEntitySorterHelper.Setup(m => m.Sort(It.IsAny<IEnumerable<IUser2>>(), It.IsAny<RelatedEntityCollection>(), It.IsAny<SortDetails>(), It.IsAny<List<RelatedEntityCollection>>()))
                                         .Callback((IEnumerable<IUser2> entities, RelatedEntityCollection c, SortDetails sd, List<RelatedEntityCollection> list) =>
                                         {
                                             sorter.Sort(entities, c, sd, list);
                                         });

            // Act
            var actual = await manager.GetRelatedEntitiesAsync(new[] { user }, attributes, expandPaths);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("User2", actual[0].Entity);
            Assert.AreEqual("1", actual[0].EntityId);
            Assert.AreEqual("UserType", actual[0].RelatedEntity);
            Assert.AreEqual(userTypeJson, actual[0][0].Object.ToString());
            _MockRepository.VerifyAll();
        }
        #endregion               
    }
}