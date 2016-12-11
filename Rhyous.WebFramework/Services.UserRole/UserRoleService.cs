using Rhyous.WebFramework.Interfaces;
using System;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    using IEntity = IUserRole;
    using Entity = UserRole;

    public partial class UserRoleService : ServiceCommonSearchable<Entity, IEntity, int>
    {
        public override Expression<Func<Entity, string>> PropertyExpression => e => e.Name;
    }
}