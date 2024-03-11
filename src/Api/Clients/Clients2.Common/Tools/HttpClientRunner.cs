using Rhyous.EntityAnywhere.Interfaces;
using System.Net.Http;

namespace Rhyous.EntityAnywhere.Clients2
{
    /// <summary>
    /// This class is used by EntityClient to run common HttpClient commands asynchronously. Whether calling GET, POST, PUT, PATCH, DELETE, etc, the way to make the call and get the response back is standardized.
    /// </summary>
    public class HttpClientRunner : HttpClientRunnerNoHeaders, IHttpClientRunner
    {
        private readonly IHeaders _Headers;
        private static string[] TokenKeys = new[] { "EntityAdminToken", "Token" }; // order is important

        public HttpClientRunner(IHttpClientFactory httpClientFactory, IHeaders headers) : base(httpClientFactory)
        {
            _Headers = headers;
        }

        protected internal override void AddTokens(HttpRequestMessage request)
        {
            // If Token or EntityAdminToken exist, add them to the subsequent request
            foreach (var headerKey in TokenKeys)
            {
                var value = _Headers?.Collection?.Get(headerKey)?.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    request.Headers.Add(headerKey, value);
                    break; // If we add EntityAdminToken, then break
                }
            }
        }
    }
}