using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using System;
using System.Text;

namespace Rhyous.EntityAnywhere.Services.Tests.Business
{
    [TestClass]
    public class BasicAuthEncoderTests
    {
        private MockRepository _MockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
        }

        private BasicAuthEncoder CreateBasicAuthEncoder()
        {
            return new BasicAuthEncoder();
        }

        #region Decode

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void BasicAuthEncoder_Decode_EncodedHeader_NullEmptyOrWhitespace_Throws(string encodedHeader)
        {
            // Arrange
            var basicAuthEncoder = CreateBasicAuthEncoder();
            Encoding encoding = Encoding.UTF8;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                basicAuthEncoder.Decode(encodedHeader, encoding);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BasicAuthEncoder_Decode_EncodedHeader_EncodingNull_Throws()
        {
            // Arrange
            var basicAuthEncoder = CreateBasicAuthEncoder();
            string encodedHeader = "a";
            Encoding encoding = null;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                basicAuthEncoder.Decode(encodedHeader, encoding);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BasicAuthEncoder_Decode_ValidHeader_Decodes()
        {
            // Arrange
            var basicAuthEncoder = CreateBasicAuthEncoder();
            string encodedHeader = "Basic dXNlcjI3OnB3LTEyMzQ=";
            Encoding encoding = Encoding.UTF8;

            string expectedUser = "user27";
            string expectedPassword = "pw-1234";

            // Act
            var result = basicAuthEncoder.Decode(encodedHeader, encoding);

            // Assert
            Assert.AreEqual(expectedUser, result.User);
            Assert.AreEqual(expectedPassword, result.Password);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Encode

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void BasicAuthEncoder_Encode_User_NullEmptyOrWhitespace_Throws(string user)
        {
            // Arrange
            var basicAuthEncoder = CreateBasicAuthEncoder();
            string password = "pw-1234";
            Encoding encoding = Encoding.UTF8;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                basicAuthEncoder.Encode(user, password, encoding);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList(null, "")]
        public void BasicAuthEncoder_Encode_Password_NullEmptyOrWhitespace_Throws(string password)
        {
            // Arrange
            var basicAuthEncoder = CreateBasicAuthEncoder();
            string user = "user27";
            Encoding encoding = Encoding.UTF8;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                basicAuthEncoder.Encode(user, password, encoding);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BasicAuthEncoder_Encode_Encoding_Null_Throws()
        {
            // Arrange
            var basicAuthEncoder = CreateBasicAuthEncoder();
            string user = "user27";
            string password = "pw-1234";
            Encoding encoding = null;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                basicAuthEncoder.Encode(user, password, encoding);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BasicAuthEncoder_Encode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var basicAuthEncoder = CreateBasicAuthEncoder();
            string user = "user27";
            string password = "pw-1234";
            Encoding encoding = Encoding.UTF8;
            var expected = "Basic dXNlcjI3OnB3LTEyMzQ=";

            // Act
            var result = basicAuthEncoder.Encode(user, password, encoding);

            // Assert
            Assert.AreEqual(expected, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
