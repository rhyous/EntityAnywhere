using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    [AdditionalWebServiceTypes(typeof(int), typeof(long))]
    public partial class UserGroupMembership : BaseEntity<long>, IUserGroupMembership
    {
        [RelatedEntity("UserGroup")]
        public int UserGroupId { get; set; }

        [RelatedEntity("User")]
        public long UserId { get; set; }
    }
}