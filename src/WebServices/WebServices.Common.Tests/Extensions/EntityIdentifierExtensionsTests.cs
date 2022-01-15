using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.WebServices;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Extensions
{
    [TestClass]
    public class EntityIdentifierExtensionsTests
    {

        [TestMethod]
        public void ToExpression_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var entityIdentifiers = new List<EntityIdentifier>
            {
                new EntityIdentifier { Entity = "EntityBasic", EntityId = "27" },
                new EntityIdentifier { Entity = "EntityBasic", EntityId = "28" },
            };

            // Act
            var expression = entityIdentifiers.ToExpression<ExtensionEntityBasic, IExtensionEntity, long>();

            // Assert
            Assert.AreEqual(expression.ToString(), "e => ((e.Entity == value(Rhyous.EntityAnywhere.WebServices.EntityIdentifierExtensions+<>c__DisplayClass0_0`3[Rhyous.EntityAnywhere.WebServices.Common.Tests.ExtensionEntityBasic,Rhyous.EntityAnywhere.Interfaces.IExtensionEntity,System.Int64]).key) AndAlso value(Rhyous.EntityAnywhere.WebServices.EntityIdentifierExtensions+<>c__DisplayClass0_0`3[Rhyous.EntityAnywhere.WebServices.Common.Tests.ExtensionEntityBasic,Rhyous.EntityAnywhere.Interfaces.IExtensionEntity,System.Int64]).value.Contains(e.EntityId))");
        }
    }
}
