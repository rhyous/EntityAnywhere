using Rhyous.Odata.Csdl;
using System.ServiceModel;

namespace Rhyous.EntityAnywhere.WebServices
{
    [ServiceContract]
    public interface IRootWebService : IMetadataWebService<CsdlDocument>, 
                                       IGenerateRepositoryAsync,
                                       ICustomWebService
    {
    }
}