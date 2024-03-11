using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.DependencyInjection;

namespace Rhyous.EntityAnywhere.Services.Tests.DependencyInjection
{
    [TestClass]
    public class TokenModuleTests
    {
        private MockRepository _MockRepository;

        private IContainer _Container;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            var builder = new ContainerBuilder();

            // Register upstream objects
            var mockLogger = _MockRepository.Create<ILogger>();
            builder.RegisterInstance(mockLogger.Object).As<ILogger>();

            var mockAppSettings = _MockRepository.Create<IAppSettings>();
            builder.RegisterInstance(mockAppSettings.Object).As<IAppSettings>();

            var mockTokenKeyPair = _MockRepository.Create<ITokenKeyPair>();
            builder.RegisterInstance(mockTokenKeyPair.Object).As<ITokenKeyPair>();

            var mockClaimConfigurationClient = _MockRepository.Create<IAdminEntityClientAsync<ClaimConfiguration, int>>();
            builder.RegisterInstance(mockClaimConfigurationClient.Object).As<IAdminEntityClientAsync<ClaimConfiguration, int>>();

            var mockUserClient = _MockRepository.Create<IAdminEntityClientAsync<User, long>>();
            builder.RegisterInstance(mockUserClient.Object).As<IAdminEntityClientAsync<User, long>>();

            builder.RegisterModule<SimplePluginLoaderModule>();

            // Register plugin Registrar
            var registrar = new Registrar();
            registrar.Register(builder);
            _Container = builder.Build();
        }

        [TestMethod]
        public void TokenModule_Resolve_IJWTToken_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IJWTToken>());
        }

        [TestMethod]
        public void TokenModule_Resolve_ITokenBuilder_Test()
        {
            Assert.IsNotNull(_Container.Resolve<ITokenBuilder<IUser>>());
        }

        [TestMethod]
        public void TokenModule_Resolve_IClaimsBuilderAsync_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IClaimsBuilderAsync>());
        }
    }
}