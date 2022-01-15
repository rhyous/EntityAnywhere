using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Rhyous.EntityAnywhere.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace Attributes.Common.Tests
{

    class A1 : Attribute { public int SomeSetting { get; set; }}
    class A2 : A1 { }
    [A1(SomeSetting = 27)]
    class B1 { }
    class B2 : B1 { }

    [A2(SomeSetting = 81)]
    class C1 { }
    class C2 : C1 { }

    [TestClass]
    public class MemberInfoAttributeExtensionsTests
    {
        #region ProveLimitation

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetCustomAttributes_Inheritance_True()
        {
            // Arrange
            // Act
            var actualB2_true = typeof(B2).GetCustomAttributes(typeof(A1), true);
            var actualB2_false = typeof(B2).GetCustomAttributes(typeof(A1), false);
            var actualC2_true = typeof(C2).GetCustomAttributes(typeof(A1), true);
            var actualC2_false = typeof(C2).GetCustomAttributes(typeof(A1), false);

            // Assert
            Assert.AreEqual(1, actualB2_true.Length);
            Assert.AreEqual(typeof(A1[]), actualB2_true.GetType());

            Assert.AreEqual(0, actualB2_false.Length);
            Assert.AreEqual(typeof(A1[]), actualB2_false.GetType());
            
            Assert.AreEqual(1, actualC2_true.Length);
            Assert.AreEqual(typeof(A1[]), actualC2_true.GetType());

            Assert.AreEqual(0, actualC2_false.Length);
            Assert.AreEqual(typeof(A1[]), actualC2_false.GetType());

            // Issues
            // 1. No way to make C2 fail, while making B2 succeed. Inheritance for object type and attribute type are bundled in one parameter. They should be separate.
            // 2. Return value is object[] not T[]
        }

        #endregion

        #region GetAttribute
        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttribute_DefaultValues()
        {
            // Arrange
            // Act
            var actual = typeof(B1).GetAttribute<A1>();

            // Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttribute_TypeInheritance()
        {
            // Arrange
            // Act
            var actual = typeof(B2).GetAttribute<A1>();

            // Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttribute_TypeInheritance_False()
        {
            // Arrange
            // Act
            var actual = typeof(B2).GetAttribute<A1>(false);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttribute_DefaultValues_AttributeInheritance()
        {
            // Arrange
            // Act
            var actual = typeof(C1).GetAttribute<A1>();

            // Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttribute_AttributeInheritance_TypeInheritance()
        {
            // Arrange
            // Act
            var actual = typeof(C2).GetAttribute<A1>();

            // Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttribute_AttributeInheritance_False_TypeInheritance_False()
        {
            // Arrange
            // Act
            var actual = typeof(C2).GetAttribute<A1>(false, false);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttribute_AttributeInheritance_False_TypeInheritance_true()
        {
            // Arrange
            // Act
            var actual = typeof(C2).GetAttribute<A1>(true, false);

            // Assert
            Assert.IsNull(actual);
        }
        #endregion

        #region GetAttributes
        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributes_DefaultValues()
        {
            // Arrange
            // Act
            var actual = typeof(B1).GetAttributes<A1>().ToArray();

            // Assert
            Assert.AreEqual(1, actual.Length);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributes_TypeInheritance()
        {
            // Arrange
            // Act
            var actual = typeof(B2).GetAttributes<A1>().ToArray();

            // Assert
            Assert.AreEqual(1, actual.Length);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributes_TypeInheritance_False()
        {
            // Arrange
            // Act
            var actual = typeof(B2).GetAttributes<A1>(false).ToArray();

            // Assert
            Assert.AreEqual(0, actual.Length);
        }


        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributes_DefaultValues_AttributeInheritance()
        {
            // Arrange
            // Act
            var actual = typeof(C1).GetAttributes<A1>().ToArray();

            // Assert
            Assert.AreEqual(1, actual.Length);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributes_AttributeInheritance_TypeInheritance()
        {
            // Arrange
            // Act
            var actual = typeof(C2).GetAttributes<A1>().ToArray();

            // Assert
            Assert.AreEqual(1, actual.Length);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributes_AttributeInheritance_False_TypeInheritance_False()
        {
            // Arrange
            // Act
            var actual = typeof(C2).GetAttributes<A1>(false, false).ToArray();

            // Assert
            Assert.AreEqual(0, actual.Length);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributes_AttributeInheritance_False_TypeInheritance_true()
        {
            // Arrange
            // Act
            var actual = typeof(C2).GetAttributes<A1>(true, false).ToArray();

            // Assert
            Assert.AreEqual(0, actual.Length);
        }
        #endregion

        #region GetAttributePropertyValue
        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributePropertyValue_DefaultValues()
        {
            // Arrange
            // Act
            var actual = typeof(B1).GetAttributePropertyValue<A1, int>("SomeSetting", 0);

            // Assert
            Assert.AreEqual(27, actual);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributePropertyValue_ChildAttribute_DefaultValues()
        {
            // Arrange
            // Act
            var actual = typeof(C1).GetAttributePropertyValue<A1, int>("SomeSetting", 0);

            // Assert
            Assert.AreEqual(81, actual);
        }


        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributePropertyValue_ChildAttribute_AttributeInheritanceFalse_DefaultValues()
        {
            // Arrange
            // Act
            var actual = typeof(C1).GetAttributePropertyValue<A1, int>("SomeSetting", 10, true, false);

            // Assert
            Assert.AreEqual(10, actual);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributePropertyValue_NullType_DefaultValues()
        {
            // Arrange
            Type t = null;
            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => { t.GetAttributePropertyValue<A1, int>("SomeValue", 0); });
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributePropertyValue_Nullprop_DefaultValues()
        {
            // Arrange
            Type t = typeof(C1);
            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => { t.GetAttributePropertyValue<A1, int>(null, 0); });
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributePropertyValue_Emptyprop_DefaultValues()
        {
            // Arrange
            Type t = typeof(C1);
            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => { t.GetAttributePropertyValue<A1, int>("", 0); });
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributePropertyValue_Whitespaceprop_DefaultValues()
        {
            // Arrange
            Type t = typeof(C1);
            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => { t.GetAttributePropertyValue<A1, int>("", 0); });
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributePropertyValue_Invalidprop_DefaultValues()
        {
            // Arrange
            Type t = typeof(C1);
            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => { t.GetAttributePropertyValue<A1, int>("NotAProp", 0); });
        }
        #endregion
        
        #region GetAttributePropertyValue MultipleAttributes

        internal class E1
        {
            [DataMember(Order = 1)]
            [JsonProperty(Order = 3)]
            [Display(Order = 5)]
            public int Id { get; set; }

            [DataMember]
            [JsonProperty]
            [Display]
            public string Name { get; set; }
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributePropertyValue_DataMemberFirst_Test()
        {
            // Arrange
            var types = new[] { typeof(DataMemberAttribute), typeof(JsonPropertyAttribute), typeof(DisplayAttribute) };
            var propInfo = typeof(E1).GetProperty("Id");

            // Act
            var actual = propInfo.GetAttributePropertyValue(types, "Order", 0);

            // Assert
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributePropertyValue_JsonPropertyFirst_Test()
        {
            // Arrange
            var types = new[] { typeof(JsonPropertyAttribute), typeof(DataMemberAttribute) };
            var propInfo = typeof(E1).GetProperty("Id");

            // Act
            var actual = propInfo.GetAttributePropertyValue(types, "Order", 0);

            // Assert
            Assert.AreEqual(3, actual);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributePropertyValue_DisplayFirst_Test()
        {
            // Arrange
            var types = new[] { typeof(DisplayAttribute), typeof(JsonPropertyAttribute), typeof(DataMemberAttribute) };
            var propInfo = typeof(E1).GetProperty("Id");

            // Act
            var actual = propInfo.GetAttributePropertyValue(types, "Order", 0);

            // Assert
            Assert.AreEqual(5, actual);
        }

        [TestMethod]
        public void MemberInfoAttributeExtensions_GetAttributePropertyValue_DisplayFirst_OrderNotSet_Test()
        {
            // Arrange
            var types = new[] { typeof(DisplayAttribute) };
            var propInfo = typeof(E1).GetProperty("Name");

            // Act
            var actual = propInfo.GetAttributePropertyValue(types, "Order", 10000);

            // Assert
            Assert.AreEqual(10000, actual);
        }

        #endregion
    }
}
