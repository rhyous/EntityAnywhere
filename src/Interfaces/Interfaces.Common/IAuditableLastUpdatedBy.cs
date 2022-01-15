namespace Rhyous.EntityAnywhere.Interfaces
{    /// <summary>
     /// This provides the contract for entities to standardize CreateDate into a common auditable property.
     /// </summary>
    public interface IAuditableLastUpdatedBy
    {
        /// <summary>
        /// The user who updated the row most recently. 
        /// This value should be null on row creation and populated on first change.
        /// Ways to change the Property name to a custom value:
        /// 1. If the repository is Entity Framework, you can use the Column attribute to change the actual database column name if needed.
        /// Example: [Column("ModifiedBy")]
        /// 2. Implement the interface exclicitly so a new property has the correct name. However, this breaks uniformity.
        /// public int? IAuditableLastUpdatedBy.LastUpdatedBy { get { return ModifiedBy; } set { ModifiedBy = value; } }
        /// 3. Have a different concrete instance of the entity interface in the repository and only implement this in the repository's concrete instance.
        /// </summary>
        long? LastUpdatedBy { get; set; }
    }
}