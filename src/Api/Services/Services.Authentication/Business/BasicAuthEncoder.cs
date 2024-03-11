using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Text;

namespace Rhyous.EntityAnywhere.Services
{
    public class BasicAuthEncoder : IBasicAuthEncoder
    {
        private const string Prefix = "Basic ";

        public Credentials Decode(string encodedHeader, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(encodedHeader)) { throw new ArgumentException($"'{nameof(encodedHeader)}' cannot be null or whitespace.", nameof(encodedHeader)); }
            if (encoding is null) { throw new ArgumentNullException(nameof(encoding)); }

            var decodedHeader = encodedHeader.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase)
                ? encodedHeader.Substring(Prefix.Length).Base64Decode(encoding)
                : encodedHeader.Base64Decode(encoding);
            var credArray = decodedHeader.Split(':');
            return new Credentials { User = credArray[0], Password = credArray[1] };
        }

        public string Encode(string user, string password, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(user)) { throw new ArgumentException($"'{nameof(user)}' cannot be null or whitespace.", nameof(user)); }
            if (string.IsNullOrEmpty(password)) { throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password)); }
            if (encoding is null) { throw new ArgumentNullException(nameof(encoding)); }

            return Prefix + $"{user}:{password}".Base64Encode();
        }
    }
}