using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>Extension methods for <see cref="IEnumerable{T}"/></summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Replaces the ToHashSet extension method that is missing from in LINQ in .net standard 2.0
        /// </summary>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items, IEqualityComparer<T> comparer = null)
        {
            return new HashSet<T>(items, comparer);
        }
    }
}
