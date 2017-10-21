using Newtonsoft.Json;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Clients
{
    /// <summary>
    /// A common class that any client can implement to talk to a mapping entity's web services. This inherits EntityClient adding the ability to get mapping entities by either of the mapped entities' ids.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TE1Id">The Entity1 id type. Entity1 should always be the entity with less instances.</typeparam>
    /// <typeparam name="TE2Id">The Entity2 id type. Entity2 should always be the entity with more instances.</typeparam>
    public class MappingEntityClient<TEntity, TId, TE1Id, TE2Id> : MappingEntityClientAsync<TEntity,TId, TE1Id, TE2Id>, IMappingEntityClient<TEntity, TId, TE1Id, TE2Id>
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
        public List<OdataObject<TEntity, TId>> GetByE1Ids(IEnumerable<TE1Id> ids)
        {
            return GetByE1Ids(ids.ToList());
        }

        /// <inheritdoc />
        public List<OdataObject<TEntity, TId>> GetByE1Ids(List<TE1Id> ids)
        {
            return TaskRunner.RunSynchonously(GetByMappedEntityAsync, Entity1Pluralized, ids);
        }
        
        /// <inheritdoc />
        public List<OdataObject<TEntity, TId>> GetByE2Ids(IEnumerable<TE2Id> ids)
        {
            return GetByE2Ids(ids.ToList());
        }

        /// <inheritdoc />
        public List<OdataObject<TEntity, TId>> GetByE2Ids(List<TE2Id> ids)
        {
            return TaskRunner.RunSynchonously(GetByMappedEntityAsync, Entity2Pluralized, ids);
        }
    }
}