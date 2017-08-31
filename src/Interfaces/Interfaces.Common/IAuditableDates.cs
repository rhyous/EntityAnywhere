namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// And interface that adds together the auditble date fields: 
    /// CreateDate, LastUpdated
    /// </summary>
    public interface IAuditableDates : IAuditableCreateDate, IAuditableLastUpdatedDate
    {
    }
}
