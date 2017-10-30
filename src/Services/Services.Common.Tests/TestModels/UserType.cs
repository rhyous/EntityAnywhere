using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services.Common.Tests
{
    public class UserType : Entity<int>, IUserType
    {
        public string Name { get; set; }
    }
}
