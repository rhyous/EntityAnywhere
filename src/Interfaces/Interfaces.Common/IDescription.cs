namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// An interface that helps provide a standard Description property for Entities.
    /// </summary>
    public interface IDescription
    {
        /// <summary>
        /// Provides a description of an entity.
        /// Ways to change the Property name to a custom value:
        /// 1. If the repository is Entity Framework, you can use the Column attribute to change the actual database column name if needed.
        /// Example: [Column("MoreInfo")]
        /// 2. Implement the interface exclicitly so a new property has the correct name. However, this breaks uniformity.
        /// public int? IDescription.Description { get { return MoreInfo; } set { Date = MoreInfo; } }
        /// 3. Have a different concrete instance of the entity interface in the repository and only implement this in the repository's concrete instance.
        /// </summary>
        string Description { get; set; }
    }
}
