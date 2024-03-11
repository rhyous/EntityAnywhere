using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Model
{
    [TestClass]
    public class ExtensionEntityListTests
    {
        private ExtensionEntityList CreateExtensionEntityList()
        {
            return new ExtensionEntityList();
        }

        #region ShouldAutoExpand
        [TestMethod]
        [PrimitiveList(nameof(Addendum), nameof(AlternateId))]
        public void ExtensionEntityList_ShouldAutoExpand_True(string entityName)
        {
            // Arrange
            var extensionEntityList = CreateExtensionEntityList();
            extensionEntityList.Entities.AddRange(new[] { typeof(Addendum), typeof(AlternateId), typeof(Note) });

            // Act
            var result = extensionEntityList.ShouldAutoExpand(entityName);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [PrimitiveList(nameof(Note))]
        public void ExtensionEntityList_ShouldAutoExpand_False(string entityName)
        {
            // Arrange
            var extensionEntityList = CreateExtensionEntityList();
            extensionEntityList.Entities.AddRange(new[] { typeof(Addendum), typeof(AlternateId), typeof(Note) });

            // Act
            var result = extensionEntityList.ShouldAutoExpand(entityName);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion
    }
}