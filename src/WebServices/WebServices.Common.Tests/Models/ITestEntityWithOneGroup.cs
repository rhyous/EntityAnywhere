using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Business.TestEntities
{
    public interface ITestEntityWithOneGroup : IBaseEntity<long>
    {
        string Entity { get; set; }
        
        string EntityId { get; set; }
        
        string Property { get; set; }

        string Value { get; set; }
    }
}
