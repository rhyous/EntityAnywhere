using Rhyous.WebFramework.Interfaces;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    using IEntity = IUserRole;
    using Entity = UserRole;

    public class UserRoleService : ServiceCommonSearchable<Entity, IEntity>
    {
        public override Expression<Func<Entity, string>> PropertyExpression => e => e.Name;        
    }
}