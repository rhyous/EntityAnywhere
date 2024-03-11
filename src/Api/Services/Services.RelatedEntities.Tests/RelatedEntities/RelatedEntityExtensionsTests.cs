using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Expand;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Services.Common.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities.Tests
{
    [TestClass]
    public class RelatedEntityExtensionsTests
    {
        private MockRepository _MockRepository;

        private Mock<INamedFactory<IAdminEntityClientAsync>> _MockNamedFactory;
        private Mock<IExtensionEntityList> _MockExtensionEntityList;
        private Mock<IRelatedEntitySorterHelper<IUser, int>> _MockRelatedEntitySorterHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockNamedFactory = _MockRepository.Create<INamedFactory<IAdminEntityClientAsync>>();
            _MockExtensionEntityList = _MockRepository.Create<IExtensionEntityList>();
            _MockRelatedEntitySorterHelper = _MockRepository.Create<IRelatedEntitySorterHelper<IUser, int>>();
        }

        private RelatedEntityExtensions<TEntity, TInterface, TId> CreateRelatedEntityExtensions<TEntity, TInterface, TId>(IRelatedEntitySorterHelper<TInterface, TId> sorter)
                    where TEntity : class, TInterface, new()
                    where TInterface : IId<TId>
                    where TId : IComparable, IComparable<TId>, IEquatable<TId>
        {

            return new RelatedEntityExtensions<TEntity, TInterface, TId>(
                _MockNamedFactory.Object,
                _MockExtensionEntityList.Object,
                sorter);
        }

        [TestMethod]
        public async Task RelatedEntityExtensions_GetRelatedExtensionEntitiesAsync_Addenda_Test()
        {
            // Arrange
            var extensionEntities = new List<string> { "Addendum", "AlternateId" };
            var mockClient = _MockRepository.Create<IAdminEntityClientAsync>();
            mockClient.Setup(c => c.CallByCustomUrlAsync(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<bool>()))
                      .ReturnsAsync(AddendumTypeJson);
            mockClient.Setup(c => c.EntityPluralized).Returns("Addenda");
            _MockNamedFactory.Setup(m=>m.Create("Addendum")).Returns(mockClient.Object);
            var mockRelatedEntitySorterHelper = _MockRepository.Create<IRelatedEntitySorterHelper<IUser, int>>();
            var relatedEntitySorterHelper = new RelatedEntitySorterHelper<IUser, int>(new RelatedEntitySorterWrapper<IUser, int>());
            mockRelatedEntitySorterHelper.Setup(m => m.Sort(It.IsAny<IEnumerable<IUser>>(), It.IsAny<RelatedEntityCollection>(), It.IsAny<SortDetails>(), It.IsAny<List<RelatedEntityCollection>>()))
                                         .Callback((IEnumerable<IUser> users, RelatedEntityCollection c, SortDetails sd, List<RelatedEntityCollection> list) =>
                                         {
                                             relatedEntitySorterHelper.Sort(users, c, sd, list);
                                         });
            var manager = CreateRelatedEntityExtensions<User, IUser, int>(mockRelatedEntitySorterHelper.Object);
            var evaluator = new AttributeEvaluator();

            var user = new User { Id = 1, UserTypeId = 2 };
            var expandPaths = new List<ExpandPath>();

            // Act
            var actual = await manager.GetRelatedExtensionEntitiesAsync(new[] { user }, new[] { "Addendum" });

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("User", actual[0].Entity);
            Assert.AreEqual("1", actual[0].EntityId);
            Assert.AreEqual("Addendum", actual[0].RelatedEntity);
            Assert.AreEqual(AddendumTypeJsonObject1, actual[0][0].Object.ToString());
            Assert.AreEqual(AddendumTypeJsonObject2, actual[0][1].Object.ToString());
            _MockRepository.VerifyAll();
        }
        private string AddendumTypeJson = "{\"Count\":2,\"Entities\":[{\"Id\":1,\"Object\":{\"Id\":1,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"1\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop1\",\"Value\":\"val1\"},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/AddendumService.svc/Addenda(1)\"},{\"Id\":2,\"Object\":{\"Id\":2,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"1\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop2\",\"Value\":\"val2\"},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/AddendumService.svc/Addenda(2)\"}],\"Entity\":\"Addendum\",\"RelatedEntities\":[]}";
        private string AddendumTypeJsonObject1 = "{\"Id\":1,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"1\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop1\",\"Value\":\"val1\"}";
        private string AddendumTypeJsonObject2 = "{\"Id\":2,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"1\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop2\",\"Value\":\"val2\"}";


        [TestMethod]
        public void RelatedEntityExtensions_GetExtensionEntitiesToExpand_AllExcluded_Test()
        {
            // Arrange
            var addendum = nameof(Addendum);
            var alternateId = nameof(AlternateId);
            var extensionEntities = new List<string> { addendum, alternateId };
            _MockExtensionEntityList.Setup(m => m.EntityNames).Returns(extensionEntities);
            _MockExtensionEntityList.Setup(m => m.ShouldAutoExpand(addendum)).Returns(true);
            _MockExtensionEntityList.Setup(m => m.ShouldAutoExpand(alternateId)).Returns(true);

            var mockRelatedEntitySorterHelper = _MockRepository.Create<IRelatedEntitySorterHelper<IEntityInt, int>>();
            // EntityInt has this attribute: [RelatedEntityExclusions("*")]
            var manager = CreateRelatedEntityExtensions<EntityInt, IEntityInt, int>(mockRelatedEntitySorterHelper.Object);

            // Act
            var actual = manager.GetExtensionEntitiesToExpand(new List<string>());

            // Assert
            Assert.IsFalse(actual.Any());
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityExtensions_GetExtensionEntitiesToExpand_AllExpandedByDefault_Test()
        {
            // Arrange
            var addendum = nameof(Addendum);
            var alternateId = nameof(AlternateId);
            var extensionEntities = new List<string> { addendum, alternateId };
            _MockExtensionEntityList.Setup(m => m.EntityNames).Returns(extensionEntities);
            _MockExtensionEntityList.Setup(m => m.ShouldAutoExpand(addendum)).Returns(true);
            _MockExtensionEntityList.Setup(m => m.ShouldAutoExpand(alternateId)).Returns(true);

            var mockRelatedEntitySorterHelper = _MockRepository.Create<IRelatedEntitySorterHelper<IUser, int>>();
            // User has no RelatedEntityExclusions attribute
            var manager = CreateRelatedEntityExtensions<User, IUser, int>(mockRelatedEntitySorterHelper.Object);

            // Act
            var actual = manager.GetExtensionEntitiesToExpand(new List<string>());

            // Assert
            CollectionAssert.AreEqual(extensionEntities, actual.ToList());
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityExtensions_GetExtensionEntitiesToExpand_OnlySpecified_Test()
        {
            // Arrange
            var extensionEntities = new List<string> { "Addendum", "AlternateId" };
            _MockExtensionEntityList.Setup(m => m.EntityNames).Returns(extensionEntities);

            var mockRelatedEntitySorterHelper = _MockRepository.Create<IRelatedEntitySorterHelper<IUser, int>>();
            var manager = CreateRelatedEntityExtensions<User, IUser, int>(mockRelatedEntitySorterHelper.Object);

            // Act
            var actual = manager.GetExtensionEntitiesToExpand(new List<string> { "Addendum" });

            // Assert
            var list = actual.ToList();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(extensionEntities[0], list[0]);
            _MockRepository.VerifyAll();
        }
    }
}