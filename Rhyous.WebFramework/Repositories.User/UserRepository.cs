using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Repositories
{
    public class UserRepository : BaseRepository<User,IUser>
    {
        public UserRepository()
        {
            DbContext = new BaseDbContext<User>();
        }    
    }
}
