using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public interface IEntityCache<T, TId>
    {
        bool UseCache { get; set; }
        Dictionary<TId, T> Cache { get; set; }
        Task<T> GetWithCache(TId id, Func<string, Task<HttpResponseMessage>> methodIfNotCached, string url);
        Task<List<T>> GetWithCache(IEnumerable<TId> ids, Func<string, HttpContent, Task<HttpResponseMessage>> methodIfNotCached, string url);
    }
}
