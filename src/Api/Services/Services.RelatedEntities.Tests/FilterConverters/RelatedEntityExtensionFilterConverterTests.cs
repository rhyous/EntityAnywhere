using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.Odata.Filter;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities.Tests.FilterConverters
{
    [TestClass]
    public class RelatedEntityExtensionFilterConverterTests
    {
        private MockRepository _MockRepository;

        private Mock<IRelatedEntityFilterDataProvider> _MockRelatedEntityFilterDataProvider;
        private CsdlSchema _CsdlSchema;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRelatedEntityFilterDataProvider = _MockRepository.Create<IRelatedEntityFilterDataProvider>();

            _CsdlSchema = new CsdlSchema();
        }

        private RelatedEntityExtensionFilterConverter<T> CreateConverter<T>()
        {
            return new RelatedEntityExtensionFilterConverter<T>(
                _CsdlSchema,
                _MockRelatedEntityFilterDataProvider.Object);
        }

        #region CanConvert
        [TestMethod]
        public void RelatedEntityExtensionFilterConverter_CanConvert_FilterNull_False()
        {
            // Arrange
            var relatedEntityExtensionFilterConverter = CreateConverter<A>();
            Filter<A> filter = null;

            // Act
            var result = relatedEntityExtensionFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityExtensionFilterConverter_CanConvert_Filter_NewEmptyFilter_False()
        {
            // Arrange
            var relatedEntityExtensionFilterConverter = CreateConverter<A>();
            Filter<A> filter = new Filter<A>();

            // Act
            var result = relatedEntityExtensionFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityExtensionFilterConverter_CanConvert_Filter_IsSimpleString_False()
        {
            // Arrange
            var relatedEntityExtensionFilterConverter = CreateConverter<A>();
            Filter<A> filter = "Id";

            // Act
            var result = relatedEntityExtensionFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityExtensionFilterConverter_CanConvert_Filter_HasNoPeriod_False()
        {
            // Arrange
            var relatedEntityExtensionFilterConverter = CreateConverter<A>();
            Filter<A> filter = "Id eq 10";

            // Act
            var result = relatedEntityExtensionFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityExtensionFilterConverter_CanConvert_PeriodLastLeftCharacter_False()
        {
            // Arrange
            var relatedEntityExtensionFilterConverter = CreateConverter<A>();
            Filter<A> filter = "B. eq 11";

            // Act
            var result = relatedEntityExtensionFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityExtensionFilterConverter_CanConvert_True()
        {
            // Arrange
            var relatedEntityExtensionFilterConverter = CreateConverter<A>();
            var bName = "Value 1";
            Filter<A> filter = new Filter<A> { Left = $"{nameof(ExtensionEntity1)}.Prop1", Method = "EQ", Right = bName };
            var aCsdl = typeof(A).ToCsdl();
            var extensionEntitiesList = new List<Type> { typeof(ExtensionEntity1) };
            aCsdl.AddExtensionEntityNavigationProperties(typeof(A), extensionEntitiesList);
            _CsdlSchema.Entities.Add(typeof(A).Name, aCsdl);
            _CsdlSchema.Entities.Add(typeof(ExtensionEntity1).Name, typeof(ExtensionEntity1).ToCsdl());


            // Act
            var result = relatedEntityExtensionFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityFilterConverter_CanConvert_NotExtensionEntity_False()
        {
            // Arrange
            var converter = CreateConverter<A>();
            var bName = "My B 27";
            Filter<A> filter = new Filter<A> { Left = "B.Name", Method = "EQ", Right = bName };
            _CsdlSchema.Entities.Add(typeof(A).Name, typeof(A).ToCsdl());
            _CsdlSchema.Entities.Add(typeof(B).Name, typeof(B).ToCsdl());

            // Act
            var result = converter.CanConvert(filter);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityExtensionFilterConverter_CanConvert_Array_True()
        {
            // Arrange
            var relatedEntityExtensionFilterConverter = CreateConverter<A>();
            var bName = "My B 27";
            Filter<A> filter = new Filter<A> { Left = $"{nameof(ExtensionEntity1)}.Prop1", Method = "EQ", Right = new ArrayFilter<A, string> { Array = new[] { bName } } };
            var aCsdl = typeof(A).ToCsdl();
            var extensionEntitiesList = new List<Type> { typeof(ExtensionEntity1) };
            aCsdl.AddExtensionEntityNavigationProperties(typeof(A), extensionEntitiesList);
            _CsdlSchema.Entities.Add(typeof(A).Name, aCsdl);
            _CsdlSchema.Entities.Add(typeof(ExtensionEntity1).Name, typeof(ExtensionEntity1).ToCsdl());

            // Act
            var result = relatedEntityExtensionFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region ConvertAsync
        [TestMethod]
        public async Task RelatedEntityExtensionFilterConverter_Convert_RelatedEntity_Works()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateConverter<A>();
            string extValue = "Ext Val 27";
            var extensionProp = "Prop1";
            Filter<A> filter = new Filter<A>
            {
                Left = $"{nameof(ExtensionEntity1)}.{extensionProp}",
                Method = "eq",
                Right = new Filter<A> { NonFilter = extValue }
            };

            var aCsdl = typeof(A).ToCsdl();
            var extensionEntitiesList = new List<Type> { typeof(ExtensionEntity1) };
            aCsdl.AddExtensionEntityNavigationProperties(typeof(A), extensionEntitiesList);
            _CsdlSchema.Entities.Add(typeof(A).Name, aCsdl);
            _CsdlSchema.Entities.Add(typeof(ExtensionEntity1).Name, typeof(ExtensionEntity1).ToCsdl());

            extValue = extValue.IsQuoted() ? extValue : extValue.Quote('\'');
            var expectedFilter = $"$Filter=Entity eq '{nameof(A)}'"
                               + $" and Property eq '{extensionProp}'"
                               + $" and Value eq {extValue}";
            var ext27 = new ExtensionEntity1
            {
                Id = 27,
                Entity = nameof(ExtensionEntity1),
                EntityId = "1099",
                Property = extensionProp,
                Value = extValue
            };
            var odataObjectExt27 = new OdataObject();
            var ext27Json = JsonConvert.SerializeObject(ext27);
            odataObjectExt27.Object = new JRaw(ext27Json);
            var odataExtCollection = new OdataObjectCollection { odataObjectExt27 };
            _MockRelatedEntityFilterDataProvider.Setup(m => m.ProvideAsync(nameof(ExtensionEntity1), expectedFilter))
                                      .ReturnsAsync(odataExtCollection);

            // Act
            var result = await relatedEntityFilterConverter.ConvertAsync(filter);

            // Assert
            Assert.AreEqual($"Id in ({ext27.EntityId})", result.ToString());
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task RelatedEntityExtensionFilterConverter_Convert_RelatedEntity_QuotesShouldBePartOfFilter_Works()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateConverter<A>();
            string extValue = "'Ext Val 27'";
            var extensionProp = "Prop1";
            Filter<A> filter = new Filter<A>
            {
                Left = $"{nameof(ExtensionEntity1)}.{extensionProp}",
                Method = "eq",
                Right = new Filter<A> { NonFilter = extValue }
            };

            var aCsdl = typeof(A).ToCsdl();
            var extensionEntitiesList = new List<Type> { typeof(ExtensionEntity1) };
            aCsdl.AddExtensionEntityNavigationProperties(typeof(A), extensionEntitiesList);
            _CsdlSchema.Entities.Add(typeof(A).Name, aCsdl);
            _CsdlSchema.Entities.Add(typeof(ExtensionEntity1).Name, typeof(ExtensionEntity1).ToCsdl());

            extValue = extValue.IsQuoted() ? extValue : extValue.Quote('\'');
            var expectedFilter = $"$Filter=Entity eq '{nameof(A)}'"
                               + $" and Property eq '{extensionProp}'"
                               + $" and Value eq \"{extValue}\"";
            var ext27 = new ExtensionEntity1
            {
                Id = 27,
                Entity = nameof(ExtensionEntity1),
                EntityId = "1099",
                Property = extensionProp,
                Value = extValue
            };
            var odataObjectExt27 = new OdataObject();
            var ext27Json = JsonConvert.SerializeObject(ext27);
            odataObjectExt27.Object = new JRaw(ext27Json);
            var odataExtCollection = new OdataObjectCollection { odataObjectExt27 };
            _MockRelatedEntityFilterDataProvider.Setup(m => m.ProvideAsync(nameof(ExtensionEntity1), expectedFilter))
                                      .ReturnsAsync(odataExtCollection);

            // Act
            var result = await relatedEntityFilterConverter.ConvertAsync(filter);

            // Assert
            Assert.AreEqual($"Id in ({ext27.EntityId})", result.ToString());
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task RelatedEntityExtensionFilterConverter_Convert_RelatedEntity_Works_Array()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateConverter<A>();
            var extensionProp = "Prop1";
            var extValue = "Ext Val 27";
            Filter<A> filter = new Filter<A>
            {
                Left = $"{nameof(ExtensionEntity1)}.{extensionProp}",
                Method = "in",
                Right = new ArrayFilter<A, string> { Array = new[] { extValue } }
            };

            var aCsdl = typeof(A).ToCsdl();
            var extensionEntitiesList = new List<Type> { typeof(ExtensionEntity1) };
            aCsdl.AddExtensionEntityNavigationProperties(typeof(A), extensionEntitiesList);
            _CsdlSchema.Entities.Add(typeof(A).Name, aCsdl);
            _CsdlSchema.Entities.Add(typeof(ExtensionEntity1).Name, typeof(ExtensionEntity1).ToCsdl());

            extValue = extValue.IsQuoted() ? extValue : extValue.Quote('\'');
            var expectedFilter = $"$Filter=Entity eq '{nameof(A)}'"
                               + $" and Property eq '{extensionProp}'"
                               + $" and Value in ({extValue})";
            var ext27 = new ExtensionEntity1
            {
                Id = 27,
                Entity = nameof(ExtensionEntity1),
                EntityId = "1099",
                Property = extensionProp,
                Value = extValue
            };
            var odataObjectExt27 = new OdataObject();
            var ext27Json = JsonConvert.SerializeObject(ext27);
            odataObjectExt27.Object = new JRaw(ext27Json);
            var odataExtCollection = new OdataObjectCollection { odataObjectExt27 };
            _MockRelatedEntityFilterDataProvider.Setup(m => m.ProvideAsync(nameof(ExtensionEntity1), expectedFilter))
                                      .ReturnsAsync(odataExtCollection);

            // Act
            var result = await relatedEntityFilterConverter.ConvertAsync(filter);

            // Assert
            Assert.AreEqual($"Id in ({ext27.EntityId})", result.ToString());
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task RelatedEntityExtensionFilterConverter_Convert_RelatedEntity_Works_StartsWith()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateConverter<A>();
            var extensionProp = "Prop1";
            var extValue = "Ext Val 27";
            Filter<A> filter = new Filter<A>
            {
                Left = $"{nameof(ExtensionEntity1)}.{extensionProp}",
                Method = "StartsWith",
                Right = new Filter<A> { NonFilter = extValue }
            };

            var aCsdl = typeof(A).ToCsdl();
            var extensionEntitiesList = new List<Type> { typeof(ExtensionEntity1) };
            aCsdl.AddExtensionEntityNavigationProperties(typeof(A), extensionEntitiesList);
            _CsdlSchema.Entities.Add(typeof(A).Name, aCsdl);
            _CsdlSchema.Entities.Add(typeof(ExtensionEntity1).Name, typeof(ExtensionEntity1).ToCsdl());

            var expectedFilter = $"$Filter=Entity eq '{nameof(A)}'"
                               + $" and Property eq '{extensionProp}'"
                               + $" and StartsWith(Value, '{extValue}')";
            var ext27 = new ExtensionEntity1
            {
                Id = 27,
                Entity = nameof(ExtensionEntity1),
                EntityId = "1099",
                Property = extensionProp,
                Value = extValue
            };
            var odataObjectExt27 = new OdataObject();
            var ext27Json = JsonConvert.SerializeObject(ext27);
            odataObjectExt27.Object = new JRaw(ext27Json);
            var odataExtCollection = new OdataObjectCollection { odataObjectExt27 };
            _MockRelatedEntityFilterDataProvider.Setup(m => m.ProvideAsync(nameof(ExtensionEntity1), expectedFilter))
                                      .ReturnsAsync(odataExtCollection);

            // Act
            var result = await relatedEntityFilterConverter.ConvertAsync(filter);

            // Assert
            Assert.AreEqual($"Id in ({ext27.EntityId})", result.ToString());
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
