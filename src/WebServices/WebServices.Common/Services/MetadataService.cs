using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This is the primary Metadata service. Currently it only returns a string list of entities. It should be enhanced to return a list of all Entities that this installation can manage as well as their service urls.
    /// </summary>
    public class MetadataService : IMetadataService
    {
        /// <inheritdoc />
        public List<string> Get()
        {
            return EntityLoader.LoadedEntities.Select(t=>t.Name).ToList();
        }
    }
}
