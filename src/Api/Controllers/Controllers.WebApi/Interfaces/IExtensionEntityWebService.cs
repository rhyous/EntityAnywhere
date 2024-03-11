using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Odata;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// A service contract for an Entity Extension. This inherits the service contract for a regular entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TId">The entity id type. However, because TEntity must implement IExtension entity, TId will always be a long.</typeparam>
    public interface IExtensionEntityWebService<TEntity> : IEntityWebService<TEntity, long>
        where TEntity : class, IExtensionEntity, new()
    {
        /// <summary>
        /// Gets a list of Entity Extensions by a list of EntityIdentifiers.
        /// </summary>
        /// <param name="entityIdentifiers">The list of entity identifiers, which includes Entity name and the Entity's id.</param>
        /// <returns>A list of Entity Extensions.</returns>
        /// <remarks>You can get Entity Extensions for multiple entities with this call.</remarks>
        OdataObjectCollection<TEntity, long> GetByEntityIdentifiers(List<EntityIdentifier> EntityIdentifiers);

        /// <summary>
        /// Gets a list of Entity Extensions by a list of Property Value Pairs.
        /// </summary>
        /// <param name="propertyValues">The list of Property Value Pairs, which includes Property and Value.</param>
        /// <returns>A list of Entity Extensions.</returns>
        /// <remarks>You can get Entity Extensions for multiple entities with this call.</remarks>
        OdataObjectCollection<TEntity, long> GetByPropertyValuePairs(List<PropertyValue> propertyValues);

        /// <summary>
        /// Gets a list of Entity Extensions by a list of EntityIds.
        /// </summary>
        /// <param name="entityIdList">The list of entity identifiers, which includes Entity name and a list of that Entity's id.</param>
        /// <returns>A list of Entity Extensions.</returns>
        /// <remarks>You can get Entity Extensions for only one entity with this call.</remarks>
        OdataObjectCollection<TEntity, long> GetByEntityIds(string entity, List<string> ids);

        /// <summary>
        /// Gets a list of Distinct Entity Extensions Properties by Entity.
        /// </summary>
        /// <param name="entity">The entity name.</param>
        /// <returns>A distinct list of Entity Extensions properties.</returns>
        List<object> GetDistinctExtensionPropertyValues(string entity, string propertyName);
    }
}