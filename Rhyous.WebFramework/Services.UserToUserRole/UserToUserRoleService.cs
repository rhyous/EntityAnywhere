using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public partial class UserToUserRoleService : ServiceCommonManyToMany<UserToUserRole, IUserToUserRole>
    {
        public override string PrimaryEntity => "User";
        public override string SecondaryEntity => "UserRole";
    }
}