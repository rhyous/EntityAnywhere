using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Services.Common.Tests;
using System.Collections.Generic;
using System.Threading.Tasks;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.UserType;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IUserType;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities.Tests
{
    [TestClass]
    public class RelatedEntityOneToManyTests
    {
        private MockRepository _MockRepository;

        private Mock<INamedFactory<IEntityClientAsync>> _MockNamedFactory;
        private Mock<IEntityClientAsync> _MockUserClient;
        private Mock<IEntityClientAsync> _MockUser2Client;
        private Mock<AttributeEvaluator> _MockAttributeEvaluator;
        private Mock<IRelatedEntitySorterHelper<TInterface, TId>> _MockRelatedEntitySorterHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockNamedFactory = _MockRepository.Create<INamedFactory<IEntityClientAsync>>();
            _MockUserClient = _MockRepository.Create<IEntityClientAsync>();
            _MockUser2Client = _MockRepository.Create<IEntityClientAsync>();
            _MockAttributeEvaluator = _MockRepository.Create<AttributeEvaluator>();
            _MockRelatedEntitySorterHelper = _MockRepository.Create<IRelatedEntitySorterHelper<TInterface, TId>>();
        }

        private RelatedEntityOneToMany<TEntity, TInterface, TId> CreateRelatedEntityOneToMany()
        {
            return new RelatedEntityOneToMany<TEntity, TInterface, TId>(
                _MockNamedFactory.Object,
                _MockAttributeEvaluator.Object,
                _MockRelatedEntitySorterHelper.Object);
        }

        #region Minimum RelatedEntityOneToMany test
        [TestMethod]
        public async Task GetRelatedEntitiesTest()
        {
            // Arrange
            var user = new User { Id = 1, UserTypeId = 2 };
            var userType = new TEntity { Id = 2, Name = "Internal" };

            var userJson = JsonConvert.SerializeObject(user);
            var odataUser = user.AsOdata<User, int>();
            var odataUserJson = JsonConvert.SerializeObject(odataUser);
            var odataCollection = new[] { user }.AsOdata<User, int>();
            var odataCollectionJson = JsonConvert.SerializeObject(odataCollection);

            _MockUserClient.Setup(c => c.GetByPropertyValuesAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), false))
                                   .ReturnsAsync(odataCollectionJson);

            var manager = CreateRelatedEntityOneToMany();
            var evaluator = new AttributeEvaluator();

            var attributes = evaluator.GetAttributesToExpand<RelatedEntityForeignAttribute>(typeof(UserType), new[] { "User" });
            var expandPaths = new List<ExpandPath> { new ExpandPath { Entity = "User" } };

            var sorter = new RelatedEntitySorterHelper<TInterface, TId>(new RelatedEntitySorterWrapper<TInterface, TId>());
            _MockRelatedEntitySorterHelper.Setup(m => m.Sort(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<RelatedEntityCollection>(), It.IsAny<SortDetails>(), It.IsAny<List<RelatedEntityCollection>>()))
                                         .Callback((IEnumerable<TInterface> ts, RelatedEntityCollection c, SortDetails sd, List<RelatedEntityCollection> list) =>
                                         {
                                             sorter.Sort(ts, c, sd, list);
                                         });

            _MockNamedFactory.Setup(m => m.Create("User")).Returns(_MockUserClient.Object);

            // Act
            var actual = await manager.GetRelatedEntitiesAsync(new[] { userType }, attributes, expandPaths);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("UserType", actual[0].Entity);
            Assert.AreEqual("2", actual[0].EntityId);
            Assert.AreEqual("User", actual[0].RelatedEntity);
            Assert.AreEqual(userJson, actual[0][0].Object.ToString());
        }
        #endregion

        #region RelatedEntityOneToMany Property other than Id test
        [TestMethod]
        public async Task GetRelatedEntitiesByPropertyOtherThanIdTest()
        {
            // Arrange
            var user = new User2 { Id = 1, UserTypeName = "Internal" };
            var userType = new TEntity { Id = 2, Name = "Internal" };

            var userJson = JsonConvert.SerializeObject(user);
            var odataUser = user.AsOdata<User2, int>();
            var odataUserJson = JsonConvert.SerializeObject(odataUser);
            var odataCollection = new[] { user }.AsOdata<User2, int>();
            var odataCollectionJson = JsonConvert.SerializeObject(odataCollection);

            _MockUser2Client.Setup(c => c.GetByPropertyValuesAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), false))
                                   .ReturnsAsync(odataCollectionJson);

            var manager = CreateRelatedEntityOneToMany();
            var evaluator = new AttributeEvaluator();

            var attributes = evaluator.GetAttributesToExpand<RelatedEntityForeignAttribute>(typeof(UserType), new[] { "User2" });
            var expandPaths = new List<ExpandPath> { new ExpandPath { Entity = "User2" } };

            var sorter = new RelatedEntitySorterHelper<TInterface, TId>(new RelatedEntitySorterWrapper<TInterface, TId>());
            _MockRelatedEntitySorterHelper.Setup(m => m.Sort(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<RelatedEntityCollection>(), It.IsAny<SortDetails>(), It.IsAny<List<RelatedEntityCollection>>()))
                                         .Callback((IEnumerable<TInterface> ts, RelatedEntityCollection c, SortDetails sd, List<RelatedEntityCollection> list) =>
                                         {
                                             sorter.Sort(ts, c, sd, list);
                                         });

            _MockNamedFactory.Setup(m => m.Create("User2")).Returns(_MockUser2Client.Object);

            // Act
            var actual = await manager.GetRelatedEntitiesAsync(new[] { userType }, attributes, expandPaths);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("UserType", actual[0].Entity);
            Assert.AreEqual("2", actual[0].EntityId);
            Assert.AreEqual("User2", actual[0].RelatedEntity);
            Assert.AreEqual(userJson, actual[0][0].Object.ToString());
        }
        #endregion        
    }
}