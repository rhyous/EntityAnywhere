using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Attributes;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Business.TestEntities
{
    public class TestEntityWithOneGroup : AuditableEntity<long>, ITestEntityWithOneGroup
    {
        [DistinctProperty("ExtensionEntityGroup")]
        public string Entity { get; set; }
        
        [DistinctProperty("ExtensionEntityGroup")]
        public string EntityId { get; set; }
        
        [DistinctProperty("ExtensionEntityGroup")]
        public string Property { get; set; }
        
        [DistinctProperty("ExtensionEntityGroup")]
        public string Value { get; set; }
    }
}
