using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Models;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Rhyous.EntityAnywhere.Clients2.Common;

namespace Rhyous.EntityAnywhere.Clients2
{
    /// <summary>
    /// This class is used by any tool to run common HttpClient commands asynchronously. Whether calling GET, POST, PUT, PATCH, DELETE, etc, the way to make the call and get the response back is standardized.
    /// </summary>
    public class HttpClientRunnerNoHeaders : IHttpClientRunner
    {
        private readonly IHttpClientFactory _HttpClientFactory;

        public HttpClientRunnerNoHeaders(IHttpClientFactory httpClientFactory)
        {
            _HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        #region Send without content (GET, DELETE)
        internal async Task<HttpResponseMessage> GetResponse(HttpMethod method, string url, bool forwardExceptions = true)
        {
            if (method is null) { throw new ArgumentNullException(nameof(method)); }
            if (string.IsNullOrWhiteSpace(url)) { throw new ArgumentException($"The {nameof(url)} cannot be null, empty, or whitespace.", nameof(url)); }
            HttpResponseMessage response = null;
            int retries = Retry.DefaultRetries;
            await new Retry().RetryAsync(async () =>
            {
                using (var request = new HttpRequestMessage(method, url))
                {
                    AddTokens(request);
                    var httpClient = _HttpClientFactory.GetHttpClient();
                    response = await httpClient.SendAsync(request);
                    if (!response.IsSuccessStatusCode && !(response.StatusCode == HttpStatusCode.NotFound && request.Method == HttpMethod.Get) && (forwardExceptions || --retries > 0))
                        await ForwardException(url, request.Method, response);
                }
            });
            return response;
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and returns a deserialized HttpResponseMessage. This was built for HttpClient methods such as HttpClient.GetAsync();
        /// </summary>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message. Default true.</param>
        /// <returns>A Task{TResult}, where the instance of TResult is instantiated by deserializing the string result of the HttpClient method us JsonConvert.DeserializeObject.</returns>
        public async Task<TResult> RunAndDeserialize<TResult>(HttpMethod method, string url, bool forwardExceptions = true)
        {
            var jsonResult = await Run(method, url, forwardExceptions);
            try { return jsonResult == null ? default(TResult) : JsonConvert.DeserializeObject<TResult>(jsonResult); }
            catch { return default(TResult); };
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and returns HttpResponseMessage. This was built for HttpClient methods such as HttpClient.GetAsync();
        /// </summary>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message. Default true.</param>
        /// <returns>A Task{string}, where string is the actually result of the HttpClient method.</returns>
        public async Task<string> Run(HttpMethod method, string url, bool forwardExceptions = true)
        {
            HttpResponseMessage response = await GetResponse(method, url, forwardExceptions);
            return response == null ? null : await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and returns HttpResponseMessage. This was built for HttpClient methods such as HttpClient.GetAsync();
        /// </summary>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message. Default true.</param>
        /// <returns>A Task{stream}, where stream is the actually result of the HttpClient method.</returns>
        public async Task<Stream> RunAndReturnStream(HttpMethod method, string url, bool forwardExceptions = true)
        {
            HttpResponseMessage response = await GetResponse(method, url, forwardExceptions);
            return response == null ? null : await response.Content.ReadAsStreamAsync();
        }
        #endregion

        #region Send With Content (POST, PATCH, PUT)

        internal async Task<HttpResponseMessage> GetResponse(HttpMethod method, string url, HttpContent content, bool forwardExceptions = true)
        {
            if (method is null) { throw new ArgumentNullException(nameof(method)); }
            if (string.IsNullOrWhiteSpace(url)) { throw new ArgumentException($"The {nameof(url)} cannot be null, empty, or whitespace.", nameof(url)); }
            // We can't retry here as HttpContent can only be used once and it can't be cloned easily.
            HttpResponseMessage response = null;
            using (var request = new HttpRequestMessage(method, url))
            {
                var contentType = content.Headers.ContentType.MediaType;
                var contentStream = await content.ReadAsStreamAsync();
                contentStream.Position = 0;
                var storedStream = new MemoryStream();
                await contentStream.CopyToAsync(storedStream);
                contentStream.Position = 0;
                storedStream.Position = 0;
                if (content != null)
                    request.Content = content;
                AddTokens(request);
                var httpClient = _HttpClientFactory.GetHttpClient();
                response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode && !(response.StatusCode == HttpStatusCode.NotFound && request.Method == HttpMethod.Get) && forwardExceptions)
                {
                    await ForwardException(url, request.Method, response, storedStream, contentType);
                }
            }

            return response;
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and POST content and returns HttpResponseMessage. This was built for HttpClient methods such as HttpClient.PostAsync(content);
        /// </summary>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="content">The content to post as an HttpContent.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message.</param>
        /// <returns>A Task{string}, where string is the actually result of the HttpClient method.</returns>
        /// <remarks>This handles string specifically, not as a generic.</remarks>
        public async Task<string> Run(HttpMethod method, string url, HttpContent content, bool forwardExceptions = true)
        {
            // We can't retry here as HttpContent can only be used once and it can't be cloned easily.
            HttpResponseMessage response = await GetResponse(method, url, content, forwardExceptions);
            return response == null ? null : await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and POST content and returns HttpResponseMessage. This was built for HttpClient methods such as HttpClient.PostAsync(content);
        /// </summary>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="content">The content to post as an HttpContent.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message. Default true.</param>
        /// <returns>A Task{stream}, where string is the actually result of the HttpClient method.</returns>
        /// <remarks>This handles stream specifically, not as a generic.</remarks>
        public async Task<Stream> RunAndReturnStream(HttpMethod method, string url, HttpContent content, bool forwardExceptions = true)
        {
            // We can't retry here as HttpContent can only be used once and it can't be cloned easily.
            HttpResponseMessage response = await GetResponse(method, url, content, forwardExceptions);
            return response == null ? null : await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and POST content and returns a deserialized HttpResponseMessage. This was built for HttpClient methods such as HttpClient.PostAsync(content);
        /// </summary>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="content">The content to post as an HttpContent.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message. Default true.</param>
        /// <returns>A Task{string}, where string is the actually result of the HttpClient method.</returns>
        public async Task<TResult> RunAndDeserialize<TResult>(HttpMethod method, string url, HttpContent content, bool forwardExceptions = true)
        {
            var jsonResult = await Run(method, url, content, forwardExceptions);
            return jsonResult == null ? default(TResult) : JsonConvert.DeserializeObject<TResult>(jsonResult);
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and POST content that is not yet in HttpContent form and returns a deserialized HttpResponseMessage. This was built for HttpClient methods such as HttpClient.PostAsync(content);
        /// </summary>
        /// <typeparam name="TContent">The type to input, convert to json and send as HttpContent.</typeparam>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="content">The content to post.</param>
        /// <param name="settings">Json serializer settings.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message. Default true.</param>
        /// <returns>A string, where the string is the actually result of the HttpClient method.</returns>
        /// <remarks>This handles string specifically, not as a generic, though content is generic, and doesn't deserialize the string.</remarks>
        public async Task<string> Run<TContent>(HttpMethod method, string url, TContent content, JsonSerializerSettings settings = null, bool forwardExceptions = true)
        {
            // This retry could affect POSTs. If the call fails after updating the repo but before the reply is received,
            // then this retry will cause the data to be entered in three times.
            // We could fix this by giving all POST calls a unique identifier, and then storing that unique identifier in the database
            // and associating it with an entity id. Then a retry would return the entity with that id. Load balancing requires the unique
            // identifier be stored in a location all load-balanced servers have access to.
            var jsonResult = await new Retry().RetryAsync(async () =>
            {
                HttpContent requestContent = ConvertToHttpContent(content, settings);
                return await Run(method, url, requestContent, forwardExceptions);
            });
            return jsonResult;
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and POST content that is not yet in HttpContent form and returns a deserialized HttpResponseMessage. This was built for HttpClient methods such as HttpClient.PostAsync(content);
        /// </summary>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="content">The content to post.</param>
        /// <param name="settings">Json serializer settings.</param>
        /// <param name="forwardExceptions">Whether or not we want the exception forwarded on to be displayed as a service error response message. Default true.</param>
        /// <returns>A Task{string}, where string is the actually result of the HttpClient method.</returns>
        public async Task<TResult> RunAndDeserialize<T1, TResult>(HttpMethod method, string url, T1 content, JsonSerializerSettings settings = null, bool forwardExceptions = true)
        {
            // This retry could affect POSTs. If the call fails after updating the repo but before the reply is received,
            // then this retry will cause the data to be entered in three times.
            // We could fix this by giving all POST calls a unique identifier, and then storing that unique identifier in the database
            // and associating it with an entity id. Then a retry would return the entity with that id. Load balancing requires the unique
            // identifier be stored in a location all load-balanced servers have access to.
            return await new Retry().RetryAsync(async () =>
            {
                HttpContent postContent = ConvertToHttpContent(content, settings);
                return await RunAndDeserialize<TResult>(method, url, postContent, forwardExceptions);
            });
        }

        /// <summary>
        /// Converts an object to the appropriately derived type that inherties from HttpContent.
        /// StringContent: string, JObject, JArray, JToken. Non defined objects are Serialized with
        /// to Json and added to a <see cref="JsonContent"/>.
        /// StreamContent: Stream (or objects deriving from Stream)        
        /// </summary>
        /// <typeparam name="T1">The Type of the content.</typeparam>
        /// <param name="content">The content object.</param>
        /// <param name="settings">JsonSerializer settings.</param>
        /// <returns>An appropriately derived type that inherties from HttpContent.</returns>
        public HttpContent ConvertToHttpContent<T1>(T1 content, JsonSerializerSettings settings)
        {
            if (content is string)
            {
                var str = content.ToString();
                // If it isn't quoted and it isn't JSON, quote it
                if (!str.StartsWithAny("{", "[", "\""))
                    str = str.Quote();
                return new JsonContent(str);
            }
            if (content is JObject || content is JArray || content is JToken)
            {
                return new JsonContent(content.ToString());
            }
            var stream = content as Stream;
            if (stream != null)
                return new StreamContent(stream);
            return new JsonContent(JsonConvert.SerializeObject(content, settings));
        }

        #endregion

        private static async Task ForwardException(string url, HttpMethod method, HttpResponseMessage response, Stream stream = null, string contentType = null)
        {
            var responseText = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseText))
            {
                var serviceErrorResponse = new ServiceErrorResponse
                {
                    Message = $"{response.ReasonPhrase}",
                    Source = $"Method: {method}, Url: {url}{Environment.NewLine}, Content-Type: {contentType}, Content: {stream?.AsString()}",
                    HResult = (int)response.StatusCode
                };
                throw new ServiceErrorResponseForwarderException(serviceErrorResponse, response.StatusCode);
            }
            throw new ServiceErrorResponseForwarderException(responseText, response.StatusCode);
        }

        protected internal virtual void AddTokens(HttpRequestMessage request)
        {
            // No headers, so nothing to add
        }
    }
}