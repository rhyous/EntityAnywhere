using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces.Common.Tests.Dictionaries
{
    [TestClass]
    public class NullSafeDictionaryTests
    {
        [TestMethod]
        public void NullSafeDictionaryDefaultValueTest()
        {
            // Arrange
            var dict = new NullSafeDictionary<string, int>();

            // Act
            var value = dict["a"];

            // Assert
            Assert.AreEqual(0, value);
        }

        [TestMethod]
        public void NullSafeDictionaryDefaultValueFromCustomMethodTest()
        {
            // Arrange
            var dict = new NullSafeDictionary<string, int>((key) => { return -1; });

            // Act
            var value = dict["a"];

            // Assert
            Assert.AreEqual(-1, value);
        }

        [TestMethod]
        public void NullSafeDictionaryDefaultValueAddKeysTest()
        {
            // Arrange
            var dict = new NullSafeDictionary<string, int>((key) => { return -1; });
            var array = new[] { "A", "B", "C" };
            // Act
            dict.AddKeys(array);

            // Assert
            for (int i = 0; i < dict.Keys.Count; i++)
            {
                Assert.AreEqual(-1, dict.Values.Skip(i).First());
            }
        }
    }
}