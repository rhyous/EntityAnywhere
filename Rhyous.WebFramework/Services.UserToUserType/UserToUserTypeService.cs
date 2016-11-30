using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    public class UserToUserTypeService : ServiceCommon<UserToUserType, IUserToUserType>
    {
        public List<IUserToUserType> GetByPropertyId(int id, string propertyName)
        {
            if (propertyName != "UserId" && propertyName != "UserTypeId")
                throw new ArgumentException("Valid property names are UserId or UserTypeId", "propertyName");
            return Repo.GetByExpression(propertyName.ToLambda<UserToUserType, int>(id)).ToList();
        }
    }
}