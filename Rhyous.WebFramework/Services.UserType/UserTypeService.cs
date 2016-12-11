using Rhyous.WebFramework.Interfaces;
using System;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    using IEntity = IUserType;
    using Entity = UserType;

    public partial class UserTypeService : ServiceCommonSearchable<Entity, IEntity, int>
    {
        public override Expression<Func<Entity, string>> PropertyExpression => e => e.Type;
    }
}