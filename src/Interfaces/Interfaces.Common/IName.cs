namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// An interface that helps provide a standard Name property for Entities.
    /// </summary>
    public interface IName
    {
        /// <summary>
        /// Provides the name of an entity.
        /// Ways to change the Property name to a custom value:
        /// 1. If the repository is Entity Framework, you can use the Column attribute to change the actual database column name if needed.
        /// Example: [Column("DisplayName")]
        /// 2. Implement the interface exclicitly so a new property has the correct name. However, this break suniformity.
        /// public int? IName.Name { get { return DisplayName; } set { Date = DisplayName; } }
        /// 3. Have a different concrete instance of the entity interface in the repository and only implement this in the repository's concrete instance.
        /// </summary>
        string Name { get; set; }
    }
}
