using System.Collections.Generic;
using System.Threading.Tasks;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Entities;

namespace Rhyous.WebFramework.Services
{
    public interface IClaimsBuilderAsync
    {
        Task<List<ClaimDomain>> BuildAsync(IUser user, IEnumerable<ClaimConfiguration> claimsConfigurations);
    }
}