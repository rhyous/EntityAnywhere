using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Rhyous.WebFramework.WebServices.Common.Tests
{
    [TestClass]
    public class ListMakerTests
    {
        [TestMethod]
        public void MakeTest()
        {
            // Arrange    
            var expected = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var array = new [] { 5, 6, 7, 8, 9 };

            // Act
            var list = ListMaker.Make(0, 1, 2, 3, 4, array);

            // Assert
            CollectionAssert.AreEqual(expected, list);
        }

        [TestMethod]
        public void Add2Test()
        {
            // Arrange    
            var expected = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };

            // Act
            list.Add(8, 9);

            // Assert
            CollectionAssert.AreEqual(expected, list);
        }

        [TestMethod]
        public void Add3Test()
        {
            // Arrange    
            var expected = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6 };

            // Act
            list.Add(7, 8, 9);

            // Assert
            CollectionAssert.AreEqual(expected, list);
        }

        [TestMethod]
        public void Add4Test()
        {
            // Arrange    
            var expected = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var list = new List<int> { 0, 1, 2, 3, 4, 5};

            // Act
            list.Add(6, 7, 8, 9);

            // Assert
            CollectionAssert.AreEqual(expected, list);
        }

        [TestMethod]
        public void Add5Test()
        {
            // Arrange    
            var expected = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var list = new List<int> { 0, 1, 2, 3, 4 };

            // Act
            list.Add(5, 6, 7, 8, 9);

            // Assert
            CollectionAssert.AreEqual(expected, list);
        }


        [TestMethod]
        public void Add6Test()
        {
            // Arrange    
            var expected = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var list = new List<int> { 0, 1, 2, 3 };

            // Act
            list.Add(4, 5, 6, 7, 8, 9);

            // Assert
            CollectionAssert.AreEqual(expected, list);
        }

        #region Add + params
        [TestMethod]
        public void Add1AndParamsTest()
        {
            // Arrange    
            var expected = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var array = new[] { 10, 11, 12 };

            // Act
            list.Add(9, array);

            // Assert
            CollectionAssert.AreEqual(expected, list);
        }

        [TestMethod]
        public void Add2AndParamsTest()
        {
            // Arrange    
            var expected = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
            var array = new[] { 10, 11, 12 };

            // Act
            list.Add(8, 9, array);

            // Assert
            CollectionAssert.AreEqual(expected, list);
        }

        [TestMethod]
        public void Add3AndParamsTest()
        {
            // Arrange    
            // Arrange    
            var expected = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
            var array = new[] { 10, 11, 12 };

            // Act
            list.Add(7, 8, 9, array);

            // Assert
            CollectionAssert.AreEqual(expected, list);
        }

        [TestMethod]
        public void Add4AndParamsTest()
        {
            // Arrange    
            // Arrange    
            var expected = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var list = new List<int> { 0, 1, 2, 3, 4, 5 };
            var array = new[] { 10, 11, 12 };

            // Act
            list.Add(6, 7, 8, 9, array);

            // Assert
            CollectionAssert.AreEqual(expected, list);
        }

        [TestMethod]
        public void Add5AndParamsTest()
        {
            // Arrange    
            // Arrange    
            var expected = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var list = new List<int> { 0, 1, 2, 3, 4 };
            var array = new[] { 10, 11, 12 };

            // Act
            list.Add(5, 6, 7, 8, 9, array);

            // Assert
            CollectionAssert.AreEqual(expected, list);
        }
        #endregion
    }
}
