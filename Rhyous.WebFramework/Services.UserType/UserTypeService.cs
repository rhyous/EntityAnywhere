using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public class UserTypeService : ServiceCommon<UserType, IUserType>, ISearchableServiceCommon<UserType, IUserType>
    {
        public IUserType Get(string type)
        {
            return Repo.Get(type, e => e.Type);
        }

        public List<IUserType> Search(string type)
        {
            return Repo.Search(type, u => u.Type);
        }

        public override IUserType Add(IUserType entity)
        {
            if (Get(entity.Type) != null)
                throw new Exception("Duplicate UserType detected.");
            return base.Add(entity);
        }
    }
}