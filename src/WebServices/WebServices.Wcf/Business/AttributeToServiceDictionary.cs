using Rhyous.Collections;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class AttributeToServiceDictionary : Dictionary<Type, AttributeServiceTypes>, IDictionaryDefaultValueProvider<Type, AttributeServiceTypes>
    {
        public AttributeToServiceDictionary()
        {
            Add(typeof(AlternateKeyAttribute), new AttributeServiceTypes { ServiceGenericType = typeof(ServiceCommonAlternateKey<,,,>), WebServiceGenericType = typeof(EntityWebServiceAlternateKey<,,,>), LoaderType = typeof(IEntityWebServiceLoader<,>) });
            Add(typeof(MappingEntityAttribute), new AttributeServiceTypes { ServiceGenericType = typeof(ServiceCommon<,,>), WebServiceGenericType = typeof(MappingEntityWebService<,,,,>), LoaderType = typeof(IEntityWebServiceLoader<,>) });
            Add(typeof(ReadOnlyEntityAttribute), new AttributeServiceTypes { ServiceGenericType = typeof(ServiceCommon<,,>), WebServiceGenericType = typeof(EntityWebServiceReadOnly<,,>), LoaderType = typeof(IEntityWebServiceLoader<,>) });
            Add(typeof(ExtensionEntityAttribute), new AttributeServiceTypes { ServiceGenericType = typeof(ServiceCommon<,,>), WebServiceGenericType = typeof(ExtensionEntityWebService<,>), LoaderType = typeof(IEntityWebServiceLoader<,>) });
        }

        public AttributeServiceTypes DefaultValueProvider(Type key)
        {
            return DefaultValue;
        }

        public AttributeServiceTypes DefaultValue => _DefaultValue;
        private readonly AttributeServiceTypes _DefaultValue = new AttributeServiceTypes
        {
            ServiceGenericType = typeof(ServiceCommon<,,>),
            WebServiceGenericType = typeof(EntityWebService<,,>),
            LoaderType = typeof(IEntityWebServiceLoader<,>)
        };
    }
}