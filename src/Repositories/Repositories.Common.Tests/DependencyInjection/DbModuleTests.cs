using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Repositories.DependencyInjection;
using System.Collections.Specialized;
using IAppSettings = Rhyous.EntityAnywhere.Interfaces.IAppSettings;

namespace Rhyous.EntityAnywhere.Repositories.Common.Tests.DependencyInjection
{
    [TestClass]
    public class DbModuleTests
    {
        private MockRepository _MockRepository;

        private Mock<IAppSettings> _MockAppSettings;
        private Mock<IUrlParameters> _MockUrlParameters;
        private Mock<IUserDetails> _MockUserDetails;
        private Mock<IObjectCreator<IBaseDbContext<User>>> _MockObjectCreator;

        private IContainer _Container;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAppSettings = _MockRepository.Create<IAppSettings>();
            _MockUrlParameters = _MockRepository.Create<IUrlParameters>();
            _MockUserDetails = _MockRepository.Create<IUserDetails>();
            _MockObjectCreator = _MockRepository.Create<IObjectCreator<IBaseDbContext<User>>>();

            var builder = new ContainerBuilder();
            // Register dependencies that should come from upstream
            builder.RegisterInstance(_MockAppSettings.Object).As<IAppSettings>();
            builder.RegisterInstance(_MockUrlParameters.Object).As<IUrlParameters>();
            builder.RegisterInstance(_MockUserDetails.Object).As<IUserDetails>();
            builder.RegisterInstance(_MockObjectCreator.Object).As<IObjectCreator<IBaseDbContext<User>>>();

            var appSettings = new NameValueCollection
            {
                { "SystemSecurityKey", "cypherKey" },
                { "SystemSecurityDerivationIterations", "1000" }
            };
            builder.RegisterInstance(_MockAppSettings.Object).As<IAppSettings>();

            // Register this modules dependencies
            var registrar = new Registrar();
            registrar.Register(builder);
            _Container = builder.Build();
        }

        [TestMethod]
        public void DbModule_IAuditablesHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IAuditablesHandler>());
        }

        [TestMethod]
        public void DbModule_IAuditablesHandler_IsRegistered_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IAuditablesHandler>(), _Container.Resolve<IAuditablesHandler>());
        }

        [TestMethod]
        public void DbModule_IEntityConnectionStringNameProvider_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityConnectionStringNameProvider<User>>());
        }

        [TestMethod]
        public void DbModule_IEntityConnectionStringNameProvider_IsRegistered_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IEntityConnectionStringNameProvider<User>>(), _Container.Resolve<IEntityConnectionStringNameProvider<User>>());
        }

        [TestMethod]
        public void DbModule_ISettings_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<ISettings<User>>());
        }

        [TestMethod]
        public void DbModule_ISettings_IsRegistered_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<ISettings<User>>(), _Container.Resolve<ISettings<User>>());
        }

        [TestMethod]
        public void DbModule_IMigrationsConfigurationContainer_IsRegistered()
        {
            using (var scope = _Container.BeginLifetimeScope(b =>
            {
                var mockSettings = _MockRepository.Create<ISettings<User>>();
                b.RegisterInstance(mockSettings.Object).As<ISettings<User>>();
                mockSettings.Setup(m => m.AutomaticMigrationsEnabled).Returns(true);
                mockSettings.Setup(m => m.AutomaticMigrationDataLossAllowed).Returns(true);
                mockSettings.Setup(m => m.ContextKey).Returns("EAF.User");
            }))
            {
                Assert.IsNotNull(scope.Resolve<IMigrationsConfigurationContainer<User>>());
            };
        }

        [TestMethod]
        public void DbModule_IBaseDbContext_IsRegistered()
        {
            using (var scope = _Container.BeginLifetimeScope(b =>
            {
                var mockEntityConnectionStringNameProvider = _MockRepository.Create<IEntityConnectionStringNameProvider<User>>();
                b.RegisterInstance(mockEntityConnectionStringNameProvider.Object).As<IEntityConnectionStringNameProvider<User>>();
                mockEntityConnectionStringNameProvider.Setup(m => m.Provide())
                                                      .Returns("User");
                var mockSettings = _MockRepository.Create<ISettings<User>>();
                b.RegisterInstance(mockSettings.Object).As<ISettings<User>>();
                mockSettings.Setup(m => m.ProxyCreationEnabled).Returns(true);
                mockSettings.Setup(m => m.LazyLoadingEnabled).Returns(true);
                mockSettings.Setup(m => m.UseEntityFrameworkDatabaseManagement).Returns(true);
                mockSettings.Setup(m => m.AutomaticMigrationsEnabled).Returns(true);
                mockSettings.Setup(m => m.AutomaticMigrationDataLossAllowed).Returns(true);
                mockSettings.Setup(m => m.ContextKey).Returns("EAF.User");
            }))
            {
                Assert.IsNotNull(scope.Resolve<IBaseDbContext<User>>());
            };
        }

        [TestMethod]
        public void DbModule_IUpdateDbContext_IsRegistered()
        {
            using (var scope = _Container.BeginLifetimeScope(b =>
            {
                var mockEntityConnectionStringNameProvider = _MockRepository.Create<IEntityConnectionStringNameProvider<User>>();
                b.RegisterInstance(mockEntityConnectionStringNameProvider.Object).As<IEntityConnectionStringNameProvider<User>>();
                mockEntityConnectionStringNameProvider.Setup(m => m.Provide())
                                                      .Returns("User");
                var mockSettings = _MockRepository.Create<ISettings<User>>();
                b.RegisterInstance(mockSettings.Object).As<ISettings<User>>();
                mockSettings.Setup(m => m.ProxyCreationEnabled).Returns(true);
                mockSettings.Setup(m => m.LazyLoadingEnabled).Returns(true);
                mockSettings.Setup(m => m.UseEntityFrameworkDatabaseManagement).Returns(true);
                mockSettings.Setup(m => m.AutomaticMigrationsEnabled).Returns(true);
                mockSettings.Setup(m => m.AutomaticMigrationDataLossAllowed).Returns(true);
                mockSettings.Setup(m => m.ContextKey).Returns("EAF.User");
            }))
            {
                Assert.IsNotNull(scope.Resolve<IUpdateDbContext<User>>());
            };
        }

        [TestMethod]
        public void DbModule_IRepository_User_IUser_Long_IsRegistered()
        {
            using (var scope = _Container.BeginLifetimeScope(b =>
            {
                b.RegisterGeneric(typeof(SqlRepository<,,>)).As(typeof(IRepository<,,>));
            }))
            {
                Assert.IsNotNull(scope.Resolve<IRepository<User, IUser, int>>());
            };
        }
    }
}
