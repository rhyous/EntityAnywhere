using Rhyous.Odata.Csdl;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IGetMetadataHandler
    {
        Task<CsdlEntity> Handle(Type type);
        Task<CsdlDocument> Handle();
    }
}
