using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Rhyous.Odata;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Entities;
using System.Text;

namespace Behaviors.CustomDispatchMessageFormatter.Tests
{
    [TestClass]
    public class SerializerTests
    {
        [TestMethod]
        public void SerializeOdataOjectAsJsonTest()
        {
            // Arrange
            var expected = "﻿{\"Id\":10,\"Object\":{\"Id\":10,\"CreateDate\":\"0001-01-01T00:00:00\",\"CreatedBy\":0,\"Entity\":null,\"EntityId\":null,\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"A\",\"Value\":\"B\"},\"PropertyUris\":null,\"RelatedEntities\":[{\"Count\":2,\"RelatedEntity\":\"Fake\",\"Entities\":[{\"Id\":\"1\",\"Object\":{ \"Id\" : \"1\" },\"PropertyUris\":null,\"RelatedEntities\":[],\"Uri\":null},{\"Id\":\"2\",\"Object\":{ \"Id\" : \"2\" },\"PropertyUris\":null,\"RelatedEntities\":[],\"Uri\":null}]}],\"Uri\":null}";
            var relatedEntityCollection = new RelatedEntityCollection { Entity = "Addendum", EntityId = "10", RelatedEntity = "Fake" };
            var json1 = "{ \"Id\" : \"1\" }";
            var jObject1 = JObject.Parse(json1);
            var json2 = "{ \"Id\" : \"2\" }";
            var jObject2 = JObject.Parse(json1);
            var relatedEntity1 = new RelatedEntity { Id = jObject1.GetValue("Id").ToString(), Object = new JRaw(json1) };
            var relatedEntity2 = new RelatedEntity { Id = jObject2.GetValue("Id").ToString(), Object = new JRaw(json2) };
            relatedEntityCollection.Add(relatedEntity1);
            relatedEntityCollection.Add(relatedEntity2);
            var odata = new OdataObject<Addendum, long>() { Object = new Addendum { Id = 10, Property = "A", Value = "B" } };
            odata.RelatedEntities.Add(relatedEntityCollection);

            // Act
            var actual = new Serializer().Json(odata);

            // Assert
            Assert.AreEqual(expected, Encoding.UTF8.GetString(actual));
        }
    }
}