using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The Repository interface for common Entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public interface IRepository<TEntity,TInterface, TId>
        where TEntity : TInterface
    {
        /// <summary>
        /// List all items of T.
        /// </summary>
        /// <param name="orderBy">The name of the property to order by.</param>
        /// <returns>A list of all items of T</returns>
        IQueryable<TInterface> Get(bool order = false, string orderBy = "Id");

        /// <summary>
        /// List all items of T.
        /// </summary>
        /// <param name="orderExpression">The expression of the property to order by.</param>
        /// <returns>A list of all items of T</returns>
        IQueryable<TInterface> Get(Expression<Func<TEntity, TId>> orderExpression);

        /// <summary>
        /// List all items of T with the given ids.
        /// </summary>
        /// <returns>A list of all items of T with the give ids</returns>
        IQueryable<TInterface> Get(List<TId> ids);

        /// <summary>
        /// Gets an item by Id
        /// </summary>
        /// <param name="userId">The id of the item to return.</param>
        /// <returns></returns>
        TInterface Get(TId id);

        /// <summary>
        /// Gets an item by comparing a key string property, such as Name, to its value
        /// </summary>
        /// <param name="value">The string value the property expression must be equal to
        /// to find the item to return.</param>
        /// <returns></returns>
        TInterface Get(string name, Expression<Func<TEntity, string>> propertyExpression);

        /// <summary>
        /// List items that match the query expression.
        /// </summary>
        /// <param name="expression">A LINQ expression</param>
        /// <param name="orderBy">The name of the property to order by.</param>
        /// <returns>A list of all items that match the expression</returns>
        IQueryable<TInterface> GetByExpression(Expression<Func<TEntity, bool>> expression, string orderBy = "Id");

        /// <summary>
        /// List items that match the query expression.
        /// </summary>
        /// <param name="expression">A LINQ expression</param>
        /// <param name="orderExpression">The expression of the property to order by.</param>
        /// <returns>A list of all items that match the expression</returns>
        IQueryable<TInterface> GetByExpression(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TId>> orderExpression);

        /// <summary>
        /// List items that match the query string.
        /// </summary>
        /// <returns>A list of all items that match the query string</returns>
        IQueryable<TInterface> Search(string searchString, params Expression<Func<TEntity, string>>[] propertyExpressions);

        /// <summary>
        /// A item(s) to add.
        /// </summary>
        /// <param name="item">One or more items.</param>
        /// <returns></returns>
        List<TInterface> Create(IList<TInterface> items);

        /// <summary>
        /// The item to change. The item Id must be specific in the item object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="changedProperties">The properties of the item object to change. If null or empty list, all properties will be changed.</param>
        /// <returns></returns>
        TInterface Update(TInterface item, IEnumerable<string> changedProperties);

        /// <summary>
        /// Delete the item.
        /// </summary>
        /// <param name="id">The Id of the item to delete.</param>
        /// <returns></returns>
        bool Delete(TId id);
    }
}
