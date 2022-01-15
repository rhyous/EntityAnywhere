using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
using TIService = Rhyous.EntityAnywhere.Interfaces.IServiceCommon<Rhyous.EntityAnywhere.PluginLoaders.Tests.EntityInt, Rhyous.EntityAnywhere.PluginLoaders.Tests.IEntityInt, int>;

namespace Rhyous.EntityAnywhere.PluginLoaders.Tests
{
    [TestClass]
    public class EntityServiceLoaderTests
    {
        private MockRepository _MockRepository;

        private Mock<ILifetimeScope> _MockLifetimeScope;
        private Mock<IAppDomain> _MockAppDomain;
        private Mock<IPluginLoaderSettings> _MockPluginLoaderSettings;
        private Mock<IPluginLoaderFactory<TIService>> _MockPluginLoaderFactory;
        private Mock<IPluginObjectCreator<TIService>> _MockPluginObjectCreator;
        private Mock<IPluginPaths> _MockPluginPaths;
        private Mock<IEntityServiceLoaderCommon<TIService, EntityInt, IEntityInt, int>> _MockEntityServiceLoaderCommon;
        private Mock<IPluginLoaderLogger> _MockPluginLoaderLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockLifetimeScope = _MockRepository.Create<ILifetimeScope>();
            _MockAppDomain = _MockRepository.Create<IAppDomain>();
            _MockPluginLoaderSettings = _MockRepository.Create<IPluginLoaderSettings>();
            _MockPluginLoaderFactory = _MockRepository.Create<IPluginLoaderFactory<TIService>>();
            _MockPluginObjectCreator = _MockRepository.Create<IPluginObjectCreator<TIService>>();
            _MockPluginPaths = _MockRepository.Create<IPluginPaths>();
            _MockEntityServiceLoaderCommon = _MockRepository.Create<IEntityServiceLoaderCommon<TIService, EntityInt, IEntityInt, int>>();
            _MockPluginLoaderLogger = _MockRepository.Create<IPluginLoaderLogger>();
        }

        private EntityServiceLoader<TIService, EntityInt, IEntityInt, int>
        CreateEntityServiceLoader()
        {
            return new EntityServiceLoader<TIService, EntityInt, IEntityInt, int>(
                _MockLifetimeScope.Object,
                _MockAppDomain.Object,
                _MockPluginLoaderSettings.Object,
                _MockPluginLoaderFactory.Object,
                _MockPluginObjectCreator.Object,
                _MockPluginPaths.Object,
                _MockEntityServiceLoaderCommon.Object,
                _MockPluginLoaderLogger.Object);
        }
    }
}