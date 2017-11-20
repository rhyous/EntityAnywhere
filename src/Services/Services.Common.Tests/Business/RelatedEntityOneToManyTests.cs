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
    public class RelatedEntityOneToManyTests
    {
        [TestMethod]
        public void GetRelatedEntitiesTest()
        {
            // Arrange
            var manager = new RelatedEntityOneToMany<User, IUser, int>();
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

        private string UserTypeJson = "[{\"Id\":2,\"Object\":{\"Id\":2,\"Type\":\"Internal\",\"CreateDate\":\"2017 - 08 - 08T10: 18:00\",\"CreatedBy\":1,\"LastUpdated\":null,\"LastUpdatedBy\":null},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/UserTypeService.svc/UserTypes(8)\"}]";
        private string UserTypeJsonObject = "{\"Id\":2,\"Type\":\"Internal\",\"CreateDate\":\"2017 - 08 - 08T10: 18:00\",\"CreatedBy\":1,\"LastUpdated\":null,\"LastUpdatedBy\":null}";
    }
}