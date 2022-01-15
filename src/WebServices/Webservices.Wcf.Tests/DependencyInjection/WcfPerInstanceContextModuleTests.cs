using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.WebServices;
using Rhyous.Wrappers;
using System;
using System.Net;
using System.ServiceModel.Channels;

namespace Rhyous.EntityAnywhere.Webservices.Tests.DependencyInjection
{
    [TestClass]
    public class WcfPerInstanceContextModuleTests
    {
        private MockRepository _MockRepository;

        private Mock<IOperationContext> _MockOperationContext;
        private Mock<IRequestContext> _MockRequestContext;
        private Mock<IMessage> _MockRequestMessage;
        private Mock<IWebOperationContext> _MockWebOperationContext;
        private Mock<IIncomingWebRequestContext> _MockIncomingWebRequestContext;
        private Mock<IOutgoingWebResponseContext> _MockOutgoingWebResponseContext;
        private Mock<IJWTToken> _MockJWTToken;
        private Mock<IAppSettings> _MockAppSettings;

        private ILifetimeScope _PerCallScope;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockOperationContext = _MockRepository.Create<IOperationContext>();
            _MockRequestContext = _MockRepository.Create<IRequestContext>();
            _MockRequestMessage = _MockRepository.Create<IMessage>();
            _MockWebOperationContext = _MockRepository.Create<IWebOperationContext>();
            _MockIncomingWebRequestContext = _MockRepository.Create<IIncomingWebRequestContext>();
            _MockOutgoingWebResponseContext = _MockRepository.Create<IOutgoingWebResponseContext>();
            _MockJWTToken = _MockRepository.Create<IJWTToken>();
            _MockAppSettings = _MockRepository.Create<IAppSettings>();


            _MockOperationContext.Setup(m => m.RequestContext).Returns(_MockRequestContext.Object);
            _MockRequestContext.Setup(m => m.RequestMessage).Returns(_MockRequestMessage.Object);
            var messageHeaders = new MessageHeaders(MessageVersion.None);
            messageHeaders.To = new Uri("https://test.domain.tld?Q1=val1&Q2=val2");
            _MockRequestMessage.Setup(m => m.Headers).Returns(messageHeaders);

            var messageProperties = new MessageProperties();
            var remoteEndpointMessageProperty = new RemoteEndpointMessageProperty("10.0.0.1", 443);
            messageProperties.Add(RemoteEndpointMessageProperty.Name, remoteEndpointMessageProperty);
            _MockOperationContext.Setup(m => m.IncomingMessageProperties).Returns(messageProperties);

            _MockWebOperationContext.Setup(m => m.IncomingRequest)
                                    .Returns(_MockIncomingWebRequestContext.Object);
            _MockWebOperationContext.Setup(m => m.OutgoingResponse)
                                    .Returns(_MockOutgoingWebResponseContext.Object);
            var headers = new WebHeaderCollection();
            headers.Add("H1", "Value1");
            headers.Add("H2", "Value2");
            _MockIncomingWebRequestContext.Setup(m => m.Headers).Returns(headers);

            var uriTemplateMatch = new UriTemplateMatch
            {
                RequestUri = messageHeaders.To
            };
            _MockIncomingWebRequestContext.Setup(m => m.UriTemplateMatch).Returns(uriTemplateMatch);

            var builder = new ContainerBuilder();
            // Register root scope mocks
            builder.RegisterInstance(_MockJWTToken.Object).As<IJWTToken>().SingleInstance();
            builder.RegisterInstance(_MockAppSettings.Object).As<IAppSettings>().SingleInstance();
            var mockUserRoleEntityDataCache = _MockRepository.Create<IUserRoleEntityDataCache>();
            builder.RegisterInstance(mockUserRoleEntityDataCache.Object).As<IUserRoleEntityDataCache>().SingleInstance();
            var mockAdminClaimsProvider = _MockRepository.Create<IAdminClaimsProvider>();
            builder.RegisterInstance(mockAdminClaimsProvider.Object).As<IAdminClaimsProvider>().SingleInstance();
            var rootContainer = builder.Build();

            var wcfScope = rootContainer.BeginLifetimeScope("wcfScope", (wcfBuilder) =>
            {
                // Register Wcf scope mocks
            });

            _PerCallScope = wcfScope.BeginLifetimeScope((perCallBuilder) =>
            {
                // Register module
                var module = new WcfPerInstanceContextModule(_MockOperationContext.Object,
                                                             _MockWebOperationContext.Object);
                perCallBuilder.RegisterModule(module);
            });
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IOperationContext_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IOperationContext>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IOperationContext_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IOperationContext>(), _PerCallScope.Resolve<IOperationContext>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IRequestUri_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRequestUri>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IRequestUri_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IRequestUri>(), _PerCallScope.Resolve<IRequestUri>());
        }


        [TestMethod]
        public void WcfPerInstanceContextModule_IRequestSourceIpAddress_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRequestSourceIpAddress>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IRequestSourceIpAddress_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IRequestSourceIpAddress>(), _PerCallScope.Resolve<IRequestSourceIpAddress>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IWebOperationContext_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IWebOperationContext>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IWebOperationContext_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IWebOperationContext>(), _PerCallScope.Resolve<IWebOperationContext>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IOutgoingWebResponseContext_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IOutgoingWebResponseContext>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IOutgoingWebResponseContext_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IOutgoingWebResponseContext>(), _PerCallScope.Resolve<IOutgoingWebResponseContext>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IHeaders_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IHeaders>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IHeaders_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IHeaders>(), _PerCallScope.Resolve<IHeaders>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IClaimsProvider_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IClaimsProvider>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IClaimsProvider_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IClaimsProvider>(), _PerCallScope.Resolve<IClaimsProvider>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IAdminClaimsProvider_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IAdminClaimsProvider>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IAdminClaimsProvider_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IAdminClaimsProvider>(), _PerCallScope.Resolve<IAdminClaimsProvider>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IUserDetails_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IUserDetails>());
        }

        [TestMethod]
        public void WcfPerInstanceContextModule_IUserDetails_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IUserDetails>(), _PerCallScope.Resolve<IUserDetails>());
        }
    }
}
