using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.WebServices;
using System.Text;

namespace Behaviors.CustomDispatchMessageFormatter.Tests
{
    [TestClass]
    public class SerializerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var expected = "﻿{\"Id\":0,\"Addenda\":[],\"Object\":{\"Id\":0,\"CreateDate\":\"0001-01-01T00:00:00\",\"CreatedBy\":0,\"Entity\":null,\"EntityId\":null,\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"A\",\"Value\":\"B\"},\"PropertyUris\":null,\"RelatedEntities\":[{ \"Id\" : \"1\" }],\"Uri\":null}";
            var o = new JRaw("{ \"Id\" : \"1\" }");
            var odata = new OdataObject<Addendum, long>() { Object = new Addendum { Property = "A", Value = "B" } };
            odata.RelatedEntities.Add(o);

            // Act
            var actual = new Serializer().Json(odata);

            // Assert
            Assert.AreEqual(expected, Encoding.UTF8.GetString(actual));
        }
    }
}
