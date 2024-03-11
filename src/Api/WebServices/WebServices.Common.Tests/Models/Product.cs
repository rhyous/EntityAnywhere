using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests
{
    /// <summary>
    /// This represents an entitleable or licenseable product. Usually a software product, but could be hardware.
    /// If the TypeId maps to "Suite" then this is a product that explodes into multiple Products.
    /// </summary>
    /// <remarks>In the future, we support nested Suites, where a Product of a Suite is also a Suite, and the explosion could be recursive.</remarks>
    [RelatedEntityForeign("ProductRelease", "Product")]

    [RelatedEntityForeign("ProductFeatureMap", "Product")]
    [RelatedEntityMapping("Feature", "ProductFeatureMap", "Product")]

    [RelatedEntityForeign("ProductGroupMembership", "Product")]
    [RelatedEntityMapping("ProductGroup", "ProductGroupMembership", "Product")]

    [RelatedEntityForeign("SuiteMembership", "Product")] // Product (not Suite) to SuiteMembership
    [RelatedEntityForeign("SuiteMembership", "Product", RelatedEntityAlias = "ProductMembership", ForeignKeyProperty = "SuiteId")] // Suite to SuiteMembership
    [RelatedEntityMapping("Product", "SuiteMembership", "Product", RelatedEntityAlias = "Suite")]
    [RelatedEntityMapping("Product", "SuiteMembership", "Product", EntityAlias = "Suite", MappingEntityAlias = "ProductMembership", RelatedEntityAlias = "ProductInSuite")]

    [RelatedEntityForeign("UpgradeProductMembership", "Product")] // Product (not UpgradeProduct) to UpgradeProductMembership
    [RelatedEntityForeign("UpgradeProductMembership", "Product", RelatedEntityAlias = "ProductToUpgradeMembership", ForeignKeyProperty = "UpgradeProductId")] // UpgradeProduct to UpgradeProductMembership
    [RelatedEntityMapping("Product", "UpgradeProductMembership", "Product", RelatedEntityAlias = "UpgradeProduct")]
    [RelatedEntityMapping("Product", "UpgradeProductMembership", "Product", EntityAlias = "UpgradeProduct", MappingEntityAlias = "ProductToUpgradeMembership", RelatedEntityAlias = "ProductToUpgrade")]
    public class Product : AuditableEntity<int>, IProduct
    {
        /// <inheritdoc />
        [Required]
        public string Name { get; set; }
        /// <inheritdoc />
        /// <remarks>Version is to go away and be replaced by ProductRelease</remarks>
        [MaxLength(64)]
        public string Version { get; set; }
        /// <inheritdoc />
        [IgnoreTrim]
        public string Description { get; set; }
        /// <inheritdoc />
        [RelatedEntity("ProductType")]
        public int TypeId { get; set; }
        public bool Enabled { get; set; }
    }
}