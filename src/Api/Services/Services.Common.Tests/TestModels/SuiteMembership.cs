using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    public class SuiteMembership : BaseEntity<int>, ISuiteMembership
    {
        [RelatedEntity("Product", Property = "SuiteId", RelatedEntityAlias = "Suite")]
        public int SuiteId { get; set; }

        [RelatedEntity("Product")]
        public int ProductId { get ; set ; }
    }
}
