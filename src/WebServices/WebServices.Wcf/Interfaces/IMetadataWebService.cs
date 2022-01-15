using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// The service contract for the top level $Metadata service.
    /// </summary>
    [ServiceContract]
    public interface IMetadataWebService<T>
    {
        /// <summary>
        /// Gets the top level $Metadata for all services.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "$Metadata", ResponseFormat = WebMessageFormat.Json)]
        Task<T> GetMetadataAsync();
    }
}