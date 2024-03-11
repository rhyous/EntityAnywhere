using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.DependencyInjection;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Services.Tests.DependencyInjection
{
    [TestClass]
    public class AuthenticationServiceModuleTests
    {
        private MockRepository _MockRepository;

        private IContainer _Container;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);


            var builder = new ContainerBuilder();

            // Register upstream objects
            var mockLogger = _MockRepository.Create<ILogger>();
            builder.RegisterInstance(mockLogger.Object).As<ILogger>();

            var mockAppSettings = _MockRepository.Create<IAppSettings>();
            mockAppSettings.Setup(m=>m.Collection).Returns(new NameValueCollection());
            builder.RegisterInstance(mockAppSettings.Object).As<IAppSettings>();

            var mockHeaders = _MockRepository.Create<IHeaders>();
            builder.RegisterInstance(mockHeaders.Object).As<IHeaders>();

            var mockUserClient = _MockRepository.Create<IAdminEntityClientAsync<User, long>>();
            builder.RegisterInstance(mockUserClient.Object).As<IAdminEntityClientAsync<User, long>>();

            var mockUserRoleClient = _MockRepository.Create<IAdminEntityClientAsync<UserRole, int>>();
            builder.RegisterInstance(mockUserRoleClient.Object).As<IAdminEntityClientAsync<UserRole, int>>();

            var mockUserRoleMembershipClient = _MockRepository.Create<IAdminEntityClientAsync<UserRoleMembership, long>>();
            builder.RegisterInstance(mockUserRoleMembershipClient.Object).As<IAdminEntityClientAsync<UserRoleMembership, long>>();

            var mockAuthenticationAttemptClient = _MockRepository.Create<IAdminEntityClientAsync<AuthenticationAttempt, long>>();
            builder.RegisterInstance(mockAuthenticationAttemptClient.Object).As<IAdminEntityClientAsync<AuthenticationAttempt, long>>();

            var mockAlternateIdClient = _MockRepository.Create<IAdminEntityClientAsync<AlternateId, long>>();
            builder.RegisterInstance(mockAlternateIdClient.Object).As<IAdminEntityClientAsync<AlternateId, long>>();

            var mockUserRoleEntityDataCache = _MockRepository.Create<IUserRoleEntityDataCache>();
            builder.RegisterInstance(mockUserRoleEntityDataCache.Object).As<IUserRoleEntityDataCache>();

            builder.RegisterModule<SimplePluginLoaderModule>();

            // Register plugin Registrar
            builder.RegisterModule<AuthenticationServiceModule>();
            _Container = builder.Build();
        }

        [TestMethod]
        public void AuthenticationServiceModule_Resolve_IUserRoleProvider_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IUserRoleProvider>());
        }
        [TestMethod]
        public void AuthenticationServiceModule_Resolve_IAccountLocker_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAccountLocker>());
        }

        [TestMethod]
        public void AuthenticationServiceModule_Resolve_ICredentialsValidatorAsync_Test()
        {
            Assert.IsNotNull(_Container.Resolve<ICredentialsValidatorAsync>());
        }

        [TestMethod]
        public void AuthenticationServiceModule_Resolve_IBasicAuthCredentialsReader_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IBasicAuth>());
        }

        [TestMethod]
        public void AuthenticationServiceModule_Resolve_IBasicAuthEncoder_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IBasicAuthEncoder>());
        }
    }
}