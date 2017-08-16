using System.Collections.Generic;

namespace Rhyous.WebFramework.WebServices
{
    public class PatchedEntity<TEntity>
    {
        public TEntity Entity { get; set; }
        public List<string> ChangedProperties { get; set; }
    }
}
