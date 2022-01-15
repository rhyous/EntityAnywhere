using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.EntityAnywhere.Services.Common.Tests;
using System.Collections.Generic;
using System.Threading.Tasks;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.User;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IUser;
using TId = System.Int32;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities.Tests
{
    [TestClass]
    public class RelatedEntityManyToManyTests
    {
        private MockRepository _MockRepository;

        private Mock<IRelatedEntityOneToMany<TEntity, TInterface, TId>> _MockRelatedEntityOneToMany;
        private Mock<AttributeEvaluator> _MockAttributeEvaluator;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRelatedEntityOneToMany = _MockRepository.Create<IRelatedEntityOneToMany<TEntity, TInterface, TId>>();
            _MockAttributeEvaluator = _MockRepository.Create<AttributeEvaluator>();
        }

        private RelatedEntityManyToMany<TEntity, TInterface, TId> CreateRelatedEntityManyToMany()
        {
            return new RelatedEntityManyToMany<TEntity, TInterface, TId>(
                _MockRelatedEntityOneToMany.Object,
                _MockAttributeEvaluator.Object);
        }

        [TestMethod]
        public async Task GetRelatedEntityMappingsTest()
        {
            // Arrange
            var adminUserGroupJson = "{\"Id\":1,\"Name\":\"Admin\",\"CreateDate\":\"2017-12-04T03:39:26.683\",\"CreatedBy\":2,\"Description\":\"A Group to indicate that the user is an administrator\",\"Enabled\":true,\"LastUpdated\":null,\"LastUpdatedBy\":null}";
            var json = $"{{\"Count\":1,\"Entities\":[{{\"Id\":1,\"Object\":{{\"Id\":1,\"UserId\":7247,\"UserGroupId\":1}},\"RelatedEntityCollection\":[{{\"Count\":1,\"RelatedEntity\":\"UserGroup\",\"RelatedEntities\":[{{\"Id\":\"1\",\"Object\":{adminUserGroupJson},\"Uri\":\"http://localhost:3896/UserGroupService.svc/UserGroups/Ids(1)\"}}]}}],\"Uri\":\"http://localhost:3896/UserGroupMembershipService.svc/UserGroupMemberships/UserId/Values(1)\"}}],\"Entity\":\"UserGroupMembership\"}}";
            var odataObjectCollection = JsonConvert.DeserializeObject<OdataObjectCollection>(json);

            var collections = new List<RelatedEntityCollection>();
            collections.Add(odataObjectCollection);

            _MockRelatedEntityOneToMany.Setup(f => f.GetRelatedEntitiesAsync(It.IsAny<IEnumerable<TInterface>>(), It.IsAny<IEnumerable<ExpandPath>>())).ReturnsAsync(collections);
            var userEntity = new User { Id = 7247, Name = "User7247", UserTypeId = 1};
            var reFetcher = CreateRelatedEntityManyToMany();

            // Act
            var relatedEntities = await reFetcher.GetRelatedEntitiesAsync(new[] { userEntity }, new[] { new ExpandPath { Entity = "UserGroup" } });

            // Assert
            Assert.AreEqual(1, relatedEntities.Count);
            Assert.AreEqual(1, relatedEntities[0].Count);
            Assert.AreEqual(typeof(RelatedEntity), relatedEntities[0][0].GetType());
            Assert.AreEqual(adminUserGroupJson, relatedEntities[0][0].Object.ToString());
        }
    }
}