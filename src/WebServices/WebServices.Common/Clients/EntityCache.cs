using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Rhyous.Odata;

namespace Rhyous.WebFramework.Clients
{
    /// <summary>
    /// This object exists to help eliminate make the same web service call multiple times.
    /// </summary>
    /// <typeparam name="T">The Type to cache.</typeparam>
    /// <typeparam name="TId">The type of the Id property of the type T to cache.</typeparam>
    public class EntityCache<T, TId> : IEntityCache<T, TId>
        where T : class
    {
        public static string Id = "Id";        
        public bool UseCache { get; set; }
        public HttpClient HttpClient { get; set; }

        public Dictionary<TId, OdataObject<T, TId>> Cache
        {
            get { return _Cache ?? (_Cache = new Dictionary<TId, OdataObject<T, TId>>()); }
            set { _Cache = value; }
        } private Dictionary<TId, OdataObject<T, TId>> _Cache;

        public async Task<OdataObject<T, TId>> GetWithCache(TId id, Func<string, Task<HttpResponseMessage>> methodIfNotCached, string url)
        {
            if (UseCache && Cache.TryGetValue(id, out OdataObject<T, TId> e))
                return e;
            else
                return await UpdateCache(methodIfNotCached, url);
        }

        public async Task<OdataObjectCollection<T, TId>> GetWithCache(IEnumerable<TId> ids, Func<string, HttpContent, Task<HttpResponseMessage>> methodIfNotCached, string url)
        {
            if (UseCache)
            {
                if (Cache.Keys.Except(ids).Any())
                    return await UpdateCache(ids, methodIfNotCached, url);
                var collection = new OdataObjectCollection<T, TId>();
                collection.AddRange(ids.Where(Cache.ContainsKey).Select(id => Cache[id]));
                return collection;
            }
            else
                return await UpdateCache(ids, methodIfNotCached, url);
        }

        internal async Task<OdataObject<T, TId>> UpdateCache(Func<string, Task<HttpResponseMessage>> methodIfNotCached, string url)
        {
            OdataObject<T, TId> e = await HttpClientRunner.RunAndDeserialize<OdataObject<T,TId>>(methodIfNotCached, url);
            if (e != null)
            {
                
                var key = (TId)e.GetType().GetProperty(Id).GetValue(e);
                Cache[key] = e;
            }
            return e;
        }

        internal async Task<OdataObjectCollection<T, TId>> UpdateCache(IEnumerable<TId> ids, Func<string, HttpContent, Task<HttpResponseMessage>> methodIfNotCached, string url)
        {
            if (ids == null && !ids.Any())
                return null;
            var list = await HttpClientRunner.RunAndDeserialize<IEnumerable<TId>, OdataObjectCollection<T, TId>>(methodIfNotCached, url, ids);
            if (list != null && list.Count > 0)
            {
                foreach (var e in list)
                {
                    var key = (TId)e.GetType().GetProperty(Id).GetValue(e);
                    Cache[key] = e;
                }
            }
            return list;
        }
    }
}