using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests
{
    public interface IMappingEntityInt : IMappingEntity<int, string>
    {
        public int EntityIntId { get; set; }
        public string EntityStringId { get; set; }
    }
}