using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services
{
    public class DuplicateUsernameDetector : IDuplicateUsernameDetector
    {
        private readonly IRepository<User, IUser, long> _Repo;
        public DuplicateUsernameDetector(IRepository<User, IUser, long> repository)
        {
            _Repo = repository;
        }

        public IEnumerable<string> Detect(IEnumerable<string> usernames, bool throwIfDuplicatesFound = true)
        {
            var users = _Repo.GetByExpression(u => usernames.Contains(u.Username));
            if (users != null && users.Any())
            {
                if (throwIfDuplicatesFound)
                    throw new DuplicateUsernameException("Duplicate username(s) detected: " + string.Join(", ", users.Select(u => u.Username)));
                return users.Select(u => u.Username);
            }
            return null;
        }
    }
}
