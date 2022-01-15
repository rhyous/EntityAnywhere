using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Behaviors.DependencyInjection.Tests
{
    [TestClass]
    public class BehaviorsHeaderValidationModuleTests
    {
        private MockRepository _MockRepository;
        private Mock<IAppSettings> _MockAppSettings;

        private IContainer _Container;


        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockAppSettings = _MockRepository.Create<IAppSettings>();

            var builder = new ContainerBuilder();

            // Register upstream dependencies
            builder.RegisterModule<SimplePluginLoaderModule>();

            builder.RegisterInstance(_MockAppSettings.Object).As<IAppSettings>();

            var mockLogger = _MockRepository.Create<ILogger>();
            builder.RegisterInstance(mockLogger.Object).As<ILogger>();

            // Register module
            var registrar = new Registrar();
            registrar.Register(builder);
            _Container = builder.Build();
        }

        [TestMethod]
        public void EntitledProductServiceModule_IAccessController_Registered()
        {
            Assert.IsNotNull(_Container.Resolve<IAccessController>());
        }

        [TestMethod]
        public void EntitledProductServiceModule_IAnonymousAllowedUrls_Registered()
        {
            Assert.IsNotNull(_Container.Resolve<IAnonymousAllowedUrls>());
        }

        [TestMethod]
        public void EntitledProductServiceModule_IPluginHeaderValidator_Registered()
        {
            Assert.IsNotNull(_Container.Resolve<IPluginHeaderValidator>());
        }

        [TestMethod]
        public void EntitledProductServiceModule_IPluginHeaderValidator_InstancePerDependency()
        {
            var a = _Container.Resolve<IPluginHeaderValidator>();
            var b = _Container.Resolve<IPluginHeaderValidator>();
            Assert.AreNotEqual(a, b);
        }

        [TestMethod]
        public void EntitledProductServiceModule_IHeaderValidationInspector_Registered()
        {
            Assert.IsNotNull(_Container.Resolve<IHeaderValidationInspector>());
        }

        [TestMethod]
        public void EntitledProductServiceModule_IHeaderValidationInspector_InstancePerDependency()
        {
            var a = _Container.Resolve<IHeaderValidationInspector>();
            var b = _Container.Resolve<IHeaderValidationInspector>();
            Assert.AreNotEqual(a, b);
        }
    }
}
