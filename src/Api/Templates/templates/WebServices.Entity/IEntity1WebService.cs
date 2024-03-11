using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;
using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    [ServiceContract]
    interface IEntity1WebService : IEntityWebService<Entity1, int>, ICustomWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "NewEndpoint", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Task<OdataObjectCollection<Entity1, int>> NewEndpoint();
    }
}
