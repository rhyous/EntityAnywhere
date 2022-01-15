using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    public interface IEntitlementService : IServiceCommon<Entitlement, IEntitlement, long>
    {
        Task<IQueryable<IEntitlement>> FilterAsync(string key, string filter, NameValueCollection UrlParameters);
    }
}
