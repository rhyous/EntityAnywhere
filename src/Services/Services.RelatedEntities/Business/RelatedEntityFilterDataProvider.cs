using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.Odata.Filter;
using Rhyous.EntityAnywhere.Clients2;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    class RelatedEntityFilterDataProvider : IRelatedEntityFilterDataProvider
    {
        private readonly INamedFactory<IEntityClientAsync> _NamedFactory;

        public RelatedEntityFilterDataProvider(INamedFactory<IEntityClientAsync> namedFactory)
        {
            _NamedFactory = namedFactory;
        }

        public async Task<OdataObjectCollection> ProvideAsync(string relatedEntityName, string filter)
        {
            var client = _NamedFactory.Create(relatedEntityName);
            var jsonResult = await client.GetByQueryParametersAsync(filter);
            var collection = JsonConvert.DeserializeObject<OdataObjectCollection>(jsonResult);
            return collection;
        }
    }
}
