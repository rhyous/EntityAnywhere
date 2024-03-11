using Rhyous.Odata;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    public interface IRelatedEntityProvider<TEntity, TInterface, TId>
    {
        Task ProvideAsync(IEnumerable<OdataObject<TEntity, TId>> entities, NameValueCollection urlParameters);
    }
}
