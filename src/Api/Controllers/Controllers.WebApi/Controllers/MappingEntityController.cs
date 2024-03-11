using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Security;
using Rhyous.EntityAnywhere.WebServices;
using Rhyous.Odata;

namespace Rhyous.EntityAnywhere.WebApi
{
    /// <summary>
    /// A web service for a Mapping Entity. This inherits the service contract for a regular entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TE1Id">The Entity1 id type. Entity1 should always be the entity with less instances.</typeparam>
    /// <typeparam name="TE2Id">The Entity2 id type. Entity2 should always be the entity with more instances.</typeparam>
    [Authorize(AuthenticationSchemes = TokenAuthenticationSchemeOptions.Name)]
    public class MappingEntityController<TEntity, TInterface, TId, TE1Id, TE2Id>
        : EntityController<TEntity, TInterface, TId>, IMappingEntityController<TEntity, TId, TE1Id, TE2Id>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>, IMappingEntity<TE1Id, TE2Id>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        /// <summary>The cosntructor</summary>
        /// <param name="restHandlerProvider">A provider to provide the handler the rest call.</param>
        public MappingEntityController(IRestHandlerProvider restHandlerProvider)
            : base(restHandlerProvider)
        {
        }

        /// <inheritdoc />
        [HttpPost("[action]/Ids")]
        [EntityActionName("{EntityPluralized}/{E1Pluralized}")]
        public async Task<OdataObjectCollection<TEntity, TId>> GetByE1IdsAsync([FromBody] List<TE1Id> ids)
                   => await _RestHandlerProvider.Provide<IGetByMappedIdsHandler<TEntity, TInterface, TId, TE1Id>>()
                                                .HandleAsync(typeof(TEntity).GetMappedEntity1Property(), ids);

        /// <inheritdoc />
        [HttpPost("[action]/Ids")]
        [EntityActionName("{EntityPluralized}/{E2Pluralized}")]
        public async Task<OdataObjectCollection<TEntity, TId>> GetByE2IdsAsync([FromBody] List<TE2Id> ids)
                   => await _RestHandlerProvider.Provide<IGetByMappedIdsHandler<TEntity, TInterface, TId, TE2Id>>()
                                                .HandleAsync(typeof(TEntity).GetMappedEntity2Property(), ids);

        /// <inheritdoc />
        [HttpPost("[action]/Mappings")]
        [EntityActionName("{EntityPluralized}")]
        public async Task<OdataObjectCollection<TEntity, TId>> GetByMappingsAsync([FromBody] List<TEntity> mappings)
                   => await _RestHandlerProvider.Provide<IGetByMappingsHandler<TEntity, TInterface, TId, TE1Id, TE2Id>>()
                                                .HandleAsync(mappings);

    }
}