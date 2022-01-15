using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using Rhyous.EntityAnywhere.Services.DependencyInjection;
using System.Collections.Specialized;

namespace WebServices.Tests.DependencyInjection
{
    [TestClass]
    public class UserServiceModuleTests
    {
        private MockRepository _MockRepository;

        private Mock<IServiceHandlerProviderAltKey<User, IUser, long, string>> _MockServiceHandlerProviderAltKey;
        private Mock<IRepository<User, IUser, long>> _MockRepo;
        private Mock<IAppSettings> _MockAppSettings;
        private Mock<ILogger> _MockLogger;

        private Mock<IEntityConfigurationProvider> _MockEntityConfigurationProvider;

        private IContainer _Container;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockServiceHandlerProviderAltKey = _MockRepository.Create<IServiceHandlerProviderAltKey<User, IUser, long, string>>();
            _MockRepo = _MockRepository.Create<IRepository<User, IUser, long>>();
            _MockAppSettings = _MockRepository.Create<IAppSettings>();
            _MockLogger = _MockRepository.Create<ILogger>();

            _MockEntityConfigurationProvider = _MockRepository.Create<IEntityConfigurationProvider>();

            var builder = new ContainerBuilder();

            // Register upstream dependencies
            builder.RegisterInstance(_MockServiceHandlerProviderAltKey.Object).As<IServiceHandlerProviderAltKey<User, IUser, long, string>>().SingleInstance();
            builder.RegisterInstance(_MockEntityConfigurationProvider.Object).As<IEntityConfigurationProvider>().SingleInstance();
            builder.RegisterInstance(_MockAppSettings.Object).As<IAppSettings>();
            builder.RegisterInstance(_MockRepo.Object).As<IRepository<User, IUser, long>>();
            builder.RegisterInstance(_MockLogger.Object).As<ILogger>();

            var mockEntityClient = _MockRepository.Create<IAdminEntityClientAsync<Entity, int>>();
            builder.RegisterInstance(mockEntityClient.Object).As<IAdminEntityClientAsync<Entity, int>>();
            var mockEntityPropertyClient = _MockRepository.Create<IAdminEntityClientAsync<EntityProperty, int>>();
            builder.RegisterInstance(mockEntityPropertyClient.Object).As<IAdminEntityClientAsync<EntityProperty, int>>();

            // Register plugin Registrar
            builder.RegisterModule<UserServiceModule>();
            _Container = builder.Build();
        }

        [TestMethod]
        public void UserServiceModule_Resolve_IDuplicateUsernameDetector_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IDuplicateUsernameDetector>());
        }

        [TestMethod]
        public void UserServiceModule_Resolve_IPasswordManager_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IPasswordManager>());
        }

        [TestMethod]
        public void UserServiceModule_Resolve_IUserService_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IUserService>());
        }

        [TestMethod]
        public void UserServiceModule_Resolve_IServiceCommonAlternateKey_User_IUser_long_string_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IServiceCommonAlternateKey<User, IUser, long, string>>());
        }

        [TestMethod]
        public void UserServiceModule_Resolve_IServiceCommon_User_IUser_long_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IServiceCommon<User, IUser, long>>());
        }

        [TestMethod]
        public void UserServiceModule_Resolve_IRepository_User_IUser_long_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IRepository<User, IUser, long>>());
        }
    }
}