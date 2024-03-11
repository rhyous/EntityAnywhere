using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// A common service layer for all entities that have a property as a second key, other than the Id property.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public class ServiceCommonAlternateKey<TEntity, TInterface, TId, TAltKey> : ServiceCommon<TEntity, TInterface, TId>,
                                                                                IServiceCommonAlternateKey<TEntity, TInterface, TId, TAltKey>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {

        public ServiceCommonAlternateKey(IServiceHandlerProvider serviceHandlerProvider)
            : base(serviceHandlerProvider)
        {
        }

        /// <inheritdoc />
        public TInterface GetByAlternateKey(TAltKey propertyValue)
            => _ServiceHandlerProvider.Provide<IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>>()
                                      .Get(propertyValue);

        /// <inheritdoc />
        public List<TInterface> Search(TAltKey searchValue)
            => _ServiceHandlerProvider.Provide<ISearchByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>>()
                                      .Search(searchValue);

        /// <inheritdoc />
        public override async Task<List<TInterface>> AddAsync(IEnumerable<TInterface> entities)
            => await _ServiceHandlerProvider.Provide<IAddAltKeyHandler<TEntity, TInterface, TId, TAltKey>>()
                                            .AddAsync(entities);

        public override TInterface Update(TId id, PatchedEntity<TInterface, TId> patchedEntity)
            => _ServiceHandlerProvider.Provide<IUpdateAltKeyHandler<TEntity, TInterface, TId, TAltKey>>()
                                      .Update(id, patchedEntity);

        public override List<TInterface> Update(PatchedEntityCollection<TInterface, TId> patchedEntityCollection)
            => _ServiceHandlerProvider.Provide<IUpdateAltKeyHandler<TEntity, TInterface, TId, TAltKey>>()
                                      .Update(patchedEntityCollection);
    }
}