using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Rhyous.StringLibrary;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class AuthorizationClient : IAuthorizationClient
    {
        private readonly IEntityClientConfig _EntityClientConfig;
        protected readonly IHttpClientRunner _HttpClientRunner;
        private const string AuthorizationService = nameof(AuthorizationService);
        private const string MyRoleData = nameof(MyRoleData);

        public AuthorizationClient(IEntityClientConfig entityClientConfig,
                                   IHttpClientRunner httpClientRunner)
        {
            _EntityClientConfig = entityClientConfig;
            _HttpClientRunner = httpClientRunner;
        }

        internal AuthorizationClient(string serviceUrl,
                                     IHttpClientRunner httpClientRunner)
        {
            _ServiceUrl = serviceUrl;
            _HttpClientRunner = httpClientRunner;
        }

        internal string ServiceUrl
        {
            get => _ServiceUrl ?? (_ServiceUrl = _EntityClientConfig.GetServiceUrl(AuthorizationService));
            set => _ServiceUrl = value;
        } private string _ServiceUrl;

        public async Task<UserRoleEntityData> GetRoleDataAsync() 
            => await _HttpClientRunner.RunAndDeserialize<UserRoleEntityData>(HttpMethod.Get, StringConcat.WithSeparator('/', ServiceUrl, MyRoleData), true);

    }
}