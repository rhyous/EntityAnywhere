using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The service interface for common Entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public interface IServiceCommon<TEntity, TInterface, TId> : IDisposable
        where TEntity : class, TInterface
        where TInterface : IId<TId>
    {
        /// <summary>
        /// Gets a count all entities after passing through parameters
        /// </summary>
        /// <returns>The count of all entities after passing through parameters</returns>
        Task<int> GetCountAsync(NameValueCollection parameters = null);

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>All entities</returns>
        List<TInterface> Get();

        /// <summary>
        /// Gets a list of entities based on the URL parameters passed in as a NameValueCollection.
        /// </summary>
        /// <param name="parameters">A list of url parameters</param>
        /// <returns>A list of entities based on the URL parameters passed in as a NameValueCollection</returns>
        Task<IQueryable<TInterface>> GetAsync(NameValueCollection parameters);

        /// <summary>
        /// Gets a list of entities based on the ids.
        /// </summary>
        /// <param name="ids">The list of ids to return.</param>
        /// <param name="parameters">A list of url parameters</param>
        /// <returns>All entities where the entity id is contained in the list of ids provided</returns>
        Task<IQueryable<TInterface>> GetAsync(IEnumerable<TId> ids, NameValueCollection parameters);

        /// <summary>
        /// Gets a list of entities based on the property values.
        /// </summary>
        /// <param name="property">The property name of a valid property of TEntity.</param>
        /// <param name="values">The list of values to match against a property.</param>
        /// <param name="parameters">A list of url parameters</param>
        /// <returns>All entities where the property value is in the list of values provided</returns>
        Task<IQueryable<TInterface>> GetAsync(string property, IEnumerable<string> values, NameValueCollection parameters);

        /// <summary>
        /// Gets an entity based on its id.
        /// </summary>
        /// <param name="id">the id of the entity to get.</param>
        /// <returns>An entity based on its id</returns>
        TInterface Get(TId id);

        /// <summary>
        /// Gets a list of entities based on the expression passed in.
        /// </summary>
        /// <param name="expression">An expression that determines which entities to return.</param>
        /// <returns>A list of entities that match the expression.</returns>
        IQueryable<TInterface> Get(Expression<Func<TEntity, bool>> expression, int take = -1, int skip = -1);

        /// <summary>
        /// Gets a list of entities based on both an expression and a custom IQueryable<![CDATA[>]]> passed in.
        /// </summary>
        /// <param name="queryableModifier">A function that modifies and IQueryable<![CDATA[>]]></param>
        /// <returns>A list of returned objects.</returns>
        List<TInterface> Get(Func<IQueryable<TInterface>, IEnumerable<TInterface>> queryableModifier, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Gets the value of a specific property of an entity of the specified id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="property">The property to get the value from.</param>
        /// <returns>The value of a specific property of an entity of the specified id.</returns>
        string GetProperty(TId id, string property);

        /// <summary>
        /// Updates the value of a specific property of an entity of the specified id
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="property">The property to get the value from.</param>
        /// <param name="value">The new value.</param>
        /// <returns>The new value.</returns>
        string UpdateProperty(TId id, string property, string value);

        /// <summary>
        /// Updates the value of a specific property of an entity of the specified id. This is different than Replace in that replace changes all properties, but this only replaces specified properties.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="patchedEntity">The patched Entity which has an instance of the entity with the new values and a list of changed properties. The entity can be a stub entity where all unchanged properties are null.</param>
        /// <returns>The updated entity.</returns>
        TInterface Update(TId id, PatchedEntity<TInterface, TId> patchedEntity);

        /// <summary>
        /// Updates the value of a specific property of an entity of the specified id. This is different than Replace in that replace changes all properties, but this only replaces specified properties.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="patchedEntities">The patched Entities, each of which has an instance of the entity with the new values and a list of changed properties. The entity can be a stub entity where all unchanged properties are null.</param>
        /// <returns>The updated entities.</returns>
        List<TInterface> Update(PatchedEntityCollection<TInterface, TId> patchedEntityCollection);

        /// <summary>
        /// Adds the entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        Task<TInterface> AddAsync(TInterface entity);

        /// <summary>
        /// Adds a list of entities.
        /// </summary>
        /// <param name="entities">A list of entities to add.</param>
        /// <returns>The list of added entities.</returns>
        Task<List<TInterface>> AddAsync(IEnumerable<TInterface> entities);

        /// <summary>
        /// Replaces an entity at the given Id with the supplied entity. This is different than update in that update only changes certain properties. This would change all properties.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="entity">An instance of the entity with the new values. It all entity property values should be accurate.</param>
        /// <returns>The replaced entity.</returns>
        TInterface Replace(TId id, TInterface entity);

        /// <summary>
        /// Deletes the entity with the specified id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>True of deleted, false otherwise. If the entity doesn't exist, this should return true.</returns>
        bool Delete(TId id);

        /// <summary>
        /// Deletes the entity with the specified id.
        /// </summary>
        /// <param name="ids">The ids of the entities to delete.</param>
        /// <returns>True of deleted, false otherwise. If the entity doesn't exist, this should return true.</returns>
        Dictionary<TId, bool> DeleteMany(IEnumerable<TId> ids);

        /// <summary>
        /// This will ask the repository to generate whatever it needs to exist.
        /// For example, if the repo is SQL and Entity Framework, it would create a table.
        /// It does not create the database.
        /// </summary>
        /// <returns>RepositoryGenerationResult</returns>
        RepositoryGenerationResult GenerateRepository();

        /// <summary>
        /// This will check if an entity has seed data and if so, insert it into the repository.
        /// </summary>
        /// <returns>RepositorySeedResult</returns>
        RepositorySeedResult InsertSeedData();
    }
}