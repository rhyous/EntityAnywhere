using Rhyous.StringLibrary;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// A web service for a Mapping Entity. This inherits the service contract for a regular entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TService">The entity service type.</typeparam>
    /// <typeparam name="TE1Id">The Entity1 id type. Entity1 should always be the entity with less instances.</typeparam>
    /// <typeparam name="TE2Id">The Entity2 id type. Entity2 should always be the entity with more instances.</typeparam>
    public class MappingEntityWebService<TEntity, TInterface, TId, TService, TE1Id, TE2Id>
        : EntityWebService<TEntity, TInterface, TId, TService>, IMappingEntityWebService<TEntity, TId, TE1Id, TE2Id>
        where TEntity : class, TInterface, new()
        where TInterface : IEntity<TId>, IMappingEntity<TE1Id, TE2Id>
        where TService : class, IServiceCommon<TEntity, TInterface, TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        /// <inheritdoc />
        public List<OdataObject<TEntity, TId>> GetByE1Ids(List<TE1Id> ids)
        {
            var propertyName = typeof(TEntity).GetMappedEntity1Property();
            var lambda = propertyName.ToLambda<TEntity, TE1Id>(ids);
            return Service.Get(lambda)?.ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(RequestUri);
        }

        /// <inheritdoc />
        public List<OdataObject<TEntity, TId>> GetByE2Ids(List<TE2Id> ids)
        {
            var propertyName = typeof(TEntity).GetMappedEntity2Property();
            var lambda = propertyName.ToLambda<TEntity, TE2Id>(ids);
            return Service.Get(lambda)?.ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(RequestUri);
        }
    }
}
