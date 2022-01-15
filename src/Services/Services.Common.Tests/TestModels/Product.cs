using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    [RelatedEntityForeign("SuiteMembership", "Product")] // Product (not Suite) to SuiteMembership
    [RelatedEntityForeign("SuiteMembership", "Product", RelatedEntityAlias = "ProductMembership", ForeignKeyProperty = "SuiteId")] // Suite to SuiteMembership
    [RelatedEntityMapping("Product", "SuiteMembership", "Product", RelatedEntityAlias = "Suite")]
    [RelatedEntityMapping("Product", "SuiteMembership", "Product", EntityAlias = "Suite", MappingEntityAlias = "ProductMembership")]
    public class Product : BaseEntity<int>, IProduct
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public int Type { get; set; }
    }
}
