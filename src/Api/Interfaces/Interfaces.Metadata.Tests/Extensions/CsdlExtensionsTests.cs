using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces.Common.Tests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces.Tests
{
    [TestClass]
    public class CsdlExtensionsTests
    {
        [TestMethod]
        public void CsdlExtensions_AddFileUpload_NoInterface_Test()
        {
            // Arrange
            var type = typeof(EntityInt);
            var csdl = new CsdlEntity();

            // Act
            csdl.AddFileUpload(type);

            // Assert
            Assert.AreEqual(0, csdl.Properties.Count);
        }

        [TestMethod]
        public void CsdlExtensions_AddFileUpload_HasInterface_Test()
        {
            // Arrange
            var type = typeof(EntityWithFileUpload);
            var csdl = new CsdlEntity();

            // Act
            csdl.AddFileUpload(type);

            // Assert
            Assert.AreEqual(1, csdl.Properties.Count);
            Assert.AreEqual("@EAF.FileUpload", csdl.Properties.Keys.First());
            Assert.IsTrue((bool)csdl.Properties["@EAF.FileUpload"]);
        }

        #region AddExtensionEntityNavigationProperties
        [TestMethod]
        public void CsdlExtensions_AddExtensionEntityNavigationProperties_Test()
        {
            // Arrange
            var collection = new ReadOnlyCollection<Type>(new List<Type> { typeof(Addendum), typeof(AlternateId) });
            var type = typeof(EntityInt);
            var csdl = type.ToCsdl();

            // Act
            csdl.AddExtensionEntityNavigationProperties(type, collection);

            // Assert
            Assert.IsTrue(csdl.Properties.TryGetValue("Addenda", out object navPropAddenda));
            Assert.IsTrue(navPropAddenda is CsdlNavigationProperty);
            var navProp = navPropAddenda as CsdlNavigationProperty;
            Assert.AreEqual("self.Addendum", navProp.Type);
            Assert.IsTrue(navProp.IsCollection);
            Assert.IsTrue(navProp.Nullable);
            Assert.IsTrue(navProp.CustomData.TryGetValue("@EAF.RelatedEntity.Type", out object value));
            Assert.AreEqual("Extension", value);
        }

        [TestMethod]
        public void CsdlExtensions_AddExtensionEntityNavigationProperties_ExcludedExtensionEntity_Test()
        {
            // Arrange
            var collection = new ReadOnlyCollection<Type>(new List<Type> { typeof(Addendum), typeof(AlternateId) });
            var type = typeof(EntityInt);
            var csdl = type.ToCsdl();

            // Act
            csdl.AddExtensionEntityNavigationProperties(type, collection);

            // Assert
            Assert.IsFalse(csdl.Properties.TryGetValue("AlternateId", out object navPropAltIds));
        }

        [TestMethod]
        public void CsdlExtensions_AddExtensionEntityNavigationProperties_ExcludeAllExtensionEntities_Test()
        {
            // Arrange
            var collection = new ReadOnlyCollection<Type>(new List<Type> { typeof(Addendum), typeof(AlternateId) });
            var type = typeof(EntityIntNullable);
            var csdl = type.ToCsdl();

            // Act
            csdl.AddExtensionEntityNavigationProperties(type, collection);

            // Assert
            Assert.IsFalse(csdl.Properties.TryGetValue("AlternateId", out object navPropAltIds));
            Assert.IsFalse(csdl.Properties.TryGetValue("Addenda", out object navPropAddenda));
        }
        #endregion
    }
}
