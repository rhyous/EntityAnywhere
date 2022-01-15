using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices.Tests
{
    [TestClass]
    public class GenerateHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityCaller> _MockEntityCaller;
        private Mock<IEntityList> _MockEntityList;
        private IRequestUri _Uri;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityCaller = _MockRepository.Create<IEntityCaller>();
            _MockEntityList = _MockRepository.Create<IEntityList>();
            var entities = new List<Type> { typeof(EntityBasic), typeof(EntityInt) };
            _MockEntityList.Setup(m => m.Entities).Returns(entities);
            _Uri = new RequestUri { Uri = new Uri("https://fake.domain.tld/Api/Service/$Generate") };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private GenerateHandler CreateGenerateHandler()
        {
            return new GenerateHandler(_MockEntityCaller.Object, _MockEntityList.Object, _Uri);
        }

        [TestMethod]
        public async Task GenerateHandler_Handle_Test()
        {
            // Arrange
            var generateHandler = CreateGenerateHandler();
            IEnumerable<RepositoryGenerationResult> results = null;
            _MockEntityCaller.Setup(m => m.CallAll<RepositoryGenerationResult>(It.IsAny<string>(), It.IsAny<string>()))
                             .ReturnsAsync(results);

            // Act
            var result = await generateHandler.Handle();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsFalse(result[0].RepositoryReady);
            Assert.IsFalse(result[1].RepositoryReady);
            _MockEntityCaller.Verify(m => m.CallAll<RepositoryGenerationResult>(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
