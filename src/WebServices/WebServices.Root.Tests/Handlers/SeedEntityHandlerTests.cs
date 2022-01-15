using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices.Tests
{
    [TestClass]
    public class SeedEntityHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityCaller> _MockEntityCaller;
        private IEntityList _EntityList;
        private IRequestUri _RequestUri;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityCaller = _MockRepository.Create<IEntityCaller>();
            _EntityList = new EntityList();
            _EntityList.Entities.Add(typeof(EntityBasic));
            _EntityList.Entities.Add(typeof(EntityInt));
            _RequestUri = new RequestUri { Uri = new Uri("https://fake.domain.tld/Api/Service/$Seed") };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private SeedEntityHandler CreateSeedEntityHandler()
        {
            return new SeedEntityHandler(
                _MockEntityCaller.Object,
                _EntityList,
                _RequestUri);
        }

        [TestMethod]
        public async Task SeedEntityHandler_Handle_ZeroReturned_Test()
        {
            // Arrange
            var seedEntityHandler = CreateSeedEntityHandler();
            var list = new List<RepositorySeedResult>();
            _MockEntityCaller.Setup(m => m.CallAll<RepositorySeedResult>(It.IsAny<string>(), It.IsAny<string>()))
                             .ReturnsAsync(list);

            // Act
            var result = await seedEntityHandler.Handle();

            // Assert -- Verify all
            Assert.IsFalse(result[0].SeedSuccessful);
            Assert.IsFalse(result[1].SeedSuccessful);
        }

        [TestMethod]
        public async Task SeedEntityHandler_Handle_OneOfTwoReturned_Test()
        {
            // Arrange
            var seedEntityHandler = CreateSeedEntityHandler();
            var list = new List<RepositorySeedResult>
            {
                new RepositorySeedResult { Name = "EntityBasic", SeedSuccessful = true, EntityHasSeedData = true }
            };
            _MockEntityCaller.Setup(m => m.CallAll<RepositorySeedResult>(It.IsAny<string>(), It.IsAny<string>()))
                             .ReturnsAsync(list);

            // Act
            var result = await seedEntityHandler.Handle();

            // Assert -- Verify all
            Assert.IsTrue(result[0].SeedSuccessful);
            Assert.IsFalse(result[1].SeedSuccessful);
        }

        [TestMethod]
        public async Task SeedEntityHandler_Handle_BothReturned_Test()
        {
            // Arrange
            var seedEntityHandler = CreateSeedEntityHandler();
            var list = new List<RepositorySeedResult>
            {
                new RepositorySeedResult { Name = "EntityBasic", SeedSuccessful = true, EntityHasSeedData = true },
                new RepositorySeedResult { Name = "EntityInt", SeedSuccessful = true, EntityHasSeedData = true }
            };
            _MockEntityCaller.Setup(m => m.CallAll<RepositorySeedResult>(It.IsAny<string>(), It.IsAny<string>()))
                             .ReturnsAsync(list);

            // Act
            var result = await seedEntityHandler.Handle();

            // Assert -- Verify all
            Assert.IsTrue(result[0].SeedSuccessful);
            Assert.IsTrue(result[1].SeedSuccessful);
        }
    }
}
