using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.WebFramework.Clients2;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Authenticators.DependencyInjection.Tests
{
    [TestClass]
    public class ActivationCredentialsValidatorModuleTests
    {
        private MockRepository _MockRepository;
        private Mock<IAdminEntityClientAsync<Organization, int>> _MockOrganizationClient;
        private Mock<IAdminEntityClientAsync<ActivationCredential, long>> _MockActivationCredentialClient;
        private Mock<IClaimsBuilder> _MockClaimsBuilder;
        private Mock<IJWTToken> _MockJwtToken;
        private Mock<IAppSettings> _MockAppSettings;
        private Mock<ILogger> _MockLogger;

        private IContainer _Container;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            var builder = new ContainerBuilder();

            // Register upstream dependencies
            builder.RegisterModule<SimplePluginLoaderModule>();

            _MockOrganizationClient = _MockRepository.Create<IAdminEntityClientAsync<Organization, int>>();
            builder.RegisterInstance(_MockOrganizationClient.Object).As<IAdminEntityClientAsync<Organization, int>>();

            _MockActivationCredentialClient = _MockRepository.Create<IAdminEntityClientAsync<ActivationCredential, long>>();
            builder.RegisterInstance(_MockActivationCredentialClient.Object).As<IAdminEntityClientAsync<ActivationCredential, long>>();

            _MockClaimsBuilder = _MockRepository.Create<IClaimsBuilder>();
            builder.RegisterInstance(_MockClaimsBuilder.Object).As<IClaimsBuilder>();

            _MockJwtToken = _MockRepository.Create<IJWTToken>();
            builder.RegisterInstance(_MockJwtToken.Object).As<IJWTToken>();

            _MockAppSettings = _MockRepository.Create<IAppSettings>();
            builder.RegisterInstance(_MockAppSettings.Object).As<IAppSettings>();

            _MockLogger = _MockRepository.Create<ILogger>();
            builder.RegisterInstance(_MockLogger.Object).As<ILogger>();

            // Register module
            var registrar = new Registrar();
            registrar.Register(builder);
            _Container = builder.Build();
        }

        [TestMethod]
        public void ProductServiceModule_IClaimsBuilder_Registered()
        {
            Assert.IsNotNull(_Container.Resolve<IClaimsBuilder>());
        }

        [TestMethod]
        public void ProductServiceModule_ActivationCredentialCredentialsValidator_Registered()
        {
            using (var scope = _Container.BeginLifetimeScope((b) =>
            {
                b.RegisterType<ActivationCredentialCredentialsValidator>();
            }))
            {
                Assert.IsNotNull(scope.Resolve<ActivationCredentialCredentialsValidator>());
            };
        }
    }
}