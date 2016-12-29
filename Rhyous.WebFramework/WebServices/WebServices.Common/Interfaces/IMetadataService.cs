using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    [ServiceContract]
    public interface IMetadataService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "$Metadata", ResponseFormat = WebMessageFormat.Json)]
        List<string> Get();
    }
}
