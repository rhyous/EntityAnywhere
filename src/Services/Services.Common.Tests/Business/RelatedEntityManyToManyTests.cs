using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Expand;
using Rhyous.WebFramework.Clients;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Services.Common.Tests.Business
{
    [TestClass]
    public class RelatedEntityManyToManyTests
    {
        [TestMethod]
        public void GetRelatedEntityMappingsTest()
        {
            //// Arrange
            //var manager = new RelatedEntityManyToMany<User, IUser, int>();
            //var evaluator = new AttributeEvaluator();
            //var attributes = evaluator.GetForeignAttributesToExpand(typeof(User), new[] { "UserGroup" });
            //var a = attributes.First();
            //var key = $"{a.RelatedEntity}:{a.Entity}";
            //var mockClient = new Mock<IMappingEntityClientAsync>();
            //mockClient.Setup(c => c.GetByE2IdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<string>())).ReturnsAsync(UserGroupMembershipJson);
            //manager.MappingClientsCache.Add(key, mockClient.Object);
            //var user = new User { Id = 1, UserTypeId = 2 };
            //var expandPaths = new List<ExpandPath>();

            //// Act
            //var actual = TaskRunner.RunSynchonously(manager.GetRelatedMappingEntitiesAsync, new[] { user }, attributes, expandPaths);

            //// Assert
            //Assert.AreEqual(1, actual.Count);
            //Assert.AreEqual(1, actual.Count);
            //Assert.AreEqual("User", actual[0].Entity);
            //Assert.AreEqual("1", actual[0].EntityId);
            //Assert.AreEqual("Group", actual[0].RelatedEntity);

            //Assert.AreEqual(UserGroupMembershipJsonObject, actual[0][0].Object.ToString());
            //Assert.AreEqual(UserGroupMembershipJsonRelatedEntities, actual[0][0].RelatedEntityCollection[0][0].Object.ToString());
        }

        private string UserGroupMembershipJson = "[{\"Id\":3,\"Object\":{\"Id\":3,\"UserGroupId\":4,\"UserId\":5},\"PropertyUris\":[],\"RelatedEntities\":[{\"RelatedEntity\":\"UserGroup\",\"Entities\":[{\"Id\":\"4\",\"Object\":{\"Id\":4,\"Name\":\"Example Users\",\"CreateDate\":\"2017-10-24T09:09:59.76\",\"CreatedBy\":1,\"Description\":null,\"LastUpdated\":null,\"LastUpdatedBy\":null},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/UserGroupService.svc/UserGroups/Ids\"}]}],\"Uri\":\"http://localhost:3896/UserGroupMembershipService.svc/UserGroupMemberships(3)\"}]";
        private string UserGroupMembershipJsonObject = "{\"Id\":3,\"UserGroupId\":4,\"UserId\":5}";
        private string UserGroupMembershipJsonRelatedEntities = "{\"Id\":4,\"Name\":\"Example Users\",\"CreateDate\":\"2017-10-24T09:09:59.76\",\"CreatedBy\":1,\"Description\":null,\"LastUpdated\":null,\"LastUpdatedBy\":null}";
    }
}