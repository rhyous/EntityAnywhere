using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// The service contract for the top level $Metadata service.
    /// </summary>
    [ServiceContract]
    public interface IMetadataService
    {
        /// <summary>
        /// Gets the top level $Metadata for all services.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "$Metadata", ResponseFormat = WebMessageFormat.Json)]
        List<string> Get();
    }
}
