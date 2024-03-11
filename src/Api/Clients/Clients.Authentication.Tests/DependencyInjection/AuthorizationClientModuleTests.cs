using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Rhyous.EntityAnywhere.Clients2.DependencyInjection
{
    [TestClass]
    public class AuthorizationClientModuleTests
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

            var mockHttpClientRunner = _MockRepository.Create<IHttpClientRunner>();
            builder.RegisterInstance(mockHttpClientRunner.Object).As<IHttpClientRunner>();

            // Register plugin Registrar
            var registrar = new AuthorizationClientRegistrar();
            registrar.Register(builder);
            _Container = builder.Build();

        }

        #region AuthorizationClientModule Registration

        [TestMethod]
        public void AuthorizationClientModule_Resolve_IAuthorizationClientFactory_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAuthorizationClientFactory>());
        }

        [TestMethod]
        public void AuthorizationClientModule_Resolve_IAuthorizationClient_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAuthorizationClient>());
        }
        #endregion
    }
}