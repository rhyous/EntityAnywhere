using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public class UserToUserTypeService : ServiceCommonMap<UserToUserType, IUserToUserType>
    {
        public override string PrimaryEntity => "UserId";
        public override string SecondaryEntity => "UserTypeId";
    }
}