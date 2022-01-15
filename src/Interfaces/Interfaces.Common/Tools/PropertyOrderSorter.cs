using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Tools
{
    public class PropertyOrderSorter : IPropertyOrderSorter
    {
        private readonly IPreferentialPropertyComparer _Comparer;

        public PropertyOrderSorter(IPreferentialPropertyComparer comparer)
        {
            _Comparer = comparer;
        }

        /// <summary>
        /// This method sorts EntityProperties first by sort order,
        /// and then by Name. The second sort uses the provided
        /// comparer so sorting by name can be more than just
        /// alphabetical.
        /// </summary>
        /// <typeparam name="T">The time</typeparam>
        /// <typeparam name="TKey">The type of the property in the first sort.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="groupSelector">The first sortable lambda: x=>x.Order</param>
        /// <param name="stringSelector">The second sortable: x=>x.Name</param>
        /// <returns>The list sorted.</returns>
        public IDictionary<TKey, List<T>> Sort<T, TKey>(IEnumerable<T> list, 
                                                        Func<T, TKey> groupSelector, 
                                                        Func<T, TKey> sortSelector, 
                                                        Func<T, string> stringSelector)
        {
            return list.GroupBy(groupSelector)
                       .ToDictionary(g => g.Key,
                                   g => g.OrderBy(sortSelector)
                                       .ThenBy(stringSelector, _Comparer)
                                       .ToList());
        }

        /// <summary>
        /// Every time an EntityProperty is added, deleted,
        /// or the sort order changes, all EntityProperty.Order
        /// values for a given EntityId should be reordered
        /// from 1 to N.
        /// </summary>
        /// <param name="items">The items to </param>
        public IEnumerable<T> UpdateSortOrder<T>(IEnumerable<T> items)
            where T: ISortOrder
        {
            var updated = new List<T>();
            var list = items?.ToList();
            if (list == null || !list.Any())
                return updated;
            for (int i = 0; i < list.Count; i++)
            {
                int newOrder = i + 1;
                if (list[i].Order == newOrder)
                    continue;
                list[i].Order = newOrder;
                updated.Add(list[i]);
            }
            return updated;
        }

        /// <summary>
        /// This takes items and adds them in.
        /// </summary>
        /// <param name="existingMap"></param>
        /// <param name="orderedEntities">A list of items to collate in
        /// order. It is assumed they are already ordered. See remarks.</param>
        /// <remarks>If you were had A:1,D:2,E:3,F:4 and wanted to add two
        /// items, B:2,C:3, but you provided C:3,B:2, then the order would be
        /// incorrect: A:1,B:2,D:3,C:4,E:5,F:6. So sort them first.</remarks>
        public void Collate<T, TKey>(IDictionary<TKey, List<T>> existingMap, IDictionary<TKey, List<T>> newMap, Func<T, TKey> groupBySelector)
            where T : ISortOrder
        {
            foreach (var kvp in newMap)
            {
                if (!existingMap.TryGetValue(kvp.Key, out List<T> list))
                    existingMap.Add(kvp.Key, list = new List<T>());
                foreach (var item in kvp.Value)
                {
                    SafeInsert(list, item);
                }
            }
        }

        public void SafeInsert<T>(List<T> list, T item) where T : ISortOrder
        {
            if (list.Count > item.Order)
                list.Insert(item.Order - 1, item);
            else
                list.Add(item);
        }
    }
}
