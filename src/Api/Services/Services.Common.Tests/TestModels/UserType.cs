using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    [RelatedEntityForeign("User", "UserType")]
    [RelatedEntityForeign("User2", "UserType", ForeignKeyProperty = "UserTypeName", EntityProperty = "Name")]
    public class UserType : BaseEntity<int>, IUserType
    {
        public string Name { get; set; }
    }
}
