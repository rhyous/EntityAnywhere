using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class GetMetadataHandlerTests
    {
        private MockRepository _MockRepository;
        private Mock<IMetadataServiceFactory> _MockMetadataServiceFactory;
        private Mock<IMetadataService> _MockMetadataService;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockMetadataServiceFactory = _MockRepository.Create<IMetadataServiceFactory>();
            _MockMetadataService = _MockRepository.Create<IMetadataService>();
            _MockMetadataServiceFactory.Setup(m => m.MetadataService)
                                       .Returns(_MockMetadataService.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private GetMetadataHandler CreateGetMetadataHandler()
        {
            return new GetMetadataHandler(_MockMetadataServiceFactory.Object);
        }

        [TestMethod]
        public async Task GetMetadataAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var getMetadataHandler = CreateGetMetadataHandler();
            Type type = null;
            CsdlEntity csdlEntity = new CsdlEntity();
            _MockMetadataService.Setup(m => m.GetCsdlEntity(It.IsAny<Type>())).Returns(csdlEntity);

            // Act
            var result = await getMetadataHandler.Handle(type);

            // Assert
            Assert.AreEqual(csdlEntity, result);
            _MockMetadataService.Verify(m => m.GetCsdlEntity(It.IsAny<Type>()), Times.Once);
        }
    }
}
