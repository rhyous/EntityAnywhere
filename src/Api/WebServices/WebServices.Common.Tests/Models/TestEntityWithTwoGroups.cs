using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Attributes;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Business.TestEntities
{
    public class TestEntityWithTwoGroups : AuditableEntity<long>, ITestEntityWithTwoGroups
    {
        [DistinctProperty("Group1")]
        public string Entity { get; set; }

        [DistinctProperty("Group1")]
        public string EntityId { get; set; }

        [DistinctProperty("Group2")]
        public string Property { get; set; }

        [DistinctProperty("Group2")]
        public string Value { get; set; }
    }
}
