using Rhyous.EntityAnywhere.Interfaces;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.EntityAnywhere.WebServices
{
    [ServiceContract]
    public interface IGenerateRespository
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        RepositoryGenerationResult GenerateRepository();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        RepositorySeedResult InsertSeedData();
    }
}
