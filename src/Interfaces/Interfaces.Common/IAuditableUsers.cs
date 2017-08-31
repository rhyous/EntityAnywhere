namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// And interface that adds together the auditble user fields: 
    /// CreatedBy, LastUpdatedBy
    /// </summary>
    public interface IAuditableUsers : IAuditableCreatedBy, IAuditableLastUpdatedBy
    {
    }
}
