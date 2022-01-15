using Rhyous.Odata;
using Rhyous.WebFramework.Entities;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    interface IEntity1WebService : IServiceCommon<Entity1, IEntity1, int>
    {        
    }
}