namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// An interface that helps provide a standard Enable/Disable property for Entities.
    /// </summary>
    public interface IEnabled
    {
        /// <summary>
        /// True means it is enabled or active. False mean it is inactive or disabled.
        /// Ways to change the Property name to a custom value:
        /// 1. If the repository is Entity Framework, you can use the Column attribute to change the actual database column name if needed.
        /// Example: [Column("Active")]
        /// 2. Implement the interface exclicitly so a new property has the correct name. However, this breaks uniformity.
        /// public int? IEnabled.Enabled { get { return Active; } set { Date = Active; } }
        /// 3. Have a different concrete instance of the entity interface in the repository and only implement this in the repository's concrete instance.
        /// </summary>
        bool Enabled { get; set; }
    }
}
