using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class MappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id> : EntityClientAsync<TEntity, TId>, IMappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id>
        where TEntity : class, IId<TId>, IMappingEntity<TE1Id, TE2Id>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        private readonly IMappingEntitySettings<TEntity> _MappingEntitySettings;

        public MappingEntityClientAsync(IEntityClientConnectionSettings<TEntity> entityClientConnectionSettings,
                                        IMappingEntitySettings<TEntity> mappingEntitySettings,
                                        IHttpClientRunner httpClientRunner)
            : base(entityClientConnectionSettings, httpClientRunner)
        {
            _MappingEntitySettings = mappingEntitySettings ?? throw new ArgumentNullException(nameof(mappingEntitySettings));
        }

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByE1IdsAsync(IEnumerable<TE1Id> ids, bool forwardExceptions = true)
               => await GetByE1IdsAsync(ids, null, forwardExceptions);        

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByE1IdsAsync(IEnumerable<TE1Id> ids, string urlParameters, bool forwardExceptions = true)
               => await GetByMappedEntityAsync(_MappingEntitySettings.Entity1Pluralized, ids, urlParameters, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByE2IdsAsync(IEnumerable<TE2Id> ids, bool forwardExceptions = true)
               => await GetByE2IdsAsync(ids, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByE2IdsAsync(IEnumerable<TE2Id> ids, string urlParameters, bool forwardExceptions = true)
               => await GetByMappedEntityAsync(_MappingEntitySettings.Entity2Pluralized, ids, urlParameters, forwardExceptions);
       
        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByMappingsAsync(IEnumerable<TEntity> mappings, bool forwardExceptions = true)
               => await GetByMappingsAsync(mappings, null, forwardExceptions);

        /// <inheritdoc />
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByMappingsAsync(IEnumerable<TEntity> mappings, string urlParameters, bool forwardExceptions = true)
               => await SendAsync<IEnumerable<TEntity>, OdataObjectCollection<TEntity, TId>>(HttpMethod.Post, $"{ServiceUrl}/{EntityPluralized}/Mappings", urlParameters, mappings, forwardExceptions);

        /// <inheritdoc />
        internal virtual async Task<OdataObjectCollection<TEntity, TId>> GetByMappedEntityAsync<Eid>(string pluralizedEntityName, IEnumerable<Eid> ids, string urlParameters = null, bool forwardExceptions = true)
        {
            if (ids is null || !ids.Any()) { throw new ArgumentException(nameof(ids)); }
            return await SendAsync<List<Eid>, OdataObjectCollection<TEntity, TId>>(HttpMethod.Post, $"{ServiceUrl}/{EntityPluralized}/{pluralizedEntityName}/Ids", urlParameters, ids.ToList(), forwardExceptions);
        }
    }
}