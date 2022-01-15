using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Expand;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Interfaces.Tools;
using Rhyous.EntityAnywhere.WebServices;
using Rhyous.EntityAnywhere.WebServices.Common.Tests;
using Rhyous.Wrappers;
using System.ServiceModel.Description;
using System.Web.Routing;
using IAppSettings = Rhyous.EntityAnywhere.Interfaces.IAppSettings;

namespace Rhyous.EntityAnywhere.Webservices.Tests.DependencyInjection
{
    [TestClass]
    public class RootModuleTests
    {
        private MockRepository _MockRepository;

        private IContainer _Container;

        private Mock<INamedFactory<IAdminEntityClientAsync>> _MockNamedFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            var builder = new ContainerBuilder();

            // Register upstream dependencies
            var mockHeaders = _MockRepository.Create<IHeaders>();
            builder.RegisterInstance(mockHeaders.Object).As<IHeaders>();

            _MockNamedFactory = _MockRepository.Create<INamedFactory<IAdminEntityClientAsync>>();
            builder.RegisterInstance(_MockNamedFactory.Object).As<INamedFactory<IAdminEntityClientAsync>>();

            // Register module
            builder.RegisterModule<RootModule>();

            _Container = builder.Build();
        }

        [TestMethod]
        public void RootModule_ICustomPluralizer_CanResolve()
        {
            Assert.IsNotNull(_Container.Resolve<ICustomPluralizer>());
        }

        [TestMethod]
        public void RootModule_ICustomPluralizer_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<ICustomPluralizer>(), _Container.Resolve<ICustomPluralizer>());
        }

        [TestMethod]
        public void RootModule_ILogger_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<ILogger>());
        }

        [TestMethod]
        public void RootModule_ILogger_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<ILogger>(), _Container.Resolve<ILogger>());
        }

        [TestMethod]
        public void RootModule_IFileIO_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<IFileIO>());
        }

        [TestMethod]
        public void RootModule_IFileIO_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IFileIO>(), _Container.Resolve<IFileIO>());
        }

        [TestMethod]
        public void RootModule_IPreventSimultaneousFuncCalls_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<IPreventSimultaneousFuncCalls<int>>());
        }

        [TestMethod]
        public void RootModule_IPreventSimultaneousFuncCalls_Not_Singleton()
        {
            Assert.AreNotEqual(_Container.Resolve<IPreventSimultaneousFuncCalls<int>>(), _Container.Resolve<IPreventSimultaneousFuncCalls<int>>());
        }

        [TestMethod]
        public void RootModule_ITokenKeyPair_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<ITokenKeyPair>());
        }

        [TestMethod]
        public void RootModule_ITokenKeyPair_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<ITokenKeyPair>(), _Container.Resolve<ITokenKeyPair>());
        }

        [TestMethod]
        public void RootModule_IAppSettings_CanBeLoaded()
        {
            Assert.IsNotNull(_Container.Resolve<IAppSettings>());
        }

        [TestMethod]
        public void RootModule_IAppSettings_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IAppSettings>(), _Container.Resolve<IAppSettings>());
        }

        [TestMethod]
        public void RootModule_IPluginPaths_CanResolve()
        {
            Assert.IsNotNull(_Container.Resolve<IPluginPaths>());
        }

        [TestMethod]
        public void RootModule_IPluginPaths_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IPluginPaths>(), _Container.Resolve<IPluginPaths>());
        }

        [TestMethod]
        public void RootModule_IPluginLoaderLogger_CanResolve()
        {
            Assert.IsNotNull(_Container.Resolve<IPluginLoaderLogger>());
        }

        [TestMethod]
        public void RootModule_IPluginLoaderLogger_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IPluginLoaderLogger>(), _Container.Resolve<IPluginLoaderLogger>());
        }

        [TestMethod]
        public void RootModule_IEntityInfo_CanResolve()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityInfo<EntityInt>>());
        }

        [TestMethod]
        public void RootModule_IEntityInfo_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IEntityInfo<EntityInt>>(), _Container.Resolve<IEntityInfo<EntityInt>>());
        }

        [TestMethod]
        public void RootModule_IEntityInfoAltKey_CanResolve()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityInfoAltKey<EntityInt, string>>());
        }

        [TestMethod]
        public void RootModule_IEntityInfoAltkey_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IEntityInfoAltKey<EntityInt, string>>(), _Container.Resolve<IEntityInfoAltKey<EntityInt, string>>());
        }

        [TestMethod]
        public void RootModule_IPropertyOrderSorter_CanResolve()
        {
            Assert.IsNotNull(_Container.Resolve<IPropertyOrderSorter>());
        }

        [TestMethod]
        public void RootModule_IPropertyOrderSorter_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IPropertyOrderSorter>(), _Container.Resolve<IPropertyOrderSorter>());
        }

        [TestMethod]
        public void RootModule_IPreferentialPropertyComparer_CanResolve()
        {
            Assert.IsNotNull(_Container.Resolve<IPreferentialPropertyComparer>());
        }

        [TestMethod]
        public void RootModule_IPreferentialPropertyComparer_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IPreferentialPropertyComparer>(), _Container.Resolve<IPreferentialPropertyComparer>());
        }

        [TestMethod]
        public void RootModule_RouteCollection_CanResolve()
        {
            Assert.IsNotNull(_Container.Resolve<RouteCollection>());
        }

        [TestMethod]
        public void RootModule_RouteCollection_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<RouteCollection>(), _Container.Resolve<RouteCollection>());
        }

        [TestMethod]
        public void RootModule_IRuntimePluginLoader_IServiceBehavior_CanResolve()
        {
            Assert.IsNotNull(_Container.Resolve<IRuntimePluginLoader<IServiceBehavior>>());
        }

        [TestMethod]
        public void RootModule_IRuntimePluginLoader_IServiceBehavior_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IRuntimePluginLoader<IServiceBehavior>>(), _Container.Resolve<IRuntimePluginLoader<IServiceBehavior>>());
        }
        [TestMethod]
        public void RootModule_IEntityList_CanResolve()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityList>());
        }

        [TestMethod]
        public void RootModule_IEntityList_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IEntityList>(), _Container.Resolve<IEntityList>());
        }

        [TestMethod]
        public void RootModule_IExtensionEntityList_CanResolve()
        {
            Assert.IsNotNull(_Container.Resolve<IExtensionEntityList>());
        }

        [TestMethod]
        public void RootModule_IExtensionEntityList_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IExtensionEntityList>(), _Container.Resolve<IExtensionEntityList>());
        }

        [TestMethod]
        public void RootModule_IMappingEntityList_CanResolve()
        {
            Assert.IsNotNull(_Container.Resolve<IMappingEntityList>());
        }

        [TestMethod]
        public void RootModule_IMappingEntityList_Singleton()
        {
            Assert.AreEqual(_Container.Resolve<IMappingEntityList>(), _Container.Resolve<IMappingEntityList>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IInputValidator_Registered()
        {
            Assert.IsNotNull(_Container.Resolve<IInputValidator<EntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_AttributeEvaluator_Registered()
        {
            Assert.IsNotNull(_Container.Resolve<AttributeEvaluator>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IIdDisambiguator_Registered()
        {
            Assert.IsNotNull(_Container.Resolve<IIdDisambiguator<EntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IIdDisambiguator_TAltKey_Registered()
        {
            Assert.IsNotNull(_Container.Resolve<IIdDisambiguator<EntityInt, int, string>>());
        }
    }
}
