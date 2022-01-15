using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using Rhyous.EntityAnywhere.WebServices;
using System;
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace Rhyous.EntityAnywhere.WebServices.Tests.Business
{
    [TestClass]
    public class RestEndpointBuilderTests
    {
        private MockRepository _MockRepository;

        private Mock<AttributeToServiceDictionary> _MockAttributeToServiceDictionary;
        private Mock<IWebServiceRegistrar> _MockWebServiceRegistrar;
        private Mock<IRuntimePluginLoader<IServiceBehavior>> _MockRuntimePluginLoader;
        private Mock<IWebServiceLoaderFactory> _MockWebServiceLoaderFactory;
        private Mock<ILoadedEntitiesTracker> _MockLoadedEntitiesTracker;
        private Mock<ILogger> _MockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAttributeToServiceDictionary = _MockRepository.Create<AttributeToServiceDictionary>();
            _MockWebServiceRegistrar = _MockRepository.Create<IWebServiceRegistrar>();
            _MockRuntimePluginLoader = _MockRepository.Create<IRuntimePluginLoader<IServiceBehavior>>();
            _MockWebServiceLoaderFactory = _MockRepository.Create<IWebServiceLoaderFactory>();
            _MockLoadedEntitiesTracker = _MockRepository.Create<ILoadedEntitiesTracker>();
            _MockLogger = _MockRepository.Create<ILogger>();
        }

        private RestEndpointBuilder CreateRestEndpointBuilder()
        {
            return new RestEndpointBuilder(
                _MockAttributeToServiceDictionary.Object,
                _MockWebServiceRegistrar.Object,
                _MockRuntimePluginLoader.Object,
                _MockWebServiceLoaderFactory.Object,
                _MockLoadedEntitiesTracker.Object,
                _MockLogger.Object);
        }

        [TestMethod]
        public void RestEndpointBuilder_BuildWebServiceType_ExtensionEntity_TIdLongNotGeneric_Succeeds()
        {
            // Arrange
            var restEndpointBuilder = CreateRestEndpointBuilder();
            var typesModel = new WebServiceTypesModel
            {
                EntityType = typeof(Addendum),
                InterfaceType = typeof(IAddendum),
                IdType = typeof(long),
                WebServiceGenericType = typeof(ExtensionEntityWebService<,>),
                LoaderType = typeof(IEntityWebServiceLoader<,>),
                AdditionalWebServiceTypes = null,
            };
            var idTypeAttribute = new IdTypeAttribute { IsGenericForWebService = false };
            var loaderType = typeof(IEntityWebServiceLoader<Addendum, ExtensionEntityWebService<Addendum, IAddendum>>);
            var mockLoader = _MockRepository.Create<IEntityWebServiceLoader<Addendum, ExtensionEntityWebService<Addendum, IAddendum>>>();
            _MockWebServiceLoaderFactory.Setup(m => m.Create(loaderType))
                                        .Returns(mockLoader.Object);
            var expectedType = typeof(ExtensionEntityWebService<Addendum, IAddendum>);
            var pluginTypes = new List<Type> { expectedType };
            mockLoader.Setup(m => m.PluginTypes)
                      .Returns(pluginTypes);

            // Act
            var type = restEndpointBuilder.BuildWebServiceType(typesModel, idTypeAttribute);

            // Assert
            Assert.AreEqual(expectedType, type);
            _MockRepository.VerifyAll();
        }
    }
}