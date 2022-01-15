using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Behaviors;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Rhyous.EntityAnywhere.Behaviors.RestEndpoint.Tests
{
    [TestClass]
    public class RestWebServiceHostTests
    {
        private MockRepository _MockRepository;

        private Mock<ILogger> _MockLogger;
        private Mock<ICustomBindingProvider> _MockCustomBindingProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockCustomBindingProvider = _MockRepository.Create<ICustomBindingProvider>();
            _MockLogger = _MockRepository.Create<ILogger>();
        }

        private RestWebServiceHost CreateRestWebServiceHost(Type type)
        {
            return new RestWebServiceHost(type, _MockCustomBindingProvider.Object, _MockLogger.Object);
        }

        [TestMethod]
        public void AddServiceEndpoint_DefaultBinding_Test()
        {
            // Arrange
            _MockCustomBindingProvider.Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((WebHttpBinding)null);

            var host = CreateRestWebServiceHost(typeof(TestService));
            var contractDescription = ContractDescription.GetContract(typeof(ITestService));
            var defaultBinding = new WebHttpBinding();
            var endpoint = new ServiceEndpoint(contractDescription, defaultBinding, new EndpointAddress("https://localhost:27001"));
            endpoint.Name = "TestService";

            // Act
            host.AddServiceEndpoint(endpoint);

            // Assert
            Assert.AreEqual(defaultBinding, endpoint.Binding);
            Assert.AreEqual(RestWebServiceHost.FourMegabytes, (endpoint.Binding as WebHttpBinding).MaxReceivedMessageSize);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void AddServiceEndpoint_CustomBinding_Test()
        {
            // Arrange
            int eightMegabytes = 8388608;
            var binding = new WebHttpBinding { MaxReceivedMessageSize = eightMegabytes };
            _MockCustomBindingProvider.Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(binding);

            _MockLogger.Setup(m => m.Debug("TestService: Provided custom binding found. Default binding will be overwritten.",
                                           "AddServiceEndpoint", 
                                           It.Is<string>(s=>s.EndsWith("RestWebServiceHost.cs")),
                                           It.IsAny<int>()));

            var host = CreateRestWebServiceHost(typeof(TestService));
            var contractDescription = ContractDescription.GetContract(typeof(ITestService));
            var endpoint = new ServiceEndpoint(contractDescription, new WebHttpBinding(), new EndpointAddress("https://localhost:27001"));
            endpoint.Name = "TestService";

            // Act
            host.AddServiceEndpoint(endpoint);

            // Assert
            Assert.AreEqual(binding, endpoint.Binding);
            Assert.AreEqual(eightMegabytes, (endpoint.Binding as WebHttpBinding).MaxReceivedMessageSize);
            _MockRepository.VerifyAll();
        }
    }
}
