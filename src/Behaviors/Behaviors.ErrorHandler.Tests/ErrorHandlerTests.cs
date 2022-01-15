using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Authentication;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace Rhyous.EntityAnywhere.Behaviors.Tests
{
    [TestClass]
    public class ErrorHandlerTests
    {
        [TestMethod]
        public void HandleError_ValidArguments_ReturnsTrue()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var errorHandler = new ErrorHandler(mockLogger.Object);

            // Act
            var result = errorHandler.HandleError(new Exception());

            // Assert
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void ProvideFault_AuthenticationException_LoggerWritesError()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var errorHandler = new ErrorHandler(mockLogger.Object);

            var message = GetFakeJsonMessage(true);
            // Act
            errorHandler.ProvideFault(new AuthenticationException(), MessageVersion.Default, ref message);

            // Assert
            mockLogger.Verify(x => x.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once());            
        }

        [TestMethod]
        public void ProvideFault_SerializationException_LoggerWritesError()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var errorHandler = new ErrorHandler(mockLogger.Object);

            var message = GetFakeJsonMessage(true);
            // Act
            errorHandler.ProvideFault(new SerializationException(), MessageVersion.Default, ref message);

            // Assert
            mockLogger.Verify(x => x.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ProvideFault_RestException_LoggerWritesError()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var errorHandler = new ErrorHandler(mockLogger.Object);

            var message = GetFakeJsonMessage(true);
            // Act
            errorHandler.ProvideFault(new RestException(HttpStatusCode.Unauthorized), MessageVersion.Default, ref message);

            // Assert
            mockLogger.Verify(x => x.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ProvideFault_NoSpecificException_LoggerWritesError()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var errorHandler = new ErrorHandler(mockLogger.Object);

            var message = GetFakeJsonMessage(true);
            // Act
            errorHandler.ProvideFault(new Exception(), MessageVersion.Default, ref message);

            // Assert
            mockLogger.Verify(x => x.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }
        [TestMethod]
        public void ProvideFault_NoSpecificExceptionNullMessage_MessageNotNull()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var errorHandler = new ErrorHandler(mockLogger.Object);

            Message message = null;
            // Act
            errorHandler.ProvideFault(new SerializationException(), MessageVersion.Default, ref message);

            // Assert
            Assert.IsNotNull(message);
        }

        [TestMethod]
        public void ProvideFault_LoggerIsNull_MessageNotNull()
        {
            // Arrange
            var errorHandler = new ErrorHandler(null);

            Message message = null;
            // Act
            errorHandler.ProvideFault(new SerializationException(), MessageVersion.Default, ref message);

            // Assert
            Assert.IsNotNull(message);
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
