using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public partial class UserToUserTypeService : ServiceCommonManyToMany<UserToUserType, IUserToUserType, long, long, int>
    {
        public override string PrimaryEntity => "User";
        public override string SecondaryEntity => "UserType";
    }
}