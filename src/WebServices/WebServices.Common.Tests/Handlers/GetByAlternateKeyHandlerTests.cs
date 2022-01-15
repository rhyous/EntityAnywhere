using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using Rhyous.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using TAltKey = System.String;
using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityBasic;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityBasic;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class GetByAlternateKeyHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IServiceCommonAlternateKey<TEntity, TInterface, TId, TAltKey>> _MockServiceCommonAlternateKey;
        private Mock<IRelatedEntityProvider<TEntity, TInterface, TId>> _MockRelatedEntityProvider;
        private Mock<IIdDisambiguator<TEntity, TId, TAltKey>> _MockIdDisambiguator;
        private IUrlParameters _UrlParameters;
        private IRequestUri _RequestUri;
        private Mock<IOutgoingWebResponseContext> _MockOutgoingWebResponseContext;
        private Mock<IGetByIdHandler<TEntity, TInterface, TId>> _MockGetByIdHandler;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockServiceCommonAlternateKey = _MockRepository.Create<IServiceCommonAlternateKey<TEntity, TInterface, TId, TAltKey>>();
            _MockRelatedEntityProvider = _MockRepository.Create<IRelatedEntityProvider<TEntity, TInterface, TId>>();
            _MockIdDisambiguator = _MockRepository.Create<IIdDisambiguator<TEntity, TId, TAltKey>>();
            _MockOutgoingWebResponseContext = _MockRepository.Create<IOutgoingWebResponseContext>();
            _MockGetByIdHandler = _MockRepository.Create<IGetByIdHandler<TEntity, TInterface, TId>>();
            _UrlParameters = new UrlParameters { };
            _RequestUri = new RequestUri { Uri = new Uri("https://fake.domain.tld/Api/EntityBasicService/EntityBasics(101)") };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private GetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> CreateGetByAlternateKeyHandler()
        {
            return new GetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>(
                _MockServiceCommonAlternateKey.Object,
                _MockRelatedEntityProvider.Object,
                _MockIdDisambiguator.Object,
                _UrlParameters,
                _RequestUri,
                _MockOutgoingWebResponseContext.Object,
                _MockGetByIdHandler.Object);
        }

        [TestMethod]
        public async Task GetByAlternateKeyHandler_Handle_NullId_Throws_Test()
        {
            // Arrange
            var getByAlternateKeyHandler = CreateGetByAlternateKeyHandler();
            string idOrAltId = null;
            var ret = new OdataObject<TEntity, TId>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                var result = await getByAlternateKeyHandler.HandleAsync(idOrAltId);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByAlternateKeyHandler_Handle_EmptyStringId_Throws_Test()
        {
            // Arrange
            var getByAlternateKeyHandler = CreateGetByAlternateKeyHandler();
            string idOrAltId = "";
            var ret = new OdataObject<TEntity, TId>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                var result = await getByAlternateKeyHandler.HandleAsync(idOrAltId);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByAlternateKeyHandler_Handle_WhitespaceStringId_Throws_Test()
        {
            // Arrange
            var getByAlternateKeyHandler = CreateGetByAlternateKeyHandler();
            string idOrAltId = "    ";
            var ret = new OdataObject<TEntity, TId>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                var result = await getByAlternateKeyHandler.HandleAsync(idOrAltId);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByAlternateKeyHandler_Handle_QuotedWhitespaceStringId_Throws_Test()
        {
            // Arrange
            var getByAlternateKeyHandler = CreateGetByAlternateKeyHandler();
            string idOrAltId = "\"    \"";
            var ret = new OdataObject<TEntity, TId>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                var result = await getByAlternateKeyHandler.HandleAsync(idOrAltId);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByAlternateKeyHandler_Handle_ValidIntId_Test()
        {
            // Arrange
            var getByAlternateKeyHandler = CreateGetByAlternateKeyHandler();
            string idOrAltId = "127";
            var disambiguated = new DisambiguatedId<TId> { Id = 127 };
            TEntity entity = new TEntity();
            var odataObject = new OdataObject<TEntity, TId> { Object = entity };
            _MockGetByIdHandler.Setup(m => m.HandleAsync(idOrAltId)).ReturnsAsync(odataObject);
            _MockIdDisambiguator.Setup(m => m.Disambiguate(It.IsAny<string>()))
                                .Returns(disambiguated);

            // Act
            var result = await getByAlternateKeyHandler.HandleAsync(idOrAltId);

            // Assert
            Assert.AreEqual(entity, result.Object);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("DataDriven")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"Data\IdsWithEmbeddedQuotes.csv", "IdsWithEmbeddedQuotes#csv", DataAccessMethod.Sequential)]
        public async Task GetByIdHandler_Handle_Valid_TwoQuotesToOneQuote_Test()
        {
            // Arrange
            var expected = TestContext.DataRow["Expected"].ToString();
            var orginalId = TestContext.DataRow["Id"].ToString();
            var msg = TestContext.DataRow["Message"].ToString();
            string inputId = null;

            var disambiguated = new DisambiguatedId<TId> { Id = 81 };
            _MockIdDisambiguator.Setup(m => m.Disambiguate(It.IsAny<string>()))
                    .Returns(disambiguated);
            var getByAlternateKeyHandler = CreateGetByAlternateKeyHandler();
            _MockGetByIdHandler.Setup(m => m.HandleAsync(It.IsAny<string>())) // Can't be specific as we are changing the quoting of the string
                               .ReturnsAsync((string inId) => 
                               {
                                   inputId = inId;
                                   return null;
                               });

            // Act
            var actual = await getByAlternateKeyHandler.HandleAsync(orginalId);

            // Assert
            Assert.AreEqual(expected, inputId);
            Assert.IsNull(actual);
            _MockRepository.VerifyAll();
        }
    }
}
