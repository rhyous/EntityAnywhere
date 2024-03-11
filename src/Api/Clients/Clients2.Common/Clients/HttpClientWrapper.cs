using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    /// <summary>
    /// HttpClient wasn't written with an interface. This wrapper implements 
    /// IHttpClient which has the same signature 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HttpClientWrapper : IHttpClient
    {
        internal HttpClient HttpClient;

        #region Constructors
        public HttpClientWrapper(HttpClient httpClient) => HttpClient = httpClient;

        #endregion

        #region Properties
        public Uri BaseAddress { get => HttpClient.BaseAddress; set => HttpClient.BaseAddress = value; }

        public HttpRequestHeaders DefaultRequestHeaders => HttpClient.DefaultRequestHeaders;

        public TimeSpan Timeout { get => HttpClient.Timeout; set => HttpClient.Timeout = value; }
        public long MaxResponseContentBufferSize { get => HttpClient.MaxResponseContentBufferSize; set => HttpClient.MaxResponseContentBufferSize = value; }
        #endregion

        public void CancelPendingRequests() => HttpClient.CancelPendingRequests();

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri) => await HttpClient.DeleteAsync(requestUri);

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken) => await HttpClient.DeleteAsync(requestUri, cancellationToken);

        public async Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken) => await HttpClient.DeleteAsync(requestUri, cancellationToken);

        public async Task<HttpResponseMessage> DeleteAsync(Uri requestUri) => await HttpClient.DeleteAsync(requestUri);

        public async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) => await HttpClient.GetAsync(requestUri, completionOption, cancellationToken);

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken) => await HttpClient.GetAsync(requestUri, cancellationToken);

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) => await HttpClient.GetAsync(requestUri, completionOption, cancellationToken);

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption) => await HttpClient.GetAsync(requestUri, completionOption);

        public async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption) => await HttpClient.GetAsync(requestUri, completionOption);

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri) => await HttpClient.GetAsync(requestUri);

        public async Task<HttpResponseMessage> GetAsync(string requestUri) => await HttpClient.GetAsync(requestUri);

        public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken) => await HttpClient.GetAsync(requestUri, cancellationToken);

        public async Task<byte[]> GetByteArrayAsync(string requestUri) => await HttpClient.GetByteArrayAsync(requestUri);

        public async Task<byte[]> GetByteArrayAsync(Uri requestUri) => await HttpClient.GetByteArrayAsync(requestUri);

        public async Task<Stream> GetStreamAsync(Uri requestUri) => await HttpClient.GetStreamAsync(requestUri);

        public async Task<Stream> GetStreamAsync(string requestUri) => await HttpClient.GetStreamAsync(requestUri);

        public async Task<string> GetStringAsync(string requestUri) => await HttpClient.GetStringAsync(requestUri);

        public async Task<string> GetStringAsync(Uri requestUri) => await HttpClient.GetStringAsync(requestUri);

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken) => await HttpClient.PostAsync(requestUri, content, cancellationToken);

        public async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken) => await HttpClient.PostAsync(requestUri, content, cancellationToken);

        public async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content) => await HttpClient.PostAsync(requestUri, content);

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content) => await HttpClient.PostAsync(requestUri, content);

        public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken) => await HttpClient.PutAsync(requestUri, content, cancellationToken);

        public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content) => await HttpClient.PutAsync(requestUri, content);

        public async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken) => await HttpClient.PutAsync(requestUri, content, cancellationToken);

        public async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content) => await HttpClient.PutAsync(requestUri, content);

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) => await HttpClient.SendAsync(request);

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => await HttpClient.SendAsync(request, cancellationToken);

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption) => await HttpClient.SendAsync(request, completionOption);

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) => await HttpClient.SendAsync(request, completionOption, cancellationToken);

        #region Custom code

        /// <summary>
        /// Send a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// 
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1"/>.The task object representing the asynchronous operation.
        /// </returns>
        /// <param name="client">The instantiated Http Client <see cref="HttpClient"/></param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="client"/> was null.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri"/> was null.</exception>
        public Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content)
        {
            return PatchAsync(CreateUri(requestUri), content);
        }

        /// <summary>
        /// Send a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// 
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1"/>.The task object representing the asynchronous operation.
        /// </returns>
        /// <param name="client">The instantiated Http Client <see cref="HttpClient"/></param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="client"/> was null.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri"/> was null.</exception>
        public Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content)
        {
            return PatchAsync(requestUri, content, CancellationToken.None);
        }
        /// <summary>
        /// Send a PATCH request with a cancellation token as an asynchronous operation.
        /// </summary>
        /// 
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1"/>.The task object representing the asynchronous operation.
        /// </returns>
        /// <param name="client">The instantiated Http Client <see cref="HttpClient"/></param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="client"/> was null.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri"/> was null.</exception>
        public Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return PatchAsync(CreateUri(requestUri), content, cancellationToken);
        }

        /// <summary>
        /// Send a PATCH request with a cancellation token as an asynchronous operation.
        /// </summary>
        /// 
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1"/>.The task object representing the asynchronous operation.
        /// </returns>
        /// <param name="client">The instantiated Http Client <see cref="HttpClient"/></param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="client"/> was null.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri"/> was null.</exception>
        public Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            var message = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = content };
            return SendAsync(message, cancellationToken);
        }

        internal static Uri CreateUri(string uri)
        {
            return string.IsNullOrEmpty(uri) ? null : new Uri(uri, UriKind.RelativeOrAbsolute);
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    HttpClient.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
