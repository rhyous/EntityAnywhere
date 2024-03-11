using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    [TestClass]
    public class ConcreteConverterTests
    {
        #region ConcreteCopy
        [TestMethod]
        public void ConcreteCopy_CreatesNewObject_Tests()
        {
            // Arrange
            var expected1 = 10;
            var expected2 = 10;
            var b = new ConcreteB1 { Prop1 = expected1, Prop2 = expected2 };

            // Act
            var b2 = b.ConcreteCopy<ConcreteB2, IB>();

            // Assert
            Assert.AreEqual(expected1, b2.Prop1);
            Assert.AreEqual(expected2, b2.Prop2);
        }

        [TestMethod]
        public void ConcreteCopy_IntoExistingObject_Tests()
        {
            // Arrange
            var expected1 = 10;
            var expected2 = 10;
            var b = new ConcreteB1 { Prop1 = expected1, Prop2 = expected2 };
            var b2 = new ConcreteB2();

            // Act
            b.ConcreteCopy<ConcreteB2, IB>(b2);

            // Assert
            Assert.AreEqual(expected1, b2.Prop1);
            Assert.AreEqual(expected2, b2.Prop2);
        }

        [TestMethod]
        public void ConcreteCopy_ExcludeProperties_Tests()
        {
            // Arrange
            var expected1 = 10;
            var expected2 = 10;
            var b = new ConcreteB1 { Prop1 = expected1, Prop2 = expected2 };
            var excludeProperties = new HashSet<string>
                {
                    "Prop2"
                };
            // Act
            var b2 = b.ConcreteCopy<ConcreteB2, IB>(null, excludeProperties);

            // Assert
            Assert.AreEqual(expected1, b2.Prop1);
            Assert.AreEqual(0, b2.Prop2);
        }

        [TestMethod]
        public void ConcreteCopy_ExcludeProperties_CaseInsensitive_Tests()
        {
            // Arrange
            var expected1 = 10;
            var expected2 = 10;
            var b = new ConcreteB1 { Prop1 = expected1, Prop2 = expected2 };
            var excludeProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "prop2"
                };

            // Act
            var b2 = b.ConcreteCopy<ConcreteB2, IB>(null, excludeProperties);

            // Assert
            Assert.AreEqual(expected1, b2.Prop1);
            Assert.AreEqual(0, b2.Prop2);
        }

        #endregion

        #region ToConcrete

        [TestMethod]
        public void ConcreteCopy_ToConcrete_IntefaceInstantionIsSameConcrete_DoesNotCreateNewObject_Tests()
        {
            // Arrange
            var expected1 = 10;
            IA a = new ConcreteA { Prop1 = expected1 };

            // Act
            var actual = a.ToConcrete<ConcreteA, IA>();

            // Assert
            Assert.AreEqual(a, actual);
        }

        [TestMethod]
        public void ConcreteCopy_ToConcrete_IntefaceInstantionIsDifferentConcrete_DoesCreateNewObject_Tests()
        {
            // Arrange
            var b = new ConcreteB1 { Prop1 = 10, Prop2 = 10 };

            // Act
            var b2 = b.ToConcrete<ConcreteB2, IB>();

            // Assert
            Assert.AreNotEqual<IB>(b, b2);
            Assert.AreEqual(b.Prop1, b2.Prop1);
            Assert.AreEqual(b.Prop2, b2.Prop2);
        }
        #endregion
    }

    public interface IA { int Prop1 { get; set; } }
    public interface IB : IA { int Prop2 { get; set; } }

    public class ConcreteA : IA { public int Prop1 { get; set; } }
    public class ConcreteB1 : IB { public int Prop1 { get; set; } public int Prop2 { get; set; } }
    public class ConcreteB2 : IB { public int Prop1 { get; set; } public int Prop2 { get; set; } }
}
