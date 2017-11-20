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
    public class RelatedEntityExtensionsTests
    {

        [TestMethod]
        public void GetRelatedAddendaTest()
        {
            // Arrange
            var manager = new RelatedEntityExtensions<User, IUser, int>();
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
            var actual = TaskRunner.RunSynchonously(manager.GetRelatedExtensionEntitiesAsync, new[] { user } , new[] { "Addendum" });

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("User", actual[0].Entity);
            Assert.AreEqual("1", actual[0].EntityId);
            Assert.AreEqual("Addendum", actual[0].RelatedEntity);
            Assert.AreEqual(AddendumTypeJsonObject1, actual[0][0].Object.ToString());
            Assert.AreEqual(AddendumTypeJsonObject2, actual[0][1].Object.ToString());
        }
        private string AddendumTypeJson = "{\"Count\":2,\"Entities\":[{\"Id\":1,\"Object\":{\"Id\":1,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"1\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop1\",\"Value\":\"val1\"},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/AddendumService.svc/Addenda(1)\"},{\"Id\":2,\"Object\":{\"Id\":2,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"1\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop2\",\"Value\":\"val2\"},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/AddendumService.svc/Addenda(2)\"}],\"Entity\":\"Addendum\",\"RelatedEntities\":[]}";
        private string AddendumTypeJsonObject1 = "{\"Id\":1,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"1\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop1\",\"Value\":\"val1\"}";
        private string AddendumTypeJsonObject2 = "{\"Id\":2,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"1\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop2\",\"Value\":\"val2\"}";
    }
}