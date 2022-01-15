using Autofac;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class AuthenticationClientFactory : IAuthenticationClientFactory
    {
        private readonly ILifetimeScope _LifetimeScope;

        public AuthenticationClientFactory(ILifetimeScope lifetimeScope)
        {
            _LifetimeScope = lifetimeScope;
        }

        public IAuthenticationClient Create(string serviceUrl = null)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl))
                return _LifetimeScope.Resolve<IAuthenticationClient>();

            var settings = new AuthenticationSettings(serviceUrl);
            return new AuthenticationClient(settings, new HttpClientRunnerNoHeaders(HttpClientFactory.Instance));
        }
    }
}