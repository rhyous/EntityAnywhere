using Rhyous.Odata;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public interface IEntityCache<T, TId>
    {
        bool UseCache { get; set; }
        Dictionary<TId, OdataObject<T, TId>> Cache { get; set; }
        Task<OdataObject<T, TId>> GetWithCache(TId id, Func<string, Task<HttpResponseMessage>> methodIfNotCached, string url);
        Task<OdataObjectCollection<T, TId>> GetWithCache(IEnumerable<TId> ids, Func<string, HttpContent, Task<HttpResponseMessage>> methodIfNotCached, string url);
    }
}
