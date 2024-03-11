using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    public interface ISuiteMembership : IBaseEntity<int>, IMappingEntity<int, int>
    {
        int SuiteId { get; set; }
        int ProductId { get; set; }
    }
}
