using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class AdminHeaders : IAdminHeaders 
    {
        public AdminHeaders(IEntityClientConfig config)
        {
            Collection = new NameValueCollection();
            Collection.Add(nameof(config.EntityAdminToken), config.EntityAdminToken);            
        }

        public NameValueCollection Collection { get; }
    }
}