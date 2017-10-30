using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services.Common.Tests
{
    [AdditionalWebServiceTypes(typeof(int), typeof(int))]
    public partial class UserGroupMembership : Entity<long>, IUserGroupMembership
    {
        [RelatedEntity("UserGroup")]
        public int UserGroupId { get; set; }

        [RelatedEntity("User")]
        public int UserId { get; set; }
    }
}