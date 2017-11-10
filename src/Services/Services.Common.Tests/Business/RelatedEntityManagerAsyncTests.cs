using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Expand;
using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services.Common.Tests.Business
{
    [TestClass]
    public class RelatedEntityManagerAsyncTests
    {

        [TestMethod]
        public void GetRelatedAddendaTest()
        {
            // Arrange
            var manager = new RelatedEntityManager<User, IUser, int>();
            var evaluator = new AttributeEvaluator();
            var mockClient = new Mock<IEntityClientAsync>();
            mockClient.Setup(c => c.GetByCustomUrlAsync(It.IsAny<string>(), It.IsAny<Func<string, HttpContent, Task<HttpResponseMessage>>>(), It.IsAny<object>()))
                      .ReturnsAsync(AddendumTypeJson);
            mockClient.Setup(c => c.EntityPluralized).Returns("Addenda");
            mockClient.Setup(c => c.HttpClient).Returns(new HttpClient());
            manager.ClientsCache.Json.Add("Addendum", mockClient.Object);
            var user = new User { Id = 1, UserTypeId = 2 };
            var expandPaths = new List<ExpandPath>();

            // Act
            var actual = TaskRunner.RunSynchonously(manager.GetRelatedExtensionEntitiesAsync, new[] { user }, new[] { "Addendum" });

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("User", actual[0].Entity);
            Assert.AreEqual("1", actual[0].EntityId);
            Assert.AreEqual("Addendum", actual[0].RelatedEntity);
            Assert.AreEqual(AddendumTypeJsonObject1, actual[0][0].Object.ToString());
            Assert.AreEqual(AddendumTypeJsonObject2, actual[0][1].Object.ToString());
        }

        [TestMethod]
        public void GetRelatedEntitiesTest()
        {
            // Arrange
            var manager = new RelatedEntityManager<User, IUser, int>();
            var evaluator = new AttributeEvaluator();
            var mockClient = new Mock<IEntityClientAsync>();
            mockClient.Setup(c => c.GetByIdsAsync(It.IsAny<IEnumerable<string>>(),It.IsAny<string>())).ReturnsAsync(UserTypeJson);
            manager.ClientsCache.Json.Add("UserType", mockClient.Object);
            var user = new User { Id = 1, UserTypeId = 2 };
            var attributes = evaluator.GetAttributesToExpand(typeof(User),new[] { "UserType" });
            var expandPaths = new List<ExpandPath>();
            
            // Act
            var actual = TaskRunner.RunSynchonously(manager.GetRelatedEntitiesAsync, new[] { user }, attributes, expandPaths);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("User", actual[0].Entity);
            Assert.AreEqual("1", actual[0].EntityId);
            Assert.AreEqual("UserType", actual[0].RelatedEntity);
            Assert.AreEqual(UserTypeJsonObject, actual[0][0].Object.ToString());
        }

        [TestMethod]
        public void GetRelatedEntityMappingsTest()
        {
            // Arrange
            var manager = new RelatedEntityManager<User, IUser, int>();
            var evaluator = new AttributeEvaluator();
            var attributes = evaluator.GetMappingAttributesToExpand(typeof(User), new[] { "UserGroup" });
            var a = attributes.First();
            var key = $"{a.MappingEntity}:{a.RelatedEntity}:{a.Entity}";
            var mockClient = new Mock<IMappingEntityClientAsync>();
            mockClient.Setup(c => c.GetByE2IdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<string>())).ReturnsAsync(UserGroupMembershipJson);
            RelatedEntityManager<User, IUser, int>.MappingClientsCache.Add(key, mockClient.Object);
            var user = new User { Id = 1, UserTypeId = 2 };
            var expandPaths = new List<ExpandPath>();

            // Act
            var actual = TaskRunner.RunSynchonously(manager.GetRelatedMappingEntitiesAsync, new[] { user }, attributes, expandPaths);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("User", actual[0].Entity);
            Assert.AreEqual("1", actual[0].EntityId);
            Assert.AreEqual("Group", actual[0].RelatedEntity);

            Assert.AreEqual(UserGroupMembershipJsonObject, actual[0][0].Object.ToString());
            Assert.AreEqual(UserGroupMembershipJsonRelatedEntities, actual[0][0].RelatedEntities[0][0].Object.ToString());
        }

        private string AddendumTypeJson = "{\"Count\":2,\"Entities\":[{\"Id\":1,\"Object\":{\"Id\":1,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"1\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop1\",\"Value\":\"val1\"},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/AddendumService.svc/Addenda(1)\"},{\"Id\":2,\"Object\":{\"Id\":2,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"1\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop2\",\"Value\":\"val2\"},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/AddendumService.svc/Addenda(2)\"}],\"Entity\":\"Addendum\",\"RelatedEntities\":[]}";
        private string AddendumTypeJsonObject1 = "{\"Id\":1,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"1\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop1\",\"Value\":\"val1\"}";
        private string AddendumTypeJsonObject2 = "{\"Id\":2,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"1\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop2\",\"Value\":\"val2\"}";
        private string UserTypeJson = "[{\"Id\":2,\"Object\":{\"Id\":2,\"Type\":\"Internal\",\"CreateDate\":\"2017 - 08 - 08T10: 18:00\",\"CreatedBy\":1,\"LastUpdated\":null,\"LastUpdatedBy\":null},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/UserTypeService.svc/UserTypes(8)\"}]";
        private string UserTypeJsonObject = "{\"Id\":2,\"Type\":\"Internal\",\"CreateDate\":\"2017 - 08 - 08T10: 18:00\",\"CreatedBy\":1,\"LastUpdated\":null,\"LastUpdatedBy\":null}";
        private string UserGroupMembershipJson = "[{\"Id\":3,\"Object\":{\"Id\":3,\"UserGroupId\":4,\"UserId\":5},\"PropertyUris\":[],\"RelatedEntities\":[{\"RelatedEntity\":\"UserGroup\",\"Entities\":[{\"Id\":\"4\",\"Object\":{\"Id\":4,\"Name\":\"Example Users\",\"CreateDate\":\"2017-10-24T09:09:59.76\",\"CreatedBy\":1,\"Description\":null,\"LastUpdated\":null,\"LastUpdatedBy\":null},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/UserGroupService.svc/UserGroups/Ids\"}]}],\"Uri\":\"http://localhost:3896/UserGroupMembershipService.svc/UserGroupMemberships(3)\"}]";
        private string UserGroupMembershipJsonObject = "{\"Id\":3,\"UserGroupId\":4,\"UserId\":5}";
        private string UserGroupMembershipJsonRelatedEntities = "{\"Id\":4,\"Name\":\"Example Users\",\"CreateDate\":\"2017-10-24T09:09:59.76\",\"CreatedBy\":1,\"Description\":null,\"LastUpdated\":null,\"LastUpdatedBy\":null}";
    }
}