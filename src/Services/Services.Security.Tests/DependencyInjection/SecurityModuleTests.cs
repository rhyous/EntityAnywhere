using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services.Security;
using Rhyous.WebFramework.Services.Security.DependencyInjection;
using System.Collections.Specialized;

namespace Rhyous.WebFramework.Services.Tests.DependencyInjection
{
    [TestClass]
    public class SecurityModuleTests
    {
        private MockRepository _MockRepository;

        private IContainer _Container;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            var builder = new ContainerBuilder();

            // Register upstream objects
            var cipherKey = "somekey000011112223333";
            var iterations = 2500;
            var nvc = new NameValueCollection
            {
                { PasswordSecuritySettings.SystemSecurityKey, cipherKey },
                { PasswordSecuritySettings.SystemSecurityDerivationIterations, iterations.ToString() },
            };
            var mockAppSettings = _MockRepository.Create<IAppSettings>();
            mockAppSettings.Setup(m => m.Collection).Returns(nvc);
            builder.RegisterInstance(mockAppSettings.Object).As<IAppSettings>();

            // Register plugin Registrar
            var registrar = new Registrar();
            registrar.Register(builder);
            _Container = builder.Build();
        }

        [TestMethod]
        public void SecurityModule_IPasswordSecuritySettings_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IPasswordSecuritySettings>());
        }

        [TestMethod]
        public void SecurityModule_IPasswordSecuritySettings_IsRegistered_Singleton_Test()
        {
            var item1 = _Container.Resolve<IPasswordSecuritySettings>();
            var item2 = _Container.Resolve<IPasswordSecuritySettings>();
            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        public void SecurityModule_IPasswordSecurity_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IPasswordSecurity>());
        }

        [TestMethod]
        public void SecurityModule_IPasswordSecurity_IsRegistered_Singleton_Test()
        {
            var item1 = _Container.Resolve<IPasswordSecurity>();
            var item2 = _Container.Resolve<IPasswordSecurity>();
            Assert.AreEqual(item1, item2);
        }
    }
}
