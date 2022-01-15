using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests
{
    [MappingEntity(Entity1 = nameof(EntityInt), Entity2 = nameof(EntityString))]
    public class MappingEntityInt : IMappingEntityInt, IBaseEntity<int>
    {
        public int Id { get; set; }
        public int EntityIntId { get; set; }
        public string EntityStringId { get; set; }
    }
}
