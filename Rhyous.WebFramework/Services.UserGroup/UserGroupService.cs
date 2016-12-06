using Rhyous.WebFramework.Interfaces;
using System;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    using IEntity = IUserGroup;
    using Entity = UserGroup;

    public class UserGroupService : ServiceCommonSearchable<Entity, IEntity>
    {
        public override Expression<Func<Entity, string>> PropertyExpression => e => e.Name;        
    }
}