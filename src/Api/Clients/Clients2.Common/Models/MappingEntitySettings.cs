using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Clients2
{
    [ExcludeFromCodeCoverage]
    public partial class MappingEntitySettings : IMappingEntitySettings
    {
        public MappingEntitySettings(string entityName, 
                                     string entity1, string entity1Pluralized, string entity1Property,
                                     string entity2, string entity2Pluralized, string entity2Property)
        {
            Name = entityName;
            Entity1 = entity1;
            Entity1Pluralized = entity1Pluralized;
            Entity1Property = entity1Property;
            Entity2 = entity2;
            Entity2Pluralized = entity2Pluralized;
            Entity2Property = entity2Property;
        }
        public virtual string Name { get; }
        public virtual string Entity1 { get; }
        public virtual string Entity1Pluralized { get; }
        public virtual string Entity1Property { get; }
        public virtual string Entity2 { get; }
        public virtual string Entity2Pluralized { get; }
        public virtual string Entity2Property { get; }
    }

    public partial class MappingEntitySettings<TEntity> : MappingEntitySettings,
                                                          IMappingEntitySettings<TEntity>
    {
        private readonly MappingEntityAttribute _MappingEntityAttribute;

        public MappingEntitySettings()
            : base(typeof(TEntity).Name, null, null, null, null, null, null)
        {
            _MappingEntityAttribute = typeof(TEntity).GetAttribute<MappingEntityAttribute>() 
                ?? throw new ArgumentException($"The type used for TEntity must be decorated with {nameof(MappingEntityAttribute)}", nameof(TEntity));
        }

        public override string Entity1 => _MappingEntityAttribute.Entity1;
        public override string Entity1Pluralized => typeof(TEntity).GetMappedEntity1Pluralized();
        public override string Entity1Property => _MappingEntityAttribute.Entity1MappingProperty;
        public override string Entity2 => _MappingEntityAttribute.Entity2;
        public override string Entity2Pluralized => typeof(TEntity).GetMappedEntity2Pluralized();
        public override string Entity2Property => _MappingEntityAttribute.Entity2MappingProperty;
    }
}