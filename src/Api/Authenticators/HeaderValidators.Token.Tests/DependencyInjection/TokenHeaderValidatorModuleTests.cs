using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.HeaderValidators.DependencyInjection.Tests
{
    [TestClass]
    public class TokenHeaderValidatorModuleTests
    {
        private MockRepository _MockRepository;

        private IContainer _Container;


        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            var builder = new ContainerBuilder();

            // Register upstream dependencies
            builder.RegisterModule<SimplePluginLoaderModule>();

            var mockTokenDecoder = _MockRepository.Create<ITokenDecoder>();
            builder.RegisterInstance(mockTokenDecoder.Object).As<ITokenDecoder>();

            var mockAppSettings = _MockRepository.Create<IAppSettings>();
            builder.RegisterInstance(mockAppSettings.Object).As<IAppSettings>();

            var mockUserRoleEntityDataCache = _MockRepository.Create<IUserRoleEntityDataCache>();
            builder.RegisterInstance(mockUserRoleEntityDataCache.Object).As<IUserRoleEntityDataCache>();

            // Register module
            var registrar = new Registrar();
            registrar.Register(builder);
            _Container = builder.Build();
        }

        [TestMethod]
        public void TokenHeaderValidatorModule_IHeadersUpdater_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<IHeadersUpdater>());
        }

        [TestMethod]
        public void TokenHeaderValidatorModule_IHeadersUpdater_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IHeadersUpdater>(), _Container.Resolve<IHeadersUpdater>());
        }

        [TestMethod]
        public void TokenHeaderValidatorModule_ICustomCustomerRoleAuthorization_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<ICustomCustomerRoleAuthorization>());
        }

        [TestMethod]
        public void TokenHeaderValidatorModule_ICustomCustomerRoleAuthorization_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<ICustomCustomerRoleAuthorization>(), _Container.Resolve<ICustomCustomerRoleAuthorization>());
        }

        [TestMethod]
        public void TokenHeaderValidatorModule_TokenHeaderValidator_CanBeLoaded()
        {
            using (var scope = _Container.BeginLifetimeScope(builder =>
            {
                builder.RegisterType<TokenHeaderValidator>();
            }))
            {
                Assert.IsNotNull(scope.Resolve<TokenHeaderValidator>());
            }
        }
    }
}
