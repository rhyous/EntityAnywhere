using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices.Tests
{
    [TestClass]
    public class RootWebServiceTests
    {
        public class TestEntity1 : AuditableEntity<int> { public string Name { get; set; } }
        public class TestEntity2 : AuditableEntity<int> { public string Name { get; set; } }

        private MockRepository _MockRepository;

        private Mock<IRootHandlerProvider> _MockRootHandlerProvider;
        private IEntityList _EntityList;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRootHandlerProvider = _MockRepository.Create<IRootHandlerProvider>();
            _EntityList = new EntityList();
            _EntityList.Entities.Add(typeof(EntityBasic));
            _EntityList.Entities.Add(typeof(EntityInt));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private RootWebService CreateRootService(IEntityList entityList = null)
        {
            entityList = entityList ?? _EntityList;
            return new RootWebService(_MockRootHandlerProvider.Object, entityList);
        }

        [TestMethod]
        public void RootService_GetMetadataAsync_Test()
        {
            // Arrange
            var type1 = typeof(TestEntity1);
            var type2 = typeof(TestEntity2);
            var entityList = new EntityList();
            entityList.Entities.Add(type1);
            entityList.Entities.Add(type2);
            var rootService = CreateRootService(entityList);
            var doc = new CsdlDocument();
            var expected = "{\"$Version\":null,\"$EntityContainer\":null}";
            var mockGetMetadataHandler = _MockRepository.Create<IGetMetadataHandler>();
            mockGetMetadataHandler.Setup(m => m.Handle(It.IsAny<IEnumerable<Type>>()))
                                  .ReturnsAsync(doc);
            _MockRootHandlerProvider.Setup(m => m.GetMetadataHandler).Returns(mockGetMetadataHandler.Object);

            // Act
            var result = rootService.GetMetadataAsync().Result;

            // Assert
            Assert.AreEqual(expected, JsonConvert.SerializeObject(result));
        }        

        [TestMethod]
        public async Task TestGenerate()
        {
            // Arrange
            var type1 = typeof(TestEntity1);
            var type2 = typeof(TestEntity2);
            var entityList = new EntityList();
            entityList.Entities.Add(type1);
            entityList.Entities.Add(type2);
            var rootService = CreateRootService(entityList);

            var result1 = new RepositoryGenerationResult { Name = type1.Name, RepositoryReady = true };
            var result2 = new RepositoryGenerationResult { Name = type2.Name, RepositoryReady = true };
            var list = new List<RepositoryGenerationResult> { result1, result2 };
            var mockGenerateHandler = _MockRepository.Create<IGenerateHandler>();
            mockGenerateHandler.Setup(m => m.Handle()).ReturnsAsync(list);
            _MockRootHandlerProvider.Setup(m => m.GenerateHandler)
                                    .Returns(mockGenerateHandler.Object);

            // Act
            var actual = await rootService.GenerateAsync();

            // Assert
            Assert.AreEqual(result1, actual[0]);
            Assert.AreEqual(result2, actual[1]);
        }

        [TestMethod]
        public async Task TestSeed()
        {
            // Arrange
            var type1 = typeof(TestEntity1);
            var type2 = typeof(TestEntity2);
            var entityList = new EntityList();
            entityList.Entities.Add(type1);
            entityList.Entities.Add(type2);
            var rootService = CreateRootService(entityList);
            var result1 = new RepositorySeedResult { Name = type1.Name, SeedSuccessful = true, EntityHasSeedData = true };
            var result2 = new RepositorySeedResult { Name = type2.Name, SeedSuccessful = true, EntityHasSeedData = true };
            var list = new List<RepositorySeedResult> { result1, result2 };

            var mockSeedEntityHandler = _MockRepository.Create<ISeedEntityHandler>();
            mockSeedEntityHandler.Setup(m => m.Handle()).ReturnsAsync(list);
            _MockRootHandlerProvider.Setup(m => m.SeedEntityHandler)
                                    .Returns(mockSeedEntityHandler.Object);

            // Act
            var actual = await rootService.InsertSeedDataAsync();

            // Assert
            Assert.AreEqual(result1, actual[0]);
            Assert.AreEqual(result2, actual[1]);
        }
    }
}