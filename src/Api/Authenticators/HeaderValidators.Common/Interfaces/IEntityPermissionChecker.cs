namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface IEntityPermissionChecker
    {
        bool HasPermission(string role, string entity);
        bool HasPermission(int roleId, string entity);
    }
}