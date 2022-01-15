using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using System.Text;

namespace Rhyous.EntityAnywhere.Services
{
    partial class BasicAuth : IBasicAuth
    {
        private readonly IHeaders _Headers;
        private readonly IBasicAuthEncoder _BasicAuthEncoder;

        public BasicAuth(IHeaders headers,
                                    IBasicAuthEncoder basicAuthEncoder)
        {
            _Headers = headers;
            _BasicAuthEncoder = basicAuthEncoder;
        }

        public Credentials Credentials => ReadBasicAuthCredentials();

        public string HeaderValue => _Headers?.Collection.Get<string>("Authorization", null);

        private Credentials ReadBasicAuthCredentials()
        {
            if (string.IsNullOrWhiteSpace(HeaderValue))
                return null;
            return _BasicAuthEncoder.Decode(HeaderValue, Encoding.UTF8);
        }
    }
}