using Rhyous.WebFramework.Interfaces;
using System;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    using IEntity = IUserGroup;
    using Entity = UserGroup;
    using IdType = System.Int32;

    public class UserGroupService : ServiceCommonSearchable<Entity, IEntity, IdType>
    {
        public override Expression<Func<Entity, string>> PropertyExpression => e => e.Name;        
    }
}