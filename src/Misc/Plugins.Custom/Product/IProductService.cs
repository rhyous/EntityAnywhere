using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    public interface IProductService : IServiceCommon<Product, IProduct, int>
    {
        Task<IQueryable<IProduct>> FilterProductsBySkuAsync(string filters, NameValueCollection urlParameters);
    }
}