using Rhyous.Odata;
using Rhyous.Odata.Csdl;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityWebServiceReadOnly<TEntity, TId> : IMetadataWebService<CsdlEntity>, IDisposable
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        /// <summary>
        /// Gets count of entities.
        /// </summary>
        /// <returns>All Entities</returns>
        Task<int> GetCountAsync();

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>All Entities</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetAllAsync();

        /// <summary>
        /// Gets all entities with the provided ids.
        /// </summary>
        /// <param name="ids">A list of entity ids.</param>
        /// <returns>All entities with the provided ids.</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetByIdsAsync(List<TId> ids);

        /// <summary>
        /// Gets all entities with the provided values of a given property.
        /// </summary>
        /// <param name="collection">A list of values.</param>
        /// <returns>All entities with the provided values of a given property.</returns>
        Task<OdataObjectCollection<TEntity, TId>> GetByPropertyValuesAsync(string property, List<string> values);

        /// <summary>
        /// Gets an entity be a specific id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>The entity with the specified id.</returns>
        Task<OdataObject<TEntity, TId>> GetAsync(string id);

        /// <summary>
        /// Gets an entity's property value by a specific id and property name.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="property">The property name.</param>
        /// <returns>The value of the specific property of the specific entity.</returns>
        string GetProperty(string id, string property);

        /// <summary>
        /// Gets distinct values for a given property.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A distinct list of values.</returns>
        List<object> GetDistinctPropertyValues(string propertyName);
    }
}
