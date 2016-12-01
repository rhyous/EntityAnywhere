using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public class UserToUserTypeService : ServiceCommonManyToMany<UserToUserType, IUserToUserType>
    {
        public override string PrimaryEntity => "User";
        public override string SecondaryEntity => "UserType";
    }
}