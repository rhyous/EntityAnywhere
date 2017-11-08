using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    /// <summary>
    /// A common class that any client can implement to talk to a mapping entity's web services. This inherits EntityClient adding the ability to get mapping entities by either of the mapped entities' ids.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TE1Id">The Entity1 id type. Entity1 should always be the entity with less instances.</typeparam>
    /// <typeparam name="TE2Id">The Entity2 id type. Entity2 should always be the entity with more instances.</typeparam>
    public class MappingEntityClient<TEntity, TId, TE1Id, TE2Id> : EntityClient<TEntity,TId>, IMappingEntityClient<TEntity, TId, TE1Id, TE2Id>, IMappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id>
        where TEntity : class, IMappingEntity<TE1Id, TE2Id>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        #region Constructors
        public MappingEntityClient() { }

        public MappingEntityClient(bool useMicrosoftDateFormat) : base(useMicrosoftDateFormat) { }

        public MappingEntityClient(JsonSerializerSettings jsonSerializerSettings) : base(jsonSerializerSettings) { }
        #endregion
        
        /// <inheritdoc />
        public string Entity1 { get { return _Entity1 ?? (_Entity1 = typeof(TEntity).GetMappedEntity1()); } }
        private string _Entity1;

        /// <inheritdoc />
        public string Entity1Pluralized { get { return _Entity1Pluralized ?? (_Entity1Pluralized = typeof(TEntity).GetMappedEntity1Pluralized()); } }
        private string _Entity1Pluralized;

        /// <inheritdoc />
        public string Entity1Property { get { return _Entity1Property ?? (_Entity1Property = typeof(TEntity).GetMappedEntity1Property()); } }
        private string _Entity1Property;

        /// <inheritdoc />
        public string Entity2 { get { return _Entity2 ?? (_Entity2 = typeof(TEntity).GetMappedEntity2()); } }
        private string _Entity2;

        /// <inheritdoc />
        public string Entity2Pluralized { get { return _Entity2Pluralized ?? (_Entity2Pluralized = typeof(TEntity).GetMappedEntity2Pluralized()); } }
        private string _Entity2Pluralized;

        /// <inheritdoc />
        public string Entity2Property { get { return _Entity2Property ?? (_Entity2Property = typeof(TEntity).GetMappedEntity2Property()); } }
        private string _Entity2Property;

        /// <inheritdoc />
        public OdataObjectCollection<TEntity, TId> GetByE1Ids(IEnumerable<TE1Id> ids)
        {
            return GetByE1Ids(ids.ToList());
        }

        /// <inheritdoc />
        public OdataObjectCollection<TEntity, TId> GetByE1Ids(List<TE1Id> ids)
        {
            return TaskRunner.RunSynchonously(GetByMappedEntityAsync, Entity1Pluralized, ids);
        }
        
        /// <inheritdoc />
        public OdataObjectCollection<TEntity, TId> GetByE2Ids(IEnumerable<TE2Id> ids)
        {
            return GetByE2Ids(ids.ToList());
        }

        /// <inheritdoc />
        public OdataObjectCollection<TEntity, TId> GetByE2Ids(List<TE2Id> ids)
        {
            return TaskRunner.RunSynchonously(GetByMappedEntityAsync, Entity2Pluralized, ids);
        }

        /// <inheritdoc />
        public async Task<OdataObjectCollection<TEntity, TId>> GetByE1IdsAsync(IEnumerable<TE1Id> ids)
        {
            return await GetByMappedEntityAsync(Entity1Pluralized, ids.ToList());
        }

        /// <inheritdoc />
        public async Task<OdataObjectCollection<TEntity, TId>> GetByE2IdsAsync(IEnumerable<TE2Id> ids)
        {
            return await GetByMappedEntityAsync(Entity2Pluralized, ids.ToList());
        }

        /// <inheritdoc />
        internal async Task<OdataObjectCollection<TEntity, TId>> GetByMappedEntityAsync<Eid>(string pluralizedEntityName, List<Eid> ids)
        {
            return await HttpClientRunner.RunAndDeserialize<List<Eid>, OdataObjectCollection<TEntity, TId>>(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}/{pluralizedEntityName}/Ids", ids);
        }
    }
}