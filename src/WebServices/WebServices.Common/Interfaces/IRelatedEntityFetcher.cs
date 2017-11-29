using Rhyous.Odata;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.WebFramework.WebServices
{
    public interface IRelatedEntityFetcher<TEntity, TId>
    {
        void Fetch(IEnumerable<OdataObject<TEntity, TId>> entities, NameValueCollection urlParameters);
    }
}
