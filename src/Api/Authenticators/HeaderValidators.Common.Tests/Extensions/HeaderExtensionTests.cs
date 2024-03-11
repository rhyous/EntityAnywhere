using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Exceptions;
using System;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.HeaderValidators.Tests
{
    [TestClass]
    public class HeaderExtensionTests
    {
        [TestMethod]
        public void HeaderExtensions_UpdateValue_Headers_Null_Test()
        {
            // Arrange
            NameValueCollection headers = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                headers.UpdateValue("key1", "value1");
            });
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void HeaderExtensions_UpdateValue_Key_NullEmptyOrWhitespace_Test(string key)
        {
            // Arrange
            var headers = new NameValueCollection();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                headers.UpdateValue(key, "value1");
            });
        }

        [TestMethod]
        public void HeaderExtensions_UpdateValue_HeaderNotSet_Test()
        {
            // Arrange
            var headers = new NameValueCollection();

            // Act
            headers.UpdateValue("key1", "value1");

            // Assert
            Assert.AreEqual(1, headers.Count);
            Assert.AreEqual("value1", headers.Get("key1"));
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void HeaderExtensions_UpdateValue_UpdateValueNullEmptyOrWhitespace_Test(string value)
        {
            // Arrange
            var headers = new NameValueCollection();

            // Act
            headers.UpdateValue("key1", value);

            // Assert
            Assert.AreEqual(0, headers.Count);
        }

        [TestMethod]
        public void HeaderExtensions_UpdateValue_HeaderValue_EmptyString_Test()
        {
            // Arrange
            var headers = new NameValueCollection();
            headers.Add("key1", "");

            // Act
            headers.UpdateValue("key1", "value1");

            // Assert
            Assert.AreEqual(1, headers.Count);
            Assert.AreEqual("value1", headers.Get("key1"));
        }

        [TestMethod]
        public void HeaderExtensions_UpdateValue_Same_Test()
        {
            // Arrange
            var headers = new NameValueCollection();
            headers.Add("key1", "value1");

            // Act
            headers.UpdateValue("key1", "value1");

            // Assert
            Assert.AreEqual(1, headers.Count);
            Assert.AreEqual("value1", headers.Get("key1"));

        }

        [TestMethod]
        public void HeaderExtensions_UpdateValue_Different_Test()
        {
            // Arrange
            var headers = new NameValueCollection();
            headers.Add("key1", "value1");

            // Act & assert
            Assert.ThrowsException<RestException>(()=> { headers.UpdateValue("key1", "newvalue1"); });
        }

        /// <summary>
        /// This test was written to verify that calling Remove on a NameValueCollection
        /// won't crash if the key is not present.
        /// </summary>
        [TestMethod]
        public void HeaderExtensions_CanHeaderBeRemovedIfItDoesntExist()
        {
            // Arrange
            var headers = new NameValueCollection();
            headers.Add("key1", "value1");

            // Act
            headers.Remove("key2");

            // Assert
            Assert.AreEqual(1, headers.Count);
        }
    }
}
