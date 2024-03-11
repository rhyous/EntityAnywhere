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
    /// A common entity web service for entities that have the AlternateKey attribute, which indicates a second key field, such as Name.
    /// </summary>
    /// <typeparam name="TEntity">The Entity Type</typeparam>
    /// <typeparam name="TInterface">The Entity Interface Type</typeparam>
    /// <typeparam name="TId">The entity</typeparam>
    /// <typeparam name="TAltKey">The type of the Alternate Key.</typeparam>
    [Authorize(AuthenticationSchemes = TokenAuthenticationSchemeOptions.Name)]
    public class EntityControllerAlternateKey<TEntity, TInterface, TId, TAltKey> 
        : EntityController<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        /// <summary>The constructor.</summary>
        /// <param name="restHandlerProvider">A provider that injects the correct handler.</param>
        public EntityControllerAlternateKey(IRestHandlerProvider restHandlerProvider)
            : base(restHandlerProvider)
        {
        }

        /// <summary>
        /// Gets the entity by Id or by AlternateKey.
        /// </summary>
        /// <param name="idOrAltId">The Id or the Alternate Key.</param>
        /// <returns>The entity found.</returns>
        /// <remarks>The alternate key cannot be the same Type as the Id.</remarks>
        [HttpGet("[action]({idOrAltId})")]
        [EntityActionName("{EntityPluralized}")]
        public override async Task<OdataObject<TEntity, TId>> GetAsync([FromRoute]string idOrAltId)
        {
            return await _RestHandlerProvider.Provide<IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>>()
                                             .HandleAsync(idOrAltId);
        }
    }
}