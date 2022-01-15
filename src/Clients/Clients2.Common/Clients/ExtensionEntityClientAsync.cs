using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{

    public class ExtensionEntityClientAsync<TEntity, TId> : EntityClientAsync<TEntity, TId>,
                                                            IExtensionEntityClientAsync<TEntity, TId>
        where TEntity : class, IId<TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public ExtensionEntityClientAsync(IEntityClientConnectionSettings<TEntity> entityClientSettings,
                                          IHttpClientRunner httpClientRunner)
            : base(entityClientSettings, httpClientRunner)
        {
        }

        /// <inheritdoc />
        public async Task<OdataObjectCollection<TEntity, TId>> GetByEntityIdentifiersAsync(IEnumerable<EntityIdentifier> entityIdentifiers, bool forwardExceptions = true)
        {
            if (entityIdentifiers == null || !entityIdentifiers.Any()) { throw new ArgumentException(nameof(entityIdentifiers)); }
            var url = $"{ServiceUrl}/{EntityPluralized}/EntityIdentifiers";
            return await _HttpClientRunner.RunAndDeserialize<IEnumerable<EntityIdentifier>, OdataObjectCollection<TEntity, TId>>(
                HttpMethod.Post, url, entityIdentifiers, _EntityClientConnectionSettings.JsonSerializerSettings, forwardExceptions);
        }

        public async Task<OdataObjectCollection<TEntity, TId>> GetByEntityPropertyValueAsync(string entity, string property, string value, bool forwardExceptions = true)
        {
            if (string.IsNullOrWhiteSpace(entity))
                throw new ArgumentException($"The {nameof(entity)} cannot be null, empty, or whitespace.", nameof(entity));
            if (string.IsNullOrWhiteSpace(property))
                throw new ArgumentException($"The {nameof(property)} cannot be null, empty, or whitespace.", nameof(property));
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"The {nameof(value)} cannot be null, empty, or whitespace.", nameof(value));
            var url = $"{ServiceUrl}/{EntityPluralized}?$filter=Entity eq '{entity}' and Property eq '{property}' and Value eq '{value}'";
            return await _HttpClientRunner.RunAndDeserialize<OdataObjectCollection<TEntity, TId>>(HttpMethod.Get, url, forwardExceptions);
        }
    }
}