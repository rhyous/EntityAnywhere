namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// And interface that adds together all the auditble fields: 
    /// CreatedBy, LastUpdatedBy, CreateDate, LastUpdated
    /// </summary>
    public interface IAuditable : IAuditableDates, IAuditableUsers
    {
    }
}