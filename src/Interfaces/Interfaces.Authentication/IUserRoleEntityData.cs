using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IUserRoleEntityData : IDictionary<string, EntityPermission>
    {
        int UserRoleId { get; set; }
        string UserRoleName { get; set; }

        bool TryGetValue(int key, out EntityPermission value);
        bool TryRemove(int key);
    }
}