using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.PluginLoaders.Tests
{
    [TestClass]
    public class EntityEventLoaderTests
    {
        private MockRepository _MockRepository;

        private Mock<IAppDomain> _MockAppDomain;
        private Mock<IPluginLoaderSettings> _MockPluginLoaderSettings;
        private Mock<IPluginLoaderFactory<IEntityEvent<EntityInt, int>>> _MockPluginLoaderFactory;
        private Mock<IPluginObjectCreator<IEntityEvent<EntityInt, int>>> _MockPluginObjectCreator;
        private Mock<IPluginPaths> _MockPluginPaths;
        private Mock<IPluginLoaderLogger> _MockPluginLoaderLogger;
        private Mock<IEntityEventLoaderCommon<EntityInt, int>> _MockEntityEventLoaderCommon;
        private Mock<ILogger> _MockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAppDomain = _MockRepository.Create<IAppDomain>();
            _MockPluginLoaderSettings = _MockRepository.Create<IPluginLoaderSettings>();
            _MockPluginLoaderFactory = _MockRepository.Create<IPluginLoaderFactory<IEntityEvent<EntityInt, int>>>();
            _MockPluginObjectCreator = _MockRepository.Create<IPluginObjectCreator<IEntityEvent<EntityInt, int>>>();
            _MockPluginPaths = _MockRepository.Create<IPluginPaths>();
            _MockPluginLoaderLogger = _MockRepository.Create<IPluginLoaderLogger>();
            _MockEntityEventLoaderCommon = _MockRepository.Create<IEntityEventLoaderCommon<EntityInt, int>>();
            _MockLogger = _MockRepository.Create<ILogger>();
        }

        private EntityEventLoader<EntityInt, int> CreateEntityEventLoader()
        {
            return new EntityEventLoader<EntityInt, int>(
                _MockAppDomain.Object,
                _MockPluginLoaderSettings.Object,
                _MockPluginLoaderFactory.Object,
                _MockPluginObjectCreator.Object,
                _MockPluginPaths.Object,
                _MockPluginLoaderLogger.Object,
                _MockEntityEventLoaderCommon.Object,
                _MockLogger.Object);
        }

        [TestMethod]
        public void EntityEventLoader_Constructor_Test()
        {
            _MockPluginPaths.Setup(m=>m.Paths).Returns(new List<string> { @"c:\path1", @"c:\path2"});
            Assert.IsNotNull(CreateEntityEventLoader());
        }

        [TestMethod]
        public void EntityEventLoader_LoadPlugins_ReturnsPlugin()
        {
            // Arrange
            var paths = new List<string> { @"c:\path1", @"c:\path2" };
            _MockPluginPaths.Setup(m => m.Paths).Returns(paths);
            var loader = CreateEntityEventLoader();
            var mockPluginLoader = _MockRepository.Create<IPluginLoader<IEntityEvent<EntityInt, int>>>();
            _MockPluginLoaderFactory.Setup(m => m.Create(It.IsAny<IPluginPaths>()))
                                    .Returns(mockPluginLoader.Object);
            var mockEntityEvent = _MockRepository.Create<IEntityEvent<EntityInt, int>>();
            var mockPlugin = _MockRepository.Create<IPlugin<IEntityEvent<EntityInt, int>>>();
            var pluginsCollection = new PluginCollection<IEntityEvent<EntityInt, int>>(new[] { mockPlugin.Object });
            mockPluginLoader.Setup(m => m.LoadPlugins()).Returns(pluginsCollection);
            var pluginList = new List<IEntityEvent<EntityInt, int>> { mockEntityEvent.Object };
            mockPlugin.Setup(m => m.CreatePluginObjects(It.IsAny<IPluginObjectCreator<IEntityEvent<EntityInt, int>>>()))
                      .Returns(pluginList);

            // Act
            var plugins = loader.LoadPlugins();

            // Assert
            Assert.IsNotNull(plugins);
        }

        [TestMethod]
        public void EntityEventLoader_LoadPlugins_ReturnsCommonPlugin()
        {
            // Arrange
            var paths = new List<string> { @"c:\path1", @"c:\path2" };
            _MockPluginPaths.Setup(m => m.Paths).Returns(paths);
            var loader = CreateEntityEventLoader();
            var mockPluginLoader = _MockRepository.Create<IPluginLoader<IEntityEvent<EntityInt, int>>>();
            _MockPluginLoaderFactory.Setup(m => m.Create(It.IsAny<IPluginPaths>()))
                                    .Returns(mockPluginLoader.Object);
            var mockEntityEvent = _MockRepository.Create<IEntityEvent<EntityInt, int>>();
            var mockPlugin = _MockRepository.Create<IPlugin<IEntityEvent<EntityInt, int>>>();
            PluginCollection<IEntityEvent<EntityInt, int>> pluginsCollection = null;
            mockPluginLoader.Setup(m => m.LoadPlugins()).Returns(pluginsCollection);
            _MockPluginLoaderSettings.Setup(m=>m.ThrowExceptionIfNoPluginFound).Returns(false);

            _MockPluginLoaderLogger.Setup(m=> m.WriteLine(PluginLoaderLogLevel.Debug, It.IsAny<string>()));
            var mockEntityEventAll = _MockRepository.Create<IEntityEventAll<EntityInt, int>>();

            _MockEntityEventLoaderCommon.Setup(m => m.LoadPlugins()).Returns(mockEntityEventAll.Object);

            // Act
            var plugins = loader.LoadPlugins();

            // Assert
            Assert.IsNotNull(plugins);
        }
    }
}