using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Expand;
using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Services.Common.Tests.Business
{
    [TestClass]
    public class RelatedEntityManagerAsyncTests
    {
        #region GetAttributesToExpand
        [TestMethod]
        public void GetAttributesToExpandNullNoAutoexpandTest()
        {
            // Arrange
            List<string> entitiesToExpand = null;
            var manager = new RelatedEntityManager<User, IUser, int>();

            // Act
            var actual = manager.GetAttributesToExpand(entitiesToExpand).ToList();

            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GetAttributesToExpandNullAutoexpandTrueTest()
        {
            // Arrange
            List<string> entitiesToExpand = null;
            var manager = new RelatedEntityManager<Token, IToken, int>();
            var expected = new RelatedEntityAttribute("User") { Property = "UserId", AutoExpand = true };

            // Act
            var actual = manager.GetAttributesToExpand(entitiesToExpand).ToList();

            // Assert
            Assert.AreEqual(1, actual.Count);
            foreach (var prop in typeof(RelatedEntityAttribute).GetProperties())
            {
                Assert.AreEqual(prop.GetValue(expected), prop.GetValue(actual[0]));
            }
        }

        [TestMethod]
        public void GetAttributesToExpandUserTypeNoAutoexpandTest()
        {
            // Arrange
            List<string> entitiesToExpand = new List<string> { "UserType"};
            var manager = new RelatedEntityManager<User, IUser, int>();
            var expected = new RelatedEntityAttribute("UserType") { Property = "UserTypeId"};

            // Act
            var actual = manager.GetAttributesToExpand(entitiesToExpand).ToList();

            // Assert
            Assert.AreEqual(1, actual.Count);
            foreach (var prop in typeof(RelatedEntityAttribute).GetProperties())
            {
                Assert.AreEqual(prop.GetValue(expected), prop.GetValue(actual[0]));
            }
        }
        #endregion

        #region GetMappingAttributesToExpand
        [TestMethod]
        public void GetMappingAttributesToExpandNullNoAutoexpandTest()
        {
            // Arrange
            List<string> entitiesToExpand = null;
            var manager = new RelatedEntityManager<UserRole, IUserRole, int>();

            // Act
            var actual = manager.GetMappingAttributesToExpand(entitiesToExpand).ToList();

            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GetMappingAttributesToExpandNullAutoExpandTrueTest()
        {
            // Arrange
            List<string> entitiesToExpand = null;
            var manager = new RelatedEntityManager<User, IUser, int>();
            var expected = new RelatedEntityMappingAttribute("UserGroup", "UserGroupMembership", "User") { AutoExpand = true };

            // Act
            var actual = manager.GetMappingAttributesToExpand(entitiesToExpand).ToList();

            // Assert
            Assert.AreEqual(1, actual.Count);
            foreach (var prop in typeof(RelatedEntityMappingAttribute).GetProperties())
            {
                Assert.AreEqual(prop.GetValue(expected), prop.GetValue(actual[0]));
            }
        }

        [TestMethod]
        public void GetMappingAttributesToExpandUserTypeNoAutoexpandTest()
        {
            // Arrange
            List<string> entitiesToExpand = new List<string> { "UserGroup" };
            var manager = new RelatedEntityManager<User, IUser, int>();
            var expected = new RelatedEntityMappingAttribute("UserGroup", "UserGroupMembership", "User") { AutoExpand = true };

            // Act
            var actual = manager.GetMappingAttributesToExpand(entitiesToExpand).ToList();

            // Assert
            Assert.AreEqual(1, actual.Count);
            foreach (var prop in typeof(RelatedEntityMappingAttribute).GetProperties())
            {
                Assert.AreEqual(prop.GetValue(expected), prop.GetValue(actual[0]));
            }
        }
        #endregion

        [TestMethod]
        public void GetRelatedEntitiesTest()
        {
            // Arrange
            var manager = new RelatedEntityManager<User, IUser, int>();
            var mockClient = new Mock<IEntityClientAsync>();
            mockClient.Setup(c => c.GetByIdsAsync(It.IsAny<IEnumerable<string>>(),It.IsAny<string>())).ReturnsAsync(UserTypeJson);
            RelatedEntityManager<User, IUser, int>.ClientsCache.Add("UserType", mockClient.Object);
            var user = new User { Id = 1, UserTypeId = 2 };
            var attributes = manager.GetAttributesToExpand(new[] { "UserType" });
            var expandPaths = new List<ExpandPath>();
            
            // Act
            var actual = TaskRunner.RunSynchonously(manager.GetRelatedEntitiesAsync, new[] { user }, attributes, expandPaths);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("User", actual[0].Entity);
            Assert.AreEqual("1", actual[0].EntityId);
            Assert.AreEqual("UserType", actual[0].RelatedEntity);
            Assert.AreEqual(UserTypeJsonObject, actual[0].Entities[0].Object.ToString());
        }

        [TestMethod]
        public void GetRelatedEntityMappingsTest()
        {
            // Arrange
            var manager = new RelatedEntityManager<User, IUser, int>();
            var attributes = manager.GetMappingAttributesToExpand(new[] { "UserGroup" });
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

            Assert.AreEqual(UserGroupMembershipJsonObject, actual[0].Entities[0].Object.ToString());
            Assert.AreEqual(UserGroupMembershipJsonRelatedEntities, actual[0].Entities[0].RelatedEntities[0].Entities[0].Object.ToString());
        }

        private string UserTypeJson = "[{\"Id\":2,\"Addenda\":[],\"Object\":{\"Id\":2,\"Type\":\"Internal\",\"CreateDate\":\"2017 - 08 - 08T10: 18:00\",\"CreatedBy\":1,\"LastUpdated\":null,\"LastUpdatedBy\":null},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/UserTypeService.svc/UserTypes(8)\"}]";
        private string UserTypeJsonObject = "{\"Id\":2,\"Type\":\"Internal\",\"CreateDate\":\"2017 - 08 - 08T10: 18:00\",\"CreatedBy\":1,\"LastUpdated\":null,\"LastUpdatedBy\":null}";
        private string UserGroupMembershipJson = "[{\"Id\":3,\"Addenda\":[],\"Object\":{\"Id\":3,\"UserGroupId\":4,\"UserId\":5},\"PropertyUris\":[],\"RelatedEntities\":[{\"RelatedEntity\":\"UserGroup\",\"Entities\":[{\"Id\":\"4\",\"Object\":{\"Id\":4,\"Name\":\"Example Users\",\"CreateDate\":\"2017-10-24T09:09:59.76\",\"CreatedBy\":1,\"Description\":null,\"LastUpdated\":null,\"LastUpdatedBy\":null},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/UserGroupService.svc/UserGroups/Ids\"}]}],\"Uri\":\"http://localhost:3896/UserGroupMembershipService.svc/UserGroupMemberships(3)\"}]";
        private string UserGroupMembershipJsonObject = "{\"Id\":3,\"UserGroupId\":4,\"UserId\":5}";
        private string UserGroupMembershipJsonRelatedEntities = "{\"Id\":4,\"Name\":\"Example Users\",\"CreateDate\":\"2017-10-24T09:09:59.76\",\"CreatedBy\":1,\"Description\":null,\"LastUpdated\":null,\"LastUpdatedBy\":null}";
    }
}