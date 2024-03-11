using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests.Tools
{
    [TestClass]
    public class HttpClientRunnerTests
    {
        private MockRepository _MockRepository;

        private Mock<IHttpClientFactory> _MockHttpClientFactory;
        private Mock<IHeaders> _MockHeaders;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockHttpClientFactory = _MockRepository.Create<IHttpClientFactory>();
            _MockHeaders = _MockRepository.Create<IHeaders>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private HttpClientRunner CreateHttpClientRunner()
        {
            return new HttpClientRunner(
                _MockHttpClientFactory.Object,
                _MockHeaders.Object);
        }

        #region Constructor tests
        [TestMethod]
        public void HttpClientRunner_Constructor_HttpClient_Null_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new HttpClientRunner(null, _MockHeaders.Object);
            });
        }

        [TestMethod]
        public void HttpClientRunner_Constructor_Headers_Null_Test()
        {

            Assert.IsNotNull(new HttpClientRunner(_MockHttpClientFactory.Object, null));
        }

        #endregion

        #region GetResponse without Content
        [TestMethod]
        public async Task HttpClientRunner_GetResponse_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Get;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = false;
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent("{\"Id\":\"27\",\"Name\":\"E27\"}")
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
                           .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);
            var expected = new EntityInt { Id = 27, Name = "E27" };

            // Act
            var result = await httpClientRunner.GetResponse(method, url, forwardExceptions);

            // Assert
            Assert.AreEqual(result, response);
        }

        [TestMethod]
        public async Task HttpClientRunner_GetResponse_Fail_ForwardsException_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Get;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = true;
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new JsonContent("This was a bad failure")
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
                           .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);
            var expected = new EntityInt { Id = 27, Name = "E27" };

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ServiceErrorResponseForwarderException>(async () =>
            {
                var result = await httpClientRunner.GetResponse(method, url, forwardExceptions);
            });
        }

        [TestMethod]
        public async Task HttpClientRunner_GetResponse_Get404_DoesNotCauseForwardsException_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Get;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = true;
            var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new JsonContent("NotFound")
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
                           .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);
            var expected = new EntityInt { Id = 27, Name = "E27" };

            // Act
            var result = await httpClientRunner.GetResponse(method, url, forwardExceptions);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task HttpClientRunner_GetResponse_404_CausesForwardsExceptionIfNotGET_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Post;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = true;
            var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new JsonContent("NotFound")
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
                           .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);
            var expected = new EntityInt { Id = 27, Name = "E27" };
            var content = new JsonContent("{\"a\":\"b\"}");

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ServiceErrorResponseForwarderException>(async () =>
            {
                var result = await httpClientRunner.GetResponse(method, url, content, forwardExceptions);
            });
        }

        [TestMethod]
        public async Task HttpClientRunner_GetResponse_Fail_ForwardsException_NoMessage_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Get;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = true;
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new JsonContent("")
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
                           .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);
            var expected = new EntityInt { Id = 27, Name = "E27" };

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ServiceErrorResponseForwarderException>(async () =>
            {
                var result = await httpClientRunner.GetResponse(method, url, forwardExceptions);
            });
        }

        [TestMethod]
        public async Task HttpClientRunner_GetResponse_Null_Url()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Get;
            string url = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await httpClientRunner.GetResponse(method, url, forwardExceptions);
            });
        }

        [TestMethod]
        public async Task HttpClientRunner_GetResponse_Null_HttpMethod()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = null;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await httpClientRunner.GetResponse(method, url, forwardExceptions);
            });
        }
        #endregion

        #region GetResponse with Content
        [TestMethod]
        public async Task HttpClientRunner_GetResponse_WithContent_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Get;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = false;
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent("{\"Id\":\"27\",\"Name\":\"E27\"}")
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
                           .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);
            var expected = new EntityInt { Id = 27, Name = "E27" };

            // Act
            var result = await httpClientRunner.GetResponse(method, url, forwardExceptions);

            // Assert
            Assert.AreEqual(result, response);
        }

        [TestMethod]
        public async Task HttpClientRunner_GetResponse_WithContent_Fail_ForwardsException_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Post;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = true;
            HttpContent content = new JsonContent("");
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new JsonContent("This was a bad failure")
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
                           .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);
            var expected = new EntityInt { Id = 27, Name = "E27" };

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ServiceErrorResponseForwarderException>(async () =>
            {
                var result = await httpClientRunner.GetResponse(method, url, content, forwardExceptions);
            });
        }

        [TestMethod]
        public async Task HttpClientRunner_GetResponse_WithContent_Null_Url()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Get;
            string url = null;
            bool forwardExceptions = false;
            HttpContent content = new JsonContent("");

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await httpClientRunner.GetResponse(method, url, content, forwardExceptions);
            });
        }

        [TestMethod]
        public async Task HttpClientRunner_GetResponse_WithContent_Null_HttpMethod()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = null;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = false;
            HttpContent content = new JsonContent("");

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await httpClientRunner.GetResponse(method, url, content, forwardExceptions);
            });
        }
        #endregion

        [TestMethod]
        public async Task HttpClientRunner_RunAndDeserialize_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Get;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = false;
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent("{\"Id\":\"27\",\"Name\":\"E27\"}")
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
                           .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);
            var expected = new EntityInt { Id = 27, Name = "E27" };

            // Act
            var result = await httpClientRunner.RunAndDeserialize<EntityInt>(method, url, forwardExceptions);

            // Assert
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Name, result.Name);
        }

        [TestMethod]
        public async Task HttpClientRunner_Run_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Get;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = false;
            var expected = "{\"Id\":\"27\",\"Name\":\"E27\"}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent(expected)
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
                           .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);
            // Act
            var result = await httpClientRunner.Run(method, url, forwardExceptions);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task HttpClientRunner_RunAndReturnStream_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Get;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = false;
            var expected = "content";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(expected.ToStream())
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
               .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);

            // Act
            var result = await httpClientRunner.RunAndReturnStream(method, url, forwardExceptions);

            // Assert
            Assert.AreEqual(expected, result.AsString());
        }

        [TestMethod]
        public async Task HttpClientRunner_Run_WithContent_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Post;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = true;
            HttpContent content = new JsonContent("");
            var expected = "{\"Id\":\"27\",\"Name\":\"E27\"}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent(expected)
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
               .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);

            // Act
            var result = await httpClientRunner.Run(method, url, content, forwardExceptions);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task HttpClientRunner_RunAndReturnStream_WithContent_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Get;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = false;
            HttpContent content = new JsonContent("");
            var expected = "content";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(expected.ToStream())
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
               .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);

            // Act
            var result = await httpClientRunner.RunAndReturnStream(method, url, content, forwardExceptions);

            // Assert
            Assert.AreEqual(expected, result.AsString());
        }

        [TestMethod]
        public async Task HttpClientRunner_RunAndDeserialize_WithContent_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Post;
            string url = "https://some.domain.tld/";
            bool forwardExceptions = false;
            HttpContent content = new JsonContent("");
            var expected = new EntityInt { Id = 27, Name = "E27" };
            var json = "{\"Id\":\"27\",\"Name\":\"E27\"}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent(json)
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
               .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);

            // Act
            var result = await httpClientRunner.RunAndDeserialize<EntityInt>(method, url, content, forwardExceptions);

            // Assert
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Name, result.Name);
        }

        [TestMethod]
        public async Task HttpClientRunner_Run_GenericContent_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Post;
            string url = "https://some.domain.tld/";
            IEnumerable<int> content = new int[] { 1, 2, 3 };
            JsonSerializerSettings settings = null;
            bool forwardExceptions = false;
            var json = "{\"Id\":\"27\",\"Name\":\"E27\"}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent(json)
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
               .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);

            // Act
            var result = await httpClientRunner.Run(method, url, content, settings, forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task HttpClientRunner_RunAndDeserialize_ObjectContent_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            HttpMethod method = HttpMethod.Post;
            string url = "https://some.domain.tld/";
            object content = new int[] { 1, 2, 3 };
            JsonSerializerSettings settings = null;
            bool forwardExceptions = false;
            var expected = new EntityInt { Id = 27, Name = "E27" };
            var json = "{\"Id\":\"27\",\"Name\":\"E27\"}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent(json)
            };
            var mockHttpClient = _MockRepository.Create<IHttpClient>();
            _MockHttpClientFactory.Setup(m => m.GetHttpClient(null)).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>()))
               .ReturnsAsync(response);
            _MockHeaders.Setup(m => m.Collection)
                        .Returns<INameValueCollection>(null);

            // Act
            var result = await httpClientRunner.RunAndDeserialize<object, EntityInt>(method, url, content, settings, forwardExceptions);

            // Assert
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Name, result.Name);
        }

        [TestMethod]
        public async Task HttpClientRunner_ConvertToHttpContent_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            IEnumerable<int> content = new int[] { 1, 2, 3 };
            JsonSerializerSettings settings = null;

            // Act
            var result = httpClientRunner.ConvertToHttpContent(
                content,
                settings);

            // Assert
            Assert.AreEqual("[1,2,3]", await result.ReadAsStringAsync());
        }

        [TestMethod]
        public void HttpClientRunner_AddTokens_Test()
        {
            // Arrange
            var httpClientRunner = CreateHttpClientRunner();
            var request = new HttpRequestMessage();
            var headers = new NameValueCollection
            {
                { "Token","token-abc-1"}
            };
            _MockHeaders.Setup(m => m.Collection)
                        .Returns(headers);

            // Act
            httpClientRunner.AddTokens(request);

            // Assert
            Assert.AreEqual("token-abc-1", request.Headers.GetValues("Token").First());
        }
    }
}
