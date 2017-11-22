using Rhyous.Odata;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The service interface for common Entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public interface IServiceCommon<TEntity, TInterface, TId>
        where TEntity : class, TInterface
        where TInterface : IId<TId>
    {
        /// <summary>
        /// This is the entity's repo. Each entity gets its own specific repo.
        /// </summary>
        IRepository<TEntity, TInterface, TId> Repo { get; set; }

        /// <summary>
        /// Gets a count all entities
        /// </summary>
        /// <returns>The count of all entities</returns>
        int GetCount();

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>All entities</returns>
        List<TInterface> Get();

        /// <summary>
        /// Gets a list of entities based on the URL parameters passed in as a NameValueCollection.
        /// </summary>
        /// <returns>A list of entities based on the URL parameters passed in as a NameValueCollection</returns>
        List<TInterface> Get(NameValueCollection parameters);

        /// <summary>
        /// Gets a list of entities based on the ids.
        /// </summary>
        /// <param name="ids">The list of ids to return.</param>
        /// <returns>All entities where the entity id is contained in the list of ids provided</returns>
        List<TInterface> Get(List<TId> ids);

        /// <summary>
        /// Gets a list of entities based on the property values.
        /// </summary>
        /// <param name="property">The property name of a valid property of TEntity.</param>
        /// <param name="values">The list of values to match against a property.</param>
        /// <returns>All entities where the property value is in the list of values provided</returns>
        List<TInterface> Get(string property, IEnumerable<string> values);

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
        List<TInterface> Get(Expression<Func<TEntity, bool>> expression, int take = -1, int skip = -1);

        /// <summary>
        /// Gets a list of entities based on the custom IQueryable<![CDATA[>]]> passed in.
        /// </summary>
        /// <param name="queryableModifier">A function that modifies and IQueryable<![CDATA[>]]></param>
        /// <returns>A list of returned objects.</returns>
        List<TInterface> Get(Func<IQueryable<TInterface>, List<TInterface>> queryableModifier);

        /// <summary>
        /// Gets a list of entities based on both an expression and a custom IQueryable<![CDATA[>]]> passed in.
        /// </summary>
        /// <param name="queryableModifier">A function that modifies and IQueryable<![CDATA[>]]></param>
        /// <returns>A list of returned objects.</returns>
        List<TInterface> Get(Func<IQueryable<TInterface>, List<TInterface>> queryableModifier, Expression<Func<TEntity, bool>> expression);

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
        /// <param name="entity">An instance of the entity with the new values. It can be a stub entity where all unchanged properties are null.</param>
        /// <param name="changedProperties">A list of changed properties.</param>
        /// <returns></returns>
        TInterface Update(TId id, TInterface entity, List<string> changedProperties);

        /// <summary>
        /// Adds the entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        TInterface Add(TInterface entity);

        /// <summary>
        /// Adds a list of entities.
        /// </summary>
        /// <param name="entities">A list of entities to add.</param>
        /// <returns>The list of added entities.</returns>
        List<TInterface> Add(IList<TInterface> entities);

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
        /// Gets related entities for the type TEntity.
        /// Related entities are specified on TEntity properties using the RelatedEntityAttribute.
        /// </summary>
        /// <param name="entity">The entity to get related entities for.</param>
        /// <param name="parameters"></param>
        /// <returns>Related entites as raw json strings.</returns>
        List<RelatedEntityCollection> GetRelatedEntities(TInterface entity, NameValueCollection parameters = null);

        /// <summary>
        /// Gets related entities for the type TEntity given the passed in list of instances.
        /// Related entities are specified on TEntity properties using the RelatedEntityAttribute.
        /// </summary>
        /// <param name="entities">A list of entities to get related entities for.</param>
        /// <param name="parameters"></param>
        /// <returns>Related entites as raw json strings.</returns>

        List<RelatedEntityCollection> GetRelatedEntities(IEnumerable<TInterface> entities, NameValueCollection parameters = null);
    }
}