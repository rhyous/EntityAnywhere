using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IPropertyOrderSorter
    {
        void Collate<T, TKey>(IDictionary<TKey, List<T>> existingMap, IDictionary<TKey, List<T>> newMap, Func<T, TKey> groupBySelector) where T : ISortOrder;
        void SafeInsert<T>(List<T> list, T item) where T : ISortOrder;
        IDictionary<TKey, List<T>> Sort<T, TKey>(IEnumerable<T> list, Func<T, TKey> groupSelector, Func<T, TKey> sortSelector, Func<T, string> stringSelector);
        IEnumerable<T> UpdateSortOrder<T>(IEnumerable<T> items) where T : ISortOrder;
    }
}