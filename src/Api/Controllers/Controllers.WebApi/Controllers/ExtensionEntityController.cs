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
    /// This allows for creating a generic entity controller.
    /// All generic entity controllers will use this path:
    ///   {host}/Api/{entity}Service/{entityPluralize}
    /// Example for a User entity:
    ///   {host}/Api/UserService/Users
    /// Then the HTTP verb (GET, POST, DELETE, PUT, PATCH) determines which method will be hit
    /// </summary>
    /// <typeparam name="TEntity">The Entity Type</typeparam>
    /// <typeparam name="TId">The entity</typeparam>
    [ApiController]
    [Route("api/[controller]")]
    [EntityGenericController]
    [Authorize(AuthenticationSchemes = TokenAuthenticationSchemeOptions.Name)]
    public class ExtensionEntityController<TEntity, TInterface> : EntityController<TEntity, TInterface, long>,
        IExtensionEntityWebService<TEntity>
        where TEntity : class, TInterface, new()
        where TInterface : IExtensionEntity, IBaseEntity<long>
    {
        public ExtensionEntityController(IRestHandlerProvider restHandlerProvider)
            : base(restHandlerProvider)
        {
        }

        [HttpPost("[action]/EntityIdentifiers")]
        [EntityActionName("{EntityPluralized}")]
        public OdataObjectCollection<TEntity, long> GetByEntityIdentifiers(List<EntityIdentifier> entityIdentifiers)
            => _RestHandlerProvider.Provide<IGetByEntityIdentifiers<TEntity, TInterface, long>>()
                                   .Handle(entityIdentifiers);

        [HttpPost("[action]/PropertyValuePairs")]
        [EntityActionName("{EntityPluralized}")]
        public OdataObjectCollection<TEntity, long> GetByPropertyValuePairs(List<PropertyValue> propertyValuePairs)
            => _RestHandlerProvider.Provide<IGetByPropertyValuePairsHandler<TEntity, TInterface, long>>()
                                   .Handle(propertyValuePairs);

        [HttpPost("[action]/{entity}/Ids")]
        [EntityActionName("{EntityPluralized}")]
        public OdataObjectCollection<TEntity, long> GetByEntityIds(string entity, List<string> ids)
            => _RestHandlerProvider.Provide<IGetByEntityIdentifiers<TEntity, TInterface, long>>()
                                   .Handle(entity, ids);

        [HttpGet("[action]/{entity}/{propertyName}/Distinct")]
        [EntityActionName("{EntityPluralized}")]
        public List<object> GetDistinctExtensionPropertyValues(string entity, string propertyName)
            => _RestHandlerProvider.Provide<IGetDistinctPropertyValuesHandler<TEntity, TInterface, long>>()
                                   .Handle(propertyName, e => e.Entity == entity);
    }
}