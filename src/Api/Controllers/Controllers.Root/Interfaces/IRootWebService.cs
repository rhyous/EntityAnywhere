using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Odata.Csdl;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IRootWebService : IMetadataWebService<CsdlDocument>, 
                                       IGenerateRepositoryAsync,
                                       IImpersonationWebService,
                                       ICustomWebService
    {
    }
}