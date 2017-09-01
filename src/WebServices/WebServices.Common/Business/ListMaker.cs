using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    public static class ListMaker
    {
        public static List<T> Add<T>(this List<T> list, params T[] items)
        {
            list.AddRange(items);
            return list;
        }

        public static List<T> Add<T>(this List<T> list, T item1, params T[] items)
        {
            list.Add(item1);
            list.AddRange(items.ToList());
            return list;
        }

        public static List<T> Add<T>(this List<T> list, T item1, T item2, params T[] items)
        {
            list.Add(new[] { item1, item2 });
            list.AddRange(items.ToList());
            return list;
        }

        public static List<T> Add<T>(this List<T> list, T item1, T item2, T item3, params T[] items)
        {
            list.Add(new[] { item1, item2, item3 });
            list.AddRange(items.ToList());
            return list;
        }

        public static List<T> Add<T>(this List<T> list, T item1, T item2, T item3, T item4, params T[] items)
        {
            list.Add(new[] { item1, item2, item3, item4 });
            list.AddRange(items.ToList());
            return list;
        }

        public static List<T> Add<T>(this List<T> list, T item1, T item2, T item3, T item4, T item5, params T[] items)
        {
            list.Add(new[] { item1, item2, item3, item4, item5 });
            list.AddRange(items.ToList());
            return list;
        }

        public static List<T> Make<T>(params T[] items)
        {
            return items.ToList();
        }

        public static List<T> Make<T>(T item1, params T[] items)
        {
            return new List<T>().Add(item1, items);
        }

        public static List<T> Make<T>(T item1, T item2, params T[] items)
        {
            return new List<T>().Add(item1, item2, items);
        }

        public static List<T> Make<T>(T item1, T item2, T item3, params T[] items)
        {
            return new List<T>().Add(item1, item2, item3, items);
        }

        public static List<T> Make<T>(T item1, T item2, T item3, T item4, params T[] items)
        {
            return new List<T>().Add(item1, item2, item3, item4, items);
        }

        public static List<T> Make<T>(T item1, T item2, T item3, T item4, T item5, params T[] items)
        {
            return new List<T>().Add(item1, item2, item3, item4, item5, items);
        }
    }
}
