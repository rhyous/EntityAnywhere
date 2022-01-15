using Newtonsoft.Json;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class AuthenticationClient : IAuthenticationClient
    {
        private readonly IAuthenticationSettings _AuthenticationSettings;
        protected readonly IHttpClientRunnerNoHeaders _HttpClientRunner;

        public AuthenticationClient(IAuthenticationSettings authenticationSettings,
                                    IHttpClientRunnerNoHeaders httpClientRunner)
        {
            _AuthenticationSettings = authenticationSettings ?? throw new ArgumentNullException(nameof(authenticationSettings));
            _HttpClientRunner = httpClientRunner ?? throw new ArgumentNullException(nameof(httpClientRunner));
        }

        public async Task<IToken> AuthenticateAsync(string user, string password) 
            => await AuthenticateAsync(new Credentials { User = user, Password = password });

        public async Task<IToken> AuthenticateAsync(ICredentials credentials)
        {
            if (credentials is null) { throw new ArgumentNullException(nameof(credentials)); }
            if (string.IsNullOrWhiteSpace(credentials.User)) { throw new ArgumentException($"'{nameof(credentials)}'.{nameof(Credentials.User)} cannot be null or whitespace.", nameof(credentials)); }
            if (string.IsNullOrWhiteSpace(credentials.Password)) { throw new ArgumentException($"'{nameof(credentials)}'.{nameof(Credentials.Password)} cannot be null or whitespace.", nameof(credentials)); }
            var json = JsonConvert.SerializeObject(credentials);
            var content = new StringContent(json);
            var actionUrl = StringConcat.WithSeparator('/', _AuthenticationSettings.ServiceUrl, _AuthenticationSettings.Action);
            return await _HttpClientRunner.RunAndDeserialize<Token>(HttpMethod.Post, actionUrl, content, true);
        }
    }
}