using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class ExtensionEntityClientAsync : EntityClientAsync, IExtensionEntityClientAsync
    {
        public ExtensionEntityClientAsync(IEntityClientConnectionSettings entityClientSettings,
                                 IHttpClientRunner httpClientRunner)
            : base(entityClientSettings, httpClientRunner)
        {
        }

        /// <inheritdoc />
        public virtual async Task<string> GetByEntityIdentifiersAsync(IEnumerable<EntityIdentifier> entityIdentifiers, bool forwardExceptions = true)
        {
            if (entityIdentifiers is null || !entityIdentifiers.Any()) { throw new ArgumentException(nameof(entityIdentifiers)); }
            var url = $"{ServiceUrl}/{EntityPluralized}/EntityIdentifiers";
            return await _HttpClientRunner.Run(HttpMethod.Post, url, entityIdentifiers, _EntityClientConnectionSettings.JsonSerializerSettings, forwardExceptions);
        }

        public virtual async Task<string> GetByEntityPropertyValueAsync(string entity, string property, string value, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(entity))
                throw new ArgumentException($"The {nameof(entity)} cannot be null, empty, or whitespace.", nameof(entity));
            if (string.IsNullOrWhiteSpace(property))
                throw new ArgumentException($"The {nameof(property)} cannot be null, empty, or whitespace.", nameof(property));
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"The {nameof(value)} cannot be null, empty, or whitespace.", nameof(value));
            var url = $"{ServiceUrl}/{EntityPluralized}?$filter=Entity eq '{entity}' and Property eq '{property}' and Value eq '{value}'";
            return await _HttpClientRunner.Run(HttpMethod.Get, url, forwardExceptions);
        }

        public async Task<List<string>> GetDistinctExtensionPropertyValuesAsync(string entity, string propertyName, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(entity))
                throw new ArgumentException($"The {nameof(entity)} cannot be null, empty, or whitespace.", nameof(entity));

            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException($"The {nameof(propertyName)} cannot be null, empty, or whitespace.", nameof(propertyName));

            var url = $"{ServiceUrl}/{EntityPluralized}/{entity}/{propertyName}/Distinct";
            return await _HttpClientRunner.RunAndDeserialize<List<string>>(HttpMethod.Get, url, forwardExceptions);
        }
    }
}