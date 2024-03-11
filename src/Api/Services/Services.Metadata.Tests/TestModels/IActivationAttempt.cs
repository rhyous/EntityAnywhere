using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    public interface IActivationAttempt : IBaseEntity<int>, IAuditableCreateDate
    {
        string HostName { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
