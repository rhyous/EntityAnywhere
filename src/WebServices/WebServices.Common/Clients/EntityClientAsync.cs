using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public class EntityClientAsync<TEntity, TId> : EntityClientBase, IEntityClientAsync<TEntity, TId>
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        #region Constructors
        public EntityClientAsync()
        {
        }

        public EntityClientAsync(HttpClient httpClient, bool useMicrosoftDateFormat = false) 
            : this(useMicrosoftDateFormat ? new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat } : null)
        {
            _HttpClient = httpClient;
        }

        public EntityClientAsync(bool useMicrosoftDateFormat) 
            : this(useMicrosoftDateFormat ? new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat } : null)
        {
        }

        public EntityClientAsync(JsonSerializerSettings jsonSerializerSettings)
        {
            JsonSerializerSettings = jsonSerializerSettings;
        }
        #endregion

        /// <inheritdoc />
        public override string Entity => typeof(TEntity).Name;

        public IEntityCache<TEntity, TId> EntityCache
        {
            get { return _EntityCache ?? (_EntityCache = new EntityCache<TEntity, TId>()); }
            set { _EntityCache = value; }
        } private IEntityCache<TEntity, TId> _EntityCache;

        /// <inheritdoc />
        public virtual async Task<bool> DeleteAsync(string id)
        {
            return await HttpClientRunner.RunAndDeserialize<bool>(HttpClient.DeleteAsync, $"{ServiceUrl}/{EntityPluralized}({id})");
        }

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> GetAsync(string idOrName)
        {
            var id = idOrName.To<TId>();
            if (id.Equals(default(TId)))
            {
                var urlEncodedAlternateId = WebUtility.UrlEncode(idOrName);
                return await HttpClientRunner.RunAndDeserialize<OdataObject<TEntity, TId>>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({urlEncodedAlternateId})");
            }
            return await EntityCache.GetWithCache(id, HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({idOrName})");
        }

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> GetByAlternateKeyAsync(string altKey)
        {
            var urlEncodedAlternateId = WebUtility.UrlEncode(altKey);
            return await HttpClientRunner.RunAndDeserialize<OdataObject<TEntity, TId>>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({urlEncodedAlternateId})");
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetAllAsync()
        {
            return await HttpClientRunner.RunAndDeserialize<OdataObjectCollection<TEntity, TId>>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}");
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByCustomUrlAsync(string urlPart)
        {
            return await HttpClientRunner.RunAndDeserialize<OdataObjectCollection<TEntity, TId>>(HttpClient.GetAsync, $"{ServiceUrl}/{urlPart}");
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByCustomUrlAsync(string urlPart, Func<string, HttpContent, Task<HttpResponseMessage>> httpMethod, HttpContent content)
        {
            return await HttpClientRunner.RunAndDeserialize<OdataObjectCollection<TEntity, TId>>(httpMethod, $"{ServiceUrl}/{urlPart}", content);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByCustomUrlAsync(string urlPart, Func<string, HttpContent, Task<HttpResponseMessage>> httpMethod, object content)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(content, JsonSerializerSettings), Encoding.UTF8, "application/json");
            return await HttpClientRunner.RunAndDeserialize<OdataObjectCollection<TEntity, TId>>(httpMethod, $"{ServiceUrl}/{urlPart}", postContent);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByQueryParametersAsync(string queryParameters)
        {
            return await HttpClientRunner.RunAndDeserialize<OdataObjectCollection<TEntity, TId>>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}?{queryParameters}");
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByIdsAsync(IEnumerable<TId> ids)
        {
            return await EntityCache.GetWithCache(ids, HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}/Ids");
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByPropertyValuesAsync(string property, IEnumerable<string> values)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(values, JsonSerializerSettings), Encoding.UTF8, "application/json");
            return await HttpClientRunner.RunAndDeserialize<OdataObjectCollection<TEntity, TId>>(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}/{property}/Values", postContent);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByIdsAsync(List<TId> ids)
        {
            return await GetByIdsAsync((IEnumerable<TId>)ids);
        }

        /// <inheritdoc />
        public virtual async Task<string> GetPropertyAsync(string id, string property)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({id})/{property}");
        }

        /// <inheritdoc />
        public virtual async Task<CsdlEntity<TEntity>> GetMetadataAsync()
        {
            return await HttpClientRunner.RunAndDeserialize<CsdlEntity<TEntity>>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}/$Metadata");
        }

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> PatchAsync(string id, PatchedEntity<TEntity> patchedEntity)
        {
            return await HttpClientRunner.RunAndDeserialize<PatchedEntity<TEntity>, OdataObject<TEntity, TId>>(HttpClient.PatchAsync, $"{ServiceUrl}/{EntityPluralized}({id})", patchedEntity, JsonSerializerSettings);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> PostAsync(List<TEntity> entities)
        {
            return await HttpClientRunner.RunAndDeserialize<List<TEntity>, OdataObjectCollection<TEntity, TId>>(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}", entities, JsonSerializerSettings);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> PutAsync(string id, TEntity entity)
        {
            return await HttpClientRunner.RunAndDeserialize<TEntity, OdataObject<TEntity, TId>>(HttpClient.PutAsync, $"{ServiceUrl}/{EntityPluralized}({id})", entity, JsonSerializerSettings);
        }

        public virtual async Task<string> UpdatePropertyAsync(string id, string property, string value)
        {
            return await HttpClientRunner.RunAndDeserialize<string, string>(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}({id})/{property}", value, JsonSerializerSettings);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<Addendum, long>> GetAddendaAsync(string id)
        {
            return await HttpClientRunner.RunAndDeserialize<OdataObjectCollection<Addendum, long>>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({id})/Addenda");
        }

        /// <inheritdoc />
        public virtual async Task<Addendum> GetAddendaByNameAsync(string id, string name)
        {
            return await HttpClientRunner.RunAndDeserialize<Addendum>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({id})/Addenda({name})");
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<Addendum, long>> GetAddendaByEntityIdsAsync(List<string> ids)
        {
            return await HttpClientRunner.RunAndDeserialize<List<string>, OdataObjectCollection<Addendum, long>>(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}/Ids/Addenda", ids);
        }

        public virtual async Task<int> GetCountAsync()
        {
            return await HttpClientRunner.RunAndDeserialize<int>(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}?$count");
        }
    }
}