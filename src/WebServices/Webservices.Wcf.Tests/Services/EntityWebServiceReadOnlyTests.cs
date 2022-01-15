using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Csdl;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Services
{
    [TestClass]
    public class EntityWebServiceReadOnlyTests
    {
        public TestContext TestContext { get; set; }

        private EntityWebServiceReadOnly<TEntity, TInterface, TId> GetWebService<TEntity, TInterface, TId>(
            Mock<IRestHandlerProviderReadOnly<TEntity, TInterface, TId>> mockRestHandlerProvider = null)
            where TEntity : class, TInterface, new()
            where TInterface : IBaseEntity<TId>
            where TId : IComparable, IComparable<TId>, IEquatable<TId>
        {
            mockRestHandlerProvider = mockRestHandlerProvider 
                                    ?? new Mock<IRestHandlerProviderReadOnly<TEntity, TInterface, TId>>();
            return new EntityWebServiceReadOnly<TEntity, TInterface, TId>(mockRestHandlerProvider.Object);
        }

        [TestMethod] 
        public async Task EntityWebServiceReadOnly_Metadata_Tests()
        {
            // Arrange
            var csdl = new CsdlEntity();
            var mockGetMetadataHandler = new Mock<IGetMetadataHandler>();
            mockGetMetadataHandler.Setup(m => m.Handle(It.IsAny<Type>())).ReturnsAsync(csdl);

            var mockRestHandlerProvider = new Mock<IRestHandlerProviderReadOnly<EntityInt, IEntityInt, int>>();
            mockRestHandlerProvider.Setup(m => m.GetMetadataHandler).Returns(mockGetMetadataHandler.Object);

            var webservice = GetWebService<EntityInt, IEntityInt, int>(mockRestHandlerProvider);
            
            // Act
            var metadata = await webservice.GetMetadataAsync();

            // Assert
            Assert.AreEqual(csdl, metadata);
            mockGetMetadataHandler.Verify(m => m.Handle(It.IsAny<Type>()), Times.Once);
        }
    }
}
