using Microsoft.AspNetCore.Http;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Security
{
    /// <summary>Extenion methods for IHeaderValidator.</summary>
    public static class HeaderValidatorExtensions
    {
        /// <summary>Checks if a HeaderValidator can validate the given header.
        /// If IValidator.Headers contains a header in the provided headerKeys, it can.</summary>
        public static bool CanValidateHeaders(this IHeaderValidator validator, IEnumerable<string> headerKeys)
        {
            var headers = validator.Headers;
            return validator != null 
                && (headers == null 
                || !headers.Any() 
                || (headerKeys != null && headerKeys.Intersect(headers, StringComparer.OrdinalIgnoreCase).Any()));
        }

        /// <summary>Converts a <see cref="IHeaderDictionary"/> to a NameValueCollection.</summary>
        public static NameValueCollection ToNameValueCollection(this IHeaderDictionary headerDictionary)
        {
            var nvc = new NameValueCollection();
            foreach (var kvp in headerDictionary)
            {
                if (kvp.Value.Any())
                    nvc.Add(kvp.Key.ToString(), string.Join(",", kvp.Value.Where(v => !string.IsNullOrWhiteSpace(v))));
                else
                    nvc.Add(kvp.Key.ToString(), null);
            }
            return nvc;
        }
    }
}