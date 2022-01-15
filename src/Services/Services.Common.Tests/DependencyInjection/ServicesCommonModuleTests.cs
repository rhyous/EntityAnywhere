using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
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

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockLogger = _MockRepository.Create<ILogger>();

            var builder = new ContainerBuilder();

            // Register mocks of upstream dependencies
            var mockRepoEntity = _MockRepository.Create<IRepository<Entity, IEntity, int>>();
            builder.RegisterInstance(mockRepoEntity.Object).As<IRepository<Entity, IEntity, int>>();

            var mockRepoEntityInt = _MockRepository.Create<IRepository<EntityInt, IEntityInt, int>>();
            builder.RegisterInstance(mockRepoEntityInt.Object).As<IRepository<EntityInt, IEntityInt, int>>();

            var mockRepoAltIdEntityt = _MockRepository.Create<IRepository<AltKeyEntity, IAltKeyEntity, int>>();
            builder.RegisterInstance(mockRepoAltIdEntityt.Object).As<IRepository<AltKeyEntity, IAltKeyEntity, int>>();

            var mockEntityClient = _MockRepository.Create<IAdminEntityClientAsync<Entity, int>>();
            builder.RegisterInstance(mockEntityClient.Object).As<IAdminEntityClientAsync<Entity, int>>();

            var mockEntityPropertyClient = _MockRepository.Create<IAdminEntityClientAsync<EntityProperty, int>>();
            builder.RegisterInstance(mockEntityPropertyClient.Object).As<IAdminEntityClientAsync<EntityProperty, int>>();

            builder.RegisterGeneric(typeof(FilterExpressionParser<>)).As(typeof(IFilterExpressionParser<>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(CustomFilterConvertersRunner<>)).As(typeof(ICustomFilterConvertersRunner<>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(MyTestCustomFilterConverterCollection<>)).As(typeof(ICustomFilterConverterCollection<>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(FilterExpressionParserActionDictionary<>)).As(typeof(IFilterExpressionParserActionDictionary<>))
                   .SingleInstance();

            builder.RegisterGeneric(typeof(EntityInfo<>)).As(typeof(IEntityInfo<>))
                   .SingleInstance();

            builder.RegisterGeneric(typeof(EntityInfoAltKey<,>)).As(typeof(IEntityInfoAltKey<,>))
                   .SingleInstance();

            builder.RegisterGeneric(typeof(ServiceCommon<,,>)).As(typeof(IServiceCommon<,,>));
            builder.RegisterGeneric(typeof(ServiceCommonAlternateKey<,,,>)).As(typeof(IServiceCommonAlternateKey<,,,>));

            // Register this modules dependencies
            var registrar = new Registrar();
            registrar.Register(builder);
            builder.RegisterInstance(_MockLogger.Object).As<ILogger>();
            _Container = builder.Build();
        }

        [TestMethod]
        public void ServicesCommonModule_IEntityConfigurationProvider_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityConfigurationProvider>());
        }

        [TestMethod]
        public void ServicesCommonModule_IAddAltKeyHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IAddAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IAddAltKeyHandler_IsRegistered_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<IAddAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>(),
                            _Container.Resolve<IAddAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IAddHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IAddHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IDeleteHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IDeleteHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IAddAltKeyHandler_IDeleteHandler_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<IDeleteHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _Container.Resolve<IDeleteHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGenerateRepositoryHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IGenerateRepositoryHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGenerateRepositoryHandler_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<IGenerateRepositoryHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _Container.Resolve<IGenerateRepositoryHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByAlternateKeyHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IGetByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByAlternateKeyHandler_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<IGetByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>(),
                            _Container.Resolve<IGetByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByIdHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IGetByIdHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByIdHandler_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<IGetByIdHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _Container.Resolve<IGetByIdHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByIdsHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IGetByIdsHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByIdsHandler_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<IGetByIdsHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _Container.Resolve<IGetByIdsHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByPropertyValuesHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IGetByPropertyValuesHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetByPropertyValuesHandler_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<IGetByIdsHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _Container.Resolve<IGetByIdsHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetPropertyValueHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IGetPropertyValueHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IGetPropertyValueHandler_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<IGetPropertyValueHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _Container.Resolve<IGetPropertyValueHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IInsertSeedDataHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IInsertSeedDataHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IInsertSeedDataHandler_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<IInsertSeedDataHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _Container.Resolve<IInsertSeedDataHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IQueryableHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IQueryableHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IQueryableHandler_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<IQueryableHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _Container.Resolve<IQueryableHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_ISearchByAlternateKeyHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<ISearchByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_ISearchByAlternateKeyHandler_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<ISearchByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>(),
                            _Container.Resolve<ISearchByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IUpdateAltKeyHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IUpdateAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IUpdateAltKeyHandler_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<IUpdateAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>(),
                            _Container.Resolve<IUpdateAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IUpdateHandler_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IUpdateHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IUpdateHandler_SingleInstance()
        {
            Assert.AreEqual(_Container.Resolve<IUpdateHandler<AltKeyEntity, IAltKeyEntity, int>>(),
                            _Container.Resolve<IUpdateHandler<AltKeyEntity, IAltKeyEntity, int>>());
        }

        [TestMethod]
        public void ServicesCommonModule_IServicesCommon_IsRegistered()
        {
            Assert.IsNotNull(_Container.Resolve<IServiceCommon<Entity, IEntity, int>>());
        }
    }
}