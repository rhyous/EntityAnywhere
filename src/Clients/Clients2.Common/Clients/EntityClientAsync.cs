using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class EntityClientAsync<TEntity, TId> : EntityClientBase, IEntityClientAsync<TEntity, TId>
        where TEntity : class, IId<TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        protected readonly IHttpClientRunner _HttpClientRunner;

        public EntityClientAsync(IEntityClientConnectionSettings<TEntity> entityClientSettings, 
                                 IHttpClientRunner httpClientRunner) 
            : base (entityClientSettings)
        {
            _HttpClientRunner = httpClientRunner ?? throw new ArgumentNullException(nameof(httpClientRunner));
        }

        /// <inheritdoc />
        public virtual async Task<bool> DeleteAsync(TId id, bool forwardExceptions = true) => await DeleteAsync(id.ToString(), forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<bool> DeleteAsync(string id, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            return await _HttpClientRunner.RunAndDeserialize<bool>(HttpMethod.Delete, $"{ServiceUrl}/{EntityPluralized}({id})", forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<Dictionary<TId, bool>> DeleteManyAsync(IEnumerable<TId> ids, bool forwardExceptions = true)
        {
            if (ids == null || !ids.Any()) { throw new ArgumentException($"The {nameof(ids)} cannot be null, empty, or whitespace.", nameof(ids)); }
            var distinctValidIds = ids.Distinct();
            return await _HttpClientRunner.RunAndDeserialize<object, Dictionary<TId, bool>>(HttpMethod.Delete, $"{ServiceUrl}/{EntityPluralized}", distinctValidIds, _EntityClientConnectionSettings.JsonSerializerSettings, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetAllAsync(bool forwardExceptions = true)
            => await _HttpClientRunner.RunAndDeserialize<OdataObjectCollection<TEntity, TId>>(HttpMethod.Get, $"{ServiceUrl}/{EntityPluralized}", forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> GetAsync(TId id, bool forwardExceptions = true)
                        => await GetAsync(id.ToString(), null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> GetAsync(TId id, string urlParameters, bool forwardExceptions = true)
                        => await GetAsync(id.ToString(), urlParameters, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> GetAsync(string idOrName, bool forwardExceptions = true)
                        => await GetAsync(idOrName, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> GetAsync(string idOrName, string urlParameters, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(idOrName)) { throw new ArgumentException($"The {nameof(idOrName)} cannot be null, empty, or whitespace.", nameof(idOrName)); }
            if (idOrName.ToString().StartsWith(IdDisambiguator.AltKey)) // Get by Alternate Key
                return await GetByAlternateKeyAsync(idOrName, urlParameters, forwardExceptions);
            if (idOrName.ToString().StartsWith(IdDisambiguator.Alt + IdDisambiguator.Separator)) // Get by Alternate Id
                return await GetByAlternateIdAsync(idOrName, urlParameters, forwardExceptions);
            var id = idOrName.To<TId>();
            if (id.Equals(default(TId))) // If Null or 0 (or any default primitive) then try AlternateKey or AlternateId
            {
                if (typeof(TEntity).GetCustomAttributes(typeof(AlternateKeyAttribute), true).Any())
                    return await GetByAlternateKeyAsync(idOrName, urlParameters, forwardExceptions);
                if (idOrName.ToString().Contains(IdDisambiguator.Separator))
                    return await GetByAlternateIdAsync(idOrName, urlParameters, forwardExceptions);
                throw new Exception($"The value for the Id of the Entity, {typeof(TEntity).Name}, was invalid: {idOrName}");
            }
            idOrName = idOrName.Quote('\'');
            return await SendAsync<OdataObject<TEntity, TId>>(HttpMethod.Get, $"{ServiceUrl}/{EntityPluralized}({idOrName})", urlParameters, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> GetByAlternateKeyAsync(string altKey, bool forwardExceptions = true)
            => await GetByAlternateKeyAsync(altKey, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> GetByAlternateKeyAsync(string altKey, string urlParameters, bool forwardExceptions = true)
            => await GetByDisambiguatedId(altKey, IdDisambiguator.AltKey, urlParameters, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> GetByAlternateIdAsync(string altId, bool forwardExceptions = true)
            => await GetByAlternateIdAsync(altId, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> GetByAlternateIdAsync(string altId, string urlParameters, bool forwardExceptions = true)
            => await GetByDisambiguatedId(altId, $"{IdDisambiguator.Alt}{IdDisambiguator.Separator}", urlParameters, forwardExceptions);

        internal virtual async Task<OdataObject<TEntity, TId>> GetByDisambiguatedId(string altKey, string disabmiguator, string urlParameters, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(altKey)) { throw new ArgumentException($"The {nameof(altKey)} cannot be null, empty, or whitespace.", nameof(altKey)); }
            if (!altKey.StartsWith(disabmiguator, StringComparison.OrdinalIgnoreCase))
                altKey = $"{disabmiguator}{altKey}";
            var urlEncodedAlternateId = Uri.EscapeDataString(altKey).Quote('\'');
            return await SendAsync<OdataObject<TEntity, TId>>(HttpMethod.Get, $"{ServiceUrl}/{EntityPluralized}({urlEncodedAlternateId})", urlParameters, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByCustomUrlAsync(string urlPart, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(urlPart)) { throw new ArgumentException($"The {nameof(urlPart)} cannot be null, empty, or whitespace.", nameof(urlPart)); }
            return await SendAsync<OdataObjectCollection<TEntity, TId>>(HttpMethod.Get, $"{ServiceUrl}/{urlPart}", null, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> CallByCustomUrlAsync(string urlPart, HttpMethod httpMethod, object content, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(urlPart)) { throw new ArgumentException($"The {nameof(urlPart)} cannot be null, empty, or whitespace.", nameof(urlPart)); }
            return await SendAsync<object, OdataObjectCollection<TEntity, TId>>(httpMethod, $"{ServiceUrl}/{urlPart}", null, content, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> GetObjectByCustomUrlAsync(string urlPart, HttpMethod httpMethod, object content, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(urlPart)) { throw new ArgumentException($"The {nameof(urlPart)} cannot be null, empty, or whitespace.", nameof(urlPart)); }
            return await SendAsync<object, OdataObject<TEntity, TId>>(httpMethod, $"{ServiceUrl}/{urlPart}", null, content, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByQueryParametersAsync(string queryParameters, bool forwardExceptions = true)
            => await SendAsync<OdataObjectCollection<TEntity, TId>>(HttpMethod.Get, $"{ServiceUrl}/{EntityPluralized}", queryParameters, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByIdsAsync(IEnumerable<TId> ids, bool forwardExceptions = true)
            => await GetByIdsAsync(ids, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByIdsAsync(IEnumerable<TId> ids, string urlParameters, bool forwardExceptions = true)
        {
            if (ids is null || !ids.Any()) { throw new ArgumentException($"The {nameof(ids)} cannot be null or empty.", nameof(ids)); }
            return await SendAsync<IEnumerable<TId>, OdataObjectCollection<TEntity, TId>>(HttpMethod.Post, $"{ServiceUrl}/{EntityPluralized}/Ids", urlParameters, ids, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByPropertyValuesAsync(string property, IEnumerable<string> values, bool forwardExceptions = true)
            => await GetByPropertyValuesAsync(property, values, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByPropertyValuesAsync(string property, IEnumerable<string> values, string urlParameters, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(property)) { throw new ArgumentException($"The {nameof(property)} cannot be null, empty, or whitespace.", nameof(property)); }
            return await SendAsync<IEnumerable<string>, OdataObjectCollection<TEntity, TId>>(HttpMethod.Post, $"{ServiceUrl}/{EntityPluralized}/{property}/Values", urlParameters, values, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> GetPropertyAsync(string id, string property, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            if (string.IsNullOrWhiteSpace(property)) { throw new ArgumentException($"The {nameof(property)} cannot be null, empty, or whitespace.", nameof(property)); }
            return await _HttpClientRunner.Run(HttpMethod.Get, $"{ServiceUrl}/{EntityPluralized}({id})/{property}", forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<CsdlEntity> GetMetadataAsync(bool forwardExceptions = true)
            => await SendAsync<CsdlEntity>(HttpMethod.Get, $"{ServiceUrl}/$Metadata", null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> PatchAsync(string id, PatchedEntity<TEntity, TId> patchedEntity, bool forwardExceptions = true)
            => await PatchAsync(id, patchedEntity, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> PatchAsync(string id, PatchedEntity<TEntity, TId> patchedEntity, string urlParameters, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            if (patchedEntity is null) { throw new ArgumentNullException(nameof(patchedEntity)); }
            return await SendAsync<PatchedEntity<TEntity, TId>, OdataObject<TEntity, TId>>(new HttpMethod("PATCH"), $"{ServiceUrl}/{EntityPluralized}({id})", urlParameters, patchedEntity, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> PatchManyAsync(PatchedEntityCollection<TEntity, TId> patchedEntityCollection, bool forwardExceptions = true)
            => await PatchManyAsync(patchedEntityCollection, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> PatchManyAsync(PatchedEntityCollection<TEntity, TId> patchedEntityCollection, string urlParameters, bool forwardExceptions = true)
            => await SendAsync<PatchedEntityCollection<TEntity, TId>, OdataObject<TEntity, TId>>(new HttpMethod("PATCH"), $"{ServiceUrl}/{EntityPluralized}", urlParameters, patchedEntityCollection, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> PostAsync(IEnumerable<TEntity> entities, bool forwardExceptions = true)
        {
            if (entities is null || !entities.Any()) { throw new ArgumentException($"{nameof(entities)} cannot be null or empty.", nameof(entities)); }
            return await PostAsync(entities, null, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> PostAsync(IEnumerable<TEntity> entities, string urlParameters, bool forwardExceptions = true)
        {
            if (entities == null || !entities.Any()) { throw new ArgumentException(nameof(entities)); }
            return await SendAsync<IEnumerable<TEntity>, OdataObjectCollection<TEntity, TId>>(HttpMethod.Post, $"{ServiceUrl}/{EntityPluralized}", urlParameters, entities, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> PutAsync(string id, TEntity entity, bool forwardExceptions = true) 
                       => await PutAsync(id, entity, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> PutAsync(string id, TEntity entity, string urlParameters, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            if (entity is null) { throw new ArgumentNullException(nameof(entity)); }
            return await SendAsync<TEntity, OdataObject<TEntity, TId>>(HttpMethod.Put, $"{ServiceUrl}/{EntityPluralized}({id})", urlParameters, entity, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> UpdatePropertyAsync(string id, string property, string value, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            if (string.IsNullOrWhiteSpace(property)) { throw new ArgumentException($"The {nameof(property)} cannot be null, empty, or whitespace.", nameof(property)); }
            // Don't check value as we could be updating a property to a value of null.
            return await SendAsync<string, string>(HttpMethod.Put, $"{ServiceUrl}/{EntityPluralized}({id})/{property}", null, value, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<int> GetCountAsync(bool forwardExceptions = true)
                       => await GetCountAsync("$count", forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<int> GetCountAsync(string urlParameters, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(urlParameters))
                urlParameters = "$count";
            else if (!urlParameters.EndsWith("$count"))
                urlParameters += "&$count";
            return await SendAsync<int>(HttpMethod.Get, $"{ServiceUrl}/{EntityPluralized}", urlParameters, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> PostExtensionAsync(string id, string extensionEntity, PropertyValue propertyValue, bool forwardExceptions = true)
            => await PostExtensionAsync(id, extensionEntity, propertyValue, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObject<TEntity, TId>> PostExtensionAsync(string id, string extensionEntity, PropertyValue propertyValue, string urlParameters, bool forwardExceptions = true)
        {
            if (propertyValue == null) { throw new ArgumentNullException(nameof(propertyValue)); }
            return await SendAsync<PropertyValue, OdataObject<TEntity, TId>>(HttpMethod.Post, $"{ServiceUrl}/{EntityPluralized}('{id}')/Extension/{extensionEntity}", urlParameters, propertyValue, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<RepositoryGenerationResult> GenerateRepositoryAsync(bool forwardExceptions = true)
            => await _HttpClientRunner.RunAndDeserialize<RepositoryGenerationResult>(HttpMethod.Get, $"{ServiceUrl}/$GenerateRepository", forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<RepositorySeedResult> InsertSeedDataAsync(bool forwardExceptions = true)
            => await _HttpClientRunner.RunAndDeserialize<RepositorySeedResult>(HttpMethod.Get, $"{ServiceUrl}/$SeedRepository", forwardExceptions);

        protected async Task<TResult> SendAsync<TContent, TResult>(HttpMethod method, string url, string urlParameters, TContent content, bool forwardExceptions)
            => await _HttpClientRunner.RunAndDeserialize<TContent, TResult>(method, AppendUrlParameters(urlParameters, url), content, _EntityClientConnectionSettings.JsonSerializerSettings, forwardExceptions);

        protected async Task<TResult> SendAsync<TResult>(HttpMethod method, string url, string urlParameters, HttpContent content, bool forwardExceptions)
            => await _HttpClientRunner.RunAndDeserialize<TResult>(method, AppendUrlParameters(urlParameters, url), content, forwardExceptions);

        protected async Task<TResult> SendAsync<TResult>(HttpMethod method, string url, string urlParameters, bool forwardExceptions)
            => await _HttpClientRunner.RunAndDeserialize<TResult>(method, AppendUrlParameters(urlParameters, url), forwardExceptions);
            
    }
}