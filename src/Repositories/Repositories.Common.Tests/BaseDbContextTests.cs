using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Repositories;
using System;
using System.Data.Entity.Migrations;

namespace Rhyous.EntityAnywhere.Repositories.Common.Tests
{
    [TestClass]
    public class BaseDbContextTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityConnectionStringNameProvider<User>> _MockEntityConnectionStringNameProvider;
        private Mock<IAuditablesHandler> _MockAuditablesHandler;
        private Mock<ISettings<User>> _MockSettings;
        private Mock<IUserDetails> _MockUserDetails;
        private Mock<IMigrationsConfigurationContainer<User>> _MockMigrationsConfigurationContainer;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityConnectionStringNameProvider = _MockRepository.Create<IEntityConnectionStringNameProvider<User>>();
            _MockEntityConnectionStringNameProvider.Setup(m => m.Provide()).Returns($"{nameof(User)}SqlRepository");
            _MockAuditablesHandler = _MockRepository.Create<IAuditablesHandler>();
            _MockSettings = _MockRepository.Create<ISettings<User>>();
            _MockSettings.Setup(m => m.ProxyCreationEnabled).Returns(true);
            _MockSettings.Setup(m => m.LazyLoadingEnabled).Returns(true);
            _MockUserDetails = _MockRepository.Create<IUserDetails>();
            _MockMigrationsConfigurationContainer = _MockRepository.Create<IMigrationsConfigurationContainer<User>>();
        }

        private BaseDbContext<User> CreateBaseDbContext()
        {
            return new BaseDbContext<User>(
                _MockEntityConnectionStringNameProvider.Object,
                _MockAuditablesHandler.Object,
                _MockSettings.Object,
                _MockUserDetails.Object,
                _MockMigrationsConfigurationContainer.Object);
        }

        #region Constructor
        [TestMethod]
        public void BaseDbContext_Constructor__UseEntityFrameworkDatabaseManagement_False()
        {
            // Arrange
            _MockSettings.Setup(m => m.UseEntityFrameworkDatabaseManagement).Returns(false);
            var baseDbContext = CreateBaseDbContext();
            bool proxyCreationEnabled = false;
            bool lazyLoadingEnabled = false;
            bool asNoTracking = false;

            // Act
            baseDbContext.SetConfig(proxyCreationEnabled, lazyLoadingEnabled, asNoTracking);

            // Assert
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BaseDbContext_Constructor_UseEntityFrameworkDatabaseManagement_True()
        {
            // Arrange
            _MockSettings.Setup(m => m.UseEntityFrameworkDatabaseManagement).Returns(true);
            _MockMigrationsConfigurationContainer.Setup(m => m.Config).Returns(new DbMigrationsConfiguration<BaseDbContext<User>>());
            var baseDbContext = CreateBaseDbContext();
            bool proxyCreationEnabled = false;
            bool lazyLoadingEnabled = false;
            bool asNoTracking = false;

            // Act
            baseDbContext.SetConfig(proxyCreationEnabled, lazyLoadingEnabled, asNoTracking);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region SetConfig
        [TestMethod]
        public void BaseDbContext_SetConfig_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            _MockSettings.Setup(m => m.UseEntityFrameworkDatabaseManagement).Returns(false);
            var baseDbContext = CreateBaseDbContext();
            bool proxyCreationEnabled = false;
            bool lazyLoadingEnabled = false;
            bool asNoTracking = false;

            // Act
            baseDbContext.SetConfig(proxyCreationEnabled, lazyLoadingEnabled, asNoTracking);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
