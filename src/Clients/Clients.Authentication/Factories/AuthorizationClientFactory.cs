using Autofac;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class AuthorizationClientFactory : IAuthorizationClientFactory
    {
        private readonly ILifetimeScope _LifetimeScope;

        public AuthorizationClientFactory(ILifetimeScope lifetimeScope)
        {
            _LifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Creates an instance of an IAuthorizationClient.. 
        /// </summary>
        /// <param name="token">The token. Optional. If using dependency injection, this can be omitted.</param>
        /// <param name="serviceUrl">The url to the service, example: https://some.site.tld/subpath/AuthorizationService. Optional. If using dependency injection, this can be omitted.</param>
        /// <returns>An instance of an IAuthorizationClient.</returns>
        public IAuthorizationClient Create(string token = null, string serviceUrl = null)
        {
            // If the optional values are null, use Dependency Injection
            if (string.IsNullOrWhiteSpace(token) && string.IsNullOrWhiteSpace(serviceUrl))
                return _LifetimeScope.Resolve<IAuthorizationClient>();

            // Otherwise, use this factory
            var headers = new Headers { Collection = new NameValueCollection { { "Token", token } } };
            var httpClientRunner = new HttpClientRunner(HttpClientFactory.Instance, headers);
            return new AuthorizationClient(serviceUrl, httpClientRunner);
        }
    }
}