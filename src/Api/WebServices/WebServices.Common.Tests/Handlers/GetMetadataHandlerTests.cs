using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Csdl;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class GetMetadataHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IMetadataCache> _MockMetadataCache;
        private Mock<IUrlParameters> _MockUrlParameters;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockMetadataCache = _MockRepository.Create<IMetadataCache>();
            _MockUrlParameters = _MockRepository.Create<IUrlParameters>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private GetMetadataHandler CreateGetMetadataHandler()
        {
            return new GetMetadataHandler(
                _MockMetadataCache.Object,
                _MockUrlParameters.Object);
        }

        [TestMethod]
        [ObjectNullOrNew(typeof(NameValueCollection))]
        public async Task GetMetadataAsync_ForwardsToHandler_NullUrlParams_Test(NameValueCollection urlParams)
        {
            // Arrange
            var getMetadataHandler = CreateGetMetadataHandler();
            Type type = null;
            CsdlEntity csdlEntity = new CsdlEntity();
            _MockMetadataCache.Setup(m => m.GetCsdlEntity(It.IsAny<Type>(), false))
                                .Returns(csdlEntity);
            _MockUrlParameters.Setup(m => m.Collection).Returns(urlParams);

            // Act
            var result = await getMetadataHandler.Handle(type);

            // Assert
            Assert.AreEqual(csdlEntity, result);
            _MockMetadataCache.Verify(m => m.GetCsdlEntity(It.IsAny<Type>(), false), Times.Once);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetMetadataAsync_ForwardsToHandler_ForceUpdateTrue_UrlParams_Test()
        {
            // Arrange
            var getMetadataHandler = CreateGetMetadataHandler();
            Type type = null;
            CsdlEntity csdlEntity = new CsdlEntity();
            _MockMetadataCache.Setup(m => m.GetCsdlEntity(It.IsAny<Type>(), true))
                                .Returns(csdlEntity);
            var urlParams = new NameValueCollection { { "ForceUpdate", true.ToString() } };
            _MockUrlParameters.Setup(m => m.Collection).Returns(urlParams);

            // Act
            var result = await getMetadataHandler.Handle(type);

            // Assert
            Assert.AreEqual(csdlEntity, result);
            _MockMetadataCache.Verify(m => m.GetCsdlEntity(It.IsAny<Type>(), true), Times.Once);
            _MockRepository.VerifyAll();
        }
    }
}
