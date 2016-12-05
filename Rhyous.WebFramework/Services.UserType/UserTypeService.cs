using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    using IEntity = IUserType;
    using Entity = UserType;
    using System.Linq.Expressions;

    public class UserTypeService : ServiceCommonSearchable<Entity, IEntity>
    {
        public override Expression<Func<Entity, string>> PropertyExpression => e => e.Type;        
    }
}