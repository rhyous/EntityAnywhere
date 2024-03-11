using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Repositories.Common.Tests
{
    public class User : BaseEntity<int>, IUser
    {
        public string Name { get; set; }
    }
}