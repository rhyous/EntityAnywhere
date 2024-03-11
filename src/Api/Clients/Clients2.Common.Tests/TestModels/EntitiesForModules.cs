using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests.DependencyInjection
{
    public class Entity1 : BaseEntity<int> { };
    public class Entity2 : BaseEntity<long> { };
    public class Entity3 : BaseEntity<string> { };

    public class ExtensionEntity1 : ExtensionEntity { };
    public class ExtensionEntity2 : ExtensionEntity { };
    public class ExtensionEntity3 : ExtensionEntity { };

    [MappingEntity(Entity1 = nameof(Entity1), Entity2 = nameof(Entity2))]
    public class MappingEntity1 : BaseEntity<int>, IMappingEntity<int, long>
    {
        [RelatedEntity(nameof(Entity1))]
        [DistinctProperty("MappingGroup")]
        public int Entity1Id { get; set; }

        [RelatedEntity(nameof(Entity2))]
        [DistinctProperty("MappingGroup")]
        public long Entity2Id { get; set; }
    };

    [MappingEntity(Entity1 = nameof(Entity1), Entity2 = nameof(Entity2))]
    public class MappingEntity2 : BaseEntity<int>, IMappingEntity<int, long>
    {
        [RelatedEntity(nameof(Entity1))]
        [DistinctProperty("MappingGroup")]
        public int Entity1Id { get; set; }

        [RelatedEntity(nameof(Entity2))]
        [DistinctProperty("MappingGroup")]
        public long Entity2Id { get; set; }
    };

    [MappingEntity(Entity1 = nameof(Entity1), Entity2 = nameof(Entity2))]
    public class MappingEntity3 : BaseEntity<int>, IMappingEntity<long, string>
    {
        [RelatedEntity(nameof(Entity2))]
        [DistinctProperty("MappingGroup")]
        public long Entity2Id { get; set; }

        [RelatedEntity(nameof(Entity3))]
        [DistinctProperty("MappingGroup")]
        public string Entity3Id { get; set; }
    };
}