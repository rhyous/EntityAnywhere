using Rhyous.EntityAnywhere.Clients2;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    public interface ITestConfiguration : IEntityClientConfig
    {
        string EntitlementServicesUrl { get; }
        string EntitlementServicesSubPath { get; }
    }
}
