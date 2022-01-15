using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services
{
    public class UserAssociation : IUserAssociation
    {
        public string System { get; set; }
        public string SystemUserId { get; set; }
    }
}