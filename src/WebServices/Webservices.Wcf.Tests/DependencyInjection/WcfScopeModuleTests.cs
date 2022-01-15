using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Interfaces.Tools;
using Rhyous.Wrappers;
using System;
using System.Threading.Tasks;
using IAppSettings = Rhyous.EntityAnywhere.Interfaces.IAppSettings;

namespace Rhyous.EntityAnywhere.WebServices.Tests.DependencyInjection
{
    [TestClass]
    public class WcfScopeModuleTests
    {
        private MockRepository _MockRepository;

        private Mock<INamedFactory<IAdminEntityClientAsync>> _MockNamedFactory;
        private Mock<IAppSettings> _MockAppSettings;
        private Mock<IPreventSimultaneousFuncCalls<Task<IUserRoleEntityDataCache>>> _MockPreventSimultaneousFuncCalls;

        private ILifetimeScope _WcfScope;
        private ILifetimeScope _PerCallScope;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);            

            var builder = new ContainerBuilder();

            // Register upstream dependencies
            _MockAppSettings = _MockRepository.Create<IAppSettings>();
            builder.RegisterInstance(_MockAppSettings.Object).As<IAppSettings>();

            var mockFile = _MockRepository.Create<IFileIO>();
            builder.RegisterInstance(mockFile.Object).As<IFileIO>();

            _MockPreventSimultaneousFuncCalls = _MockRepository.Create<IPreventSimultaneousFuncCalls<Task<IUserRoleEntityDataCache>>>();
            builder.RegisterInstance(_MockPreventSimultaneousFuncCalls.Object).As<IPreventSimultaneousFuncCalls<Task<IUserRoleEntityDataCache>>>();

            var mockTokenKeyPair = _MockRepository.Create<ITokenKeyPair>();
            builder.RegisterInstance(mockTokenKeyPair.Object).As<ITokenKeyPair>();
			
            var rootContainer = builder.Build();

            _WcfScope = rootContainer.BeginLifetimeScope("wcfScope", (wcfBuilder) =>
            {
                // Register Wcf scope mocks
                _MockNamedFactory = _MockRepository.Create<INamedFactory<IAdminEntityClientAsync>>();
                wcfBuilder.RegisterInstance(_MockNamedFactory.Object).As<INamedFactory<IAdminEntityClientAsync>>();

                // Register module
                wcfBuilder.RegisterModule<WcfScopeModule>();
            });

            _PerCallScope = _WcfScope.BeginLifetimeScope((perCallBuilder) =>
            {
                // Register per call mocks
            });
        }

        [TestMethod]
        public void WcfScopeModule_IRuntimePluginLoaderFactory_CanBeLoaded()
        {
            Assert.IsNotNull(_WcfScope.Resolve<IRuntimePluginLoaderFactory>());
        }

        [TestMethod]
        public void WcfScopeModule_IRuntimePluginLoaderFactory_NotSingleton()
        {
            Assert.AreNotEqual(_WcfScope.Resolve<IRuntimePluginLoaderFactory>(), _WcfScope.Resolve<IRuntimePluginLoaderFactory>());
        }

        [TestMethod]
        public void WcfScopeModule_AutofacRuntimePluginLoaderFactory_CanBeLoaded()
        {
            Assert.IsNotNull(_WcfScope.Resolve<AutofacRuntimePluginLoaderFactory>());
        }

        [TestMethod]
        public void WcfScopeModule_AutofacRuntimePluginLoaderFactory_NotSingleton()
        {
            Assert.AreNotEqual(_WcfScope.Resolve<AutofacRuntimePluginLoaderFactory>(), _WcfScope.Resolve<AutofacRuntimePluginLoaderFactory>());
        }

        [TestMethod]
        public void WcfScopeModule_RuntimePluginLoaderFactory_CanBeLoaded()
        {
            Assert.IsNotNull(_WcfScope.Resolve<RuntimePluginLoaderFactory>());
        }

        [TestMethod]
        public void WcfScopeModule_RuntimePluginLoaderFactory_Singleton()
        {
            Assert.AreEqual(_WcfScope.Resolve<RuntimePluginLoaderFactory>(), _WcfScope.Resolve<RuntimePluginLoaderFactory>());
        }

        [TestMethod]
        public void WcfScopeModule_IUserRoleEntityDataCacheFactory_CanBeLoaded()
        {
            Assert.IsNotNull(_WcfScope.Resolve<IUserRoleEntityDataCacheFactory>());
        }

        [TestMethod]
        public void WcfScopeModule_IUserRoleEntityDataCacheFactory_Singleton()
        {
            Assert.AreEqual(_WcfScope.Resolve<IUserRoleEntityDataCacheFactory>(), _WcfScope.Resolve<IUserRoleEntityDataCacheFactory>());
        }

        [TestMethod]
        public void WcfScopeModule_IUserRoleEntityDataCache_CanBeLoaded()
        {
            SharedMockToCreateUserRoleEntityDataCache();
            Assert.IsNotNull(_WcfScope.Resolve<IUserRoleEntityDataCache>());
        }

        [TestMethod]
        public void WcfScopeModule_IUserRoleEntityDataCache_Not_Singleton_But_Behaves_As_Singleton()
        {
            SharedMockToCreateUserRoleEntityDataCache();
            Assert.AreEqual(_WcfScope.Resolve<IUserRoleEntityDataCache>(), _WcfScope.Resolve<IUserRoleEntityDataCache>());
        }

        [TestMethod]
        public void WcfScopeModule_IJWTToken_CanBeLoaded()
        {
            Assert.IsNotNull(_WcfScope.Resolve<IJWTToken>());
        }

        [TestMethod]
        public void WcfScopeModule_IJWTToken_Singleton()
        {
            Assert.AreEqual(_WcfScope.Resolve<IJWTToken>(), _WcfScope.Resolve<IJWTToken>());
        }

        [TestMethod]
        public void WcfScopeModule_ITokenDecoder_CanBeLoaded()
        {
            SharedMockToCreateUserRoleEntityDataCache();
            Assert.IsNotNull(_WcfScope.Resolve<ITokenDecoder>());
        }

        [TestMethod]
        public void WcfScopeModule_ITokenDecoder_Singleton()
        {
            SharedMockToCreateUserRoleEntityDataCache();
            Assert.AreEqual(_WcfScope.Resolve<ITokenDecoder>(), _WcfScope.Resolve<ITokenDecoder>());
        }

        [TestMethod]
        public void WcfScopeModule_IAdminClaimsProvider_CanBeLoaded()
        {
            Assert.IsNotNull(_WcfScope.Resolve<IAdminClaimsProvider>());
        }

        [TestMethod]
        public void WcfScopeModule_IAdminClaimsProvider_Singleton()
        {
            Assert.AreEqual(_WcfScope.Resolve<IAdminClaimsProvider>(), _WcfScope.Resolve<IAdminClaimsProvider>());
        }

        private void SharedMockToCreateUserRoleEntityDataCache()
        {
            var factory = _WcfScope.Resolve<IUserRoleEntityDataCacheFactory>();
            IUserRoleEntityDataCache userRoleEntityDataCache = new UserRoleEntityDataCache();
            _MockPreventSimultaneousFuncCalls.Setup(m => m.Call(It.IsAny<Func<Task<IUserRoleEntityDataCache>>>()))
                                             .Returns(Task.FromResult(userRoleEntityDataCache));
        }
    }
}