using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;

namespace Rhyous.EntityAnywhere.HeaderValidators.DependencyInjection.Tests
{
    [TestClass]
    public class OAuthHeaderValidatorModuleTests
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
        public void HeaderValidatorsCommonModule_IBearerDecoder_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<IBearerDecoder>());
        }

        [TestMethod]
        public void OAuthHeaderValidatorModule_IBearerDecoder_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IBearerDecoder>(), _Container.Resolve<IBearerDecoder>());
        }

        [TestMethod]
        public void OAuthHeaderValidatorModule_IHeadersUpdater_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<IHeadersUpdater>());
        }

        [TestMethod]
        public void OAuthHeaderValidatorModule_IHeadersUpdater_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IHeadersUpdater>(), _Container.Resolve<IHeadersUpdater>());
        }

        [TestMethod]
        public void OAuthHeaderValidatorModule_ICustomCustomerRoleAuthorization_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<ICustomCustomerRoleAuthorization>());
        }

        [TestMethod]
        public void OAuthHeaderValidatorModule_ICustomCustomerRoleAuthorization_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<ICustomCustomerRoleAuthorization>(), _Container.Resolve<ICustomCustomerRoleAuthorization>());
        }

        [TestMethod]
        public void OAuthHeaderValidatorModule_IJWTValidator_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<IJWTValidator>());
        }

        [TestMethod]
        public void OAuthHeaderValidatorModule_IJWTValidator_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IJWTValidator>(), _Container.Resolve<IJWTValidator>());
        }

        [TestMethod]
        public void OAuthHeaderValidatorModule_ITokenFromClaimsBuilder_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<ITokenFromClaimsBuilder>());
        }

        [TestMethod]
        public void OAuthHeaderValidatorModule_ITokenFromClaimsBuilder_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<ITokenFromClaimsBuilder>(), _Container.Resolve<ITokenFromClaimsBuilder>());
        }

        [TestMethod]
        public void OAuthHeaderValidatorModule_OAuthHeaderValidator_CanBeLoaded()
        {
            using (var scope = _Container.BeginLifetimeScope(builder =>
            {
                builder.RegisterType<OAuthHeaderValidator>();
            }))
            {
                Assert.IsNotNull(scope.Resolve<OAuthHeaderValidator>());
            }
        }
    }
}
