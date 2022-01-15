using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    [TestClass]
    public class CustomMetadataProviderMultiUnitTests
    {
        #region Constructor

        [TestMethod]
        public void CustomMetadataProvider_Constructor_Null_Tests()
        {
            Assert.ThrowsException<ArgumentNullException>(() => {new CustomMetadataProvider(null, null, null, null); });
        }

        [TestMethod]
        public void CustomMetadataProvider_Constructor_Tests()
        {
            // Arrange
            var kvp = new KeyValuePair<string, object>("A", "B");
            bool propertyFuncWasCalled = false;
            Func<string, IEnumerable<KeyValuePair<string, object>>> propertyFunc = (string entity) =>
            {
                propertyFuncWasCalled = true;
                return new List<KeyValuePair<string, object>> { kvp };
            };
            var propertyFuncList = new FuncList<string>() { propertyFunc };

            bool propertyDataFuncWasCalled = false;
            Func<string, string, IEnumerable<KeyValuePair<string, object>>> propertyDataFunc = (string entity, string property) => 
                {
                    propertyDataFuncWasCalled = true;
                    return new List<KeyValuePair<string, object>> { kvp };
                };
            var propertyDataFuncList = new FuncList<string, string>() { propertyDataFunc };
            var extensionEntityList = new ExtensionEntityList();
            extensionEntityList.Entities.Add(typeof(Addendum));

            // Act
            var provider = new CustomMetadataProvider(new CsdlBuilderFactory(), extensionEntityList, propertyFuncList, propertyDataFuncList);
            provider.Provide(typeof(EntityString));

            // Assert
            Assert.IsNotNull(provider);
            Assert.IsTrue(propertyFuncWasCalled);
            Assert.IsTrue(propertyDataFuncWasCalled);
        }
        #endregion

        #region Provide
        [TestMethod]
        public void CustomMetadataProvider_Provide_AlternateKey_Tests()
        {
            // Arrange
            var extensionEntityList = new ExtensionEntityList();
            var metadataCache = new MetadataCache();
            var mockEntityClient = new Mock<IAdminEntityClientAsync<Entity, int>>();
            mockEntityClient.Setup(m => m.GetByQueryParametersAsync(It.IsAny<string>(), It.IsAny<bool>()))
                                         .ReturnsAsync(new OdataObjectCollection<Entity, int>());

            var entitySettingsProvider = new EntitySettingsProvider(mockEntityClient.Object);
            var factory = new MetadataServiceFactory(extensionEntityList, metadataCache, entitySettingsProvider);

            var provider = factory.CreateCustomMetadataProvider();

            var type = typeof(EntityInt);
            var expected = new List<string> { "Id", "Name" };

            // Act
            var actual = provider.Provide(type);

            // Assert
            Assert.IsNotNull(actual);
            CollectionAssert.AreEqual(expected, actual.Keys);
        }
        #endregion

        #region Customize
        [TestMethod]
        public void CustomMetadataProvider_Customize_AlternateKey_Tests()
        {
            // Arrange
            var extensionEntityList = new ExtensionEntityList();
            var metadataCache = new MetadataCache();
            var mockEntityClient = new Mock<IAdminEntityClientAsync<Entity, int>>();
            mockEntityClient.Setup(m => m.GetByQueryParametersAsync(It.IsAny<string>(), It.IsAny<bool>()))
                                         .ReturnsAsync(new OdataObjectCollection<Entity, int>());
            var entitySettingsProvider = new EntitySettingsProvider(mockEntityClient.Object);
            var factory = new MetadataServiceFactory(extensionEntityList, metadataCache, entitySettingsProvider);

            var provider = factory.CreateCustomMetadataProvider() 
                                   as CustomMetadataProvider;
            var type = typeof(EntityInt);
            var csdl = type.ToCsdl();
            var expected = new List<string> { "Id", "Name" };

            // Act
            provider.Customize(csdl, type);

            // Assert
            CollectionAssert.AreEqual(expected, csdl.Keys);
        }

        [TestMethod]
        public void CustomMetadataProvider_Customize_ExtensionEntity_Tests()
        {
            // Arrange
            var extensionEntityList = new ExtensionEntityList();
            extensionEntityList.Entities.Add(typeof(Addendum));
            var metadataCache = new MetadataCache();
            var mockEntityClient = new Mock<IAdminEntityClientAsync<Entity, int>>();
            mockEntityClient.Setup(m => m.GetByQueryParametersAsync(It.IsAny<string>(), It.IsAny<bool>()))
                                         .ReturnsAsync(new OdataObjectCollection<Entity, int>());
            var mockEntityGroupClient = new Mock<IAdminEntityClientAsync<EntityGroup, int>>();
            mockEntityGroupClient.Setup(m => m.GetByQueryParametersAsync(It.IsAny<string>(), It.IsAny<bool>()))
                                         .ReturnsAsync(new OdataObjectCollection<EntityGroup, int>());
            var entitySettingsProvider = new EntitySettingsProvider(mockEntityClient.Object);
            var factory = new MetadataServiceFactory(extensionEntityList, metadataCache, entitySettingsProvider);

            var provider = factory.CreateCustomMetadataProvider()
                                   as CustomMetadataProvider;
            var type = typeof(EntityInt);
            var csdl = type.ToCsdl();

            // Act
            provider.Customize(csdl, type);

            // Assert
            Assert.IsTrue(csdl.Properties.TryGetValue("Addenda", out object navPropObj));
            Assert.IsTrue(navPropObj is CsdlNavigationProperty);
        }
        #endregion
    }
}
