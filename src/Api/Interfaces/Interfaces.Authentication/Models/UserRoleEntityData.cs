using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Interfaces
{
    [ExcludeFromCodeCoverage]
    public class UserRoleEntityData : ConcurrentDictionary<string, EntityPermission>,
                                      IUserRoleEntityData
    {
        public int UserRoleId { get; set; }
        public string UserRoleName { get; set; }

        public IDictionary<int, string> EntityIds { get; } = new ConcurrentDictionary<int, string>();
        public EntityPermission this[int key]
        {
            get { return this[EntityIds[key]]; }
        }

        public bool TryGetValue(int key, out EntityPermission value)
        {
            value = null;
            return EntityIds.TryGetValue(key, out string strKey) && TryGetValue(strKey, out value);
        }

        public bool TryRemove(int key)
        {
            return TryRemove(EntityIds[key], out _);
        }
    }
}