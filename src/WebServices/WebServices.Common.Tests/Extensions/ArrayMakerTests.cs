using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.WebFramework.WebServices.Common.Tests
{
    [TestClass]
    public class ArrayMakerTests
    {
        #region AddAt tests
        [TestMethod]
        public void AddAtTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var array = new int[10];
            int position = 0;

            // Act
            array.AddAt(position, 0, 1, 2, 3, 4);
            array.AddAt(position + 5, 5, 6, 7, 8, 9);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void AddAtNullTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int position = 10;
            int[] addArray = null;
            
            // Act
            array.AddAt(position, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }
        #endregion

        #region Add tests
        [TestMethod]
        public void Add1Test()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            // Act
            ArrayMaker.Add(ref array, 9);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add2Test()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7 };

            // Act
            ArrayMaker.Add(ref array, 8, 9);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add3Test()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var array = new[] { 0, 1, 2, 3, 4, 5, 6 };

            // Act
            ArrayMaker.Add(ref array, 7, 8, 9);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add4Test()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var array = new[] { 0, 1, 2, 3, 4, 5 };

            // Act
            ArrayMaker.Add(ref array, 6, 7, 8, 9);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add5Test()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var array = new[] { 0, 1, 2, 3, 4 };

            // Act
            ArrayMaker.Add(ref array, 5, 6, 7, 8, 9);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add6Test()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var array = new[] { 0, 1, 2, 3 };

            // Act
            ArrayMaker.Add(ref array, 4, 5, 6, 7, 8, 9);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }
        #endregion

        #region Add and params
        [TestMethod]
        public void Add1AndParamsTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var addArray = new[] { 10, 11, 12 };
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            // Act
            ArrayMaker.Add(ref array, 9, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add2AndParamsTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var addArray = new[] { 10, 11, 12 };
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7 };

            // Act
            ArrayMaker.Add(ref array, 8, 9, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add3AndParamsTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var addArray = new[] { 10, 11, 12 };
            var array = new[] { 0, 1, 2, 3, 4, 5, 6 };

            // Act
            ArrayMaker.Add(ref array, 7, 8, 9, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add4AndParamsTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var addArray = new[] { 10, 11, 12 };
            var array = new[] { 0, 1, 2, 3, 4, 5 };

            // Act
            ArrayMaker.Add(ref array, 6, 7, 8, 9, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add5AndParamsTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var addArray = new[] { 10, 11, 12 };
            var array = new[] { 0, 1, 2, 3, 4 };

            // Act
            ArrayMaker.Add(ref array, 5, 6, 7, 8, 9, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }
        #endregion

        #region Add and null params
        [TestMethod]
        public void Add0AndParamsNullTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            int[] addArray = null;

            // Act
            ArrayMaker.Add(ref array, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        public void Add1AndParamsNullTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            int[] addArray = null;

            // Act
            ArrayMaker.Add(ref array, 12, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add2AndParamsNullTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] addArray = null;

            // Act
            ArrayMaker.Add(ref array, 11, 12, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add3AndParamssNullTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] addArray = null;

            // Act
            ArrayMaker.Add(ref array, 10, 11, 12, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add4AndParamssNullTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            int[] addArray = null;

            // Act
            ArrayMaker.Add(ref array, 9, 10, 11, 12, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Add5AndParamssNullTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            int[] addArray = null;

            // Act
            ArrayMaker.Add(ref array, 8, 9, 10, 11, 12, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }
        #endregion


        #region Make and params
        [TestMethod]
        public void Make1AndParamsTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var addArray = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            // Act
            var array = ArrayMaker.Make(0, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Make2AndParamsTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var addArray = new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            // Act
            var array = ArrayMaker.Make( 0, 1, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Make3AndParamsTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var addArray = new[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            // Act
            var array = ArrayMaker.Make(0, 1, 2, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Make4AndParamsTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var addArray = new[] { 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            // Act
            var array = ArrayMaker.Make(0, 1, 2, 3, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Make5AndParamsTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var addArray = new[] { 5, 6, 7, 8, 9, 10, 11, 12 };

            // Act
            var array = ArrayMaker.Make(0, 1, 2, 3, 4, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }
        #endregion

        #region Make and null params
        [TestMethod]
        public void Make0AndParamsNullTest()
        {
            // Arrange    
            int[] addArray = null;

            // Act
            var array = ArrayMaker.Make(addArray);

            // Assert
            Assert.IsNull(array);
        }

        public void Make1AndParamsNullTest()
        {
            // Arrange    
            var expected = new int[] { 0 };
            int[] addArray = null;

            // Act
            var array = ArrayMaker.Make(0, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Make2AndParamsNullTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1 };
            int[] addArray = null;

            // Act
            var array = ArrayMaker.Make(0, 1, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Make3AndParamssNullTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2 };
            int[] addArray = null;

            // Act
            var array = ArrayMaker.Make(0, 1, 2, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Make4AndParamssNullTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3 };
            int[] addArray = null;

            // Act
            var array = ArrayMaker.Make(0, 1, 2, 3, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }

        [TestMethod]
        public void Make5AndParamssNullTest()
        {
            // Arrange    
            var expected = new int[] { 0, 1, 2, 3, 4 };
            int[] addArray = null;

            // Act
            var array = ArrayMaker.Make(0, 1, 2, 3, 4, addArray);

            // Assert
            CollectionAssert.AreEqual(expected, array);
        }
        #endregion
    }
}
