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

        public override List<IUserType> Add(IList<IUserType> entities)
        {
            var duplicateTypes = new List<string>();
            foreach (var entity in entities)
            {
                if (Get(entity.Type) != null)
                    duplicateTypes.Add(entity.Type);
            }
            if (duplicateTypes.Count > 0)
                throw new Exception("Duplicate UserType detected:" + string.Concat(", ", duplicateTypes));
            return base.Add(entities);
        }
    }
}