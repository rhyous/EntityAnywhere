using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class UserRoleEntityDataCache : ConcurrentDictionary<int, IUserRoleEntityData>,
                                           IUserRoleEntityDataCache
    {
        public IDictionary<string, int> UserRoleIds { get; } = new ConcurrentDictionary<string, int>();
        public IUserRoleEntityData this[string key]
        {
            get { return this[UserRoleIds[key]]; }
        }

        public bool TryGetValue(string key, out IUserRoleEntityData value)
        {
            value = null;
            return UserRoleIds.TryGetValue(key, out int intKey) && TryGetValue(intKey, out value);
        }
    }
}