using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Behaviors;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Rhyous.EntityAnywhere.Behaviors.RestEndpoint.Tests
{
    [TestClass]
    public class RestWebServiceHostFactoryTests
    {
        private MockRepository _MockRepository;

        private Mock<IRuntimePluginLoader<IServiceBehavior>> _MockRuntimePluginLoader;
        private Mock<ILogger> _MockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRuntimePluginLoader = _MockRepository.Create<IRuntimePluginLoader<IServiceBehavior>>();
            _MockLogger = _MockRepository.Create<ILogger>();
        }

        private RestWebServiceHostFactory CreateFactory()
        {
            return new RestWebServiceHostFactory(_MockRuntimePluginLoader.Object,
                                                 _MockLogger.Object);
        }

        [TestMethod]
        public void CreateServiceHostTest()
        {
            // Arrange
            _MockLogger.Setup(m => m.Write(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var host = CreateFactory();
            Uri uri = new Uri("https://localhost:27001");
            Uri[] uris = new Uri[1];
            uris[0] = uri;
            _MockRuntimePluginLoader.Setup(m => m.CreatePluginObjects(It.IsAny<IPluginObjectCreator<IServiceBehavior>>()))
                                    .Returns((List<IServiceBehavior>)null);

            // Act
            ServiceHost internalHost = host.CreateServiceHostInternal(typeof(TestService), uris);

            // Assert
            Assert.IsNotNull(internalHost);
            Assert.IsInstanceOfType(internalHost, typeof(ServiceHost));
        }
    }
}
