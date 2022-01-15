using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Authenticators.DependencyInjection.Tests
{
    [TestClass]
    public class UserCredentialsValidatorModuleTests
    {
        private MockRepository _MockRepository;
        private Mock<IAdminEntityClientAsync<User, long>> _MockUserClient;
        private Mock<ITokenBuilder<IUser>> _MockTokenBuilder;
        private Mock<IAppSettings> _MockAppSettings;

        private IContainer _Container;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            var builder = new ContainerBuilder();

            // Register upstream dependencies
            builder.RegisterModule<SimplePluginLoaderModule>();

            _MockUserClient = _MockRepository.Create<IAdminEntityClientAsync<User, long>>();
            builder.RegisterInstance(_MockUserClient.Object).As<IAdminEntityClientAsync<User, long>>();

            _MockTokenBuilder = _MockRepository.Create<ITokenBuilder<IUser>>();
            builder.RegisterInstance(_MockTokenBuilder.Object).As<ITokenBuilder<IUser>>();

            _MockAppSettings = _MockRepository.Create<IAppSettings>();
            builder.RegisterInstance(_MockAppSettings.Object).As<IAppSettings>();

            // Register module
            var registrar = new Registrar();
            registrar.Register(builder);
            _Container = builder.Build();
        }

        [TestMethod]
        public void ProductServiceModule_ActivationCredentialCredentialsValidator_Registered()
        {
            using (var scope = _Container.BeginLifetimeScope((b) =>
            {
                b.RegisterType<UserCredentialsValidator>();
            }))
            {
                Assert.IsNotNull(scope.Resolve<UserCredentialsValidator>());
            };
        }
    }
}