using System;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// ArrayMaker gives the ability to Add to arrays as well as the ability to create an array using params.
    /// Also, params is limited in that it can be a list of single items or an array, but not both. This class overcomes that limitation.
    /// </summary>
    public static class ArrayMaker
    {
        /// <summary>
        /// Adds multiple items to the end of an array start at position. The array must already be sized appropriately. If it is not resized yet, call Add{T}(this T[] array, params T[] items).
        /// This edits the array in place. However, it also returns the array to provide one-line assignment capabilities.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="position">The position to start adding the items.</param>
        /// <param name="items">The items to add.</param>
        /// <returns></returns>
        public static T[] AddAt<T>(this T[] array, int position, params T[] items)
        {
            for (int i = 0; i < (items?.Length ?? 0); i++)
            {
                array[position + i] = items[i];
            }
            return array;
        }

        /// <summary>
        /// Resizes the array to fit the new items and then adds to the new items.
        /// It creates a clone of the array and cannot edit it in place because arrays are pased by value, not by reference.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same array.</returns>
        public static T[] Add<T>(this T[] array, params T[] items)
        {
            return Add(ref array, items);
        }

        /// <summary>
        /// Resizes the array to fit the new items and then adds to the new items.
        /// This edits the array in place. However, it also returns the array to provide one-line assignment capabilities.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same array.</returns>
        public static T[] Add<T>(ref T[] array, params T[] items)
        {
            var position = array.Length;
            Array.Resize(ref array, position + (items?.Length ?? 0));
            return array.AddAt(position, items);
        }

        /// <summary>
        /// Resizes the array to fit the new items and then adds to the new items.
        /// It creates a clone of the array and cannot edit it in place because arrays are pased by value, not by reference.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="item1">An item to add.</param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same array.</returns>
        public static T[] Add<T>(this T[] array, T item1, params T[] items)
        {
            return Add(ref array, item1, items);
        }

        /// <summary>
        /// Resizes the array to fit the new items and then adds to the new items.
        /// This edits the array in place. However, it also returns the array to provide one-line assignment capabilities.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="item1">An item to add.</param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same array.</returns>
        public static T[] Add<T>(ref T[] array, T item1, params T[] items)
        {
            var position = array.Length;
            Array.Resize(ref array, position + 1 + (items?.Length ?? 0));
            array.AddAt(position, item1);
            return array.AddAt(position + 1, items);
        }

        /// <summary>
        /// Resizes the array to fit the new items and then adds to the new items.
        /// It creates a clone of the array and cannot edit it in place because arrays are pased by value, not by reference.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="item1">An item to add.</param>
        /// <param name="item2">An item to add.</param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same array.</returns>
        public static T[] Add<T>(this T[] array, T item1, T item2, params T[] items)
        {
            return Add(ref array, item1, item2, items);
        }

        /// <summary>
        /// Resizes the array to fit the new items and then adds to the new items.
        /// This edits the array in place. However, it also returns the array to provide one-line assignment capabilities.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="item1">An item to add.</param>
        /// <param name="item2">An item to add.</param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same array.</returns>
        public static T[] Add<T>(ref T[] array, T item1, T item2, params T[] items)
        {
            var position = array.Length;
            Array.Resize(ref array, position + 2 + (items?.Length ?? 0));
            array.AddAt(position, item1, item2);
            return array.AddAt(position + 2, items);
        }

        /// <summary>
        /// Resizes the array to fit the new items and then adds to the new items.
        /// It creates a clone of the array and cannot edit it in place because arrays are pased by value, not by reference.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="item1">An item to add.</param>
        /// <param name="item2">An item to add.</param>
        /// <param name="item3">An item to add.</param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same array.</returns>
        public static T[] Add<T>(this T[] array, T item1, T item2, T item3, params T[] items)
        {
            return Add(ref array, item1, item2, item3, items);
        }

        /// <summary>
        /// Resizes the array to fit the new items and then adds to the new items.
        /// This edits the array in place. However, it also returns the array to provide one-line assignment capabilities.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="item1">An item to add.</param>
        /// <param name="item2">An item to add.</param>
        /// <param name="item3">An item to add.</param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same array.</returns>
        public static T[] Add<T>(ref T[] array, T item1, T item2, T item3, params T[] items)
        {
            var position = array.Length;
            Array.Resize(ref array, position + 3 + (items?.Length ?? 0));
            array.AddAt(position, item1, item2, item3);
            return array.AddAt(position + 3, items);
        }

        /// <summary>
        /// Resizes the array to fit the new items and then adds to the new items.
        /// It creates a clone of the array and cannot edit it in place because arrays are pased by value, not by reference.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="item1">An item to add.</param>
        /// <param name="item2">An item to add.</param>
        /// <param name="item3">An item to add.</param>
        /// <param name="item4">An item to add.</param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same array.</returns>
        public static T[] Add<T>(this T[] array, T item1, T item2, T item3, T item4, params T[] items)
        {
            return Add(ref array, item1, item2, item3, item4, items);
        }

        /// <summary>
        /// Resizes the array to fit the new items and then adds to the new items.
        /// This edits the array in place. However, it also returns the array to provide one-line assignment capabilities.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="item1">An item to add.</param>
        /// <param name="item2">An item to add.</param>
        /// <param name="item3">An item to add.</param>
        /// <param name="item4">An item to add.</param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same array.</returns>
        public static T[] Add<T>(ref T[] array, T item1, T item2, T item3, T item4, params T[] items)
        {
            var position = array.Length;
            Array.Resize(ref array, position + 4 + (items?.Length ?? 0));
            array.AddAt(position, item1, item2, item3, item4);
            return array.AddAt(position + 4, items);
        }

        /// <summary>
        /// Resizes the array to fit the new items and then adds to the new items.
        /// It creates a clone of the array and cannot edit it in place because arrays are pased by value, not by reference.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="item1">An item to add.</param>
        /// <param name="item2">An item to add.</param>
        /// <param name="item3">An item to add.</param>
        /// <param name="item4">An item to add.</param>
        /// <param name="item5">An item to add.></param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same array.</returns>
        public static T[] Add<T>(this T[] array, T item1, T item2, T item3, T item4, T item5, params T[] items)
        {
            return Add(ref array, item1, item2, item3, item4, item5, items);
        }

        /// <summary>
        /// Resizes the array to fit the new items and then adds to the new items.
        /// This edits the array in place. However, it also returns the array to provide one-line assignment capabilities.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="array">Extension method parameter for all arrays.</param>
        /// <param name="item1">An item to add.</param>
        /// <param name="item2">An item to add.</param>
        /// <param name="item3">An item to add.</param>
        /// <param name="item4">An item to add.</param>
        /// <param name="item5">An item to add.></param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same array.</returns>
        public static T[] Add<T>(ref T[] array, T item1, T item2, T item3, T item4, T item5, params T[] items)
        {
            var position = array.Length;
            Array.Resize<T>(ref array, position + 5 + (items?.Length ?? 0));
            array.AddAt(position, item1, item2, item3, item4, item5);
            return array.AddAt(position + 5, items);
        }

        /// <summary>
        /// Makes are array from parameters.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="items">The items to include when making the array.</param>
        /// <returns>The created array.</returns>
        public static T[] Make<T>(params T[] items)
        {
            return items;
        }

        /// <summary>
        /// Makes are array from parameters.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="items">The items to include when making the array.</param>
        /// <returns>The created array.</returns>
        public static T[] Make<T>(T item1, params T[] items)
        {
            var array = new T[(items?.Length ?? 0) + 1];
            array[0] = item1;
            return array.AddAt(1, items);
        }

        /// <summary>
        /// Makes are array from parameters.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="items">The items to include when making the array.</param>
        /// <returns>The created array.</returns>
        public static T[] Make<T>(T item1, T item2, params T[] items)
        {
            var array = new T[(items?.Length ?? 0) + 2];
            array.AddAt(0, item1, item2);
            return array.AddAt(2, items);
        }

        /// <summary>
        /// Makes are array from parameters.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="items">The items to include when making the array.</param>
        /// <returns>The created array.</returns>
        public static T[] Make<T>(T item1, T item2, T item3, params T[] items)
        {
            var array = new T[(items?.Length ?? 0) + 3];
            array.AddAt(0, item1, item2, item3);
            return array.AddAt(3, items);
        }

        /// <summary>
        /// Makes are array from parameters.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="items">The items to include when making the array.</param>
        /// <returns>The created array.</returns>
        public static T[] Make<T>(T item1, T item2, T item3, T item4, params T[] items)
        {
            var array = new T[(items?.Length ?? 0) + 4];
            array.AddAt(0, item1, item2, item3, item4);
            return array.AddAt(4, items);
        }

        /// <summary>
        /// Makes are array from parameters.
        /// </summary>
        /// <typeparam name="T">The type the array holds.</typeparam>
        /// <param name="items">The items to include when making the array.</param>
        /// <returns>The created array.</returns>
        public static T[] Make<T>(T item1, T item2, T item3, T item4, T item5, params T[] items)
        {
            var array = new T[(items?.Length ?? 0) + 5];
            array.AddAt(0, item1, item2, item3, item4, item5);
            return array.AddAt(5, items);
        }
    }
}
