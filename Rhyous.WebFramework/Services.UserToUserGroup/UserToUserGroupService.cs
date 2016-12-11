using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public partial class UserToUserGroupService : ServiceCommonManyToMany<UserToUserGroup, IUserToUserGroup, long, long, int>
    {
        public override string PrimaryEntity => "User";
        public override string SecondaryEntity => "UserGroup";
    }
}