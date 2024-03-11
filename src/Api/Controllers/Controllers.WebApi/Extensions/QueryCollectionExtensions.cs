using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.WebApi
{
    public static class QueryCollectionExtensions
    {
        public static NameValueCollection ToNameValueCollection(this IQueryCollection queryCollection)
        {
            var nvc = new NameValueCollection();
            foreach (var kvp in queryCollection)
            {
                foreach (var value in kvp.Value)
                {
                    var key = kvp.Key.ToString();
                    if (!string.IsNullOrEmpty(value))
                        nvc.Add(key, kvp.Value);
                }
            }
            return nvc;
        }
    }
}
