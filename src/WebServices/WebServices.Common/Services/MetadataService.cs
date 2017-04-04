using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    public class MetadataService : IMetadataService
    {
        public List<string> Get()
        {
            return EntityLoader.LoadedEntities.Select(t=>t.Name).ToList();
        }
    }
}
