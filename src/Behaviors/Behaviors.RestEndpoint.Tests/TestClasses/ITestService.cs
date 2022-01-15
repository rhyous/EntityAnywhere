using System.ServiceModel;

namespace Rhyous.EntityAnywhere.Behaviors.RestEndpoint.Tests
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITestService" in both code and config file together.
    [ServiceContract]
    public interface ITestService
    {
        [OperationContract]
        void DoWork();
    }
}
