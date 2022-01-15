using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.ExtensionEntityBasic;
using TId = System.Int64;
using TInterface = Rhyous.EntityAnywhere.Interfaces.IExtensionEntity;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class GetByEntityIdentifiersTests
    {
        private MockRepository _MockRepository;

        private Mock<IServiceCommon<TEntity, TInterface, TId>> _MockServiceCommon;
        private IRequestUri _Uri;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockServiceCommon = _MockRepository.Create<IServiceCommon<TEntity, TInterface, TId>>();
            _Uri = new RequestUri { Uri = new Uri("https://fake.domain.tld/Api/EntityBasicService/EntityBasics(101)") };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private GetByEntityIdentifiers<TEntity, TInterface, TId> CreateGetByEntityIdentifiers()
        {
            return new GetByEntityIdentifiers<TEntity, TInterface, TId>(
                _MockServiceCommon.Object,
                _Uri);
        }

        [TestMethod]
        public void GetByEntityIdentifiers_Handle_NullParam_Test()
        {
            // Arrange
            var getByEntityIdentifiers = CreateGetByEntityIdentifiers();
            List<EntityIdentifier> entityIdentifiers = null;

            // Act
            // Assert
            Assert.ThrowsException<RestException>(() =>
            {
                getByEntityIdentifiers.Handle(entityIdentifiers);
            });
        }

        [TestMethod]
        public void GetByEntityIdentifiers_Handle_EmptyListParam_Test()
        {
            // Arrange
            var getByEntityIdentifiers = CreateGetByEntityIdentifiers();
            var entityIdentifiers = new List<EntityIdentifier>();

            // Act
            // Assert
            Assert.ThrowsException<RestException>(() =>
            {
                getByEntityIdentifiers.Handle(entityIdentifiers);
            });
        }
    }
}
