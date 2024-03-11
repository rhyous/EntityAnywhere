using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.HeaderValidators.DependencyInjection.Tests
{
    [TestClass]
    public class HeaderValidatorsCommonModuleTests
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
        public void HeaderValidatorsCommonModule_ITokenSecurityList_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<ITokenSecurityList>());
        }

        [TestMethod]
        public void HeaderValidatorsCommonModule_ITokenSecurityList_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<ITokenSecurityList>(), _Container.Resolve<ITokenSecurityList>());
        }

        [TestMethod]
        public void HeaderValidatorsCommonModule_IPathNormalizer_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<IPathNormalizer>());
        }

        [TestMethod]
        public void HeaderValidatorsCommonModule_IPathNormalizer_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IPathNormalizer>(), _Container.Resolve<IPathNormalizer>());
        }

        [TestMethod]
        public void HeaderValidatorsCommonModule_IEntityNameProvider_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityNameProvider>());
        }

        [TestMethod]
        public void HeaderValidatorsCommonModule_IEntityNameProvider_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IEntityNameProvider>(), _Container.Resolve<IEntityNameProvider>());
        }

        [TestMethod]
        public void HeaderValidatorsCommonModule_IEntityPermissionChecker_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityPermissionChecker>());
        }

        [TestMethod]
        public void HeaderValidatorsCommonModule_IEntityPermissionChecker_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IEntityPermissionChecker>(), _Container.Resolve<IEntityPermissionChecker>());
        }
    }
}