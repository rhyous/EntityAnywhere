using Rhyous.Collections;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IUserRoleEntityData : IUserEntityDataModel, IConcurrentDictionary<string, EntityPermission>
    {

        bool TryGetValue(int key, out EntityPermission value);
        bool TryRemove(int key);
    }

    public interface IUserEntityDataModel
    {
        int UserRoleId { get; }
        string UserRoleName { get; }
    }
}