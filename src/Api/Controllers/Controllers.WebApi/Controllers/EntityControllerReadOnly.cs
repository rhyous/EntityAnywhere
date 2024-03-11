using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Security;
using Rhyous.EntityAnywhere.WebServices;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;

namespace Rhyous.EntityAnywhere
{
    /// <summary>
    /// This allows for creating a generic entity controller.
    /// All generic entity controllers will use this path:
    ///   {host}/Api/{entity}Service/{entityPluralize}
    /// Example for a User entity:
    ///   {host}/Api/UserService/Users
    /// Then the HTTP verb (GET, POST, DELETE, PUT, PATCH) determines which method will be hit
    /// </summary>
    /// <typeparam name="TEntity">The Entity Type</typeparam>
    /// <typeparam name="TInterface">The Entity Interace Type</typeparam>
    /// <typeparam name="TId">The entity</typeparam>
    [ApiController]
    [Route("api/[controller]")]
    [EntityGenericController]
    [Authorize(AuthenticationSchemes = TokenAuthenticationSchemeOptions.Name)]
    public class EntityControllerReadOnly<TEntity, TInterface, TId> : ControllerBase,
        IEntityWebServiceReadOnly<TEntity, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        protected readonly IRestHandlerProvider _RestHandlerProvider;

        /// <summary>The constructor.</summary>
        /// <param name="restHandlerProvider">A provider to provide the handler the rest call.</param>
        public EntityControllerReadOnly(IRestHandlerProvider restHandlerProvider)
        {
            _RestHandlerProvider = restHandlerProvider;
        }

        /// <summary>
        /// This retuns metadata about the services.
        /// </summary>
        /// <returns>Schema of entity. Should be in CSDL (option for both json or xml should exist)</returns>
        [AllowAnonymous]
        [HttpGet("$Metadata")]
        public virtual async Task<CsdlEntity> GetMetadataAsync()
            => await _RestHandlerProvider.Provide<IGetMetadataHandler>().Handle(typeof(TEntity));

        /// <summary>
        /// Gets the number of total entities
        /// </summary>
        /// <returns>The number of total entities.</returns>
        [HttpGet("[action]/count")]
        [EntityActionName("{EntityPluralized}")]
        public virtual async Task<int> GetCountAsync()
            => await _RestHandlerProvider.Provide<IGetCountHandler<TEntity, TInterface, TId>>()
                                         .HandleAsync();

        /// <summary>
        /// Gets all entities.
        /// Note: Be careful using this on entities that are extremely large in quantity.
        /// </summary>
        /// <returns>List{OdataObject{T}} containing all entities</returns>

        [HttpGet("[action]")]
        [EntityActionName("{EntityPluralized}")]
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetAllAsync()
            => await _RestHandlerProvider.Provide<IGetAllHandler<TEntity, TInterface, TId>>()
                                         .HandleAsync();

        /// <summary>
        /// Get a list of Entities where the Entity id is in the list of ids provided.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>A List{OdataObject{T}} of entities where each is wrapped in an Odata object.</returns>
        [HttpPost("[action]/Ids")]
        [EntityActionName("{EntityPluralized}")]
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByIdsAsync(List<TId> ids)
            => await _RestHandlerProvider.Provide<IGetByIdsHandler<TEntity, TInterface, TId>>()
                                         .HandleAsync(ids);

        /// <summary>
        /// Get a list of Entities where the value of the property of a given Entity is in the list of values provided.
        /// </summary>
        /// <param name="collection">A ValueCollection that has the property name and the values as strings.</param>
        /// <returns>A List{OdataObject{T}} of entities where each is wrapped in an Odata object.</returns>
        [HttpPost("[action]/{property}/values")]
        [EntityActionName("{EntityPluralized}")]
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByPropertyValuesAsync(string property, List<string> values)
            => await _RestHandlerProvider.Provide<IGetByPropertyValuesHandler<TEntity, TInterface, TId>>()
                                         .HandleAsync(property, values);

        /// <summary>
        /// Get the exact entity with the id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>A <see cref="OdataObject{T, TId}"/> object contain the single entity with the id provided.</returns>
        [HttpGet("[action]({id})")]
        [EntityActionName("{EntityPluralized}")]
        public virtual async Task<OdataObject<TEntity, TId>> GetAsync([FromRoute]string id)
            => await _RestHandlerProvider.Provide<IGetByIdHandler<TEntity, TInterface, TId>>()
                                         .HandleAsync(id);

        /// <summary>
        /// Gets the value of one of the entity properties. 
        /// Example: if the entity is User and you want to get only the value of User.Username, not 
        /// the whole entity.
        /// </summary>
        /// <param name="id">The Entity Id.</param>
        /// <param name="property">A primitive property of the entity id.</param>
        /// <returns>The value of the property.</returns>
        [HttpGet("[action]({id})/{property}")]
        [EntityActionName("{EntityPluralized}")]
        public virtual string GetProperty(string id, string property)
            => _RestHandlerProvider.Provide<IGetPropertyHandler<TEntity, TInterface, TId>>()
                                   .Handle(id, property);

        /// <summary>
        /// Gets the distinct values of one of the entity's properties. 
        /// </summary>
        /// <param name="propertyName">A primitive property of the entity id.</param>
        /// <returns>The distinct values of the property.</returns>
        [HttpGet("[action]/{propertyName}/Distinct")]
        [EntityActionName("{EntityPluralized}")]
        public virtual List<object> GetDistinctPropertyValues(string propertyName)
            => _RestHandlerProvider.Provide<IGetDistinctPropertyValuesHandler<TEntity, TInterface, TId>>()
                                   .Handle(propertyName);

        #region IDisposable

        public void Dispose()
        {
        }

        #endregion
    }
}