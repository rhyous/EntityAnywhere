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
        /// Every time an EntityProperty is added, deleted, or the sort Order property changes, all
        /// EntityProperty.Order values for a given EntityId should be reordered from 1 to N.
        /// </summary>
        /// <param name="items">The items in which the the sort Order property changed.</param>
        /// <remarks>
        /// Big O in best case = Big O(n), average = Big O(n), and worst = Big O(n).
        /// Update/Delete = could be updated to best case = Big O(1), average = Big O(n-f) (where F is first order that changes), and worst = Big O(n)
        /// However, this is used for Entity Properties today, and so 
        /// </remarks>
        public IEnumerable<T> UpdateSortOrder<T, TKey>(IEnumerable<T> items, Func<T, TKey> keySelector, IComparer<TKey> comparer)
            where T : ISortOrder
        {
            if (items == null || !items.Any())
                return new List<T>();
            var list = items.ToList(); // Avoid multiple enumerations
            var updatedMap = new Dictionary<T, int>();
            var updated = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                updatedMap.Add(item, item.Order);
                list[i].Order = i + 1;
                if (item.Order != updatedMap[item])
                    updated.Add(item);
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
            if (item.Order > 0 && list.Count > item.Order)
                list.Insert(item.Order - 1, item);
            else
                list.Add(item);
        }
    }
}