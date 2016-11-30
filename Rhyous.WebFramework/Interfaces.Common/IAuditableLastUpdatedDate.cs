using System;

namespace Rhyous.WebFramework.Interfaces
{
    public interface IAuditableLastUpdatedDate
    {
        /// <summary>
        /// The last time the row was updated. This value is null on
        /// row creation and populated on first change.
        /// You can use the Column attribute to change the actual 
        /// database column name if needed. 
        /// Example: [Column("Date")]
        /// </summary>
        DateTime? LastUpdated { get; set; }
    }
}