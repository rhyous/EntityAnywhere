using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
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
    public class MappingEntityWebService<TEntity, TInterface, TId, TE1Id, TE2Id>
        : EntityWebService<TEntity, TInterface, TId>, IMappingEntityWebService<TEntity, TId, TE1Id, TE2Id>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>, IMappingEntity<TE1Id, TE2Id>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        private readonly IGetByPropertyValuesHandler<TEntity, TInterface, TId, TE1Id> _GetByPropertyValuesHandlerE1;
        private readonly IGetByPropertyValuesHandler<TEntity, TInterface, TId, TE2Id> _GetByPropertyValuesHandlerE2;

        public MappingEntityWebService(IRestHandlerProvider<TEntity, TInterface, TId> restHandlerProvider,
                                       IGetByPropertyValuesHandler<TEntity, TInterface, TId, TE1Id> getByPropertyValuesHandlerE1,
                                       IGetByPropertyValuesHandler<TEntity, TInterface, TId, TE2Id> getByPropertyValuesHandlerE2)
            : base(restHandlerProvider)
        {
            _GetByPropertyValuesHandlerE1 = getByPropertyValuesHandlerE1;
            _GetByPropertyValuesHandlerE2 = getByPropertyValuesHandlerE2;
        }

        /// <inheritdoc />
        public async Task<OdataObjectCollection<TEntity, TId>> GetByE1IdsAsync(List<TE1Id> ids)
                   => await _GetByPropertyValuesHandlerE1.HandleAsync(typeof(TEntity).GetMappedEntity1Property(), ids);

        /// <inheritdoc />
        public async Task<OdataObjectCollection<TEntity, TId>> GetByE2IdsAsync(List<TE2Id> ids) 
                   => await _GetByPropertyValuesHandlerE2.HandleAsync(typeof(TEntity).GetMappedEntity2Property(), ids);
    }
}