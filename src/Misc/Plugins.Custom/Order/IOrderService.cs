using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    public interface IOrderService : IServiceCommon<Order, IOrder, int>
    {
        Task<bool> ClearReleasedOrder(long[] entitlements);
        /// <summary>
        /// Release orders Async
        /// </summary>
        Task<string> ReleaseOrderAsync(ReleaseOrder order);
    }
}
