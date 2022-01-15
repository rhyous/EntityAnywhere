using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.Odata.Expand;
using Rhyous.Odata.Filter;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities.Tests.DependencyInjection
{
    [TestClass]
    public class FilterConverterRegistrationModuleTests
    {
        private MockRepository _MockRepository;
        private Mock<IMetadataServiceFactory> _MockMetadataServiceFactory;
        private Mock<IMetadataService> _MockMetadataService;
        private Mock<IEntityList> _MockEntityList;
        private Mock<IExtensionEntityList> _MockExtensionEntityList;


        private IContainer _Container;
        private ILifetimeScope _WcfLifetimeScope;
        private ILifetimeScope _PerCallScope;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);


            var rootBuilder = new ContainerBuilder();
            // Register mocks of root registrations
            _MockEntityList = _MockRepository.Create<IEntityList>();
            rootBuilder.RegisterInstance(_MockEntityList.Object).As<IEntityList>();
            var entityList = new List<Type> { typeof(A), typeof(B) };
            _MockEntityList.Setup(m=>m.Entities).Returns(entityList);

            _MockExtensionEntityList = _MockRepository.Create<IExtensionEntityList>();
            rootBuilder.RegisterInstance(_MockExtensionEntityList.Object).As<IExtensionEntityList>();
            var extensionEntityList = new List<Type> { typeof(ExtensionEntity1) };
            _MockExtensionEntityList.Setup(m => m.Entities).Returns(extensionEntityList);

            _MockMetadataServiceFactory = _MockRepository.Create<IMetadataServiceFactory>();
            rootBuilder.RegisterInstance(_MockMetadataServiceFactory.Object).As<IMetadataServiceFactory>();
            _MockMetadataService = _MockRepository.Create<IMetadataService>();
            _MockMetadataServiceFactory.Setup(m=>m.MetadataService).Returns(_MockMetadataService.Object);
            var csdlSchema = new CsdlSchema();
            _MockMetadataService.Setup(m => m.GetCsdlSchema(entityList)).Returns(csdlSchema);


            _Container = rootBuilder.Build();


            _WcfLifetimeScope = _Container.BeginLifetimeScope(b =>
            {
                b.RegisterGeneric(typeof(NamedFactory<>)).As(typeof(INamedFactory<>));
            });

            // Register the module
            _PerCallScope = _WcfLifetimeScope.BeginLifetimeScope(b =>
            {
                b.RegisterModule<FilterConverterRegistrationModule>();
            });
        }

        [TestMethod]
        public void FilterConverterRegistrationModule_ICustomFilterConvertersRunner_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<ICustomFilterConvertersRunner<A>>());
        }

        [TestMethod]
        public void FilterConverterRegistrationModule_ICustomFilterConvertersRunner_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<ICustomFilterConvertersRunner<A>>(), _PerCallScope.Resolve<ICustomFilterConvertersRunner<A>>());
        }

        [TestMethod]
        public void FilterConverterRegistrationModule_ICustomFilterConverterCollectionIRelatedEntityExtensions_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<ICustomFilterConverterCollection<A>>());
        }

        [TestMethod]
        public void FilterConverterRegistrationModule_ICustomFilterConverterCollection_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<ICustomFilterConverterCollection<A>>(), _PerCallScope.Resolve<ICustomFilterConverterCollection<A>>());
        }
    }
}