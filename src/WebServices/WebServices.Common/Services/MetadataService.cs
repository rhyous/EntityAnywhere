using System.Collections.Generic;

namespace Rhyous.WebFramework.WebServices
{
    public class MetadataService : IMetadataService
    {
        public List<string> Get()
        {
            return EntityLoader.LoadedEntities;
        }
    }
}
