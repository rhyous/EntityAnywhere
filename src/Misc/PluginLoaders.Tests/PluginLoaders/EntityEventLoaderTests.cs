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
        private Mock<IPluginLoaderFactory<IEntityEvent<EntityInt>>> _MockPluginLoaderFactory;
        private Mock<IPluginObjectCreator<IEntityEvent<EntityInt>>> _MockPluginObjectCreator;
        private Mock<IPluginPaths> _MockPluginPaths;
        private Mock<IPluginLoaderLogger> _MockPluginLoaderLogger;
        private Mock<ILogger> _MockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAppDomain = _MockRepository.Create<IAppDomain>();
            _MockPluginLoaderSettings = _MockRepository.Create<IPluginLoaderSettings>();
            _MockPluginLoaderFactory = _MockRepository.Create<IPluginLoaderFactory<IEntityEvent<EntityInt>>>();
            _MockPluginObjectCreator = _MockRepository.Create<IPluginObjectCreator<IEntityEvent<EntityInt>>>();
            _MockPluginPaths = _MockRepository.Create<IPluginPaths>();
            _MockPluginLoaderLogger = _MockRepository.Create<IPluginLoaderLogger>();
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
                _MockLogger.Object);
        }

        [TestMethod]
        public void EntityEventLoader_Constructor_Test()
        {
            _MockPluginPaths.Setup(m=>m.Paths).Returns(new List<string> { @"c:\path1", @"c:\path2"});
            Assert.IsNotNull(CreateEntityEventLoader());
        }
    }
}