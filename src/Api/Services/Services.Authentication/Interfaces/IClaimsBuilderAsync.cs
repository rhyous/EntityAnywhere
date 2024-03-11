using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IClaimsBuilderAsync
    {
        Task<IUser> BuildAsync(long userId, IToken token);
        Task<List<ClaimDomain>> BuildAsync(IUser user, IEnumerable<IClaimConfiguration> claimsConfigurations, IList<RelatedEntityCollection> relatedEntityCollections);
    }
}