using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class EntityClientAsync : EntityClientBase, IEntityClientAsync
    {
        protected readonly IHttpClientRunner _HttpClientRunner;

        public EntityClientAsync(IEntityClientConnectionSettings entityClientSettings,
                                 IHttpClientRunner httpClientRunner)
            : base(entityClientSettings)
        {
            _HttpClientRunner = httpClientRunner ?? throw new ArgumentNullException(nameof(httpClientRunner));
        }

        /// <inheritdoc />
        public virtual async Task<bool> DeleteAsync(string id, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            return await _HttpClientRunner.RunAndDeserialize<bool>(HttpMethod.Delete, $"{ServiceUrl}/{EntityPluralized}({id})", forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<Dictionary<string, bool>> DeleteManyAsync(IEnumerable<string> ids, bool forwardExceptions = true)
        {
            if (ids == null || !ids.Any() || ids.All(id => string.IsNullOrWhiteSpace(id))) { throw new ArgumentException($"The {nameof(ids)} cannot be null, empty, or whitespace.", nameof(ids)); }
            var distinctValidIds = ids.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct();
            return await _HttpClientRunner.RunAndDeserialize<IEnumerable<string>, Dictionary<string, bool>>(HttpMethod.Delete, $"{ServiceUrl}/{EntityPluralized}/Bulk", distinctValidIds, _EntityClientConnectionSettings.JsonSerializerSettings, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<Dictionary<TId, bool>> DeleteManyAsync<TId>(IEnumerable<TId> ids, bool forwardExceptions = true)
        {
            if (ids == null || !ids.Any()) { throw new ArgumentException($"The {nameof(ids)} cannot be null, empty, or whitespace.", nameof(ids)); }
            var distinctValidIds = ids.Distinct();
            return await _HttpClientRunner.RunAndDeserialize<IEnumerable<TId>, Dictionary<TId, bool>>(HttpMethod.Delete, $"{ServiceUrl}/{EntityPluralized}/Bulk", distinctValidIds, _EntityClientConnectionSettings.JsonSerializerSettings, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> GetAllAsync(bool forwardExceptions = true)
            => await GetByQueryParametersAsync(null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> GetAsync<TId>(TId id, bool forwardExceptions = true)
            => await GetAsync(id.ToString(), null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> GetAsync(string id, bool forwardExceptions = true)
            => await GetAsync(id, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> GetAsync(string idOrName, string urlParameters, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(idOrName)) { throw new ArgumentException($"The {nameof(idOrName)} cannot be null, empty, or whitespace.", nameof(idOrName)); }
            if (idOrName.ToString().StartsWith(IdDisambiguator.AltKey)) // Get by Alternate Key
                return await GetByAlternateKeyAsync(idOrName, urlParameters, forwardExceptions);
            if (idOrName.ToString().StartsWith(IdDisambiguator.Alt + IdDisambiguator.Separator)) // Get by Alternate Id
                return await GetByAlternateIdAsync(idOrName, urlParameters, forwardExceptions);
            idOrName = idOrName.Quote('\'');
            return await SendAsync(HttpMethod.Get, $"{ServiceUrl}/{EntityPluralized}({idOrName})", urlParameters, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> GetByAlternateKeyAsync(string altKey, bool forwardExceptions = true)
            => await GetByAlternateKeyAsync(altKey, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> GetByAlternateKeyAsync(string altKey, string urlParameters, bool forwardExceptions = true)
            => await GetByDisambiguatedId(altKey, IdDisambiguator.AltKey, urlParameters, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> GetByAlternateIdAsync(string altId, bool forwardExceptions = true) =>
                       await GetByAlternateIdAsync(altId, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> GetByAlternateIdAsync(string altId, string urlParameters, bool forwardExceptions = true)
            => await GetByDisambiguatedId(altId, $"{IdDisambiguator.Alt}{IdDisambiguator.Separator}", urlParameters, forwardExceptions);

        internal virtual async Task<string> GetByDisambiguatedId(string altKey, string disabmiguator, string urlParameters, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(altKey)) { throw new ArgumentException($"The {nameof(altKey)} cannot be null, empty, or whitespace.", nameof(altKey)); }
            if (!altKey.StartsWith(disabmiguator, StringComparison.OrdinalIgnoreCase))
                altKey = $"{disabmiguator}{altKey}";
            var urlEncodedAlternateId = Uri.EscapeDataString(altKey).Quote('\'');
            return await SendAsync(HttpMethod.Get, $"{ServiceUrl}/{EntityPluralized}({urlEncodedAlternateId})", urlParameters, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> GetByCustomUrlAsync(string urlPart, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(urlPart)) { throw new ArgumentException($"The {nameof(urlPart)} cannot be null, empty, or whitespace.", nameof(urlPart)); }
            return await _HttpClientRunner.Run(HttpMethod.Get, $"{ServiceUrl}/{urlPart}", forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> CallByCustomUrlAsync(string urlPart, HttpMethod httpMethod, object content, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(urlPart)) { throw new ArgumentException($"The {nameof(urlPart)} cannot be null, empty, or whitespace.", nameof(urlPart)); }
            return await _HttpClientRunner.Run(httpMethod, $"{ServiceUrl}/{urlPart}", content, _EntityClientConnectionSettings.JsonSerializerSettings, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> GetObjectByCustomUrlAsync(string urlPart, HttpMethod httpMethod, object content, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(urlPart)) { throw new ArgumentException($"The {nameof(urlPart)} cannot be null, empty, or whitespace.", nameof(urlPart)); }
            return await _HttpClientRunner.Run(httpMethod, $"{ServiceUrl}/{urlPart}", content, _EntityClientConnectionSettings.JsonSerializerSettings, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> GetByQueryParametersAsync(string queryParameters, bool forwardExceptions = true)
            => await SendAsync(HttpMethod.Get, $"{ServiceUrl}/{EntityPluralized}", queryParameters, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> GetByIdsAsync(IEnumerable<string> ids, bool forwardExceptions = true)
            => await GetByIdsAsync(ids, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> GetByIdsAsync(IEnumerable<string> ids, string urlParameters, bool forwardExceptions = true)
        {
            if (ids is null || !ids.Any()) { throw new ArgumentException($"The {nameof(ids)} cannot be null or empty.", nameof(ids)); }
            return await SendAsync(HttpMethod.Post, $"{ServiceUrl}/{EntityPluralized}/Ids", urlParameters, ids, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> GetByPropertyValuesAsync(string property, IEnumerable<string> values, bool forwardExceptions = true) 
            => await GetByPropertyValuesAsync(property, values, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> GetByPropertyValuesAsync(string property, IEnumerable<string> values, string urlParameters, bool forwardExceptions = false)
        {
            if (string.IsNullOrWhiteSpace(property)) { throw new ArgumentException($"The {nameof(property)} cannot be null, empty, or whitespace.", nameof(property)); }
            return await SendAsync(HttpMethod.Post, $"{ServiceUrl}/{EntityPluralized}/{property}/Values", urlParameters, values, forwardExceptions);
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
            => await _HttpClientRunner.RunAndDeserialize<CsdlEntity>(HttpMethod.Get, $"{ServiceUrl}/$Metadata", forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> PatchAsync(string id, HttpContent content, bool forwardExceptions = true)
            => await PatchAsync(id, content, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> PatchAsync(string id, HttpContent content, string urlParameters, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            if (content == null) { throw new ArgumentNullException(nameof(content)); }
            return await SendAsync(new HttpMethod("PATCH"), $"{ServiceUrl}/{EntityPluralized}({id})", urlParameters, content, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> PatchAsync(string id, object obj, bool forwardExceptions = true)
            => await PatchAsync(id, obj, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> PatchAsync(string id, object obj, string urlParameters, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            if (obj == null) { throw new ArgumentNullException(nameof(obj)); }
            return await SendAsync(new HttpMethod("PATCH"), $"{ServiceUrl}/{EntityPluralized}({id})", urlParameters, obj, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> PatchManyAsync(HttpContent content, bool forwardExceptions = true)
            => await PatchManyAsync(content, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> PatchManyAsync(HttpContent content, string urlParameters, bool forwardExceptions = true)
            => await SendAsync(new HttpMethod("PATCH"), $"{ServiceUrl}/{EntityPluralized}", urlParameters, content, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> PatchManyAsync(object obj, bool forwardExceptions = true)
            => await PatchManyAsync(obj, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> PatchManyAsync(object obj, string urlParameters, bool forwardExceptions = true)
            => await SendAsync(new HttpMethod("PATCH"), $"{ServiceUrl}/{EntityPluralized}", urlParameters, obj, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> PostAsync(HttpContent content, bool forwardExceptions = true)
            => await PostAsync(content, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> PostAsync(HttpContent content, string urlParameters, bool forwardExceptions = false)
        {
            if (content == null) { throw new ArgumentNullException(nameof(content)); }
            return await SendAsync(HttpMethod.Post, $"{ServiceUrl}/{EntityPluralized}", urlParameters, content, forwardExceptions);
        }

        public virtual async Task<string> PostAsync(object obj, bool forwardExceptions = true)
            => await PostAsync(obj, null, forwardExceptions);

        public virtual async Task<string> PostAsync(object obj, string urlParameters, bool forwardExceptions = true)
        {
            if (obj == null) { throw new ArgumentNullException(nameof(obj)); }
            return await SendAsync(HttpMethod.Post, $"{ServiceUrl}/{EntityPluralized}", urlParameters, obj, forwardExceptions);
        }

        public virtual async Task<string> PutAsync(string id, HttpContent content, bool forwardExceptions = true)
            => await PutAsync(id, content, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> PutAsync(string id, HttpContent content, string urlParameters, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            if (content == null) { throw new ArgumentNullException(nameof(content)); }
            return await SendAsync(HttpMethod.Put, $"{ServiceUrl}/{EntityPluralized}({id})", urlParameters, content, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> PutAsync(string id, object obj, bool forwardExceptions = true) 
            => await PutAsync(id, obj, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> PutAsync(string id, object obj, string urlParameters, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            if (obj == null) { throw new ArgumentNullException(nameof(obj)); }
            return await SendAsync(HttpMethod.Put, $"{ServiceUrl}/{EntityPluralized}({id})", urlParameters, obj, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> UpdatePropertyAsync(string id, string property, string value, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            if (string.IsNullOrWhiteSpace(property)) { throw new ArgumentException($"The {nameof(property)} cannot be null, empty, or whitespace.", nameof(property)); }
            // Don't throw if value is null as we could be updating a property to a value of null.
            return await _HttpClientRunner.Run(HttpMethod.Put, $"{ServiceUrl}/{EntityPluralized}({id})/{property}", value, _EntityClientConnectionSettings.JsonSerializerSettings, forwardExceptions);
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
            var url = $"{ServiceUrl}/{EntityPluralized}";
            url = AppendUrlParameters(urlParameters, url);
            return await _HttpClientRunner.RunAndDeserialize<int>(HttpMethod.Get, url, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> PostExtensionAsync(string id, string extensionEntity, HttpContent content, bool forwardExceptions = true)
            => await PostExtensionAsync(id, extensionEntity, content, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> PostExtensionAsync(string id, string extensionEntity, HttpContent content, string urlParameters, bool forwardExceptions = false)
        {
            if (content == null) { throw new ArgumentNullException(nameof(content)); }
            return await SendAsync(HttpMethod.Post, $"{ServiceUrl}/{EntityPluralized}('{id}')/Extension/{extensionEntity}", urlParameters, content, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<string> PostExtensionAsync(string id, string extensionEntity, object obj, bool forwardExceptions = true)
            => await PostExtensionAsync(id, extensionEntity, obj, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<string> PostExtensionAsync(string id, string extensionEntity, object obj, string urlParameters, bool forwardExceptions = true)
        {
            if (obj == null) { throw new ArgumentNullException(nameof(obj)); }
            return await SendAsync(HttpMethod.Post, $"{ServiceUrl}/{EntityPluralized}('{id}')/Extension/{extensionEntity}", urlParameters, obj, forwardExceptions);
        }

        /// <inheritdoc />
        public virtual async Task<RepositoryGenerationResult> GenerateRepositoryAsync(bool forwardExceptions = true) =>
            await _HttpClientRunner.RunAndDeserialize<RepositoryGenerationResult>(HttpMethod.Get, $"{ServiceUrl}/$Generate", forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<RepositorySeedResult> InsertSeedDataAsync(bool forwardExceptions = true) =>
            await _HttpClientRunner.RunAndDeserialize<RepositorySeedResult>(HttpMethod.Get, $"{ServiceUrl}/$Seed", forwardExceptions);

        protected async Task<string> SendAsync(HttpMethod method, string url, string urlParameters, object obj, bool forwardExceptions)
            => await _HttpClientRunner.Run(method, AppendUrlParameters(urlParameters, url), obj, _EntityClientConnectionSettings.JsonSerializerSettings, forwardExceptions);

        protected async Task<string> SendAsync(HttpMethod method, string url, string urlParameters, HttpContent content, bool forwardExceptions)
            => await _HttpClientRunner.Run(method, AppendUrlParameters(urlParameters, url), content, forwardExceptions);

        protected async Task<string> SendAsync(HttpMethod method, string url, string urlParameters, bool forwardExceptions)
            => await _HttpClientRunner.Run(method, AppendUrlParameters(urlParameters, url), forwardExceptions);

        public virtual async Task<Dictionary<TId, bool>> DeleteAllExtensionsAsync<TId>(string id, string extensionEntity, bool forwardExceptions)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            if (string.IsNullOrWhiteSpace(extensionEntity)) { throw new ArgumentException($"The {nameof(extensionEntity)} cannot be null, empty, or whitespace.", nameof(extensionEntity)); }
            return await _HttpClientRunner.RunAndDeserialize<Dictionary<TId, bool>>(HttpMethod.Delete, $"{ServiceUrl}/{EntityPluralized}('{id}')/Extension/{extensionEntity}", forwardExceptions);
        }

        public virtual async Task<Dictionary<TId, bool>> DeleteExtensionsAsync<TId>(string id, string extensionEntity, IEnumerable<string> extensionEntityIdsids, bool forwardExceptions)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentException($"The {nameof(id)} cannot be null, empty, or whitespace.", nameof(id)); }
            if (string.IsNullOrWhiteSpace(extensionEntity)) { throw new ArgumentException($"The {nameof(extensionEntity)} cannot be null, empty, or whitespace.", nameof(extensionEntity)); }
            if (extensionEntityIdsids == null || !extensionEntityIdsids.Any()) { throw new ArgumentException($"The {nameof(extensionEntityIdsids)} cannot be null, empty, or whitespace.", nameof(extensionEntityIdsids)); }
            var distinctValidIds = extensionEntityIdsids.Distinct();
            return await _HttpClientRunner.RunAndDeserialize<object, Dictionary<TId, bool>>(HttpMethod.Delete, $"{ServiceUrl}/{EntityPluralized}('{id}')/Extension/{extensionEntity}", distinctValidIds, _EntityClientConnectionSettings.JsonSerializerSettings, forwardExceptions);
        }

        public async Task<List<string>> GetDistinctPropertyValuesAsync(string propertyName, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException($"The {nameof(propertyName)} cannot be null, empty, or whitespace.", nameof(propertyName));

            var url = $"{ServiceUrl}/{EntityPluralized}/{propertyName}/Distinct";
            return await _HttpClientRunner.RunAndDeserialize<List<string>>(HttpMethod.Get, url, forwardExceptions);
        }

    }
}