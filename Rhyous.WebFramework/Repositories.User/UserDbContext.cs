namespace Rhyous.WebFramework.Repositories
{
    using System;
    using System.Data.Entity;

    public class UserDbContext : BaseDbContext<User>
    {
        #region Constructors
        public UserDbContext()
        {
            Database.SetInitializer<UserDbContext>(null);
        }

        public UserDbContext(int userId) : this()
        {
            UserId = userId;
        }
        
        #endregion
    }
}
