using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System.Data.Entity.Migrations;

namespace Rhyous.EntityAnywhere.Repositories.Common.Tests
{
    [TestClass]
    public class BaseDbContextTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityConnectionStringProvider<User>> _MockEntityConnectionStringProvider;
        private Mock<IAuditablesHandler> _MockAuditablesHandler;
        private Mock<ISettings<User>> _MockSettings;
        private Mock<IUserDetails> _MockUserDetails;
        private Mock<IMigrationsConfigurationContainer<User>> _MockMigrationsConfigurationContainer;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityConnectionStringProvider = _MockRepository.Create<IEntityConnectionStringProvider<User>>();
            _MockEntityConnectionStringProvider.Setup(m => m.Provide()).Returns("Data Source=FakeServer;Initial Catalog=FakeDb;Integrated Security=True;TrustServerCertificate=True");
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
                _MockEntityConnectionStringProvider.Object,
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
