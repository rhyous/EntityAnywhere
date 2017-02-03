using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Interfaces
{
    public interface IRepository<T,Tinterface, Tid>
        where T : Tinterface
    {
        /// <summary>
        /// List all items of T.
        /// </summary>
        /// <returns>A list of all items of T</returns>
        List<Tinterface> Get();

        /// <summary>
        /// List all items of T with the given ids.
        /// </summary>
        /// <returns>A list of all items of T with the give ids</returns>
        List<Tinterface> Get(List<Tid> ids);

        /// <summary>
        /// Gets an item by Id
        /// </summary>
        /// <param name="userId">The id of the item to return.</param>
        /// <returns></returns>
        Tinterface Get(Tid id);

        /// <summary>
        /// Gets an item by Id
        /// </summary>
        /// <param name="value">The string value the property expression must be equal to
        /// to find the item to return.</param>
        /// <returns></returns>
        Tinterface Get(string name, Expression<Func<Tinterface, string>> propertyExpression);

        /// <summary>
        /// List items that match the query expression.
        /// </summary>
        /// <param name="expression">A LINQ expression</param>
        /// <returns>A list of all items that match the expression</returns>
        List<Tinterface> GetByExpression(Expression<Func<Tinterface, bool>> expression);

        /// <summary>
        /// List items that match the query string.
        /// </summary>
        /// <returns>A list of all items that match the query string</returns>
        List<Tinterface> Search(string searchString, params Expression<Func<Tinterface, string>>[] propertyExpressions);

        /// <summary>
        /// A item(s) to add.
        /// </summary>
        /// <param name="item">One or more items.</param>
        /// <returns></returns>
        List<Tinterface> Create(IList<Tinterface> items);

        /// <summary>
        /// The item to change. The item Id must be specific in the item object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="changedProperties">The properties of the item object to change. If null or empty list, all properties will be changed.</param>
        /// <returns></returns>
        Tinterface Update(Tinterface item, IEnumerable<string> changedProperties);

        /// <summary>
        /// Delete the item.
        /// </summary>
        /// <param name="id">The Id of the item to delete.</param>
        /// <returns></returns>
        bool Delete(Tid id);
    }
}
