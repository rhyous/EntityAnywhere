using Rhyous.EntityAnywhere.Attributes;
using Rhyous.Odata;

namespace Rhyous.EntityAnywhere.WebApi
{
    /// <summary>Maps an Entity to is Controller based on attributes.</summary>
    public class AttributeToServiceDictionary : Dictionary<Type, Type>, IAttributeToServiceDictionary
    {
        /// <summary>The construtor.</summary>
        public AttributeToServiceDictionary()
        {
            Add(typeof(AlternateKeyAttribute), typeof(EntityControllerAlternateKey<,,,>));
            Add(typeof(ExtensionEntityAttribute), typeof(ExtensionEntityController<,>));
            Add(typeof(MappingEntityAttribute), typeof(MappingEntityController<,,,,>));
        }

        /// <summary>The construtor.</summary>
        public Type DefaultValueProvider(Type key)
        {
            return DefaultValue;
        }

        /// <summary>The default value.</summary>
        public Type DefaultValue => typeof(EntityController<,,>);
    }
}