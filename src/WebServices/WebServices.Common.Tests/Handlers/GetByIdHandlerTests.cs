using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using Rhyous.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;

using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityInt;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests
{
    [TestClass]
    public class GetByIdHandlerTests
    {
        private MockRepository _MockRepository;
        private Mock<IServiceCommon<TEntity, TInterface, TId>> _MockServiceCommon;
        private Mock<IRelatedEntityProvider<TEntity, TInterface, TId>> _MockRelatedEntityProvider;
        private Mock<IIdDisambiguator<TEntity, TId>> _MockIdDisambiguator;
        private Mock<INamedFactory<IExtensionEntityClientAsync>> _MockExtensionEntityClientFactory;
        private Mock<IExtensionEntityClientAsync> _MockAlternateIdClientAsync;
        private IUrlParameters _UrlParameters;
        private IRequestUri _RequestUri;
        private Mock<IOutgoingWebResponseContext> _MockOutgoingWebResponseContext;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockServiceCommon = _MockRepository.Create<IServiceCommon<TEntity, TInterface, TId>>();
            _MockRelatedEntityProvider = _MockRepository.Create<IRelatedEntityProvider<TEntity, TInterface, TId>>();
            _MockIdDisambiguator = _MockRepository.Create<IIdDisambiguator<TEntity, TId>>();
            _MockExtensionEntityClientFactory = _MockRepository.Create<INamedFactory<IExtensionEntityClientAsync>>();
            _MockAlternateIdClientAsync = _MockRepository.Create<IExtensionEntityClientAsync>();
            _MockExtensionEntityClientFactory.Setup(m => m.Create(It.IsAny<string>()))
                                             .Returns(_MockAlternateIdClientAsync.Object);
            _UrlParameters = new UrlParameters { };
            _RequestUri = new RequestUri { Uri = new Uri("https://fake.domain.tld/Api/EntityBasicService/EntityBasics(101)") };
            _MockOutgoingWebResponseContext = _MockRepository.Create<IOutgoingWebResponseContext>();
        }

        private GetByIdHandler<TEntity, TInterface, int> GetHandler()
        {
            return new GetByIdHandler<TEntity, TInterface, TId>(_MockServiceCommon.Object,
                                                                _MockRelatedEntityProvider.Object,
                                                                _MockIdDisambiguator.Object,
                                                                _MockExtensionEntityClientFactory.Object,
                                                                _UrlParameters,
                                                                _RequestUri,
                                                                _MockOutgoingWebResponseContext.Object);
        }

        #region GetByTId tests

        [TestMethod]
        public async Task GetByIdHandler_GetByTId_DefaultT_Int_Test()
        {
            // Arrange
            int id = 0;
            GetByIdHandler<TEntity, TInterface, int> getHandler = GetHandler();

            // Act
            var actual = await getHandler.GetByTIdAsync(id);

            // Assert
            Assert.IsNull(actual);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Handle invalid input tests
        [TestMethod]
        public async Task GetByIdHandler_Handle_DefaultT_Test()
        {
            // Arrange
            string id = "0";
            var disambiguated = new DisambiguatedId<TId> { Id = 0 };
            _MockIdDisambiguator.Setup(m => m.Disambiguate(It.IsAny<string>()))
                    .Returns(disambiguated);
            _MockOutgoingWebResponseContext.SetupSet(m => m.StatusCode = HttpStatusCode.NotFound);
            var getHandler = GetHandler();


            // Act
            var actual = await getHandler.HandleAsync(id);

            // Assert
            Assert.IsNull(actual);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByIdHandler_Handle_IdIsNull()
        {
            // Arrange
            string id = null;
            _MockOutgoingWebResponseContext.SetupSet(m => m.StatusCode = HttpStatusCode.BadRequest);
            var getHandler = GetHandler();

            // Act
            var actual = await getHandler.HandleAsync(id);

            // Assert
            Assert.IsNull(actual);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByIdHandler_Handle_IdIsEmpty()
        {
            // Arrange
            string id = "";
            _MockOutgoingWebResponseContext.SetupSet(m => m.StatusCode = HttpStatusCode.BadRequest);
            var getHandler = GetHandler();


            // Act
            var actual = await getHandler.HandleAsync(id);

            // Assert
            Assert.IsNull(actual);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByIdHandler_Handle_IdIsWhitespace()
        {
            // Arrange
            string id = "   ";
            _MockServiceCommon.Setup(m => m.Get(It.IsAny<TId>())).Returns((TInterface)null);
            _MockRelatedEntityProvider.Setup(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()));
            var disambiguated = new DisambiguatedId<TId> { Id = 0 };
            _MockIdDisambiguator.Setup(m => m.Disambiguate(It.IsAny<string>()))
                    .Returns(disambiguated);
            _MockOutgoingWebResponseContext.SetupSet(m => m.StatusCode = HttpStatusCode.BadRequest);
            var getHandler = GetHandler();

            // Act
            var actual = await getHandler.HandleAsync(id);

            // Assert
            Assert.IsNull(actual);
            _MockServiceCommon.Verify(m => m.Get(It.IsAny<TId>()), Times.Never);
        }

        [TestMethod]
        public async Task GetByIdHandler_Handle_IdIsWrongType_NotInt_AndNotAnAlternateKey()
        {
            // Arrange
            string id = "XYZ"; // should be int
            _MockServiceCommon.Setup(m => m.Get(It.IsAny<TId>())).Returns((TInterface)null);
            _MockRelatedEntityProvider.Setup(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()));
            var disambiguated = new DisambiguatedId<TId> { Id = 0 };
            _MockIdDisambiguator.Setup(m => m.Disambiguate(It.IsAny<string>()))
                    .Returns(disambiguated);
            _MockOutgoingWebResponseContext.SetupSet(m => m.StatusCode = HttpStatusCode.NotFound);
            var getHandler = GetHandler();

            // Act
            var actual = await getHandler.HandleAsync(id);

            // Assert
            Assert.IsNull(actual);
            _MockServiceCommon.Verify(m => m.Get(It.IsAny<TId>()), Times.Never);
        }

        [TestMethod]
        public async Task GetByIdHandler_Handle_IdIsJustQuotes()
        {
            // Arrange
            string id = "\"\""; // should be int
            _MockServiceCommon.Setup(m => m.Get(It.IsAny<TId>())).Returns((TInterface)null);
            _MockRelatedEntityProvider.Setup(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()));
            var disambiguated = new DisambiguatedId<TId> { Id = 0 };
            _MockIdDisambiguator.Setup(m => m.Disambiguate(It.IsAny<string>()))
                    .Returns(disambiguated);
            _MockOutgoingWebResponseContext.SetupSet(m => m.StatusCode = HttpStatusCode.BadRequest);
            var getHandler = GetHandler();

            // Act
            var actual = await getHandler.HandleAsync(id);

            // Assert
            Assert.IsNull(actual);
            _MockServiceCommon.Verify(m => m.Get(It.IsAny<TId>()), Times.Never);
        }

        [TestMethod]
        public async Task GetByIdHandler_Handle_IdIsQuotesAndWhitespace()
        {
            // Arrange
            string id = "\"   \""; // should be int
            _MockServiceCommon.Setup(m => m.Get(It.IsAny<TId>())).Returns((TInterface)null);
            _MockRelatedEntityProvider.Setup(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()));
            var disambiguated = new DisambiguatedId<TId> { Id = 0 };
            _MockIdDisambiguator.Setup(m => m.Disambiguate(It.IsAny<string>()))
                    .Returns(disambiguated);
            _MockOutgoingWebResponseContext.SetupSet(m => m.StatusCode = HttpStatusCode.BadRequest);
            var getHandler = GetHandler();

            // Act
            var actual = await getHandler.HandleAsync(id);

            // Assert
            Assert.IsNull(actual);
            _MockServiceCommon.Verify(m => m.Get(It.IsAny<TId>()), Times.Never);
        }
        #endregion

        #region Handle valid

        [TestMethod]
        public async Task GetByIdHandler_Handle_Valid_Test()
        {
            // Arrange
            string id = "81";
            var entityInt = new TEntity { Id = 81 };
            _MockServiceCommon.Setup(m => m.Get(It.IsAny<int>())).Returns(entityInt);
            var disambiguated = new DisambiguatedId<TId> { Id = 81 };
            _MockIdDisambiguator.Setup(m => m.Disambiguate(It.IsAny<string>()))
                    .Returns(disambiguated);
            _MockRelatedEntityProvider.Setup(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()))
                                      .Returns(Task.CompletedTask);
            var getHandler = GetHandler();

            // Act
            var actual = await getHandler.HandleAsync(id);

            // Assert
            Assert.IsNotNull(actual);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByIdHandler_Handle_Valid_IdUnquotes_Test()
        {
            // Arrange
            string id = "'81'";
            var entityInt = new TEntity { Id = 81 };
            _MockServiceCommon.Setup(m => m.Get(It.IsAny<int>())).Returns(entityInt);
            var disambiguated = new DisambiguatedId<TId> { Id = 81 };
            _MockIdDisambiguator.Setup(m => m.Disambiguate(It.IsAny<string>()))
                    .Returns(disambiguated);
            _MockRelatedEntityProvider.Setup(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()))
                                      .Returns(Task.CompletedTask);
            var getHandler = GetHandler();

            // Act
            var actual = await getHandler.HandleAsync(id);

            // Assert
            Assert.IsNotNull(actual);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Handle Id Disambiguation
        [TestMethod]
        public async Task GetByAlternateKeyHandler_AlternateIdReturnsNull_Test()
        {
            // Arrange
            string idOrAltId = $"{IdDisambiguator.Alt}.System1.1";
            var disambiguated = new DisambiguatedId<TId>
            {
                Id = 127,
                IdType = IdType.Alt,
                AlternateIdProperty = IdDisambiguator.Alt
            };
            _MockAlternateIdClientAsync.Setup(m => m.GetByEntityPropertyValueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                       .ReturnsAsync((string)null);
            _MockIdDisambiguator.Setup(m => m.Disambiguate(It.IsAny<string>()))
                                .Returns(disambiguated);
            _MockOutgoingWebResponseContext.SetupSet(m => m.StatusCode = HttpStatusCode.NotFound);
            var getHandler = GetHandler();

            // Act
            var result = await getHandler.HandleAsync(idOrAltId);

            // Assert
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByAlternateKeyHandler_AlternateIdReturnsEmptyJson_Test()
        {
            // Arrange
            string idOrAltId = $"{IdDisambiguator.Alt}.System1.1";
            var disambiguated = new DisambiguatedId<TId>
            {
                Id = 127,
                IdType = IdType.Alt,
                AlternateIdProperty = IdDisambiguator.Alt
            };
            var altIdJson = "{\"Count\":1,\"Entities\":[],\"Entity\":\"AlternateId\",\"TotalCount\":1}";
            _MockAlternateIdClientAsync.Setup(m => m.GetByEntityPropertyValueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                       .ReturnsAsync(altIdJson);
            _MockIdDisambiguator.Setup(m => m.Disambiguate(It.IsAny<string>()))
                                .Returns(disambiguated);
            _MockOutgoingWebResponseContext.SetupSet(m => m.StatusCode = HttpStatusCode.NotFound);
            var getHandler = GetHandler();

            // Act
            var result = await getHandler.HandleAsync(idOrAltId);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}