using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests
{
    public class TestEntityClient : EntityClientBase
    {
        public TestEntityClient(IEntityClientConnectionSettings entityClientConnectionSettings)
            : base(entityClientConnectionSettings) { }
    }

    [TestClass]
    public class EntityClientBaseTests
    {
        private MockRepository _MockRepository;
        private Mock<IEntityClientConnectionSettings> _MockEntityClientConnectionSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityClientConnectionSettings = _MockRepository.Create<IEntityClientConnectionSettings>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private EntityClientBase CreateEntityClientBase()
        {
            return new TestEntityClient(_MockEntityClientConnectionSettings.Object);
        }

        [TestMethod]
        public void EntityClientBase_Constructor_Settings_Null_Throws_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(()=> { new TestEntityClient(null); });
        }

        [TestMethod]
        public void EntityClientBase_EntityName_ComesFromSettings_Test()
        {
            // Arrange
            var entityName = "User";
            _MockEntityClientConnectionSettings.Setup(m => m.EntityName)
                                               .Returns(entityName);
            var entityClient = CreateEntityClientBase();

            // Act
            // Assert
            Assert.AreEqual(entityName, entityClient.Entity);
        }

        [TestMethod]
        public void EntityClientBase_EntityNamePluralized_ComesFromSettings_Test()
        {
            // Arrange
            var entityName = "Users";
            _MockEntityClientConnectionSettings.Setup(m => m.EntityNamePluralized)
                                               .Returns(entityName);
            var entityClient = CreateEntityClientBase();

            // Act
            // Assert
            Assert.AreEqual(entityName, entityClient.EntityPluralized);
        }

        [TestMethod]
        public void EntityClientBase_ServiceUrl_ComesFromSettings_Test()
        {
            // Arrange
            var entityName = "https://www.domain.tld/Entity1Service";
            _MockEntityClientConnectionSettings.Setup(m => m.ServiceUrl)
                                               .Returns(entityName);
            var entityClient = CreateEntityClientBase();

            // Act
            // Assert
            Assert.AreEqual(entityName, entityClient.ServiceUrl);
        }

        [TestMethod]
        public void EntityClientBase_AppendUrlParameters_Test()
        {
            // Arrange
            string url = "http://www.domain.tld/EntityService";
            string urlParams = "$top=10";
            var expected = $"{url}?{urlParams}";

            // Act
            var actual = EntityClientBase.AppendUrlParameters(urlParams, url);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
