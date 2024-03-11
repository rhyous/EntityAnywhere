using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests
{
    public interface IMappingEntity1 : IBaseEntity<int>, IMappingEntity<int, int>
    {
        int EntityBasicId { get; set; }
        int EntityIntId { get; set; }
    }
}