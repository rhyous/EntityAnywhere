using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.WebFramework.Services;
using System.Diagnostics;
using Rhyous.WebFramework.Interfaces;

namespace Services.Common.Tests
{
    #region Some classes to help with generic testing
    public class Test1Attribute : Attribute
    {
        public string TestProperty { get; set; }
    }

    [Test1(TestProperty = "Test Value")]
    public class Test1Class { }

    [AlternateKey(KeyProperty = "Username")]
    public class User : IEntity<int>
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
    #endregion

    [TestClass]
    public class TypeExtensionsTests
    {
        [TestMethod]
        public void GetAlternateIdPropertyValueTests()
        {
            // Arrange - Done above
            // Act
            var value = typeof(User).GetAlternateKeyProperty();

            // Assert
            Assert.AreEqual("Username", value);
        }

        [TestMethod]
        public void GetAlternateIdPropertyValuePerformanceTests()
        {
            // Arrange - Done above
            // Act

            string value = string.Empty;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 100000; i++)
            {
                value = typeof(User).GetAlternateKeyProperty();
            }
            watch.Stop();
            Assert.IsTrue(watch.ElapsedMilliseconds < 100);

            // Assert
            Assert.AreEqual("Username", value);
        }
    }
}
