using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    public static class IEnumerableExtensions
    {        
        // 10-100 Rule Exception - Algorithm 
        public static bool UnorderedEquals<T>(this IEnumerable<T> left, IEnumerable<T> right, IEqualityComparer<T> comparer = null)
        {
            if (left == null && right == null) // Scenario 1 - both null
                return true;
            if (left == null || right == null) // Scenario 2 - one null one not null
                return false;
            if (!left.Any() && !right.Any())   // Scenario 3 - Both are instantiated but empty lists
                return true;
            if (!left.Any() || !right.Any())   // Scenario 4 - Both are instantiated but only one is an empty list
                return false;
            var leftList = left.ToList();
            var rightList = right.ToList();
            if (leftList.Count != rightList.Count) // Scenario 5 - Both are instantiated but have different number of items.
                return false;
            var countDictionary = new Dictionary<T, int>(comparer);
            // Scenario 6 - We have to compare the actual elements
            foreach (var item in leftList)
            {
                if (item == null)
                    continue; // No action needed
                if (!countDictionary.TryGetValue(item, out int _))
                    countDictionary.Add(item, 1);
                else
                    ++countDictionary[item];
            }
            foreach (var item in rightList)
            {
                if (item == null)
                    continue; // No action needed
                if (!countDictionary.TryGetValue(item, out int _))
                    return false;
                --countDictionary[item];
            }
            return countDictionary.All(i => i.Value == 0);
            // Why no action is needed for nulls?
            // - If there are different number of items, the comparison is false.
            // - If there are the same number of items, but different number of nulls,
            //   countDictionary.All(i => i.Value == 0) will always be false.
        }

        public enum LeftOrRight { Left, Right }

        /// <summary>
        /// Used as a return type for IEnumerable<T>.GetMisMatchedItems().
        /// </summary>
        /// <typeparam name="T">The type of the lists being compared.</typeparam>
        public class MismatchedItems<T> : IEnumerable<T>
        {
            public List<T> Right
            {
                get { return _Right ?? (_Right = new List<T>()); }
                set { _Right = value; }
            } private List<T> _Right;
            public List<T> Left
            {
                get { return _Left ?? (_Left = new List<T>()); }
                set { _Left = value; }
            } private List<T> _Left;

            public int Count => Right.Count + Left.Count;

            public IEnumerator<T> GetEnumerator() => Left.Concat(Right).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// Returns the differences between two unordered sequences of items.
        /// </summary>
        /// <typeparam name="T">The type of the items in the sequences to compare.</typeparam>
        /// <param name="left">The left sequence of items.</param>
        /// <param name="right">The right sequence of items.</param>
        /// <param name="comparer">A custom comparer.</param>
        /// <returns></returns>
        /// <remarks>10-100 Rule Exception - Algorithm</remarks>
        public static MismatchedItems<T> GetMismatchedItems<T>(this IEnumerable<T> left, IEnumerable<T> right, IEqualityComparer<T> comparer = null)
        {
            var mismatchedItems = new MismatchedItems<T>();
            if (left == null && right == null)
                return mismatchedItems;
            if (left == null)
            {
                mismatchedItems.Right.AddRange(right);
                return mismatchedItems;
            }
            if (right == null)
            {
                mismatchedItems.Left.AddRange(left);
                return mismatchedItems;
            }
            var leftList = left.ToList();
            var rightList = right.ToList();
            var leftDictionary = new Dictionary<T, int>(comparer);
            int nullCount = 0;
            foreach (var item in leftList)
            {
                if (item == null)
                {
                    nullCount++; // No action needed
                    continue;
                }
                if (!leftDictionary.TryGetValue(item, out int _))
                    leftDictionary.Add(item, 1);
                else
                    ++leftDictionary[item];
            }
            foreach (var item in rightList)
            {
                if (item == null)
                {
                    nullCount--; // No action needed
                    continue;
                }
                int value;
                if (!leftDictionary.TryGetValue(item, out value) || value < 1)
                {
                    mismatchedItems.Right.Add(item);
                    continue;
                }
                --leftDictionary[item];
            }
            mismatchedItems.Left.AddRange(leftDictionary.Where(i => i.Value > 0)?.Select(kvp => kvp.Key));
            if (nullCount == 0)
            {
                return mismatchedItems;
            }
            while (nullCount > 0)
            {
                mismatchedItems.Left.Add(default(T));
                --nullCount;
            }
            while (nullCount < 0)
            {
                mismatchedItems.Right.Add(default(T));
                ++nullCount;
            }
            return mismatchedItems;
        }
    }
}