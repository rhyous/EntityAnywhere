using Rhyous.EntityAnywhere.Exceptions;
using System;
using System.Collections.Specialized;
using System.Net;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public static class HeaderExtensions
    {
        public static void UpdateValue(this NameValueCollection headers, string key, string trustedvalue)
        {
            if (headers is null) { throw new ArgumentNullException(nameof(headers)); }
            if (string.IsNullOrWhiteSpace(key)) { throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key)); }

            var untrustedHeaderValue = headers.Get(key);
            headers.Remove(key); // Always remove untrusted header
            if (string.IsNullOrWhiteSpace(trustedvalue))
                return;
            if (!string.IsNullOrWhiteSpace(untrustedHeaderValue) && untrustedHeaderValue != trustedvalue)
                throw new RestException(HttpStatusCode.Forbidden); // Assume a hack
            headers.Add(key, trustedvalue);
        }
    }
}