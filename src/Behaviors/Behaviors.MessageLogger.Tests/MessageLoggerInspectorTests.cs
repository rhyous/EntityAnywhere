using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace Rhyous.EntityAnywhere.Behaviors.Tests
{
    [TestClass]
    public class MessageLoggerInspectorTests
    {
        [TestMethod]
        public void MessageLoggerInspector_AfterReceiveRequest_GivenValidArguments_ReturnsValedUriAndRunsDebug()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockClientChannel = new Mock<IClientChannel>();
            var mockPostLoggerHelper = new Mock<IMessageHandler>();
            var httpRequestMessageProperty = new HttpRequestMessageProperty();
            httpRequestMessageProperty.Headers.Add("a","1");
            mockPostLoggerHelper.Setup(x => x.GetHttpRequestMessageProperty(It.IsAny<Message>(), It.IsAny<string>())).Returns(httpRequestMessageProperty);
            mockPostLoggerHelper.Setup(x => x.LogRequestMessage).Returns(true);
            var message = GetFakeJsonMessage(true);
            var instanceContext = new InstanceContext(new object());
            var postLoggerInspector = new MessageLoggerInspector(mockLogger.Object, mockPostLoggerHelper.Object);

            // Act
            var result = postLoggerInspector.AfterReceiveRequest(ref message, mockClientChannel.Object, instanceContext);

            // Assert
            Assert.IsNotNull(result);
            mockLogger.Verify(x => x.Debug(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(3));
        }

        [TestMethod]
        public void MessageLoggerInspector_AfterReceiveRequest_LogPostsFalse_Returns_RequestUri()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockClientChannel = new Mock<IClientChannel>();
            var mockPostLoggerHelper = new Mock<IMessageHandler>();
            var httpRequestMessageProperty = new HttpRequestMessageProperty();
            mockPostLoggerHelper.Setup(x => x.GetHttpRequestMessageProperty(It.IsAny<Message>(), It.IsAny<string>())).Returns(httpRequestMessageProperty);
            mockPostLoggerHelper.Setup(x => x.LogRequestMessage).Returns(false);
            var message = GetFakeJsonMessage(true);
            var instanceContext = new InstanceContext(new object());
            var postLoggerInspector = new MessageLoggerInspector(mockLogger.Object, mockPostLoggerHelper.Object);
            
            // Act
            var result = postLoggerInspector.AfterReceiveRequest(ref message, mockClientChannel.Object, instanceContext) as Uri;

            // Assert
            Assert.AreEqual("https://mysite.tld/", result.AbsoluteUri);
        }

        [TestMethod]
        public void MessageLoggerInspector_AfterReceiveRequest_NoHeaders_NullUriAndRunsDebugTwice()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockClientChannel = new Mock<IClientChannel>();
            var mockPostLoggerHelper = new Mock<IMessageHandler>();
            var httpRequestMessageProperty = new HttpRequestMessageProperty();
            mockPostLoggerHelper.Setup(x => x.GetHttpRequestMessageProperty(It.IsAny<Message>(), It.IsAny<string>())).Returns(httpRequestMessageProperty);
            mockPostLoggerHelper.Setup(x => x.LogRequestMessage).Returns(true);
            var message = GetFakeJsonMessage(false);
            var instanceContext = new InstanceContext(new object());
            var postLoggerInspector = new MessageLoggerInspector(mockLogger.Object, mockPostLoggerHelper.Object);

            // Act
            var result = postLoggerInspector.AfterReceiveRequest(ref message, mockClientChannel.Object, instanceContext);

            // Assert
            Assert.AreEqual(result, null);
            mockLogger.Verify(x => x.Debug(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(1));
        }

        [TestMethod]
        public void MessageLoggerInspector_AfterReceiveRequest_NoHeadersIsEmpty_NullUriAndRunsDebugOnce()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockClientChannel = new Mock<IClientChannel>();
            var mockPostLoggerHelper = new Mock<IMessageHandler>();
            var httpRequestMessageProperty = new HttpRequestMessageProperty();
            mockPostLoggerHelper.Setup(x => x.GetHttpRequestMessageProperty(It.IsAny<Message>(), It.IsAny<string>())).Returns(httpRequestMessageProperty);
            mockPostLoggerHelper.Setup(x => x.LogRequestMessage).Returns(true);
            var message = Message.CreateMessage(MessageVersion.Default, "");
            var instanceContext = new InstanceContext(new object());
            var postLoggerInspector = new MessageLoggerInspector(mockLogger.Object, mockPostLoggerHelper.Object);

            // Act
            var result = postLoggerInspector.AfterReceiveRequest(ref message, mockClientChannel.Object, instanceContext);

            // Assert
            Assert.AreEqual(result, null);
            mockLogger.Verify(x => x.Debug(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(1));
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
