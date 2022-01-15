using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly ConcurrentDictionary<string, IHttpClient> HttpClients;

        #region Singleton

        private static readonly Lazy<HttpClientFactory> Lazy = new Lazy<HttpClientFactory>(() => new HttpClientFactory());

        public static HttpClientFactory Instance { get { return Lazy.Value; } }

        internal HttpClientFactory()
        {
            SingleHttpClient = new HttpClientWrapper(new HttpClient());
            HttpClients = new ConcurrentDictionary<string, IHttpClient>();
        }

        #endregion

        public IHttpClient SingleHttpClient { get; }

        public IHttpClient GetHttpClient(string baseAddress = null)
        {
            if (string.IsNullOrWhiteSpace(baseAddress))
                return SingleHttpClient;
            if (HttpClients.TryGetValue(baseAddress, out IHttpClient client))
                return client;
            var httpClient = new HttpClientWrapper(new HttpClient { BaseAddress = new Uri(baseAddress) });
            HttpClients.TryAdd(baseAddress, httpClient);
            return httpClient;
        }
    }
}
