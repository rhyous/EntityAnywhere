using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Filter;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.DependencyInjection;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.DependencyInjection
{
    [TestClass]
    public class ServicesCommonModuleTests
    {
        private MockRepository _MockRepository;

        private Mock<ILogger> _MockLogger;
        private IContainer _Container;
        private ILifetimeScope _WcfScope;
        private ILifetimeScope _PerCallScope;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockLogger = _MockRepository.Create<ILogger>();

            var builder = new ContainerBuilder();
            // Register mocks of root dependencies
            builder.RegisterInstance(_MockLogger.Object).As<ILogger>();

            builder.RegisterGeneric(typeof(EntityInfo<>)).As(typeof(IEntityInfo<>))
                   .SingleInstance();

            builder.RegisterGeneric(typeof(EntityInfoAltKey<,>)).As(typeof(IEntityInfoAltKey<,>))
                   .SingleInstance();

            _Container = builder.Build();

            _WcfScope = _Container.BeginLifetimeScope(b =>
            {
                // Register mocks of upstream dependencies
                var mockEntitySettingsCache = _MockRepository.Create<IEntitySettingsCache>();
                b.RegisterInstance(mockEntitySettingsCache.Object).As<IEntitySettingsCache>();

                var mockRepoEntity = _MockRepository.Create<IRepository<Entity, IEntity, int>>();
                b.RegisterInstance(mockRepoEntity.Object).As<IRepository<Entity, IEntity, int>>();

                var mockRepoEntityInt = _MockRepository.Create<IRepository<EntityInt, IEntityInt, int>>();
                b.RegisterInstance(mockRepoEntityInt.Object).As<IRepository<EntityInt, IEntityInt, int>>();

                var mockRepoAltIdEntityt = _MockRepository.Create<IRepository<AltKeyEntity, IAltKeyEntity, int>>();
                b.RegisterInstance(mockRepoAltIdEntityt.Object).As<IRepository<AltKeyEntity, IAltKeyEntity, int>>();

                var mockEntityClient = _MockRepository.Create<IAdminEntityClientAsync<Entity, int>>();
                b.RegisterInstance(mockEntityClient.Object).As<IAdminEntityClientAsync<Entity, int>>();

                var mockEntityPropertyClient = _MockRepository.Create<IAdminEntityClientAsync<EntityProperty, int>>();
                b.RegisterInstance(mockEntityPropertyClient.Object).As<IAdminEntityClientAsync<EntityProperty, int>>();

                b.RegisterGeneric(typeof(FilterExpressionParser<>)).As(typeof(IFilterExpressionParser<>))
                       .SingleInstance();
                b.RegisterGeneric(typeof(CustomFilterConvertersRunner<>)).As(typeof(ICustomFilterConvertersRunner<>))
                       .SingleInstance();
                b.RegisterGeneric(typeof(MyTestCustomFilterConverterCollection<>)).As(typeof(ICustomFilterConverterCollection<>))
                       .SingleInstance();
                b.RegisterGeneric(typeof(FilterExpressionParserActionDictionary<>)).As(typeof(IFilterExpressionParserActionDictionary<>))
                       .SingleInstance();

                b.RegisterGeneric(typeof(AlternateKeyTracker<,>));

                b.RegisterGeneric(typeof(ServiceCommon<,,>)).As(typeof(IServiceCommon<,,>));
                b.RegisterGeneric(typeof(ServiceCommonAlternateKey<,,,>)).As(typeof(IServiceCommonAlternateKey<,,,>));

            });

            _PerCallScope = _WcfScope.BeginLifetimeScope(b =>
            {
                // Register upstream per call registrations
                b.RegisterType<UrlParameters>().As<IUrlParameters>().SingleInstance();
                
                // Register this modules dependencies
                var registrar = new Registrar();
                registrar.Register(b);
            });
        }

        [TestMethod]
        public void ServicesCommonModule_IAddAltKeyHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IAddAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IAddAltKeyHandler_IsRegistered_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IAddAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>(),
                            _PerCallScope.Resolve<IAddAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IAddHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IAddHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IDeleteHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IDeleteHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IAddAltKeyHandler_IDeleteHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IDeleteHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _PerCallScope.Resolve<IDeleteHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGenerateRepositoryHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGenerateRepositoryHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGenerateRepositoryHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IGenerateRepositoryHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _PerCallScope.Resolve<IGenerateRepositoryHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByAlternateKeyHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByAlternateKeyHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IGetByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>(),
                            _PerCallScope.Resolve<IGetByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByIdHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetByIdHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByIdHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IGetByIdHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _PerCallScope.Resolve<IGetByIdHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByIdsHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetByIdsHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByIdsHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IGetByIdsHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _PerCallScope.Resolve<IGetByIdsHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetDistinctPropertyValueHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetDistinctPropertyValuesHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetDistinctPropertyValueHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IGetDistinctPropertyValuesHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _PerCallScope.Resolve<IGetDistinctPropertyValuesHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByPropertyValuesHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetByPropertyValuesHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByPropertyValuesHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IGetByIdsHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _PerCallScope.Resolve<IGetByIdsHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetPropertyValueHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetPropertyValueHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetPropertyValueHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IGetPropertyValueHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _PerCallScope.Resolve<IGetPropertyValueHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IInsertSeedDataHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IInsertSeedDataHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IInsertSeedDataHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IInsertSeedDataHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _PerCallScope.Resolve<IInsertSeedDataHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IQueryableHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IQueryableHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IQueryableHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IQueryableHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _PerCallScope.Resolve<IQueryableHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_ISearchByAlternateKeyHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<ISearchByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_ISearchByAlternateKeyHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<ISearchByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>(),
                            _PerCallScope.Resolve<ISearchByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IUpdateAltKeyHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IUpdateAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IUpdateAltKeyHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IUpdateAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>(),
                            _PerCallScope.Resolve<IUpdateAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IUpdateHandler_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IUpdateHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IUpdateHandler_SingleInstance()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IUpdateHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _PerCallScope.Resolve<IUpdateHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IServicesCommon_IsRegistered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IServiceCommon<Entity, IEntity, int>>());
        }
    }
}