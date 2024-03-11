using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Interfaces
{
    [ExcludeFromCodeCoverage]
    public class AnonymousUserRoleEntityData : IUserEntityDataModel
    {
        public int UserRoleId { get; set; }
        public string UserRoleName { get; set; }
    }
}