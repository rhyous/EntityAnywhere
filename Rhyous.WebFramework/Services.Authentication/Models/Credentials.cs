using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public class Credentials : ICredentials
    {
        public string User { get; set; }
        public string Password { get; set; }
    }
}