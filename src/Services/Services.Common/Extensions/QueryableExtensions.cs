using System.Linq;

namespace Rhyous.EntityAnywhere.Services
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Only adds take to the list of the take value is greater than 0.
        /// </summary>
        /// <typeparam name="T">The queryable type.</typeparam>
        /// <param name="queryable">The object to add this extension method to: IQuerable<T>.</param>
        /// <param name="take">the number to take. 0 or less does not include Take() to the queryable.</param>
        /// <returns>Returns queryable as is if take is 0 or less. Returns an IQueriable<T> with take added to it if take is greater than 0.</returns>
        public static IQueryable<T> IfTake<T>(this IQueryable<T> queryable, int take)
        {
            return (take > 0) ? queryable.Take(take) : queryable;
        }

        /// <summary>
        /// Only adds Skip to the list of the skip value is greater than 0.
        /// </summary>
        /// <typeparam name="T">The queryable type.</typeparam>
        /// <param name="queryable">The object to add this extension method to: IQuerable<T>.</param>
        /// <param name="skip">the number to skip. 0 or less does not not include Skip() to the queryable.</param>
        /// <returns>Returns queryable as is if skip is 0 or less. Returns an IQueriable<T> with Skip added to it if skip is greater than 0.</returns>
        public static IQueryable<T> IfSkip<T>(this IQueryable<T> queryable, int skip)
        {
            return (skip > 0) ? queryable.Skip(skip) : queryable;
        }
    }
}
