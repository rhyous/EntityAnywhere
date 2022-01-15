namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// This provides the contract for entities to standardize CreatedBy into a common auditable property.
    /// </summary>
    public interface IAuditableCreatedBy
    {
        ///  <summary>
        /// The user who created the entity.
        /// Ways to change the Property name to a custom value:
        /// 1. If the repository is Entity Framework, you can use the Column attribute to change the actual database column name if needed.
        /// Example: [Column("User")]
        /// 2. Implement the interface exclicitly so a new property has the correct name. However, this breaks uniformity.
        /// public int? IAuditableCreatedBy.CreatedBy { get { return User; } set { ModifiedBy = User; } }
        /// 3. Have a different concrete instance of the entity interface in the repository and only implement this in the repository's concrete instance.
        /// </summary>
        long CreatedBy { get; set; }
    }
}