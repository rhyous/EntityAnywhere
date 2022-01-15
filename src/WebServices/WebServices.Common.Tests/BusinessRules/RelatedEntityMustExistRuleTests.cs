using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.BusinessRules
{
    [TestClass]
    public class RelatedEntityMustExistRuleTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityClientAsync> _MockEntityClientAsync;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityClientAsync = _MockRepository.Create<IEntityClientAsync>();
        }

        private RelatedEntityMustExistRule CreateRelatedEntityMustExistRule(IEnumerable<string> ids,
                                                                            object allowedNonExistentValue,
                                                                            bool nullable)
        {
            return new RelatedEntityMustExistRule(
                _MockEntityClientAsync.Object,
                ids,
                allowedNonExistentValue,
                nullable);
        }

        #region IsMet
        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityMustExistRule_Constructor_NullEntityClientAsync_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new RelatedEntityMustExistRule(null, new[] { "10" }, -1, false);
            });
        }

        [TestMethod]
        public void RelatedEntityMustExistRule_Constructor_NullIdsAsync_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new RelatedEntityMustExistRule(new EntityClientAsync(Mock.Of<IEntityClientConnectionSettings>(), Mock.Of<IHttpClientRunner>()), null, -1, false);
            });
        }

        [TestMethod]
        public async Task RelatedEntityMustExistRule_IsMetAsync_PassesWhenAllowedNonExistentValueIsSet_Test()
        {
            // Arrange
            var ids = new[] { "-1", "-1" };

            var relatedEntityMustExistRule = CreateRelatedEntityMustExistRule(ids, -1, false);

            // Act
            var result = await relatedEntityMustExistRule.IsMetAsync();

            // Assert
            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        public async Task RelatedEntityMustExistRule_IsMetAsync_PassesWhenAllowedNonExistentValueIsSet_NoAll_Test()
        {
            // Arrange
            var ids = new[] { "21", "22", "-1" };

            var relatedEntityMustExistRule = CreateRelatedEntityMustExistRule(ids, -1, false);

            var odataObject1 = new OdataObject<TestA, int>  { Object = new TestA { Id = 21 } };
            var odataObject2 = new OdataObject<TestA, int> { Object = new TestA { Id = 22 } };
            var odataObjectCollection = new OdataObjectCollection<TestA, int> { odataObject1, odataObject2 };
            var json = JsonConvert.SerializeObject(odataObjectCollection);
            _MockEntityClientAsync.Setup(m => m.GetByIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()))
                                  .ReturnsAsync(json);

            // Act
            var result = await relatedEntityMustExistRule.IsMetAsync();

            // Assert
            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        public async Task RelatedEntityMustExistRule_IsMetAsync_FailsWhenAllowedNonExistentValueIsNotSet_Test()
        {
            // Arrange
            var ids = new[] { "-1", "-1" };
            var existingObjs = new TestA[] { new TestA { Id = 10 }, new TestA { Id = 11 } };
            var odataObjects = existingObjs.AsOdata<TestA, int>();
            var json = JsonConvert.SerializeObject(odataObjects);

            _MockEntityClientAsync.Setup(m => m.GetByIdsAsync(It.IsAny<IEnumerable<string>>(), 
                                                              It.IsAny<bool>()))
                                 .ReturnsAsync(json);

            var relatedEntityMustExistRule = CreateRelatedEntityMustExistRule(ids, null, false);

            // Act
            var result = await relatedEntityMustExistRule.IsMetAsync();

            // Assert
            Assert.IsFalse(result.Result);
        }

        [TestMethod]
        public async Task RelatedEntityMustExistRule_IsMetAsync_SucceedsWhenAllAreAllowedNonExistentValues_Test()
        {
            // Arrange
            var ids = new[] { "-1", "-1" };
            var existingObjs = new TestA[] { new TestA { Id = 10 }, new TestA { Id = 11 } };
            var odataObjects = existingObjs.AsOdata<TestA, int>();
            var json = JsonConvert.SerializeObject(odataObjects);

            var relatedEntityMustExistRule = CreateRelatedEntityMustExistRule(ids, -1, false);

            // Act
            var result = await relatedEntityMustExistRule.IsMetAsync();

            // Assert
            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        public async Task RelatedEntityMustExistRule_IsMetAsync_PassesWhenAllowedIdsExist_Test()
        {
            // Arrange
            var ids = new[] { "10", "11" };
            var existingObjs = new TestA[] { new TestA { Id = 10 }, new TestA { Id = 11 } };
            var odataObjects = existingObjs.AsOdata<TestA, int>();
            var json = JsonConvert.SerializeObject(odataObjects);

            _MockEntityClientAsync.Setup(m => m.GetByIdsAsync(It.IsAny<IEnumerable<string>>(),
                                                              It.IsAny<bool>()))
                                  .ReturnsAsync(json);

            var relatedEntityMustExistRule = CreateRelatedEntityMustExistRule(ids, null, false);

            // Act
            var result = await relatedEntityMustExistRule.IsMetAsync();

            // Assert
            Assert.IsTrue(result.Result);
        }


        [TestMethod]
        public async Task RelatedEntityMustExistRule_IsMetAsync_PassesWhen_AllowedIdsExist_And_AllowedNonExistentValue_DoesNotExist_Test()
        {
            // Arrange
            var ids = new[] { "10", "11" };
            var existingObjs = new TestA[] { new TestA { Id = 10 }, new TestA { Id = 11 } };
            var odataObjects = existingObjs.AsOdata<TestA, int>();
            var json = JsonConvert.SerializeObject(odataObjects);

            _MockEntityClientAsync.Setup(m => m.GetByIdsAsync(It.IsAny<IEnumerable<string>>(),
                                                              It.IsAny<bool>()))
                                  .ReturnsAsync(json);

            var relatedEntityMustExistRule = CreateRelatedEntityMustExistRule(ids, -1, false);

            // Act
            var result = await relatedEntityMustExistRule.IsMetAsync();

            // Assert
            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        public async Task RelatedEntityMustExistRule_IsMetAsync_AllIdsNull_PassesWhenAllNullable_Test()
        {
            // Arrange
            var ids = new string[] { null, null };

            var relatedEntityMustExistRule = CreateRelatedEntityMustExistRule(ids, -1, true);

            // Act
            var result = await relatedEntityMustExistRule.IsMetAsync();

            // Assert
            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        public async Task RelatedEntityMustExistRule_IsMetAsync_PassesWhenSomeNullable_Test()
        {
            // Arrange
            var ids = new[] { "21", "22", null };

            var odataObject1 = new OdataObject<TestA, int> { Object = new TestA { Id = 21 } };
            var odataObject2 = new OdataObject<TestA, int> { Object = new TestA { Id = 22 } };
            var odataObjectCollection = new OdataObjectCollection<TestA, int> { odataObject1, odataObject2 };
            var json = JsonConvert.SerializeObject(odataObjectCollection);
            _MockEntityClientAsync.Setup(m => m.GetByIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()))
                                  .ReturnsAsync(json);

            var relatedEntityMustExistRule = new RelatedEntityMustExistRule(_MockEntityClientAsync.Object, ids, -1, true);

            // Act
            var result = await relatedEntityMustExistRule.IsMetAsync();

            // Assert
            Assert.IsTrue(result.Result);
        }
        #endregion
    }
}