using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Interfaces.Common.Tests
{
    [TestClass]
    public class IEnumerableExtensionsTests
    {
        #region UnorderedEquals tests
        /// <summary>
        /// Scenario 1 - Both null
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsBothNullTrueTest()
        {
            // Arrange
            IEnumerable<string> items1 = null;
            IEnumerable<string> items2 = null;

            // Act
            // Assert
            Assert.IsTrue(items1.UnorderedEquals(items2));
        }

        /// <summary>
        /// Scenario 2a - Left list is null, right  is instantiated.
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsLeftNullFalseTest()
        {
            // Arrange
            IEnumerable<string> items1 = null;
            IEnumerable<string> items2 = new List<string> { "B", "C", "A" };

            // Act
            // Assert
            Assert.IsFalse(items1.UnorderedEquals(items2));
        }

        /// <summary>
        /// Scenario 2b - Right list is null, left is instantiated.
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsRightNullFalseTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C" };
            IEnumerable<string> items2 = null;

            // Act
            // Assert
            Assert.IsFalse(items1.UnorderedEquals(items2));
        }

        /// <summary>
        /// Scenario 3 - Both lists are instantiated but empty
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsBothEmptyTrueTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string>();
            IEnumerable<string> items2 = new List<string>();

            // Act
            // Assert
            Assert.IsTrue(items1.UnorderedEquals(items2));
        }

        /// <summary>
        /// Scenario 4a - Left list is empty, right list is populated.
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsLeftIsEmptyFalseTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string>();
            IEnumerable<string> items2 = new List<string> { "B", "C", "A" };

            // Act
            // Assert
            Assert.IsFalse(items1.UnorderedEquals(items2));
        }
        
        /// <summary>
        /// Scenario 4b - Right list is empty, left list is populated.
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsRightEmptyFalseTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C" };
            IEnumerable<string> items2 = new List<string>();

            // Act
            // Assert
            Assert.IsFalse(items1.UnorderedEquals(items2));
        }

        /// <summary>
        /// Scenaro 5 - Both are instantiated but have different number of items.
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsSameDistinctItemsButNotSameFalseTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C" };
            IEnumerable<string> items2 = new List<string> { "B", "C", "A", "B" };

            // Act
            // Assert
            Assert.IsFalse(items1.UnorderedEquals(items2));
        }

        /// <summary>
        /// Scenario 6a - Lists same size and items are equal
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsTrueTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C" };
            IEnumerable<string> items2 = new List<string> { "B", "C", "A" };

            // Act
            // Assert
            Assert.IsTrue(items1.UnorderedEquals(items2));
        }

        /// <summary>
        /// Scenario 6b - Lists same size but items are not equal
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsFalseTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C" };
            IEnumerable<string> items2 = new List<string> { "B", "C", "D" };

            // Act
            // Assert
            Assert.IsFalse(items1.UnorderedEquals(items2));
        }

        /// <summary>
        /// Scenario 6c - Lists same size and items are equal and some items are null
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsNullItemsTrueTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C", null, "E" };
            IEnumerable<string> items2 = new List<string> { "B", "C", "A", null, "E" };

            // Act
            // Assert
            Assert.IsTrue(items1.UnorderedEquals(items2));
        }
        
        /// <summary>
        /// Scenario 6d - Lists same size and items are equal and some items are duplicated
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsDuplicateItemsTrueTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C", "B" };
            IEnumerable<string> items2 = new List<string> { "B", "C", "A", "B" };

            // Act
            // Assert
            Assert.IsTrue(items1.UnorderedEquals(items2));
        }

        /// <summary>
        /// Scenario 6e - Lists same size and items are not equal, one side has more nulls and less items
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsDifferentNumberOfNullItemsFalseTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C", null, "E", "B" };
            IEnumerable<string> items2 = new List<string> { "B", "C", null, "A", null, "E" };

            // Act
            // Assert
            Assert.IsFalse(items1.UnorderedEquals(items2));
        }

        /// <summary>
        /// Scenario 7 - Lists same size and items are not strictly equal, but IEqualityComparer<T> makes them equal
        /// </summary>
        [TestMethod]
        public void UnorderedEqualsIgnoreCaseTrueTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "a", "b", "C", "B" };
            IEnumerable<string> items2 = new List<string> { "B", "c", "A", "b" };

            // Act
            // Assert
            Assert.IsTrue(items1.UnorderedEquals(items2, StringComparer.OrdinalIgnoreCase));
        }
        #endregion

        #region GetMisMatchedItems tests
        /// <summary>
        /// Scenario 1 - Both null
        /// </summary>
        [TestMethod]
        public void GetMisMatchedItemsBothNullTest()
        {
            // Arrange
            IEnumerable<string> items1 = null;
            IEnumerable<string> items2 = null;

            // Act
            var actual = items1.GetMismatchedItems(items2);

            // Assert
            Assert.AreEqual(0, actual.Left.Count);
            Assert.AreEqual(0, actual.Right.Count);
        }

        /// <summary>
        /// Scenario 2a - Left list is null, right  is instantiated.
        /// </summary>
        [TestMethod]
        public void GetMisMatchedItemsLeftNullTest()
        {
            // Arrange
            IEnumerable<string> items1 = null;
            IEnumerable<string> items2 = new List<string> { "B", "C", "A" };

            // Act
            var actual = items1.GetMismatchedItems(items2);

            // Assert
            Assert.AreEqual(0, actual.Left.Count);
            Assert.AreEqual(3, actual.Right.Count);
            Assert.IsTrue(items2.SequenceEqual(actual.Right));
        }

        /// <summary>
        /// Scenario 2b - Right list is null, left is instantiated.
        /// </summary>
        [TestMethod]
        public void GetMisMatchedItemsRightNullTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C" };
            IEnumerable<string> items2 = null;

            // Act
            var actual = items1.GetMismatchedItems(items2);

            // Assert
            Assert.AreEqual(3, actual.Left.Count);
            Assert.AreEqual(0, actual.Right.Count);
            Assert.IsTrue(items1.SequenceEqual(actual.Left));
        }

        /// <summary>
        /// Scenario 3 - Both lists are instantiated but empty
        /// </summary>
        [TestMethod]
        public void GetMisMatchedItemsBothEmptyTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string>();
            IEnumerable<string> items2 = new List<string>();

            // Act
            var actual = items1.GetMismatchedItems(items2);

            // Assert
            Assert.AreEqual(0, actual.Left.Count);
            Assert.AreEqual(0, actual.Right.Count);
        }

        /// <summary>
        /// Scenario 4a - Left list is empty, right list is populated.
        /// </summary>
        [TestMethod]
        public void GetMisMatchedItemsLeftIsEmptyTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string>();
            IEnumerable<string> items2 = new List<string> { "B", "C", "A" };

            // Act
            var actual = items1.GetMismatchedItems(items2);

            // Assert
            Assert.AreEqual(0, actual.Left.Count);
            Assert.AreEqual(3, actual.Right.Count);
            Assert.IsTrue(items2.SequenceEqual(actual.Right));
        }

        /// <summary>
        /// Scenario 4b - Right list is empty, left list is populated.
        /// </summary>
        [TestMethod]
        public void GetMisMatchedItemsRightIsEmptyTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C" };
            IEnumerable<string> items2 = new List<string>();

            // Act
            var actual = items1.GetMismatchedItems(items2);

            // Assert
            Assert.AreEqual(3, actual.Left.Count);
            Assert.AreEqual(0, actual.Right.Count);
            Assert.IsTrue(items1.SequenceEqual(actual.Left));
        }

        /// <summary>
        /// Scenario 5a - Lists same size and items are equal
        /// </summary>
        [TestMethod]
        public void GetMisMatchedItemsAllMatchTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C" };
            IEnumerable<string> items2 = new List<string> { "B", "C", "A" };

            // Act
            var actual = items1.GetMismatchedItems(items2);

            // Assert
            Assert.AreEqual(0, actual.Left.Count);
            Assert.AreEqual(0, actual.Right.Count);
        }

        /// <summary>
        /// Scenario 5b - Lists same size but items are not equal
        /// </summary>
        [TestMethod]
        public void GetMisMatchedItems1EachTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C" };
            IEnumerable<string> items2 = new List<string> { "B", "C", "D" };

            // Act
            var actual = items1.GetMismatchedItems(items2);

            // Assert
            Assert.AreEqual(1, actual.Left.Count);
            Assert.AreEqual("A", actual.Left[0]);
            Assert.AreEqual(1, actual.Right.Count);
            Assert.AreEqual("D", actual.Right[0]);
        }

        /// <summary>
        /// Scenario 5c - Lists same size and items are equal and some items are null
        /// </summary>
        [TestMethod]
        public void GetMisMatchedItemsNullItemsTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C", null, "E" };
            IEnumerable<string> items2 = new List<string> { "B", "C", "A", null, "E" };

            // Act
            var actual = items1.GetMismatchedItems(items2);

            // Assert
            Assert.AreEqual(0, actual.Left.Count);
            Assert.AreEqual(0, actual.Right.Count);
        }

        /// <summary>
        /// Scenario 5d - Lists same size and items are equal and some items are duplicated
        /// </summary>
        [TestMethod]
        public void GetMisMatchedItemsAllMatchDuplicateItemsTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C", "B" };
            IEnumerable<string> items2 = new List<string> { "B", "C", "A", "B" };

            // Act
            var actual = items1.GetMismatchedItems(items2);

            // Assert
            Assert.AreEqual(0, actual.Left.Count);
            Assert.AreEqual(0, actual.Right.Count);
        }

        /// <summary>
        /// Scenario 5e - Lists same size and items are not equal, one side has more nulls and less items
        /// </summary>
        [TestMethod]
        public void GetMisMatchedItemsDifferentNumberOfNullItemsTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "A", "B", "C", null, "E", "B" };
            IEnumerable<string> items2 = new List<string> { "B", "C", null, "A", null, "E" };

            // Act
            var actual = items1.GetMismatchedItems(items2);

            // Assert
            Assert.AreEqual(1, actual.Left.Count);
            Assert.AreEqual("B", actual.Left[0]);
            Assert.AreEqual(1, actual.Right.Count);
            Assert.AreEqual(null, actual.Right[0]);
        }

        /// <summary>
        /// Scenario 6 - Lists same size and items are not strictly equal, but IEqualityComparer<T> makes them equal
        /// </summary>
        [TestMethod]
        public void GetMisMatchedItemsAllMatchIgnoreCaseTest()
        {
            // Arrange
            IEnumerable<string> items1 = new List<string> { "a", "b", "C", "B" };
            IEnumerable<string> items2 = new List<string> { "B", "c", "A", "b" };

            // Act
            var actual = items1.GetMismatchedItems(items2, StringComparer.OrdinalIgnoreCase);

            // Assert
            Assert.AreEqual(0, actual.Left.Count);
            Assert.AreEqual(0, actual.Right.Count);
        }
        #endregion
    }
}