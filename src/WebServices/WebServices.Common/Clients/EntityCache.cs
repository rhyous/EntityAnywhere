using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public class EntityCache<T, TId> : IEntityCache<T, TId>
        where T : IId<TId>
    {
        public Dictionary<TId, T> Cache { get; set; }
        public bool UseCache { get; set; }
        public HttpClient HttpClient { get; set; }
        public async Task<T> GetWithCache(TId id, Func<string, Task<HttpResponseMessage>> methodIfNotCached, string url)
        {
            if (UseCache && Cache.TryGetValue(id, out T e))
                return e;
            else
                return await UpdateCache(methodIfNotCached, url);
        }

        public async Task<List<T>> GetWithCache(IEnumerable<TId> ids, Func<string, HttpContent, Task<HttpResponseMessage>> methodIfNotCached, string url)
        {
            if (UseCache)
            {
                if (Cache.Keys.Except(ids).Any())
                    return await UpdateCache(ids, methodIfNotCached, url);
                return ids.Where(Cache.ContainsKey).Select(id => Cache[id]).ToList();
            }
            else
                return await UpdateCache(ids, methodIfNotCached, url);
        }

        internal async Task<T> UpdateCache(Func<string, Task<HttpResponseMessage>> methodIfNotCached, string url)
        {
            var e = await HttpClientRunner.RunAndDeserialize<T>(methodIfNotCached, url);
            if (e != null)
                Cache[e.Id] = e;
            return e;
        }

        internal async Task<List<T>> UpdateCache(IEnumerable<TId> ids, Func<string, HttpContent, Task<HttpResponseMessage>> methodIfNotCached, string url)
        {
            if (ids == null && !ids.Any())
                return null;
            var list = await HttpClientRunner.RunAndDeserialize<IEnumerable<TId>, List<T>>(methodIfNotCached, url, ids);
            if (list != null && list.Count > 0)
                list.ForEach(e => Cache[e.Id] = e);
            return list;
        }
    }
}