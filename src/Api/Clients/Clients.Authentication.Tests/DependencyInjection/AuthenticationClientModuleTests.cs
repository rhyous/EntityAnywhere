using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Rhyous.EntityAnywhere.Clients2.DependencyInjection
{
    [TestClass]
    public class AuthenticationClientModuleTests
    {
        private MockRepository _MockRepository;

        private IContainer _Container;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            var builder = new ContainerBuilder();

            // Register upstream objects
            var mockEntityClientConfig = _MockRepository.Create<IEntityClientConfig>();
            builder.RegisterInstance(mockEntityClientConfig.Object).As<IEntityClientConfig>();

            var mockHttpClientRunner = _MockRepository.Create<IHttpClientRunnerNoHeaders>();
            builder.RegisterInstance(mockHttpClientRunner.Object).As<IHttpClientRunnerNoHeaders>();

            // Register plugin Registrar
            var registrar = new AuthenticationClientRegistrar();
            registrar.Register(builder);
            _Container = builder.Build();

        }

        #region AuthenticationClientModule Registration
        [TestMethod]
        public void AuthenticationClientModule_Resolve_IAuthenticationSettings_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAuthenticationSettings>());
        }

        [TestMethod]
        public void AuthenticationClientModule_Resolve_IAuthenticationClientFactory_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAuthenticationClientFactory>());
        }

        [TestMethod]
        public void AuthenticationClientModule_Resolve_IAuthenticationClient_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAuthenticationClient>());
        }
        #endregion
    }
}