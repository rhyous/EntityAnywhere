using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.WebServices
{
    public class AttributeToServiceDictionary : Dictionary<Type, Tuple<Type, Type, Type>>, IDictionaryDefaultValueProvider<Type, Tuple<Type, Type, Type>>
    {
        #region Singleton

        private static readonly Lazy<AttributeToServiceDictionary> Lazy = new Lazy<AttributeToServiceDictionary>(() => new AttributeToServiceDictionary());

        public static AttributeToServiceDictionary Instance { get { return Lazy.Value; } }

        internal AttributeToServiceDictionary()
        {
            Add(typeof(AlternateKeyAttribute), new Tuple<Type, Type, Type>(typeof(ServiceCommonAlternateKey<,,>), typeof(EntityWebServiceAlternateKey<,,,>), typeof(EntityWebServiceLoader<,,,,>)));
            Add(typeof(MappingEntityAttribute), new Tuple<Type, Type, Type>(typeof(ServiceCommon<,,>), typeof(MappingEntityWebService<,,,,,>), typeof(MappingEntityWebServiceLoader<,,,,,,>)));
        }

        #endregion


        public Tuple<Type, Type, Type> DefaultValueProvider(Type key)
        {
            return DefaultValue;
        }

        public Tuple<Type, Type, Type> DefaultValue => new Tuple<Type, Type, Type>(typeof(ServiceCommon<,,>), typeof(EntityWebService<,,,>), typeof(EntityWebServiceLoader<,,,,>));
    }
}
