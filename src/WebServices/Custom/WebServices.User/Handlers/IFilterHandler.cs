using Rhyous.Odata;
using Rhyous.WebFramework.Entities;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.WebServices
{
    public interface IFilterHandler
    {
        Task<OdataObjectCollection<User, long>> FilterAsync();
    }
}