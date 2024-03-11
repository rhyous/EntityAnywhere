namespace Rhyous.EntityAnywhere.Clients2
{
    public class AuthenticationSettings : IAuthenticationSettings
    {
        private const string AuthenticationService = nameof(AuthenticationService);
        private readonly IEntityClientConfig _EntityClientConfig;
        private string _ServiceUrl;

        public AuthenticationSettings(IEntityClientConfig entityClientConfig)
        {
            _EntityClientConfig = entityClientConfig;
        }

        public AuthenticationSettings(string url) => ServiceUrl = url;

        /// <summary>
        /// The Service Url, without the verb.
        /// </summary>
        /// <example>Https://eaf.domain.tld/Api/AuthenticationService</example>
        public string ServiceUrl
        {
            get => _ServiceUrl ?? (_ServiceUrl = _EntityClientConfig?.GetServiceUrl(AuthenticationService));
            set => _ServiceUrl = value;
        }

        public string Action => "Authenticate";
    }
}