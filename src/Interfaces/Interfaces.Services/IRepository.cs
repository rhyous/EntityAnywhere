using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The Repository interface for common Entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string,
    /// etc...</typeparam>
    public interface IRepository<TEntity,TInterface, TId> : IDisposable
        where TEntity : TInterface
        where TInterface : IId<TId>
    {
        /// <summary>
        /// List all items of T.
        /// </summary>
        /// <param name="orderBy">The name of the property to order by.</param>
        /// <param name="sortOrder">The direction to sort.</param>
        /// <returns>A list of all items of T</returns>
        IQueryable<TInterface> Get(string orderBy = "Id", SortOrder sortOrder = SortOrder.Ascending);

        /// <summary>
        /// List all items of T.
        /// </summary>
        /// <param name="orderBy">The name of the property to order by.</param>
        /// <param name="sortOrder">The direction to sort.</param>
        /// <returns>A list of all items of T</returns>
        IQueryable<TInterface> Get<TProperty>(string orderBy, SortOrder sortOrder = SortOrder.Ascending);

        /// <summary>
        /// List all items of T.
        /// </summary>
        /// <param name="orderExpression">The expression of the property to order by.</param>
        /// <returns>A list of all items of T</returns>
        IQueryable<TInterface> Get(Expression<Func<TEntity, TId>> orderExpression);

        /// <summary>
        /// List all items of T.
        /// </summary>
        /// <param name="orderExpression">The expression of the property to order by.</param>
        /// <returns>A list of all items of T</returns>
        IQueryable<TInterface> Get<TProperty>(Expression<Func<TEntity, TProperty>> orderExpression);

        /// <summary>
        /// List all items of T with the given ids.
        /// </summary>
        /// <returns>A list of all items of T with the give ids</returns>
        IQueryable<TInterface> Get(IEnumerable<TId> ids);

        /// <summary>
        /// Gets an item by Id
        /// </summary>
        /// <param name="userId">The id of the item to return.</param>
        /// <returns></returns>
        TInterface Get(TId id);

        /// <summary>
        /// Gets an item by comparing a key string property, such as Name, to its value
        /// </summary>
        /// <param name="propertyValue">The string value the property expression must be equal to
        /// to find the item to return.</param>
        /// <returns></returns>
        TInterface Get<TResult>(TResult propertyValue, Expression<Func<TEntity, TResult>> propertyExpression)
                   where TResult : IComparable, IComparable<TResult>, IEquatable<TResult>;

        /// <summary>
        /// List items that match the query expression.
        /// </summary>
        /// <param name="expression">A LINQ expression</param>
        /// <param name="orderBy">The name of the property to order by.</param>
        /// <returns>A list of all items that match the expression</returns>
        IQueryable<TInterface> GetByExpression(Expression<Func<TEntity, bool>> expression, 
                                               string orderBy = "Id",
                                               SortOrder sortOrder = SortOrder.Ascending);

        /// <summary>
        /// This method allows you to use any property to order by. This only works if the TEntity is an 
        /// actual Entity. In the case of an IAuditable it will not work.
        /// </summary>
        /// <typeparam name="TProperty">The property type</typeparam>
        /// <param name="expression">The Where expression</param>
        /// <param name="orderExpression">The OrderBy expression</param>
        /// <returns><see cref="IQueryable{T}"/> of results</returns>
        IQueryable<TInterface> GetByExpression<TProperty>(Expression<Func<TEntity, bool>> expression, 
                                                          Expression<Func<TEntity, TProperty>> orderExpression, 
                                                          SortOrder sortOrder = SortOrder.Ascending);

        /// <summary>
        /// List items that match the query string.
        /// </summary>
        /// <returns>A list of all items that match the query string</returns>
        IQueryable<TInterface> Search<TResult>(TResult searchValue, 
                                      params Expression<Func<TEntity, TResult>>[] propertyExpressions);

        /// <summary>
        /// A item(s) to add.
        /// </summary>
        /// <param name="entities">One or more items.</param>
        /// <returns></returns>
        List<TInterface> Create(IEnumerable<TInterface> entities);

        /// <summary>
        /// The entity to change. The entity Id must be specified in the entity object.
        /// </summary>
        /// <param name="patchedEntity">The patchedEntity to update.</param>
        /// list, all properties will be changed.</param>
        /// <param name="stage">If false, the entity must be updated in the repo. If false, update the 
        /// entity without updating the repo.</param>
        /// <returns>The updated entity</returns>
        TInterface Update(PatchedEntity<TInterface, TId> patchedEntity, bool stage = false);

        /// <summary>
        /// The entity to change. The entity Id must be specified in the entity object.
        /// </summary>
        /// <param name="entitiesToUpdate">The entities to update.</param>
        /// <param name="changedProperties">The properties of the entity object to change. If null or empty 
        /// list, all properties will be changed.</param>
        /// <param name="stage">If false, the entity must be updated in the repo. If false, update the 
        /// entity without updating the repo.</param>
        /// <returns>The updated entity</returns>
        List<TInterface> BulkUpdate(PatchedEntityCollection<TInterface, TId> patchedEntityCollection, bool stage = false);

        /// <summary>
        /// Delete the item.
        /// </summary>
        /// <param name="id">The Id of the item to delete.</param>
        /// <returns></returns>
        bool Delete(TId id);

        /// <summary>
        /// Delete the item.
        /// </summary>
        /// <param name="ids">The Ids of the items to delete.</param>
        /// <returns></returns>
        Dictionary<TId,bool> DeleteMany(IEnumerable<TId> ids);

        /// <summary>
        /// This will ask the repository to generate whatever it needs to exist. 
        /// For example, if the repo is SQL and Entity Framework, it would create a table.
        /// It does not create the database.
        /// </summary>
        /// <returns>RepositoryGenerationResult</returns>
        RepositoryGenerationResult GenerateRepository();
    }
}