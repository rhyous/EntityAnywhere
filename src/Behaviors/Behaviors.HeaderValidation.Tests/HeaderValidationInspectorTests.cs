using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using Rhyous.Wrappers;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace Rhyous.EntityAnywhere.Behaviors.Tests
{
    [TestClass]
    public class HeaderValidationInspectorTests
    {
        private MockRepository _MockRepository;
        
        private Mock<IPluginHeaderValidator> _MockHeaderValidator;
        private Mock<IAccessController> _MockAccessController;
        private Mock<ILogger> _MockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockHeaderValidator = _MockRepository.Create<IPluginHeaderValidator>();
            _MockAccessController = _MockRepository.Create<IAccessController>();
            _MockLogger = _MockRepository.Create<ILogger>();
        }

        private HeaderValidationInspector CreateHeaderValidationInspector()
        {
            return new HeaderValidationInspector(
                _MockHeaderValidator.Object,
                _MockAccessController.Object,
                _MockLogger.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(RestException))]
        public void AfterReceiveRequest_NullWebOperationContext_BadRequest()
        {
            // Arrange
            SetupMockAccessController(false, true, false, false, false, Data.token);
            var JWTTokenMock = new Mock<IJWTToken>();
            var mockClientChannel = new Mock<IClientChannel>();
            var message = GetFakeJsonMessage(true);
            var mockWebOperationContext = new Mock<IWebOperationContext>();
            var mockIncomingWebRequest = new Mock<IIncomingWebRequestContext>();

            mockIncomingWebRequest.Setup(x => x.Headers).Returns(new WebHeaderCollection());
            mockWebOperationContext.Setup(x => x.IncomingRequest).Returns(mockIncomingWebRequest.Object);

            var headerValidationInspector = CreateHeaderValidationInspector();

            // Act and Assert
            var result = headerValidationInspector.AfterReceiveRequest(ref message, mockClientChannel.Object, new InstanceContext(new object()));
        }

        [TestMethod]
        public void AfterReceiveRequest_IsAnonymousAllowedWithoutWebOperationContext_ReturnsNull()
        {
            // Arrange
            SetupMockAccessController(false, true, false, false, false, Data.token);
            var JWTTokenMock = new Mock<IJWTToken>();
            var mockClientChannel = new Mock<IClientChannel>();
            var message = GetFakeJsonMessage(true);
            var mockWebOperationContext = new Mock<IWebOperationContext>();
            var mockIncomingWebRequest = new Mock<IIncomingWebRequestContext>();

            var headers = new WebHeaderCollection();
            mockWebOperationContext.Setup(x => x.IncomingRequest).Returns(mockIncomingWebRequest.Object);

            var headerValidationInspector = CreateHeaderValidationInspector();

            // Act
            var result = headerValidationInspector.AfterReceiveRequest(ref message, mockClientChannel.Object, new InstanceContext(new object()), mockWebOperationContext.Object, headers);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void AfterReceiveRequest_IsAnonymousAllowed_ReturnsNull()
        {
            // Arrange
            SetupMockAccessController(false, true, false, false, false, Data.token);
            var JWTTokenMock = new Mock<IJWTToken>();
            var mockClientChannel = new Mock<IClientChannel>();
            var message = GetFakeJsonMessage(true);
            var mockWebOperationContext = new Mock<IWebOperationContext>();
            var mockIncomingWebRequest = new Mock<IIncomingWebRequestContext>();

            var headers = new WebHeaderCollection();
            mockWebOperationContext.Setup(x => x.IncomingRequest).Returns(mockIncomingWebRequest.Object);
            
            var headerValidationInspector = CreateHeaderValidationInspector();

            // Act
            var result = headerValidationInspector.AfterReceiveRequest(ref message, mockClientChannel.Object, new InstanceContext(new object()), mockWebOperationContext.Object, headers);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void AfterReceiveRequest_IsCustomerSystemAdmin_ReturnsNull()
        {
            SetupMockAccessController(false, false, true, false, false, Data.token);
            var JWTTokenMock = new Mock<IJWTToken>();
            var mockClientChannel = new Mock<IClientChannel>();
            var message = GetFakeJsonMessage(true);
            var mockWebOperationContext = new Mock<IWebOperationContext>();
            var mockIncomingWebRequest = new Mock<IIncomingWebRequestContext>();

            var headers = new WebHeaderCollection();
            mockWebOperationContext.Setup(x => x.IncomingRequest).Returns(mockIncomingWebRequest.Object);

            var headerValidationInspector = CreateHeaderValidationInspector(); 

            // Act
            var result = headerValidationInspector.AfterReceiveRequest(ref message, mockClientChannel.Object, new InstanceContext(new object()), mockWebOperationContext.Object, headers);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void AfterReceiveRequest_IsCustomerAdmin_ReturnsNull()
        {
            SetupMockAccessController(false, false, false, true, false, Data.token);
            var JWTTokenMock = _MockRepository.Create<IJWTToken>();
            var mockClientChannel = _MockRepository.Create<IClientChannel>();
            var message = GetFakeJsonMessage(true);
            var mockWebOperationContext = _MockRepository.Create<IWebOperationContext>();
            var mockIncomingWebRequest = _MockRepository.Create<IIncomingWebRequestContext>();
            mockIncomingWebRequest.Setup(m => m.Method).Returns("GET");
            var headerValidationInspector = CreateHeaderValidationInspector();

            var headers = new WebHeaderCollection();
            mockWebOperationContext.Setup(x => x.IncomingRequest).Returns(mockIncomingWebRequest.Object);

            _MockHeaderValidator.Setup(x => x.IsValidAsync(It.IsAny<NameValueCollection>())).ReturnsAsync(true);
            _MockHeaderValidator.Setup(x => x.UserId).Returns(27);

            // Act
            var result = headerValidationInspector.AfterReceiveRequest(ref message, mockClientChannel.Object, new InstanceContext(new object()), mockWebOperationContext.Object, headers);

            // Assert
            Assert.AreEqual(result, null);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void AfterReceiveRequest_IsCustomerAllowedUrl_ReturnsNull()
        {
            SetupMockAccessController(false, false, false, false, true, Data.token);
            var JWTTokenMock = _MockRepository.Create<IJWTToken>();
            JWTTokenMock.Setup(x => x.DecodeToken(It.IsAny<string>())).Returns("[{\"Subject\":\"User\",\"Issuer\":\"LOCAL AUTHORITY\",\"Claims\":[{\"Name\":\"Username\",\"Value\":\"Elih\",\"Subject\":\"User\",\"Issuer\":\"LOCAL AUTHORITY\"},{\"Name\":\"LastAuthenticated\",\"Value\":\"5/1/2018 2:40:03 PM\",\"Subject\":\"User\",\"Issuer\":\"LOCAL AUTHORITY\"}]},{\"Subject\":\"Organization\",\"Issuer\":\"LOCAL AUTHORITY\",\"Claims\":[{\"Name\":\"Id\",\"Value\":\"203338\",\"Subject\":\"Organization\",\"Issuer\":\"LOCAL AUTHORITY\"},{\"Name\":\"LastAuthenticated\",\"Value\":\"5/1/2018 2:40:03 PM\",\"Subject\":\"Organization\",\"Issuer\":\"LOCAL AUTHORITY\"},{\"Name\":\"Name\",\"Value\":\"Warehouse One\",\"Subject\":\"Organization\",\"Issuer\":\"LOCAL AUTHORITY\"},{\"Name\":\"SapId\",\"Value\":\"WarehouseO\",\"Subject\":\"Organization\",\"Issuer\":\"LOCAL AUTHORITY\"}]},{\"Subject\":\"UserRole\",\"Issuer\":\"LOCAL AUTHORITY\",\"Claims\":[{\"Name\":\"Role\",\"Value\":\"Customer\",\"Subject\":\"UserRole\",\"Issuer\":\"LOCAL AUTHORITY\"}]}]");
            var mockClientChannel = _MockRepository.Create<IClientChannel>();
            var message = GetFakeJsonMessage(true);
            var mockWebOperationContext = _MockRepository.Create<IWebOperationContext>();
            var mockIncomingWebRequest = _MockRepository.Create<IIncomingWebRequestContext>();
            mockIncomingWebRequest.Setup(m => m.Method).Returns("GET");

            var headers = new WebHeaderCollection();
            mockWebOperationContext.Setup(x => x.IncomingRequest).Returns(mockIncomingWebRequest.Object);

            _MockHeaderValidator.Setup(x => x.IsValidAsync(It.IsAny<NameValueCollection>())).ReturnsAsync(true);
            _MockHeaderValidator.Setup(x => x.UserId).Returns(27);

            var headerValidationInspector = CreateHeaderValidationInspector();

            // Act
            var result = headerValidationInspector.AfterReceiveRequest(ref message, mockClientChannel.Object, new InstanceContext(new object()), mockWebOperationContext.Object, headers);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        [ExpectedException(typeof(RestException))]
        public void AfterReceiveRequest_NoCustomerAccess_RestException()
        {
            // Arrange
            SetupMockAccessController(false, false, false, false, false, Data.token);
            var JWTTokenMock = _MockRepository.Create<IJWTToken>();
            var mockClientChannel = _MockRepository.Create<IClientChannel>();
            var message = GetFakeJsonMessage(true);
            var mockWebOperationContext = _MockRepository.Create<IWebOperationContext>();
            var mockIncomingWebRequest = _MockRepository.Create<IIncomingWebRequestContext>();
            mockIncomingWebRequest.Setup(m => m.Method).Returns("GET");

            var headers = new WebHeaderCollection();
            mockWebOperationContext.Setup(x => x.IncomingRequest).Returns(mockIncomingWebRequest.Object);

            _MockHeaderValidator.Setup(x => x.IsValidAsync(It.IsAny<NameValueCollection>())).ReturnsAsync(false);
            _MockHeaderValidator.Setup(x => x.UserId).Returns(27);

            _MockLogger.Setup(m => m.Debug(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));

            var headerValidationInspector = CreateHeaderValidationInspector();

            // Act and Assert
            var result = headerValidationInspector.AfterReceiveRequest(ref message, mockClientChannel.Object, new InstanceContext(new object()), mockWebOperationContext.Object, headers);
        }

        private void SetupMockAccessController(bool isRequestNull, bool isAnonymousAllowed, bool isCustomerSystemAdmin, bool isCustomerAdmin, bool isCustomerAllowedUrl, IToken token)
        {
            _MockAccessController.Setup(x => x.IsAnonymousAllowed(It.IsAny<string>())).Returns(isAnonymousAllowed);
            _MockAccessController.Setup(x => x.IsSystemAdmin(It.IsAny<NameValueCollection>(), It.IsAny<IWebOperationContext>())).Returns(isCustomerSystemAdmin);
        }

        public Message GetFakeJsonMessage(bool hasHeaders)
        {
            var json = "{\"Id\":1}";
            var bytes = Encoding.UTF8.GetBytes(json);
            string messageBody = Encoding.UTF8.GetString(bytes);

            MemoryStream ms = new MemoryStream();
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateBinaryWriter(ms);
            writer.WriteStartElement("Binary");
            writer.WriteBase64(bytes, 0, bytes.Length);
            writer.WriteEndElement();
            writer.Flush();
            ms.Position = 0;

            XmlDictionaryReader reader = XmlDictionaryReader.CreateBinaryReader(ms, XmlDictionaryReaderQuotas.Max);
            Message newMessage = Message.CreateMessage(reader, int.MaxValue, MessageVersion.None);
            var properties = new MessageProperties { { "A", "1" } };

            if (hasHeaders)
            {
                var addressHeader = AddressHeader.CreateAddressHeader("To", "http://schemas.microsoft.com/ws/2005/05/addressing/none", new Uri("https://mysite.tld"));
                newMessage.Headers.Add(addressHeader.ToMessageHeader());
            }
            newMessage.Properties.CopyProperties(properties);

            return newMessage;
        }
    }
}
