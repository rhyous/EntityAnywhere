using System;

namespace Rhyous.WebFramework.Interfaces
{
    public interface IAuditableLastUpdatedDate
    {
        /// <summary>
        /// The last time the row was updated. This value should be null on Entity creation and populated on first change.
        /// Ways to change the Property name to a custom value:
        /// 1. If the repository is Entity Framework, you can use the Column attribute to change the actual database column name if needed.
        /// Example: [Column("Date")]
        /// 2. Implement the interface exclicitly so a new property has the correct name. However, this breaks uniformity.
        /// public int? IAuditableLastUpdatedDate.LastUpdated { get { return Date; } set { Date = value; } }
        /// 3. Have a different concrete instance of the entity interface in the repository and only implement this in the repository's concrete instance.
        /// </summary>
        DateTime? LastUpdated { get; set; }
    }
}