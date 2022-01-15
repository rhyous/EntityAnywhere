using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Services
{
    [TestClass]
    public class EntityWebServiceAlternateKeyTests
    {
        private EntityWebServiceAlternateKey<TEntity, TInterface, TId, TAltKey> GetWebService<TEntity, TInterface, TId, TAltKey>(
                        Mock<IRestHandlerProviderAlternateKey<TEntity, TInterface, TId, TAltKey>> mockRestHandlerProvider = null)
            where TEntity : class, TInterface, new()
            where TInterface : IBaseEntity<TId>
            where TId : IComparable, IComparable<TId>, IEquatable<TId>
            where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
        {
            mockRestHandlerProvider = mockRestHandlerProvider ?? new Mock<IRestHandlerProviderAlternateKey<TEntity, TInterface, TId, TAltKey>>();
            var mockTokenService = new Mock<IJWTToken>();
            var service = new EntityWebServiceAlternateKey<TEntity, TInterface, TId, TAltKey>(mockRestHandlerProvider.Object);
            return service;
        }

        [TestMethod]
        public async Task EntityWebServiceAlternateKey_GetByAlternateKeyHandler_WasCalled()
        {
            // Arrange
            var id = "1";
            var entity = new EntityInt { Id = 1, Name = "E1" };
            var odataEntity = entity.AsOdata<EntityInt, int>();
            var mockRestHandlerProvider = new Mock<IRestHandlerProviderAlternateKey<EntityInt, IEntityInt, int, string>>();
            var mockGetByAlternateKeyHandler = new Mock<IGetByAlternateKeyHandler<EntityInt, IEntityInt, int, string>>();
            mockGetByAlternateKeyHandler.Setup(m => m.HandleAsync(It.IsAny<string>()))
                                        .ReturnsAsync(odataEntity);
            mockRestHandlerProvider.Setup(m => m.GetByAlternateKeyHandler).Returns(mockGetByAlternateKeyHandler.Object);
            var webService = GetWebService<EntityInt, IEntityInt, int, string>(mockRestHandlerProvider);

            // Act
            var actual = await webService.GetAsync(id);

            // Assert
            mockGetByAlternateKeyHandler.Verify(m => m.HandleAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
