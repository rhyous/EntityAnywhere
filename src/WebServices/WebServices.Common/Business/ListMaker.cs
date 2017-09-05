using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// ListMaker gives the ability to create lists using params. It also provide the feature to add to a list with an extension method using params. 
    /// Also, params is limited in that it can be a list of single items or an array, but not both. This class overcomes that limitation. You can have single entities, then end with a list.
    /// Single line list instantiation is desired but doesn't work because AddRange doesn't return the original list: var list = new List<object>{ item1, item2 }.AddRange(list1);
    /// Single line list instantiation can now be done: var list = new List<object>().Add(item1, item2, list1);
    /// </summary>
    public static class ListMaker
    {
        /// <summary>
        /// Allows 
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The original list.</param>
        /// <param name="items">The items to add to the list.</param>
        /// <returns>The updated list. If the caller already has a reference to the updated list, the return can be ignored.</returns>
        public static List<T> Add<T>(this List<T> list, params T[] items)
        {
            list.AddRange(items);
            return list;
        }
        /// <summary>
        /// This method allows the caller to add both a single item and a list of items to a list in one method.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The original list.</param>
        /// <param name="item1">A single item to add.</param>
        /// <param name="items">A list of items to add to the list.</param>
        /// <returns>The updated list. If the caller already has a reference to the updated list, the return can be ignored.</returns>
        public static List<T> Add<T>(this List<T> list, T item1, params T[] items)
        {
            list.Add(item1);
            list.AddRange(items.ToList());
            return list;
        }
        /// <summary>
        /// This method allows the caller to add two single items and a list of items to a list in one method.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The original list.</param>
        /// <param name="item1">A single item to add.</param>
        /// <param name="item2">A single item to add.</param>
        /// <param name="items">A list of items to add to the list.</param>
        /// <returns>The updated list. If the caller already has a reference to the updated list, the return can be ignored.</returns
        public static List<T> Add<T>(this List<T> list, T item1, T item2, params T[] items)
        {
            list.Add(new[] { item1, item2 });
            list.AddRange(items.ToList());
            return list;
        }
        /// <summary>
        /// This method allows the caller to add three single items and a list of items to a list in one method.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The original list.</param>
        /// <param name="item1">A single item to add.</param>
        /// <param name="item2">A single item to add.</param>
        /// <param name="item3">A single item to add.</param>
        /// <param name="items">A list of items to add to the list.</param>
        /// <returns>The updated list. If the caller already has a reference to the updated list, the return can be ignored.</returns
        public static List<T> Add<T>(this List<T> list, T item1, T item2, T item3, params T[] items)
        {
            list.Add(new[] { item1, item2, item3 });
            list.AddRange(items.ToList());
            return list;
        }
        /// <summary>
        /// This method allows the caller to add four single items and a list of items to a list in one method.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The original list.</param>
        /// <param name="item1">A single item to add.</param>
        /// <param name="item2">A single item to add.</param>
        /// <param name="item3">A single item to add.</param>
        /// <param name="item4">A single item to add.</param>
        /// <param name="items">A list of items to add to the list.</param>
        /// <returns>The updated list. If the caller already has a reference to the updated list, the return can be ignored.</returns
        public static List<T> Add<T>(this List<T> list, T item1, T item2, T item3, T item4, params T[] items)
        {
            list.Add(new[] { item1, item2, item3, item4 });
            list.AddRange(items.ToList());
            return list;
        }
        /// <summary>
        /// This method allows the caller to add five single items and a list of items to a list in one method.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The original list.</param>
        /// <param name="item1">A single item to add.</param>
        /// <param name="item2">A single item to add.</param>
        /// <param name="item3">A single item to add.</param>
        /// <param name="item4">A single item to add.</param>
        /// <param name="item5">A single item to add.</param>
        /// <param name="items">A list of items to add to the list.</param>
        /// <returns>The updated list. If the caller already has a reference to the updated list, the return can be ignored.</returns
        public static List<T> Add<T>(this List<T> list, T item1, T item2, T item3, T item4, T item5, params T[] items)
        {
            list.Add(new[] { item1, item2, item3, item4, item5 });
            list.AddRange(items.ToList());
            return list;
        }

        /// <summary>
        /// This method creates a list with a single item and an existing list. This method exists because this doesn't work:
        /// var list = new List<object>{ item1 }.AddRange(list1)
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="item1">A single item to add.</param>
        /// <param name="items">A list of items to add to the list.</param>
        /// <returns>A new list.</returns>
        public static List<T> Make<T>(T item1, params T[] items)
        {
            return new List<T>().Add(item1, items);
        }

        /// <summary>
        /// This method creates a list with two single items and an existing list. This method exists because this doesn't work:
        /// var list = new List<object>{ item1, item2 }.AddRange(list1)
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="item1">A single item to add.</param>
        /// <param name="item2">A single item to add.</param>
        /// <param name="items">A list of items to add to the list.</param>
        /// <returns>A new list.</returns>
        public static List<T> Make<T>(T item1, T item2, params T[] items)
        {
            return new List<T>().Add(item1, item2, items);
        }

        /// <summary>
        /// This method creates a list with three single items and an existing list. This method exists because this doesn't work:
        /// var list = new List<object>{ item1, item2, item3 }.AddRange(list1)
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="item1">A single item to add.</param>
        /// <param name="item2">A single item to add.</param>
        /// <param name="item3">A single item to add.</param>
        /// <param name="items">A list of items to add to the list.</param>
        /// <returns>A new list.</returns>
        public static List<T> Make<T>(T item1, T item2, T item3, params T[] items)
        {
            return new List<T>().Add(item1, item2, item3, items);
        }
        
        /// <summary>
        /// This method creates a list with four single items and an existing list. This method exists because this doesn't work:
        /// var list = new List<object>{ item1, item2, item3, item4 }.AddRange(list1)
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="item1">A single item to add.</param>
        /// <param name="item2">A single item to add.</param>
        /// <param name="item3">A single item to add.</param>
        /// <param name="item4">A single item to add.</param>
        /// <param name="items">A list of items to add to the list.</param>
        /// <returns>A new list.</returns>
        public static List<T> Make<T>(T item1, T item2, T item3, T item4, params T[] items)
        {
            return new List<T>().Add(item1, item2, item3, item4, items);
        }

        /// <summary>
        /// This method creates a list with five single items and an existing list. This method exists because this doesn't work:
        /// var list = new List<object>{ item1, item2, item3, item4, item5 }.AddRange(list1)
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="item1">A single item to add.</param>
        /// <param name="item2">A single item to add.</param>
        /// <param name="item3">A single item to add.</param>
        /// <param name="item4">A single item to add.</param>
        /// <param name="item5">A single item to add.</param>
        /// <param name="items">A list of items to add to the list.</param>
        /// <returns>A new list.</returns>
        public static List<T> Make<T>(T item1, T item2, T item3, T item4, T item5, params T[] items)
        {
            return new List<T>().Add(item1, item2, item3, item4, item5, items);
        }
    }
}
