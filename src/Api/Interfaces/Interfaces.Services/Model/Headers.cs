using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class Headers : IHeaders
    {
        public NameValueCollection Collection { get; set; }
    }
}
