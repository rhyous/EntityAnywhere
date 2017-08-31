namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// An interface that helps provide a standard Id property for Entities.
    /// </summary>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public interface IId<TId>
    {
        /// <summary>
        /// Provides a description of an entity.
        /// Ways to change the Property name to a custom value:
        /// 1. If the repository is Entity Framework, you can use the Column attribute to change the actual database column name if needed.
        /// Example: [Column("EntityId")]
        /// 2. Implement the interface exclicitly so a new property has the correct name. However, this break uniformity.
        /// public int? IId.Id { get { return EntityId; } set { Date = EntityId; } }
        /// 3. Have a different concrete instance of the entity interface in the repository and only implement this in the repository's concrete instance.
        /// </summary>
        TId Id { get; set; }
    }
}
