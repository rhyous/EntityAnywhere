using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.Odata.Expand;
using Rhyous.Odata.Filter;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities.Tests.DependencyInjection
{
    [TestClass]
    public class RelatedEntityRegistrationModuleTests
    {
        private MockRepository _MockRepository;
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

            _MockMetadataService = _MockRepository.Create<IMetadataService>();
            rootBuilder.RegisterInstance(_MockMetadataService.Object).As<IMetadataService>();
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
                b.RegisterModule<RelatedEntityRegistrationModule>();
            });
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_AttributeEvaluator_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<AttributeEvaluator>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_AttributeEvaluator_Registered_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<AttributeEvaluator>(), _PerCallScope.Resolve<AttributeEvaluator>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_ExpandParser_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<ExpandParser>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_ISomeInterface_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<ExpandParser>(), _PerCallScope.Resolve<ExpandParser>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityCollater_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRelatedEntityCollater<A, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityCollater_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IRelatedEntityCollater<A, int>>(), _PerCallScope.Resolve<IRelatedEntityCollater<A, int>>());
        }


        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntitySorterHelper_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRelatedEntitySorterHelper<A, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntitySorterHelper_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IRelatedEntitySorterHelper<A, int>>(), _PerCallScope.Resolve<IRelatedEntitySorterHelper<A, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityExtensions_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRelatedEntityExtensions<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityExtensions_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IRelatedEntityExtensions<A, IA, int>>(), _PerCallScope.Resolve<IRelatedEntityExtensions<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityManyToOne_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRelatedEntityManyToOne<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityManyToOne_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IRelatedEntityManyToOne<A, IA, int>>(), _PerCallScope.Resolve<IRelatedEntityManyToOne<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityOneToMany_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRelatedEntityOneToMany<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityOneToMany_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IRelatedEntityOneToMany<A, IA, int>>(), _PerCallScope.Resolve<IRelatedEntityOneToMany<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityManyToMany_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRelatedEntityManyToMany<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityManyToMany_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IRelatedEntityManyToMany<A, IA, int>>(), _PerCallScope.Resolve<IRelatedEntityManyToMany<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityAccessors_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRelatedEntityAccessors<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityAccessors_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IRelatedEntityAccessors<A, IA, int>>(), _PerCallScope.Resolve<IRelatedEntityAccessors<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityManager_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRelatedEntityManager<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityManager_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IRelatedEntityManager<A, IA, int>>(), _PerCallScope.Resolve<IRelatedEntityManager<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityProvider_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRelatedEntityProvider<A, IA, int>>());
        }

        [TestMethod]
        public void RelatedEntityRegistrationModule_IRelatedEntityProvider_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IRelatedEntityProvider<A, IA, int>>(), _PerCallScope.Resolve<IRelatedEntityProvider<A, IA, int>>());
        }
    }
}