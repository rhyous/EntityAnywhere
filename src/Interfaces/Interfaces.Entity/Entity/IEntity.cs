namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntity : IBaseEntity<int>, IName, IDescription, IAuditable
    {
        bool Enabled { get; set; }
        int EntityGroupId { get; set; }

        /// <summary>
        /// The primary key reference to an EntityProperty.Id
        /// </summary>
        int? SortByPropertyId { get; set; }

        /// <summary>
        /// Whether to sort by Ascending or Descending
        /// </summary>
        SortOrder SortOrder { get; set; }
    }
}
