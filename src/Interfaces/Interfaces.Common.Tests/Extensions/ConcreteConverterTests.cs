using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.WebFramework.Interfaces;

namespace Interfaces.Common.Tests
{
    [TestClass]
    public class ConcreteConverterTests
    {
        [TestMethod]
        public void ConcreteCopyTests()
        {
            // Arrange
            var expected1 = 10;
            var expected2 = 10;
            var b = new ConcreteB1 { Prop1 = expected1, Prop2 = expected2 };

            // Act
            var b2 = b.ConcreteCopy<ConcreteB2, B>();

            // Assert
            Assert.AreEqual(expected1, b2.Prop1);
            Assert.AreEqual(expected2, b2.Prop2);
        }f
    }

    public interface A { int Prop1 { get; set; } }
    public interface B : A { int Prop2 { get; set; } }

    public class ConcreteA : A { public int Prop1 { get; set; } }
    public class ConcreteB1 : B { public int Prop1 { get; set; } public int Prop2 { get; set; } }
    public class ConcreteB2 : B { public int Prop1 { get; set; } public int Prop2 { get; set; } }
}
