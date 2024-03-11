using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>Extensions methods for IHeaderDictionary.</summary>
    public static class HeaderDictionaryExtensions
    {
        /// <summary>Converts an IHeaderDictionary to a NameValueCollection.</summary>
        public static NameValueCollection ToNameValueCollection(this IHeaderDictionary headerDictionary)
        {
            var nvc = new NameValueCollection();
            foreach (var kvp in headerDictionary)
            {
                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    nvc.Add(kvp.Key.ToString(), kvp.Value);
                }
            }
            return nvc;
        }
    }
}
